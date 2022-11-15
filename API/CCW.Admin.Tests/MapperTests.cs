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
            result.AgencyLogo.Should().Be(agencyProfileRequestModel.AgencyLogo);
            result.ConvenienceFee.Should().Be(agencyProfileRequestModel.ConvenienceFee);
            result.CreditFee.Should().Be(agencyProfileRequestModel.CreditFee);
            result.InitialCost.Should().Be(agencyProfileRequestModel.InitialCost);
            result.PaymentURL.Should().Be(agencyProfileRequestModel.PaymentURL);
            result.RefreshTokenTime.Should().Be(agencyProfileRequestModel.RefreshTokenTime);
            result.ReserveCost.Should().Be(agencyProfileRequestModel.ReserveCost);
            result.StandardCost.Should().Be(agencyProfileRequestModel.StandardCost);
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
            result.AgencyLogo.Should().Be(agencyProfileModel.AgencyLogo);
            result.ConvenienceFee.Should().Be(agencyProfileModel.ConvenienceFee);
            result.CreditFee.Should().Be(agencyProfileModel.CreditFee);
            result.InitialCost.Should().Be(agencyProfileModel.InitialCost);
            result.PaymentURL.Should().Be(agencyProfileModel.PaymentURL);
            result.RefreshTokenTime.Should().Be(agencyProfileModel.RefreshTokenTime);
            result.ReserveCost.Should().Be(agencyProfileModel.ReserveCost);
            result.StandardCost.Should().Be(agencyProfileModel.StandardCost);
        }
    }
}