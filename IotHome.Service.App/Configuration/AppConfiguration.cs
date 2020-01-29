using System;

namespace IotHome.Service.App.Configuration
{
    public class AppConfiguration
    {
        public AppConfiguration(RawAppConfiguration config)
        {
            ApplicationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(config.ApplicationTimeZone);
        }

        public TimeZoneInfo ApplicationTimeZone { get; }
    }
}