namespace IotHome.Service.App.Configuration
{
    public class RawAppConfiguration
    {
        public string ApplicationTimeZone { get; set; }

        public string Secret { get; set; }

        public int ReadingsSamplingMinutes { get; set; }
    }
}
