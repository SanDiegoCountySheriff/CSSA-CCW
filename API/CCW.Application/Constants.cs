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
        { "Connecticut", "CT" },
        { "Delaware", "DE" },
        { "Florida", "FL" },
        { "Georgia", "GA" },
        { "Hawaii", "HI" },
        { "Idaho", "ID" },
        { "Illinois", "IL" },
        { "Indiana", "IN" },
        { "Iowa", "IA" },
        { "Kansas", "KS" },
        { "Kentucky", "KY" },
        { "Louisiana", "LA" },
        { "Maine", "ME" },
        { "Maryland", "MD" },
        { "Massachusetts", "MA" },
        { "Michigan", "MI" },
        { "Minnesota", "MN" },
        { "Mississippi", "MS" },
        { "Missouri", "MO" },
        { "Montana", "MT" },
        { "Nebraska", "NE" },
        { "Nevada", "NV" },
        { "New Hampshire", "NH" },
        { "New Jersey", "NJ" },
        { "New Mexico", "NM" },
        { "New York", "NY" },
        { "North Carolina", "NC" },
        { "North Dakota", "ND" },
        { "Ohio", "OH" },
        { "Oklahoma", "OK" },
        { "Oregon", "OR" },
        { "Pennsylvania", "PA" },
        { "Rhode Island", "RI" },
        { "South Carolina", "SC" },
        { "South Dakota", "SD" },
        { "Tennessee", "TN" },
        { "Texas", "TX" },
        { "Utah", "UT" },
        { "Vermont", "VT" },
        { "Virginia", "VA" },
        { "Washington", "WA" },
        { "West Virginia", "WV" },
        { "Wisconsin", "WI" },
        { "Wyoming", "WY" }
   };
}

