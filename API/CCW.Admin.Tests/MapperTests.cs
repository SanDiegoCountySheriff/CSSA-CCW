using CCW.Admin.Mappers;
using CCW.Admin.Models;
using FluentAssertions;
using CCW.Admin.Entities;

namespace CCW.Admin.Tests;

internal class MapperTests
{
    internal class AgencyProfileRequestSettingsModelToEntityMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            AgencyProfileSettingsRequestModel agencyProfileRequestModel,
            AgencyProfileRequestSettingsModelToEntityMapper sut
        )
        {
            var result = sut.Map(agencyProfileRequestModel);

            result.Id.Should().NotBeEmpty();
            result.AgencySheriffName.Should().Be(agencyProfileRequestModel.AgencySheriffName);
            result.AgencyName.Should().Be(agencyProfileRequestModel.AgencyName);
            result.ChiefOfPoliceName.Should().Be(agencyProfileRequestModel.ChiefOfPoliceName);
            result.PrimaryThemeColor.Should().Be(agencyProfileRequestModel.PrimaryThemeColor);
            result.SecondaryThemeColor.Should().Be(agencyProfileRequestModel.SecondaryThemeColor);
            result.PaymentURL.Should().Be(agencyProfileRequestModel.PaymentURL);
            result.RefreshTokenTime.Should().Be(agencyProfileRequestModel.RefreshTokenTime);
            result.Cost.ConvenienceFee.Should().Be(agencyProfileRequestModel.Cost.ConvenienceFee);
            result.Cost.CreditFee.Should().Be(agencyProfileRequestModel.Cost.CreditFee);
            result.Cost.Issuance.Should().Be(agencyProfileRequestModel.Cost.Issuance);
            result.Cost.Modify.Should().Be(agencyProfileRequestModel.Cost.Modify);
            result.Cost.New.Reserve.Should().Be(agencyProfileRequestModel.Cost.New.Reserve);
            result.Cost.New.Standard.Should().Be(agencyProfileRequestModel.Cost.New.Standard);
            result.Cost.New.Reserve.Should().Be(agencyProfileRequestModel.Cost.New.Reserve);
            result.Cost.Renew.Reserve.Should().Be(agencyProfileRequestModel.Cost.Renew.Reserve);
            result.Cost.Renew.Standard.Should().Be(agencyProfileRequestModel.Cost.Renew.Standard);
            result.Cost.Renew.Reserve.Should().Be(agencyProfileRequestModel.Cost.Renew.Reserve);
        }
    }

    internal class EntityToAgencyProfileSettingsResponseModelMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            AgencyProfileSettings agencyProfileModel,
            EntityToAgencyProfileSettingsResponseModelMapper sut
        )
        {
            var result = sut.Map(agencyProfileModel);

            result.Id.Should().Be(agencyProfileModel.Id);
            result.AgencySheriffName.Should().Be(agencyProfileModel.AgencySheriffName);
            result.AgencyName.Should().Be(agencyProfileModel.AgencyName);
            result.ChiefOfPoliceName.Should().Be(agencyProfileModel.ChiefOfPoliceName);
            result.PrimaryThemeColor.Should().Be(agencyProfileModel.PrimaryThemeColor);
            result.SecondaryThemeColor.Should().Be(agencyProfileModel.SecondaryThemeColor);
            result.PaymentURL.Should().Be(agencyProfileModel.PaymentURL);
            result.RefreshTokenTime.Should().Be(agencyProfileModel.RefreshTokenTime);
            result.Cost.ConvenienceFee.Should().Be(agencyProfileModel.Cost.ConvenienceFee);
            result.Cost.CreditFee.Should().Be(agencyProfileModel.Cost.CreditFee);
            result.Cost.Issuance.Should().Be(agencyProfileModel.Cost.Issuance);
            result.Cost.Modify.Should().Be(agencyProfileModel.Cost.Modify);
            result.Cost.New.Reserve.Should().Be(agencyProfileModel.Cost.New.Reserve);
            result.Cost.New.Standard.Should().Be(agencyProfileModel.Cost.New.Standard);
            result.Cost.New.Reserve.Should().Be(agencyProfileModel.Cost.New.Reserve);
            result.Cost.Renew.Reserve.Should().Be(agencyProfileModel.Cost.Renew.Reserve);
            result.Cost.Renew.Standard.Should().Be(agencyProfileModel.Cost.Renew.Standard);
            result.Cost.Renew.Reserve.Should().Be(agencyProfileModel.Cost.Renew.Reserve);
        }
    }
}