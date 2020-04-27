using System;
using Umbraco.Core;

namespace Vendr.Portal.Helpers
{
    public static class TimeBasedCodeHelper
    {
        public static string GenerateCode(int validForMinutes = 60)
        {
            var maxResetTime = DateTime.UtcNow.AddMinutes(validForMinutes);

            return (Guid.NewGuid().ToString("N").Substring(0, 10) + maxResetTime.Ticks.ToString()).ToUrlBase64();
        }

        public static DateTime? ParseCode(string code)
        {
            var unencoded = code.FromUrlBase64();
            if (unencoded.Length <= 10)
                return null;

            if (!long.TryParse(unencoded.Substring(10), out long ticks))
                return null;

            return new DateTime(ticks);
        }

        public static bool ValidateCode(string code)
        {
            if (code.IsNullOrWhiteSpace())
                return false;

            var maxResetTime = ParseCode(code);
            if (maxResetTime == null || maxResetTime < DateTime.UtcNow)
                return false;

            return true;
        }
    }
}