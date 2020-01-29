﻿using System;
using System.Threading.Tasks;
using IotHome.Service.Model;

namespace IotHome.Service.App.Services.Interfaces
{
    public interface IReadingsNotifier
    {
        event Func<SensorDetails, SensorReading, Task> NewReadingsAppeared;

        Task NotifyClientsAsync(string sensor, string name, DateTimeOffset date, decimal value);
    }
}