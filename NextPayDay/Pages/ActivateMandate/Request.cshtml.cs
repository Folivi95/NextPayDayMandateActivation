using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NextPayDay.Model;
using NextPayDay.Model.ResponseVM;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NextPayDay
{
    public class RequestModel : PageModel
    {
        private readonly NextPayDay.Data.OTPDbContext _context;

        public RequestModel(NextPayDay.Data.OTPDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            OTPActivation = new OTPActivation();
            return Page();
        }

        [BindProperty]
        public OTPActivation OTPActivation { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = "";

            var content = $"{{\"mandateId\":\"{OTPActivation.MandateId}\"," +
                    $"\"requestId\":{OTPActivation.RequestId}}}";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //make http request to request for OTP
            using (var client = new WebClient())
            {
                var requestId = GenerateRandomNumber(13);
                var apiHash = ComputeSHAHash(Environment.GetEnvironmentVariable("APIKEY") + requestId + Environment.GetEnvironmentVariable("APITOKEN"));
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers.Add("MERCHANT_ID", $"{Environment.GetEnvironmentVariable("MERCHANTID")}");
                client.Headers.Add("API_KEY", $"{Environment.GetEnvironmentVariable("APIKEY")}");
                client.Headers.Add("REQUEST_ID", $"{requestId}");
                client.Headers.Add("REQUEST_TS", $"{DateTime.Now.ToString("yyyy-MM-ddThh:MM:ss+000000")}");
                client.Headers.Add("API_DETAILS_HASH", $"{apiHash}");

                try
                {

                    response = client.UploadString(Environment.GetEnvironmentVariable("REQUEST_URL"), "POST", content);
                    response = response.Replace("jsonp (", "");
                    response = response.Replace(")", "");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception Occurred: {ex.Message}");
                }
            }

            OTPRequest res = JsonConvert.DeserializeObject<OTPRequest>(response);
            OTPActivation.RemitaTransRef = res.remitaTransRef;
            

            //check if OTP request is successful
            if (res.status == "SUCCESS")
            {
                foreach (var item in res.authParams)
                {
                    OTPActivation.DescriptionOne = item.description1;
                    OTPActivation.DescriptionTwo = item.description2;
                }
                _context.OTPActivations.Add(OTPActivation);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Validate", new { ID = OTPActivation.ID });
            }

            return Page();
        }

        private static string GenerateRandomNumber(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            string s;
            for (int i = 0; i < size; i++)
            {
                s = Convert.ToString(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(s);
            }

            return builder.ToString();
        }

        private static string ComputeSHAHash(string data)
        {
            Byte[] EncryptedHash;
            using (SHA512Managed sha512 = new SHA512Managed())
            {
                EncryptedHash = sha512.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
            string hashed = BitConverter.ToString(EncryptedHash).Replace("-", "").ToLower();
            return hashed;
        }
    }
}