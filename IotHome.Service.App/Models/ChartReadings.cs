using System;
using System.Collections.Generic;
using System.Linq;
using IotHome.Service.Model;

namespace IotHome.Service.App.Models
{
    public class ChartReadings
    {
        public ChartReadings(MyChartColor color, IEnumerable<SensorReading> readings)
        {
            Color = color;

            var list = readings.ToList();
            Values = list.Select(r => r.Value).ToList();
            Labels = list.Select(r => r.Date.ToString(GetTimeFormat(list))).ToList();
        }

        public MyChartColor Color { get; }

        public IList<decimal?> Values { get; }

        public IList<string> Labels { get; }

        private string GetTimeFormat(IList<SensorReading> readings)
        {
            if (readings.Any())
            {
                var span = readings.Last().Date - readings.First().Date;
                if (span <= TimeSpan.FromDays(1))
                {
                    return "HH:mm";
                }
            }

            return "yyyy-MM-dd HH:mm";
        }
    }
}