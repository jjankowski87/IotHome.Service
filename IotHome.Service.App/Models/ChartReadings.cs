using System;
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

        public IEnumerable<string> Labels => _readings.Select(r => r.Date.ToString(GetTimeFormat()));

        private string GetTimeFormat()
        {
            if (_readings.Any())
            {
                var span = _readings.Last().Date - _readings.First().Date;
                if (span <= TimeSpan.FromDays(1))
                {
                    return "HH:mm";
                }
            }

            return "yyyy-MM-dd HH:mm";
        }
    }
}