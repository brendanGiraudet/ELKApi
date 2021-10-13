using System;

namespace ELKApi.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTimestamp(this DateTime dateTime)
        {
            var baseDate = new DateTime(1970, 01, 01);
            return ((int)dateTime.Subtract(baseDate).TotalSeconds).ToString();
        }
    }
}
