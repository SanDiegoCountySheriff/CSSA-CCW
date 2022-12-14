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

    public DocumentController(
        IAzureStorage azureStorage,
        ILogger<DocumentController> logger
        )
    {
        _azureStorage = azureStorage;
        _logger = logger;
    }

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpPost("uploadApplicantFile", Name = "uploadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadApplicantFile(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.UploadApplicantFileAsync(fileToPersist, saveAsFileName, cancellationToken: default);

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
    [HttpPost("uploadAgencyFile", Name = "uploadAgencyFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadAgencyFile(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.UploadAgencyFileAsync(fileToPersist, saveAsFileName, cancellationToken: default);

            return Ok();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to upload agency file.");
        }
    }

    //[Authorize(Policy = "RequireSystemAdminOnly")]
    [Authorize(Policy = "AADUsers")]
    [HttpPost("uploadAgencyLogo", Name = "uploadAgencyLogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadAgencyLogo(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        try
        {
            await _azureStorage.UploadAgencyLogoAsync(fileToPersist, saveAsFileName, cancellationToken: default);

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
    [Authorize(Policy = "AADUsers")]
    [HttpGet("downloadApplicantFile", Name = "downloadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DownloadApplicantFile(
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
                Stream blobStream = file.OpenReadAsync().Result;

                //Response.Headers.Append("Content-Disposition", "inline");
                //Response.Headers.Add("X-Content-Type-Options", "nosniff");

                //return File(blobStream, file.Properties.ContentType);

                return File(blobStream, file.Properties.ContentType, file.Name);
            }

            return Content("Image does not exist");
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to download applicant file.");
        }
    }


    [Authorize(Policy = "AADUsers")]
    [HttpGet("downloadAgencyFile", Name = "downloadAgencyFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DownloadAgencyFile(
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

                return File(blobStream, file.Properties.ContentType, file.Name);
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
}