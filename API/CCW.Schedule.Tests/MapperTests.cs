using CCW.Schedule.Entities;
using CCW.Schedule.Mappers;
using CCW.Schedule.Models;
using FluentAssertions;


namespace CCW.Schedule.Tests;

internal class MapperTests
{
    internal class AppointmentWindowCreateRequestModelToEntityMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            AppointmentWindowCreateRequestModel appointmentCreateRequestModel,
            AppointmentWindowCreateRequestModelToEntityMapper sut
        )
        {
            var result = sut.Map(appointmentCreateRequestModel);

            result.Status.Should().Be(appointmentCreateRequestModel.Status);
            result.Name.Should().Be(appointmentCreateRequestModel.Name);
            result.ApplicationId.Should().Be(appointmentCreateRequestModel.ApplicationId);
            result.Id.Should().NotBeEmpty();
            result.End.Should().Be(appointmentCreateRequestModel.End.ToString(Constants.DateTimeFormat));
            result.Start.Should().Be(appointmentCreateRequestModel.Start.ToString(Constants.DateTimeFormat));
            result.IsManuallyCreated.Should().Be(appointmentCreateRequestModel.IsManuallyCreated);
            result.Permit.Should().Be(appointmentCreateRequestModel.Permit);
            result.Payment.Should().Be(appointmentCreateRequestModel.Payment);
        }
    }

    internal class AppointmentWindowUpdateRequestModelToEntityMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            AppointmentWindowUpdateRequestModel appointmentUpdateRequestModel,
            AppointmentWindowUpdateRequestModelToEntityMapper sut
        )
        {
            appointmentUpdateRequestModel.Id = Guid.NewGuid().ToString();
            var result = sut.Map(appointmentUpdateRequestModel);

            result.Status.Should().Be(appointmentUpdateRequestModel.Status);
            result.Name.Should().Be(appointmentUpdateRequestModel.Name);
            result.ApplicationId.Should().Be(appointmentUpdateRequestModel.ApplicationId);
            result.Id.Should().Be(appointmentUpdateRequestModel.Id);
            result.End.Should().Be(appointmentUpdateRequestModel.End.ToString(Constants.DateTimeFormat));
            result.Start.Should().Be(appointmentUpdateRequestModel.Start.ToString(Constants.DateTimeFormat));
            result.IsManuallyCreated.Should().Be(appointmentUpdateRequestModel.IsManuallyCreated);
            result.Permit.Should().Be(appointmentUpdateRequestModel.Permit);
            result.Payment.Should().Be(appointmentUpdateRequestModel.Payment);
        }
    }

    internal class EntityToAppointmentWindowResponseModelMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            AppointmentWindow appointment,
            EntityToAppointmentWindowResponseModelMapper sut
        )
        {
            var result = sut.Map(appointment);

            result.Status.Should().Be(appointment.Status);
            result.Name.Should().Be(appointment.Name);
            result.ApplicationId.Should().Be(appointment.ApplicationId);
            result.Id.Should().Be(appointment.Id.ToString());
            result.End.Should().Be(appointment.End);
            result.Start.Should().Be(appointment.Start);
            result.IsManuallyCreated.Should().Be(appointment.IsManuallyCreated);
            result.Permit.Should().Be(appointment.Permit);
            result.Payment.Should().Be(appointment.Payment);
        }
    }
}