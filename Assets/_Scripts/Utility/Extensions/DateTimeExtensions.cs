using System;

namespace Utility.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime SubtractSeconds(this DateTime dateTime, int seconds)
        {
            return dateTime.AddSeconds(-seconds);
        }
        
        public static DateTime SubtractMinutes(this DateTime dateTime, int minutes)
        {
            return dateTime.AddMinutes(-minutes);
        }
        
        public static DateTime SubtractHours(this DateTime dateTime, int hours)
        {
            return dateTime.AddHours(-hours);
        }
        
        public static DateTime SubtractDays(this DateTime dateTime, int days)
        {
            return dateTime.AddDays(-days);
        }
    }
}