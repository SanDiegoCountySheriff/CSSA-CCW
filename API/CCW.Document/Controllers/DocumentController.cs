using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using Azure.Storage.Blobs.Models;

namespace CCW.Document.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        [HttpPost("uploadImage", Name = "uploadImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage(
         IFormFile fileToPersist,
         string saveAsFileName,
         CancellationToken cancellationToken)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=sdsdtestdoc;AccountKey=WeXIMJVMrWUYV2p4q5gBxIQtPvQ/D6jvk67uN7tlPpoTJx9eu9FROOhITsnKSdaa0IZdQlfpuK91+AStA0u+hA==;EndpointSuffix=core.usgovcloudapi.net";
            string containerName = "ccw-documents";
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
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=sdsdtestdoc;AccountKey=WeXIMJVMrWUYV2p4q5gBxIQtPvQ/D6jvk67uN7tlPpoTJx9eu9FROOhITsnKSdaa0IZdQlfpuK91+AStA0u+hA==;EndpointSuffix=core.usgovcloudapi.net";
            string containerName = "ccw-documents";
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


        //private bool CheckIfExcelFile(IFormFile file)
        //{
        //    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
        //    return (extension == ".xlsx" || extension == ".xls"); // Change the extension based on your need
        //}

        //private async Task<bool> WriteFile(IFormFile file)
        //{
        //    bool isSaveSuccess = false;
        //    string fileName;
        //    try
        //    {
        //        var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
        //        fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

        //        var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

        //        if (!Directory.Exists(pathBuilt))
        //        {
        //            Directory.CreateDirectory(pathBuilt);
        //        }

        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
        //           fileName);

        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        isSaveSuccess = true;
        //    }
        //    catch (Exception e)
        //    {
        //        //log error
        //    }

        //    return isSaveSuccess;
        //}
    }
}
