using System.Collections.Generic;

namespace CCW.Application;

public class Constants
{
    public const string AppName = "Application";
    public const string ControllerAttributeName = AppName + "/[controller]";
    public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

    public static readonly Dictionary<string, string> StateAbbreviations = new Dictionary<string, string>()
    {
        { "Alabama", "AL" },
        { "Alaska", "AK" },
        { "Arizona", "AZ" },
        { "Arkansas", "AR" },
        { "California", "CA" },
        { "Colorado", "CO" },
        // Add more states here
    };
}

