using CCW.Application.Services.Contracts;
using CCW.Common.Enums;
using CCW.Common.Models;
using iText.Forms;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Rectangle = iText.Kernel.Geom.Rectangle;

namespace CCW.Application.Services;

public class PdfService : IPdfService
{
    private readonly IDocumentAzureStorage _documentService;
    private readonly IUserProfileCosmosDbService _userProfileCosmosDbService;
    private readonly IApplicationCosmosDbService _applicationCosmosDbService;
    private readonly IAdminCosmosDbService _adminCosmosDbService;

    public PdfService(
        IDocumentAzureStorage documentService,
        IApplicationCosmosDbService applicationCosmosDbService,
        IUserProfileCosmosDbService userProfileCosmosDbService,
        IAdminCosmosDbService adminCosmosDbService)
    {
        _documentService = documentService;
        _userProfileCosmosDbService = userProfileCosmosDbService;
        _applicationCosmosDbService = applicationCosmosDbService;
        _adminCosmosDbService = adminCosmosDbService;
    }

    public async Task<MemoryStream> GetApplicationMemoryStream(PermitApplication userApplication, string licensingUserName, string fileName)
    {
        string applicationType = userApplication.Application.ApplicationType;

        if (string.IsNullOrEmpty(applicationType))
        {
            throw new ArgumentNullException("ApplicationType");
        }

        var applicationTemplateStream = await _documentService.GetApplicationTemplateAsync(cancellationToken: default);
        var adminUserProfile = await _userProfileCosmosDbService.GetAdminUserProfileAsync(licensingUserName, cancellationToken: default);

        MemoryStream outStream = new MemoryStream();

        PdfReader pdfReader = new PdfReader(applicationTemplateStream);
        PdfWriter pdfWriter = new PdfWriter(outStream);
        PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);

        Document mainDocument = new Document(pdfDoc);
        pdfWriter.SetCloseStream(false);

        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
        form.SetGenerateAppearance(true);
        var issueDate = string.Empty;
        var expDate = string.Empty;

        if (userApplication.Application.License != null && !string.IsNullOrEmpty(userApplication.Application.License.IssueDate))
        {
            issueDate = userApplication.Application.License.IssueDate;
            expDate = userApplication.Application.License.ExpirationDate;
        }
        else
        {
            issueDate = DateTime.Now.ToString("MM/dd/yyyy");

            switch (applicationType)
            {
                case "reserve":
                case "renew-reserve":
                    expDate = DateTime.Now.AddYears(4).ToString("MM/dd/yyyy");
                    break;
                case "judge":
                case "renew-judge":
                    expDate = DateTime.Now.AddYears(3).ToString("MM/dd/yyyy");
                    break;
                default:
                    expDate = DateTime.Now.AddYears(2).ToString("MM/dd/yyyy");
                    break;
            }

            //save to db issue date and expiration date
            History[] history = new[]{
                    new History
                    {
                        ChangeMadeBy =  licensingUserName,
                        Change = "Record license issue date and expiration date.",
                        ChangeDateTimeUtc = DateTime.UtcNow,
                    }
            };

            userApplication.History = history;
            userApplication.Application.License.IssueDate = issueDate;
            userApplication.Application.License.ExpirationDate = expDate;

            await _applicationCosmosDbService.UpdateUserApplicationAsync(userApplication, cancellationToken: default);
        }

        await AddProcessorsSignatureImageForApplication(licensingUserName, mainDocument);
        await AddApplicantSignatureImageForApplication(userApplication, mainDocument);

        string applicantFullName = BuildApplicantFullName(userApplication);
        string digitallySigned = $"DIGITALLY SIGNED BY: {applicantFullName}, ON {DateTime.Now.ToString("MM/dd/yyyy")}";
        /*form.GetField("form1[0].#subform[16].SIGNATURE[0]").SetValue(digitallySigned, true);
        form.GetField("form1[0].#subform[16].SIGNATURE[1]").SetValue(digitallySigned, true);
        form.GetField("form1[0].#subform[16].SIGNATURE[1]").SetValue(digitallySigned, true);

        form.GetField("form1[0].#subform[16].BADGE_NUMBER[0]").SetValue(adminUserProfile.BadgeNumber, true);
        form.GetField("form1[0].#subform[16].BADGE_NUMBER[1]").SetValue(adminUserProfile.BadgeNumber, true);

        form.GetField("form1[0].#subform[16].DATE[6]").SetValue(issueDate, true);
        form.GetField("form1[0].#subform[16].DATE[7]").SetValue(issueDate, true);*/

        switch (applicationType)
        {
            case "reserve":
            case "renew-reserve":
                form.GetField("form1[0].#subform[3].RESERVE_OFFICER[0]").SetValue("true", true);
                break;
            case "judge":
            case "renew-judge":
                form.GetField("form1[0].#subform[3].JUDGE[0]").SetValue("true", true);
                break;
            default:
                form.GetField("form1[0].#subform[3].STANDARD[0]").SetValue("true", true);
                break;
        }

        switch (applicationType)
        {
            case "renew-reserve":
            case "renew-judge":
                form.GetField("form1[0].#subform[3].RENEWAL_APP[0]").SetValue("true", true);
                break;
            default:
                form.GetField("form1[0].#subform[3].INITIAL_APP[0]").SetValue("true", true);
                break;
        }

        var personalInfo = userApplication.Application.PersonalInfo;
        if (personalInfo == null)
        {
            throw new ArgumentNullException("PersonalInfo");
        }

        //Applicant Personal Information
        form.GetField("form1[0].#subform[3].APP_LAST_NAME[0]").SetValue(personalInfo?.LastName ?? "", true);
        form.GetField("form1[0].#subform[3].APP_FIRST_NAME[0]").SetValue(personalInfo?.FirstName ?? "", true);
        form.GetField("form1[0].#subform[3].APP_MIDDLE_NAME[0]").SetValue(personalInfo?.MiddleName ?? "", true);

        string maidenAndAliases = string.Empty;
        if (!string.IsNullOrWhiteSpace(personalInfo?.MaidenName))
        {
            maidenAndAliases += personalInfo?.MaidenName;
        }

        if (userApplication.Application.Aliases?.Length > 0)
        {
            var aliases = " Aliases: ";
            foreach (var item in userApplication.Application.Aliases)
            {
                aliases += item.PrevLastName + " " + item.PrevFirstName + "; ";
            }

            maidenAndAliases += aliases;
        }
        form.GetField("form1[0].#subform[3].APP_MAIDEN_NAME[0]").SetValue(maidenAndAliases, true);

        form.GetField("form1[0].#subform[3].CA_DRIVER_LICENSE_ID[0]").SetValue(userApplication.Application.IdInfo.IdNumber, true);
        form.GetField("form1[0].#subform[3].APP_CITIZENSHIP[0]").SetValue(userApplication.Application.ImmigrantInformation?.CountryOfCitizenship ?? "", true);

        form.GetField("form1[0].#subform[3].RESIDENCE_Address[0]").SetValue(userApplication.Application.CurrentAddress?.AddressLine1 ?? "", true);
        form.GetField("form1[0].#subform[3].APP_City[0]").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
        form.GetField("form1[0].#subform[3].APP_State[0]").SetValue(userApplication.Application.CurrentAddress?.State ?? "", true);
        form.GetField("form1[0].#subform[3].APP_ZipCode[0]").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
        form.GetField("form1[0].#subform[3].APP_DAY_PhoneNum[0]").SetValue(userApplication.Application.Contact.PrimaryPhoneNumber ?? "", true);

