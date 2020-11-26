using System;

namespace Qnify.Utility
{
    public static class DefaultValue
    {
        public static string Role => "member";
        public static string GuestRole => "guest";
        public static DateTime DatetimeTomorrow => DateTime.UtcNow.AddDays(1);     
    }
}
