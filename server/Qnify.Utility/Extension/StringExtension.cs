using System;

namespace Qnify.Utility
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return (value == string.Empty || value == null);
        }

        public static Guid ToGuid(this string value)
        {
            Guid guid;
            Guid.TryParse(value, out guid);
            return guid;
        }
    }
}