        form.GetField("form1[0].#subform[3].APP_DOB[0]").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);

        DateTime birthDate = DateTime.Parse(userApplication.Application.DOB.BirthDate);
        int age = DateTime.Today.Year - birthDate.Year;
        if (birthDate > DateTime.Today.AddYears(-age))
        {
            age--;
        }
        string ageString = age.ToString();

        form.GetField("form1[0].#subform[3].AGE[0]").SetValue(ageString, true);

        form.GetField("form1[0].#subform[3].APP_OCC[0]").SetValue(userApplication.Application.WorkInformation.Occupation ?? "", true);
        form.GetField("form1[0].#subform[3].EMPOYER_NAME[0]").SetValue(userApplication.Application.WorkInformation.EmployerName ?? "", true);
        form.GetField("form1[0].#subform[3].CURRENT_EMP_Address[0]").SetValue(userApplication.Application.WorkInformation.EmployerAddressLine1 + " " + userApplication.Application.WorkInformation.EmployerAddressLine2 ?? "", true);
        form.GetField("form1[0].#subform[3].CURRENT_EMP_City[0]").SetValue(userApplication.Application.WorkInformation.EmployerCity ?? "", true);
        form.GetField("form1[0].#subform[3].CURRENT_EMPLOYER_State[0]").SetValue(userApplication.Application.WorkInformation.EmployerState ?? "", true);
        form.GetField("form1[0].#subform[3].CURRENT_EMPLOYER_ZipCode[0]").SetValue(userApplication.Application.WorkInformation.EmployerZip ?? "", true);
        form.GetField("form1[0].#subform[3].CURRENT_EMPLOYER_PhoneNum[0]").SetValue(userApplication.Application.WorkInformation.EmployerPhone ?? "", true);

        var birthPlace = string.Empty;
        if (!string.IsNullOrEmpty(userApplication.Application.DOB?.BirthCity))
        {
            birthPlace = userApplication.Application.DOB?.BirthCity + "  " +
                         userApplication.Application.DOB?.BirthState + "  " +
                         userApplication.Application.DOB?.BirthCountry;
        }
        form.GetField("form1[0].#subform[3].APP_BIRTH_PLACE[0]").SetValue(birthPlace, true);

        string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "ft" + " " +
                        userApplication.Application.PhysicalAppearance?.HeightInch + "in";
        form.GetField("form1[0].#subform[3].APP_HEIGHT[0]").SetValue(height, true);
        form.GetField("form1[0].#subform[3].APP_LBS[0]").SetValue(userApplication.Application.PhysicalAppearance?.Weight + "lbs" ?? "", true);
        form.GetField("form1[0].#subform[3].APP_EYE_CLR[0]").SetValue(userApplication.Application.PhysicalAppearance?.EyeColor ?? "", true);
        form.GetField("form1[0].#subform[3].APP_HAIR_CLR[0]").SetValue(userApplication.Application.PhysicalAppearance?.HairColor ?? "", true);
        string gender = userApplication.Application.PhysicalAppearance?.Gender.First().ToString().ToUpper() ?? "";

#if DEBUG
        foreach (var key in form.GetFormFields().Keys)
        {
            Console.WriteLine(key);
        }
