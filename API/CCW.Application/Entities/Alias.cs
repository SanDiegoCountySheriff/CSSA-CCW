namespace CCW.Application.Entities
{
    public class Alias
    {
        public string PrevLastName { get; set; }
        public string PrevFirstName { get; set; }
        public string? PrevMiddleName { get; set; }
        public string? PrevSuffix { get; set; }
        public string? CityWhereChanged { get; set; }
        public string? StateWhereChanged { get; set; }
        public string? CourtFileNumber { get; set; }
    }
}
