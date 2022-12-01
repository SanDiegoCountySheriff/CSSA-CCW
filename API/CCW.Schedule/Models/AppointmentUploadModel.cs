using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;

namespace CCW.Schedule.Models;

public class AppointmentUploadModel
{
    [Index(0)]
    public string StartDate { get; set; }
    [Index(1)]
    public string StartTime { get; set; }
    [Index(2)]
    public string EndDate { get; set; }
    [Index(3)]
    public string EndTime { get; set; }
}

public class AppointmentUploadModelMap : ClassMap<AppointmentUploadModel>
{
    public AppointmentUploadModelMap()
    {
        Map(p => p.StartDate).Index(0);
        Map(p => p.StartTime).Index(1);
        Map(p => p.EndDate).Index(2);
        Map(p => p.EndTime).Index(3);
    }
}