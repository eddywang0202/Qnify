using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qnify.Utility
{
    public class Database
    {
        private static IConfiguration Configuration { get; set; }
        public Database(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static string _default;

        public string Default => _default ?? (_default = GetDbAccess("Default"));

        private static string GetDbAccess(string database)
        {
            var databaseSetting = $"{Configuration[$"DbMapping:{database}"]}";
            var db = !string.IsNullOrEmpty(databaseSetting)
                ? $"{Configuration[$"ConnectionStrings:{databaseSetting}"]}"
                : $"{Configuration[$"ConnectionStrings:{database}"]}";
            return string.IsNullOrEmpty(db) ? $"{Configuration["ConnectionStrings:Default"]}" : db;
        }
    }
}
