using Microsoft.AspNetCore.Mvc;
using CCW.Document.Services;
using Microsoft.AspNetCore.Authorization;


namespace CCW.Document.Controllers;

[Route(Constants.AppName + "/v1/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private IAzureStorage _azureStorage;
    private readonly ILogger<DocumentController> _logger;

    private string[] _allowedFileTypes = new[] { "image/jpeg", "image/png", "application/pdf" };

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

            await _azureStorage.UploadApplicantFileAsync(fileToUpload, saveAsFileName, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to upload applicant file.");
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

            await _azureStorage.UploadApplicantFileAsync(fileToUpload, saveAsFileName, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to upload user applicant file.");
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

            await _azureStorage.UploadAgencyFileAsync(fileToUpload, saveAsFileName, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to upload agency file.");
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

            await _azureStorage.UploadAgencyLogoAsync(fileToUpload, saveAsFileName, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to upload agency logo.");
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

            var file = await _azureStorage.DownloadApplicantFileAsync(applicantFileName, cancellationToken: default);
            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);

                if (file.Properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync().Result;

                    Response.Headers.Append("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    return new FileStreamResult(blobStream, file.Properties.ContentType);
                }

                //images
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
            throw new Exception("An error occur while trying to download applicant file.");
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

            var file = await _azureStorage.DownloadApplicantFileAsync(applicantFileName, cancellationToken: default);
            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);

                if (file.Properties.ContentType == "application/pdf")
                {
                    Stream blobStream = file.OpenReadAsync().Result;

                    Response.Headers.Append("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    return new FileStreamResult(blobStream, file.Properties.ContentType);
                }

                //images
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

            var file = await _azureStorage.DownloadAgencyFileAsync(agencyFileName, cancellationToken: default);
            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;

                if (file.Properties.ContentType == "application/pdf")
                {
                    Response.Headers.Append("Content-Disposition", "inline");
                    Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                return new FileStreamResult(blobStream, file.Properties.ContentType);
            }

            return Content("Image does not exist");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to download agency file.");
        }
    }


    [HttpGet("downloadAgencyLogo", Name = "downloadAgencyLogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<string> DownloadAgencyLogo(
        string agencyLogoName,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _azureStorage.DownloadAgencyLogoAsync(agencyLogoName, cancellationToken: default);

            return result;
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to download agency logo.");
        }
    }


    //[Authorize(Policy = "RequireSystemAdminOnly")]
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
            throw new Exception("An error occur while trying to delete agency logo.");
        }
    }


    //[Authorize(Policy = "RequireAdminOnly")]
    [Authorize(Policy = "AADUsers")]
    [HttpDelete("deleteApplicantFile", Name = "deleteApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteApplicantFile(
        string applicantFileName, CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.DeleteApplicantFileAsync(applicantFileName, cancellationToken: default);

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