namespace CCW.Schedule.Models;

public class OrganizationHolidaysRequestModel
{
    public List<HolidayRequestModel> HolidayRequestModels { get; set; }
}

public class HolidayRequestModel
{
    public string Name { get; set; }
}
