using System.Collections;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using CCW.Document.Services;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.PortableExecutable;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec;
using iTextSharp.text;

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
    [Authorize(Policy = "AADUsers")]
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
    [Authorize(Policy = "AADUsers")]
    [HttpGet("downloadApplicantFile", Name = "downloadApplicantFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadApplicantFile(
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
            throw new Exception("An error occur while trying to download applicant file.");
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


    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [HttpGet("getFile", Name = "getFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFile(
        string fileName,
        CancellationToken cancellationToken)
    {
        iTextSharp.text.Document document = new iTextSharp.text.Document();

        MemoryStream stream = new MemoryStream();

        try
        {
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, stream);
            pdfWriter.CloseStream = false;

            MemoryStream ms = new MemoryStream();
            fileName = "Template_2022";
            var file = await _azureStorage.DownloadAgencyFileAsync(fileName, cancellationToken: default);
            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
            }

            document.Open();
            document.Add(new Paragraph("Hello World"));
        }
        catch (DocumentException de)
        {
            Console.Error.WriteLine(de.Message);
        }
        catch (IOException ioe)
        {
            Console.Error.WriteLine(ioe.Message);
        }

        document.Close();

        stream.Flush(); //Always catches me out
        stream.Position = 0; //Not sure if this is required

        return File(stream, "application/pdf", "DownloadName.pdf");
        //try
        //{
        //    iTextSharp.text.Document document = new iTextSharp.text.Document();

        //    MemoryStream stream = new MemoryStream();

        //    MemoryStream ms = new MemoryStream();
        //    agencyFileName = "Template_2022";
        //    var file = await _azureStorage.DownloadAgencyFileAsync(agencyFileName, cancellationToken: default);
        //    if (await file.ExistsAsync())
        //    {
        //        await file.DownloadToStreamAsync(ms);
        //        Stream blobStream = file.OpenReadAsync().Result;

        //        PdfWriter pdfWriter = PdfWriter.GetInstance(document, ms);

        //        using (PdfReader pdfReader = new PdfReader(agencyFileName))
        //        {
        //            using (PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(agencyFileName, FileMode.Create)))
        //            {
        //                AcroFields pdfFormFields = pdfStamper.AcroFields;

        //                pdfFormFields.SetField("11", "Porumb");
        //                pdfFormFields.SetField("12", "Ofelia");


        //                //foreach (var field in this.textFields)
        //                //{
        //                //    //Locate the field
        //                //    IList<AcroFields.FieldPosition> fieldLocations = pdfFormFields.GetFieldPositions(field.FieldName);
        //                //    if (field.FieldText != null)
        //                //    {
        //                //        foreach (var location in fieldLocations)
        //                //        {
        //                //            canvas = pdfStamper.GetOverContent(location.page);
        //                //            position = location.position;

        //                //            ct = new ColumnText(canvas);
        //                //            ct.SetSimpleColumn(position);
        //                //            ct.SetLeading(field.FixedLeading, field.MultipliedLeading);
        //                //            ct.SetIndent(field.Indent, false);
        //                //            ct.AddText(new Phrase(field.FieldText));//Insert the value for the field
        //                //            ct.Go();
        //                //        }

        //                //        //Remove the field name so that it cannot be edited.
        //                //        pdfFormFields.RemoveField(field.FieldName);
        //                //    }
        //                //}


        //                pdfStamper.Close();
        //            }
        //        }


        //    //using (FileStream outFile = new FileStream(blobStream, FileMode.Create))
        //    //    {
        //    //        PdfReader pdfReader = new PdfReader(blobStream);
        //    //        PdfStamper pdfStamper = new PdfStamper(pdfReader, outFile);
        //    //        AcroFields fields = pdfStamper.AcroFields;

        //    //        fields.SetField("11", "Porumb");
        //    //        fields.SetField("12", "Ofelia");

        //    //        pdfStamper.FormFlattening = true;


        //            //Response.Headers.Append("Content-Disposition", "inline");
        //            //Response.Headers.Add("X-Content-Type-Options", "nosniff");

        //            return File(ms, "application/pdf", "DownloadName.pdf");
        //      //  }
        //    }

        //    return null!;
        //}
        //catch (Exception e)
        //{
        //    var originalException = e.GetBaseException();
        //    _logger.LogError(originalException, originalException.Message);
        //    throw new Exception("An error occur while trying to download agency file.");
        //}
    }

}