namespace CCW.Common.RequestModels;

public class OrganizationHolidaysRequestModel
{
    public List<HolidayRequestModel> HolidayRequestModels { get; set; }
}

public class HolidayRequestModel
{
    public string Name { get; set; }
}
