﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IotHome.Service.App.Configuration;
using IotHome.Service.App.Services.Interfaces;
using IotHome.Service.App.Utils;
using IotHome.Service.Model;
using IotHome.Service.Services.Interfaces;

namespace IotHome.Service.App.Services
{
    public class ReadingsService : IReadingsService
    {
        private const int DefaultWindowMinutes = 60;
        private static readonly IDictionary<TimeSpan, TimeSpan> SamplingWindows = new Dictionary<TimeSpan, TimeSpan>
        {
            { TimeSpan.FromHours(2), TimeSpan.FromMinutes(5) },
            { TimeSpan.FromHours(4), TimeSpan.FromMinutes(10) },
            { TimeSpan.FromHours(8), TimeSpan.FromMinutes(15) },
            { TimeSpan.FromHours(24), TimeSpan.FromMinutes(30) }
        };

        private readonly IStorageExplorer _storageExplorer;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly AppConfiguration _configuration;

        public ReadingsService(IStorageExplorer storageExplorer, IDateTimeProvider dateTimeProvider, AppConfiguration configuration)
        {
            _storageExplorer = storageExplorer;
            _dateTimeProvider = dateTimeProvider;
            _configuration = configuration;
        }

        public async Task<IDictionary<SensorDetails, SensorReading>> GetLatestReadingsAsync()
        {
            var utcNow = _dateTimeProvider.UtcNow;
            var readings = await _storageExplorer.ListSensorDetailsAsync(utcNow.AddMinutes(-30), utcNow.AddHours(1));

            return readings.GroupBy(r => new SensorDetails(r.Body.Sensor, r.Body.Name))
                .ToDictionary(g => g.Key, GetLatestReading);
        }

        public async Task<IDictionary<SensorDetails, IList<SensorReading>>> GetReadingsAsync(DateTime fromDate, DateTime toDate)
        {
            if (toDate < fromDate || (toDate - fromDate) > TimeSpan.FromDays(7))
            {
                return new Dictionary<SensorDetails, IList<SensorReading>>();
            }

            var samplingWindow = SelectSamplingWindow(fromDate, toDate);
            var from = NormalizeDateTime(fromDate, samplingWindow);
            var to = NormalizeDateTime(toDate, samplingWindow);

            var readings = await _storageExplorer.ListSensorDetailsAsync(from.AddMinutes(-samplingWindow), to.AddMinutes(samplingWindow));

            return readings.GroupBy(r => new SensorDetails(r.Body.Sensor, r.Body.Name))
                .ToDictionary(g => g.Key, g => (IList<SensorReading>)AdjustReadings(g.ToList(), from, to, samplingWindow).ToList());
        }

        private IEnumerable<SensorReading> AdjustReadings(IList<IotMessage<Reading>> readings, DateTimeOffset from, DateTimeOffset to, int samplingWindow)
        {
            var window = (to - from).TotalMinutes;

            var result = new List<SensorReading>();
            var lastIndex = 0;
            var index = 0;

            foreach (var readingTime in Enumerable.Range(0, (int)Math.Ceiling(window / samplingWindow) + 1).Select(i => from.AddMinutes(i * samplingWindow)))
            {
                var matching = readings.Where(r => Math.Abs((r.EnqueuedTimeUtc - readingTime).TotalMinutes) <= Math.Ceiling(samplingWindow / 2f)).ToList();
                var localTime = TimeZoneInfo.ConvertTime(readingTime, _configuration.ApplicationTimeZone);

                var readingValue = matching.Any() ? Math.Round(matching.Average(m => m.Body.Value), 1) : (decimal?) null;
                result.Add(new SensorReading(localTime, readingValue));

                if (readingValue != null)
                {
                    lastIndex = index;
                }

                index++;
            }

            while (result.Count - 1 > lastIndex)
            {
                result.RemoveAt(result.Count - 1);
            }

            return result;
        }

        private DateTimeOffset NormalizeDateTime(DateTime dateTime, int samplingWindow)
        {
            DateTimeOffset utc;
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                utc = new DateTimeOffset(dateTime, TimeSpan.Zero);
            }
            else
            {
                var unspecified = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
                utc = new DateTimeOffset(unspecified, _configuration.ApplicationTimeZone.GetUtcOffset(unspecified))
                    .ToUniversalTime();
            }

            var normalizedMinutes = Math.Round(utc.TimeOfDay.TotalMinutes / samplingWindow) * samplingWindow;
            return utc.Add(-utc.TimeOfDay).AddMinutes(normalizedMinutes);
        }

        private static int SelectSamplingWindow(DateTime fromDate, DateTime toDate)
        {
            var window = (toDate - fromDate).TotalMinutes;
            var matchingWindow = SamplingWindows.FirstOrDefault(sw => window <= sw.Key.TotalMinutes);
            if (matchingWindow.Equals(default(KeyValuePair<TimeSpan, TimeSpan>)))
            {
                return DefaultWindowMinutes;
            }

            return (int)matchingWindow.Value.TotalMinutes;
        }

        private static SensorReading GetLatestReading(IEnumerable<IotMessage<Reading>> readings)
        {
            var latest = readings.OrderByDescending(r => r.EnqueuedTimeUtc).First();
            return new SensorReading(latest.EnqueuedTimeUtc, latest.Body.Value);
        }
    }
}
