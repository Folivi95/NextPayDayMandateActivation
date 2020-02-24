using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextPayDay.Model.ResponseVM
{
    public class OTPValidate
    {
        public string statuscode { get; set; }
        public string mandateId { get; set; }
        public string status { get; set; }
    }
}
