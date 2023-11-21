using AutoMapper;
using CCW.Application.Clients;
using CCW.Application.Entities;
using CCW.Application.Enum;
using CCW.Application.Models;
using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CCW.Application.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PermitApplicationController : ControllerBase
{
    private readonly IDocumentServiceClient _documentHttpClient;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IPdfService _pdfService;
    private readonly ILogger<PermitApplicationController> _logger;
    private readonly IMapper _mapper;

    public PermitApplicationController(
        IDocumentServiceClient documentHttpClient,
        ICosmosDbService cosmosDbService,
        IPdfService pdfService,
        ILogger<PermitApplicationController> logger,
        IMapper mapper
        )
    {
        _documentHttpClient = documentHttpClient;
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _pdfService = pdfService;
        _logger = logger;
        _mapper = mapper;
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] PermitApplicationRequestModel permitApplication)
    {
        GetUserId(out string userId);
        permitApplication.UserId = userId;

        try
        {
            var existingApplication = await _cosmosDbService.GetAllOpenApplicationsForUserAsync(userId, cancellationToken: default);

            if (existingApplication.Any())
            {
                return Conflict("In progress application/s found. Please delete open application/s or update.");
            }

            var result = await _cosmosDbService.AddAsync(_mapper.Map<PermitApplication>(permitApplication), cancellationToken: default);

            return Ok(_mapper.Map<UserPermitApplicationResponseModel>(result));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound("An error occur while trying to create permit application.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpGet("getApplication")]
    public async Task<IActionResult> GetApplication(string applicationId)
    {
        GetUserId(out string userId);

        try
        {
            var result = await _cosmosDbService.GetLastApplicationAsync(userId, applicationId, cancellationToken: default);

            return (result != null) ? Ok(_mapper.Map<UserPermitApplicationResponseModel>(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user permit application.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpGet("getSSN")]
    public async Task<IActionResult> GetSSN()
    {
        GetUserId(out string userId);

        try
        {
            var result = await _cosmosDbService.GetSSNAsync(userId, cancellationToken: default);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user permit application.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpGet("getAgreementPdf")]
    public async Task<IActionResult> GetAgreementPdf(string agreement)
    {
        GetUserId(out string userId);

        var response = await _documentHttpClient.GetAgreementPDF(agreement, cancellationToken: default);
        response.EnsureSuccessStatusCode();

        var contentStream = await response.Content.ReadAsStreamAsync();

        Response.Headers.Add("Content-Disposition", "inline; filename=agreement.pdf");
        Response.Headers.Add("X-Content-Type-Options", "nosniff");

        return new FileStreamResult(contentStream, "application/pdf");

    }


    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserSSN")]
    public async Task<IActionResult> GetUserSSN(string userId)
    {
        try
        {
            var result = await _cosmosDbService.GetSSNAsync(userId, cancellationToken: default);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user permit application.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserApplication")]
    public async Task<IActionResult> GetUserApplication(string userEmailOrOrderId, bool isOrderId = false, bool isComplete = false)
    {
        try
        {
            var result = await _cosmosDbService.GetUserLastApplicationAsync(userEmailOrOrderId, isOrderId, isComplete, cancellationToken: default);

            return (result != null) ? Ok(_mapper.Map<PermitApplicationResponseModel>(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve specific user permit application.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpGet("getApplications")]
    public async Task<IActionResult> GetApplications(string userEmail)
    {
        GetUserId(out string userId);

        try
        {
            var responseModels = new List<UserPermitApplicationResponseModel>();
            var result = await _cosmosDbService.GetAllApplicationsAsync(userId, userEmail, cancellationToken: default);

            if (result.Any())
            {
                responseModels = _mapper.Map<List<UserPermitApplicationResponseModel>>(result);
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user permit applications.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserApplications")]
    public async Task<IActionResult> GetUserApplications(string userEmail)
    {
        try
        {
            var responseModels = new List<PermitApplicationResponseModel>();
            var result = await _cosmosDbService.GetAllUserApplicationsAsync(userEmail, cancellationToken: default);

            if (result.Any())
            {
                responseModels = _mapper.Map<List<PermitApplicationResponseModel>>(result);
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve all user permit applications.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var responseModels = new List<SummarizedPermitApplicationResponseModel>();

            var result = await _cosmosDbService.GetAllInProgressApplicationsSummarizedAsync(cancellationToken: default);

            if (result.Any())
            {
                responseModels = _mapper.Map<List<SummarizedPermitApplicationResponseModel>>(result);
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve all permit applications.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("search")]
    public async Task<IActionResult> Search(string searchValue)
    {
        try
        {
            var responseModels = new List<SummarizedPermitApplicationResponseModel>();
            var result = await _cosmosDbService.SearchApplicationsAsync(searchValue, cancellationToken: default);

            if (result.Any())
            {
                responseModels = _mapper.Map<List<SummarizedPermitApplicationResponseModel>>(result);
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to search all permit applications.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("updateApplication")]
    [HttpPut]
    public async Task<IActionResult> UpdateApplication([FromBody] UserPermitApplicationRequestModel application)
    {
        GetUserId(out string userId);

        try
        {
            application.UserId = userId;

            var existingApplication = await _cosmosDbService.GetLastApplicationAsync(userId,
                application.Id.ToString(),
                cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found or application already submitted.");
            }

            if (application.Application.PersonalInfo.Ssn.ToLower().Contains("xxx"))
            {
                application.Application.PersonalInfo.Ssn = existingApplication.Application.PersonalInfo.Ssn;
            }

            await _cosmosDbService.UpdateApplicationAsync(_mapper.Map<PermitApplication>(application), existingApplication, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            if (e.Message.Contains("Permit application"))
            {
                return NotFound(e.Message);
            }
            return NotFound("An error occur while trying to update permit application.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("updateAssignedMultipleUsersApplications")]
    [HttpPut]
    public async Task<IActionResult> UpdateAssignedMultipleUsersApplications(string[] applicationsIds, string assignedAdminUser)
    {
        try
        {
            GetAADUserName(out string userName);

            var existingApplications = await _cosmosDbService.GetMultipleApplicationsAsync(applicationsIds, cancellationToken: default);

            if (existingApplications == null)
            {
                return NotFound("Permit applications cannot be found.");
            }

            foreach (var application in existingApplications)
            {
                History[] history = new[]{
                new History
                    {
                        ChangeMadeBy =  userName,
                        Change = "Assigned to admin: " + assignedAdminUser,
                        ChangeDateTimeUtc = DateTime.UtcNow,
                    }
                };

                application.History = history;
                application.Application.AssignedTo = assignedAdminUser;
                await _cosmosDbService.UpdateUserApplicationAsync(application, cancellationToken: default);
            }

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to update permit applications.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getHistory")]
    public async Task<IActionResult> GetHistory(string applicationIdOrOrderId, bool isOrderId = false)
    {
        try
        {
            IEnumerable<HistoryResponseModel> responseModels = new List<HistoryResponseModel>();
            var result = await _cosmosDbService.GetApplicationHistoryAsync(applicationIdOrOrderId, cancellationToken: default, isOrderId);

            if (result.Any())
            {
                responseModels = _mapper.Map<List<HistoryResponseModel>>(result);
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve permit application history.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("updateUserApplication")]
    [HttpPut]
    public async Task<IActionResult> UpdateUserApplication([FromBody] PermitApplicationRequestModel application, string updatedSection)
    {
        try
        {
            GetAADUserName(out string userName);

            var existingApplication = await _cosmosDbService.GetUserApplicationAsync(application.Id.ToString(), cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            if (application.Application.PersonalInfo != null && application.Application.PersonalInfo.Ssn.ToLower().Contains("xxx"))
            {
                application.Application.PersonalInfo.Ssn = existingApplication.Application.PersonalInfo.Ssn;
            }

            History[] history = new[]{
                new History
                    {
                        ChangeMadeBy =  userName,
                        Change = updatedSection,
                        ChangeDateTimeUtc = DateTime.UtcNow,
                    }
                };

            application.History = history;

            await _cosmosDbService.UpdateUserApplicationAsync(_mapper.Map<PermitApplication>(application), cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to update permit application.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("noShowUserAppointment")]
    [HttpPut]
    public async Task<IActionResult> NoShowUserAppointment(string applicationId)
    {
        try
        {
            GetAADUserName(out string userName);

            var existingApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = "Set appointment to No Show",
                    ChangeDateTimeUtc = DateTime.UtcNow,
                }
            };

            existingApplication.Application.Status = ApplicationStatus.AppointmentNoShow;
            existingApplication.History = history;
            existingApplication.Application.AppointmentStatus = AppointmentStatus.NoShow;

            await _cosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to remove permit application appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("checkInUserAppointment")]
    [HttpPut]
    public async Task<IActionResult> CheckInUserAppointment(string applicationId)
    {
        try
        {
            GetAADUserName(out string userName);

            var existingApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = "Checked In appointment",
                    ChangeDateTimeUtc = DateTime.UtcNow,
                }
            };

            existingApplication.History = history;
            existingApplication.Application.AppointmentStatus = AppointmentStatus.CheckedIn;

            await _cosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to remove permit application appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("setUserAppointmentToScheduled")]
    [HttpPut]
    public async Task<IActionResult> SetUserAppointmentToScheduled(string applicationId)
    {
        try
        {
            GetAADUserName(out string userName);

            var existingApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = "Set user appointment to scheduled",
                    ChangeDateTimeUtc = DateTime.UtcNow,
                }
            };

            existingApplication.Application.Status = ApplicationStatus.ReadyForAppointment;
            existingApplication.History = history;
            existingApplication.Application.AppointmentStatus = AppointmentStatus.Scheduled;

            await _cosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to remove permit application appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("removeUserApplicationAppointment")]
    [HttpPut]
    public async Task<IActionResult> RemoveUserApplicationAppointment(string applicationId)
    {
        try
        {
            GetAADUserName(out string userName);

            var existingApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = "Removed appointment from " + existingApplication.Application.AppointmentDateTime,
                    ChangeDateTimeUtc = DateTime.UtcNow,
                }
            };

            existingApplication.History = history;
            existingApplication.Application.AppointmentDateTime = null;
            existingApplication.Application.AppointmentStatus = AppointmentStatus.NotScheduled;
            existingApplication.Application.AppointmentId = null;

            await _cosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to remove permit application appointment.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("updateUserAppointment")]
    [HttpPut]
    public async Task<IActionResult> UpdateUserAppointment(string applicationId, string appointmentDate, string appointmentId)
    {
        try
        {
            GetAADUserName(out string userName);

            var existingApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            DateTime utcTime = (DateTime)existingApplication.Application.AppointmentDateTime;
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
            string formattedTime = localTime.ToString("MM/dd/yyyy hh:mm tt");

            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = "Updated appointment from " + formattedTime + " PST",
                    ChangeDateTimeUtc = DateTime.UtcNow,
                }
            };

            existingApplication.History = history;
            existingApplication.Application.AppointmentDateTime = DateTime.Parse(appointmentDate, null, DateTimeStyles.RoundtripKind);
            existingApplication.Application.AppointmentStatus = AppointmentStatus.Scheduled;
            existingApplication.Application.AppointmentId = appointmentId;

            await _cosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to update permit application appointment.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("deleteApplication")]
    [HttpPut]
    public async Task<IActionResult> DeleteApplication(string applicationId)
    {
        GetUserId(out string userId);

        try
        {
            var existingApp = await _cosmosDbService.GetLastApplicationAsync(userId, applicationId, cancellationToken: default);

            if (existingApp == null)
            {
                return NotFound("Permit application cannot be found or has been completed and no longer can be deleted.");
            }

            if (existingApp.Application.IsComplete)
            {
                return NotFound("Permit application submitted changes cannot be deleted.");
            }

            await _cosmosDbService.DeleteApplicationAsync(userId, existingApp.Id.ToString(), cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            return NotFound("An error occur while trying to delete permit application.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("deleteUserApplication")]
    [HttpPut]
    public async Task<IActionResult> DeleteUserApplication(string applicationId)
    {
        try
        {
            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            await _cosmosDbService.DeleteUserApplicationAsync(userApplication.UserId, userApplication.Id.ToString(), cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete permit application.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printRevocationLetter")]
    [HttpPut]
    public async Task<IActionResult> PrintRevocationLetter(string applicationId, string fileName, string user, string reason, string date, bool shouldAddDownloadFilename = true)
    {
        try
        {
            GetAADUserName(out string userName);
            GetUserId(out string userId);

            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var outStream = await _pdfService.GetRevocationLetterMemoryStream(userApplication, user, userName, reason, date, fileName);

            FileStreamResult fileStreamResultDownload = new FileStreamResult(outStream, "application/pdf");

            if (shouldAddDownloadFilename)
            {
                fileStreamResultDownload.FileDownloadName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-") + fileName + ".pdf";
            }

            return fileStreamResultDownload;

        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to print the revocation letter.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printApplication")]
    [HttpPut]
    public async Task<IActionResult> PrintApplication(string applicationId, string fileName, bool shouldAddDownloadFilename = true)
    {
        try
        {
            GetAADUserName(out string userName);
            GetUserId(out string userId);

            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var outStream = await _pdfService.GetApplicationMemoryStream(userApplication, userName, fileName);

            FileStreamResult fileStreamResultDownload = new FileStreamResult(outStream, "application/pdf");

            if (shouldAddDownloadFilename)
            {
                fileStreamResultDownload.FileDownloadName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-") + fileName + ".pdf";
            }

            return fileStreamResultDownload;
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to print permit application.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printOfficialLicense")]
    [HttpPut]
    public async Task<IActionResult> PrintOfficialLicense(string applicationId, string fileName, bool shouldAddDownloadFilename = true)
    {
        try
        {
            GetAADUserName(out string userName);

            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var outStream = await _pdfService.GetOfficialLicenseMemoryStream(userApplication, userName, fileName);

            FileStreamResult fileStreamResultDownload = new FileStreamResult(outStream, "application/pdf");

            if (shouldAddDownloadFilename)
            {
                fileStreamResultDownload.FileDownloadName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-") + fileName + ".pdf";
            }

            return fileStreamResultDownload;
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            return NotFound("An error occur while trying to print official license.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printUnofficialLicense")]
    [HttpPut]
    public async Task<IActionResult> PrintUnofficialLicense(string applicationId, string fileName, bool shouldAddDownloadFilename = true)
    {
        try
        {
            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var outStream = await _pdfService.GetUnofficialLicenseMemoryStream(userApplication, fileName);

            FileStreamResult fileStreamResultDownload = new FileStreamResult(outStream, "application/pdf");

            if (shouldAddDownloadFilename)
            {
                fileStreamResultDownload.FileDownloadName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-") + fileName + ".pdf";
            }

            return fileStreamResultDownload;
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            return NotFound("An error occur while trying to print unofficial license.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printLiveScan")]
    [HttpPut]
    public async Task<IActionResult> PrintLiveScanForm(string applicationId, string fileName, bool shouldAddDownloadFilename = true)
    {
        try
        {
            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var outStream = await _pdfService.GetLivescanMemoryStream(userApplication, fileName);

            FileStreamResult fileStreamResultDownload = new FileStreamResult(outStream, "application/pdf");

            if (shouldAddDownloadFilename)
            {
                fileStreamResultDownload.FileDownloadName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-") + fileName + ".pdf";
            }

            return fileStreamResultDownload;
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            return NotFound("An error occur while trying to print live scan.");
        }
    }

    private void GetAADUserName(out string userName)
    {
        userName = HttpContext.User.Claims
            .Where(c => c.Type == "preferred_username").Select(c => c.Value)
            .FirstOrDefault();

        if (userName == null)
        {
            throw new ArgumentNullException("userName", "Invalid token.");
        }
    }

    private void GetUserId(out string userId)
    {
        userId = HttpContext.User.Claims
            .Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
            .Select(c => c.Value).FirstOrDefault();

        if (userId == null)
        {
            throw new ArgumentNullException("userId", "Invalid token.");
        }
    }
}