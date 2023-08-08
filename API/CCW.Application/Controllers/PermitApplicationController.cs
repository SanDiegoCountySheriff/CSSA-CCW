using AutoMapper;
using CCW.Application.Clients;
using CCW.Application.Entities;
using CCW.Application.Models;
using CCW.Application.Services;
using CCW.Common.Models;
using iText.Forms;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Rectangle = iText.Kernel.Geom.Rectangle;

namespace CCW.Application.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PermitApplicationController : ControllerBase
{
    private readonly IDocumentServiceClient _documentHttpClient;
    private readonly IAdminServiceClient _adminHttpClient;
    private readonly IUserProfileServiceClient _userProfileServiceClient;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly ILogger<PermitApplicationController> _logger;
    private readonly IMapper _mapper;

    public PermitApplicationController(
        IDocumentServiceClient documentHttpClient,
        IAdminServiceClient adminHttpClient,
        IUserProfileServiceClient userProfileServiceClient,
        ICosmosDbService cosmosDbService,
        ILogger<PermitApplicationController> logger,
        IMapper mapper
        )
    {
        _documentHttpClient = documentHttpClient;
        _adminHttpClient = adminHttpClient;
        _userProfileServiceClient = userProfileServiceClient;
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
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
    [Route("printApplication")]
    [HttpPut]
    public async Task<IActionResult> PrintApplication(string applicationId, bool shouldAddDownloadFilename = true)
    {
        //string applicationId = "97fa060f-473f-48d8-8b20-18d4b890a265";
        //applicationId = "caeb8369-4fbf-4f66-9c97-a9be2d73c24c";

        //bool shouldAddDownloadFilename = true;

        try
        {
            GetAADUserName(out string userName);
            GetUserId(out string userId);

            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            string? applicationType = userApplication.Application.ApplicationType;
            if (string.IsNullOrEmpty(applicationType))
            {
                throw new ArgumentNullException("ApplicationType");
            }

            var response = await _documentHttpClient.GetApplicationTemplateAsync(cancellationToken: default);
            response.EnsureSuccessStatusCode();

            var adminUserProfile = await _userProfileServiceClient.GetAdminUserProfileAsync(cancellationToken: default);

            Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            MemoryStream outStream = new MemoryStream();

            PdfReader pdfReader = new PdfReader(streamToReadFrom);
            PdfWriter pdfWriter = new PdfWriter(outStream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);

            Document mainDocument = new Document(pdfDoc);
            pdfWriter.SetCloseStream(false);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            form.SetGenerateAppearance(true);

            var issueDate = string.Empty;
            var expDate = string.Empty;

            if (userApplication.Application.License != null && !string.IsNullOrEmpty(userApplication.Application.License.IssueDate))
            {
                issueDate = userApplication.Application.License.IssueDate;
                expDate = userApplication.Application.License.ExpirationDate;
            }
            else
            {
                issueDate = DateTime.Now.ToString("MM/dd/yyyy");

                switch (applicationType)
                {
                    case "reserve":
                    case "renew-reserve":
                        expDate = DateTime.Now.AddYears(4).ToString("MM/dd/yyyy");
                        break;
                    case "judge":
                    case "renew-judge":
                        expDate = DateTime.Now.AddYears(3).ToString("MM/dd/yyyy");
                        break;
                    default:
                        expDate = DateTime.Now.AddYears(2).ToString("MM/dd/yyyy");
                        break;
                }

                //save to db issue date and expiration date
                History[] history = new[]{
                    new History
                    {
                        ChangeMadeBy =  userName,
                        Change = "Record license issue date and expiration date.",
                        ChangeDateTimeUtc = DateTime.UtcNow,
                    }
                };

                userApplication.History = history;
                userApplication.Application.License.IssueDate = issueDate;
                userApplication.Application.License.ExpirationDate = expDate;

                await _cosmosDbService.UpdateUserApplicationAsync(userApplication, cancellationToken: default);
            }

            await AddProcessorsSignatureImageForApplication(userApplication, mainDocument);
            await AddApplicantSignatureImageForApplication(userApplication, mainDocument);

            string applicantFullName = BuildApplicantFullName(userApplication);
            string digitallySigned = $"DIGITALLY SIGNED BY: {applicantFullName}, ON {DateTime.Now.ToString("MM/dd/yyyy")}";
            form.GetField("form1[0].#subform[2].SIGNATURE[0]").SetValue(digitallySigned, true);
            form.GetField("form1[0].#subform[7].SIGNATURE[1]").SetValue(digitallySigned, true);
            form.GetField("form1[0].#subform[10].SIGNATURE[2]").SetValue(digitallySigned, true);

            form.GetField("form1[0].#subform[2].BADGE_NUMBER[0]").SetValue(adminUserProfile.BadgeNumber, true);
            form.GetField("form1[0].#subform[7].BADGE_NUMBER[1]").SetValue(adminUserProfile.BadgeNumber, true);
            form.GetField("form1[0].#subform[10].BADGE_NUMBER[2]").SetValue(adminUserProfile.BadgeNumber, true);

            form.GetField("form1[0].#subform[2].DATE[0]").SetValue(issueDate, true);
            form.GetField("form1[0].#subform[7].DATE[7]").SetValue(issueDate, true);
            form.GetField("form1[0].#subform[10].DATE[8]").SetValue(issueDate, true);

            form.GetField("form1[0].#subform[2].DateTimeField1[0]").SetValue(issueDate, true);
            form.GetField("form1[0].#subform[7].DateTimeField1[1]").SetValue(issueDate, true);
            form.GetField("form1[0].#subform[10].DateTimeField1[2]").SetValue(issueDate, true);

            switch (applicationType)
            {
                case "reserve":
                case "renew-reserve":
                    form.GetField("form1[0].#subform[2].RESERVE_OFFICER[0]").SetValue("true", true);
                    break;
                case "judge":
                case "renew-judge":
                    form.GetField("form1[0].#subform[2].JUDGE[0]").SetValue("true", true);
                    break;
                default:
                    form.GetField("form1[0].#subform[2].STANDARD[0]").SetValue("true", true);
                    break;
            }

            switch (applicationType)
            {
                case "renew-reserve":
                case "renew-judge":
                    form.GetField("form1[0].#subform[2].RENEWAL_APP[0]").SetValue("true", true);
                    break;
                default:
                    form.GetField("form1[0].#subform[2].INITIAL_APP[0]").SetValue("true", true);
                    break;
            }

            var paersonalInfo = userApplication.Application.PersonalInfo;
            if (paersonalInfo == null)
            {
                throw new ArgumentNullException("PersonalInfo");
            }

            //Applicant Personal Information
            form.GetField("form1[0].#subform[2].APP_LAST_NAME[0]").SetValue(paersonalInfo?.LastName ?? "", true);
            form.GetField("form1[0].#subform[2].APP_FIRST_NAME[0]").SetValue(paersonalInfo?.FirstName ?? "", true);
            form.GetField("form1[0].#subform[2].APP_MIDDLE_NAME[0]").SetValue(paersonalInfo?.MiddleName ?? "", true);

            string maidenAndAliases = string.Empty;
            if (!string.IsNullOrWhiteSpace(paersonalInfo?.MaidenName))
            {
                maidenAndAliases += paersonalInfo?.MaidenName;
            }

            if (userApplication.Application.Aliases?.Length > 0)
            {
                var aliases = " Aliases: ";
                foreach (var item in userApplication.Application.Aliases)
                {
                    aliases += item.PrevLastName + " " + item.PrevFirstName + "; ";
                }

                maidenAndAliases += aliases;
            }
            form.GetField("form1[0].#subform[2].APP_MAIDEN_NAME[0]").SetValue(maidenAndAliases, true);

            form.GetField("form1[0].#subform[2].APP_RESIDENT_CITY[0]").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
            form.GetField("form1[0].#subform[2].APP_RESIDENT_COUNTY[0]").SetValue(userApplication.Application.CurrentAddress?.County ?? "", true);
            form.GetField("form1[0].#subform[2].APP_CITIZENSHIP[0]").SetValue(userApplication.Application.ImmigrantInformation?.CountryOfCitizenship ?? "", true);

            form.GetField("form1[0].#subform[2].APP_DOB[0]").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);

            var birthPlace = string.Empty;
            if (!string.IsNullOrEmpty(userApplication.Application.DOB?.BirthCity))
            {
                birthPlace = userApplication.Application.DOB?.BirthCity + "  " +
                             userApplication.Application.DOB?.BirthState + "  " +
                             userApplication.Application.DOB?.BirthCountry;
            }
            form.GetField("form1[0].#subform[2].APP_BIRTH_PLACE[0]").SetValue(birthPlace, true);

            string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "ft" + " " +
                            userApplication.Application.PhysicalAppearance?.HeightInch + "in";
            form.GetField("form1[0].#subform[2].APP_HEIGHT[0]").SetValue(height, true);
            form.GetField("form1[0].#subform[2].APP_LBS[0]").SetValue(userApplication.Application.PhysicalAppearance?.Weight + "lbs" ?? "", true);
            form.GetField("form1[0].#subform[2].APP_EYE_CLR[0]").SetValue(userApplication.Application.PhysicalAppearance?.EyeColor ?? "", true);
            form.GetField("form1[0].#subform[2].APP_HAIR_CLR[0]").SetValue(userApplication.Application.PhysicalAppearance?.HairColor ?? "", true);
            string gender = userApplication.Application.PhysicalAppearance?.Gender.First().ToString().ToUpper() ?? "";
            form.GetField("form1[0].#subform[2].APP_GENDER[0]").SetValue(gender, true);

#if DEBUG
            foreach (var key in form.GetFormFields().Keys)
            {
                Console.WriteLine(key);
            }
#endif   
            var qualifyingQuestions = userApplication.Application.QualifyingQuestions;
            if (qualifyingQuestions == null)
            {
                throw new ArgumentNullException("QualifyingQuestions");
            }

            string questionYesNo = qualifyingQuestions.QuestionOne.Value ? "0" : "1";
            if (qualifyingQuestions.QuestionOne.Value)
            {
                form.GetField("form1[0].#subform[2].CURRENT_CCW[1]").SetValue("0", true);
            }
            else
            {
                form.GetField("form1[0].#subform[2].CURRENT_CCW[1]").SetValue("1", true);
            }

            if (qualifyingQuestions.QuestionOne.Value)
            {
                form.GetField("form1[0].#subform[2].ISSUING_AGENCY[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionOneAgency, true);
                form.GetField("form1[0].#subform[2].ISSUE_DATE[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionOneIssueDate, true);
                form.GetField("form1[0].#subform[2].CCW_NO[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionOneNumber, true);
            }

            questionYesNo = qualifyingQuestions.QuestionTwo.Value ? "0" : "1";
            form.GetField("form1[0].#subform[2].CCW_DENIAL[1]").SetValue(questionYesNo, true);
            if (questionYesNo == "0")
            {
                string[] questionTwoSubstrings = qualifyingQuestions?.QuestionTwoExp.Split(',');
                form.GetField("form1[0].#subform[2].AGENCY_NAME[0]").SetValue(questionTwoSubstrings[0], true);
                form.GetField("form1[0].#subform[2].DATE[1]").SetValue(questionTwoSubstrings[1], true);
                form.GetField("form1[0].#subform[2].DENIAL_REASON[0]").SetValue(questionTwoSubstrings[2], true);
            }

            questionYesNo = qualifyingQuestions.QuestionThree.Value ? "0" : "1";
            form.GetField("form1[0].#subform[2].US_CITIZENSHIP[1]").SetValue(questionYesNo, true);
            if (qualifyingQuestions.QuestionThree.Value)
            {
                form.GetField("form1[0].#subform[2].US_CITIZENSHIP[2]").SetValue(qualifyingQuestions?.QuestionThreeExp, true);
            }

            questionYesNo = qualifyingQuestions.QuestionFour.Value ? "0" : "1";
            form.GetField("form1[0].#subform[2].DISHONORABLE_DISCHARGE[0]").SetValue(questionYesNo, true);
            if (qualifyingQuestions.QuestionFour.Value)
            {
                form.GetField("form1[0].#subform[2].DISHONORBALE_DISCHARGE[0]").SetValue(qualifyingQuestions?.QuestionFourExp, true);
            }

            questionYesNo = qualifyingQuestions.QuestionFive.Value ? "0" : "1";
            form.GetField("form1[0].#subform[3].PARTY_TO_LAWSUIT[1]").SetValue(questionYesNo, true);
            if (qualifyingQuestions.QuestionFive.Value)
            {
                form.GetField("form1[0].#subform[3].PARTY_TO_LAWSUIT[2]").SetValue(qualifyingQuestions?.QuestionFiveExp, true);
            }

            questionYesNo = qualifyingQuestions.QuestionSix.Value ? "0" : "1";
            form.GetField("form1[0].#subform[3].RESTRAINING_ORDER[1]").SetValue(questionYesNo, true);
            if (qualifyingQuestions.QuestionSix.Value)
            {
                form.GetField("form1[0].#subform[3].RESTRAINING_ORDER[2]").SetValue(qualifyingQuestions?.QuestionSixExp, true);
            }

            questionYesNo = qualifyingQuestions.QuestionSeven.Value ? "0" : "1";
            if (qualifyingQuestions.QuestionSeven.Value)
            {
                form.GetField("form1[0].#subform[3].PROBATION[1]").SetValue("0", true);
                form.GetField("form1[0].#subform[3].PROBATION[2]").SetValue(qualifyingQuestions?.QuestionSevenExp, true);
            }
            else
            {
                form.GetField("form1[0].#subform[3].PROBATION[1]").SetValue("1", true);
            }


            // TODO: Current data does not have extended violation data, need to get from end user
            ////traffic violations don't have the data : userApplication.Application.QualifyingQuestions?.QuestionEightExp 
            var tempDate = DateTime.Now.ToShortDateString();
            var tempViolation = "Violation";
            var tempAgency = "Agency";
            var tempCitation = "Citation";

            form.GetField("form1[0].#subform[3].DATE[2]").SetValue(tempDate, true);
            form.GetField("form1[0].#subform[3].VIOLATION[0]").SetValue(tempViolation, true);
            form.GetField("form1[0].#subform[3].AGENCY[0]").SetValue(tempAgency, true);
            form.GetField("form1[0].#subform[3].CITATION_NO[0]").SetValue(tempCitation, true);

            form.GetField("form1[0].#subform[3].DATE[3]").SetValue(tempDate, true);
            form.GetField("form1[0].#subform[3].VIOLATION[1]").SetValue(tempViolation, true);
            form.GetField("form1[0].#subform[3].AGENCY[1]").SetValue(tempAgency, true);
            form.GetField("form1[0].#subform[3].CITATION_NO[1]").SetValue(tempCitation, true);

            form.GetField("form1[0].#subform[3].DATE[4]").SetValue(tempDate, true);
            form.GetField("form1[0].#subform[3].VIOLATION[2]").SetValue(tempViolation, true);
            form.GetField("form1[0].#subform[3].AGENCY[2]").SetValue(tempAgency, true);
            form.GetField("form1[0].#subform[3].CITATION_NO[2]").SetValue(tempCitation, true);

            form.GetField("form1[0].#subform[3].DATE[5]").SetValue(tempDate, true);
            form.GetField("form1[0].#subform[3].VIOLATION[3]").SetValue(tempViolation, true);
            form.GetField("form1[0].#subform[3].AGENCY[3]").SetValue(tempAgency, true);
            form.GetField("form1[0].#subform[3].CITATION_NO[3]").SetValue(tempCitation, true);

            form.GetField("form1[0].#subform[3].DATE[6]").SetValue(tempDate, true);
            form.GetField("form1[0].#subform[3].VIOLATION[4]").SetValue(tempViolation, true);
            form.GetField("form1[0].#subform[3].AGENCY[4]").SetValue(tempAgency, true);
            form.GetField("form1[0].#subform[3].CITATION_NO[4]").SetValue(tempCitation, true);

            // NOTE: LM: This is here as an example as to how to handle adding numerous data rows and adding new page to PDF
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 30; i++)
            {
                sb.AppendLine($"{tempDate}\t{tempViolation}\t{tempAgency}\t{tempCitation}\t{i}");
            }

            AddAppendixPage("Apendix A: Additional Moving Violations", sb.ToString(), form, pdfDoc, true);

            questionYesNo = qualifyingQuestions.QuestionNine.Value ? "0" : "1";
            form.GetField("form1[0].#subform[3].CONVICTION[1]").SetValue(questionYesNo, true);
            if (qualifyingQuestions.QuestionNine.Value)
            {
                form.GetField("form1[0].#subform[3].CONVICTION[2]").SetValue(qualifyingQuestions.QuestionNineExp, true);
            }

            questionYesNo = qualifyingQuestions.QuestionTen.Value ? "0" : "1";
            form.GetField("form1[0].#subform[3].WITHELD_INFO[0]").SetValue(questionYesNo, true);
            if (qualifyingQuestions.QuestionTen.Value)
            {
                form.GetField("form1[0].#subform[3].WITHHELD_INFO[1]").SetValue(qualifyingQuestions?.QuestionTenExp ?? "", true);
            }

            //Description of Weapons
            var weapons = userApplication.Application.Weapons;
            if (null != weapons && weapons.Length > 0)
            {
                int totalWeapons = (weapons.Length > 3) ? 3 : weapons.Length;

                for (int i = 0; i < totalWeapons; i++)
                {
                    form.GetField("form1[0].#subform[4].MAKE[" + i + "]").SetValue(weapons[i].Make, true);
                    form.GetField("form1[0].#subform[4].MODEL[" + i + "]").SetValue(weapons[i].Model, true);
                    form.GetField("form1[0].#subform[4].CALIBER[" + i + "]").SetValue(weapons[i].Caliber, true);
                    form.GetField("form1[0].#subform[4].SERIAL_NUMBER[" + i + "]").SetValue(weapons[i].SerialNumber, true);
                }

                // NOTE: LM: Add additional page(s) for extra weapons
                if (weapons.Length > 3)
                {
                    StringBuilder weaponsSb = new StringBuilder();
                    int currentSetCount = 0;
                    int currentWeaponCounter = 3;
                    bool isContinuation = false;

                    totalWeapons = weapons.Length;
                    while (currentWeaponCounter < totalWeapons)
                    {
                        var weapon = weapons[currentWeaponCounter++];
                        weaponsSb.AppendLine($"{weapon.Make}\t{weapon.Model}\t{weapon.Caliber}\t{weapon.SerialNumber}");
                        currentSetCount++;

                        if (currentSetCount >= 30)
                        {
                            currentSetCount = 0;
                            string headerText = "Appendix B: Additional Weapons" + (isContinuation ? " - Continued" : "");
                            AddAppendixPage(headerText, weaponsSb.ToString(), form, pdfDoc, true);
                            isContinuation = true;
                            weaponsSb.Clear();
                        }
                    }

                    if (weaponsSb.Length > 0)
                    {
                        string headerText = "Appendix B: Additional Weapons" + (isContinuation ? " - Continued" : "");
                        AddAppendixPage(headerText, weaponsSb.ToString(), form, pdfDoc, true);
                    }
                }
            }

            form.GetField("form1[0].#subform[8].APPL_LAST_NAME[0]").SetValue(paersonalInfo?.LastName ?? "", true);
            form.GetField("form1[0].#subform[8].APPL_FIRST_NAME[0]").SetValue(paersonalInfo?.FirstName ?? "", true);
            form.GetField("form1[0].#subform[8].APPL_MIDDLE_NAME[0]").SetValue(paersonalInfo?.MiddleName ?? "", true);
            form.GetField("form1[0].#subform[8].APP_DOB[1]").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);

            //Investigator's Interview Notes

            if (!string.IsNullOrEmpty(userApplication.Application.DOB?.BirthDate))
            {
                DateTime birthDateTime = DateTime.ParseExact(userApplication.Application.DOB.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var age = CalculateAge(birthDateTime);
                form.GetField("form1[0].#subform[8].APP_AGE[0]").SetValue(age.ToString(), true);
            }

            form.GetField("form1[0].#subform[8].APP_SSN[0]").SetValue(FormatSSN(userApplication.Application.PersonalInfo?.Ssn) ?? "", true);
            form.GetField("form1[0].#subform[8].APP_CDL[0]").SetValue(userApplication.Application.IdInfo?.IdNumber ?? "", true);
            form.GetField("form1[0].#subform[8].APP_CDL_RESTRICTIONS[0]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSixteenExp, true);

            string? residenceAddress = userApplication.Application.CurrentAddress?.AddressLine1 + " " +
                                       userApplication.Application.CurrentAddress?.AddressLine2;
            form.GetField("form1[0].#subform[8].APP_Address[0]").SetValue(residenceAddress ?? "", true);
            form.GetField("form1[0].#subform[8].APP_City[0]").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
            form.GetField("form1[0].#subform[8].APP_State[0]").SetValue(GetStateByName(userApplication.Application.CurrentAddress?.State) ?? "", true);
            form.GetField("form1[0].#subform[8].APP_ZipCode[0]").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
            form.GetField("form1[0].#subform[8].APP_DAY_PhoneNum[0]").SetValue(FormatPhoneNumber(userApplication.Application.Contact?.PrimaryPhoneNumber), true);

            string? mailingAddress = userApplication.Application.MailingAddress?.AddressLine1 + " " +
                                    userApplication.Application.MailingAddress?.AddressLine2;
            form.GetField("form1[0].#subform[8].APP_MAILINGAddress[0]").SetValue(mailingAddress ?? "", true);
            form.GetField("form1[0].#subform[8].APP_MAILING_City[0]").SetValue(userApplication.Application.MailingAddress?.City ?? "", true);
            form.GetField("form1[0].#subform[8].APP_MAILING_State[0]").SetValue(GetStateByName(userApplication.Application.MailingAddress?.State) ?? "", true);
            form.GetField("form1[0].#subform[8].APP_MAILING_Zip[0]").SetValue(userApplication.Application.MailingAddress?.Zip ?? "", true);
            form.GetField("form1[0].#subform[8].APP_EVE_PhoneNum[0]").SetValue(FormatPhoneNumber(userApplication.Application.Contact?.CellPhoneNumber), true);

            form.GetField("form1[0].#subform[8].SPOUSE_LAST_NAME[0]").SetValue(userApplication.Application.SpouseInformation?.LastName ?? "", true);
            form.GetField("form1[0].#subform[8].SPOUSE_FIRST_NAME[0]").SetValue(userApplication.Application.SpouseInformation?.FirstName ?? "", true);
            form.GetField("form1[0].#subform[8].SPOUSE_MIDDLE_NAME[0]").SetValue(userApplication.Application.SpouseInformation?.MiddleName ?? "", true);

            string? spouseAddress = userApplication.Application.SpouseAddressInformation?.AddressLine1 + " " +
                                    userApplication.Application.SpouseAddressInformation?.AddressLine2;
            form.GetField("form1[0].#subform[8].SPOUSE_Address[0]").SetValue(spouseAddress ?? "", true);
            form.GetField("form1[0].#subform[8].SPOUSE_City[0]").SetValue(userApplication.Application.SpouseAddressInformation?.City ?? "", true);
            form.GetField("form1[0].#subform[8].SPOUSE_State[0]").SetValue(GetStateByName(userApplication.Application.SpouseAddressInformation?.State) ?? "", true);
            form.GetField("form1[0].#subform[8].SPOUSE_ZipCode[0]").SetValue(userApplication.Application.SpouseAddressInformation?.Zip ?? "", true);
            form.GetField("form1[0].#subform[8].SPOUSE_PhoneNum[0]").SetValue(FormatPhoneNumber(userApplication.Application.SpouseInformation?.PhoneNumber) ?? "", true);

            form.GetField("form1[0].#subform[8].APP_OCC[0]").SetValue(userApplication.Application.WorkInformation?.Occupation ?? "", true);
            form.GetField("form1[0].#subform[8].EMPOYER_NAME[0]").SetValue(userApplication.Application.WorkInformation?.EmployerName ?? "", true);

            string? workAddress = userApplication.Application.WorkInformation?.EmployerAddressLine1 + " " +
                                  userApplication.Application.WorkInformation?.EmployerAddressLine2;
            form.GetField("form1[0].#subform[8].CURRENT_EMP_Address[0]").SetValue(workAddress ?? "", true);
            form.GetField("form1[0].#subform[8].CURRENT_EMP_City[0]").SetValue(userApplication.Application.WorkInformation?.EmployerCity ?? "", true);
            form.GetField("form1[0].#subform[8].CURRENT_EMPLOYER_State[0]").SetValue(GetStateByName(userApplication.Application.WorkInformation?.EmployerState) ?? "", true);
            form.GetField("form1[0].#subform[8].CURRENT_EMPLOYER_ZipCode[0]").SetValue(userApplication.Application.WorkInformation?.EmployerZip ?? "", true);
            form.GetField("form1[0].#subform[8].CURRENT_EMPLOYER_PhoneNum[0]").SetValue(FormatPhoneNumber(userApplication.Application.WorkInformation?.EmployerPhone), true);

            //Description of previous addresses
            var previousAddresses = userApplication.Application.PreviousAddresses;
            if (previousAddresses != null && previousAddresses?.Length > 0)
            {
                int totalAddresses = (previousAddresses.Length > 4) ? 4 : previousAddresses.Length;

                for (int i = 0; i < totalAddresses; i++)
                {
                    int index = i + 1;
                    string address = previousAddresses[i].AddressLine1 + " " + previousAddresses[i].AddressLine2;
                    form.GetField("form1[0].#subform[8].APP_Address[" + index + "]").SetValue(address, true);
                    form.GetField("form1[0].#subform[8].APP_City[" + index + "]").SetValue(previousAddresses[i].City, true);
                    form.GetField("form1[0].#subform[8].APP_State[" + index + "]").SetValue(GetStateByName(previousAddresses[i].State), true);
                    form.GetField("form1[0].#subform[8].APP_ZipCode[" + index + "]").SetValue(previousAddresses[i].Zip, true);
                }

                // NOTE: LM: Add additional page(s) for extra addresses
                if (previousAddresses.Length > 3)
                {
                    StringBuilder addressesSb = new StringBuilder();
                    int currentSetCount = 0;
                    int currentAddressCounter = 3;
                    bool isContinuation = false;

                    totalAddresses = previousAddresses.Length;
                    while (currentAddressCounter < totalAddresses)
                    {
                        var previousAddress = previousAddresses[currentAddressCounter++];

                        string address = previousAddress.AddressLine1 + " " + previousAddress.AddressLine2;
                        addressesSb.AppendLine($"{address}, {previousAddress.City}, {previousAddress.State} {previousAddress.Zip}");

                        currentSetCount++;
                        if (currentSetCount >= 30)
                        {
                            currentSetCount = 0;
                            string headerText = "Appendix C: Additional Previous Addresses" + (isContinuation ? " - Continued" : "");
                            AddAppendixPage(headerText, addressesSb.ToString(), form, pdfDoc, true);
                            isContinuation = true;
                            addressesSb.Clear();
                        }
                    }

                    if (addressesSb.Length > 0)
                    {
                        string headerText = "Appendix C: Additional Previous Addresses" + (isContinuation ? " - Continued" : "");
                        AddAppendixPage(headerText, addressesSb.ToString(), form, pdfDoc, true);
                    }
                }
            }

            if (userApplication.Application.QualifyingQuestions.QuestionEleven.Value)
            {
                form.GetField("form1[0].#subform[8].MENTAL_FACILITY[1]").SetValue("0", true);
                form.GetField("form1[0].#subform[8].MENTAL_FACILITY[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionElevenExp ?? "", true);
            }
            else
            {
                form.GetField("form1[0].#subform[8].MENTAL_FACILITY[1]").SetValue("1", true);
            }

            if (userApplication.Application.QualifyingQuestions.QuestionTwelve.Value)
            {
                form.GetField("form1[0].#subform[8].ADDICTION[1]").SetValue("0", true);
                form.GetField("form1[0].#subform[8].ADDICTION[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionTwelveExp ?? "", true);
            }
            else
            {
                form.GetField("form1[0].#subform[8].ADDICTION[1]").SetValue("1", true);
            }

            if (userApplication.Application.QualifyingQuestions.QuestionThirteen.Value)
            {
                form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[1]").SetValue("0", true);
                form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionThirteenExp ?? "", true);
            }
            else
            {
                form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[1]").SetValue("1", true);
            }

            if (userApplication.Application.QualifyingQuestions.QuestionFourteen.Value)
            {
                form.GetField("form1[0].#subform[9].DV[1]").SetValue("0", true);
                form.GetField("form1[0].#subform[9].DV[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFourteenExp ?? "", true);
            }
            else
            {
                form.GetField("form1[0].#subform[9].DV[1]").SetValue("1", true);
            }

            if (userApplication.Application.QualifyingQuestions.QuestionFifteen.Value)
            {
                form.GetField("form1[0].#subform[9].FORMAL_CHARGES[1]").SetValue("0", true);
                form.GetField("form1[0].#subform[9].FORMAL_CHARGES[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFifteenExp ?? "", true);
            }
            else
            {
                form.GetField("form1[0].#subform[9].FORMAL_CHARGES[1]").SetValue("1", true);
            }

            form.GetField("form1[0].#subform[9].GOOD_CAUSE_STATEMENT[0]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSeventeenExp ?? "", true);

            mainDocument.Flush();
            form.FlattenFields();
            mainDocument.Close();

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            var fileName = BuildApplicantDocumentName(userApplication, "Application");
            fileName = fileName + "_" + DateTime.Today.ToString();
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

            return NotFound("An error occur while trying to print permit application.");
        }
    }

    private void AddAppendixPage(string header, string content, PdfAcroForm form, PdfDocument pdfDoc, bool userBorder = false)
    {
        PdfPage page = pdfDoc.AddNewPage(PageSize.LETTER);

        int x = 25;
        int y = 20;
        int w = 560;
        int h = 750;
        float f = 10f;

        Text headerText = new Text(header + "\n").SetFontSize(14f);
        Text paragraphText = new Text(content);

        // Pick any font from existing fields
        var font = form.GetField("form1[0].#subform[3].VIOLATION[3]").GetFont();

        Paragraph paragraph = new Paragraph();
        paragraph.SetFont(font).SetFontSize(f).SetBorder(new SolidBorder(ColorConstants.BLUE, .2F));
        paragraph.Add(headerText).Add(paragraphText);

        Rectangle rectangle = new Rectangle(x, y, w, h);
        Canvas canvas = new Canvas(page, rectangle);
        if (userBorder)
        {
            canvas.SetBorder(new SolidBorder(ColorConstants.BLUE, .2F));
        }
        canvas.Add(paragraph);
        canvas.Close();
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printOfficialLicense")]
    [HttpPut]
    public async Task<IActionResult> PrintOfficialLicense(string applicationId, bool shouldAddDownloadFilename = true)
    {
        try
        {
            GetAADUserName(out string userName);

            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            string? applicationType = userApplication.Application.ApplicationType;
            if (string.IsNullOrEmpty(applicationType))
            {
                throw new ArgumentNullException("ApplicationType");
            }

            var adminResponse = await _adminHttpClient.GetAgencyProfileSettingsAsync(cancellationToken: default);
            var response = await _documentHttpClient.GetOfficialLicenseTemplateAsync(cancellationToken: default);

            Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            MemoryStream outStream = new MemoryStream();

            PdfReader pdfReader = new PdfReader(streamToReadFrom);
            PdfWriter pdfWriter = new PdfWriter(outStream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);

            Document mainDocument = new Document(pdfDoc);
            pdfWriter.SetCloseStream(false);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            form.SetGenerateAppearance(true);

            await AddSheriffSignatureImageForOfficial(userApplication, mainDocument);
            await AddApplicantSignatureImageForOfficial(userApplication, mainDocument);
            await AddApplicantThumbprintImageForOfficial(userApplication, mainDocument);
            await AddApplicantPhotoImageForOfficial(userApplication, mainDocument);

            form.GetField("AGENCY").SetValue(adminResponse.AgencyName ?? "", true);
            form.GetField("ORI").SetValue(adminResponse.ORI ?? "", true);
            form.GetField("CII_NUMBER").SetValue(userApplication.Application.OrderId, true);
            form.GetField("LOCAL_AGENCY_NUMBER").SetValue(adminResponse.LocalAgencyNumber ?? "", true);

            var issueDate = string.Empty;
            var expDate = string.Empty;

            if (userApplication.Application.License != null && !string.IsNullOrEmpty(userApplication.Application.License?.IssueDate))
            {
                issueDate = userApplication.Application.License.IssueDate;
                expDate = userApplication.Application.License.ExpirationDate;
            }
            else
            {
                issueDate = DateTime.Now.ToString("MM/dd/yyyy");

                switch (applicationType)
                {
                    case "reserve":
                    case "renew-reserve":
                        expDate = DateTime.Now.AddYears(4).ToString("MM/dd/yyyy");
                        break;
                    case "judge":
                    case "renew-judge":
                        expDate = DateTime.Now.AddYears(3).ToString("MM/dd/yyyy");
                        break;
                    default:
                        expDate = DateTime.Now.AddYears(2).ToString("MM/dd/yyyy");
                        break;
                }

                //save to db issue date and expiration date
                History[] history = new[]{
                    new History
                    {
                        ChangeMadeBy =  userName,
                        Change = "Record license issue date and expiration date.",
                        ChangeDateTimeUtc = DateTime.UtcNow,
                    }
                };

                userApplication.History = history;
                userApplication.Application.License.IssueDate = issueDate;
                userApplication.Application.License.ExpirationDate = expDate;

                await _cosmosDbService.UpdateUserApplicationAsync(userApplication, cancellationToken: default);
            }

            switch (applicationType)
            {
                case "reserve":
                case "renew-reserve":
                    form.GetField("RESERVE_LICENSE_TYPE_CHECKBOX").SetValue("true", true);
                    break;
                case "judge":
                case "renew-judge":
                    form.GetField("JUDICIAL_LICENSE_TYPE_CHECKBOX").SetValue("true", true);
                    break;
                default:
                    form.GetField("STANDARD_LICENSE_TYPE_CHECKBOX").SetValue("true", true);
                    break;
            }

            form.GetField("ISSUE_DATE").SetValue(issueDate, true);
            form.GetField("EXPIRATION_DATE").SetValue(expDate, true);

            if (applicationType.Contains("renew"))
            {
                form.GetField("SUBSEQUENT_CHECKBOX").SetValue(expDate, true);
            }
            else
            {
                form.GetField("NEW_PERMIT_CHECKBOX").SetValue(expDate, true);
            }

            //Section A
            string fullName = BuildApplicantFullName(userApplication);

            form.GetField("FULL_NAME").SetValue(fullName.Replace("  ", "").Trim(), true);

            string? residenceAddress = userApplication.Application.CurrentAddress?.AddressLine1 +
                                       userApplication.Application.CurrentAddress?.AddressLine2;
            form.GetField("RESIDENCE_ADDRESS").SetValue(residenceAddress ?? "", true);
            form.GetField("CITY").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
            form.GetField("ZIP").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
            form.GetField("COUNTY").SetValue(userApplication.Application.CurrentAddress?.County ?? "", true);

            string workAddress = userApplication.Application.WorkInformation?.EmployerAddressLine1 + " " +
                                 userApplication.Application.WorkInformation?.EmployerAddressLine2 + ", " +
                                 userApplication.Application.WorkInformation?.EmployerCity + ", " +
                                 GetStateByName(userApplication.Application.WorkInformation?.EmployerState) + " " +
                                 userApplication.Application.WorkInformation?.EmployerZip;

            if (workAddress.Replace(" ", "") != ",,")
            {
                form.GetField("BUSINESS_ADDRESS").SetValue(workAddress, true);
            }

            form.GetField("OCCUPATION").SetValue(userApplication.Application.WorkInformation?.Occupation ?? "", true);
            form.GetField("BIRTHDATE").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);
            form.GetField("HEIGHT_FEET").SetValue(userApplication.Application.PhysicalAppearance?.HeightFeet + "'" ?? "", true);
            form.GetField("HEIGHT_INCHES").SetValue(userApplication.Application.PhysicalAppearance?.HeightInch + "\"" ?? "", true);
            form.GetField("WEIGHT").SetValue(userApplication.Application.PhysicalAppearance?.Weight ?? "", true);
            form.GetField("EYE_COLOR").SetValue(GetEyeColor(userApplication.Application.PhysicalAppearance?.EyeColor), true);
            form.GetField("HAIR_COLOR").SetValue(GetHairColor(userApplication.Application.PhysicalAppearance?.HairColor), true);

            var weapons = userApplication.Application.Weapons;
            //Section B
            if (null != weapons && weapons.Length > 0)
            {
                int totalWeapons = (weapons.Length > 3) ? 3 : weapons.Length;

                StringBuilder makeSB = new StringBuilder();
                StringBuilder serialSB = new StringBuilder();
                StringBuilder caliberSB = new StringBuilder();
                StringBuilder modelSB = new StringBuilder();

                for (int i = 0; i < totalWeapons; i++)
                {
                    makeSB.AppendLine(weapons[i].Make);
                    serialSB.AppendLine(weapons[i].SerialNumber);
                    caliberSB.AppendLine(weapons[i].Caliber);
                    modelSB.AppendLine(weapons[i].Model);
                }

                form.GetField("WEAPON_MAKE").SetValue(makeSB.ToString(), true);
                form.GetField("WEAPON_SERIAL").SetValue(serialSB.ToString(), true);
                form.GetField("WEAPON_CALIBER").SetValue(caliberSB.ToString(), true);
                form.GetField("WEAPON_MODEL").SetValue(modelSB.ToString(), true);

                if (weapons.Length > 3)
                {
                    makeSB = new StringBuilder();
                    serialSB = new StringBuilder();
                    caliberSB = new StringBuilder();
                    modelSB = new StringBuilder();

                    totalWeapons = weapons.Length > 42 ? 42 : weapons.Length;

                    for (int x = 3; x < totalWeapons; x++)
                    {
                        makeSB.AppendLine(weapons[x].Make);
                        serialSB.AppendLine(weapons[x].SerialNumber);
                        caliberSB.AppendLine(weapons[x].Caliber);
                        modelSB.AppendLine(weapons[x].Model);
                    }

                    form.GetField("ADDITIONAL_WEAPON_MAKE").SetValue(makeSB.ToString(), true);
                    form.GetField("ADDITIONAL_WEAPON_SERIAL").SetValue(serialSB.ToString(), true);
                    form.GetField("ADDITIONAL_WEAPON_CALIBER").SetValue(caliberSB.ToString(), true);
                    form.GetField("ADDITIONAL_WEAPON_MODEL").SetValue(modelSB.ToString(), true);
                }
            }

            //don't have restrictions
            //form.GetField("RESTRICTIONS").SetValue("", true);
            //form.GetField("APPLICANT_THUMBPRINT").SetValue(thumbprint, true);

            // TODO: 
            //form.GetField("ADDITIONAL_WEAPON_MAKE").SetValue("Ofelia Test", true);

            mainDocument.Flush();
            form.FlattenFields();
            mainDocument.Close();

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            var fileName = BuildApplicantDocumentName(userApplication, "Official_License");
            fileName = fileName + "_" + DateTime.Today.ToString();
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

            return NotFound("An error occur while trying to print official license.");
        }
    }

    private static string BuildApplicantFullName(PermitApplication? userApplication)
    {
        return (userApplication.Application.PersonalInfo?.FirstName + " " +
                                   userApplication.Application.PersonalInfo?.MiddleName + " " +
                                   userApplication.Application.PersonalInfo?.LastName + " " +
                                   userApplication.Application.PersonalInfo?.Suffix).Trim();
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printUnofficialLicense")]
    [HttpPut]
    public async Task<IActionResult> PrintUnofficialLicense(string applicationId, bool shouldAddDownloadFilename = true)
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
            await AddSheriffLogoForUnOfficial(userApplication, docFileAll);
            await AddSheriffIssuingOfficierSignatureImageForUnOfficial(userApplication, docFileAll);

            form.GetField("AGENCY_NAME").SetValue(adminResponse.AgencyName ?? "", true);
            form.GetField("AGENCY_ORI").SetValue(adminResponse.ORI ?? "", true);
            form.GetField("LOCAL_AGENCY_NUMBER").SetValue(adminResponse.LocalAgencyNumber ?? "", true);
            string fullname = BuildApplicantFullName(userApplication);
            form.GetField("APPLICANT_NAME").SetValue(fullname.Trim(), true);

            string? residenceAddress1 = userApplication.Application.CurrentAddress?.AddressLine1;
            string? residenceAddress2 = userApplication.Application.CurrentAddress?.AddressLine2;
            if (residenceAddress2 != null)
            {
                residenceAddress1 = residenceAddress1 + ", " + residenceAddress2;
            }
            form.GetField("APPLICATION_ADDRESS_LINE_1").SetValue(residenceAddress1 ?? "", true);
            string? residenceAddress3 = userApplication.Application.CurrentAddress?.City
                                       + ", " + userApplication.Application.CurrentAddress?.State
                                       + " " + userApplication.Application.CurrentAddress?.Zip;
            form.GetField("APPLICATION_ADDRESS_LINE_2").SetValue(residenceAddress3 ?? "", true);
            string? licenseType = userApplication.Application.ApplicationType?.ToString();
            licenseType = char.ToUpper(licenseType[0]) + licenseType.Substring(1);
            form.GetField("LICENSE_TYPE").SetValue(licenseType ?? "", true);
            form.GetField("DATE_OF_BIRTH").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);
            form.GetField("ISSUED_DATE").SetValue(userApplication.Application.License?.IssueDate ?? "", true);
            form.GetField("EXPIRED_DATE").SetValue(userApplication.Application.License?.ExpirationDate ?? "", true);

            string? height = userApplication.Application.PhysicalAppearance?.HeightFeet + "'" + userApplication.Application.PhysicalAppearance?.HeightInch;
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

            var fileName = BuildApplicantDocumentName(userApplication, "Unofficial_License");
            fileName = fileName + "_" + DateTime.Today.ToString();
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
    public async Task<IActionResult> PrintLiveScanForm(string applicationId, bool shouldAddDownloadFilename = true)
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
            string? licenseType = userApplication.Application.ApplicationType?.ToString();
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
            string? height = userApplication.Application.PhysicalAppearance?.HeightFeet + "'" + userApplication.Application.PhysicalAppearance?.HeightInch;
            form.GetField("HEIGHT").SetValue(height ?? "", true);
            form.GetField("WEIGHT").SetValue(userApplication.Application.PhysicalAppearance.Weight ?? "", true);
            form.GetField("EYE_COLOR").SetValue(userApplication.Application.PhysicalAppearance.EyeColor ?? "", true);
            form.GetField("HAIR_COLOR").SetValue(userApplication.Application.PhysicalAppearance.HairColor ?? "", true);
            form.GetField("AGENCY_BILLING_NUMBER").SetValue(adminResponse.AgencyBillingNumber ?? "", true);
            form.GetField("BIRTH_STATE").SetValue(GetStateByName(userApplication.Application.DOB.BirthState) ?? "", true);
            form.GetField("SSN").SetValue(userApplication.Application.PersonalInfo.Ssn ?? "", true);
            string? residenceAddress1 = userApplication.Application.CurrentAddress?.AddressLine1;
            string? residenceAddress2 = userApplication.Application.CurrentAddress?.AddressLine2;
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

            var fileName = BuildApplicantDocumentName(userApplication, "Live_Scan");
            fileName = fileName + "_" + DateTime.Today.ToString();
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

    private async Task AddApplicantSignatureImageForApplication(PermitApplication? userApplication, Document mainDocument)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "signature");
        var imageData = await GetImageDataForPdf(signatureFileName, shouldResize: true);

        var pageThreePosition = new ImagePosition()
        {
            Page = 3,
            Width = 200,
            Height = 13,
            Left = 40,
            Bottom = 596
        };

        var pageThreeImage = GetImageForImageData(imageData, pageThreePosition);
        mainDocument.Add(pageThreeImage);

        var pageEightPosition = new ImagePosition()
        {
            Page = 8,
            Width = 200,
            Height = 15,
            Left = 40,
            Bottom = 368
        };

        var pageEightImage = GetImageForImageData(imageData, pageEightPosition);
        mainDocument.Add(pageEightImage);

        var pageElevenPosition = new ImagePosition()
        {
            Page = 11,
            Width = 200,
            Height = 15,
            Left = 40,
            Bottom = 538
        };

        var pageElevenImage = GetImageForImageData(imageData, pageElevenPosition);
        mainDocument.Add(pageElevenImage);
    }

    private async Task AddProcessorsSignatureImageForApplication(PermitApplication? userApplication, Document mainDocument)
    {
        GetUserId(out string userId);

        var documentResponse = await _documentHttpClient.GetProcessorSignatureAsync(cancellationToken: default);
        var streamContent = await documentResponse.Content.ReadAsStreamAsync();

        var sr = new StreamReader(streamContent);
        string imageUri = sr.ReadToEnd();
        string imageBase64Data = imageUri.Remove(0, 22);
        byte[] imageBinaryData = Convert.FromBase64String(imageBase64Data);

        var imageData = ImageDataFactory.Create(imageBinaryData);

        var pageThreePosition = new ImagePosition()
        {
            Page = 3,
            Width = 200,
            Height = 13,
            Left = 40,
            Bottom = 560
        };

        var pageThreeImage = GetImageForImageData(imageData, pageThreePosition);
        mainDocument.Add(pageThreeImage);

        var pageEightPosition = new ImagePosition()
        {
            Page = 8,
            Width = 200,
            Height = 15,
            Left = 40,
            Bottom = 330
        };

        var pageEightImage = GetImageForImageData(imageData, pageEightPosition);
        mainDocument.Add(pageEightImage);

        var pageElevenPosition = new ImagePosition()
        {
            Page = 11,
            Width = 200,
            Height = 15,
            Left = 40,
            Bottom = 500
        };

        var pageElevenImage = GetImageForImageData(imageData, pageElevenPosition);
        mainDocument.Add(pageElevenImage);
    }

    private async Task AddSheriffSignatureImageForOfficial(PermitApplication? userApplication, Document mainDocument)
    {
        var documentResponse = await _documentHttpClient.GetSheriffSignatureAsync(cancellationToken: default);
        var streamContent = await documentResponse.Content.ReadAsStreamAsync();

        var memoryStream = new MemoryStream();
        await streamContent.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var imageData = ImageDataFactory.Create(memoryStream.ToArray());

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 180,
            Height = 17,
            Left = 90,
            Bottom = 667
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 180,
            Height = 17,
            Left = 395,
            Bottom = 667
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantSignatureImageForOfficial(PermitApplication? userApplication, Document mainDocument)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "signature");
        var imageData = await GetImageDataForPdf(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 180,
            Height = 30,
            Left = 125,
            Bottom = 457
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 180,
            Height = 30,
            Left = 430,
            Bottom = 457
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantThumbprintImageForOfficial(PermitApplication? userApplication, Document mainDocument)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "thumbprint");
        var imageData = await GetImageDataForPdf(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 60,
            Height = 70,
            Left = 35,
            Bottom = 425
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 60,
            Height = 70,
            Left = 340,
            Bottom = 425
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantPhotoImageForOfficial(PermitApplication? userApplication, Document mainDocument)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "portrait");
        var imageData = await GetImageDataForPdf(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 70,
            Height = 95,
            Left = 127,
            Bottom = 374
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 70,
            Height = 95,
            Left = 432,
            Bottom = 374
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantSignatureImageForLiveScan(PermitApplication? userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "signature");
        var imageData = await GetImageDataForPdf(signatureFileName, shouldResize: true);

        var position = new ImagePosition()
        {
            Page = 1,
            Width = 200,
            Height = 13,
            Left = 65,
            Bottom = 290
        };

        var image = GetImageForImageData(imageData, position);
        docFileAll.Add(image);
    }

    private async Task AddApplicantSignatureImageForUnOfficial(PermitApplication? userApplication, Document docFileAll)
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
    private async Task AddSheriffLogoForUnOfficial(PermitApplication userApplication, Document docFileAll)
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
    private async Task AddSheriffIssuingOfficierSignatureImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
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

    private async Task<ImageData> GetImageDataForPdf(string fileName, Stream? contentStream = null, bool shouldResize = false)
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

    private static int CalculateAge(DateTime birthDate)
    {
        var birthday = new DateTime(birthDate.Year, birthDate.Month, birthDate.Day);
        int age = (int)((DateTime.Now - birthday).TotalDays / 365.242199);

        return age;
    }

    private static string FormatPhoneNumber(string? phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return string.Empty;
        }

        if (phone.Length == 10)
        {
            Regex regex = new Regex(@"[^\d]");
            phone = regex.Replace(phone, "");
            phone = Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            return phone;
        }

        return phone;
    }

    private static string FormatSSN(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value.Length == 9)
        {
            Regex regex = new Regex(@"[^\d]");
            value = regex.Replace(value, "");
            value = Regex.Replace(value, @"(\d{3})(\d{2})(\d{4})", "$1-$2-$3");
            return value;
        }

        return value;
    }

    private static string GetStateByName(string? name)
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

    private static string GetHairColor(string? color)
    {
        if (string.IsNullOrEmpty(color))
        {
            return string.Empty;
        }

        switch (color.ToUpper())
        {
            case "BLACK":
                return "BLK";

            case "BLONDE":
                return "BLD";

            case "BROWN":
                return "BRN";

            case "GRAY":
                return "GRY";

            case "LIGHT BROWN":
                return "LBRN";

            case "RED":
                return "RED";

            case "UNNATURAL":
                return "UNNAT";

            default:
                return color;
        }
    }

    private static string GetEyeColor(string? color)
    {
        if (string.IsNullOrEmpty(color))
        {
            return string.Empty;
        }

        switch (color.ToUpper())
        {
            case "BLACK":
                return "BLK";

            case "MIXED":
                return "MIX";

            case "BROWN":
                return "BRN";

            case "HAZEL":
                return "HAZ";

            case "BLUE":
                return "BLU";

            case "GREEN":
                return "GRN";

            case "GRAY":
                return "GRY";

            default:
                return color;
        }
    }
}