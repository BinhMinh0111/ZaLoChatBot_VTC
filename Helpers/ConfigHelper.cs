using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ZaloOA_v2.Helpers
{
    public class ConfigHelper
    {
        private static ConfigHelper connString;
        private static ConfigHelper log;
        public string ConnStringValue { get; set; }
        public string logValue { get; set; }
        public ConfigHelper(IConfiguration configuration, string key)
        {
            this.ConnStringValue = configuration.GetValue<string>(key);
            this.logValue = configuration.GetValue<string>(key);
        }
        public static string ConnString (string key)
        {
            connString = GetConnectionString(key);
            return connString.ConnStringValue;
        }
        public static string Logtring(string key)
        {
            log = GetLogString(key);
            return log.logValue;
        }
        public static ConfigHelper GetConnectionString(string key)
        {
            var builder = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json", false,reloadOnChange:true).
                AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            var settings = new ConfigHelper(configuration.GetSection("ConnectionStrings"), key);

            return settings;
        }

        public static ConfigHelper GetLogString(string key)
        {
            var builder = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json", false, reloadOnChange: true).
                AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            var settings = new ConfigHelper(configuration.GetSection("Logging"), key);

            return settings;
        }

    }
}
