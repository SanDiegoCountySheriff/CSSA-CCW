using CCW.Application.Clients;
using CCW.Application.Entities;
using CCW.Application.Mappers;
using CCW.Application.Models;
using CCW.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace CCW.Application.Controllers;


[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PermitApplicationController : ControllerBase
{
    private readonly IDocumentServiceClient _documentHttpClient;
    // private readonly IHttpClientFactory _httpClientFactory;
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

            if(result.Any())
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

            var existingApp = await _cosmosDbService.GetLastApplicationAsync(userId,
                application.Id.ToString(), isComplete: false,
                cancellationToken: default);

            if (existingApp == null)
            {
                return NotFound("Permit application cannot be found or application already submitted.");
            }

            bool isNewApplication = false;
            await _cosmosDbService.UpdateApplicationAsync(_userPermitApplicationMapper.Map(isNewApplication,
                    existingApp.Application.Comments, application),
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
    public async Task<IActionResult> UpdateUserApplication([FromBody] PermitApplicationRequestModel application)
    {
        try
        {
            GetAADUserName(out var userName);

            var existingApplication = await _cosmosDbService.GetUserApplicationAsync(application.Id.ToString(), cancellationToken: default);

            if (existingApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            var app = SetBackgroundCheckHistory(application, existingApplication, userName);

            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = "update application",
                    ChangeDateTimeUtc = DateTime.UtcNow,
                }
            };

            app.History = history;
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


    [Authorize(Policy = "B2CUsers")]
    [Route("deleteApplication")]
    [HttpPut]
    public async Task<IActionResult> DeleteApplication(string applicationId)
    {
        GetUserId(out var userId);

        try
        {
            var existingApp = await _cosmosDbService.GetLastApplicationAsync(userId, applicationId, isComplete:false, cancellationToken: default);

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
    public async Task<IActionResult> PrintApplication(string applicationId)
    {
        try
        {
            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            var template = "ApplicationTemplate";
            var response = await _documentHttpClient.GetApplicationTemplateAsync(template, cancellationToken: default);

            Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            MemoryStream outStream = new MemoryStream();

            PdfReader pdfReader = new PdfReader(streamToReadFrom);
            PdfWriter pdfWriter = new PdfWriter(outStream);
            PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

            Document docFileAll = new Document(doc);
            pdfWriter.SetCloseStream(false);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(doc, true);
            form.SetGenerateAppearance(true);

            if (userApplication.Application.ApplicationType.Contains("reserve"))
            {
                form.GetField("Reserve Officer - Official Use Only")
                    .SetValue("true", true);
            }
            else if (userApplication.Application.ApplicationType.Contains("judge"))
            {
                form.GetField("Judge - Official Use Only")
                    .SetValue("true", true);
            }
            else
            {
                form.GetField("Standard - Official Use Only")
                    .SetValue("true", true);
            }


            if (userApplication.Application.ApplicationType.Contains("renew"))
            {
                form.GetField("BOOLEAN(ccw_renewal_permit)~order.data.application.permitSelector")
                    .SetValue(userApplication.Application.ApplicationType ?? "", true);
            }
            else
            {
                form.GetField("BOOLEAN(ccw_new_permit)~order.data.application.permitSelector")
                    .SetValue(userApplication.Application.ApplicationType ?? "", true);
            }

            //Personal Information
            form.GetField("order.data.application.lastname").SetValue(userApplication.Application.PersonalInfo?.LastName ?? "", true);
            form.GetField("order.data.application.firstname").SetValue(userApplication.Application.PersonalInfo?.FirstName ?? "", true);
            form.GetField("order.data.application.middlename").SetValue(userApplication.Application.PersonalInfo?.MiddleName ?? "", true);
            form.GetField("order.data.application.ssn").SetValue(userApplication.Application.PersonalInfo?.Ssn ?? "", true);
            form.GetField("order.data.application.idnumber").SetValue(userApplication.Application.IdInfo?.IdNumber ?? "", true);

            //Current Address
            //string? residenceAddress = userApplication.Application.CurrentAddress?.AddressLine1 +
            //                           userApplication.Application.CurrentAddress?.AddressLine2;
            //form.GetField("Order.data.application.resiaddressdence").SetValue(residenceAddress ?? "", true);
            form.GetField("order.data.application.currentcity").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
            form.GetField("order.data.application.currentstate").SetValue(GetStateByName(userApplication.Application.CurrentAddress?.State) ?? "", true);
            form.GetField("order.data.application.currentzip").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
            form.GetField("order.data.application.currentcounty").SetValue(userApplication.Application.CurrentAddress?.County ?? "", true);

            //DOB
            form.GetField("DATE~order.data.application.bpbobvalue").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);
            form.GetField("order.data.application.bpcity").SetValue(userApplication.Application.DOB?.BirthCity ?? "", true);
            form.GetField("order.data.application.bpstate").SetValue(GetStateByName(userApplication.Application.DOB?.BirthState) ?? "", true);
            form.GetField("order.data.application.bpcountry").SetValue(userApplication.Application.DOB?.BirthCountry ?? "", true);
            if (!string.IsNullOrEmpty(userApplication.Application.DOB?.BirthDate))
            {
                DateTime birthDateTime = DateTime.ParseExact(userApplication.Application.DOB.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var age = CalculateAge(birthDateTime);
                form.GetField("order.data.application.bpage").SetValue(age.ToString(), true);
            }

            //Physical Appearance
            form.GetField("order.data.application.weight").SetValue(userApplication.Application.PhysicalAppearance?.Weight + "lbs" ?? "", true);
            form.GetField("order.data.application.eyeColor").SetValue(userApplication.Application.PhysicalAppearance?.EyeColor ?? "", true);
            form.GetField("order.data.application.gender").SetValue(userApplication.Application.PhysicalAppearance?.Gender.First().ToString().ToUpper() ?? "", true);
            form.GetField("order.data.application.hairColor").SetValue(userApplication.Application.PhysicalAppearance?.HairColor ?? "", true);
            form.GetField("POST_ft~order.data.application.heightFeet").SetValue(userApplication.Application.PhysicalAppearance?.HeightFeet + "ft" ?? "", true);
            form.GetField("POST_in~order.data.application.heightInches").SetValue(userApplication.Application.PhysicalAppearance?.HeightInch + "in" ?? "", true);

            //Immigration
            form.GetField("order.data.application.citizenCountry").SetValue(userApplication.Application.ImmigrantInformation?.CountryOfCitizenship ?? "", true);

            //Mailing Address
            form.GetField("PRE_Line 1:~order.data.application.mailingaddressline1").SetValue(userApplication.Application.MailingAddress?.AddressLine1 ?? "", true);
            form.GetField("PRE_Line 2:~order.data.application.mailingaddressline2").SetValue(userApplication.Application.MailingAddress?.AddressLine2 ?? "", true);
            form.GetField("order.data.application.mailingcity").SetValue(userApplication.Application.MailingAddress?.City ?? "", true);
            form.GetField("order.data.application.mailingstate").SetValue(GetStateByName(userApplication.Application.MailingAddress?.State) ?? "", true);
            form.GetField("order.data.application.mailingzip").SetValue(userApplication.Application.MailingAddress?.Zip ?? "", true);
            
            //Spouse
            form.GetField("order.data.application.spouselastname").SetValue(userApplication.Application.SpouseInformation?.LastName ?? "", true);
            form.GetField("order.data.application.spousefirstname").SetValue(userApplication.Application.SpouseInformation?.FirstName ?? "", true);
            form.GetField("order.data.application.spousemiddlename").SetValue(userApplication.Application.SpouseInformation?.MiddleName ?? "", true);
            form.GetField("PRE_Line 1:~order.data.application.spouseaddressline1").SetValue(userApplication.Application.SpouseAddressInformation?.AddressLine1 ?? "", true);
            form.GetField("PRE_Line 2:~order.data.application.spouseaddressline2").SetValue(userApplication.Application.SpouseAddressInformation?.AddressLine2 ?? "", true);
            form.GetField("order.data.application.spousecity").SetValue(userApplication.Application.SpouseAddressInformation?.City ?? "", true);
            form.GetField("order.data.application.spousestate").SetValue(GetStateByName(userApplication.Application.SpouseAddressInformation?.State) ?? "", true);
            form.GetField("order.data.application.spousezip").SetValue(userApplication.Application.SpouseAddressInformation?.Zip ?? "", true);
            
            //Work
            form.GetField("order.data.application.workoccupationfield").SetValue(userApplication.Application.WorkInformation?.Occupation ?? "", true);
            form.GetField("order.data.application.workname").SetValue(userApplication.Application.WorkInformation?.EmployerName ?? "", true);
            form.GetField("order.data.application.workaddressline1").SetValue(userApplication.Application.WorkInformation?.EmployerAddressLine1 ?? "", true);
            form.GetField("order.data.application.workaddressline2").SetValue(userApplication.Application.WorkInformation?.EmployerAddressLine2 ?? "", true);
            form.GetField("order.data.application.workcity").SetValue(userApplication.Application.WorkInformation?.EmployerCity ?? "", true);
            form.GetField("order.data.application.workstate").SetValue(GetStateByName(userApplication.Application.WorkInformation?.EmployerState) ?? "", true);
            form.GetField("order.data.application.workzip").SetValue(userApplication.Application.WorkInformation?.EmployerZip ?? "", true);

            //Investigator Notes
            form.GetField("PRE_Cell:~order.data.application.cellphone").SetValue(FormatPhoneNumber(userApplication.Application.Contact?.PrimaryPhoneNumber), true);
            form.GetField("PRE_Home:~order.data.application.homephone").SetValue(FormatPhoneNumber(userApplication.Application.Contact?.CellPhoneNumber), true);
            form.GetField("PRE_Work:~order.data.application.workphone").SetValue(FormatPhoneNumber(userApplication.Application.WorkInformation?.EmployerPhone), true);

            //Questions
            Dictionary<string, string> addendumPageAnswers = new Dictionary<string, string>();

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionOne.Value ? 
                    "BOOLEAN(true)~order.data.questions.issuepermit" : "NOTBOOLEAN(true)~order.data.questions.issuepermit")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionOne.Value.ToString(), true);
            form.GetField("order.data.questions.issuepermitExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionOneExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionTwo.Value ? "BOOLEAN(true)~order.data.questions.deniedCCW" : "NOTBOOLEAN(true)~order.data.questions.deniedCCW")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionTwo.Value.ToString(), true);
            form.GetField("order.data.questions.deniedCCWExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionTwoExp ?? "", true);

            var q3SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionThreeExp, 535);
            if (q3SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 3 Page 3 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionThreeExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionThree.Value ? "BOOLEAN(true)~order.data.questions.us_citizenship" : "NOTBOOLEAN(true)~order.data.questions.us_citizenship")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionThree.Value.ToString(), true);
            form.GetField("order.data.questions.us_citizenshipExplanation").SetValue(q3SplitValue.Item1, true);

            var q4SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionFourExp, 535);
            if (q4SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 4 Page 3 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionFourExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFour.Value ? "BOOLEAN(true)~order.data.questions.dishonorable_discharge" : "NOTBOOLEAN(true)~order.data.questions.dishonorable_discharge")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFour.Value.ToString(), true);
            form.GetField("order.data.questions.dishonorable_dischargeExplanation").SetValue(q4SplitValue.Item1, true);

            var q5SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionFiveExp, 535);
            if (q5SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 5 Page 4 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionFiveExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFive.Value ? "BOOLEAN(true)~order.data.questions.party_to_lawsuit" : "NOTBOOLEAN(true)~order.data.questions.party_to_lawsuit")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFive.Value.ToString(), true);
            form.GetField("order.data.questions.party_to_lawsuitExplanation").SetValue(q5SplitValue.Item1, true);

            var q6SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionSixExp, 535);
            if (q6SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 6 Page 4 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionSixExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSix.Value ? "BOOLEAN(true)~order.data.questions.restraining_order" : "NOTBOOLEAN(true)~order.data.questions.restraining_order")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSix.Value.ToString(), true);
            form.GetField("order.data.questions.restraining_orderExplanation").SetValue(q6SplitValue.Item1, true);

            var q7SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionSevenExp, 535);
            if (q7SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 7 Page 4 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionSevenExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSeven.Value ? "BOOLEAN(true)~order.data.questions.probation" : "NOTBOOLEAN(true)~order.data.questions.probation")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSeven.Value.ToString(), true);
            form.GetField("order.data.questions.probationExplanation").SetValue(q7SplitValue.Item1, true);

            //Question 8 - Does not have a Yes or No Checkbox
            var q8SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionEightExp, 1750);
            if (q8SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 8 Page 4 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionEightExp);
            }
            form.GetField("order.data.questions.trafficviolationsExplanation").SetValue(q8SplitValue.Item1, true);

            var q9SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionNineExp, 535);
            if (q9SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 9 Page 4 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionNineExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionNine.Value ? "BOOLEAN(true)~order.data.questions.conviction" : "NOTBOOLEAN(true)~order.data.questions.conviction")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionNine.Value.ToString(), true);
            form.GetField("order.data.questions.convictionExplanation").SetValue(q9SplitValue.Item1, true);

            var q10SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionTenExp, 535);
            if (q10SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 10 Page 4 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionTenExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionTen.Value ? "BOOLEAN(true)~order.data.questions.withheld_info" : "NOTBOOLEAN(true)~order.data.questions.withheld_info")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionTen.Value.ToString(), true);
            form.GetField("order.data.questions.withheld_infoExplanation").SetValue(q10SplitValue.Item1, true);

            //Question 11 - Does not have a Yes or No Checkbox
            var q11SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionElevenExp, 910);
            if (q11SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 11 Page 5 - Weapons: ", userApplication.Application.QualifyingQuestions?.QuestionElevenExp);
            }
            form.GetField("order.data.questions.dlRestrictionsExplanation").SetValue(q11SplitValue.Item1, true);

            var q12SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionTwelveExp, 550);
            if (q12SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 3 Page 9 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionTwelveExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionTwelve.Value ? "BOOLEAN(true)~order.data.questions.controlledsubstance" : "NOTBOOLEAN(true)~order.data.questions.controlledsubstance")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionTwelve.Value.ToString(), true);
            form.GetField("order.data.questions.controlledsubstanceExplanation").SetValue(q12SplitValue.Item1, true);

            var q13SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionThirteenExp, 550);
            if (q13SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 2 Page 9 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionThirteenExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionThirteen.Value ?
                    "BOOLEAN(true)~order.data.questions.mental" : "NOTBOOLEAN(true)~order.data.questions.mental")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionThirteen.Value.ToString(), true);
            form.GetField("order.data.questions.mentalExplanation").SetValue(q13SplitValue.Item1, true);

            var q14SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionFourteenExp, 550);
            if (q14SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 4 Page 10 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionFourteenExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFourteen.Value ? "BOOLEAN(true)~order.data.questions.firearms_incident" : "NOTBOOLEAN(true)~order.data.questions.firearms_incident")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFourteen.Value.ToString(), true);
            form.GetField("order.data.questions.firearms_incidentExplanation").SetValue(q14SplitValue.Item1, true);

            var q15SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionFifteenExp, 935);
            if (q15SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 6 Page 10 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionFifteenExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFifteen.Value ? "BOOLEAN(true)~order.data.questions.formal_charges" : "NOTBOOLEAN(true)~order.data.questions.formal_charges")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFifteen.Value.ToString(), true);
            form.GetField("order.data.questions.formal_chargesExplanation").SetValue(q15SplitValue.Item1, true);

            var q16SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionSixteenExp, 550);
            if (q16SplitValue.Item2)
            {
                addendumPageAnswers.Add("Question 5 Page 10 - Explanation: ", userApplication.Application.QualifyingQuestions?.QuestionSixteenExp);
            }
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSixteen.Value ? 
                    "BOOLEAN(true)~order.data.questions.DV" : "NOTBOOLEAN(true)~order.data.questions.DV")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSixteen.Value.ToString(), true);
            form.GetField("order.data.questions.DVExplanation").SetValue(q16SplitValue.Item1, true);

            //Question 17 - Does not have a Yes or No Checkbox
            var q17SplitValue = SplitAt(userApplication.Application.QualifyingQuestions?.QuestionSeventeenExp, 3375);
            if (q17SplitValue.Item2)
            {
                addendumPageAnswers.Add("Additional Information Page 10: ", userApplication.Application.QualifyingQuestions?.QuestionSeventeenExp);
            }
            form.GetField("order.data.questions.Good_cause_statementExplanation").SetValue(q17SplitValue.Item1, true);

            //others
            var maidenName = "Maiden name: " + userApplication.Application.PersonalInfo?.MaidenName;
            var aliases = string.Empty;
            foreach (var item in userApplication.Application.Aliases)
            {
                aliases += item.PrevLastName + " " + item.PrevFirstName + "; ";
            }

            var previousNames = string.IsNullOrEmpty(userApplication.Application.PersonalInfo?.MaidenName)
                ? aliases
                : maidenName + " - Other name(s): " + aliases;

            form.GetField("order.data.aliases[lastname, firstname, aliascity, aliasstate, aliasfilenumber]").SetValue(previousNames, true);
         
            var weapons = string.Empty;
            foreach (var item in userApplication.Application.Weapons)
            {
                weapons += item.Make + " " + item.Model + " " + item.Caliber + " " + item.SerialNumber + "; ";
            }
            form.GetField("order.data.weapons[make, model, caliber, serial]").SetValue(weapons, true);

            var previousAddresses = string.Empty;
            foreach (var item in userApplication.Application.PreviousAddresses)
            {
                previousAddresses += item.AddressLine1 + " " + item.AddressLine2 +  " " + item.City + " " + item.State + " " + item.Zip + ", " + item.County + ", " + item.Country + "\n ";
            }
            form.GetField("order.data.previousAddresses[street, street2, city, state, country, zip]").SetValue(previousAddresses, true);

            addendumPageAnswers.Add("Addendum ", "Section 2 - Applicant Clearance Questions");


            await streamToReadFrom.DisposeAsync();
            docFileAll.Close();

            byte[] byteInfo = outStream.ToArray();
            outStream.Write(byteInfo, 0, byteInfo.Length);
            outStream.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            //var fileName = userApplication.Id + "_" +
            //               userApplication.Application.PersonalInfo.LastName + "_" +
            //               userApplication.Application.PersonalInfo.FirstName + "_Printed_Application";

            //FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);
            
            //var saveFileResult = await _documentHttpClient.SaveApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            //Uncomment this to return the file as a download
           // fileStreamResult.FileDownloadName = "Output.pdf";

            return fileStreamResult;
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            throw new Exception("An error occur while trying to print permit application.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("printUnofficialLicense")]
    [HttpPut]
    public async Task<IActionResult> PrintUnofficialLicense(string applicationId)
    {
        try
        {
            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            var template = "UnofficialPermitTemplate";
            var response = await _documentHttpClient.GetApplicationTemplateAsync(template, cancellationToken: default);

            Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            MemoryStream outStream = new MemoryStream();

            PdfReader pdfReader = new PdfReader(streamToReadFrom);
            PdfWriter pdfWriter = new PdfWriter(outStream);
            PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

            Document docFileAll = new Document(doc);
            pdfWriter.SetCloseStream(false);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(doc, true);
            form.SetGenerateAppearance(true);

            //TODO: Get value from admin settings but waiting to know who will provide the value in cosmos
            form.GetField("REPLACE_PASS:~data.officeuse.CIINUMBERstatus").SetValue("", true);

            //TODO: Where are restrictions?
            //form.GetField("REPLACE_PASS:~data.officeuse.RESTRICTIONSstatus").SetValue("", true);

            var issueDate = DateTime.Now.ToString("MM/dd/yyyy");
            var expDate = DateTime.Now.AddYears(2).ToString("MM/dd/yyyy");
            form.GetField("ISSUE_DATE").SetValue(issueDate, true);
            form.GetField("EXPIRATION_DATE").SetValue(expDate, true);

            //Name
            var fullName = userApplication.Application.PersonalInfo?.LastName + " " +
                           userApplication.Application.PersonalInfo?.FirstName + " " +
                           userApplication.Application.PersonalInfo?.MiddleName;
            form.GetField("FULL_NAME").SetValue(fullName, true);

            //Personal Info
            form.GetField("order.data.application.weight").SetValue(userApplication.Application.PhysicalAppearance?.Weight + "lbs" ?? "", true);
            form.GetField("order.data.application.eyeColor").SetValue(userApplication.Application.PhysicalAppearance?.EyeColor ?? "", true);
            form.GetField("order.data.application.hairColor").SetValue(userApplication.Application.PhysicalAppearance?.HairColor ?? "", true);

            var height = (userApplication.Application.PhysicalAppearance?.HeightFeet + "ft " +
                          userApplication.Application.PhysicalAppearance?.HeightInch + "in");
            form.GetField("${(data_application_heightFeet!\"\")}' ${(data_application_heightInches!\"\")}").SetValue(height, true);

            //Current Address
            string? residenceAddress = string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.AddressLine1) ? "" :
                                        userApplication.Application.CurrentAddress?.AddressLine1 + " " +
                                        userApplication.Application.CurrentAddress?.AddressLine2 + ",\n" +
                                        userApplication.Application.CurrentAddress?.City + ", " +
                                        GetStateByName(userApplication.Application.CurrentAddress?.State) + " " +
                                        userApplication.Application.CurrentAddress?.Zip;
            form.GetField("${(data_application_currentaddressline1!\"\")} ${(data_application_currentaddressline2!\"\")}").SetValue(residenceAddress, true);

            //Work
            string workAddress = string.IsNullOrEmpty(userApplication.Application.WorkInformation?.EmployerAddressLine1) ? "" :
                                userApplication.Application.WorkInformation?.EmployerAddressLine1 + " " +
                                userApplication.Application.WorkInformation?.EmployerAddressLine2 + ", " +
                                userApplication.Application.WorkInformation?.EmployerCity + ", " +
                                GetStateByName(userApplication.Application.WorkInformation?.EmployerState) + " " +
                                userApplication.Application.WorkInformation?.EmployerZip;
            form.GetField("${(data_application_workaddressline1!\"\")} ${(data_application_workaddressline2!\"\")}").SetValue(workAddress, true);
            form.GetField("order.data.application.workoccupationfield").SetValue(userApplication.Application.WorkInformation?.Occupation ?? "", true);

            //DOB
            form.GetField("DATE~order.data.application.bpbobvalue").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);

            //Weapons
            var weapons = string.Empty;

            //only the first 3 weapons
            foreach (var item in userApplication.Application.Weapons)
            {
                form.GetField("order.data.weapons[make]").SetValue(item.Make ?? "", true);
                form.GetField("order.data.weapons[serial]").SetValue(item.SerialNumber ?? "", true);
                form.GetField("order.data.weapons[caliber]").SetValue(item.Caliber ?? "", true);
                form.GetField("order.data.weapons[model]").SetValue(item.Model ?? "", true);
            }

            docFileAll.Close();

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            byte[] byteInfo = outStream.ToArray();
            outStream.Write(byteInfo, 0, byteInfo.Length);
            outStream.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            //Uncomment this to return the file as a download
             //fileStreamResult.FileDownloadName = "Output.pdf";

            return fileStreamResult;
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            throw new Exception("An error occur while trying to print unofficial license.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [Route("printOfficialLicense")]
    [HttpPut]
    public async Task<IActionResult> PrintOfficialLicense(string applicationId)
    {
        try
        {
            var userApplication = await _cosmosDbService.GetUserApplicationAsync(applicationId, cancellationToken: default);

            if (userApplication == null)
            {
                return NotFound("Permit application cannot be found.");
            }

            var template = "OfficialPermitTemplate";
            var response = await _documentHttpClient.GetOfficialLicenseTemplateAsync(template, cancellationToken: default);

            Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            MemoryStream outStream = new MemoryStream();

            PdfReader pdfReader = new PdfReader(streamToReadFrom);
            PdfWriter pdfWriter = new PdfWriter(outStream);
            PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

            Document docFileAll = new Document(doc);
            pdfWriter.SetCloseStream(false);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(doc, true);
            form.SetGenerateAppearance(true);

            if (userApplication.Application.ApplicationType.Contains("reserve"))
            {
                form.GetField("BOOLEAN(Reserve)~order.data.application")
                    .SetValue("true", true);
            }
            else if (userApplication.Application.ApplicationType.Contains("judge"))
            {
                form.GetField("BOOLEAN(Judicial)~order.data.application.productName")
                    .SetValue("true", true);
            }
            else
            {
                form.GetField("BOOLEAN(Standard)~order.data.application.productName")
                    .SetValue("true", true);
            }

            var issueDate = DateTime.Now.ToString("MM/dd/yyyy");
            var expDate = DateTime.Now.AddYears(2).ToString("MM/dd/yyyy");
            form.GetField("ISSUE_DATE").SetValue(issueDate, true);
            form.GetField("EXPIRATION_DATE").SetValue(expDate, true);

            if (userApplication.Application.ApplicationType.Contains("renew"))
            {
                form.GetField("BOOLEAN(ccw_renewal_permit)~order").SetValue(expDate, true);
            }
            else
            {
                form.GetField("BOOLEAN(ccw_new_permit)~order").SetValue(expDate, true);
            }

            //TODO: Get values from admin when in cosmos db
            //TODO: ID pdf?
            //form.GetField("").SetValue("agency id here", true);
            //form.GetField("~data.officeuse.CIINUMBERstatus").SetValue("cii number here", true);
            //form.GetField("").SetValue("local agency number here", true);
            //form.GetField("").SetValue("ORI number here", true);

            //Name
            var fullName = userApplication.Application.PersonalInfo?.LastName + " " +
                           userApplication.Application.PersonalInfo?.FirstName + " " +
                           userApplication.Application.PersonalInfo?.MiddleName;

            form.GetField("FULL_NAME").SetValue(fullName, true);

            //Personal Info
            form.GetField("order.data.application.weight").SetValue(userApplication.Application.PhysicalAppearance?.Weight + "lbs" ?? "", true);
            form.GetField("order.data.application.eyeColor").SetValue(userApplication.Application.PhysicalAppearance?.EyeColor ?? "", true);
            form.GetField("order.data.application.hairColor").SetValue(userApplication.Application.PhysicalAppearance?.HairColor ?? "", true);
            form.GetField("POST_ft~order.data.application.heightFeet").SetValue(userApplication.Application.PhysicalAppearance?.HeightFeet + "ft" ?? "", true);
            form.GetField("POST_in~order.data.application.heightInches").SetValue(userApplication.Application.PhysicalAppearance?.HeightInch + "in" ?? "", true);

            //Current Address
            string? residenceAddress = userApplication.Application.CurrentAddress?.AddressLine1 +
                                       userApplication.Application.CurrentAddress?.AddressLine2;
            form.GetField("order.data.application.currentaddressline1").SetValue(residenceAddress ?? "", true);
            form.GetField("order.data.application.currentcity").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
            form.GetField("order.data.application.currentzip").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
            form.GetField("order.data.application.currentcounty").SetValue(userApplication.Application.CurrentAddress?.County ?? "", true);

            //Work
            string workAddress = userApplication.Application.WorkInformation?.EmployerAddressLine1 + " " +
                                 userApplication.Application.WorkInformation?.EmployerAddressLine2 + ", " +
                                 userApplication.Application.WorkInformation?.EmployerCity + ", " +
                                 GetStateByName(userApplication.Application.WorkInformation?.EmployerState) + " " +
                                 userApplication.Application.WorkInformation?.EmployerZip;
            form.GetField("order.data.application.workaddressline1").SetValue(workAddress, true);
            form.GetField("order.data.application.workoccupationfield").SetValue(userApplication.Application.WorkInformation?.Occupation ?? "", true);
            
            //DOB
            form.GetField("DATE~order.data.application.bpbobvalue").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);

            //Weapons
            var weapons = string.Empty;
            foreach (var item in userApplication.Application.Weapons)
            {
                form.GetField("order.data.weapons[make]").SetValue(item.Make ?? "", true);
                form.GetField("order.data.weapons[serial]").SetValue(item.SerialNumber ?? "", true);
                form.GetField("order.data.weapons[caliber]").SetValue(item.Caliber ?? "", true);
                form.GetField("order.data.weapons[model]").SetValue(item.Model ?? "", true);

                form.GetField("order.data.weapons[make].1").SetValue(item.Make ?? "", true);
                form.GetField("order.data.weapons[serial].1").SetValue(item.SerialNumber ?? "", true);
                form.GetField("order.data.weapons[caliber].1").SetValue(item.Caliber ?? "", true);
                form.GetField("order.data.weapons[model].1").SetValue(item.Model ?? "", true);

                form.GetField("order.data.weapons[make].2").SetValue(item.Make ?? "", true);
                form.GetField("order.data.weapons[serial].2").SetValue(item.SerialNumber ?? "", true);
                form.GetField("order.data.weapons[caliber].2").SetValue(item.Caliber ?? "", true);
                form.GetField("order.data.weapons[model].2").SetValue(item.Model ?? "", true);
            }
            
            docFileAll.Close();

            Response.Headers.Append("Content-Disposition", "inline");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            byte[] byteInfo = outStream.ToArray();
            outStream.Write(byteInfo, 0, byteInfo.Length);
            outStream.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            //Uncomment this to return the file as a download
            //fileStreamResult.FileDownloadName = "Output.pdf";

            return fileStreamResult;

        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            throw new Exception("An error occur while trying to print official license.");
        }
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
        var birthday = new DateTime(birthDate.Year, birthDate.Day, birthDate.Month);
        int age = (int)((DateTime.Now - birthday).TotalDays / 365.242199);

        return age;
    }

    private static string FormatPhoneNumber(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value.Length > 10)
        {
            return Convert.ToInt64(value)
                .ToString("###-###-#### " + new String('#', (value.Length - 10)));
        }

        return value;
    }

    private Tuple<string, bool> SplitAt(string line, int charcount)
    {
        string result = "";
        bool addToAddendumPage = false;

        if (string.IsNullOrEmpty(line))
        {
            return new Tuple<string, bool>(string.Empty, addToAddendumPage);
        }

        if (charcount >= line.Length)
        {
            return new Tuple<string, bool>(line, addToAddendumPage);
        }

        addToAddendumPage = true;

        return new Tuple<string, bool>(line.Substring(0, charcount) + "...", addToAddendumPage);
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
