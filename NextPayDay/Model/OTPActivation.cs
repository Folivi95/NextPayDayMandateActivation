using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextPayDay.Model
{
    public class OTPActivation
    {
        public int ID { get; set; }
        public string MandateId { get; set; }
        public string RequestId { get; set; }
        public string RemitaTransRef { get; set; }
        public string CardNumber { get; set; }
        public string OTP { get; set; }
        public string StatusMessage { get; set; }
        public string DescriptionOne { get; set; }
        public string DescriptionTwo { get; set; }
        public string DescriptionOneValue { get; set; }
        public string DescriptionTwoValue { get; set; }

    }
}
