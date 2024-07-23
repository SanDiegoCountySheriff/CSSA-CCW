using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using CCW.Common.Services;
using Microsoft.Azure.Cosmos;

namespace CCW.Application.Services;

public class AppointmentCosmosDbService : IAppointmentCosmosDbService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDatabaseContainerResolver _databaseContainerResolver;
    private readonly Container _appointmentContainer;
    private readonly Container _appointmentManagementContainer;

    public AppointmentCosmosDbService(
        IHttpContextAccessor contextAccessor,
        IDatabaseContainerResolver databaseContainerResolver,
        IConfiguration configuration
    )
    {
        _contextAccessor = contextAccessor;
        _databaseContainerResolver = databaseContainerResolver;

        var tenantId = _contextAccessor.HttpContext.Items["TenantId"] != null ? _contextAccessor.HttpContext.Items["TenantId"].ToString() : "";
        var appointmentContainerName = configuration.GetSection("CosmosDb").GetSection("AppointmentContainerName").Value;
        var appointmentManagementContainerName = configuration.GetSection("CosmosDb").GetSection("AppointmentManagementContainerName").Value;

        _appointmentContainer = _databaseContainerResolver.GetContainer(tenantId, appointmentContainerName);
        _appointmentManagementContainer = _databaseContainerResolver.GetContainer(tenantId, appointmentManagementContainerName);
    }

    public async Task<AppointmentWindow> CreateAppointment(AppointmentWindow appointmentWindow, CancellationToken cancellationToken)
    {
        return await _appointmentContainer.CreateItemAsync(appointmentWindow, new PartitionKey(appointmentWindow.Id.ToString()), null, cancellationToken);
    }

    public async Task DeleteAppointment(string appointmentId, CancellationToken cancellationToken)
    {
        await _appointmentContainer.DeleteItemAsync<AppointmentWindow>(appointmentId, new PartitionKey(appointmentId), null, cancellationToken);
    }

    public async Task<int> GetAppointmentLength(CancellationToken cancellationToken)
    {
        var appointmentManagement = await _appointmentManagementContainer.ReadItemAsync<AppointmentManagement>("1", new PartitionKey("1"), null, cancellationToken);

        return appointmentManagement.Resource.AppointmentLength;
    }
}
