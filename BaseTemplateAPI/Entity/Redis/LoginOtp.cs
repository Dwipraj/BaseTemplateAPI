using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Entity.Redis
{
    public class LoginOtp
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Otp { get; set; }
    }
}
