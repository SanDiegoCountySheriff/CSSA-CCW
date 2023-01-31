using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;

namespace CCW.Schedule.Models;

public class AppointmentUploadModel
{
    [Index(0)]
    public string Date { get; set; }
    [Index(1)]
    public string Time { get; set; }
    [Index(2)]
    public int Duration { get; set; }
    [Index(3)]
    public int Slots { get; set; }
    [Index(4)]
    public string Action { get; set; }
}

public class AppointmentUploadModelMap : ClassMap<AppointmentUploadModel>
{
    public AppointmentUploadModelMap()
    {
        Map(p => p.Date).Index(0);
        Map(p => p.Time).Index(1);
        Map(p => p.Duration).Index(2);
        Map(p => p.Slots).Index(3);
        Map(p => p.Action).Index(4);
    }
}