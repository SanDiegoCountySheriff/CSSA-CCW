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

            result.UserEmail.Should().Be(request.UserEmail);
            result.Id.Should().Be(request.Id);
        }
    }

    internal class UserProfileRequestModelToEntityMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            string userId,
            UserProfileRequestModel request,
            UserProfileRequestModelToEntityMapper sut
        )
        {
            var result = sut.Map(userId, request);

            result.UserEmail.Should().Be(request.EmailAddress);
            result.Id.Should().Be(userId);
        }
    }
}