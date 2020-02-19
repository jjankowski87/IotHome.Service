using System.Collections.Generic;
using System.Linq;
using IotHome.Service.Model;

namespace IotHome.Service.App.Models
{
    public class ChartData : Dictionary<ChartSensorSelector, ChartReadings>
    {
        public ChartData(IEnumerable<KeyValuePair<ChartSensorSelector, IEnumerable<SensorReading>>> chartData, IEnumerable<MyChartColor> colors)
            : base(chartData.Zip(colors).ToDictionary(d => d.First.Key, d => new ChartReadings(d.Second, d.First.Value)))
        {
        }

        public bool IsAnySelected => this.Any(x => x.Key.IsChecked);

        public IEnumerable<string> Labels => this.Any() ? this.First().Value.Labels : Enumerable.Empty<string>();

        public IEnumerable<(string, ChartReadings)> SelectedReadings =>
            this.Where(d => d.Key.IsChecked).Select(d => (d.Key.Value, d.Value));
    }
}
