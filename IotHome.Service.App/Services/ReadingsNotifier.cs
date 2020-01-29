using System;
using System.Threading.Tasks;
using IotHome.Service.App.Services.Interfaces;
using IotHome.Service.Model;

namespace IotHome.Service.App.Services
{
    public class ReadingsNotifier : IReadingsNotifier
    {
        public event Func<SensorDetails, SensorReading, Task> NewReadingsAppeared;

        public async Task NotifyClientsAsync(string sensor, string name, DateTimeOffset date, decimal value)
        {
            if (NewReadingsAppeared != null)
            {
                await NewReadingsAppeared.Invoke(new SensorDetails(sensor, name), new SensorReading(date, value));
            }
        }
    }
}