#endif
        var qualifyingQuestions = userApplication.Application.QualifyingQuestions;
        if (qualifyingQuestions == null)
        {
            throw new ArgumentNullException("QualifyingQuestions");
        }

        string questionYesNo = (bool)qualifyingQuestions.QuestionOne.Selected ? "0" : "1";
        if ((bool)qualifyingQuestions.QuestionOne.Selected)
        {
            form.GetField("form1[0].#subform[4].YES[0]").SetValue("0", true);
        }
        else
        {
            form.GetField("form1[0].#subform[4].NO[0]").SetValue("1", true);
        }

        if ((bool)qualifyingQuestions.QuestionOne.Selected)
        {
            form.GetField("form1[0].#subform[4].ISSUING_AGENCY[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionOne.Agency, true);
            form.GetField("form1[0].#subform[4].ISSUE_DATE[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionOne.IssueDate, true);
            form.GetField("form1[0].#subform[4].CCW_NO[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionOne.Number, true);
            form.GetField("form1[0].#subform[4].ISSUING_STATE[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionOne.IssuingState, true);
        }

        questionYesNo = (bool)qualifyingQuestions.QuestionTwo.Selected ? "0" : "1";
        if ((bool)qualifyingQuestions.QuestionTwo.Selected)
        {
            form.GetField("form1[0].#subform[4].YES[1]").SetValue("0", true);
        }
        else
        {
            form.GetField("form1[0].#subform[4].NO[1]").SetValue("1", true);
        }
        if (questionYesNo == "0")
        {
            form.GetField("form1[0].#subform[4].AGENCY_NAME[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionTwo.Agency, true);
            form.GetField("form1[0].#subform[4].DATE[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionTwo.DenialDate, true);
            form.GetField("form1[0].#subform[4].DENIAL_REASON[0]").SetValue(userApplication.Application.QualifyingQuestions.QuestionTwo.DenialReason, true);
        }

        if ((bool)qualifyingQuestions.QuestionThree.Selected)
        {
            form.GetField("form1[0].#subform[4].YES[2]").SetValue("0", true);
            form.GetField("form1[0].#subform[4].US_CITIZENSHIP[1]").SetValue(qualifyingQuestions?.QuestionThree.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[4].NO[2]").SetValue("1", true);
        }

        if ((bool)qualifyingQuestions.QuestionFour.Selected)
        {
            form.GetField("form1[0].#subform[4].YES[3]").SetValue("0", true);
            form.GetField("form1[0].#subform[4].CRIMINAL_OFFENSE_CIVORMILIARTY[0]").SetValue(qualifyingQuestions?.QuestionFour.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[4].NO[3]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionFive.Selected)
        {
            form.GetField("form1[0].#subform[4].YES[4]").SetValue("0", true);
            form.GetField("form1[0].#subform[4].ARRESTED_IN_US[1]").SetValue(qualifyingQuestions?.QuestionFive.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[4].NO[4]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionSix.Selected)
        {
            form.GetField("form1[0].#subform[4].YES[5]").SetValue("0", true);
            form.GetField("form1[0].#subform[4].PROBATION_OR_PAROLE[1]").SetValue(qualifyingQuestions?.QuestionSix.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[4].NO[5]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionSeven.Selected)
        {
            form.GetField("form1[0].#subform[4].YES[6]").SetValue("0", true);
            form.GetField("form1[0].#subform[4].PARTY_TO_LAWSUIT[1]").SetValue(qualifyingQuestions?.QuestionSeven.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[4].NO[6]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionEight.Selected)
        {
            form.GetField("form1[0].#subform[5].YES[7]").SetValue("0", true);
            form.GetField("form1[0].#subform[5].SERVED_ARMED_FORCES[1]").SetValue(qualifyingQuestions?.QuestionEight.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[5].NO[7]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionNine.Selected)
        {
            form.GetField("form1[0].#subform[5].YES[8]").SetValue("0", true);
            form.GetField("form1[0].#subform[5].TRO[1]").SetValue(qualifyingQuestions?.QuestionNine.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[5].NO[8]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionTen.Selected)
        {
            form.GetField("form1[0].#subform[5].YES[9]").SetValue("0", true);
            form.GetField("form1[0].#subform[5].TRO_DOMESTIC[0]").SetValue(qualifyingQuestions?.QuestionTen.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[5].NO[9]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionEleven.Selected)
        {
            form.GetField("form1[0].#subform[5].YES[10]").SetValue("0", true);
            form.GetField("form1[0].#subform[5].TRO_DOMESTIC_COURT[0]").SetValue(qualifyingQuestions?.QuestionEleven.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[5].NO[10]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionTwelve.Selected)
        {
            for (int i = 0; i < qualifyingQuestions.QuestionTwelve.TrafficViolations.Count && i <= 4; i++)
            {
                form.GetField($"form1[0].#subform[5].DATE[{i + 1}]").SetValue(qualifyingQuestions.QuestionTwelve.TrafficViolations[i].Date, true);
                form.GetField($"form1[0].#subform[5].VIOLATION[{i}]").SetValue(qualifyingQuestions.QuestionTwelve.TrafficViolations[i].Violation, true);
                form.GetField($"form1[0].#subform[5].AGENCY[{i}]").SetValue(qualifyingQuestions.QuestionTwelve.TrafficViolations[i].Agency, true);
                form.GetField($"form1[0].#subform[5].CITATION_NO[{i}]").SetValue(qualifyingQuestions.QuestionTwelve.TrafficViolations[i].CitationNumber, true);
            }

            if (qualifyingQuestions.QuestionTwelve.TrafficViolations.Count > 5)
            {
                StringBuilder stringBuilder = new();

                for (int i = 5; i < qualifyingQuestions.QuestionTwelve.TrafficViolations.Count; i++)
                {
                    var violation = qualifyingQuestions.QuestionTwelve.TrafficViolations[i];
                    stringBuilder.AppendLine($"{violation.Date}\t{violation.Violation}\t{violation.Agency}\t{violation.CitationNumber}");
                }

                AddAppendixPage("Appendix A: Additional Moving Violations", stringBuilder.ToString(), form, pdfDoc, true);
            }
        }
        if ((bool)qualifyingQuestions.QuestionThirteen.Selected)
        {
            form.GetField("form1[0].#subform[6].YES[11]").SetValue("0", true);
            form.GetField("form1[0].#subform[6].WIC_MENTAL[1]").SetValue(qualifyingQuestions?.QuestionThirteen.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[6].NO[11]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionFourteen.Selected)
        {
            form.GetField("form1[0].#subform[6].YES[12]").SetValue("0", true);
            form.GetField("form1[0].#subform[6].MENTAL_FACILITY[1]").SetValue(qualifyingQuestions?.QuestionFourteen.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[6].NO[12]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionFifteen.Selected)
        {
            form.GetField("form1[0].#subform[6].YES[13]").SetValue("0", true);
            form.GetField("form1[0].#subform[6].REASON_OF_INSANITY[0]").SetValue(qualifyingQuestions?.QuestionFifteen.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[6].NO[13]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionSixteen.Selected)
        {
            form.GetField("form1[0].#subform[6].YES[14]").SetValue("0", true);
            form.GetField("form1[0].#subform[6].ADDICTION[1]").SetValue(qualifyingQuestions?.QuestionSixteen.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[6].NO[14]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionSeventeen.Selected)
        {
            form.GetField("form1[0].#subform[6].YES[15]").SetValue("0", true);
            form.GetField("form1[0].#subform[6].BRANDISHING_OF_FIREARM[1]").SetValue(qualifyingQuestions?.QuestionSeventeen.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[6].NO[15]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionEighteen.Selected)
        {
            form.GetField("form1[0].#subform[6].YES[16]").SetValue("0", true);
            form.GetField("form1[0].#subform[6].FIREARMS_INCIDENT[1]").SetValue(qualifyingQuestions?.QuestionEighteen.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[6].NO[16]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionNineteen.Selected)
        {
            form.GetField("form1[0].#subform[6].YES[17]").SetValue("0", true);
            form.GetField("form1[0].#subform[6].DV[1]").SetValue(qualifyingQuestions?.QuestionNineteen.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[6].NO[17]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionTwenty.Selected)
        {
            form.GetField("form1[0].#subform[7].YES[18]").SetValue("0", true);
            form.GetField("form1[0].#subform[7].WITHHELD_FACTS[1]").SetValue(qualifyingQuestions?.QuestionTwenty.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[7].NO[18]").SetValue("1", true);
        }
        if ((bool)qualifyingQuestions.QuestionTwentyOne.Selected)
        {
            form.GetField("form1[0].#subform[7].YES[19]").SetValue("0", true);
            form.GetField("form1[0].#subform[7].FIREARMS_INCIDENT[1]").SetValue(qualifyingQuestions?.QuestionTwentyOne.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[7].NO[19]").SetValue("1", true);
        }

        /*
                questionYesNo = (bool)qualifyingQuestions.QuestionFive.Selected ? "0" : "1";
                form.GetField("form1[0].#subform[3].PARTY_TO_LAWSUIT[1]").SetValue(questionYesNo, true);
                if ((bool)qualifyingQuestions.QuestionFive.Selected)
                {
                    form.GetField("form1[0].#subform[3].PARTY_TO_LAWSUIT[2]").SetValue(qualifyingQuestions?.QuestionFive.Explanation, true);
                }

                questionYesNo = (bool)qualifyingQuestions.QuestionSix.Selected ? "0" : "1";
                form.GetField("form1[0].#subform[3].RESTRAINING_ORDER[1]").SetValue(questionYesNo, true);
                if ((bool)qualifyingQuestions.QuestionSix.Selected)
                {
                    form.GetField("form1[0].#subform[3].RESTRAINING_ORDER[2]").SetValue(qualifyingQuestions?.QuestionSix.Explanation, true);
                }

                questionYesNo = (bool)qualifyingQuestions.QuestionSeven.Selected ? "0" : "1";
                if ((bool)qualifyingQuestions.QuestionSeven.Selected)
                {
                    form.GetField("form1[0].#subform[3].PROBATION[1]").SetValue("0", true);
                    form.GetField("form1[0].#subform[3].PROBATION[2]").SetValue(qualifyingQuestions?.QuestionSeven.Explanation, true);
                }
                else
                {
                    form.GetField("form1[0].#subform[3].PROBATION[1]").SetValue("1", true);
                }

                if ((bool)qualifyingQuestions.QuestionEight.Selected)
                {
                    for (int i = 0; i < qualifyingQuestions.QuestionTwelve.TrafficViolations.Count && i <= 4; i++)
                    {
                        form.GetField($"form1[0].#subform[3].DATE[{i + 2}]").SetValue(qualifyingQuestions.QuestionTwelve.TrafficViolations[i].Date, true);
                        form.GetField($"form1[0].#subform[3].VIOLATION[{i}]").SetValue(qualifyingQuestions.QuestionTwelve.TrafficViolations[i].Violation, true);
                        form.GetField($"form1[0].#subform[3].AGENCY[{i}]").SetValue(qualifyingQuestions.QuestionTwelve.TrafficViolations[i].Agency, true);
                        form.GetField($"form1[0].#subform[3].CITATION_NO[{i}]").SetValue(qualifyingQuestions.QuestionTwelve.TrafficViolations[i].CitationNumber, true);
                    }

                    if (qualifyingQuestions.QuestionTwelve.TrafficViolations.Count > 5)
                    {
                        StringBuilder stringBuilder = new();

                        for (int i = 5; i < qualifyingQuestions.QuestionTwelve.TrafficViolations.Count; i++)
                        {
                            var violation = qualifyingQuestions.QuestionTwelve.TrafficViolations[i];
                            stringBuilder.AppendLine($"{violation.Date}\t{violation.Violation}\t{violation.Agency}\t{violation.CitationNumber}");
                        }

                        AddAppendixPage("Appendix A: Additional Moving Violations", stringBuilder.ToString(), form, pdfDoc, true);
                    }
                }

                questionYesNo = (bool)qualifyingQuestions.QuestionNine.Selected ? "0" : "1";
                form.GetField("form1[0].#subform[3].CONVICTION[1]").SetValue(questionYesNo, true);
                if ((bool)qualifyingQuestions.QuestionNine.Selected)
                {
                    form.GetField("form1[0].#subform[3].CONVICTION[2]").SetValue(qualifyingQuestions.QuestionNine.Explanation, true);
                }

                questionYesNo = (bool)qualifyingQuestions.QuestionTen.Selected ? "0" : "1";
                form.GetField("form1[0].#subform[3].WITHELD_INFO[0]").SetValue(questionYesNo, true);
                if ((bool)qualifyingQuestions.QuestionTen.Selected)
                {
                    form.GetField("form1[0].#subform[3].WITHHELD_INFO[1]").SetValue(qualifyingQuestions?.QuestionTen.Explanation ?? "", true);
                }

                //Description of Weapons
                var weapons = userApplication.Application.Weapons;
                if (null != weapons && weapons.Length > 0)
                {
                    int totalWeapons = (weapons.Length > 3) ? 3 : weapons.Length;

                    for (int i = 0; i < totalWeapons; i++)
                    {
                        form.GetField("form1[0].#subform[4].MAKE[" + i + "]").SetValue(weapons[i].Make, true);
                        form.GetField("form1[0].#subform[4].MODEL[" + i + "]").SetValue(weapons[i].Model, true);
                        form.GetField("form1[0].#subform[4].CALIBER[" + i + "]").SetValue(weapons[i].Caliber, true);
                        form.GetField("form1[0].#subform[4].SERIAL_NUMBER[" + i + "]").SetValue(weapons[i].SerialNumber, true);
                    }

                    // NOTE: LM: Add additional page(s) for extra weapons
                    if (weapons.Length > 3)
                    {
                        StringBuilder weaponsSb = new StringBuilder();
                        int currentSetCount = 0;
                        int currentWeaponCounter = 3;
                        bool isContinuation = false;

                        totalWeapons = weapons.Length;
                        while (currentWeaponCounter < totalWeapons)
                        {
                            var weapon = weapons[currentWeaponCounter++];
                            weaponsSb.AppendLine($"{weapon.Make}\t{weapon.Model}\t{weapon.Caliber}\t{weapon.SerialNumber}");
                            currentSetCount++;

                            if (currentSetCount >= 30)
                            {
                                currentSetCount = 0;
                                string headerText = "Appendix B: Additional Weapons" + (isContinuation ? " - Continued" : "");
                                AddAppendixPage(headerText, weaponsSb.ToString(), form, pdfDoc, true);
                                isContinuation = true;
                                weaponsSb.Clear();
                            }
                        }

                        if (weaponsSb.Length > 0)
                        {
                            string headerText = "Appendix B: Additional Weapons" + (isContinuation ? " - Continued" : "");
                            AddAppendixPage(headerText, weaponsSb.ToString(), form, pdfDoc, true);
                        }
                    }
                }*/


        //Description of previous addresses
        var previousAddresses = userApplication.Application.PreviousAddresses;

        if (previousAddresses != null && previousAddresses?.Length > 0)
        {
            int totalAddresses = (previousAddresses.Length > 4) ? 4 : previousAddresses.Length;

            for (int i = 0; i < totalAddresses; i++)
            {
                int index = i + 1;
                string address = previousAddresses[i].AddressLine1 + " " + previousAddresses[i].AddressLine2;
                form.GetField("form1[0].#subform[3].APP_Address[" + (index - 1) + "]").SetValue(address, true);
                form.GetField("form1[0].#subform[3].APP_City[" + index + "]").SetValue(previousAddresses[i].City, true);
                form.GetField("form1[0].#subform[3].APP_State[" + index + "]").SetValue(GetStateByName(previousAddresses[i].State), true);
                form.GetField("form1[0].#subform[3].APP_ZipCode[" + index + "]").SetValue(previousAddresses[i].Zip, true);
            }

            // NOTE: LM: Add additional page(s) for extra addresses
            if (previousAddresses.Length > 3)
            {
                StringBuilder addressesSb = new StringBuilder();
                int currentSetCount = 0;
                int currentAddressCounter = 3;
                bool isContinuation = false;

                totalAddresses = previousAddresses.Length;
                while (currentAddressCounter < totalAddresses)
                {
                    var previousAddress = previousAddresses[currentAddressCounter++];

                    string address = previousAddress.AddressLine1 + " " + previousAddress.AddressLine2;
                    addressesSb.AppendLine($"{address}, {previousAddress.City}, {previousAddress.State} {previousAddress.Zip}");

                    currentSetCount++;
                    if (currentSetCount >= 30)
                    {
                        currentSetCount = 0;
                        string headerText = "Appendix C: Additional Previous Addresses" + (isContinuation ? " - Continued" : "");
                        AddAppendixPage(headerText, addressesSb.ToString(), form, pdfDoc, true);
                        isContinuation = true;
                        addressesSb.Clear();
                    }
                }

                if (addressesSb.Length > 0)
                {
                    string headerText = "Appendix C: Additional Previous Addresses" + (isContinuation ? " - Continued" : "");
                    AddAppendixPage(headerText, addressesSb.ToString(), form, pdfDoc, true);
                }
            }
        }

        /*if ((bool)userApplication.Application.QualifyingQuestions.QuestionEleven.Selected)
        {
            form.GetField("form1[0].#subform[8].MENTAL_FACILITY[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[8].MENTAL_FACILITY[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionEleven.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[8].MENTAL_FACILITY[1]").SetValue("1", true);
        }

        if ((bool)userApplication.Application.QualifyingQuestions.QuestionEight.Selected)
        {
            form.GetField("form1[0].#subform[8].ADDICTION[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[8].ADDICTION[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionEight.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[8].ADDICTION[1]").SetValue("1", true);
        }

        if ((bool)userApplication.Application.QualifyingQuestions.QuestionThirteen.Selected)
        {
            form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionThirteen.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[1]").SetValue("1", true);
        }

        if ((bool)userApplication.Application.QualifyingQuestions.QuestionFourteen.Selected)
        {
            form.GetField("form1[0].#subform[9].DV[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[9].DV[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFourteen.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[9].DV[1]").SetValue("1", true);
        }

        if ((bool)userApplication.Application.QualifyingQuestions.QuestionFifteen.Selected)
        {
            form.GetField("form1[0].#subform[9].FORMAL_CHARGES[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[9].FORMAL_CHARGES[2]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionFifteen.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[9].FORMAL_CHARGES[1]").SetValue("1", true);
        }

        form.GetField("form1[0].#subform[9].GOOD_CAUSE_STATEMENT[0]").SetValue(userApplication.Application.QualifyingQuestions?.QuestionSeventeen.Explanation ?? "", true);*/

        mainDocument.Flush();
        form.FlattenFields();
        mainDocument.Close();

        FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");
        FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

        await _documentService.SaveAdminApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

        byte[] byteInfo = outStream.ToArray();
        outStream.Write(byteInfo, 0, byteInfo.Length);
        outStream.Position = 0;

        return outStream;
    }

    public async Task<MemoryStream> GetRevocationLetterMemoryStream(PermitApplication userApplication, string user, string licensingUser, string reason, string date, string fileName)
    {
        string applicationType = userApplication.Application.ApplicationType;

        if (string.IsNullOrEmpty(applicationType))
        {
            throw new ArgumentNullException("ApplicationType");
        }

        var streamToReadFrom = await _documentService.GetRevocationLetterTemplateAsync(cancellationToken: default);

        var adminResponse = await _adminCosmosDbService.GetAgencyProfileSettingsAsync(cancellationToken: default);
        var adminUserProfile = await _userProfileCosmosDbService.GetAdminUserProfileAsync(licensingUser, cancellationToken: default);

        MemoryStream outStream = new MemoryStream();

        PdfReader pdfReader = new PdfReader(streamToReadFrom);
        PdfWriter pdfWriter = new PdfWriter(outStream);
        PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);

        pdfWriter.SetCloseStream(false);

        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
        form.SetGenerateAppearance(true);

        form.GetField("form1[0].#subform[0].Issuing_LEA[0]").SetValue(adminResponse.AgencyName, true);
        form.GetField("form1[0].#subform[0].ORI_Number[0]").SetValue(adminResponse.ORI, true);
        form.GetField("form1[0].#subform[0].Agency_Mailing_Address[0]").SetValue(adminResponse.AgencyShippingStreetAddress, true);
        form.GetField("form1[0].#subform[0].City[0]").SetValue(adminResponse.AgencyShippingCity, true);
        form.GetField("form1[0].#subform[0].County_Code[0]").SetValue(adminResponse.MailCode, true);
        form.GetField("form1[0].#subform[0].ZIP[0]").SetValue(adminResponse.AgencyShippingZip, true);
        string[] nameParts = user.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string lastName = nameParts[0].Trim();
        string firstName = nameParts[1].Trim();
        form.GetField("form1[0].#subform[0].LastName[0]").SetValue(lastName, true);
        form.GetField("form1[0].#subform[0].FirstName[0]").SetValue(firstName, true);
        form.GetField("form1[0].#subform[0].Job_Title_or_Rank[0]").SetValue(adminUserProfile.JobTitle, true);
        form.GetField("form1[0].#subform[0].Phone_Number[0]").SetValue(adminResponse.AgencyTelephone, true);
        form.GetField("form1[0].#subform[0].Fax_Number[0]").SetValue(adminResponse.AgencyFax, true);
        form.GetField("form1[0].#subform[0].EmailAddress[0]").SetValue(licensingUser, true);
        form.GetField("form1[0].#subform[0].CII_Number[0]").SetValue(userApplication.Application.CiiNumber, true);
        form.GetField("form1[0].#subform[0].Local_Agency_Number[0]").SetValue(userApplication.Application.OrderId, true);
        form.GetField("form1[0].#subform[0].ZIP[1]").SetValue(userApplication.Application.License.IssueDate, true);
        form.GetField("form1[0].#subform[0].ZIP[2]").SetValue(userApplication.Application.License.ExpirationDate, true);
        form.GetField("form1[0].#subform[0].CII_Number[1]").SetValue(userApplication.Application.PersonalInfo.LastName, true);
        form.GetField("form1[0].#subform[0].Local_Agency_Number[1]").SetValue(userApplication.Application.PersonalInfo.FirstName, true);
        form.GetField("form1[0].#subform[0].ZIP[3]").SetValue(userApplication.Application.DOB.BirthDate, true);
        form.GetField("form1[0].#subform[0].Local_Agency_Number[2]").SetValue(userApplication.Application.PersonalInfo.MiddleName ?? "", true);
        form.GetField("form1[0].#subform[0].CII_Number[2]").SetValue(userApplication.Application.CurrentAddress.AddressLine1, true);
        form.GetField("form1[0].#subform[0].Local_Agency_Number[3]").SetValue(userApplication.Application.CurrentAddress.City, true);
        form.GetField("form1[0].#subform[0].Local_Agency_Number[4]").SetValue(userApplication.Application.CurrentAddress.County, true);
        form.GetField("form1[0].#subform[0].Local_Agency_Number[5]").SetValue(userApplication.Application.CurrentAddress.Zip, true);
        switch (userApplication.Application.ApplicationType)
        {
            case "renew-standard":
            case "standard":
                form.GetField("form1[0].#subform[0].CheckBox2[0]").SetValue("Yes", true);
                break;
            case "renew-judicial":
            case "judicial":
                form.GetField("form1[0].#subform[0].CheckBox3[0]").SetValue("Yes", true);
                break;
            case "renew-reserve":
            case "reserve":
                form.GetField("form1[0].#subform[0].CheckBox4[0]").SetValue("Yes", true);
                break;

        }
        //fields for employment and Custodial checkboxes
        //form.GetField("form1[0].#subform[0].CheckBox1[0]").SetValue("Yes", true);
        //form.GetField("form1[0].#subform[0].CheckBox5[0]").SetValue("Yes", true);

        switch (userApplication.Application.Status)
        {
            case ApplicationStatus.Canceled:
                form.GetField("form1[0].#subform[0].CheckBox1[1]").SetValue("Yes", true);
                form.GetField("form1[0].#subform[0].Local_Agency_Number[6]").SetValue(reason, true);
                form.GetField("form1[0].#subform[0].ZIP[4]").SetValue(date, true);
                break;
            case ApplicationStatus.Denied:
                form.GetField("form1[0].#subform[0].CheckBox1[2]").SetValue("Yes", true);
                //70 characters
                form.GetField("form1[0].#subform[0].Local_Agency_Number[7]").SetValue(reason, true);
                form.GetField("form1[0].#subform[0].ZIP[5]").SetValue(date, true);
                break;
            case ApplicationStatus.Revoked:
                form.GetField("form1[0].#subform[0].CheckBox1[3]").SetValue("Yes", true);
                form.GetField("form1[0].#subform[0].Local_Agency_Number[8]").SetValue(reason, true);
                form.GetField("form1[0].#subform[0].ZIP[6]").SetValue(date, true);
                break;

        }

        form.FlattenFields();
        pdfDoc.Close();

        FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");
        FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

        await _documentService.SaveAdminApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

        byte[] byteInfo = outStream.ToArray();
        outStream.Write(byteInfo, 0, byteInfo.Length);
        outStream.Position = 0;

        return outStream;
    }

    public async Task<MemoryStream> GetOfficialLicenseMemoryStream(PermitApplication userApplication, string licensingUser, string fileName)
    {
        string applicationType = userApplication.Application.ApplicationType;

        if (string.IsNullOrEmpty(applicationType))
        {
            throw new ArgumentNullException("ApplicationType");
        }

        var adminResponse = await _adminCosmosDbService.GetAgencyProfileSettingsAsync(cancellationToken: default);
        var streamToReadFrom = await _documentService.GetOfficialLicenseTemplateAsync(cancellationToken: default);

        MemoryStream outStream = new MemoryStream();

        PdfReader pdfReader = new PdfReader(streamToReadFrom);
        PdfWriter pdfWriter = new PdfWriter(outStream);
        PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);

        Document mainDocument = new Document(pdfDoc);
        pdfWriter.SetCloseStream(false);

        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
        form.SetGenerateAppearance(true);

        await AddSheriffSignatureImageForOfficial(userApplication, mainDocument);
        await AddApplicantSignatureImageForOfficial(userApplication, mainDocument);
        await AddApplicantThumbprintImageForOfficial(userApplication, mainDocument);
        await AddApplicantPhotoImageForOfficial(userApplication, mainDocument);

        form.GetField("AGENCY").SetValue(adminResponse.AgencyName ?? "", true);
        form.GetField("ORI").SetValue(adminResponse.ORI ?? "", true);
        form.GetField("CII_NUMBER").SetValue(userApplication.Application.OrderId, true);
        form.GetField("LOCAL_AGENCY_NUMBER").SetValue(adminResponse.LocalAgencyNumber ?? "", true);

        var issueDate = string.Empty;
        var expDate = string.Empty;

        if (userApplication.Application.License != null && !string.IsNullOrEmpty(userApplication.Application.License?.IssueDate))
        {
            issueDate = userApplication.Application.License.IssueDate;
            expDate = userApplication.Application.License.ExpirationDate;
        }
        else
        {
            issueDate = DateTime.Now.ToString("MM/dd/yyyy");

            switch (applicationType)
            {
                case "reserve":
                case "renew-reserve":
                    expDate = DateTime.Now.AddYears(4).ToString("MM/dd/yyyy");
                    break;
                case "judge":
                case "renew-judge":
                    expDate = DateTime.Now.AddYears(3).ToString("MM/dd/yyyy");
                    break;
                default:
                    expDate = DateTime.Now.AddYears(2).ToString("MM/dd/yyyy");
                    break;
            }

            //save to db issue date and expiration date
            History[] history = new[]{
                    new History
                    {
                        ChangeMadeBy =  licensingUser,
                        Change = "Record license issue date and expiration date.",
                        ChangeDateTimeUtc = DateTime.UtcNow,
                    }
                };

            userApplication.History = history;
            userApplication.Application.License.IssueDate = issueDate;
            userApplication.Application.License.ExpirationDate = expDate;

            await _applicationCosmosDbService.UpdateUserApplicationAsync(userApplication, cancellationToken: default);
        }

        switch (applicationType)
        {
            case "reserve":
            case "renew-reserve":
                form.GetField("RESERVE_LICENSE_TYPE_CHECKBOX").SetValue("true", true);
                break;
            case "judge":
            case "renew-judge":
                form.GetField("JUDICIAL_LICENSE_TYPE_CHECKBOX").SetValue("true", true);
                break;
            default:
                form.GetField("STANDARD_LICENSE_TYPE_CHECKBOX").SetValue("true", true);
                break;
        }

        form.GetField("ISSUE_DATE").SetValue(issueDate, true);
        form.GetField("EXPIRATION_DATE").SetValue(expDate, true);

        if (applicationType.Contains("renew"))
        {
            form.GetField("SUBSEQUENT_CHECKBOX").SetValue(expDate, true);
        }
        else
        {
            form.GetField("NEW_PERMIT_CHECKBOX").SetValue(expDate, true);
        }

        //Section A
        string fullName = BuildApplicantFullName(userApplication);

        form.GetField("FULL_NAME").SetValue(fullName, true);

        string residenceAddress = userApplication.Application.CurrentAddress?.AddressLine1 +
                                   userApplication.Application.CurrentAddress?.AddressLine2;
        form.GetField("RESIDENCE_ADDRESS").SetValue(residenceAddress ?? "", true);
        form.GetField("CITY").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
        form.GetField("ZIP").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
        form.GetField("COUNTY").SetValue(userApplication.Application.CurrentAddress?.County ?? "", true);

        string workAddress = userApplication.Application.WorkInformation?.EmployerAddressLine1 + " " +
                             userApplication.Application.WorkInformation?.EmployerAddressLine2 + ", " +
                             userApplication.Application.WorkInformation?.EmployerCity + ", " +
                             GetStateByName(userApplication.Application.WorkInformation?.EmployerState) + " " +
                             userApplication.Application.WorkInformation?.EmployerZip;

        if (workAddress.Replace(" ", "") != ",,")
        {
            form.GetField("BUSINESS_ADDRESS").SetValue(workAddress, true);
        }

        form.GetField("OCCUPATION").SetValue(userApplication.Application.WorkInformation?.Occupation ?? "", true);
        form.GetField("BIRTHDATE").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);
        form.GetField("HEIGHT_FEET").SetValue(userApplication.Application.PhysicalAppearance?.HeightFeet + "'" ?? "", true);
        form.GetField("HEIGHT_INCHES").SetValue(userApplication.Application.PhysicalAppearance?.HeightInch + "\"" ?? "", true);
        form.GetField("WEIGHT").SetValue(userApplication.Application.PhysicalAppearance?.Weight ?? "", true);
        form.GetField("EYE_COLOR").SetValue(GetEyeColor(userApplication.Application.PhysicalAppearance?.EyeColor), true);
        form.GetField("HAIR_COLOR").SetValue(GetHairColor(userApplication.Application.PhysicalAppearance?.HairColor), true);

        var weapons = userApplication.Application.Weapons;
        //Section B
        if (null != weapons && weapons.Length > 0)
        {
            int totalWeapons = (weapons.Length > 3) ? 3 : weapons.Length;

            StringBuilder makeSB = new StringBuilder();
            StringBuilder serialSB = new StringBuilder();
            StringBuilder caliberSB = new StringBuilder();
            StringBuilder modelSB = new StringBuilder();

            for (int i = 0; i < totalWeapons; i++)
            {
                makeSB.AppendLine(weapons[i].Make);
                serialSB.AppendLine(weapons[i].SerialNumber);
                caliberSB.AppendLine(weapons[i].Caliber);
                modelSB.AppendLine(weapons[i].Model);
            }

            form.GetField("WEAPON_MAKE").SetValue(makeSB.ToString(), true);
            form.GetField("WEAPON_SERIAL").SetValue(serialSB.ToString(), true);
            form.GetField("WEAPON_CALIBER").SetValue(caliberSB.ToString(), true);
            form.GetField("WEAPON_MODEL").SetValue(modelSB.ToString(), true);

            if (weapons.Length > 3)
            {
                makeSB = new StringBuilder();
                serialSB = new StringBuilder();
                caliberSB = new StringBuilder();
                modelSB = new StringBuilder();

                totalWeapons = weapons.Length > 42 ? 42 : weapons.Length;

                for (int x = 3; x < totalWeapons; x++)
                {
                    makeSB.AppendLine(weapons[x].Make);
                    serialSB.AppendLine(weapons[x].SerialNumber);
                    caliberSB.AppendLine(weapons[x].Caliber);
                    modelSB.AppendLine(weapons[x].Model);
                }

                form.GetField("ADDITIONAL_WEAPON_MAKE").SetValue(makeSB.ToString(), true);
                form.GetField("ADDITIONAL_WEAPON_SERIAL").SetValue(serialSB.ToString(), true);
                form.GetField("ADDITIONAL_WEAPON_CALIBER").SetValue(caliberSB.ToString(), true);
                form.GetField("ADDITIONAL_WEAPON_MODEL").SetValue(modelSB.ToString(), true);
            }
        }

        //don't have restrictions
        //form.GetField("RESTRICTIONS").SetValue("", true);
        //form.GetField("APPLICANT_THUMBPRINT").SetValue(thumbprint, true);

        // TODO: 
        //form.GetField("ADDITIONAL_WEAPON_MAKE").SetValue("Ofelia Test", true);

        mainDocument.Flush();
        form.FlattenFields();
        mainDocument.Close();

        FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");
        FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

        await _documentService.SaveAdminApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

        byte[] byteInfo = outStream.ToArray();
        outStream.Write(byteInfo, 0, byteInfo.Length);
        outStream.Position = 0;

        return outStream;
    }

    public async Task<MemoryStream> GetUnofficialLicenseMemoryStream(PermitApplication userApplication, string fileName)
    {
        var adminResponse = await _adminCosmosDbService.GetAgencyProfileSettingsAsync(cancellationToken: default);
        var streamToReadFrom = await _documentService.GetUnofficialLicenseTemplateAsync(cancellationToken: default);

        MemoryStream outStream = new MemoryStream();

        PdfReader pdfReader = new PdfReader(streamToReadFrom);
        PdfWriter pdfWriter = new PdfWriter(outStream);
        PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

        Document docFileAll = new Document(doc);
        pdfWriter.SetCloseStream(false);

        PdfAcroForm form = PdfAcroForm.GetAcroForm(doc, true);
        form.SetGenerateAppearance(true);

        await AddApplicantSignatureImageForUnOfficial(userApplication, docFileAll);
        await AddApplicantThumbprintImageForUnOfficial(userApplication, docFileAll);
        await AddApplicantPhotoImageForUnOfficial(userApplication, docFileAll);
        await AddSheriffLogoForUnOfficial(docFileAll);
        await AddSheriffIssuingOfficierSignatureImageForUnOfficial(docFileAll);

        form.GetField("AGENCY_NAME").SetValue(adminResponse.AgencyName ?? "", true);
        form.GetField("AGENCY_ORI").SetValue(adminResponse.ORI ?? "", true);
        form.GetField("LOCAL_AGENCY_NUMBER").SetValue(adminResponse.LocalAgencyNumber ?? "", true);
        string fullname = BuildApplicantFullName(userApplication);
        form.GetField("APPLICANT_NAME").SetValue(fullname.Trim(), true);

        string residenceAddress1 = userApplication.Application.CurrentAddress?.AddressLine1;
        string residenceAddress2 = userApplication.Application.CurrentAddress?.AddressLine2;
        if (residenceAddress2 != null)
        {
            residenceAddress1 = residenceAddress1 + ", " + residenceAddress2;
        }
        form.GetField("APPLICATION_ADDRESS_LINE_1").SetValue(residenceAddress1 ?? "", true);
        string residenceAddress3 = userApplication.Application.CurrentAddress?.City
                                   + ", " + userApplication.Application.CurrentAddress?.State
                                   + " " + userApplication.Application.CurrentAddress?.Zip;
        form.GetField("APPLICATION_ADDRESS_LINE_2").SetValue(residenceAddress3 ?? "", true);
        string licenseType = userApplication.Application.ApplicationType?.ToString();
        licenseType = char.ToUpper(licenseType[0]) + licenseType.Substring(1);
        form.GetField("LICENSE_TYPE").SetValue(licenseType ?? "", true);
        form.GetField("DATE_OF_BIRTH").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);
        form.GetField("ISSUED_DATE").SetValue(userApplication.Application.License?.IssueDate ?? "", true);
        form.GetField("EXPIRED_DATE").SetValue(userApplication.Application.License?.ExpirationDate ?? "", true);

        string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "'" + userApplication.Application.PhysicalAppearance?.HeightInch;
        form.GetField("HEIGHT").SetValue(height ?? "", true);
        form.GetField("WEIGHT").SetValue(userApplication.Application.PhysicalAppearance?.Weight ?? "", true);
        form.GetField("EYE_COLOR").SetValue(userApplication.Application.PhysicalAppearance?.EyeColor ?? "", true);
        form.GetField("HAIR_COLOR").SetValue(userApplication.Application.PhysicalAppearance?.HairColor ?? "", true);

        var weapons = userApplication.Application.Weapons;
        if (null != weapons && weapons.Length > 0)
        {
            int totalWeapons = weapons.Length;
            int fieldIteration = 1;
            string makeField;
            string modelField;
            string serialField;
            string caliberField;

            for (int i = 0; i < totalWeapons && i < 4; i++)
            {
                makeField = "MANUFACTURER" + fieldIteration.ToString();
                modelField = "MODEL" + fieldIteration.ToString();
                serialField = "SERIAL" + fieldIteration.ToString();
                caliberField = "CALIBER" + fieldIteration.ToString();

                form.GetField(makeField).SetValue(weapons[i].Make);
                form.GetField(modelField).SetValue(weapons[i].Model);
                form.GetField(serialField).SetValue(weapons[i].SerialNumber);
                form.GetField(caliberField).SetValue(weapons[i].Caliber);

                fieldIteration++;
            }
        }

        form.GetField("ISSUING_NAME").SetValue(adminResponse.AgencySheriffName ?? "", true);
        form.GetField("INFO_NUMBER").SetValue(adminResponse.AgencyTelephone ?? "", true);
        docFileAll.Flush();
        form.FlattenFields();
        docFileAll.Close();

        FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");
        FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

        await _documentService.SaveAdminApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

        byte[] byteInfo = outStream.ToArray();
        outStream.Write(byteInfo, 0, byteInfo.Length);
        outStream.Position = 0;

        return outStream;
    }

    public async Task<MemoryStream> GetLivescanMemoryStream(PermitApplication userApplication, string fileName)
    {
        var adminResponse = await _adminCosmosDbService.GetAgencyProfileSettingsAsync(cancellationToken: default);
        var streamToReadFrom = await _documentService.GetLiveScanTemplateAsync(cancellationToken: default);

        // Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
        MemoryStream outStream = new MemoryStream();

        PdfReader pdfReader = new PdfReader(streamToReadFrom);
        PdfWriter pdfWriter = new PdfWriter(outStream);
        PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

        Document docFileAll = new Document(doc);
        pdfWriter.SetCloseStream(false);

        PdfAcroForm form = PdfAcroForm.GetAcroForm(doc, true);
        form.SetGenerateAppearance(true);

        await AddApplicantSignatureImageForLiveScan(userApplication, docFileAll);
        var submittedDate = DateTime.Now.ToString("MM/dd/yyyy");
        form.GetField("DATE").SetValue(submittedDate ?? "", true);
        form.GetField("ORI").SetValue(adminResponse.ORI ?? "", true);
        string licenseType = userApplication.Application.ApplicationType?.ToString();
        licenseType = licenseType.ToUpper() + " CCW";
        form.GetField("AUTHORIZED_APPLICANT_TYPE").SetValue(licenseType ?? "", true);
        form.GetField("LICENSE_TYPE").SetValue(licenseType ?? "", true);
        form.GetField("AGENCY_NAME").SetValue(adminResponse.AgencyName ?? "", true);
        form.GetField("AGENCY_MAIL_CODE").SetValue(adminResponse.MailCode ?? "", true);
        form.GetField("AGENCY_ADDRESS_1").SetValue(adminResponse.AgencyShippingStreetAddress ?? "", true);
        form.GetField("AGENCY_CONTACT_NAME").SetValue(adminResponse.ContactName ?? "", true);
        form.GetField("AGENCY_CITY").SetValue(adminResponse.AgencyShippingCity ?? "", true);
        form.GetField("AGENCY_STATE").SetValue(GetStateByName(adminResponse.AgencyShippingState) ?? "", true);
        form.GetField("AGENCY_ZIP").SetValue(adminResponse.AgencyShippingZip ?? "", true);
        form.GetField("AGENCY_CONTACT_NUMBER").SetValue(adminResponse.ContactNumber ?? "", true);
        string fullname = BuildApplicantFullName(userApplication);
        form.GetField("LAST_NAME").SetValue(userApplication.Application.PersonalInfo?.LastName ?? "", true);
        form.GetField("FIRST_NAME").SetValue(userApplication.Application.PersonalInfo?.FirstName ?? "", true);
        if (userApplication.Application.PersonalInfo?.MiddleName != "" && userApplication.Application.PersonalInfo?.MiddleName != null)
        {
            form.GetField("MIDDLE_INITIAL").SetValue(userApplication.Application.PersonalInfo?.MiddleName.Substring(0, 1) ?? "", true);
        }

        form.GetField("SUFFIX").SetValue(userApplication.Application.PersonalInfo?.Suffix ?? "", true);
        if (userApplication.Application.Aliases.Length > 0)
        {
            form.GetField("LAST_NAME_2").SetValue(userApplication.Application.Aliases[0].PrevLastName ?? "", true);
            form.GetField("FIRST_NAME_2").SetValue(userApplication.Application.Aliases[0].PrevFirstName ?? "", true);
            form.GetField("SUFFIX_2").SetValue(userApplication.Application.Aliases[0].PrevSuffix ?? "", true);
        }
        form.GetField("DATE_OF_BIRTH").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);
        if (userApplication.Application.PhysicalAppearance.Gender == "male")
        {
            form.GetField("MALE").SetValue("true");
        }
        else
        {
            form.GetField("FEMALE").SetValue("true");
        }
        form.GetField("DL_NUMBER").SetValue(userApplication.Application.IdInfo.IdNumber ?? "", true);
        string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "'" + userApplication.Application.PhysicalAppearance?.HeightInch;
        form.GetField("HEIGHT").SetValue(height ?? "", true);
        form.GetField("WEIGHT").SetValue(userApplication.Application.PhysicalAppearance.Weight ?? "", true);
        form.GetField("EYE_COLOR").SetValue(userApplication.Application.PhysicalAppearance.EyeColor ?? "", true);
        form.GetField("HAIR_COLOR").SetValue(userApplication.Application.PhysicalAppearance.HairColor ?? "", true);
        form.GetField("AGENCY_BILLING_NUMBER").SetValue(adminResponse.AgencyBillingNumber ?? "", true);
        form.GetField("BIRTH_STATE").SetValue(GetStateByName(userApplication.Application.DOB.BirthState) ?? "", true);
        form.GetField("SSN").SetValue(userApplication.Application.PersonalInfo.Ssn ?? "", true);
        string residenceAddress1 = userApplication.Application.CurrentAddress?.AddressLine1;
        string residenceAddress2 = userApplication.Application.CurrentAddress?.AddressLine2;
        if (residenceAddress2 != null)
        {
            residenceAddress1 = residenceAddress1 + ", " + residenceAddress2;
        }
        form.GetField("ADDRESS_1").SetValue(residenceAddress1 ?? "", true);
        form.GetField("CITY").SetValue(userApplication.Application.CurrentAddress?.City ?? "", true);
        form.GetField("STATE").SetValue(GetStateByName(userApplication.Application.CurrentAddress?.State) ?? "", true);
        form.GetField("ZIP").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
        docFileAll.Flush();
        form.FlattenFields();
        docFileAll.Close();

        FileStreamResult fileStreamResult = new FileStreamResult(outStream, "application/pdf");
        FormFile fileToSave = new FormFile(fileStreamResult.FileStream, 0, outStream.Length, null!, fileName);

        await _documentService.SaveAdminApplicationPdfAsync(fileToSave, fileName, cancellationToken: default);

        byte[] byteInfo = outStream.ToArray();
        outStream.Write(byteInfo, 0, byteInfo.Length);
        outStream.Position = 0;

        return outStream;
    }

    private async Task AddApplicantSignatureImageForApplication(PermitApplication userApplication, Document mainDocument)
    {
        string fullFilename = $"{userApplication.UserId}_{userApplication.Application.PersonalInfo.LastName}_{userApplication.Application.PersonalInfo.FirstName}_signature";
        var imageBinaryData = await _documentService.GetApplicantImageAsync(fullFilename, cancellationToken: default);

        var imageData = ImageDataFactory.Create(imageBinaryData);

        var pageThreePosition = new ImagePosition()
        {
            Page = 3,
            Width = 200,
            Height = 22,
            Left = 175,
            Bottom = 590
        };

        var pageThreeImage = GetImageForImageData(imageData, pageThreePosition);
        mainDocument.Add(pageThreeImage);

        var pageEightPosition = new ImagePosition()
        {
            Page = 8,
            Width = 200,
            Height = 22,
            Left = 175,
            Bottom = 365
        };

        var pageEightImage = GetImageForImageData(imageData, pageEightPosition);
        mainDocument.Add(pageEightImage);

        var pageElevenPosition = new ImagePosition()
        {
            Page = 11,
            Width = 200,
            Height = 22,
            Left = 175,
            Bottom = 535
        };

        var pageElevenImage = GetImageForImageData(imageData, pageElevenPosition);
        mainDocument.Add(pageElevenImage);
    }

    private async Task AddProcessorsSignatureImageForApplication(string processorUserName, Document mainDocument)
    {
        var imageBinaryData = await _documentService.GetProcessorSignatureAsync(processorUserName, cancellationToken: default);

        var imageData = ImageDataFactory.Create(imageBinaryData);

        var pageThreePosition = new ImagePosition()
        {
            Page = 3,
            Width = 200,
            Height = 22,
            Left = 80,
            Bottom = 555
        };

        var pageThreeImage = GetImageForImageData(imageData, pageThreePosition);
        mainDocument.Add(pageThreeImage);

        var pageEightPosition = new ImagePosition()
        {
            Page = 8,
            Width = 200,
            Height = 22,
            Left = 80,
            Bottom = 327
        };

        var pageEightImage = GetImageForImageData(imageData, pageEightPosition);
        mainDocument.Add(pageEightImage);

        var pageElevenPosition = new ImagePosition()
        {
            Page = 11,
            Width = 200,
            Height = 22,
            Left = 80,
            Bottom = 498
        };

        var pageElevenImage = GetImageForImageData(imageData, pageElevenPosition);
        mainDocument.Add(pageElevenImage);
    }

    private iText.Layout.Element.Image GetImageForImageData(ImageData imageData, ImagePosition imagePosition)
    {
        return new iText.Layout.Element.Image(imageData)
            .ScaleToFit(imagePosition.Width, imagePosition.Height)
            .SetFixedPosition(imagePosition.Page, imagePosition.Left, imagePosition.Bottom);
    }

    private static string BuildApplicantFullName(PermitApplication userApplication)
    {
        return (userApplication.Application.PersonalInfo?.FirstName + " " +
                                   userApplication.Application.PersonalInfo?.MiddleName + " " +
                                   userApplication.Application.PersonalInfo?.LastName + " " +
                                   userApplication.Application.PersonalInfo?.Suffix).Trim();
    }

    private void AddAppendixPage(string header, string content, PdfAcroForm form, PdfDocument pdfDoc, bool userBorder = false)
    {
        PdfPage page = pdfDoc.AddNewPage(PageSize.LETTER);

        int x = 25;
        int y = 20;
        int w = 560;
        int h = 750;
        float f = 10f;

        Text headerText = new Text(header + "\n").SetFontSize(14f);
        Text paragraphText = new Text(content);

        // Pick any font from existing fields
        var font = form.GetField("form1[0].#subform[5].VIOLATION[3]").GetFont();

        Paragraph paragraph = new Paragraph();
        paragraph.SetFont(font).SetFontSize(f).SetBorder(new SolidBorder(ColorConstants.BLUE, .2F));
        paragraph.Add(headerText).Add(paragraphText);

        Rectangle rectangle = new Rectangle(x, y, w, h);
        Canvas canvas = new Canvas(page, rectangle);
        if (userBorder)
        {
            canvas.SetBorder(new SolidBorder(ColorConstants.BLUE, .2F));
        }
        canvas.Add(paragraph);
        canvas.Close();
    }

    private static int CalculateAge(DateTime birthDate)
    {
        var birthday = new DateTime(birthDate.Year, birthDate.Month, birthDate.Day);
        int age = (int)((DateTime.Now - birthday).TotalDays / 365.242199);

        return age;
    }

    private static string FormatSSN(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value.Length == 9)
        {
            Regex regex = new Regex(@"[^\d]");
            value = regex.Replace(value, "");
            value = Regex.Replace(value, @"(\d{3})(\d{2})(\d{4})", "$1-$2-$3");
            return value;
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

    private static string FormatPhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return string.Empty;
        }

        if (phone.Length == 10)
        {
            Regex regex = new Regex(@"[^\d]");
            phone = regex.Replace(phone, "");
            phone = Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            return phone;
        }

        return phone;
    }

    private async Task AddSheriffSignatureImageForOfficial(PermitApplication userApplication, Document mainDocument)
    {
        var streamContent = await _documentService.GetSheriffSignatureAsync(cancellationToken: default);

        var memoryStream = new MemoryStream();
        await streamContent.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var imageData = ImageDataFactory.Create(memoryStream.ToArray());

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 180,
            Height = 17,
            Left = 90,
            Bottom = 667
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 180,
            Height = 17,
            Left = 395,
            Bottom = 667
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantSignatureImageForOfficial(PermitApplication userApplication, Document mainDocument)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "signature");
        var imageData = await GetImageData(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 160,
            Height = 20,
            Left = 145,
            Bottom = 465
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 160,
            Height = 20,
            Left = 450,
            Bottom = 465
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantSignatureImageForLiveScan(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "signature");
        var imageData = await GetImageData(signatureFileName);

        var position = new ImagePosition()
        {
            Page = 1,
            Width = 250,
            Height = 30,
            Left = 150,
            Bottom = 280
        };

        var image = GetImageForImageData(imageData, position);
        docFileAll.Add(image);
    }

    private async Task AddApplicantThumbprintImageForOfficial(PermitApplication userApplication, Document mainDocument)
    {
        var thumbprintFileName = BuildApplicantDocumentName(userApplication, "thumbprint");
        var imageData = await GetImageData(thumbprintFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 60,
            Height = 70,
            Left = 35,
            Bottom = 425
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 60,
            Height = 70,
            Left = 340,
            Bottom = 425
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantPhotoImageForOfficial(PermitApplication userApplication, Document mainDocument)
    {
        var portraitFileName = BuildApplicantDocumentName(userApplication, "portrait");
        var imageData = await GetImageData(portraitFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 70,
            Height = 95,
            Left = 127,
            Bottom = 374
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 1,
            Width = 70,
            Height = 95,
            Left = 432,
            Bottom = 374
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantSignatureImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "signature");
        var imageData = await GetImageData(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 185,
            Height = 20,
            Left = 133,
            Bottom = 0
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }

    private async Task AddApplicantThumbprintImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
    {
        var thumbprintFileName = BuildApplicantDocumentName(userApplication, "thumbprint");
        var imageData = await GetImageData(thumbprintFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 2,
            Width = 40,
            Height = 50,
            Left = 181,
            Bottom = 6
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }

    private async Task AddApplicantPhotoImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
    {
        var portraitFileName = BuildApplicantDocumentName(userApplication, "portrait");
        var imageData = await GetImageData(portraitFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 80,
            Height = 70,
            Left = 6,
            Bottom = 50
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }

    private async Task AddSheriffLogoForUnOfficial(Document docFileAll)
    {
        var streamContent = await _documentService.GetSheriffLogoAsync(cancellationToken: default);

        var memoryStream = new MemoryStream();
        await streamContent.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var imageData = ImageDataFactory.Create(memoryStream.ToArray());

        var leftPosition = new ImagePosition()
        {
            Page = 2,
            Width = 50,
            Height = 60,
            Left = 92,
            Bottom = 6
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }

    private async Task AddSheriffIssuingOfficierSignatureImageForUnOfficial(Document docFileAll)
    {
        var streamContent = await _documentService.GetSheriffSignatureAsync(cancellationToken: default);

        var memoryStream = new MemoryStream();
        await streamContent.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var imageData = ImageDataFactory.Create(memoryStream.ToArray());

        var leftPosition = new ImagePosition()
        {
            Page = 2,
            Width = 180,
            Height = 17,
            Left = 2,
            Bottom = 15
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }

    private string BuildApplicantDocumentName(PermitApplication userApplication, string documentName)
    {
        string fullFilename = userApplication.UserId + "_" +
            userApplication.Application.PersonalInfo?.LastName + "_" +
            userApplication.Application.PersonalInfo?.FirstName + "_" + documentName;

        return fullFilename;
    }

    private async Task<ImageData> GetImageData(string fileName)
    {
        var imageBinaryData = await _documentService.GetApplicantImageAsync(fileName, cancellationToken: default);

        return ImageDataFactory.Create(imageBinaryData);
    }

    private static string GetHairColor(string color)
    {
        if (string.IsNullOrEmpty(color))
        {
            return string.Empty;
        }

        switch (color.ToUpper())
        {
            case "BLACK":
                return "BLK";

            case "BLONDE":
                return "BLD";

            case "BROWN":
                return "BRN";

            case "GRAY":
                return "GRY";

            case "LIGHT BROWN":
                return "LBRN";

            case "RED":
                return "RED";

            case "UNNATURAL":
                return "UNNAT";

            default:
                return color;
        }
    }

    private static string GetEyeColor(string color)
    {
        if (string.IsNullOrEmpty(color))
        {
            return string.Empty;
        }

        switch (color.ToUpper())
        {
            case "BLACK":
                return "BLK";

            case "MIXED":
                return "MIX";

            case "BROWN":
                return "BRN";

            case "HAZEL":
                return "HAZ";

            case "BLUE":
                return "BLU";

            case "GREEN":
                return "GRN";

            case "GRAY":
                return "GRY";

            default:
                return color;
        }
    }

    private sealed class ImagePosition
    {
        public int Page { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Bottom { get; set; }
    }
}
