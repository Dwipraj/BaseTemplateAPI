using BaseTemplateAPI.Entity.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Interfaces
{
    interface IOtpRepository
    {
        Task<LoginOtp> GetOtpAsync(string token);
        Task<LoginOtp> UpdateOtpAsync(LoginOtp loginOtp);
        Task<bool> DeleteOtpAsync(string token);
        Task<TimeSpan?> GetTTL(string token);
    }
}
