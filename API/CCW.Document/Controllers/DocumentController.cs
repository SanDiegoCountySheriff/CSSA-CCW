using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Azure.Security.KeyVault.Secrets;

namespace CCW.Document.Controllers;

[Route("/Api/" + Constants.AppName + "/v1/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private IConfiguration _configuration;

    private string[] _imageTypes = new[] { "jpeg", "png" };

    public DocumentController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("uploadApplicantFile", Name = "uploadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadApplicantFile(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        var client = new SecretClient(new Uri(_configuration.GetSection("KeyVault:VaultUri").Value),
            credential: new DefaultAzureCredential());
        var storageConnection = client.GetSecret("storage-account-test-conn2").Value.Value;

        string containerName = _configuration.GetSection("Storage").GetSection("PublicContainerName").Value;
        BlobContainerClient container = new BlobContainerClient(storageConnection, containerName);

        try
        {
            //get file type
            var fileType = saveAsFileName.Substring(saveAsFileName.LastIndexOf('.') + 1);

            var contentType = _imageTypes.Contains(fileType) ? "image" : "application";

            // get reference to blob
            BlobClient blob = container.GetBlobClient(saveAsFileName);

            // open the file and upload its data
            using (Stream file = fileToPersist.OpenReadStream())
            {
                blob.Upload(file, new BlobHttpHeaders { ContentType = contentType + "/" + fileType });
            }

            //var uri = blob.Uri.AbsoluteUri;
        }
        catch
        {
            // log error
        }

        return Ok();
    }


    [HttpPost("uploadAgencyFile", Name = "uploadAgencyFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AgencyUpload(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        var client = new SecretClient(new Uri(_configuration.GetSection("KeyVault:VaultUri").Value),
            credential: new DefaultAzureCredential());
        var storageConnection = client.GetSecret("storage-account-test-conn2").Value.Value;

        string containerName = _configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;
        BlobContainerClient container = new BlobContainerClient(storageConnection, containerName);

        try
        {
            //get file type
            var fileType = saveAsFileName.Substring(saveAsFileName.LastIndexOf('.') + 1);

            var contentType = _imageTypes.Contains(fileType) ? "image" : "application";

            // get reference to blob
            BlobClient blob = container.GetBlobClient(saveAsFileName);

            // open the file and upload its data
            using (Stream file = fileToPersist.OpenReadStream())
            {
                blob.Upload(file, new BlobHttpHeaders { ContentType = contentType + "/" + fileType });
            }

            //var uri = blob.Uri.AbsoluteUri;
        }
        catch
        {
            // log error
        }

        return Ok();
    }


    [HttpPost("downloadApplicantFile", Name = "downloadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DownloadApplicantFile(
        string applicantImageName,
        CancellationToken cancellationToken)
    {
        var client = new SecretClient(new Uri(_configuration.GetSection("KeyVault:VaultUri").Value),
            credential: new DefaultAzureCredential());
        var storageConnection = client.GetSecret("storage-account-test-conn2").Value.Value;

        string containerName = _configuration.GetSection("Storage").GetSection("PublicContainerName").Value;

        MemoryStream ms = new MemoryStream();

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);

        CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer c1 = BlobClient.GetContainerReference(containerName);

        if (await c1.ExistsAsync())
        {
            CloudBlob file = c1.GetBlobReference(applicantImageName);

            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
                return File(blobStream, file.Name);
                //return File(blobStream, "application/pdf", file.Name);
                //return File(blobStream, file.Properties.ContentType, file.Name);
            }
            else
            {
                return Content("Image does not exist");
            }
        }
        else
        {
            return Content("Container does not exist");
        }
    }


    [HttpPost("downloadAgencyFile", Name = "downloadAgencyFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DownloadAgencyFile(
        string agencyLogoName,
        CancellationToken cancellationToken)
    {
        var client = new SecretClient(new Uri(_configuration.GetSection("KeyVault:VaultUri").Value),
            credential: new DefaultAzureCredential());
        var storageConnection = client.GetSecret("storage-account-test-conn2").Value.Value;

        string containerName = _configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;

        MemoryStream ms = new MemoryStream();

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);

        CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer c1 = BlobClient.GetContainerReference(containerName);

        if (await c1.ExistsAsync())
        {
            CloudBlob file = c1.GetBlobReference(agencyLogoName);

            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
                return File(blobStream, file.Name);
            }
            else
            {
                return Content("Image does not exist");
            }
        }
        else
        {
            return Content("Container does not exist");
        }
    }
}
