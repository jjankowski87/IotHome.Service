using System.Collections.Generic;
using System.Linq;
using IotHome.Service.Model;

namespace IotHome.Service.App.Models
{
    public class ChartReadings
    {
        private readonly IList<SensorReading> _readings;

        public ChartReadings(MyChartColor color, IEnumerable<SensorReading> readings)
        {
            Color = color;
            _readings = readings.ToList();
        }

        public MyChartColor Color { get; }

        public IEnumerable<decimal?> Values => _readings.Select(r => r.Value);

        public IEnumerable<string> Labels => _readings.Select(r => r.Date.ToString("yyyy-MM-dd HH:mm"));
    }
}