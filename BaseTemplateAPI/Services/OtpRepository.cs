using BaseTemplateAPI.Entity.Redis;
using BaseTemplateAPI.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Services
{
    public class OtpRepository : IOtpRepository
    {
        private readonly IDatabase _database;
        public OtpRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase(0);
        }

        public async Task<bool> DeleteOtpAsync(string token)
        {
            token = "otp." + token;
            return await _database.KeyDeleteAsync(token);
        }

        public async Task<LoginOtp> GetOtpAsync(string token)
        {
            token = "otp." + token;

            var data = await _database.StringGetAsync(token);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<LoginOtp>(data);
        }

        public async Task<LoginOtp> UpdateOtpAsync(LoginOtp loginOtp)
        {
            string token = "otp." + loginOtp.Token;
            string userToken = "otp." + loginOtp.UserId;

            var result = await GetOtpAsync(loginOtp.UserId);

            if (result != null)
            {
                return result;
            }

            //The time limit could be set via appsettings.json but as we want this to be fast,
            //we decided to setup the time limit at compile time rather than run time
            var created = await _database.StringSetAsync(token, JsonSerializer.Serialize(loginOtp), DateTime.Now.AddDays(1).Date.Subtract(DateTime.Now));

            if (!created)
            {
                return null;
            }

            created = await _database.StringSetAsync(userToken, JsonSerializer.Serialize(loginOtp), DateTime.Now.AddDays(1).Date.Subtract(DateTime.Now));

            if (!created)
            {
                _ = await DeleteOtpAsync(token);
                return null;
            }

            return await GetOtpAsync(loginOtp.Token);
        }

        public async Task<TimeSpan?> GetTTL(string token)
        {
            token = "otp." + token;
            return await _database.KeyTimeToLiveAsync(token);
        }
    }
}
