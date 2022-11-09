using CCW.Admin.Entities;
using CCW.Admin.Models;

namespace CCW.Admin.Mappers;

public class AgencyProfileRequestSettingsModelToEntityMapper : IMapper<AgencyProfileSettingsRequestModel, AgencyProfileSettings>
{
    public AgencyProfileSettings Map(AgencyProfileSettingsRequestModel source)
    {
        return new AgencyProfileSettings
        {
            Id = Guid.NewGuid().ToString(),
            AgencySheriffName = source.AgencySheriffName,
            AgencyName = source.AgencyName,
            ChiefOfPoliceName = source.ChiefOfPoliceName,
            PrimaryThemeColor = source.PrimaryThemeColor,
            SecondaryThemeColor = source.SecondaryThemeColor,
            AgencyLogo = source.AgencyLogo,
            ConvenienceFee = source.ConvenienceFee,
            CreditFee = source.CreditFee,
            InitialCost = source.InitialCost,
            PaymentURL = source.PaymentURL,
            RefreshTokenTime = source.RefreshTokenTime,
            ReserveCost = source.ReserveCost,
            StandardCost = source.StandardCost,
        };
    }
}