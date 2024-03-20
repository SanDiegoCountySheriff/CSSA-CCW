using System;

namespace CCW.Common.Models;

public class UploadedDocument
{
    public string Name { get; set; }
    public DateTimeOffset UploadedDateTimeUtc { get; set; }
    public string UploadedBy { get; set; }
    public string DocumentType { get; set; }
}