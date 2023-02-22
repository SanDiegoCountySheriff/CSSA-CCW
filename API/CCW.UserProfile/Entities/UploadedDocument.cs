namespace CCW.UserProfile.Entities;

public class UploadedDocument
{
    public string Name { get; set; }
    public DateTime UploadedDateTimeUtc { get; set; }
    public string UploadedBy { get; set; }
    public string DocumentType { get; set; }
}