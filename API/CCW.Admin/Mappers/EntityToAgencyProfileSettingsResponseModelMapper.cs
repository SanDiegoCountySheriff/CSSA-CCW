using CCW.Admin.Entities;
using CCW.Admin.Models;

namespace CCW.Admin.Mappers;

public class EntityToAgencyProfileSettingsResponseModelMapper : IMapper<AgencyProfileSettings, AgencyProfileSettingsResponseModel>
{
    public AgencyProfileSettingsResponseModel Map(AgencyProfileSettings source)
    {
        return new AgencyProfileSettingsResponseModel
        {
            Id = source.Id,
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