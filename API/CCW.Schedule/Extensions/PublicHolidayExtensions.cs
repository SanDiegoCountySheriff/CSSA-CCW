using PublicHoliday;

namespace CCW.Schedule.Extensions;

public static class PublicHolidayExtensions
{
    public static Holiday CezarChaves(this Holiday holiday, int year)
    {
        DateTime dateTime = new DateTime(year, 3, 31);
        return new Holiday(dateTime, FixWeekendSaturdayBeforeSundayAfter(dateTime), "CezarChaves");
    }

    public static DateTime FixWeekendSaturdayBeforeSundayAfter(DateTime hol)
    {
        if (hol.DayOfWeek == DayOfWeek.Sunday)
        {
            hol = hol.AddDays(1.0);
        }
        else if (hol.DayOfWeek == DayOfWeek.Saturday)
        {
            hol = hol.AddDays(-1.0);
        }

        return hol;
    }
}
