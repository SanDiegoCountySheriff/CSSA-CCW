using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;

namespace CCW.Schedule.Models;

public class AppointmentUploadModel
{
    [Index(0)]
    public string Id { get; set; }
    [Index(1)]
    public DateTime Start { get; set; }
    [Index(2)]
    public DateTime End { get; set; }
}

public class AppointmentUploadModelMap : ClassMap<AppointmentUploadModel>
{
    public AppointmentUploadModelMap()
    {
        Map(p => p.Id).Index(0);
        Map(p => p.Start).Index(1);
        Map(p => p.End).Index(2);
    }
}