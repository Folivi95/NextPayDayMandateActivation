using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NextPayDay.Data;
using NextPayDay.Model;
using NextPayDay.Model.ResponseVM;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NextPayDay
{
    public class ValidateModel : PageModel
    {
        private readonly OTPDbContext _context;

        [BindProperty]
        public OTPActivation OTPActivation { get; set; }

        public ValidateModel(OTPDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int ID)
        {
            OTPActivation = _context.OTPActivations.FirstOrDefault(m => m.ID == ID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //OTPActivation = _context.OTPActivations.FirstOrDefault(m => m.MandateId == OTPActivation.MandateId);
            var response = "";
            var mandateToActivate = _context.OTPActivations.FirstOrDefault(m => m.ID == OTPActivation.ID);

            var content = $"{{\"remitaTransRef\":\"{mandateToActivate.RemitaTransRef}\"," +
                    $"\"authParams\": [" + 
                            $"{{\"param1\":\"OTP\"," +
                            $"\"value\":\"{OTPActivation.DescriptionOneValue}\"}}," +
                            $"{{\"param2\":\"CARD\"," +
                            $"\"value\":\"{OTPActivation.DescriptionTwoValue}\"}}" +
                        $"]" +
                        $"}}";

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

                    response = client.UploadString(Environment.GetEnvironmentVariable("VALIDATION_URL"), "POST", content);
                    response = response.Replace("jsonp (", "");
                    response = response.Replace(")", "");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception Occurred: {ex.Message}");
                }
            }

            OTPValidate res = JsonConvert.DeserializeObject<OTPValidate>(response);
            //OTPActivation.MandateId = res.mandateId;
            mandateToActivate.StatusMessage = res.status;

            //check if OTP request is successful
            if (res.statuscode == "00")
            {
                _context.OTPActivations.Update(mandateToActivate); //update value in database
                await _context.SaveChangesAsync(); //save changes to database.
                TempData["success"] = $"{mandateToActivate.StatusMessage}";
                return Page();
            }
            else
            {
                TempData["failure"]= $"{mandateToActivate.StatusMessage}";
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