namespace IotHome.Service.App.Models
{
    public class ChartSensorSelector : CheckboxValue<string>
    {
        public ChartSensorSelector(string sensorName, bool isChecked) : base(sensorName, isChecked)
        {
        }
    }
}