using Microsoft.AspNetCore.Mvc;
using CCW.Document.Services;


namespace CCW.Document.Controllers;

[Route("/Api/" + Constants.AppName + "/v1/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private IAzureStorage _azureStorage;

    public DocumentController(IAzureStorage azureStorage)
    {
        _azureStorage = azureStorage;
    }

    [HttpPost("uploadApplicantFile", Name = "uploadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadApplicantFile(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        await _azureStorage.UploadApplicantFileAsync(fileToPersist, saveAsFileName, cancellationToken: default);

        return Ok();
    }

    [HttpPost("uploadAgencyLogo", Name = "uploadAgencyLogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadAgencyLogo(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        await _azureStorage.UploadAgencyLogoAsync(fileToPersist, saveAsFileName, cancellationToken: default);

        return Ok();
    }

    [HttpGet("downloadApplicantFile", Name = "downloadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DownloadApplicantFile(
        string applicantFileName,
        CancellationToken cancellationToken)
    {
        MemoryStream ms = new MemoryStream();

        var file = await _azureStorage.DownloadApplicantFileAsync(applicantFileName, cancellationToken: default);
        if (await file.ExistsAsync())
        {
            await file.DownloadToStreamAsync(ms);
            Stream blobStream = file.OpenReadAsync().Result;

            return File(blobStream, file.Properties.ContentType, file.Name);
        }

        return Content("Image does not exist");
    }

    [HttpGet("downloadAgencyLogo", Name = "downloadAgencyLogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<string> DownloadAgencyLogo(
        string agencyLogoName,
        CancellationToken cancellationToken)
    {
       var result =  await _azureStorage.DownloadAgencyLogoAsync(agencyLogoName, cancellationToken: default);

        return result;
    }
}