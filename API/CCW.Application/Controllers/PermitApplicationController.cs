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


namespace CCW.Application.Controllers;


[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class PermitApplicationController : ControllerBase
{
   // private readonly IDocumentServiceClient _documentHttpClient;
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
      //  IDocumentServiceClient documentHttpClient,
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
      //  _documentHttpClient = documentHttpClient;
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
            History[] history = new[]{
                new History
                {
                    ChangeMadeBy =  userName,
                    Change = "update application",
                    ChangeDateTimeUtc = DateTime.UtcNow,
                }
            };

            application.History = history;
            bool isNewApplication = false;

            await _cosmosDbService.UpdateUserApplicationAsync(_permitApplicationMapper.Map(isNewApplication, application), cancellationToken: default);
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


  //  [Authorize(Policy = "AADUsers")]
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

            //go to document get application template
       //     var pdfTemplate = _documentHttpClient.GetApplicationTemplateAsync(template, cancellationToken: default);

            //fill pdf
            IDictionary<string, PdfFormField> fields;
            MemoryStream outStream = new MemoryStream();

            //await pdfTemplate.DownloadToStreamAsync(outStream);

            //var stream = await pdfTemplate.OpenReadAsync();
            //var docFile = File(stream, pdfTemplate.Properties.ContentType);

            //PdfReader pdfReader = new PdfReader(docFile.FileStream);


            PdfReader pdfReader = new PdfReader("PdfFiles/Application_Template.pdf");
            PdfWriter pdfWriter = new PdfWriter(outStream);
            PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

            iText.Layout.Document docFileAll = new iText.Layout.Document(doc);
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
            //form.GetField("order.data.application.currentaddressline1").SetValue(userApplication.Application.CurrentAddress?.AddressLine1 ?? "", true);
            //form.GetField("order.data.application.currentaddressline2").SetValue(userApplication.Application.CurrentAddress?.AddressLine2 ?? "", true);
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
            form.GetField("PRE_Cell:~order.data.application.cellphone").SetValue(userApplication.Application.Contact?.PrimaryPhoneNumber ?? "", true);
            form.GetField("PRE_Home:~order.data.application.homephone").SetValue(userApplication.Application.Contact?.CellPhoneNumber ?? "", true);
            form.GetField("PRE_Work:~order.data.application.workphone").SetValue(userApplication.Application.WorkInformation?.EmployerPhone ?? "", true);
            //  form.GetField("PRE_Line 2:~order.data.application.mailingaddressline1").SetValue(userApplication.Application.MailingAddress?.AddressLine1 ?? "", true);
            //  form.GetField("PRE_Line 2:~order.data.application.mailingaddressline2").SetValue(userApplication.Application.MailingAddress?.AddressLine2 ?? "", true);
            //  form.GetField("PRE_Line 2:~order.data.application.spouseaddressline1").SetValue(userApplication.Application.SpouseAddressInformation?.AddressLine1 ?? "", true);
            //  form.GetField("PRE_Line 2:~order.data.application.spouseaddressline2").SetValue(userApplication.Application.SpouseAddressInformation?.AddressLine2 ?? "", true);


            //Questions
            form.GetField(userApplication.Application.QualifyingQuestions.QuestionOne.Value ? 
                    "BOOLEAN(true)~order.data.questions.issuepermit" : "NOTBOOLEAN(true)~order.data.questions.issuepermit")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionOne.Value.ToString(), true);
            form.GetField("order.data.questions.issuepermitExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionOneExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionTwo.Value ? "BOOLEAN(true)~order.data.questions.deniedCCW" : "NOTBOOLEAN(true)~order.data.questions.deniedCCW")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionTwo.Value.ToString(), true);
            form.GetField("order.data.questions.deniedCCWExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionTwoExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionThree.Value ? "BOOLEAN(true)~order.data.questions.us_citizenship" : "NOTBOOLEAN(true)~order.data.questions.us_citizenship")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionThree.Value.ToString(), true);
            form.GetField("order.data.questions.us_citizenshipExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionThreeExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFour.Value ? "BOOLEAN(true)~order.data.questions.dishonorable_discharge" : "NOTBOOLEAN(true)~order.data.questions.dishonorable_discharge")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFour.Value.ToString(), true);
            form.GetField("order.data.questions.dishonorable_dischargeExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFourExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFive.Value ? "BOOLEAN(true)~order.data.questions.party_to_lawsuit" : "NOTBOOLEAN(true)~order.data.questions.party_to_lawsuit")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFive.Value.ToString(), true);
            form.GetField("order.data.questions.party_to_lawsuitExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFiveExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSix.Value ? "BOOLEAN(true)~order.data.questions.restraining_order" : "NOTBOOLEAN(true)~order.data.questions.restraining_order")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSix.Value.ToString(), true);
            form.GetField("order.data.questions.restraining_orderExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSixExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSeven.Value ? "BOOLEAN(true)~order.data.questions.probation" : "NOTBOOLEAN(true)~order.data.questions.probation")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSeven.Value.ToString(), true);
            form.GetField("order.data.questions.probationExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSevenExp ?? "", true);

            //Question 8 - Does not have a Yes or No Checkbox
            form.GetField("order.data.questions.trafficviolationsExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionEightExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionNine.Value ? "BOOLEAN(true)~order.data.questions.conviction" : "NOTBOOLEAN(true)~order.data.questions.conviction")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionNine.Value.ToString(), true);
            form.GetField("order.data.questions.convictionExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionNineExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionTen.Value ? "BOOLEAN(true)~order.data.questions.withheld_info" : "NOTBOOLEAN(true)~order.data.questions.withheld_info")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionTen.Value.ToString(), true);
            form.GetField("order.data.questions.withheld_infoExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionTenExp ?? "", true);

            //Question 11 - Does not have a Yes or No Checkbox
            form.GetField("order.data.questions.dlRestrictionsExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionElevenExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionTwelve.Value ? "BOOLEAN(true)~order.data.questions.controlledsubstance" : "NOTBOOLEAN(true)~order.data.questions.controlledsubstance")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionTwelve.Value.ToString(), true);
            form.GetField("order.data.questions.controlledsubstanceExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionTwelveExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionThirteen.Value ?
                    "BOOLEAN(true)~order.data.questions.mental" : "NOTBOOLEAN(true)~order.data.questions.mental")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionThirteen.Value.ToString(), true);
            form.GetField("order.data.questions.mentalExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionThirteenExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFourteen.Value ? "BOOLEAN(true)~order.data.questions.firearms_incident" : "NOTBOOLEAN(true)~order.data.questions.firearms_incident")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFourteen.Value.ToString(), true);
            form.GetField("order.data.questions.firearms_incidentExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFourteenExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionFifteen.Value ? "BOOLEAN(true)~order.data.questions.formal_charges" : "NOTBOOLEAN(true)~order.data.questions.formal_charges")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionFifteen.Value.ToString(), true);
            form.GetField("order.data.questions.formal_chargesExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFifteenExp ?? "", true);

            form.GetField(userApplication.Application.QualifyingQuestions.QuestionSixteen.Value ? 
                    "BOOLEAN(true)~order.data.questions.DV" : "NOTBOOLEAN(true)~order.data.questions.DV")
                .SetValue(userApplication.Application.QualifyingQuestions.QuestionSixteen.Value.ToString(), true);
            form.GetField("order.data.questions.DVExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSixteenExp ?? "", true);

            //Question 17 - Does not have a Yes or No Checkbox
            form.GetField("order.data.questions.Good_cause_statementExplanation").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSeventeenExp ?? "", true);

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
          
      
            docFileAll.Close();

            //Response.Headers.Append("Content-Disposition", "inline");
            //Response.Headers.Add("X-Content-Type-Options", "nosniff");

            byte[] byteInfo = outStream.ToArray();
            outStream.Write(byteInfo, 0, byteInfo.Length);
            outStream.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");

            //Uncomment this to return the file as a download
            fileStreamResult.FileDownloadName = "Output.pdf";

            return fileStreamResult;

            //send to document filled application

            //return to user

            return Ok();
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

            //go to document get unofficial license template

            //go to document get sheriffs signature

            //fill pdf
            PdfReader reader = new PdfReader("Permit_Template.pdf");
            PdfWriter writer = new PdfWriter("ccw_application_mp_complete.pdf");

            //send to document unofficial license application

            //return to user

            return Ok();
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

            //go to document get official license template

            //go to document get sheriffs signature

            //fill pdf

            //send to document official license application

            //return to user

            return Ok();
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