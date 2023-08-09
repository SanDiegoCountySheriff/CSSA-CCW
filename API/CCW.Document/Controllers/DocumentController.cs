using Azure.Storage.Blobs.Models;
using CCW.Document.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCW.Document.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IAzureStorage _azureStorage;
    private readonly ILogger<DocumentController> _logger;

    private readonly string[] _allowedFileTypes = new[] { "image/jpeg", "image/png", "application/pdf", "multipart/form-data" };

    public DocumentController(
        IAzureStorage azureStorage,
        ILogger<DocumentController> logger
    )
    {
        _azureStorage = azureStorage;
        _logger = logger;
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
    [HttpPost("uploadAdminApplicationFile", Name = "uploadAdminApplicationFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadAdminApplicationFile(
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

            await _azureStorage.UploadAdminApplicationFileAsync(fileToUpload, saveAsFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to upload admin application file.");
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


    [Authorize(Policy = "RequireSystemAdminOnly")]
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

            if (await file.ExistsAsync())
            {
                await file.DownloadToAsync(ms);
                BlobProperties properties = await file.GetPropertiesAsync();

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync().Result;

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
            if (await file.ExistsAsync())
            {
                await file.DownloadToAsync(ms);
                BlobProperties properties = await file.GetPropertiesAsync();

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync().Result;

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
            throw new Exception("An error occur while trying to download user applicant file.");
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

            if (await file.ExistsAsync())
            {
                await file.DownloadToAsync(ms);
                BlobProperties properties = await file.GetPropertiesAsync();

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync().Result;

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
            if (await file.ExistsAsync())
            {
                await file.DownloadToAsync(ms);
                BlobProperties properties = await file.GetPropertiesAsync();

                if (properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync().Result;

                    Response.Headers.Add("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    return new FileStreamResult(blobStream, properties.ContentType);
                }

                byte[] bytes = ms.ToArray();

                return new FileContentResult(bytes, properties.ContentType);
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
            if (await file.ExistsAsync())
            {
                await file.DownloadToAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
                BlobProperties properties = await file.GetPropertiesAsync();

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
        string agencyLogoName,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _azureStorage.DownloadAgencyLogoAsync(agencyLogoName, cancellationToken: cancellationToken);

            return Ok(result);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to download agency logo.");
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

    [Authorize(Policy = "AADUsers")]
    [HttpDelete("deleteAdminApplicationFile", Name = "deleteAdminApplicationFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAdminApplicationFile(
        string adminApplicationFileName, CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.DeleteApplicantFileAsync(adminApplicationFileName, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to delete applicant file.");
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
}