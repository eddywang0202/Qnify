using Microsoft.Extensions.Configuration;

namespace Qnify.Utility
{
    public static class Config
    {
        public static IConfiguration AppSettings { get; set; }

        public static IConfiguration GetConfiguration()
        {
            return AppSettings;
        }
    }
}
