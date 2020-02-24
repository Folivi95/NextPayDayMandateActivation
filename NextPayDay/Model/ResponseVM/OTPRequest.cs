using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextPayDay.Model.ResponseVM
{
    public class OTPRequest
    {
        public string statuscode { get; set; }
        public string status { get; set; }
        public string remitaTransRef { get; set; }
        public IEnumerable<RequestAuthParams> authParams { get; set; }
    }

    public class RequestAuthParams
    {
        public string description1 { get; set; }
        public string description2 { get; set; }
    }
}
