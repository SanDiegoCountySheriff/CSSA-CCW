using AutoMapper;
using CCW.Application.Models;
using CCW.Application.Services.Contracts;
using CCW.Common.Enums;
using CCW.Common.Models;
using CCW.Common.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Application.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PermitApplicationController : ControllerBase
{
    private readonly IDocumentAzureStorage _documentService;
    private readonly IApplicationCosmosDbService _applicationCosmosDbService;
    private readonly IPdfService _pdfService;
    private readonly ILogger<PermitApplicationController> _logger;
    private readonly IMapper _mapper;

    public PermitApplicationController(
        IDocumentAzureStorage documentService,
        IApplicationCosmosDbService applicationCosmosDbService,
        IPdfService pdfService,
        ILogger<PermitApplicationController> logger,
        IMapper mapper
        )
    {
        _documentService = documentService;
        _applicationCosmosDbService = applicationCosmosDbService;
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
            var existingApplication = await _applicationCosmosDbService.GetAllOpenApplicationsForUserAsync(userId, cancellationToken: default);

            if (existingApplication.Any())
            {
                return Conflict("In progress application/s found. Please delete open application/s or update.");
            }

            var result = await _applicationCosmosDbService.AddAsync(_mapper.Map<PermitApplication>(permitApplication), cancellationToken: default);

            return Ok(_mapper.Map<UserPermitApplicationResponseModel>(result));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound("An error occur while trying to create permit application.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("putAddHistoricalApplication")]
    [HttpPut]
    public async Task<IActionResult> PutAddHistoricalApplication([FromBody] PermitApplicationRequestModel permitApplication)
    {
        try
        {
            var result = await _applicationCosmosDbService.AddHistoricalApplicationAsync(_mapper.Map<PermitApplication>(permitApplication), cancellationToken: default);

            return Ok(_mapper.Map<UserPermitApplicationResponseModel>(result));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound("An error occur while trying to create historical permit application.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpGet("getApplication")]
    public async Task<IActionResult> GetApplication(string applicationId)
    {
        GetUserId(out string userId);

        try
        {
            var result = await _applicationCosmosDbService.GetLastApplicationAsync(userId, applicationId, cancellationToken: default);

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
            var result = await _applicationCosmosDbService.GetSSNAsync(userId, cancellationToken: default);

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
    public async Task<IActionResult> GetAgreementPdf(string fileName)
    {
        try
        {
            var contentStream = await _documentService.GetAgreementPDF(fileName, cancellationToken: default);

            Response.Headers.Add("Content-Disposition", "inline; filename=agreement.pdf");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return new FileStreamResult(contentStream, "application/pdf");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve the agreement PDF.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserSSN")]
    public async Task<IActionResult> GetUserSSN(string userId)
    {
        try
        {
            var result = await _applicationCosmosDbService.GetSSNAsync(userId, cancellationToken: default);

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
            var result = await _applicationCosmosDbService.GetUserLastApplicationAsync(userEmailOrOrderId, isOrderId, isComplete, cancellationToken: default);

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
            var result = await _applicationCosmosDbService.GetAllApplicationsAsync(userId, userEmail, cancellationToken: default);

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
    [HttpGet("getApplicationSummaryCount")]
    public async Task<IActionResult> GetApplicationSummaryCount()
    {
        try
        {
            var result = await _applicationCosmosDbService.GetApplicationSummaryCount(cancellationToken: default);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to get the application summary count");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAssignedApplicationsSummary")]
    public async Task<IActionResult> GetAssignedApplicationsSummary()
    {
        try
        {
            GetAADName(out string name);

            List<AssignedApplicationSummary> result = await _applicationCosmosDbService.GetAssignedApplicationsSummary(name, cancellationToken: default);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to get the application summary count");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserApplications")]
    public async Task<IActionResult> GetUserApplications(string userEmail)
    {
        try
        {
            var responseModels = new List<PermitApplicationResponseModel>();
            var result = await _applicationCosmosDbService.GetAllUserApplicationsAsync(userEmail, cancellationToken: default);

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
    [HttpGet("getAllPermitsSummary")]
    public async Task<IActionResult> GetAllPermitsSummary([FromQuery] PermitsOptions options)
    {
        try
        {
            var (result, count) = await _applicationCosmosDbService.GetAllInProgressApplicationsSummarizedAsync(options, cancellationToken: default);

            var response = new SummaryResponse()
            {
                Items = result.ToList(),
                Total = count,
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var originalException = ex.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve all permit applications.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAllLegacyApplications")]
    public async Task<IActionResult> GetAllLegacyApplications([FromQuery] PermitsOptions options)
    {
        try
        {
            var (result, count) = await _applicationCosmosDbService.GetAllLegacyApplicationsAsync(options, cancellationToken: default);

            var response = new LegacySummaryResponse()
            {
                Items = result.ToList(),
                Total = count,
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var originalException = ex.GetBaseException();
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
            var result = await _applicationCosmosDbService.SearchApplicationsAsync(searchValue, cancellationToken: default);

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

            var existingApplication = await _applicationCosmosDbService.GetLastApplicationAsync(userId,
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

            await _applicationCosmosDbService.UpdateApplicationAsync(_mapper.Map<PermitApplication>(application), existingApplication, cancellationToken: default);

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

            var existingApplications = await _applicationCosmosDbService.GetMultipleApplicationsAsync(applicationsIds, cancellationToken: default);

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
                await _applicationCosmosDbService.UpdateUserApplicationAsync(application, cancellationToken: default);
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
            var result = await _applicationCosmosDbService.GetApplicationHistoryAsync(applicationIdOrOrderId, cancellationToken: default, isOrderId);

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

            var existingApplication = await _applicationCosmosDbService.GetUserApplicationAsync(application.Id.ToString(), cancellationToken: default);

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

            await _applicationCosmosDbService.UpdateUserApplicationAsync(_mapper.Map<PermitApplication>(application), cancellationToken: default);

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
    [Route("deleteUserApplication")]
    [HttpPut]
    public async Task<IActionResult> DeleteUserApplication(string applicationId)
    {
        try
        {
            var userApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            await _applicationCosmosDbService.DeleteUserApplicationAsync(userApplication.UserId, userApplication.Id.ToString(), cancellationToken: default);

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

            var userApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var outStream = await _pdfService.GetRevocationLetterMemoryStream(userApplication, user, userId, reason, date, fileName, userName);

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

            var userApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var outStream = await _pdfService.GetApplicationMemoryStream(userApplication, userId, fileName);

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

            var userApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

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
            var userApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

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
            var userApplication = await _applicationCosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

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

    private void GetAADName(out string name)
    {
        name = HttpContext.User.Claims
            .Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Select(c => c.Value).FirstOrDefault();

        if (name == null)
        {
            throw new ArgumentNullException("name", "Invalid token.");
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

    public class SummaryResponse
    {
        public List<SummarizedPermitApplication> Items { get; set; }
        public int Total { get; set; }
    }

    public class LegacySummaryResponse
    {
        public List<SummarizedLegacyApplication> Items { get; set; }
        public int Total { get; set; }
    }

    public class PermitsOptions
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string[] SortBy { get; set; }
        public bool[] SortDesc { get; set; }
        public string[] GroupBy { get; set; }
        public bool[] GroupDesc { get; set; }
        public ApplicationStatus[] Statuses { get; set; }
        public AppointmentStatus[] AppointmentStatuses { get; set; }
        public ApplicationType[] ApplicationTypes { get; set; }
        public string Search { get; set; }
        public string ApplicationSearch { get; set; }
        public bool ShowingTodaysAppointments { get; set; }
        public DateTimeOffset? SelectedDate { get; set; }
    }
}