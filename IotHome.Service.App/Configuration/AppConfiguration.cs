using System;

namespace IotHome.Service.App.Configuration
{
    public class AppConfiguration
    {
        public AppConfiguration(RawAppConfiguration config)
        {
            ApplicationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(config.ApplicationTimeZone);
            Secret = config.Secret;
            ReadingsSamplingMinutes = config.ReadingsSamplingMinutes;
        }

        public TimeZoneInfo ApplicationTimeZone { get; }

        public string Secret { get; }

        public int ReadingsSamplingMinutes { get; }
    }
}