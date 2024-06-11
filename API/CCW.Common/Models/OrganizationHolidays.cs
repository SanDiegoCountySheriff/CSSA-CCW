using Newtonsoft.Json;

namespace CCW.Common.Models;

public class OrganizationHolidays
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    public List<OrganizationalHoliday> Holidays { get; set; }
}

public class OrganizationalHoliday
{
    public string Name { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
}
