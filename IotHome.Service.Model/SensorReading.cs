using System;

namespace IotHome.Service.Model
{
    public class SensorReading
    {
        public SensorReading(DateTimeOffset date, decimal? value)
        {
            Date = date;
            Value = value;
        }

        public DateTimeOffset Date { get; }

        public  decimal? Value { get; }
    }
}