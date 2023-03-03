using CCW.Application.Clients;
using CCW.Application.Entities;
using CCW.Application.Mappers;
using CCW.Application.Models;
using CCW.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iText.Forms;
using iText.Kernel.Pdf;
using System.Globalization;
using System.Text.RegularExpressions;
using iText.Layout;
using CCW.Application.Enum;
using iText.IO.Image;
using iText.Layout.Element;
using iText.Layout.Borders;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Azure;

namespace CCW.Application.Controllers;


[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PermitApplicationController : ControllerBase
{
    private readonly IDocumentServiceClient _documentHttpClient;
    private readonly IAdminServiceClient _adminHttpClient;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel> _summaryPermitApplicationResponseMapper;
    private readonly IMapper<PermitApplication, PermitApplicationResponseModel> _permitApplicationResponseMapper;
    private readonly IMapper<PermitApplication, UserPermitApplicationResponseModel> _userPermitApplicationResponseMapper;
    private readonly IMapper<bool, PermitApplicationRequestModel, PermitApplication> _permitApplicationMapper;
    private readonly IMapper<bool, string, UserPermitApplicationRequestModel, PermitApplication> _userPermitApplicationMapper;
    private readonly IMapper<History, HistoryResponseModel> _historyMapper;
    private readonly ILogger<PermitApplicationController> _logger;

    public PermitApplicationController(
        IDocumentServiceClient documentHttpClient,
        IAdminServiceClient adminHttpClient,
        ICosmosDbService cosmosDbService,
        IMapper<SummarizedPermitApplication, SummarizedPermitApplicationResponseModel> summaryPermitApplicationResponseMapper,
        IMapper<PermitApplication, PermitApplicationResponseModel> permitApplicationResponseMapper,
        IMapper<PermitApplication, UserPermitApplicationResponseModel> userPermitApplicationResponseMapper,
        IMapper<bool, PermitApplicationRequestModel, PermitApplication> permitApplicationMapper,
        IMapper<bool, string, UserPermitApplicationRequestModel, PermitApplication> userPermitApplicationMapper,
        IMapper<History, HistoryResponseModel> historyMapper,
        ILogger<PermitApplicationController> logger
        )
    {
        _documentHttpClient = documentHttpClient;
        _adminHttpClient = adminHttpClient;
        _summaryPermitApplicationResponseMapper = summaryPermitApplicationResponseMapper;
        _permitApplicationResponseMapper = permitApplicationResponseMapper;
        _userPermitApplicationResponseMapper = userPermitApplicationResponseMapper;
        _permitApplicationMapper = permitApplicationMapper;
        _userPermitApplicationMapper = userPermitApplicationMapper;
        _historyMapper = historyMapper;
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _logger = logger;
    }


    [Authorize(Policy = "B2CUsers")]
    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] UserPermitApplicationRequestModel permitApplicationRequest)
    {
        GetUserId(out var userId);
        permitApplicationRequest.UserId = userId;

        try
        {
            var existingApplication =
                await _cosmosDbService.GetAllOpenApplicationsForUserAsync(userId, cancellationToken: default);

            if (existingApplication.Any())
            {
                return Conflict("In progress application/s found. Please delete open application/s or update.");
            }

            var result = await _cosmosDbService.AddAsync(_userPermitApplicationMapper.Map(true, null!, permitApplicationRequest), cancellationToken: default);

            return Ok(_permitApplicationResponseMapper.Map(result));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to create permit application.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [HttpGet("getApplication")]
    public async Task<IActionResult> GetApplication(string applicationId, bool isComplete = false)
    {
        GetUserId(out var userId);

        try
        {
            var result = await _cosmosDbService.GetLastApplicationAsync(userId, applicationId,
                isComplete, cancellationToken: default);

            return (result != null) ? Ok(_userPermitApplicationResponseMapper.Map(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve user permit application.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [HttpGet("getSSN")]
    public async Task<IActionResult> GetSSN()
    {
        GetUserId(out var userId);

        try
        {
            var result = await _cosmosDbService.GetSSNAsync(userId, cancellationToken: default);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve user permit application.");
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
            throw new Exception("An error occur while trying to retrieve user permit application.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserApplication")]
    public async Task<IActionResult> GetUserApplication(string userEmailOrOrderId, bool isOrderId = false, bool isComplete = false)
    {
        try
        {
            var result = await _cosmosDbService.GetUserLastApplicationAsync(userEmailOrOrderId, isOrderId, isComplete, cancellationToken: default);

            return (result != null) ? Ok(_permitApplicationResponseMapper.Map(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve specific user permit application.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [HttpGet("getApplications")]
    public async Task<IActionResult> GetApplications(string userEmail)
    {
        GetUserId(out var userId);

        try
        {
            IEnumerable<UserPermitApplicationResponseModel> responseModels = new List<UserPermitApplicationResponseModel>();
            var result = await _cosmosDbService.GetAllApplicationsAsync(userId, userEmail, cancellationToken: default);

            if (result.Any())
            {
                responseModels = result.Select(x => _userPermitApplicationResponseMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve user permit applications.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserApplications")]
    public async Task<IActionResult> GetUserApplications(string userEmail)
    {
        try
        {
            IEnumerable<PermitApplicationResponseModel> responseModels = new List<PermitApplicationResponseModel>();
            var result = await _cosmosDbService.GetAllUserApplicationsAsync(userEmail, cancellationToken: default);

            if (result.Any())
            {
                responseModels = result.Select(x => _permitApplicationResponseMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve all user permit applications.");
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
                responseModels = result.Select(x => _historyMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve permit application history.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            IEnumerable<SummarizedPermitApplicationResponseModel> responseModels = new List<SummarizedPermitApplicationResponseModel>();

            var result = await _cosmosDbService.GetAllInProgressApplicationsSummarizedAsync(cancellationToken: default);

            if (result.Any())
            {
                responseModels = result.Select(x => _summaryPermitApplicationResponseMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve all permit applications.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [HttpGet("search")]
    public async Task<IActionResult> Search(string searchValue)
    {
        try
        {
            IEnumerable<SummarizedPermitApplicationResponseModel> responseModels = new List<SummarizedPermitApplicationResponseModel>();
            var result = await _cosmosDbService.SearchApplicationsAsync(searchValue, cancellationToken: default);

            if (result.Any())
            {
                responseModels = result.Select(x => _summaryPermitApplicationResponseMapper.Map(x));
            }

            return Ok(responseModels);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to search all permit applications.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [Route("updateApplication")]
    [HttpPut]
    public async Task<IActionResult> UpdateApplication([FromBody] UserPermitApplicationRequestModel application)
    {
        GetUserId(out var userId);

        try
        {
            application.UserId = userId;

            var existingApplication = await _cosmosDbService.GetLastApplicationAsync(userId,
                application.Id.ToString(), isComplete: false,
                cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found or application already submitted.");
            }

            if (application.Application.PersonalInfo.Ssn.ToLower().Contains("xxx"))
            {
                application.Application.PersonalInfo.Ssn = existingApplication.Application.PersonalInfo.Ssn;
            }

            if (application.Application.Status == ApplicationStatus.Submitted &&
                existingApplication.Application.Status != ApplicationStatus.Approved &&
                existingApplication.Application.Status != ApplicationStatus.Submitted)
            {
                application.Application.SubmittedToLicensingDateTime = DateTime.UtcNow;
            }

            bool isNewApplication = false;
            await _cosmosDbService.UpdateApplicationAsync(_userPermitApplicationMapper.Map(isNewApplication,
                    existingApplication.Application.Comments, application),
                cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            if (e.Message.Contains("Permit application"))
            {
                throw new Exception(e.Message);
            }
            throw new Exception("An error occur while trying to update permit application.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("updateUserApplication")]
    [HttpPut]
    public async Task<IActionResult> UpdateUserApplication([FromBody] PermitApplicationRequestModel application, string updatedSection)
    {
        try
        {
            GetAADUserName(out var userName);

            var existingApplication = await _cosmosDbService.GetUserApplicationAsync(application.Id.ToString(), cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            if (application.Application.PersonalInfo != null && application.Application.PersonalInfo.Ssn.ToLower().Contains("xxx"))
            {
                application.Application.PersonalInfo.Ssn = existingApplication.Application.PersonalInfo.Ssn;
            }

            var app = SetBackgroundCheckHistory(application, existingApplication, userName);

            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = updatedSection,
                    ChangeDateTimeUtc = DateTime.UtcNow,
                }
            };

            app.History = history;
            app.PaymentHistory = application.PaymentHistory;
            bool isNewApplication = false;

            await _cosmosDbService.UpdateUserApplicationAsync(_permitApplicationMapper.Map(isNewApplication, app), cancellationToken: default);
            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to update permit application.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("removeUserApplicationAppointment")]
    [HttpPut]
    public async Task<IActionResult> RemoveUserApplicationAppointment(string applicationId)
    {
        try
        {
            GetAADUserName(out var userName);

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
            existingApplication.Application.AppointmentStatus = null;

            await _cosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to remove permit application appointment.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("updateUserAppointment")]
    [HttpPut]
    public async Task<IActionResult> UpdateUserAppointment(string applicationId, string appointmentDate)
    {
        try
        {
            GetAADUserName(out var userName);

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
            existingApplication.Application.AppointmentStatus = true;

            await _cosmosDbService.UpdateUserApplicationAsync(existingApplication, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to update permit application appointment.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [Route("deleteApplication")]
    [HttpPut]
    public async Task<IActionResult> DeleteApplication(string applicationId)
    {
        GetUserId(out var userId);

        try
        {
            var existingApp = await _cosmosDbService.GetLastApplicationAsync(userId, applicationId, isComplete: false, cancellationToken: default);

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

            throw new Exception("An error occur while trying to delete permit application.");
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

            throw new Exception("An error occur while trying to delete permit application.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("printApplication")]
    [HttpPut]
    public async Task<IActionResult> PrintApplication() //string applicationId, bool shouldAddDownloadFilename = true)
    {
        string applicationId = "97fa060f-473f-48d8-8b20-18d4b890a265";
        applicationId = "caeb8369-4fbf-4f66-9c97-a9be2d73c24c";

        bool shouldAddDownloadFilename = true;

        try
        {
            GetAADUserName(out var userName);

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
            // TODO: LM: Add check for OK

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
                    form.GetField("form1[0].#subform[2].RENEWAL_APP[0]").SetValue(applicationType, true);
                    break;
                default:
                    form.GetField("form1[0].#subform[2].INITIAL_APP[0]").SetValue(applicationType, true);
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

            if (!string.IsNullOrWhiteSpace(paersonalInfo?.MaidenName))
            {
                form.GetField("form1[0].#subform[2].APP_MAIDEN_NAME[0]").SetValue(paersonalInfo?.MaidenName, true);
            }
            else if (userApplication.Application.Aliases?.Length > 0)
            {
                var aliases = string.Empty;
                foreach (var item in userApplication.Application.Aliases)
                {
                    aliases += item.PrevLastName + " " + item.PrevFirstName + "; ";
                }

                form.GetField("form1[0].#subform[2].APP_MAIDEN_NAME[0]").SetValue(aliases, true);
            }
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
            form.GetField("form1[0].#subform[2].APP_GENDER[0]").SetValue(
                userApplication.Application.PhysicalAppearance?.Gender.First().ToString().ToUpper() ?? "", true);

            //Applicant Clearance Questions
            form.GetField("form1[0].#subform[2].ISSUING_AGENCY[0]").SetValue(userApplication.Application.License?.IssuingCounty ?? "", true);
            //? issue date or exp date
            form.GetField("form1[0].#subform[2].ISSUE_DATE[0]").SetValue(issueDate, true);//? which permit date
            form.GetField("form1[0].#subform[2].CCW_NO[0]").SetValue(userApplication.Application.License?.PermitNumber ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionTwo.Value ?
                    "form1[0].#subform[2].CCW_DENIAL[1].1" : "form1[0].#subform[2].CCW_DENIAL[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionTwo.Value.ToString(), true);

            //don't have the data
            //form.GetField("form1[0].#subform[2].AGENCY_NAME[0]").SetValue("", true);
            //form.GetField("form1[0].#subform[2].DATE[1]").SetValue("", true);

            form.GetField("form1[0].#subform[2].DENIAL_REASON[0]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionTwoExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionThree.Value ?
                    "form1[0].#subform[2].US_CITIZENSHIP[1]" : "form1[0].#subform[2].US_CITIZENSHIP[1]")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionThree.Value.ToString(), true);
            form.GetField("form1[0].#subform[2].US_CITIZENSHIP[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionThreeExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFour.Value ?
                    "form1[0].#subform[2].DISHONORABLE_DISCHARGE[0].1" : "form1[0].#subform[2].DISHONORABLE_DISCHARGE[0].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFour.Value.ToString(), true);
            form.GetField("form1[0].#subform[2].DISHONORBALE_DISCHARGE[0]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFourExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFive.Value ?
                    "form1[0].#subform[3].PARTY_TO_LAWSUIT[1].1" : "form1[0].#subform[3].PARTY_TO_LAWSUIT[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFive.Value.ToString(), true);
            form.GetField("form1[0].#subform[3].PARTY_TO_LAWSUIT[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFiveExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSeven.Value ?
                    "form1[0].#subform[3].RESTRAINING_ORDER[1].1" : "form1[0].#subform[3].RESTRAINING_ORDER[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSeven.Value.ToString(), true);
            form.GetField("form1[0].#subform[3].RESTRAINING_ORDER[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSevenExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSeven.Value ?
                    "form1[0].#subform[3].PROBATION[1].1" : "form1[0].#subform[3].PROBATION[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSeven.Value.ToString(), true);
            form.GetField("form1[0].#subform[3].PROBATION[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSevenExp ?? "", true);

            ////traffic violations don't have the data : userApplication.Application.QualifyingQuestions?.QuestionEightExp 
            //form.GetField("form1[0].#subform[3].DATE[2]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].VIOLATION[0]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].AGENCY[0]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].CITATION_NO[0]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].DATE[3]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].VIOLATION[1]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].AGENCY[1]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].CITATION_NO[1]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].DATE[4]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].VIOLATION[2]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].AGENCY[2]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].CITATION_NO[2]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].DATE[5]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].VIOLATION[3]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].AGENCY[3]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].CITATION_NO[3]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].DATE[6]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].VIOLATION[4]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].AGENCY[4]").SetValue("", true);
            //form.GetField("form1[0].#subform[3].CITATION_NO[4]").SetValue("", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionNine.Value ?
                    "form1[0].#subform[3].CONVICTION[1].1" : "form1[0].#subform[3].CONVICTION[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionNine.Value.ToString(), true);
            form.GetField("form1[0].#subform[3].CONVICTION[2]").SetValue("", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionOne.Value ?
                    "form1[0].#subform[3].WITHELD_INFO[0].1" : "form1[0].#subform[3].WITHELD_INFO[0].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionOne.Value.ToString(), true);
            form.GetField("form1[0].#subform[3].WITHHELD_INFO[1]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionOneExp ?? "", true);

            //Description of Weapons
            if (userApplication.Application.Weapons != null && userApplication.Application.Weapons.Length > 0)
            {
                int totalWeapons = (userApplication.Application.Weapons.Length > 3)
                    ? 3
                    : userApplication.Application.Weapons.Length;

                for (int i = 0; i < totalWeapons; i++)
                {
                    form.GetField("form1[0].#subform[4].MAKE[" + i + "]")
                            .SetValue(userApplication.Application.Weapons[i].Make, true);
                    form.GetField("form1[0].#subform[4].MODEL[" + i + "]")
                        .SetValue(userApplication.Application.Weapons[i].Model, true);
                    form.GetField("form1[0].#subform[4].CALIBER[" + i + "]")
                        .SetValue(userApplication.Application.Weapons[i].Caliber, true);
                    form.GetField("form1[0].#subform[4].SERIAL_NUMBER[" + i + "]")
                        .SetValue(userApplication.Application.Weapons[i].SerialNumber, true);
                }
            }

            //Investigator's Interview Notes
            form.GetField("form1[0].#subform[8].APPL_LAST_NAME[0]").SetValue(userApplication.Application.PersonalInfo?.Ssn ?? "", true);
            form.GetField("form1[0].#subform[8].APPL_FIRST_NAME[0]").SetValue(userApplication.Application.PersonalInfo?.Ssn ?? "", true);
            form.GetField("form1[0].#subform[8].APPL_MIDDLE_NAME[0]").SetValue(userApplication.Application.PersonalInfo?.Ssn ?? "", true);
            form.GetField("form1[0].#subform[8].APP_DOB[1]").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);

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

            if (userApplication.Application.PreviousAddresses != null && userApplication.Application.PreviousAddresses?.Length > 0)
            {
                int totalAddress = (userApplication.Application.PreviousAddresses.Length > 4)
                    ? 4
                    : userApplication.Application.PreviousAddresses.Length;

                for (int i = 0; i < totalAddress; i++)
                {
                    string address = string.Empty;
                    address = userApplication.Application.PreviousAddresses[i].AddressLine1 + " " +
                              userApplication.Application.PreviousAddresses[i].AddressLine2;
                    form.GetField("form1[0].#subform[8].APP_Address[" + (i + 1) + "]").SetValue(address, true);
                    form.GetField("form1[0].#subform[8].APP_City[" + (i + 1) + "]")
                        .SetValue(userApplication.Application.PreviousAddresses[i].City ?? "", true);
                    form.GetField("form1[0].#subform[8].APP_State[" + (i + 1) + "]")
                        .SetValue(GetStateByName(userApplication.Application.PreviousAddresses[i].State) ?? "", true);
                    form.GetField("form1[0].#subform[8].APP_ZipCode[" + (i + 1) + "]")
                        .SetValue(userApplication.Application.PreviousAddresses[i].Zip, true);
                }
            }

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionThirteen.Value ?
                    "form1[0].#subform[8].MENTAL_FACILITY[1].1" : "form1[0].#subform[8].MENTAL_FACILITY[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionThirteen.Value.ToString(), true);
            form.GetField("form1[0].#subform[8].MENTAL_FACILITY[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionThirteenExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionTwelve.Value ?
                    "form1[0].#subform[8].ADDICTION[1].1" : "form1[0].#subform[8].ADDICTION[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionTwelve.Value.ToString(), true);
            form.GetField("form1[0].#subform[8].ADDICTION[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionTwelveExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFourteen.Value ?
                    "form1[0].#subform[9].FIREARMS_INCIDENT[1].1" : "form1[0].#subform[9].FIREARMS_INCIDENT[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFourteen.Value.ToString(), true);
            form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFourteenExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSix.Value ?
                    "form1[0].#subform[9].DV[1].1" : "form1[0].#subform[9].DV[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSix.Value.ToString(), true);
            form.GetField("form1[0].#subform[9].DV[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSixExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFifteen.Value ?
                    "form1[0].#subform[9].FORMAL_CHARGES[1].1" : "form1[0].#subform[9].FORMAL_CHARGES[1].2")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFifteen.Value.ToString(), true);
            form.GetField("form1[0].#subform[9].FORMAL_CHARGES[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFifteenExp ?? "", true);

            form.GetField("form1[0].#subform[9].GOOD_CAUSE_STATEMENT[0]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSeventeenExp ?? "", true);

            mainDocument.Close();

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            var fileName = BuildApplicantDocumentName(userApplication, "Application");

            FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

            var saveFileResult = await _documentHttpClient.SaveApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

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

            throw new Exception("An error occur while trying to print permit application.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printOfficialLicense")]
    [HttpPut]
    public async Task<IActionResult> PrintOfficialLicense(string applicationId, bool shouldAddDownloadFilename = true)
    {
        try
        {
            GetAADUserName(out var userName);

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

            await AddSheriffSignatureImage(userApplication, mainDocument);
            await AddApplicantSignatureImage(userApplication, mainDocument);
            await AddApplicantThumbprintImage(userApplication, mainDocument);
            await AddApplicantPhotoImage(userApplication, mainDocument);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            form.SetGenerateAppearance(true);

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
            var fullName = userApplication.Application.PersonalInfo?.FirstName + " " +
                           userApplication.Application.PersonalInfo?.MiddleName + " " +
                           userApplication.Application.PersonalInfo?.LastName + " " +
                           userApplication.Application.PersonalInfo?.Suffix;

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


            mainDocument.Close();

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            var fileName = BuildApplicantDocumentName(userApplication, "Officical_License");

            FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

            var saveFileResult = await _documentHttpClient.SaveApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

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

            throw new Exception("An error occur while trying to print official license.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("printUnofficialLicense")]
    [HttpPut]
    public async Task<IActionResult> PrintUnofficialLicense(string applicationId, bool shouldAddDownloadFilename = false)
    {
        try
        {
            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

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

            ////TODO: Get value from admin settings but waiting to know who will provide the value in cosmos
            //form.GetField("REPLACE_PASS:~data.officeuse.CIINUMBERstatus").SetValue("", true);

            ////TODO: Where are restrictions?
            ////form.GetField("REPLACE_PASS:~data.officeuse.RESTRICTIONSstatus").SetValue("", true);

            //var issueDate = DateTime.Now.ToString("MM/dd/yyyy");
            //var expDate = DateTime.Now.AddYears(2).ToString("MM/dd/yyyy");
            //form.GetField("ISSUE_DATE").SetValue(issueDate, true);
            //form.GetField("EXPIRATION_DATE").SetValue(expDate, true);

            ////Name
            //var fullName = userApplication.Application.PersonalInfo?.LastName + " " +
            //               userApplication.Application.PersonalInfo?.FirstName + " " +
            //               userApplication.Application.PersonalInfo?.MiddleName;
            //form.GetField("FULL_NAME").SetValue(fullName, true);

            ////Personal Info
            //form.GetField("order.data.application.weight").SetValue(userApplication.Application.PhysicalAppearance?.Weight + "lbs" ?? "", true);
            //form.GetField("order.data.application.eyeColor").SetValue(userApplication.Application.PhysicalAppearance?.EyeColor ?? "", true);
            //form.GetField("order.data.application.hairColor").SetValue(userApplication.Application.PhysicalAppearance?.HairColor ?? "", true);

            //var height = (userApplication.Application.PhysicalAppearance?.HeightFeet + "ft " +
            //              userApplication.Application.PhysicalAppearance?.HeightInch + "in");
            //form.GetField("${(data_application_heightFeet!\"\")}' ${(data_application_heightInches!\"\")}").SetValue(height, true);

            ////Current Address
            //string? residenceAddress = string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.AddressLine1) ? "" :
            //                            userApplication.Application.CurrentAddress?.AddressLine1 + " " +
            //                            userApplication.Application.CurrentAddress?.AddressLine2 + ",\n" +
            //                            userApplication.Application.CurrentAddress?.City + ", " +
            //                            GetStateByName(userApplication.Application.CurrentAddress?.State) + " " +
            //                            userApplication.Application.CurrentAddress?.Zip;
            //form.GetField("${(data_application_currentaddressline1!\"\")} ${(data_application_currentaddressline2!\"\")}").SetValue(residenceAddress, true);

            ////Work
            //string workAddress = string.IsNullOrEmpty(userApplication.Application.WorkInformation?.EmployerAddressLine1) ? "" :
            //                    userApplication.Application.WorkInformation?.EmployerAddressLine1 + " " +
            //                    userApplication.Application.WorkInformation?.EmployerAddressLine2 + ", " +
            //                    userApplication.Application.WorkInformation?.EmployerCity + ", " +
            //                    GetStateByName(userApplication.Application.WorkInformation?.EmployerState) + " " +
            //                    userApplication.Application.WorkInformation?.EmployerZip;
            //form.GetField("${(data_application_workaddressline1!\"\")} ${(data_application_workaddressline2!\"\")}").SetValue(workAddress, true);
            //form.GetField("order.data.application.workoccupationfield").SetValue(userApplication.Application.WorkInformation?.Occupation ?? "", true);

            ////DOB
            //form.GetField("DATE~order.data.application.bpbobvalue").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);

            ////Weapons
            //var weapons = string.Empty;

            ////only the first 3 weapons
            //foreach (var item in userApplication.Application.Weapons)
            //{
            //    form.GetField("order.data.weapons[make]").SetValue(item.Make ?? "", true);
            //    form.GetField("order.data.weapons[serial]").SetValue(item.SerialNumber ?? "", true);
            //    form.GetField("order.data.weapons[caliber]").SetValue(item.Caliber ?? "", true);
            //    form.GetField("order.data.weapons[model]").SetValue(item.Model ?? "", true);
            //}

            docFileAll.Close();

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            var fileName = BuildApplicantDocumentName(userApplication, "Unofficial_license");

            FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

            var saveFileResult = await _documentHttpClient.SaveApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

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

            throw new Exception("An error occur while trying to print unofficial license.");
        }
    }

    private async Task AddSheriffSignatureImage(PermitApplication? userApplication, Document mainDocument)
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

    private async Task AddApplicantSignatureImage(PermitApplication? userApplication, Document mainDocument)
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

    private async Task AddApplicantThumbprintImage(PermitApplication? userApplication, Document mainDocument)
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

    private async Task AddApplicantPhotoImage(PermitApplication? userApplication, Document mainDocument)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "portrait");
        var imageData = await GetImageDataForPdf(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 70,
            Height = 95,
            Left = 145,
            Bottom = 385
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 70,
            Height = 95,
            Left = 450,
            Bottom = 385
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }


    private async Task<ImageData> GetImageDataForPdf(string fileName, Stream? contentStream = null)
    {
        var streamContent = contentStream;
        if (null == contentStream)
        {
            var documentResponse = await _documentHttpClient.GetApplicantImageAsync(fileName, cancellationToken: default);
            streamContent = await documentResponse.Content.ReadAsStreamAsync();
        }

        using (var memoryStream = new MemoryStream())
        {
            await streamContent.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var sr = new StreamReader(memoryStream);
            string imageUri = sr.ReadToEnd();
            string imageBase64Data = imageUri.Remove(0, 22);
            byte[] imageBinaryData = Convert.FromBase64String(imageBase64Data);

            //FileStream fs = new FileStream(@"C:\\temp\pdf-test.png", FileMode.OpenOrCreate);
            //fs.Write(imageBinaryData);
            //fs.Flush();
            //fs.Close();
            //fs.Dispose();

            return ImageDataFactory.Create(imageBinaryData);
        }

        throw new FileNotFoundException("File not found: " + fileName);
    }

    private Image GetImageForImageData(ImageData imageData, ImagePosition imagePosition)
    {
        return new Image(imageData)
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

    private void GetUserId(out string? userId)
    {
        userId = this.HttpContext.User.Claims
            .Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
            .Select(c => c.Value).FirstOrDefault();

        if (userId == null)
        {
            throw new ArgumentNullException("userId", "Invalid token.");
        }
    }

    private void GetAADUserName(out string? userName)
    {
        userName = this.HttpContext.User.Claims
            .Where(c => c.Type == "preferred_username").Select(c => c.Value)
            .FirstOrDefault();

        if (userName == null)
        {
            throw new ArgumentNullException("userName", "Invalid token.");
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

    private static PermitApplicationRequestModel SetBackgroundCheckHistory(PermitApplicationRequestModel application,
    PermitApplication existingApplication, string? userName)
    {
        if (existingApplication.Application.BackgroudCheck?.ProofOfID.Value !=
            application.Application.BackgroudCheck?.ProofOfID.Value)
        {
            application.Application.BackgroudCheck.ProofOfID.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.ProofOfID.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.ProofOfResidency.Value !=
            application.Application.BackgroudCheck?.ProofOfResidency.Value)
        {
            application.Application.BackgroudCheck.ProofOfResidency.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.ProofOfResidency.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.NCICWantsWarrants.Value !=
            application.Application.BackgroudCheck?.NCICWantsWarrants.Value)
        {
            application.Application.BackgroudCheck.NCICWantsWarrants.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.NCICWantsWarrants.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.Locals.Value !=
            application.Application.BackgroudCheck?.Locals.Value)
        {
            application.Application.BackgroudCheck.Locals.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.Locals.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.Probations.Value !=
            application.Application.BackgroudCheck?.Probations.Value)
        {
            application.Application.BackgroudCheck.Probations.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.Probations.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.DMVRecord.Value !=
            application.Application.BackgroudCheck?.DMVRecord.Value)
        {
            application.Application.BackgroudCheck.DMVRecord.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.DMVRecord.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.AKSsChecked.Value !=
            application.Application.BackgroudCheck?.AKSsChecked.Value)
        {
            application.Application.BackgroudCheck.AKSsChecked.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.AKSsChecked.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.Coplink.Value !=
            application.Application.BackgroudCheck?.Coplink.Value)
        {
            application.Application.BackgroudCheck.Coplink.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.Coplink.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.TrafficCourtPortal.Value !=
            application.Application.BackgroudCheck?.TrafficCourtPortal.Value)
        {
            application.Application.BackgroudCheck.TrafficCourtPortal.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.TrafficCourtPortal.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.PropertyAssesor.Value !=
            application.Application.BackgroudCheck?.PropertyAssesor.Value)
        {
            application.Application.BackgroudCheck.PropertyAssesor.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.PropertyAssesor.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.VoterRegistration.Value !=
            application.Application.BackgroudCheck?.VoterRegistration.Value)
        {
            application.Application.BackgroudCheck.VoterRegistration.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.VoterRegistration.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.DOJApprovalLetter.Value !=
            application.Application.BackgroudCheck?.DOJApprovalLetter.Value)
        {
            application.Application.BackgroudCheck.DOJApprovalLetter.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.DOJApprovalLetter.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.CIINumber.Value !=
            application.Application.BackgroudCheck?.CIINumber.Value)
        {
            application.Application.BackgroudCheck.CIINumber.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.CIINumber.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.DOJ.Value !=
            application.Application.BackgroudCheck?.DOJ.Value)
        {
            application.Application.BackgroudCheck.DOJ.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.DOJ.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.FBI.Value !=
            application.Application.BackgroudCheck?.FBI.Value)
        {
            application.Application.BackgroudCheck.FBI.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.FBI.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.SR14.Value !=
            application.Application.BackgroudCheck?.SR14.Value)
        {
            application.Application.BackgroudCheck.SR14.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.SR14.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.FirearmsReg.Value !=
            application.Application.BackgroudCheck?.FirearmsReg.Value)
        {
            application.Application.BackgroudCheck.FirearmsReg.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.FirearmsReg.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.AllDearChiefLTRsRCRD.Value !=
            application.Application.BackgroudCheck?.AllDearChiefLTRsRCRD.Value)
        {
            application.Application.BackgroudCheck.AllDearChiefLTRsRCRD.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.AllDearChiefLTRsRCRD.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.SafetyCertificate.Value !=
            application.Application.BackgroudCheck?.SafetyCertificate.Value)
        {
            application.Application.BackgroudCheck.SafetyCertificate.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.SafetyCertificate.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        if (existingApplication.Application.BackgroudCheck?.Restrictions.Value !=
            application.Application.BackgroudCheck?.Restrictions.Value)
        {
            application.Application.BackgroudCheck.Restrictions.ChangeMadeBy = userName;
            application.Application.BackgroudCheck.Restrictions.ChangeDateTimeUtc = DateTime.UtcNow;
        }

        return application;
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