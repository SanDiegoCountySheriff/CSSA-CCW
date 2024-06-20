using CCW.Common.Models;

namespace CCW.Schedule.Services.Contracts;

public interface IAppointmentCosmosDbService
{
    Task<AppointmentWindow> GetAsync(string applicationId, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> ResetApplicantAppointmentsAsync(string applicationId, CancellationToken cancellationToken);
    Task<AppointmentWindow> GetAppointmentByIdAsync(string appointmentId, CancellationToken cancellationToken);
    Task<AppointmentWindow> GetAppointmentByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetAvailableTimesAsync(bool includePastAppointments, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetBookedAppointmentsAsync(bool includePastAppointments, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetAvailableSlotByDateTime(DateTimeOffset startTime, CancellationToken cancellationToken);
    Task<List<AppointmentWindow>> GetAllBookedAppointmentsAsync(CancellationToken cancellationToken);
    Task<AppointmentWindow> AddAsync(AppointmentWindow appointment, CancellationToken cancellationToken);
    Task AddAvailableTimesAsync(List<AppointmentWindow> appointments, CancellationToken cancellationToken);
    Task<AppointmentWindow> UpdateAsync(AppointmentWindow appointment, CancellationToken cancellationToken);
    Task AddDeleteTimeSlotsAsync(List<AppointmentWindow> appointments, bool isCreateAction,
        CancellationToken cancellationToken);
    Task DeleteAsync(string appointmentId, CancellationToken cancellationToken);
    Task<(int, int)> CreateAppointmentsFromAppointmentManagementTemplate(AppointmentManagement appointmentManagement, CancellationToken cancellationToken);
    Task<int> DeleteAllAppointmentsByDate(DateTimeOffset date, CancellationToken cancellationToken);
    Task<int> DeleteAppointmentsByTimeSlot(DateTimeOffset date, CancellationToken cancellationToken);
    Task<int> GetNumberOfNewAppointments(int numberOfDays, CancellationToken cancellationToken);
    Task<string> GetNextAvailableAppointment();
    Task AddOrganizationalHoliday(OrganizationHolidays organizationalHolidays, CancellationToken cancellationToken);
    Task<OrganizationHolidays> GetOrganizationalHolidays();
    Task<AppointmentManagement> GetAppointmentManagementTemplate();
}