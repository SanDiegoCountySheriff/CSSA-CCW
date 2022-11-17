using CCW.UserProfile.Mappers;
using CCW.UserProfile.Entities;
using FluentAssertions;
using CCW.UserProfile.Models;

namespace CCW.UserProfile.Tests;

internal class MapperTests
{
    internal class EntityToUserProfileResponseModelMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            User request,
            EntityToUserProfileResponseModelMapper sut
        )
        {
            var result = sut.Map(request);

            result.Email.Should().Be(request.Email);
            result.Id.Should().Be(request.Id);
        }
    }

    internal class UserProfileRequestModelToEntityMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            UserProfileRequestModel request,
            UserProfileRequestModelToEntityMapper sut
        )
        {
            var result = sut.Map(request);

            result.Email.Should().Be(request.EmailAddress);
            result.Id.Should().NotBeEmpty();
        }
    }
}