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
            PaymentURL = source.PaymentURL,
            RefreshTokenTime = source.RefreshTokenTime,
            Cost = new Cost
            {
                ConvenienceFee = source.Cost.ConvenienceFee,
                CreditFee = source.Cost.CreditFee,
                Issuance = source.Cost.Issuance,
                Modify = source.Cost.Modify,
                New = new CostType
                {
                    Judicial = source.Cost.New.Judicial,
                    Reserve = source.Cost.New.Reserve,
                    Standard = source.Cost.New.Standard,
                },
                Renew = new CostType
                {
                    Judicial = source.Cost.Renew.Judicial,
                    Reserve = source.Cost.Renew.Reserve,
                    Standard = source.Cost.Renew.Standard,
                },
            },
        };
    }
}