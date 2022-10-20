using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace CCW.Document.Controllers;

[Route(Constants.AppName + "api/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private IConfiguration _configuration;

    public DocumentController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("uploadImage", Name = "uploadImage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadImage(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        string connectionString = _configuration.GetSection("Storage").GetSection("ConnectionString").Value;
        string containerName = _configuration.GetSection("Storage").GetSection("PublicContainerName").Value;
        BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

        try
        {
            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(saveAsFileName);

            // Open the file and upload its data
            using (Stream file = fileToPersist.OpenReadStream())
            {
                blob.Upload(file, new BlobHttpHeaders { ContentType = "image/jpeg" });
            }

            var uri = blob.Uri.AbsoluteUri;
        }
        catch
        {
            // log error
        }

        return Ok();
    }

    [HttpPost("uploadFile", Name = "uploadFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile(
        IFormFile fileToPersist,
        string saveAsFileName,
        CancellationToken cancellationToken)
    {
        string connectionString = _configuration.GetSection("Storage").GetSection("ConnectionString").Value;
        string containerName = _configuration.GetSection("Storage").GetSection("PublicContainerName").Value;
        BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

        try
        {
            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(saveAsFileName);

            // Open the file and upload its data
            using (Stream file = fileToPersist.OpenReadStream())
            {
                blob.Upload(file, new BlobHttpHeaders { ContentType = "application/pdf" });
            }

            var uri = blob.Uri.AbsoluteUri;
        }
        catch
        {
            // log error
        }

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
        string connectionString = _configuration.GetSection("Storage").GetSection("ConnectionString").Value;
        string containerName = _configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;
        BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

        try
        {
            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(saveAsFileName);

            // Open the file and upload its data
            using (Stream file = fileToPersist.OpenReadStream())
            {
                blob.Upload(file, new BlobHttpHeaders { ContentType = "image/png" });
            }

            var uri = blob.Uri.AbsoluteUri;
        }
        catch
        {
            // log error
        }

        return Ok();
    }


    [HttpPost("downloadAgencyLogo", Name = "downloadAgencyLogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DownloadAgencyLogoImage(
        string agencyLogoName,
        CancellationToken cancellationToken)
    {
        string connectionString = _configuration.GetSection("Storage").GetSection("ConnectionString").Value;
        string containerName = _configuration.GetSection("Storage").GetSection("AgencyContainerName").Value;

        MemoryStream ms = new MemoryStream();

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

        CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer c1 = BlobClient.GetContainerReference(containerName);

        if (await c1.ExistsAsync())
        {
            CloudBlob file = c1.GetBlobReference(agencyLogoName);

            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
                return File(blobStream, "image/png", file.Name);
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

    [HttpPost("downloadFile", Name = "downloadFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DownloadFile(
        string fileName,
        CancellationToken cancellationToken)
    {
        string connectionString = _configuration.GetSection("Storage").GetSection("ConnectionString").Value;
        string containerName = _configuration.GetSection("Storage").GetSection("PublicContainerName").Value;

        MemoryStream ms = new MemoryStream();

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

        CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer c1 = BlobClient.GetContainerReference(containerName);

        if (await c1.ExistsAsync())
        {
            CloudBlob file = c1.GetBlobReference(fileName);

            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
                return File(blobStream, "application/pdf", file.Name);
                //return File(blobStream, file.Properties.ContentType, file.Name);
            }
            else
            {
                return Content("File does not exist");
            }
        }
        else
        {
            return Content("Container does not exist");
        }
    }

    [HttpPost("downloadApplicantImage", Name = "downloadApplicantImage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DownloadApplicantImage(
        string applicantImageName,
        CancellationToken cancellationToken)
    {
        string connectionString = _configuration.GetSection("Storage").GetSection("ConnectionString").Value;
        string containerName = _configuration.GetSection("Storage").GetSection("PublicContainerName").Value;

        MemoryStream ms = new MemoryStream();

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

        CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer c1 = BlobClient.GetContainerReference(containerName);

        if (await c1.ExistsAsync())
        {
            CloudBlob file = c1.GetBlobReference(applicantImageName);

            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
                return File(blobStream, "image/jpg", file.Name);
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
}
