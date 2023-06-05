using CCW.Admin.Entities;
using CCW.Admin.Models;

namespace CCW.Admin.Mappers;

public class AgencyProfileRequestSettingsModelToEntityMapper : IMapper<AgencyProfileSettingsRequestModel, AgencyProfileSettings>
{
    public AgencyProfileSettings Map(AgencyProfileSettingsRequestModel source)
    {
        return new AgencyProfileSettings
        {
            Id = source.Id,
            AgencySheriffName = source.AgencySheriffName,
            AgencyName = source.AgencyName,
            ChiefOfPoliceName = source.ChiefOfPoliceName,
            PrimaryThemeColor = source.PrimaryThemeColor,
            SecondaryThemeColor = source.SecondaryThemeColor,
            PaymentURL = source.PaymentURL,
            RefreshTokenTime = source.RefreshTokenTime,
            AgencyEmail = source.AgencyEmail,
            AgencyFax = source.AgencyFax,
            AgencyTelephone = source.AgencyTelephone,
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
            Courthouse = source.Courthouse,
            LocalAgencyNumber = source.LocalAgencyNumber,
            ORI = source.ORI,
            AgencyBillingNumber = source.AgencyBillingNumber,
            ContactName = source.ContactName,
            ContactNumber = source.ContactNumber,
            MailCode = source.MailCode,
            AgencyStreetAddress = source.AgencyStreetAddress,
            AgencyAptBuildingNumber = source.AgencyAptBuildingNumber,
            AgencyCity = source.AgencyCity,
            AgencyState = source.AgencyState,
            AgencyZip = source.AgencyZip,
            AgencyCounty = source.AgencyCounty,
            AgencyShippingStreetAddress = source.AgencyShippingStreetAddress,
            AgencyShippingAptBuildingNumber = source.AgencyShippingAptBuildingNumber,
            AgencyShippingCity = source.AgencyShippingCity,
            AgencyShippingState = source.AgencyShippingState,
            AgencyShippingZip = source.AgencyShippingZip,
            AgencyShippingCounty = source.AgencyShippingCounty,
        };
    }
}