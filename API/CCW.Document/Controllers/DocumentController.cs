using Azure.Storage.Blobs.Models;
using CCW.Common.Services.Contracts;
using CCW.Document.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Document.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IAzureStorage _azureStorage;
    private readonly ILogger<DocumentController> _logger;
    private readonly ITenantIdResolver _tenantIdResolver;

    private readonly string[] _allowedFileTypes = new[] { "image/jpeg", "image/png", "application/pdf", "multipart/form-data" };

    public DocumentController(
        IAzureStorage azureStorage,
        ILogger<DocumentController> logger,
        ITenantIdResolver tenantIdResolver
    )
    {
        _azureStorage = azureStorage;
        _logger = logger;
        _tenantIdResolver = tenantIdResolver;
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpPost("uploadApplicantFile", Name = "uploadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadApplicantFile(
        IFormFile fileToUpload,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            GetUserId(out var userId);
            saveAsFileName = userId + "_" + saveAsFileName;

            if (string.IsNullOrEmpty(fileToUpload.ContentType) || !_allowedFileTypes.Contains(fileToUpload.ContentType))
            {
                return ValidationProblem("Content type missing or invalid.");
            }

            await _azureStorage.UploadApplicantFileAsync(fileToUpload, saveAsFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to upload applicant file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpPost("uploadUserApplicantFile", Name = "uploadUserApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadUserApplicantFile(
        IFormFile fileToUpload,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(fileToUpload.ContentType) || !_allowedFileTypes.Contains(fileToUpload.ContentType))
            {
                return ValidationProblem("Content type missing or invalid.");
            }

            await _azureStorage.UploadApplicantFileAsync(fileToUpload, saveAsFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to upload user applicant file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpPost("updateAdminApplicationFileName", Name = "updateAdminApplicationFileName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAdminApplicationFileName(
        string oldName,
        string newName,
        CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.UpdateAdminApplicationFileNameAsync(oldName, newName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to update admin application file name.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpPost("updateApplicationFileName", Name = "updateApplicationFileName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateApplicationFileName(
        string oldName,
        string newName,
        CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.UpdateApplicationFileNameAsync(oldName, newName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to update application file name.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpPost("uploadAdminUserFile", Name = "uploadAdminUserFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadAdminUserFile(
        IFormFile fileToUpload,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(fileToUpload.ContentType) || !_allowedFileTypes.Contains(fileToUpload.ContentType))
            {
                return ValidationProblem("Content type missing or invalid");
            }

            GetUserId(out var userId);

            saveAsFileName = $"{userId}_{saveAsFileName}";

            await _azureStorage.UploadAdminUserFileAsync(fileToUpload, saveAsFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to upload admin user file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpPost("uploadAgencyFile", Name = "uploadAgencyFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadAgencyFile(
        IFormFile fileToUpload,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(fileToUpload.ContentType) || !_allowedFileTypes.Contains(fileToUpload.ContentType))
            {
                return ValidationProblem("Content type missing or invalid.");
            }

            await _azureStorage.UploadAgencyFileAsync(fileToUpload, saveAsFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to upload agency file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpPost("uploadAgencyLogo", Name = "uploadAgencyLogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadAgencyLogo(
        IFormFile fileToUpload,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(fileToUpload.ContentType) || !_allowedFileTypes.Contains(fileToUpload.ContentType))
            {
                return ValidationProblem("Content type missing or invalid.");
            }

            await _azureStorage.UploadAgencyLogoAsync(fileToUpload, saveAsFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to upload agency logo.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("downloadAdminUserFile", Name = "downloadAdminUserFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadAdminUserFile(
        string adminUserFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            GetUserId(out var userId);

            adminUserFileName = $"{userId}_{adminUserFileName}";

            MemoryStream ms = new MemoryStream();

            var file = await _azureStorage.DownloadAdminUserFileAsync(adminUserFileName, cancellationToken: cancellationToken);

            if (await file.ExistsAsync(cancellationToken))
            {
                await file.DownloadToAsync(ms, cancellationToken);
                BlobProperties properties = await file.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

                    Response.Headers.Add("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    return new FileStreamResult(blobStream, properties.ContentType);
                }

                var bytes = ms.ToArray();
                var b64String = Convert.ToBase64String(bytes);

                return Content("data:image/png;base64," + b64String);
            }

            return Content("File/image does not exist");
        }
        catch (Exception ex)
        {
            var originalException = ex.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download applicant file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("downloadAdminApplicationFile", Name = "downloadAdminApplicationFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadAdminApplicationFile(
    string adminApplicationFileName,
    CancellationToken cancellationToken)
    {
        try
        {
            MemoryStream ms = new MemoryStream();

            var file = await _azureStorage.DownloadAdminApplicationFileAsync(adminApplicationFileName, cancellationToken: cancellationToken);
            if (await file.ExistsAsync(cancellationToken))
            {
                await file.DownloadToAsync(ms, cancellationToken);
                BlobProperties properties = await file.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

                    Response.Headers.Add("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    return new FileStreamResult(blobStream, properties.ContentType);
                }

                var bytes = ms.ToArray();
                var b64String = Convert.ToBase64String(bytes);

                return Content("data:image/png;base64," + b64String);
            }

            return Content("File/image does not exist");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download admin applicant file.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpGet("downloadApplicantFile", Name = "downloadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadApplicantFile(
        string applicantFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            GetUserId(out var userId);

            applicantFileName = userId + "_" + applicantFileName;

            MemoryStream ms = new MemoryStream();

            var file = await _azureStorage.DownloadApplicantFileAsync(applicantFileName, cancellationToken: cancellationToken);

            if (await file.ExistsAsync(cancellationToken))
            {
                await file.DownloadToAsync(ms, cancellationToken);
                BlobProperties properties = await file.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

                    Response.Headers.Add("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    return new FileStreamResult(blobStream, properties.ContentType);
                }

                var bytes = ms.ToArray();
                var b64String = Convert.ToBase64String(bytes);

                return Content("data:image/png;base64," + b64String);
            }

            return Content("File/image does not exist");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download applicant file.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpGet("downloadAgreementFile", Name = "downloadAgreementFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadAgreementFile(
        string agreementFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            MemoryStream ms = new MemoryStream();

            var file = await _azureStorage.DownloadAgencyFileAsync(agreementFileName, cancellationToken: cancellationToken);

            if (await file.ExistsAsync(cancellationToken))
            {
                await file.DownloadToAsync(ms, cancellationToken);
                BlobProperties properties = await file.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

                    Response.Headers.Add("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    return new FileStreamResult(blobStream, properties.ContentType);
                }
            }

            return Content("File/image does not exist");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);

            return NotFound("An error occur while trying to download applicant file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getPdfFormValidation", Name = "getPdfFormValidation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPdfFormValidation(CancellationToken cancellationToken)
    {
        var documents = new string[] {
            "BOF_4012_rev_01_2024",
            "BOF_4012_rev_08_2022",
            "BOF_4502_rev_01_2024",
            "BOF_1032_rev_01_2024",
            "BOF_1031_orig_01_2024",
            "BOF_8018_rev_01_2024",
            "BCIA_8016_rev_04_2020",
            "BOF_1027_rev_01_2024",
            "BOF_1034_orig_01_2024",
            "BCIA_8020_rev_01_2014",
            "Prohibiting_Categories_rev_01_2024",
            "Official_License",
            "Unofficial_License",
            "Conditions_for_Issuance",
            "False_Info",
        };

        var result = new Dictionary<string, bool>();

        foreach (var document in documents)
        {
            result.Add(document, await _azureStorage.ValidateAgencyFileAsync(document, cancellationToken));
        }

        return new OkObjectResult(result);
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("getUserPortrait", Name = "getUserPortrait")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserPortrait(
    string applicantFileName,
    CancellationToken cancellationToken)
    {
        try
        {
            MemoryStream ms = new();

            var file = await _azureStorage.DownloadApplicantFileAsync(applicantFileName, cancellationToken: cancellationToken);

            if (await file.ExistsAsync(cancellationToken))
            {
                await file.DownloadToAsync(ms, cancellationToken);
                var bytes = ms.ToArray();
                var b64String = Convert.ToBase64String(bytes);

                return Content("data:image/png;base64," + b64String);
            }

            return Content("File/image does not exist");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download user applicant file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("downloadUserApplicantFile", Name = "downloadUserApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadUserApplicantFile(
    string applicantFileName,
    CancellationToken cancellationToken)
    {
        try
        {
            MemoryStream ms = new MemoryStream();

            var file = await _azureStorage.DownloadApplicantFileAsync(applicantFileName, cancellationToken: cancellationToken);
            if (await file.ExistsAsync(cancellationToken))
            {
                await file.DownloadToAsync(ms, cancellationToken);
                BlobProperties properties = await file.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;

                    Response.Headers.Add("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    return new FileStreamResult(blobStream, properties.ContentType);
                }

                byte[] bytes = ms.ToArray();

                var b64String = Convert.ToBase64String(bytes);

                return Content("data:image/png;base64," + b64String);

            }

            return Content("File/image does not exist");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download user applicant file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("downloadAgencyFile", Name = "downloadAgencyFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadAgencyFile(
        string agencyFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            MemoryStream ms = new MemoryStream();

            var file = await _azureStorage.DownloadAgencyFileAsync(agencyFileName, cancellationToken: cancellationToken);
            if (await file.ExistsAsync(cancellationToken))
            {
                await file.DownloadToAsync(ms, cancellationToken);
                Stream blobStream = file.OpenReadAsync(cancellationToken: cancellationToken).Result;
                BlobProperties properties = await file.GetPropertiesAsync(cancellationToken: cancellationToken);

                if (properties.ContentType == "application/pdf")
                {
                    Response.Headers.Add("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                return new FileStreamResult(blobStream, properties.ContentType);
            }

            return Content("File does not exist");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download agency file.");
        }
    }

    [HttpGet("downloadAgencyLogo", Name = "downloadAgencyLogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadAgencyLogo(
        CancellationToken cancellationToken)
    {
        try
        {
            var tenantId = GetTenantId(HttpContext);

            var result = await _azureStorage.DownloadAgencyLogoAsync(tenantId, "agency_logo", cancellationToken: cancellationToken);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download agency logo.");
        }
    }

    [HttpGet("downloadAgencyLandingPageImage", Name = "downloadAgencyLandingPageImage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadAgencyLandingPageImage(
        CancellationToken cancellationToken)
    {
        try
        {
            var tenantId = GetTenantId(HttpContext);

            var result = await _azureStorage.DownloadAgencyLogoAsync(tenantId, "agency_landing_page_image", cancellationToken: cancellationToken);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download agency landing page image.");
        }
    }

    [HttpGet("downloadAgencyHomePageImage", Name = "downloadAgencyHomePageImage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadAgencyHomePageImage(
    CancellationToken cancellationToken)
    {
        try
        {
            var tenantId = GetTenantId(HttpContext);

            var result = await _azureStorage.DownloadAgencyLogoAsync(tenantId, "agency_home_page_image", cancellationToken: cancellationToken);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download agency landing page image.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpGet("downloadAgencySignature", Name = "downloadAgencySignature")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadAgencySignature(
        CancellationToken cancellationToken)
    {
        try
        {
            var tenantId = GetTenantId(HttpContext);

            var result = await _azureStorage.DownloadAgencyLogoAsync(tenantId, "agency_sheriff_signature_image", cancellationToken: cancellationToken);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download agency landing page image.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpDelete("deleteAgencyLogo", Name = "deleteAgencyLogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAgencyLogo(
        string agencyLogoName, CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.DeleteAgencyLogoAsync(agencyLogoName, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete agency logo.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpDelete("deleteApplicantFile", Name = "deleteApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteApplicantFile(
        string applicantFileName, CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.DeleteApplicantFileAsync(applicantFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete applicant file.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [HttpDelete("deleteApplicantFilePublic", Name = "deleteApplicantFilePublic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteApplicantFilePublic(
        string applicantFileName, CancellationToken cancellationToken)
    {
        try
        {
            GetUserId(out var userId);
            applicantFileName = userId + "_" + applicantFileName;

            await _azureStorage.DeleteApplicantFilePublicAsync(applicantFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete applicant file.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [HttpDelete("deleteAdminApplicationFile", Name = "deleteAdminApplicationFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAdminApplicationFile(
        string adminApplicationFileName, CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.DeleteAdminApplicationFileAsync(adminApplicationFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to delete admin applicant file.");
        }
    }

    private void GetUserId(out string userId)
    {
        userId = this.HttpContext.User.Claims
            .Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
            .Select(c => c.Value).FirstOrDefault();

        if (userId == null)
        {
            throw new ArgumentNullException("userId", "Invalid token.");
        }
    }

    private string GetTenantId(HttpContext context)
    {
        var referrer = context.Request.Headers.Referer.ToString().Split("/")[2].Split(".")[0];

        if (referrer.Contains(':'))
        {
            referrer = referrer.Split(":")[1];
        }

        var tenantId = _tenantIdResolver.GetTenantId(referrer);

        return tenantId;
    }
}