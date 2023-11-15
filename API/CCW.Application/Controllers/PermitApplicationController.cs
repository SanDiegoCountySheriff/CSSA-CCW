using AutoMapper;
using CCW.Application.Clients;
using CCW.Application.Entities;
using CCW.Application.Enum;
using CCW.Application.Models;
using CCW.Application.Services.Contracts;
using CCW.Common.Models;
using iText.Forms;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace CCW.Application.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PermitApplicationController : ControllerBase
{
    private readonly IDocumentServiceClient _documentHttpClient;
    private readonly IAdminServiceClient _adminHttpClient;
    private readonly IUserProfileServiceClient _userProfileServiceClient;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IPdfService _pdfService;
    private readonly ILogger<PermitApplicationController> _logger;
    private readonly IMapper _mapper;

    public PermitApplicationController(
        IDocumentServiceClient documentHttpClient,
        IAdminServiceClient adminHttpClient,
        IUserProfileServiceClient userProfileServiceClient,
        ICosmosDbService cosmosDbService,
        IPdfService pdfService,
        ILogger<PermitApplicationController> logger,
        IMapper mapper
        )
    {
        _documentHttpClient = documentHttpClient;
        _adminHttpClient = adminHttpClient;
        _userProfileServiceClient = userProfileServiceClient;
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

            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = "Updated appointment from " + existingApplication.Application.AppointmentDateTime,
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

    private static string BuildApplicantFullName(PermitApplication userApplication)
    {
        return (userApplication.Application.PersonalInfo?.FirstName + " " +
                                   userApplication.Application.PersonalInfo?.MiddleName + " " +
                                   userApplication.Application.PersonalInfo?.LastName + " " +
                                   userApplication.Application.PersonalInfo?.Suffix).Trim();
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
            var adminResponse = await _adminHttpClient.GetAgencyProfileSettingsAsync(cancellationToken: default);
            var response = await _documentHttpClient.GetUnofficialLicenseTemplateAsync(cancellationToken: default);

            Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            MemoryStream outStream = new MemoryStream();

            PdfReader pdfReader = new PdfReader(streamToReadFrom);
            PdfWriter pdfWriter = new PdfWriter(outStream);
            PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

            Document docFileAll = new Document(doc);
            pdfWriter.SetCloseStream(false);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(doc, true);
            form.SetGenerateAppearance(true);

            await AddApplicantSignatureImageForUnOfficial(userApplication, docFileAll);
            await AddApplicantThumbprintImageForUnOfficial(userApplication, docFileAll);
            await AddApplicantPhotoImageForUnOfficial(userApplication, docFileAll);
            await AddSheriffLogoForUnOfficial(docFileAll);
            await AddSheriffIssuingOfficierSignatureImageForUnOfficial(docFileAll);

            form.GetField("AGENCY_NAME").SetValue(adminResponse.AgencyName ?? "", true);
            form.GetField("AGENCY_ORI").SetValue(adminResponse.ORI ?? "", true);
            form.GetField("LOCAL_AGENCY_NUMBER").SetValue(adminResponse.LocalAgencyNumber ?? "", true);
            string fullname = BuildApplicantFullName(userApplication);
            form.GetField("APPLICANT_NAME").SetValue(fullname.Trim(), true);

            string residenceAddress1 = userApplication.Application.CurrentAddress?.AddressLine1;
            string residenceAddress2 = userApplication.Application.CurrentAddress?.AddressLine2;
            if (residenceAddress2 != null)
            {
                residenceAddress1 = residenceAddress1 + ", " + residenceAddress2;
            }
            form.GetField("APPLICATION_ADDRESS_LINE_1").SetValue(residenceAddress1 ?? "", true);
            string residenceAddress3 = userApplication.Application.CurrentAddress?.City
                                       + ", " + userApplication.Application.CurrentAddress?.State
                                       + " " + userApplication.Application.CurrentAddress?.Zip;
            form.GetField("APPLICATION_ADDRESS_LINE_2").SetValue(residenceAddress3 ?? "", true);
            string licenseType = userApplication.Application.ApplicationType?.ToString();
            licenseType = char.ToUpper(licenseType[0]) + licenseType.Substring(1);
            form.GetField("LICENSE_TYPE").SetValue(licenseType ?? "", true);
            form.GetField("DATE_OF_BIRTH").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);
            form.GetField("ISSUED_DATE").SetValue(userApplication.Application.License?.IssueDate ?? "", true);
            form.GetField("EXPIRED_DATE").SetValue(userApplication.Application.License?.ExpirationDate ?? "", true);

            string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "'" + userApplication.Application.PhysicalAppearance?.HeightInch;
            form.GetField("HEIGHT").SetValue(height ?? "", true);
            form.GetField("WEIGHT").SetValue(userApplication.Application.PhysicalAppearance?.Weight ?? "", true);
            form.GetField("EYE_COLOR").SetValue(userApplication.Application.PhysicalAppearance?.EyeColor ?? "", true);
            form.GetField("HAIR_COLOR").SetValue(userApplication.Application.PhysicalAppearance?.HairColor ?? "", true);

            var weapons = userApplication.Application.Weapons;
            if (null != weapons && weapons.Length > 0)
            {
                int totalWeapons = weapons.Length;
                int fieldIteration = 1;
                string makeField;
                string modelField;
                string serialField;
                string caliberField;

                for (int i = 0; i < totalWeapons && i < 4; i++)
                {
                    makeField = "MANUFACTURER" + fieldIteration.ToString();
                    modelField = "MODEL" + fieldIteration.ToString();
                    serialField = "SERIAL" + fieldIteration.ToString();
                    caliberField = "CALIBER" + fieldIteration.ToString();

                    form.GetField(makeField).SetValue(weapons[i].Make);
                    form.GetField(modelField).SetValue(weapons[i].Model);
                    form.GetField(serialField).SetValue(weapons[i].SerialNumber);
                    form.GetField(caliberField).SetValue(weapons[i].Caliber);

                    fieldIteration++;
                }
            }

            form.GetField("ISSUING_NAME").SetValue(adminResponse.AgencySheriffName ?? "", true);
            form.GetField("INFO_NUMBER").SetValue(adminResponse.AgencyTelephone ?? "", true);
            docFileAll.Flush();
            form.FlattenFields();
            docFileAll.Close();

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");
            FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

            var saveFileResult = await _documentHttpClient.SaveAdminApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            byte[] byteInfo = outStream.ToArray();
            outStream.Write(byteInfo, 0, byteInfo.Length);
            outStream.Position = 0;

            FileStreamResult fileStreamResultDownload = new FileStreamResult(outStream, "application/pdf");

            //Uncomment this to return the file as a download
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
            var adminResponse = await _adminHttpClient.GetAgencyProfileSettingsAsync(cancellationToken: default);
            var response = await _documentHttpClient.GetLiveScanTemplateAsync(cancellationToken: default);

            Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            MemoryStream outStream = new MemoryStream();

            PdfReader pdfReader = new PdfReader(streamToReadFrom);
            PdfWriter pdfWriter = new PdfWriter(outStream);
            PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

            Document docFileAll = new Document(doc);
            pdfWriter.SetCloseStream(false);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(doc, true);
            form.SetGenerateAppearance(true);

            await AddApplicantSignatureImageForLiveScan(userApplication, docFileAll);
            var submittedDate = DateTime.Now.ToString("MM/dd/yyyy");
            form.GetField("DATE").SetValue(submittedDate ?? "", true);
            form.GetField("ORI").SetValue(adminResponse.ORI ?? "", true);
            string licenseType = userApplication.Application.ApplicationType?.ToString();
            licenseType = licenseType.ToUpper() + " CCW";
            form.GetField("AUTHORIZED_APPLICANT_TYPE").SetValue(licenseType ?? "", true);
            form.GetField("LICENSE_TYPE").SetValue(licenseType ?? "", true);
            form.GetField("AGENCY_NAME").SetValue(adminResponse.AgencyName ?? "", true);
            form.GetField("AGENCY_MAIL_CODE").SetValue(adminResponse.MailCode ?? "", true);
            form.GetField("AGENCY_ADDRESS_1").SetValue(adminResponse.AgencyShippingStreetAddress ?? "", true);
            form.GetField("AGENCY_CONTACT_NAME").SetValue(adminResponse.ContactName ?? "", true);
            form.GetField("AGENCY_CITY").SetValue(adminResponse.AgencyShippingCity ?? "", true);
            form.GetField("AGENCY_STATE").SetValue(GetStateByName(adminResponse.AgencyShippingState) ?? "", true);
            form.GetField("AGENCY_ZIP").SetValue(adminResponse.AgencyShippingZip ?? "", true);
            form.GetField("AGENCY_CONTACT_NUMBER").SetValue(adminResponse.ContactNumber ?? "", true);
            string fullname = BuildApplicantFullName(userApplication);
            form.GetField("LAST_NAME").SetValue(userApplication.Application.PersonalInfo?.LastName ?? "", true);
            form.GetField("FIRST_NAME").SetValue(userApplication.Application.PersonalInfo?.FirstName ?? "", true);
            if (userApplication.Application.PersonalInfo?.MiddleName != "" && userApplication.Application.PersonalInfo?.MiddleName != null)
            {
                form.GetField("MIDDLE_INITIAL").SetValue(userApplication.Application.PersonalInfo?.MiddleName.Substring(0, 1) ?? "", true);
            }

            form.GetField("SUFFIX").SetValue(userApplication.Application.PersonalInfo?.Suffix ?? "", true);
            if (userApplication.Application.Aliases.Length > 0)
            {
                form.GetField("LAST_NAME_2").SetValue(userApplication.Application.Aliases[0].PrevLastName ?? "", true);
                form.GetField("FIRST_NAME_2").SetValue(userApplication.Application.Aliases[0].PrevFirstName ?? "", true);
                form.GetField("SUFFIX_2").SetValue(userApplication.Application.Aliases[0].PrevSuffix ?? "", true);
            }
            form.GetField("DATE_OF_BIRTH").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);
            if (userApplication.Application.PhysicalAppearance.Gender == "male")
            {
                form.GetField("MALE").SetValue("true");
            }
            else
            {
                form.GetField("FEMALE").SetValue("true");
            }
            form.GetField("DL_NUMBER").SetValue(userApplication.Application.IdInfo.IdNumber ?? "", true);
            string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "'" + userApplication.Application.PhysicalAppearance?.HeightInch;
            form.GetField("HEIGHT").SetValue(height ?? "", true);
            form.GetField("WEIGHT").SetValue(userApplication.Application.PhysicalAppearance.Weight ?? "", true);
            form.GetField("EYE_COLOR").SetValue(userApplication.Application.PhysicalAppearance.EyeColor ?? "", true);
            form.GetField("HAIR_COLOR").SetValue(userApplication.Application.PhysicalAppearance.HairColor ?? "", true);
            form.GetField("AGENCY_BILLING_NUMBER").SetValue(adminResponse.AgencyBillingNumber ?? "", true);
            form.GetField("BIRTH_STATE").SetValue(GetStateByName(userApplication.Application.DOB.BirthState) ?? "", true);
            form.GetField("SSN").SetValue(userApplication.Application.PersonalInfo.Ssn ?? "", true);
            string residenceAddress1 = userApplication.Application.CurrentAddress?.AddressLine1;
            string residenceAddress2 = userApplication.Application.CurrentAddress?.AddressLine2;
            if (residenceAddress2 != null)
            {
                residenceAddress1 = residenceAddress1 + ", " + residenceAddress2;
            }
            form.GetField("ADDRESS_1").SetValue(residenceAddress1 ?? "", true);
            form.GetField("CITY").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
            form.GetField("STATE").SetValue(GetStateByName(userApplication.Application.CurrentAddress?.State) ?? "", true);
            form.GetField("ZIP").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
            docFileAll.Flush();
            form.FlattenFields();
            docFileAll.Close();

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");
            FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

            var saveFileResult = await _documentHttpClient.SaveAdminApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            byte[] byteInfo = outStream.ToArray();
            outStream.Write(byteInfo, 0, byteInfo.Length);
            outStream.Position = 0;

            FileStreamResult fileStreamResultDownload = new FileStreamResult(outStream, "application/pdf");

            //Uncomment this to return the file as a download
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

    private async Task AddApplicantSignatureImageForLiveScan(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "signature");
        var documentResponse = await _documentHttpClient.GetApplicantImageAsync(signatureFileName, cancellationToken: default);
        var streamContent = await documentResponse.Content.ReadAsStreamAsync();

        var sr = new StreamReader(streamContent);
        string imageUri = sr.ReadToEnd();
        string imageBase64Data = imageUri.Remove(0, 22);
        byte[] imageBinaryData = Convert.FromBase64String(imageBase64Data);

        var imageData = ImageDataFactory.Create(imageBinaryData);
        var position = new ImagePosition()
        {
            Page = 1,
            Width = 250,
            Height = 30,
            Left = 150,
            Bottom = 280
        };

        var image = GetImageForImageData(imageData, position);
        docFileAll.Add(image);
    }

    private async Task AddApplicantSignatureImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "signature");
        var imageData = await GetImageDataForPdf(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 210,
            Height = 30,
            Left = 145,
            Bottom = -6
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }
    private async Task AddApplicantThumbprintImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "thumbprint");
        var imageData = await GetImageDataForPdf(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 2,
            Width = 40,
            Height = 50,
            Left = 181,
            Bottom = 6
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }
    private async Task AddApplicantPhotoImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "portrait");
        var imageData = await GetImageDataForPdf(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 80,
            Height = 70,
            Left = 6,
            Bottom = 50
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }
    private async Task AddSheriffLogoForUnOfficial(Document docFileAll)
    {
        var documentResponse = await _documentHttpClient.GetSheriffLogoAsync(cancellationToken: default);
        var streamContent = await documentResponse.Content.ReadAsStreamAsync();

        var memoryStream = new MemoryStream();
        await streamContent.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var imageData = ImageDataFactory.Create(memoryStream.ToArray());

        var leftPosition = new ImagePosition()
        {
            Page = 2,
            Width = 50,
            Height = 60,
            Left = 92,
            Bottom = 6
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }
    private async Task AddSheriffIssuingOfficierSignatureImageForUnOfficial(Document docFileAll)
    {
        var documentResponse = await _documentHttpClient.GetSheriffSignatureAsync(cancellationToken: default);
        var streamContent = await documentResponse.Content.ReadAsStreamAsync();

        var memoryStream = new MemoryStream();
        await streamContent.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var imageData = ImageDataFactory.Create(memoryStream.ToArray());

        var leftPosition = new ImagePosition()
        {
            Page = 2,
            Width = 180,
            Height = 17,
            Left = 2,
            Bottom = 15
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }

    private async Task<ImageData> GetImageDataForPdf(string fileName, Stream contentStream = null, bool shouldResize = false)
    {
        byte[] imageBinaryData;
        if (contentStream != null)
        {
            var ms = new MemoryStream();
            await contentStream.CopyToAsync(ms);
            imageBinaryData = ms.ToArray();
        }
        else
        {
            var documentResponse = await _documentHttpClient.GetApplicantImageAsync(fileName, cancellationToken: default);
            imageBinaryData = await documentResponse.Content.ReadAsByteArrayAsync();
        }

        if (shouldResize)
        {
            try
            {
                // Ignore these warnings. Technically System.Drawing.Common is NOT cross platform
                // However, runtimeconfig.template.json setting "System.Drawing.EnableUnixSupport": true
                // Allows it work on Linux (kind of)
                System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageBinaryData));
                Bitmap bmp = new Bitmap(new MemoryStream(imageBinaryData));
                var resized = ResizeImage(bmp);
                MemoryStream resizedImageStream = new MemoryStream();
                resized.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Bmp);

                imageBinaryData = resizedImageStream.GetBuffer();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error converting image");
                Console.WriteLine($"Error converting image: {exception.Message}");
            }
        }

        var imageData = ImageDataFactory.Create(imageBinaryData);

        return imageData;

        throw new FileNotFoundException("File not found: " + fileName);
    }

    private Bitmap ResizeImage(Bitmap bitmapToResize)
    {
        int w = bitmapToResize.Width;
        int h = bitmapToResize.Height;

        Func<int, bool> allWhiteRow = row =>
        {
            for (int i = 0; i < w; ++i)
                if (bitmapToResize.GetPixel(i, row).R != 255)
                    return false;
            return true;
        };

        Func<int, bool> allWhiteColumn = col =>
        {
            for (int i = 0; i < h; ++i)
                if (bitmapToResize.GetPixel(col, i).R != 255)
                    return false;
            return true;
        };

        int topmost = 0;

        for (int row = 0; row < h; ++row)
        {
            if (allWhiteRow(row))
                topmost = row;
            else break;
        }

        int bottommost = 0;
        for (int row = h - 1; row >= 0; --row)
        {
            if (allWhiteRow(row))
                bottommost = row;
            else break;
        }

        int leftmost = 0, rightmost = 0;
        for (int col = 0; col < w; ++col)
        {
            if (allWhiteColumn(col))
                leftmost = col;
            else
                break;
        }

        for (int col = w - 1; col >= 0; --col)
        {
            if (allWhiteColumn(col))
                rightmost = col;
            else
                break;
        }

        if (rightmost == 0) rightmost = w; // As reached left
        if (bottommost == 0) bottommost = h; // As reached top.

        int croppedWidth = rightmost - leftmost;
        int croppedHeight = bottommost - topmost;

        if (croppedWidth == 0) // No border on left or right
        {
            leftmost = 0;
            croppedWidth = w;
        }

        if (croppedHeight == 0) // No border on top or bottom
        {
            topmost = 0;
            croppedHeight = h;
        }

        try
        {
            var target = new Bitmap(croppedWidth, croppedHeight);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(bitmapToResize,
                  new System.Drawing.RectangleF(0, 0, croppedWidth, croppedHeight),
                  new System.Drawing.RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                  GraphicsUnit.Pixel);
            }
            return target;
        }
        catch (Exception ex)
        {
            throw new Exception(
              string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
              ex);
        }
    }

    private iText.Layout.Element.Image GetImageForImageData(ImageData imageData, ImagePosition imagePosition)
    {
        return new iText.Layout.Element.Image(imageData)
            .ScaleToFit(imagePosition.Width, imagePosition.Height)
            .SetFixedPosition(imagePosition.Page, imagePosition.Left, imagePosition.Bottom);
    }

    private string BuildApplicantDocumentName(PermitApplication? userApplication, string documentName)
    {
        string fullFilename = userApplication.UserId + "_" +
            userApplication.Application.PersonalInfo?.LastName + "_" +
            userApplication.Application.PersonalInfo?.FirstName + "_" + documentName;

        return fullFilename;
    }

    private sealed class ImagePosition
    {
        public int Page { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Bottom { get; set; }
    }

    private void GetAADUserName(out string userName)
    {
        userName = this.HttpContext.User.Claims
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

    private static string GetStateByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return string.Empty;
        }

        switch (name.ToUpper())
        {
            case "ALABAMA":
                return "AL";

            case "ALASKA":
                return "AK";

            case "AMERICAN SAMOA":
                return "AS";

            case "ARIZONA":
                return "AZ";

            case "ARKANSAS":
                return "AR";

            case "CALIFORNIA":
                return "CA";

            case "COLORADO":
                return "CO";

            case "CONNECTICUT":
                return "CT";

            case "DELAWARE":
                return "DE";

            case "DISTRICT OF COLUMBIA":
                return "DC";

            case "FEDERATED STATES OF MICRONESIA":
                return "FM";

            case "FLORIDA":
                return "FL";

            case "GEORGIA":
                return "GA";

            case "GUAM":
                return "GU";

            case "HAWAII":
                return "HI";

            case "IDAHO":
                return "ID";

            case "ILLINOIS":
                return "IL";

            case "INDIANA":
                return "IN";

            case "IOWA":
                return "IA";

            case "KANSAS":
                return "KS";

            case "KENTUCKY":
                return "KY";

            case "LOUISIANA":
                return "LA";

            case "MAINE":
                return "ME";

            case "MARSHALL ISLANDS":
                return "MH";

            case "MARYLAND":
                return "MD";

            case "MASSACHUSETTS":
                return "MA";

            case "MICHIGAN":
                return "MI";

            case "MINNESOTA":
                return "MN";

            case "MISSISSIPPI":
                return "MS";

            case "MISSOURI":
                return "MO";

            case "MONTANA":
                return "MT";

            case "NEBRASKA":
                return "NE";

            case "NEVADA":
                return "NV";

            case "NEW HAMPSHIRE":
                return "NH";

            case "NEW JERSEY":
                return "NJ";

            case "NEW MEXICO":
                return "NM";

            case "NEW YORK":
                return "NY";

            case "NORTH CAROLINA":
                return "NC";

            case "NORTH DAKOTA":
                return "ND";

            case "NORTHERN MARIANA ISLANDS":
                return "MP";

            case "OHIO":
                return "OH";

            case "OKLAHOMA":
                return "OK";

            case "OREGON":
                return "OR";

            case "PALAU":
                return "PW";

            case "PENNSYLVANIA":
                return "PA";

            case "PUERTO RICO":
                return "PR";

            case "RHODE ISLAND":
                return "RI";

            case "SOUTH CAROLINA":
                return "SC";

            case "SOUTH DAKOTA":
                return "SD";

            case "TENNESSEE":
                return "TN";

            case "TEXAS":
                return "TX";

            case "UTAH":
                return "UT";

            case "VERMONT":
                return "VT";

            case "VIRGIN ISLANDS":
                return "VI";

            case "VIRGINIA":
                return "VA";

            case "WASHINGTON":
                return "WA";

            case "WEST VIRGINIA":
                return "WV";

            case "WISCONSIN":
                return "WI";

            case "WYOMING":
                return "WY";

            default:
                return name;
        }
    }
}