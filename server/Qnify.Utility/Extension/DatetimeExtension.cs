using System;

namespace Qnify.Utility.Extension
{
    public static class DatetimeExtension
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateTime(this long unixTimestamp)
        {
            var unixTimeStampInTicks = unixTimestamp * TimeSpan.TicksPerMillisecond;
            return new DateTime(Epoch.Ticks + unixTimeStampInTicks);
        }

        public static DateTime? ToDateTime(this long? unixTimestamp)
        {
            if (unixTimestamp == null)
                return null;
            return ToDateTime((long)unixTimestamp);
        }

        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            var unixTimestamp = dateTime.Ticks - Epoch.Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public static long? ToUnixTimestamp(this DateTime? dateTime)
        {
            if (dateTime == null)
                return null;
            return ToUnixTimestamp(((DateTime)dateTime).AddHours(8));
        }
    }
}
