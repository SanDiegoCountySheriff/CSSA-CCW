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
        if (userApplication.Application.ApplicationType == default(ApplicationType))
        {
            throw new ArgumentNullException(nameof(userApplication.Application.ApplicationType));
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

        await AddProcessorsSignatureImageForApplication(licensingUserName, mainDocument);
        await AddApplicantSignatureImageForApplication(userApplication, mainDocument);

        string applicantFullName = BuildApplicantFullName(userApplication);
        string digitallySigned = $"DIGITALLY SIGNED BY: {applicantFullName}, ON {DateTime.Now.ToString("MM/dd/yyyy")}";
        form.GetField("form1[0].#subform[16].SIGNATURE[0]").SetValue(digitallySigned, true);
        form.GetField("form1[0].#subform[16].SIGNATURE[1]").SetValue(digitallySigned, true);
        form.GetField("form1[0].#subform[16].WITNESS_SIGNATURE[0]").SetValue(digitallySigned, true);
        form.GetField("form1[0].#subform[16].WITNESS_SIGNATURE[1]").SetValue(digitallySigned, true);

        form.GetField("form1[0].#subform[16].BADGE_NUMBER[0]").SetValue(adminUserProfile.BadgeNumber, true);
        form.GetField("form1[0].#subform[16].BADGE_NUMBER[1]").SetValue(adminUserProfile.BadgeNumber, true);

        form.GetField("form1[0].#subform[16].DATE[6]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"), true);
        form.GetField("form1[0].#subform[16].DATE[7]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"), true);
        form.GetField("form1[0].#subform[16].DateTimeField1[0]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"), true);
        form.GetField("form1[0].#subform[16].DateTimeField1[1]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"), true);

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.Reserve:
            case ApplicationType.RenewReserve:
                form.GetField("form1[0].#subform[3].RESERVE_OFFICER[0]").SetValue("true", true);
                break;
            case ApplicationType.Judicial:
            case ApplicationType.RenewJudicial:
                form.GetField("form1[0].#subform[3].JUDGE[0]").SetValue("true", true);
                break;
            case ApplicationType.Employment:
            case ApplicationType.RenewEmployment:
                form.GetField("form1[0].#subform[3].EMPLOYMENT[0]").SetValue("true", true);
                break;
            default:
                form.GetField("form1[0].#subform[3].STANDARD[0]").SetValue("true", true);
                break;
        }

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.RenewReserve:
            case ApplicationType.RenewJudicial:
            case ApplicationType.RenewStandard:
            case ApplicationType.RenewEmployment:
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
        form.GetField("form1[0].#subform[3].APP_LAST_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.LastName) ? personalInfo?.LastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].APP_FIRST_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.FirstName) ? personalInfo?.FirstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].APP_MIDDLE_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.MiddleName) ? personalInfo?.MiddleName.ToUpper() : "", true);

        string currentState = userApplication.Application.CurrentAddress?.State?.Trim() ?? "";
        string mailingState = userApplication.Application.MailingAddress?.State?.Trim() ?? "";
        string spouseState = userApplication.Application.SpouseAddressInformation?.State?.Trim() ?? "";
        string employerState = userApplication.Application.WorkInformation?.EmployerState?.Trim() ?? "";
        string abbreviation;

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
        form.GetField("form1[0].#subform[3].APP_MAIDEN_NAME[0]").SetValue(!string.IsNullOrEmpty(maidenAndAliases) ? maidenAndAliases.ToUpper() : "", true);

        form.GetField("form1[0].#subform[3].CA_DRIVER_LICENSE_ID[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.IdInfo.IdNumber) ? userApplication.Application.IdInfo.IdNumber.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].CA_DRIVER_RESTRICTIONS[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.IdInfo.Restrictions) ? userApplication.Application.IdInfo.Restrictions.ToUpper() : "", true);
        if (userApplication.Application.Citizenship.Citizen)
        {
            form.GetField("form1[0].#subform[3].APP_CITIZENSHIP[0]").SetValue("UNITED STATES", true);
        }
        else
        {
            form.GetField("form1[0].#subform[3].APP_CITIZENSHIP[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.ImmigrantInformation.CountryOfCitizenship) ? userApplication.Application.ImmigrantInformation.CountryOfCitizenship.ToUpper() : "", true);
        }

        form.GetField("form1[0].#subform[3].RESIDENCE_Address[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.StreetAddress) ? userApplication.Application.CurrentAddress?.StreetAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].APP_City[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.City) ? userApplication.Application.CurrentAddress?.City.ToUpper() : "", true);
        if (Constants.StateAbbreviations.TryGetValue(currentState, out abbreviation))
        {
            form.GetField("form1[0].#subform[3].APP_State[0]").SetValue(!string.IsNullOrEmpty(abbreviation) ? abbreviation.ToUpper() : "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[3].APP_State[0]").SetValue(!string.IsNullOrEmpty(currentState) ? currentState.ToUpper() : "", true);
        }
        form.GetField("form1[0].#subform[3].APP_ZipCode[0]").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
        form.GetField("form1[0].#subform[3].APP_DAY_PhoneNum[0]").SetValue(userApplication.Application.Contact.PrimaryPhoneNumber ?? "", true);

        form.GetField("form1[0].#subform[3].APP_DOB[0]").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);

        DateTimeOffset birthDate = DateTimeOffset.Parse(userApplication.Application.DOB.BirthDate);
        int age = DateTime.Today.Year - birthDate.Year;
        if (birthDate > DateTime.Today.AddYears(-age))
        {
            age--;
        }
        string ageString = age.ToString();

        form.GetField("form1[0].#subform[3].AGE[0]").SetValue(ageString, true);

        form.GetField("form1[0].#subform[3].APP_OCC[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation.Occupation) ? userApplication.Application.WorkInformation.Occupation.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].EMPOYER_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation.EmployerName) ? userApplication.Application.WorkInformation.EmployerName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].CURRENT_EMP_Address[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation.EmployerStreetAddress) ? userApplication.Application.WorkInformation.EmployerStreetAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].CURRENT_EMP_City[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation.EmployerCity) ? userApplication.Application.WorkInformation.EmployerCity.ToUpper() : "", true);
        if (Constants.StateAbbreviations.TryGetValue(employerState, out abbreviation))
        {
            form.GetField("form1[0].#subform[3].CURRENT_EMPLOYER_State[0]").SetValue(!string.IsNullOrEmpty(abbreviation) ? abbreviation.ToUpper() : "", true);
        } else
        {
            form.GetField("form1[0].#subform[3].CURRENT_EMPLOYER_State[0]").SetValue(!string.IsNullOrEmpty(employerState) ? employerState.ToUpper() : "");
        }
        form.GetField("form1[0].#subform[3].CURRENT_EMPLOYER_ZipCode[0]").SetValue(userApplication.Application.WorkInformation.EmployerZip ?? "", true);
        form.GetField("form1[0].#subform[3].CURRENT_EMPLOYER_PhoneNum[0]").SetValue(userApplication.Application.WorkInformation.EmployerPhone ?? "", true);

        var birthPlace = string.Empty;
        if (!string.IsNullOrEmpty(userApplication.Application.DOB?.BirthCity))
        {
            birthPlace = userApplication.Application.DOB?.BirthCity + "  " +
                         userApplication.Application.DOB?.BirthState + "  " +
                         userApplication.Application.DOB?.BirthCountry;
        }
        form.GetField("form1[0].#subform[3].APP_BIRTH_PLACE[0]").SetValue(!string.IsNullOrEmpty(birthPlace) ? birthPlace.ToUpper() : "", true);

        string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "ft" + " " +
                        userApplication.Application.PhysicalAppearance?.HeightInch + "in";
        form.GetField("form1[0].#subform[3].APP_HEIGHT[0]").SetValue(!string.IsNullOrEmpty(height) ? height.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].APP_LBS[0]").SetValue(userApplication.Application.PhysicalAppearance?.Weight + "lbs" ?? "", true);
        form.GetField("form1[0].#subform[3].APP_EYE_CLR[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance?.EyeColor) ? userApplication.Application.PhysicalAppearance?.EyeColor.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].APP_HAIR_CLR[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance?.HairColor) ? userApplication.Application.PhysicalAppearance?.HairColor.ToUpper() : "", true);
        string gender = !string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance?.Gender.First().ToString()) ? userApplication.Application.PhysicalAppearance?.Gender.First().ToString().ToUpper() : "";

        form.GetField("form1[0].#subform[3].APP_MAILINGAddress[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.MailingAddress?.StreetAddress) ? userApplication.Application.MailingAddress?.StreetAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].APP_MAILING_City[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.MailingAddress?.City) ? userApplication.Application.MailingAddress?.City.ToUpper() : "", true);
        if (Constants.StateAbbreviations.TryGetValue(mailingState, out abbreviation))
        {
            form.GetField("form1[0].#subform[3].APP_MAILING_State[0]").SetValue(!string.IsNullOrEmpty(abbreviation) ? abbreviation.ToUpper() : "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[3].APP_MAILING_State[0]").SetValue(!string.IsNullOrEmpty(mailingState) ? mailingState.ToUpper() : "", true);
        }
        form.GetField("form1[0].#subform[3].APP_MAILING_Zip[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.MailingAddress?.Zip) ? userApplication.Application.MailingAddress?.Zip.ToUpper() : "", true);

        form.GetField("form1[0].#subform[3].SPOUSE_LAST_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseInformation?.LastName) ? userApplication.Application.SpouseInformation?.LastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].SPOUSE_FIRST_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseInformation?.FirstName) ? userApplication.Application.SpouseInformation?.FirstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].SPOUSE_MIDDLE_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseInformation?.MiddleName) ? userApplication.Application.SpouseInformation?.MiddleName.ToUpper() : "", true);

        form.GetField("form1[0].#subform[3].SPOUSE_physical_Address[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseAddressInformation?.StreetAddress) ? userApplication.Application.SpouseAddressInformation?.StreetAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[3].City[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseAddressInformation?.City) ? userApplication.Application.SpouseAddressInformation?.City.ToUpper() : "", true);
        if (Constants.StateAbbreviations.TryGetValue(spouseState, out abbreviation))
        {
            form.GetField("form1[0].#subform[3].State[0]").SetValue(!string.IsNullOrEmpty(abbreviation) ? abbreviation.ToUpper() : "", true);
        } else
        {
            form.GetField("form1[0].#subform[3].State[0]").SetValue(!string.IsNullOrEmpty(spouseState) ? spouseState.ToUpper() : "", true);
        }
        form.GetField("form1[0].#subform[3].Zip[0]").SetValue(userApplication.Application.SpouseAddressInformation?.Zip ?? "", true);
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
            form.GetField("form1[0].#subform[6].REASON_OF_INSANTITY[0]").SetValue(qualifyingQuestions?.QuestionFifteen.Explanation, true);
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
            form.GetField("form1[0].#subform[7].FIREARM_STATEMENT[0]").SetValue(qualifyingQuestions?.QuestionTwentyOne.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[7].NO[19]").SetValue("1", true);
        }

        // Character References
        var characterReferences = userApplication.Application.CharacterReferences;
        if (null != characterReferences && characterReferences.Any())
        {
            for (int i = 0; i < 3; i++)
            {
                form.GetField("form1[0].#subform[8].NAME[" + i + "]").SetValue(!string.IsNullOrEmpty(characterReferences[i].Name) ? characterReferences[i].Name.ToUpper() : "", true);
                form.GetField("form1[0].#subform[8].RELATIONSHIP[" + i + "]").SetValue(!string.IsNullOrEmpty(characterReferences[i].Relationship) ? characterReferences[i].Relationship.ToUpper() : "", true);
                form.GetField("form1[0].#subform[8].PHONE_NUMBER[" + i + "]").SetValue(!string.IsNullOrEmpty(characterReferences[i].PhoneNumber) ? characterReferences[i].PhoneNumber.ToUpper() : "", true);
            }
        }

        //Description of Weapons
        var weapons = userApplication.Application.Weapons;
        if (null != weapons && weapons.Length > 0)
        {
            int totalWeapons = (weapons.Length > 9) ? 9 : weapons.Length;

            for (int i = 0; i < totalWeapons; i++)
            {
                form.GetField("form1[0].#subform[8].MAKE[" + i + "]").SetValue(!string.IsNullOrEmpty(weapons[i].Make) ? weapons[i].Make.ToUpper() : "", true);
                form.GetField("form1[0].#subform[8].MODEL[" + i + "]").SetValue(!string.IsNullOrEmpty(weapons[i].Model) ? weapons[i].Model.ToUpper() : "", true);
                form.GetField("form1[0].#subform[8].CALIBER[" + i + "]").SetValue(!string.IsNullOrEmpty(weapons[i].Caliber) ? weapons[i].Caliber.ToUpper() : "", true);
                form.GetField("form1[0].#subform[8].SERIAL_NUMBER[" + i + "]").SetValue(!string.IsNullOrEmpty(weapons[i].SerialNumber) ? weapons[i].SerialNumber.ToUpper() : "", true);
            }

            // NOTE: LM: Add additional page(s) for extra weapons
            if (weapons.Length > 9)
            {
                StringBuilder weaponsSb = new StringBuilder();
                int currentSetCount = 0;
                int currentWeaponCounter = 9;
                bool isContinuation = false;

                totalWeapons = weapons.Length;
                while (currentWeaponCounter < totalWeapons)
                {
                    var weapon = weapons[currentWeaponCounter++];
                    weaponsSb.AppendLine($"{weapon.Make.ToUpper()}\t{weapon.Model.ToUpper()}\t{weapon.Caliber.ToUpper()}\t{weapon.SerialNumber.ToUpper()}");
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
        }

        //Description of previous addresses
        var previousAddresses = userApplication.Application.PreviousAddresses;

        if (previousAddresses != null && previousAddresses?.Length > 0)
        {
            int totalAddresses = (previousAddresses.Length > 4) ? 4 : previousAddresses.Length;

            for (int i = 0; i < totalAddresses; i++)
            {
                int index = i + 1;
                string address = previousAddresses[i].StreetAddress;
                string state = previousAddresses[i].State?.Trim() ?? "";
                form.GetField("form1[0].#subform[3].APP_Address[" + (index - 1) + "]").SetValue(!string.IsNullOrEmpty(address) ? address.ToUpper() : "", true);
                form.GetField("form1[0].#subform[3].APP_City[" + index + "]").SetValue(!string.IsNullOrEmpty(previousAddresses[i].City) ? previousAddresses[i].City.ToUpper() : "", true);
                if (Constants.StateAbbreviations.TryGetValue(state, out abbreviation))
                {
                    form.GetField("form1[0].#subform[3].APP_State[" + index + "]").SetValue(GetStateByName(abbreviation), true);
                }
                else
                {
                    form.GetField("form1[0].#subform[3].APP_State[" + index + "]").SetValue(GetStateByName(state), true);
                }
                form.GetField("form1[0].#subform[3].APP_ZipCode[" + index + "]").SetValue(previousAddresses[i].Zip, true);
            }

            if (previousAddresses.Length > 4)
            {
                StringBuilder addressesSb = new StringBuilder();
                int currentSetCount = 0;
                int currentAddressCounter = 4;
                bool isContinuation = false;

                totalAddresses = previousAddresses.Length;
                while (currentAddressCounter < totalAddresses)
                {
                    var previousAddress = previousAddresses[currentAddressCounter++];

                    string address = previousAddress.StreetAddress;
                    addressesSb.AppendLine($"{address.ToUpper()}, {previousAddress.City.ToUpper()}, {previousAddress.State.ToUpper()} {previousAddress.Zip}");

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

    public async Task<MemoryStream> GetLegacyApplicationMemoryStream(PermitApplication userApplication, string licensingUserName, string fileName)
    {
        if (userApplication.Application.ApplicationType == default(ApplicationType))
        {
            throw new ArgumentNullException(nameof(userApplication.Application.ApplicationType));
        }

        var applicationTemplateStream = await _documentService.GetLegacyApplicationTemplateAsync(cancellationToken: default);
        var adminUserProfile = await _userProfileCosmosDbService.GetAdminUserProfileAsync(licensingUserName, cancellationToken: default);

        MemoryStream outStream = new MemoryStream();

        PdfReader pdfReader = new PdfReader(applicationTemplateStream);
        PdfWriter pdfWriter = new PdfWriter(outStream);
        PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);

        Document mainDocument = new Document(pdfDoc);
        pdfWriter.SetCloseStream(false);

        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
        form.SetGenerateAppearance(true);

        await AddProcessorsSignatureImageForLegacyApplication(licensingUserName, mainDocument);
        await AddApplicantSignatureImageForLegacyApplication(userApplication, mainDocument);

        string applicantFullName = BuildApplicantFullName(userApplication);
        string digitallySigned = $"DIGITALLY SIGNED BY: {applicantFullName}, ON {DateTime.Now.ToString("MM/dd/yyyy")}";
        form.GetField("form1[0].#subform[2].SIGNATURE[0]").SetValue(digitallySigned, true);
        form.GetField("form1[0].#subform[7].SIGNATURE[1]").SetValue(digitallySigned, true);
        form.GetField("form1[0].#subform[10].SIGNATURE[2]").SetValue(digitallySigned, true);

        form.GetField("form1[0].#subform[2].BADGE_NUMBER[0]").SetValue(adminUserProfile.BadgeNumber, true);
        form.GetField("form1[0].#subform[7].BADGE_NUMBER[1]").SetValue(adminUserProfile.BadgeNumber, true);
        form.GetField("form1[0].#subform[10].BADGE_NUMBER[2]").SetValue(adminUserProfile.BadgeNumber, true);

        form.GetField("form1[0].#subform[2].DATE[0]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"));
        form.GetField("form1[0].#subform[7].DATE[7]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"));
        form.GetField("form1[0].#subform[10].DATE[8]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"));

        form.GetField("form1[0].#subform[2].DateTimeField1[0]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"));
        form.GetField("form1[0].#subform[7].DateTimeField1[1]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"));
        form.GetField("form1[0].#subform[10].DateTimeField1[2]").SetValue(DateTime.Now.ToString("MM/dd/yyyy"));

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.Reserve:
            case ApplicationType.RenewReserve:
                form.GetField("form1[0].#subform[2].RESERVE_OFFICER[0]").SetValue("true", true);
                break;
            case ApplicationType.Judicial:
            case ApplicationType.RenewJudicial:
                form.GetField("form1[0].#subform[2].JUDGE[0]").SetValue("true", true);
                break;
            case ApplicationType.Employment:
            case ApplicationType.RenewEmployment:
                form.GetField("form1[0].#subform[2].NINETY_DAY[0]").SetValue("true", true);
                break;
            default:
                form.GetField("form1[0].#subform[2].STANDARD[0]").SetValue("true", true);
                break;
        }

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.RenewReserve:
            case ApplicationType.RenewJudicial:
            case ApplicationType.RenewStandard:
            case ApplicationType.RenewEmployment:
                form.GetField("form1[0].#subform[2].RENEWAL_APP[0]").SetValue("true", true);
                break;
            default:
                form.GetField("form1[0].#subform[2].INITIAL_APP[0]").SetValue("true", true);
                break;
        }

        var personalInfo = userApplication.Application.PersonalInfo;
        if (personalInfo == null)
        {
            throw new ArgumentNullException("PersonalInfo");
        }

        //Applicant Personal Information
        form.GetField("form1[0].#subform[2].APP_LAST_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.LastName) ? personalInfo?.LastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[2].APP_FIRST_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.FirstName) ? personalInfo?.FirstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[2].APP_MIDDLE_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.MiddleName) ? personalInfo?.MiddleName.ToUpper() : "", true);

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
        form.GetField("form1[0].#subform[2].APP_MAIDEN_NAME[0]").SetValue(!string.IsNullOrEmpty(maidenAndAliases) ? maidenAndAliases.ToUpper() : "", true);

        form.GetField("form1[0].#subform[2].APP_RESIDENT_CITY[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.City) ? userApplication.Application.CurrentAddress?.City.ToUpper() : "", true);
        form.GetField("form1[0].#subform[2].APP_RESIDENT_COUNTY[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.County) ? userApplication.Application.CurrentAddress?.County.ToUpper() : "", true);
        form.GetField("form1[0].#subform[2].APP_CITIZENSHIP[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.Country) ? userApplication.Application.CurrentAddress?.Country.ToUpper() : "", true);

        form.GetField("form1[0].#subform[2].APP_DOB[0]").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);

        var birthPlace = string.Empty;
        if (!string.IsNullOrEmpty(userApplication.Application.DOB?.BirthCity))
        {
            birthPlace = userApplication.Application.DOB?.BirthCity + "  " +
                         userApplication.Application.DOB?.BirthState + "  " +
                         userApplication.Application.DOB?.BirthCountry;
        }
        form.GetField("form1[0].#subform[2].APP_BIRTH_PLACE[0]").SetValue(!string.IsNullOrEmpty(birthPlace) ? birthPlace.ToUpper() : "", true);

        string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "ft" + " " +
                        userApplication.Application.PhysicalAppearance?.HeightInch + "in";
        form.GetField("form1[0].#subform[2].APP_HEIGHT[0]").SetValue(!string.IsNullOrEmpty(height) ? height.ToUpper() : "", true);
        form.GetField("form1[0].#subform[2].APP_LBS[0]").SetValue(userApplication.Application.PhysicalAppearance?.Weight + "lbs" ?? "", true);
        form.GetField("form1[0].#subform[2].APP_EYE_CLR[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance?.EyeColor) ? userApplication.Application.PhysicalAppearance?.EyeColor.ToUpper() : "", true);
        form.GetField("form1[0].#subform[2].APP_HAIR_CLR[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance?.HairColor) ? userApplication.Application.PhysicalAppearance?.HairColor.ToUpper() : "", true);
        string gender = userApplication.Application.PhysicalAppearance?.Gender.First().ToString() ?? "";
        form.GetField("form1[0].#subform[2].APP_GENDER[0]").SetValue(!string.IsNullOrEmpty(gender) ? gender.ToUpper() : "", true);

#if DEBUG
        foreach (var key in form.GetFormFields().Keys)
        {
            Console.WriteLine(key);
        }
#endif
        var qualifyingQuestions = userApplication.Application.LegacyQualifyingQuestions;
        if (qualifyingQuestions == null)
        {
            throw new ArgumentNullException("QualifyingQuestions");
        }

        if ((bool)qualifyingQuestions.QuestionOne.Selected)
        {
            form.GetField("form1[0].#subform[2].CURRENT_CCW[1]").SetValue("0", true);
        }
        else
        {
            form.GetField("form1[0].#subform[2].CURRENT_CCW[1]").SetValue("1", true);
        }

        if ((bool)qualifyingQuestions.QuestionOne.Selected)
        {
            form.GetField("form1[0].#subform[2].ISSUING_AGENCY[0]").SetValue(userApplication.Application.LegacyQualifyingQuestions.QuestionOne.Agency, true);
            form.GetField("form1[0].#subform[2].ISSUE_DATE[0]").SetValue(userApplication.Application.LegacyQualifyingQuestions.QuestionOne.IssueDate, true);
            form.GetField("form1[0].#subform[2].CCW_NO[0]").SetValue(userApplication.Application.LegacyQualifyingQuestions.QuestionOne.Number, true);
        }

        string questionYesNo = (bool)qualifyingQuestions.QuestionTwo.Selected ? "0" : "1";
        form.GetField("form1[0].#subform[2].CCW_DENIAL[1]").SetValue(questionYesNo, true);
        if (questionYesNo == "0")
        {
            form.GetField("form1[0].#subform[2].AGENCY_NAME[0]").SetValue(userApplication.Application.LegacyQualifyingQuestions.QuestionTwo.Agency, true);
            form.GetField("form1[0].#subform[2].DATE[1]").SetValue(userApplication.Application.LegacyQualifyingQuestions.QuestionTwo.DenialDate, true);
            form.GetField("form1[0].#subform[2].DENIAL_REASON[0]").SetValue(userApplication.Application.LegacyQualifyingQuestions.QuestionTwo.DenialReason, true);
        }

        questionYesNo = (bool)qualifyingQuestions.QuestionThree.Selected ? "0" : "1";
        form.GetField("form1[0].#subform[2].US_CITIZENSHIP[1]").SetValue(questionYesNo, true);
        if ((bool)qualifyingQuestions.QuestionThree.Selected)
        {
            form.GetField("form1[0].#subform[2].US_CITIZENSHIP[2]").SetValue(qualifyingQuestions?.QuestionThree.Explanation, true);
        }

        questionYesNo = (bool)qualifyingQuestions.QuestionFour.Selected ? "0" : "1";
        form.GetField("form1[0].#subform[2].DISHONORABLE_DISCHARGE[0]").SetValue(questionYesNo, true);
        if ((bool)qualifyingQuestions.QuestionFour.Selected)
        {
            form.GetField("form1[0].#subform[2].DISHONORBALE_DISCHARGE[0]").SetValue(qualifyingQuestions?.QuestionFour.Explanation, true);
        }

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
        form.GetField("form1[0].#subform[3].PROBATION[1]").SetValue(questionYesNo, true);
        if ((bool)qualifyingQuestions.QuestionSeven.Selected)
        {
            form.GetField("form1[0].#subform[3].PROBATION[2]").SetValue(qualifyingQuestions?.QuestionSeven.Explanation, true);
        }
        else
        {
            form.GetField("form1[0].#subform[3].PROBATION[1]").SetValue("1", true);
        }

        if ((bool)qualifyingQuestions.QuestionEight.Selected)
        {
            for (int i = 0; i < qualifyingQuestions.QuestionEight.TrafficViolations.Count && i <= 4; i++)
            {
                form.GetField($"form1[0].#subform[3].DATE[{i + 2}]").SetValue(qualifyingQuestions.QuestionEight.TrafficViolations[i].Date, true);
                form.GetField($"form1[0].#subform[3].VIOLATION[{i}]").SetValue(qualifyingQuestions.QuestionEight.TrafficViolations[i].Violation, true);
                form.GetField($"form1[0].#subform[3].AGENCY[{i}]").SetValue(qualifyingQuestions.QuestionEight.TrafficViolations[i].Agency, true);
                form.GetField($"form1[0].#subform[3].CITATION_NO[{i}]").SetValue(qualifyingQuestions.QuestionEight.TrafficViolations[i].CitationNumber, true);
            }

            if (qualifyingQuestions.QuestionEight.TrafficViolations.Count > 5)
            {
                StringBuilder stringBuilder = new();

                for (int i = 5; i < qualifyingQuestions.QuestionEight.TrafficViolations.Count; i++)
                {
                    var violation = qualifyingQuestions.QuestionEight.TrafficViolations[i];
                    stringBuilder.AppendLine($"{violation.Date}\t{violation.Violation}\t{violation.Agency}\t{violation.CitationNumber}");
                }

                AddLegacyAppendixPage("Appendix A: Additional Moving Violations", stringBuilder.ToString(), form, pdfDoc, true);
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
                form.GetField("form1[0].#subform[4].MAKE[" + i + "]").SetValue(!string.IsNullOrEmpty(weapons[i].Make) ? weapons[i].Make.ToUpper() : "", true);
                form.GetField("form1[0].#subform[4].MODEL[" + i + "]").SetValue(!string.IsNullOrEmpty(weapons[i].Model) ? weapons[i].Model.ToUpper() : "", true);
                form.GetField("form1[0].#subform[4].CALIBER[" + i + "]").SetValue(!string.IsNullOrEmpty(weapons[i].Caliber) ? weapons[i].Caliber.ToUpper() : "", true);
                form.GetField("form1[0].#subform[4].SERIAL_NUMBER[" + i + "]").SetValue(!string.IsNullOrEmpty(weapons[i].SerialNumber) ? weapons[i].SerialNumber.ToUpper() : "", true);
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
                    weaponsSb.AppendLine($"{weapon.Make.ToUpper()}\t{weapon.Model.ToUpper()}\t{weapon.Caliber.ToUpper()}\t{weapon.SerialNumber.ToUpper()}");
                    currentSetCount++;

                    if (currentSetCount >= 30)
                    {
                        currentSetCount = 0;
                        string headerText = "Appendix B: Additional Weapons" + (isContinuation ? " - Continued" : "");
                        AddLegacyAppendixPage(headerText, weaponsSb.ToString(), form, pdfDoc, true);
                        isContinuation = true;
                        weaponsSb.Clear();
                    }
                }

                if (weaponsSb.Length > 0)
                {
                    string headerText = "Appendix B: Additional Weapons" + (isContinuation ? " - Continued" : "");
                    AddLegacyAppendixPage(headerText, weaponsSb.ToString(), form, pdfDoc, true);
                }
            }
        }

        form.GetField("form1[0].#subform[8].APPL_LAST_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.LastName) ? personalInfo?.LastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].APPL_FIRST_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.FirstName) ? personalInfo?.FirstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].APPL_MIDDLE_NAME[0]").SetValue(!string.IsNullOrEmpty(personalInfo?.MiddleName) ? personalInfo?.MiddleName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].APP_DOB[1]").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);

        //Investigator's Interview Notes

        if (!string.IsNullOrEmpty(userApplication.Application.DOB?.BirthDate))
        {
            DateTime birthDateTime = DateTime.ParseExact(userApplication.Application.DOB.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var age = CalculateAge(birthDateTime);
            form.GetField("form1[0].#subform[8].APP_AGE[0]").SetValue(age.ToString(), true);
        }

        form.GetField("form1[0].#subform[8].APP_SSN[0]").SetValue(FormatSSN(userApplication.Application.PersonalInfo?.Ssn) ?? "", true);
        form.GetField("form1[0].#subform[8].APP_CDL[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.IdInfo?.IdNumber) ? userApplication.Application.IdInfo?.IdNumber.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].APP_CDL_RESTRICTIONS[0]").SetValue(userApplication.Application.LegacyQualifyingQuestions?.QuestionSixteen.Explanation ?? "", true);
        string residenceAddress = userApplication.Application.CurrentAddress?.StreetAddress;
        form.GetField("form1[0].#subform[8].APP_Address[0]").SetValue(!string.IsNullOrEmpty(residenceAddress) ? residenceAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].APP_City[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.City) ? userApplication.Application.CurrentAddress?.City.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].APP_State[0]").SetValue(GetStateByName(userApplication.Application.CurrentAddress?.State) ?? "", true);
        form.GetField("form1[0].#subform[8].APP_ZipCode[0]").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
        form.GetField("form1[0].#subform[8].APP_DAY_PhoneNum[0]").SetValue(FormatPhoneNumber(userApplication.Application.Contact?.PrimaryPhoneNumber), true);

        string mailingAddress = userApplication.Application.MailingAddress?.StreetAddress;
        form.GetField("form1[0].#subform[8].APP_MAILINGAddress[0]").SetValue(!string.IsNullOrEmpty(mailingAddress) ? mailingAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].APP_MAILING_City[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.MailingAddress.City) ? userApplication.Application.MailingAddress.City.ToUpper() : "", true); ;
        form.GetField("form1[0].#subform[8].APP_MAILING_State[0]").SetValue(GetStateByName(userApplication.Application.MailingAddress?.State) ?? "", true);
        form.GetField("form1[0].#subform[8].APP_MAILING_Zip[0]").SetValue(userApplication.Application.MailingAddress?.Zip ?? "", true);
        form.GetField("form1[0].#subform[8].APP_EVE_PhoneNum[0]").SetValue(FormatPhoneNumber(userApplication.Application.Contact?.CellPhoneNumber), true);

        form.GetField("form1[0].#subform[8].SPOUSE_LAST_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseInformation?.LastName) ? userApplication.Application.SpouseInformation?.LastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].SPOUSE_FIRST_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseInformation?.FirstName) ? userApplication.Application.SpouseInformation?.FirstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].SPOUSE_MIDDLE_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseInformation?.MiddleName) ? userApplication.Application.SpouseInformation?.MiddleName.ToUpper() : "", true);

        string spouseAddress = userApplication.Application.SpouseAddressInformation?.StreetAddress;
        form.GetField("form1[0].#subform[8].SPOUSE_Address[0]").SetValue(!string.IsNullOrEmpty(spouseAddress) ? spouseAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].SPOUSE_City[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.SpouseAddressInformation?.City) ? userApplication.Application.SpouseAddressInformation?.City.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].SPOUSE_State[0]").SetValue(GetStateByName(userApplication.Application.SpouseAddressInformation?.State) ?? "", true);
        form.GetField("form1[0].#subform[8].SPOUSE_ZipCode[0]").SetValue(userApplication.Application.SpouseAddressInformation?.Zip ?? "", true);
        form.GetField("form1[0].#subform[8].SPOUSE_PhoneNum[0]").SetValue(FormatPhoneNumber(userApplication.Application.SpouseInformation?.PhoneNumber) ?? "", true);

        form.GetField("form1[0].#subform[8].APP_OCC[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation?.Occupation) ? userApplication.Application.WorkInformation?.Occupation.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].EMPOYER_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation?.EmployerName) ? userApplication.Application.WorkInformation?.EmployerName.ToUpper() : "", true);

        string workAddress = userApplication.Application.WorkInformation?.EmployerStreetAddress;
        form.GetField("form1[0].#subform[8].CURRENT_EMP_Address[0]").SetValue(!string.IsNullOrEmpty(workAddress) ? workAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].CURRENT_EMP_City[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation?.EmployerCity) ? userApplication.Application.WorkInformation?.EmployerCity.ToUpper() : "", true);
        form.GetField("form1[0].#subform[8].CURRENT_EMPLOYER_State[0]").SetValue(GetStateByName(userApplication.Application.WorkInformation?.EmployerState) ?? "", true);
        form.GetField("form1[0].#subform[8].CURRENT_EMPLOYER_ZipCode[0]").SetValue(userApplication.Application.WorkInformation?.EmployerZip ?? "", true);
        form.GetField("form1[0].#subform[8].CURRENT_EMPLOYER_PhoneNum[0]").SetValue(FormatPhoneNumber(userApplication.Application.WorkInformation?.EmployerPhone), true);

        //Description of previous addresses
        var previousAddresses = userApplication.Application.PreviousAddresses;

        if (previousAddresses != null && previousAddresses?.Length > 0)
        {
            int totalAddresses = (previousAddresses.Length > 4) ? 4 : previousAddresses.Length;

            for (int i = 0; i < totalAddresses; i++)
            {
                int index = i + 1;
                string address = previousAddresses[i].StreetAddress;
                form.GetField("form1[0].#subform[8].APP_Address[" + index + "]").SetValue(!string.IsNullOrEmpty(address) ? address.ToUpper() : "", true);
                form.GetField("form1[0].#subform[8].APP_City[" + index + "]").SetValue(!string.IsNullOrEmpty(previousAddresses[i].City) ? previousAddresses[i].City.ToUpper() : "", true);
                form.GetField("form1[0].#subform[8].APP_State[" + index + "]").SetValue(GetStateByName(previousAddresses[i].State), true);
                form.GetField("form1[0].#subform[8].APP_ZipCode[" + index + "]").SetValue(previousAddresses[i].Zip, true);
            }

            // NOTE: LM: Add additional page(s) for extra addresses
            if (previousAddresses.Length > 4)
            {
                StringBuilder addressesSb = new StringBuilder();
                int currentSetCount = 0;
                int currentAddressCounter = 4;
                bool isContinuation = false;

                totalAddresses = previousAddresses.Length;
                while (currentAddressCounter < totalAddresses)
                {
                    var previousAddress = previousAddresses[currentAddressCounter++];

                    string address = previousAddress.StreetAddress;
                    addressesSb.AppendLine($"{address.ToUpper()}, {previousAddress.City.ToUpper()}, {previousAddress.State.ToUpper()} {previousAddress.Zip}");

                    currentSetCount++;
                    if (currentSetCount >= 30)
                    {
                        currentSetCount = 0;
                        string headerText = "Appendix C: Additional Previous Addresses" + (isContinuation ? " - Continued" : "");
                        AddLegacyAppendixPage(headerText, addressesSb.ToString(), form, pdfDoc, true);
                        isContinuation = true;
                        addressesSb.Clear();
                    }
                }

                if (addressesSb.Length > 0)
                {
                    string headerText = "Appendix C: Additional Previous Addresses" + (isContinuation ? " - Continued" : "");
                    AddLegacyAppendixPage(headerText, addressesSb.ToString(), form, pdfDoc, true);
                }
            }
        }

        if ((bool)userApplication.Application.LegacyQualifyingQuestions.QuestionEleven.Selected)
        {
            form.GetField("form1[0].#subform[8].MENTAL_FACILITY[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[8].MENTAL_FACILITY[2]").SetValue(userApplication.Application.LegacyQualifyingQuestions?.QuestionEleven.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[8].MENTAL_FACILITY[1]").SetValue("1", true);
        }

        if ((bool)userApplication.Application.LegacyQualifyingQuestions.QuestionTwelve.Selected)
        {
            form.GetField("form1[0].#subform[8].ADDICTION[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[8].ADDICTION[2]").SetValue(userApplication.Application.LegacyQualifyingQuestions?.QuestionTwelve.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[8].ADDICTION[1]").SetValue("1", true);
        }

        if ((bool)userApplication.Application.LegacyQualifyingQuestions.QuestionThirteen.Selected)
        {
            form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[2]").SetValue(userApplication.Application.LegacyQualifyingQuestions?.QuestionThirteen.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[9].FIREARMS_INCIDENT[1]").SetValue("1", true);
        }

        if ((bool)userApplication.Application.LegacyQualifyingQuestions.QuestionFourteen.Selected)
        {
            form.GetField("form1[0].#subform[9].DV[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[9].DV[2]").SetValue(userApplication.Application.LegacyQualifyingQuestions?.QuestionFourteen.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[9].DV[1]").SetValue("1", true);
        }

        if ((bool)userApplication.Application.LegacyQualifyingQuestions.QuestionFifteen.Selected)
        {
            form.GetField("form1[0].#subform[9].FORMAL_CHARGES[1]").SetValue("0", true);
            form.GetField("form1[0].#subform[9].FORMAL_CHARGES[2]").SetValue(userApplication.Application.LegacyQualifyingQuestions?.QuestionFifteen.Explanation ?? "", true);
        }
        else
        {
            form.GetField("form1[0].#subform[9].FORMAL_CHARGES[1]").SetValue("1", true);
        }

        form.GetField("form1[0].#subform[9].GOOD_CAUSE_STATEMENT[0]").SetValue(userApplication.Application.LegacyQualifyingQuestions?.QuestionSeventeen.Explanation ?? "", true);

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

    public async Task<MemoryStream> GetRevocationLetterMemoryStream(PermitApplication userApplication, string user, string licensingUserName, string reason, string date, string fileName, string licensingEmail)
    {

        if (userApplication.Application.ApplicationType == default(ApplicationType))
        {
            throw new ArgumentNullException(nameof(userApplication.Application.ApplicationType));
        }

        var streamToReadFrom = await _documentService.GetRevocationLetterTemplateAsync(cancellationToken: default);

        var adminResponse = await _adminCosmosDbService.GetAgencyProfileSettingsAsync(cancellationToken: default);
        var adminUserProfile = await _userProfileCosmosDbService.GetAdminUserProfileAsync(licensingUserName, cancellationToken: default);

        MemoryStream outStream = new MemoryStream();

        PdfReader pdfReader = new PdfReader(streamToReadFrom);
        pdfReader.SetUnethicalReading(true);
        PdfWriter pdfWriter = new PdfWriter(outStream);
        PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);

        pdfWriter.SetCloseStream(false);

        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
        form.SetGenerateAppearance(true);

        form.GetField("form1[0].#subform[0].Issuing_LEA[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyName) ? adminResponse.AgencyName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].ORI_Number[0]").SetValue(!string.IsNullOrEmpty(adminResponse.ORI) ? adminResponse.ORI.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].Address[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyShippingStreetAddress) ? adminResponse.AgencyShippingStreetAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].City[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyShippingCity) ? adminResponse.AgencyShippingCity.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].County_Code[0]").SetValue(!string.IsNullOrEmpty(adminResponse.MailCode) ? adminResponse.MailCode.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].ZIP[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyShippingZip) ? adminResponse.AgencyShippingZip.ToUpper() : "", true);
        string[] nameParts = user.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string lastName = nameParts[0].Trim();
        string firstName = nameParts[1].Trim();
        form.GetField("form1[0].#subform[0].LastName[0]").SetValue(!string.IsNullOrEmpty(lastName) ? lastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].FirstName[0]").SetValue(!string.IsNullOrEmpty(firstName) ? firstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].Job_Title_or_Rank[0]").SetValue(!string.IsNullOrEmpty(adminUserProfile.JobTitle) ? adminUserProfile.JobTitle.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].Phone_Number[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyTelephone) ? adminResponse.AgencyTelephone.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].Fax_Number[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyFax) ? adminResponse.AgencyFax.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].EmailAddress[0]").SetValue(!string.IsNullOrEmpty(licensingEmail) ? licensingEmail.ToUpper() : "", true);

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.RenewStandard:
            case ApplicationType.RenewJudicial:
            case ApplicationType.RenewReserve:
            case ApplicationType.RenewEmployment:
                form.GetField("form1[0].#subform[0].RENEWAL[0]").SetValue("0", true);
                break;
            default:
                form.GetField("form1[0].#subform[0].INITIAL[0]").SetValue("1", true);
                break;

        }

        form.GetField("form1[0].#subform[0].DateofLiveScanorRenewal[0]").SetValue(userApplication.Application.LiveScanInfo.Date ?? "", true);
        form.GetField("form1[0].#subform[0].ApplicantTrackingIdentifier[0]").SetValue(userApplication.Application.LiveScanInfo.ATINumber ?? "", true);
        form.GetField("form1[0].#subform[0].CII_Number[0]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("form1[0].#subform[0].Local_Agency_Number[0]").SetValue(adminResponse.LocalAgencyNumber ?? "", true);
        form.GetField("form1[0].#subform[0].dateofissue[0]").SetValue(userApplication.Application.License.IssueDate.Value.Date.ToShortDateString(), true);
        form.GetField("form1[0].#subform[0].expirationDate[0]").SetValue(userApplication.Application.License.ExpirationDate.Value.Date.ToShortDateString(), true);
        form.GetField("form1[0].#subform[0].LastName[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.LastName) ? userApplication.Application.PersonalInfo.LastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].Suffix[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.Suffix) ? userApplication.Application.PersonalInfo.Suffix.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].FirstName[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.FirstName) ? userApplication.Application.PersonalInfo.FirstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].MiddleName[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.MiddleName) ? userApplication.Application.PersonalInfo.MiddleName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].dob[0]").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);
        form.GetField("form1[0].#subform[0].streetaddress[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress.StreetAddress) ? userApplication.Application.CurrentAddress.StreetAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].city[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress.City) ? userApplication.Application.CurrentAddress.City.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].county[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress.County) ? userApplication.Application.CurrentAddress.County.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].zipcode[0]").SetValue(userApplication.Application.CurrentAddress.Zip ?? "", true);

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.RenewStandard:
            case ApplicationType.Standard:
            case ApplicationType.ModifyStandard:
                form.GetField("form1[0].#subform[0].CCWType[0]").SetValue("STANDARD", true);
                break;
            case ApplicationType.RenewJudicial:
            case ApplicationType.Judicial:
            case ApplicationType.ModifyJudicial:
                form.GetField("form1[0].#subform[0].CCWType[0]").SetValue("JUDICIAL", true);
                break;
            case ApplicationType.RenewReserve:
            case ApplicationType.Reserve:
            case ApplicationType.ModifyReserve:
                form.GetField("form1[0].#subform[0].CCWType[0]").SetValue("RESERVE", true);
                break;
            case ApplicationType.RenewEmployment:
            case ApplicationType.Employment:
            case ApplicationType.ModifyEmployment:
                form.GetField("form1[0].#subform[0].CCWType[0]").SetValue("EMPLOYMENT", true);
                break;
        }
       
        switch (userApplication.Application.Status)
        {
            case ApplicationStatus.Denied:
                form.GetField("form1[0].#subform[0].denial[0]").SetValue("Yes", true);
                form.GetField("form1[0].#subform[0].SelectReason[0]").SetValue(userApplication.Application.DenialInfo.Reason, true);
                form.GetField("form1[0].#subform[0].OtherReason[0]").SetValue(userApplication.Application.DenialInfo.OtherReason ?? "", true);
                form.GetField("form1[0].#subform[0].Date[0]").SetValue(userApplication.Application.DenialInfo.Date, true);
                break;
            case ApplicationStatus.Revoked:
                form.GetField("form1[0].#subform[0].revocation[0]").SetValue("Yes", true);
                form.GetField("form1[0].#subform[0].SelectReason[1]").SetValue(userApplication.Application.RevocationInfo.Reason, true);
                form.GetField("form1[0].#subform[0].OtherReason[1]").SetValue(userApplication.Application.RevocationInfo.OtherReason ?? "", true);
                form.GetField("form1[0].#subform[0].Date[1]").SetValue(userApplication.Application.RevocationInfo.Date, true);
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
        if (userApplication.Application.ApplicationType == default(ApplicationType))
        {
            throw new ArgumentNullException(nameof(userApplication.Application.ApplicationType));
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
        await AddSheriffLogoForOfficial(mainDocument);

        form.GetField("AGENCY_NAME[0]").SetValue(adminResponse.AgencyName ?? "", true);
        form.GetField("AGENCY_ORI[0]").SetValue(adminResponse.ORI ?? "", true);
        form.GetField("CII_NUMBER[0]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("AGENCY_LICENSE_NUMBER[0]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("HEAD_OF_AGENCY[0]").SetValue(adminResponse.AgencySheriffName);
        form.GetField("ISSUED_DATE[0]").SetValue(userApplication.Application.License.IssueDate.Value.Date.ToShortDateString() ?? "", true);
        form.GetField("EXPIRED_DATE[0]").SetValue(userApplication.Application.License.ExpirationDate.Value.Date.ToShortDateString() ?? "", true);
        form.GetField("CII_NUMBER[0]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("COUNTY_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress.County) ? userApplication.Application.CurrentAddress.County.ToUpper() : "", true);
        string fullname = BuildApplicantFullName(userApplication);
        form.GetField("APPLICANT_NAME[0]").SetValue(!string.IsNullOrEmpty(fullname.Trim()) ? fullname.Trim().ToUpper() : "", true);
        string residenceAddress = userApplication.Application.CurrentAddress?.StreetAddress + ", " + userApplication.Application.CurrentAddress?.City +
         " " + userApplication.Application.CurrentAddress.State + " " + userApplication.Application.CurrentAddress.Zip;
        form.GetField("RESIDENTIAL_ADDRESS[0]").SetValue(!string.IsNullOrEmpty(residenceAddress) ? residenceAddress.ToUpper() : "", true);
        form.GetField("DATE_OF_BIRTH[0]").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);
        form.GetField("ID_NUMBER[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.IdInfo.IdNumber) ? userApplication.Application.IdInfo.IdNumber.ToUpper() : "", true);
        if (userApplication.Application.Employment == "Unemployed")
        {
            form.GetField("OCCUPATION[0]").SetValue("UNEMPLOYED", true);
        }
        else if (userApplication.Application.Employment == "Retired")
        {
            form.GetField("OCCUPATION[0]").SetValue("RETIRED", true);
        }
        else
        {
            form.GetField("OCCUPATION[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation.Occupation) ? userApplication.Application.WorkInformation.Occupation.ToUpper() : "", true);
        }

        string businessAddress = userApplication.Application.WorkInformation?.EmployerStreetAddress + ", " + userApplication.Application.WorkInformation?.EmployerCity + " "
        + userApplication.Application.WorkInformation.EmployerState + " " + userApplication.Application.WorkInformation.EmployerZip;
        if (userApplication.Application.Employment != "Unemployed" && userApplication.Application.Employment != "Retired")
        {
            form.GetField("BUSINESS_ADDRESS[0]").SetValue(!string.IsNullOrEmpty(businessAddress) ? businessAddress.ToUpper() : "", true);
        }
        form.GetField("HEIGHT[0]").SetValue(userApplication.Application.PhysicalAppearance.HeightFeet + "'" + userApplication.Application.PhysicalAppearance.HeightInch ?? "", true);
        form.GetField("WEIGHT[0]").SetValue(userApplication.Application.PhysicalAppearance.Weight ?? "", true);
        if (userApplication.Application.PhysicalAppearance.EyeColor == "Multicolor")
        {
            form.GetField("EYE_COLOR[0]").SetValue("MULTI", true);
        }
        else 
        {
            form.GetField("EYE_COLOR[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance.EyeColor) ? userApplication.Application.PhysicalAppearance.EyeColor.ToUpper() : "", true);
        }
        if (userApplication.Application.PhysicalAppearance.HairColor == "Multicolor")
        {
            form.GetField("HAIR_COLOR[0]").SetValue("MULTI", true);
        }
        else
        {
            form.GetField("HAIR_COLOR[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance.HairColor) ? userApplication.Application.PhysicalAppearance.HairColor.ToUpper() : "", true);
        }

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.Reserve:
            case ApplicationType.RenewReserve:
            case ApplicationType.ModifyReserve:
                form.GetField("LICENSE_TYPE[0]").SetValue("RESERVE", true);
                break;
            case ApplicationType.Judicial:
            case ApplicationType.RenewJudicial:
            case ApplicationType.ModifyJudicial:
                form.GetField("LICENSE_TYPE[0]").SetValue("JUDICIAL", true);
                break;
            case ApplicationType.Employment:
            case ApplicationType.RenewEmployment:
            case ApplicationType.ModifyEmployment:
                form.GetField("LICENSE_TYPE[0]").SetValue("EMPLOYMENT", true);
                break;
            default:
                form.GetField("LICENSE_TYPE[0]").SetValue("STANDARD", true);
                break;
        }

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.RenewJudicial:
            case ApplicationType.RenewReserve:
            case ApplicationType.RenewEmployment:
            case ApplicationType.RenewStandard:
                form.GetField("LICENSE_ISSUED[0]").SetValue("SUBSEQUENT", true);
                break;
            case ApplicationType.ModifyJudicial:
            case ApplicationType.ModifyEmployment:
            case ApplicationType.ModifyReserve:
            case ApplicationType.ModifyStandard:
                form.GetField("LICENSE_ISSUED[0]").SetValue("SUBSEQUENT", true);
                break;
            default:
                form.GetField("LICENSE_ISSUED[0]").SetValue("INITIAL", true);
                break;
        }

        form.GetField("AGENCY_NAME[1]").SetValue(adminResponse.AgencyName ?? "", true);
        form.GetField("AGENCY_ORI[1]").SetValue(adminResponse.ORI ?? "", true);
        form.GetField("CII_NUMBER[1]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("AGENCY_LICENSE_NUMBER[1]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("HEAD_OF_AGENCY[1]").SetValue(adminResponse.AgencySheriffName);
        form.GetField("ISSUED_DATE[1]").SetValue(userApplication.Application.License.IssueDate.Value.Date.ToShortDateString() ?? "", true);
        form.GetField("EXPIRED_DATE[1]").SetValue(userApplication.Application.License.ExpirationDate.Value.Date.ToShortDateString() ?? "", true);
        form.GetField("CII_NUMBER[1]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("COUNTY_NAME[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress.County) ? userApplication.Application.CurrentAddress.County.ToUpper() : "", true);
        form.GetField("APPLICANT_NAME[1]").SetValue(fullname.Trim().ToUpper(), true);
        form.GetField("RESIDENTIAL_ADDRESS[1]").SetValue(!string.IsNullOrEmpty(residenceAddress) ? residenceAddress.ToUpper() : "", true);
        form.GetField("DATE_OF_BIRTH[1]").SetValue(userApplication.Application.DOB.BirthDate ?? "", true);
        form.GetField("ID_NUMBER[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.IdInfo.IdNumber) ? userApplication.Application.IdInfo.IdNumber.ToUpper() : "", true);
        if (userApplication.Application.Employment == "Unemployed")
        {
            form.GetField("OCCUPATION[1]").SetValue("UNEMPLOYED", true);
        }
        else if (userApplication.Application.Employment == "Retired")
        {
            form.GetField("OCCUPATION[1]").SetValue("RETIRED", true);
        }
        else
        {
            form.GetField("OCCUPATION[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.WorkInformation.Occupation) ? userApplication.Application.WorkInformation.Occupation.ToUpper() : "", true);
        }

        if (userApplication.Application.Employment != "Unemployed" && userApplication.Application.Employment != "Retired")
        {
            form.GetField("BUSINESS_ADDRESS[1]").SetValue(!string.IsNullOrEmpty(businessAddress) ? businessAddress.ToUpper() : "", true);
        }
        form.GetField("HEIGHT[1]").SetValue(userApplication.Application.PhysicalAppearance.HeightFeet + "'" + userApplication.Application.PhysicalAppearance.HeightInch ?? "", true);
        form.GetField("WEIGHT[1]").SetValue(userApplication.Application.PhysicalAppearance.Weight ?? "", true);
        if (userApplication.Application.PhysicalAppearance.EyeColor == "Multicolor")
        {
            form.GetField("EYE_COLOR[1]").SetValue("MULTI", true);
        }
        else
        {
            form.GetField("EYE_COLOR[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance.EyeColor) ? userApplication.Application.PhysicalAppearance.EyeColor.ToUpper() : "", true);
        }
        if (userApplication.Application.PhysicalAppearance.HairColor == "Multicolor")
        {
            form.GetField("HAIR_COLOR[1]").SetValue("MULTI", true);
        }
        else
        {
            form.GetField("HAIR_COLOR[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance.HairColor) ? userApplication.Application.PhysicalAppearance.HairColor.ToUpper() : "", true);
        }

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.Reserve:
            case ApplicationType.RenewReserve:
            case ApplicationType.ModifyReserve:
                form.GetField("LICENSE_TYPE[1]").SetValue("RESERVE", true);
                break;
            case ApplicationType.Judicial:
            case ApplicationType.RenewJudicial:
            case ApplicationType.ModifyJudicial:
                form.GetField("LICENSE_TYPE[1]").SetValue("JUDICIAL", true);
                break;
            case ApplicationType.Employment:
            case ApplicationType.RenewEmployment:
            case ApplicationType.ModifyEmployment:
                form.GetField("LICENSE_TYPE[1]").SetValue("EMPLOYMENT", true);
                break;
            default:
                form.GetField("LICENSE_TYPE[1]").SetValue("STANDARD", true);
                break;
        }

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.RenewJudicial:
            case ApplicationType.RenewReserve:
            case ApplicationType.RenewEmployment:
            case ApplicationType.RenewStandard:
                form.GetField("LICENSE_ISSUED[1]").SetValue("SUBSEQUENT", true);
                break;
            case ApplicationType.ModifyJudicial:
            case ApplicationType.ModifyEmployment:
            case ApplicationType.ModifyReserve:
            case ApplicationType.ModifyStandard:
                form.GetField("LICENSE_ISSUED[1]").SetValue("SUBSEQUENT", true);
                break;
            default:
                form.GetField("LICENSE_ISSUED[1]").SetValue("INITIAL", true);
                break;
        }

        form.GetField("AGENCY_NAME[2]").SetValue(adminResponse.AgencyName ?? "", true);
        form.GetField("AGENCY_ORI[2]").SetValue(adminResponse.ORI ?? "", true);
        form.GetField("CII_NUMBER[2]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("AGENCY_LICENSE_NUMBER[2]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("HEAD_OF_AGENCY[2]").SetValue(adminResponse.AgencySheriffName);
        form.GetField("ISSUED_DATE[2]").SetValue(userApplication.Application.License.IssueDate.Value.Date.ToShortDateString() ?? "", true);
        form.GetField("EXPIRED_DATE[2]").SetValue(userApplication.Application.License.ExpirationDate.Value.Date.ToShortDateString() ?? "", true);
        form.GetField("CII_NUMBER[2]").SetValue(userApplication.Application.CiiNumber ?? "", true);
        form.GetField("COUNTY_NAME[2]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress.County) ? userApplication.Application.CurrentAddress.County.ToUpper() : "");
        form.GetField("APPLICANT_NAME[2]").SetValue(!string.IsNullOrEmpty(fullname.Trim()) ? fullname.Trim().ToUpper() : "", true);
        form.GetField("RESIDENTIAL_ADDRESS[2]").SetValue(!string.IsNullOrEmpty(residenceAddress) ? residenceAddress.ToUpper() : "", true);

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.Reserve:
            case ApplicationType.RenewReserve:
            case ApplicationType.ModifyReserve:
                form.GetField("LICENSE_TYPE[2]").SetValue("RESERVE", true);
                break;
            case ApplicationType.Judicial:
            case ApplicationType.RenewJudicial:
            case ApplicationType.ModifyJudicial:
                form.GetField("LICENSE_TYPE[2]").SetValue("JUDICIAL", true);
                break;
            case ApplicationType.Employment:
            case ApplicationType.RenewEmployment:
            case ApplicationType.ModifyEmployment:
                form.GetField("LICENSE_TYPE[2]").SetValue("EMPLOYMENT", true);
                break;
            default:
                form.GetField("LICENSE_TYPE[2]").SetValue("STANDARD", true);
                break;
        }

        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.RenewJudicial:
            case ApplicationType.RenewReserve:
            case ApplicationType.RenewEmployment:
            case ApplicationType.RenewStandard:
                form.GetField("LICENSE_ISSUED[2]").SetValue("SUBSEQUENT", true);
                break;
            case ApplicationType.ModifyJudicial:
            case ApplicationType.ModifyEmployment:
            case ApplicationType.ModifyReserve:
            case ApplicationType.ModifyStandard:
                form.GetField("LICENSE_ISSUED[2]").SetValue("SUBSEQUENT", true);
                break;
            default:
                form.GetField("LICENSE_ISSUED[2]").SetValue("INITIAL", true);
                break;
        }

        var weapons = userApplication.Application.Weapons;
        if (null != weapons && weapons.Length > 0)
        {
            int totalWeapons = weapons.Length;
            string makeField;
            string modelField;
            string serialField;
            string caliberField;

            for (int i = 0; i < totalWeapons && i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {

                    makeField = $"MANUFACTURER{i + 1}[{j}]";
                    modelField = $"MODEL{i + 1}[{j}]";
                    serialField = $"SERIAL{i + 1}[{j}]";
                    caliberField = $"CALIBER{i + 1}[{j}]";

                    form.GetField(makeField).SetValue(!string.IsNullOrEmpty(weapons[i].Make) ? weapons[i].Make.ToUpper() : "", true);
                    form.GetField(modelField).SetValue(!string.IsNullOrEmpty(weapons[i].Model) ? weapons[i].Model.ToUpper() : "", true);
                    form.GetField(serialField).SetValue(!string.IsNullOrEmpty(weapons[i].SerialNumber) ? weapons[i].SerialNumber.ToUpper() : "", true);
                    form.GetField(caliberField).SetValue(!string.IsNullOrEmpty(weapons[i].Caliber) ? weapons[i].Caliber.ToUpper() : "", true);
                }
            }
        }

        if (null != weapons && weapons.Length > 0)
        {
            int totalWeapons = weapons.Length;
            string makeField;
            string modelField;
            string serialField;
            string caliberField;

            for (int i = 0; i < totalWeapons && i < 30; i++)
            {
                for (int j = 0; j < 2; j++)
                {

                    makeField = $"MANUFACTURER_AM{i + 1}[{j}]";
                    modelField = $"MODEL_AM{i + 1}[{j}]";
                    serialField = $"SERIAL_AM{i + 1}[{j}]";
                    caliberField = $"CALIBER_AM{i + 1}[{j}]";

                    form.GetField(makeField).SetValue(!string.IsNullOrEmpty(weapons[i].Make) ? weapons[i].Make.ToUpper() : "", true);
                    form.GetField(modelField).SetValue(!string.IsNullOrEmpty(weapons[i].Model) ? weapons[i].Model.ToUpper() : "", true); ;
                    form.GetField(serialField).SetValue(!string.IsNullOrEmpty(weapons[i].SerialNumber) ? weapons[i].SerialNumber.ToUpper() : "", true);
                    form.GetField(caliberField).SetValue(!string.IsNullOrEmpty(weapons[i].Caliber) ? weapons[i].Caliber.ToUpper() : "", true);
                }
            }
        }

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
        await AddSheriffIssuingOfficierSignatureImageForUnOfficial(docFileAll);

        form.GetField("AGENCY_NAME").SetValue(adminResponse.AgencyName ?? "", true);
        form.GetField("AGENCY_ORI").SetValue(adminResponse.ORI ?? "", true);
        form.GetField("LOCAL_AGENCY_NUMBER").SetValue(adminResponse.LocalAgencyNumber ?? "", true);
        string fullname = BuildApplicantFullName(userApplication);
        form.GetField("APPLICANT_NAME").SetValue(fullname.Trim(), true);


        form.GetField("APPLICATION_ADDRESS_LINE_1").SetValue(userApplication.Application.CurrentAddress?.StreetAddress ?? "", true);
        string residenceAddress3 = userApplication.Application.CurrentAddress?.City
                                   + ", " + userApplication.Application.CurrentAddress?.State
                                   + " " + userApplication.Application.CurrentAddress?.Zip;
        form.GetField("APPLICATION_ADDRESS_LINE_2").SetValue(residenceAddress3 ?? "", true);
        ApplicationType licenseType = userApplication.Application.ApplicationType;
        string licenseTypeString = licenseType.ToString();
        licenseTypeString = char.ToUpper(licenseTypeString[0]) + licenseTypeString.Substring(1);
        form.GetField("LICENSE_TYPE").SetValue(licenseTypeString ?? "", true);
        form.GetField("DATE_OF_BIRTH").SetValue(userApplication.Application.DOB?.BirthDate ?? "", true);
        form.GetField("ISSUED_DATE").SetValue(userApplication.Application.License?.IssueDate.ToString() ?? "", true);
        form.GetField("EXPIRED_DATE").SetValue(userApplication.Application.License?.ExpirationDate.ToString() ?? "", true);

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

    public async Task<MemoryStream> GetModificationMemoryStream(PermitApplication userApplication, string licensingUsername, string fileName)
    {
        var adminResponse = await _adminCosmosDbService.GetAgencyProfileSettingsAsync(cancellationToken: default);
        var adminUserProfile = await _userProfileCosmosDbService.GetAdminUserProfileAsync(licensingUsername, cancellationToken: default);
        var streamToReadFrom = await _documentService.GetModificationTemplateAsync(cancellationToken: default);

        MemoryStream outStream = new MemoryStream();

        PdfReader pdfReader = new PdfReader(streamToReadFrom);
        pdfReader.SetUnethicalReading(true);
        PdfWriter pdfWriter = new PdfWriter(outStream);
        PdfDocument doc = new PdfDocument(pdfReader, pdfWriter);

        Document docFileAll = new Document(doc);
        pdfWriter.SetCloseStream(false);

        PdfAcroForm form = PdfAcroForm.GetAcroForm(doc, true);
        form.SetGenerateAppearance(true);

        await AddApplicantSignatureImageForModification(userApplication, docFileAll);

        form.GetField("form1[0].#subform[0].AGENCY[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyName) ? adminResponse.AgencyName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].ORI[0]").SetValue(adminResponse.ORI);
        form.GetField("form1[0].#subform[0].MAILINGADDRESS[0]").SetValue(adminResponse.AgencyShippingStreetAddress.ToUpper() + " " + adminResponse.AgencyShippingCity.ToUpper() + " " 
        + adminResponse.AgencyShippingState.ToUpper() + " " + adminResponse.AgencyShippingZip);
        form.GetField("form1[0].#subform[0].CITY[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyCity) ? adminResponse.AgencyCity.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].ZIP[0]").SetValue(adminResponse.AgencyZip);
        string fullName = adminResponse.LicensingManager;
        string[] parts = fullName.Split(',');
        string lastName = !string.IsNullOrEmpty(parts[0].Trim()) ? parts[0].Trim().ToUpper() : "";
        string firstName = !string.IsNullOrEmpty(parts[1].Trim()) ? parts[1].Trim().ToUpper() : "";
        form.GetField("form1[0].#subform[0].LAST_NAME[0]").SetValue(lastName);
        form.GetField("form1[0].#subform[0].FIRSTNAME[0]").SetValue(firstName);
        form.GetField("form1[0].#subform[0].JOBRANK[0]").SetValue("LICENSING MANAGER");
        form.GetField("form1[0].#subform[0].TELEPHONENUMBER[0]").SetValue(adminResponse.AgencyTelephone);
        form.GetField("form1[0].#subform[0].FAXNUMBER[0]").SetValue(adminResponse.AgencyFax);
        form.GetField("form1[0].#subform[0].EMAILADDRESS[0]").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyEmail) ? adminResponse.AgencyEmail.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].CIINUMBER[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CiiNumber) ? userApplication.Application.CiiNumber.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].CA_DL[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.IdInfo.IdNumber) ? userApplication.Application.IdInfo.IdNumber.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].LOCAL_NUMBER[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.CiiNumber) ? userApplication.Application.CiiNumber.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].DATE_OF_ISSUE[0]").SetValue(userApplication.Application.License.IssueDate.Value.Date.ToShortDateString() ?? "", true);
        form.GetField("form1[0].#subform[0].DATE_OF_EXP[0]").SetValue(userApplication.Application.License.ExpirationDate.Value.Date.ToShortDateString() ?? "", true);
        form.GetField("form1[0].#subform[0].LAST_NAME[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.LastName) ? userApplication.Application.PersonalInfo.LastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].SUFFIX[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.Suffix) ? userApplication.Application.PersonalInfo.Suffix.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].FIRST_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.FirstName) ? userApplication.Application.PersonalInfo.FirstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].AMEND_DATE[0]").SetValue(DateTime.Now.ToShortDateString());
        form.GetField("form1[0].#subform[0].MIDDLE_NAME[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.MiddleName) ? userApplication.Application.PersonalInfo.MiddleName.ToUpper() : "", true);
        var birthDate = DateTimeOffset.Parse(userApplication.Application.DOB.BirthDate);
        form.GetField("form1[0].#subform[0].DOB[0]").SetValue(birthDate.Date.ToShortDateString() ?? "", true);
        form.GetField("form1[0].#subform[0].LAST_NAME[2]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.ModifiedLastName) ? userApplication.Application.PersonalInfo.ModifiedLastName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].FIRST_NAME[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.ModifiedFirstName) ? userApplication.Application.PersonalInfo.ModifiedFirstName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].MIDDLE_NAME[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.ModifiedMiddleName) ? userApplication.Application.PersonalInfo.ModifiedMiddleName.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].RESIDENCE_STREET_ADDRESS[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.ModifiedAddress.StreetAddress) ? userApplication.Application.ModifiedAddress.StreetAddress.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].CITY[1]").SetValue(!string.IsNullOrEmpty(userApplication.Application.ModifiedAddress.City) ? userApplication.Application.ModifiedAddress.City.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].COUNTY[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.ModifiedAddress.County) ? userApplication.Application.ModifiedAddress.County.ToUpper() : "", true);
        form.GetField("form1[0].#subform[0].ZIP_CODE[0]").SetValue(!string.IsNullOrEmpty(userApplication.Application.ModifiedAddress.Zip) ? userApplication.Application.ModifiedAddress.Zip.ToUpper() : "", true);
        if (userApplication.Application.Status == ApplicationStatus.ModificationApproved)
        {
            form.GetField("form1[0].#subform[0].CheckBox1[0]").SetValue("Yes", true);
        }
        if (userApplication.Application.Status == ApplicationStatus.ModificationDenied)
        {
            form.GetField("form1[0].#subform[0].CheckBox1[1]").SetValue("No", true);
        }
        var correctionReasons = new List<string>();
        if (!string.IsNullOrEmpty(userApplication.Application.ModifiedAddress.StreetAddress) ||
            !string.IsNullOrEmpty(userApplication.Application.ModifiedAddress.City) ||
            !string.IsNullOrEmpty(userApplication.Application.ModifiedAddress.State) ||
            !string.IsNullOrEmpty(userApplication.Application.ModifiedAddress.Zip))
        {
            correctionReasons.Add("Address Modification");
        }

        if (!string.IsNullOrEmpty(userApplication.Application.PersonalInfo.ModifiedFirstName) ||
            !string.IsNullOrEmpty(userApplication.Application.PersonalInfo.ModifiedLastName) ||
            !string.IsNullOrEmpty(userApplication.Application.PersonalInfo.ModifiedMiddleName))
        {
            correctionReasons.Add("Name Modification");
        }

        if (userApplication.Application.ModifyAddWeapons != null && userApplication.Application.ModifyAddWeapons.Any() ||
            userApplication.Application.ModifyDeleteWeapons != null && userApplication.Application.ModifyDeleteWeapons.Any())
        {
            correctionReasons.Add("Weapons Modification");
        }

        string correctionReason = string.Join("\n", correctionReasons);
        form.GetField("form1[0].#subform[0].CORRECTION_REASON[1]").SetValue(!string.IsNullOrEmpty(correctionReason) ? correctionReason.ToUpper() : "", true);

        var addedWeapons = userApplication.Application.ModifyAddWeapons;
        var deletedWeapons = userApplication.Application.ModifyDeleteWeapons;

        int currentIndex = 0;

        if (addedWeapons != null)
        {
            for (int i = 0; i < addedWeapons.Length && currentIndex < 15; i++)
            {
                form.GetField($"form1[0].#subform[1].ADD[{currentIndex}]").SetValue("Yes", true);
                form.GetField($"form1[0].#subform[1].MANUFACTURER[{currentIndex}]").SetValue(!string.IsNullOrEmpty(addedWeapons[i].Make) ? addedWeapons[i].Make.ToUpper() : "", true);
                form.GetField($"form1[0].#subform[1].MODEL[{currentIndex}]").SetValue(!string.IsNullOrEmpty(addedWeapons[i].Model) ? addedWeapons[i].Model.ToUpper() : "", true);
                form.GetField($"form1[0].#subform[1].CALIBER[{currentIndex}]").SetValue(!string.IsNullOrEmpty(addedWeapons[i].Caliber) ? addedWeapons[i].Caliber.ToUpper() : "", true);
                form.GetField($"form1[0].#subform[1].SERIAL_NUMBER[{currentIndex}]").SetValue(!string.IsNullOrEmpty(addedWeapons[i].SerialNumber) ? addedWeapons[i].SerialNumber.ToUpper() : "", true);
                currentIndex++;
            }
        }

        if (deletedWeapons != null && currentIndex < 15)
        {
            for (int i = 0; i < deletedWeapons.Length && currentIndex < 15; i++)
            {
                form.GetField($"form1[0].#subform[1].DELETE[{currentIndex}]").SetValue("Yes", true);
                form.GetField($"form1[0].#subform[1].MANUFACTURER[{currentIndex}]").SetValue(!string.IsNullOrEmpty(deletedWeapons[i].Make) ? deletedWeapons[i].Make.ToUpper() : "", true);
                form.GetField($"form1[0].#subform[1].MODEL[{currentIndex}]").SetValue(!string.IsNullOrEmpty(deletedWeapons[i].Model) ? deletedWeapons[i].Model.ToUpper() : "", true);
                form.GetField($"form1[0].#subform[1].CALIBER[{currentIndex}]").SetValue(!string.IsNullOrEmpty(deletedWeapons[i].Caliber) ? deletedWeapons[i].Caliber.ToUpper() : "", true);
                form.GetField($"form1[0].#subform[1].SERIAL_NUMBER[{currentIndex}]").SetValue(!string.IsNullOrEmpty(deletedWeapons[i].SerialNumber) ? deletedWeapons[i].SerialNumber.ToUpper() : "", true);
                currentIndex++;
            }
        }

        form.GetField("form1[0].#subform[1].DATE[0]").SetValue(DateTime.Now.ToShortDateString());

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
        switch (userApplication.Application.ApplicationType)
        {
            case ApplicationType.RenewStandard:
            case ApplicationType.Standard:
            case ApplicationType.ModifyStandard:
                form.GetField("AUTHORIZED_APPLICANT_TYPE").SetValue("STANDARD CCW", true);
                form.GetField("LICENSE_TYPE").SetValue("STANDARD CCW", true);
                break;
            case ApplicationType.RenewJudicial:
            case ApplicationType.Judicial:
            case ApplicationType.ModifyJudicial:
                form.GetField("AUTHORIZED_APPLICANT_TYPE").SetValue("JUDICIAL CCW", true);
                form.GetField("LICENSE_TYPE").SetValue("JUDICIAL CCW", true);
                break;
            case ApplicationType.RenewReserve:
            case ApplicationType.Reserve:
            case ApplicationType.ModifyReserve:
                form.GetField("AUTHORIZED_APPLICANT_TYPE").SetValue("RESERVE CCW", true);
                form.GetField("LICENSE_TYPE").SetValue("RESERVE CCW", true);
                break;
            case ApplicationType.RenewEmployment:
            case ApplicationType.Employment:
            case ApplicationType.ModifyEmployment:
                form.GetField("AUTHORIZED_APPLICANT_TYPE").SetValue("EMPLOYMENT CCW", true);
                form.GetField("LICENSE_TYPE").SetValue("EMPLOYMENT CCW", true);
                break;
        }
        form.GetField("AGENCY_NAME").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyName) ? adminResponse.AgencyName.ToUpper() : "", true);
        form.GetField("AGENCY_MAIL_CODE").SetValue(adminResponse.MailCode ?? "", true);
        form.GetField("AGENCY_ADDRESS_1").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyShippingStreetAddress) ? adminResponse.AgencyShippingStreetAddress.ToUpper() : "", true);
        form.GetField("AGENCY_CONTACT_NAME").SetValue(!string.IsNullOrEmpty(adminResponse.ContactName) ? adminResponse.ContactName.ToUpper() : "", true);
        form.GetField("AGENCY_CITY").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyShippingCity) ? adminResponse.AgencyShippingCity.ToUpper() : "", true);
        form.GetField("AGENCY_STATE").SetValue(GetStateByName(adminResponse.AgencyShippingState.ToUpper()) ?? "", true);
        form.GetField("AGENCY_ZIP").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyShippingZip) ? adminResponse.AgencyShippingZip.ToUpper() : "", true);
        form.GetField("AGENCY_CONTACT_NUMBER").SetValue(!string.IsNullOrEmpty(adminResponse.ContactNumber) ? adminResponse.ContactNumber.ToUpper() : "", true);
        string fullname = BuildApplicantFullName(userApplication);
        form.GetField("LAST_NAME").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo?.LastName) ? userApplication.Application.PersonalInfo?.LastName.ToUpper() : "", true);
        form.GetField("FIRST_NAME").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo?.FirstName) ? userApplication.Application.PersonalInfo?.FirstName.ToUpper() : "", true);
        if (userApplication.Application.PersonalInfo?.MiddleName != "" && userApplication.Application.PersonalInfo?.MiddleName != null)
        {
            form.GetField("MIDDLE_INITIAL").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo?.MiddleName.Substring(0, 1)) ? userApplication.Application.PersonalInfo?.MiddleName.Substring(0, 1).ToUpper() : "", true);
        }

        form.GetField("SUFFIX").SetValue(!string.IsNullOrEmpty(userApplication.Application.PersonalInfo?.Suffix) ? userApplication.Application.PersonalInfo?.Suffix.ToUpper() : "", true);
        if (userApplication.Application.Aliases.Length > 0)
        {
            form.GetField("LAST_NAME_2").SetValue(!string.IsNullOrEmpty(userApplication.Application.Aliases[0].PrevLastName) ? userApplication.Application.Aliases[0].PrevLastName.ToUpper() : "", true);
            form.GetField("FIRST_NAME_2").SetValue(!string.IsNullOrEmpty(userApplication.Application.Aliases[0].PrevFirstName) ? userApplication.Application.Aliases[0].PrevFirstName.ToUpper() : "", true);
            form.GetField("SUFFIX_2").SetValue(!string.IsNullOrEmpty(userApplication.Application.Aliases[0].PrevSuffix) ? userApplication.Application.Aliases[0].PrevSuffix.ToUpper() : "", true);
        }
        form.GetField("DATE_OF_BIRTH").SetValue(!string.IsNullOrEmpty(userApplication.Application.DOB.BirthDate) ? userApplication.Application.DOB.BirthDate.ToUpper() : "", true);
        if (userApplication.Application.PhysicalAppearance.Gender == "male")
        {
            form.GetField("MALE").SetValue("true");
        }
        else
        {
            form.GetField("FEMALE").SetValue("true");
        }
        form.GetField("DL_NUMBER").SetValue(!string.IsNullOrEmpty(userApplication.Application.IdInfo.IdNumber) ? userApplication.Application.IdInfo.IdNumber.ToUpper() : "", true);
        string height = userApplication.Application.PhysicalAppearance?.HeightFeet + "'" + userApplication.Application.PhysicalAppearance?.HeightInch;
        form.GetField("HEIGHT").SetValue(!string.IsNullOrEmpty(height) ? height.ToUpper() : "", true);
        form.GetField("WEIGHT").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance.Weight) ? userApplication.Application.PhysicalAppearance.Weight.ToUpper() : "", true);
        if (userApplication.Application.PhysicalAppearance.EyeColor == "Multicolor")
        {
            form.GetField("EYE_COLOR").SetValue("MULTI", true);
        }
        else
        {
            form.GetField("EYE_COLOR").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance.EyeColor) ? userApplication.Application.PhysicalAppearance.EyeColor.ToUpper() : "", true);
        }
        if (userApplication.Application.PhysicalAppearance.HairColor == "Multicolor")
        {
            form.GetField("HAIR_COLOR").SetValue("MULTI", true);
        }
        else
        {
            form.GetField("HAIR_COLOR").SetValue(!string.IsNullOrEmpty(userApplication.Application.PhysicalAppearance.HairColor) ? userApplication.Application.PhysicalAppearance.HairColor.ToUpper() : "", true);
        }
        form.GetField("AGENCY_BILLING_NUMBER").SetValue(!string.IsNullOrEmpty(adminResponse.AgencyBillingNumber) ? adminResponse.AgencyBillingNumber.ToUpper() : "", true);
        if (userApplication.Application.DOB.BirthCountry == "United States")
        {
            form.GetField("BIRTH_STATE").SetValue(GetStateByName(userApplication.Application.DOB.BirthState) ?? "", true);
        } else
        {
            string nonUsBirthCountry = !string.IsNullOrEmpty(userApplication.Application.DOB.BirthCountry) ? userApplication.Application.DOB.BirthCountry.ToUpper() : "";
            string nonUsBirthCity = !string.IsNullOrEmpty(userApplication.Application.DOB.BirthState) ? userApplication.Application.DOB.BirthState.ToUpper() : "";
            string birthRegion = nonUsBirthCity + " " + nonUsBirthCountry;
            form.GetField("BIRTH_STATE").SetValue(birthRegion);
        }
        form.GetField("SSN").SetValue(userApplication.Application.PersonalInfo.Ssn ?? "", true);
        form.GetField("ADDRESS_1").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.StreetAddress) ? userApplication.Application.CurrentAddress?.StreetAddress.ToUpper() : "", true);
        form.GetField("CITY").SetValue(!string.IsNullOrEmpty(userApplication.Application.CurrentAddress?.City) ? userApplication.Application.CurrentAddress?.City.ToUpper() : "", true);
        form.GetField("STATE").SetValue(GetStateByName(userApplication.Application.CurrentAddress?.State) ?? "", true);
        form.GetField("ZIP").SetValue(userApplication.Application.CurrentAddress?.Zip ?? "", true);
        form.GetField("DOJ").SetValue("true");
        form.GetField("FBI").SetValue("true");
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

        var fullFileName = BuildApplicantDocumentName(userApplication, "Signature");
        var imageBinaryData = await _documentService.GetApplicantImageAsync(fullFileName, cancellationToken: default);

        var imageData = ImageDataFactory.Create(imageBinaryData);

        var pageSeventeenPositionOne = new ImagePosition()
        {
            Page = 17,
            Width = 200,
            Height = 22,
            Left = 175,
            Bottom = 370
        };

        var pageSeventeenImageOne = GetImageForImageData(imageData, pageSeventeenPositionOne);
        mainDocument.Add(pageSeventeenImageOne);

        var pageSeventeenPositionTwo = new ImagePosition()
        {
            Page = 17,
            Width = 200,
            Height = 22,
            Left = 175,
            Bottom = 118
        };

        var pageSeventeenImageTwo = GetImageForImageData(imageData, pageSeventeenPositionTwo);
        mainDocument.Add(pageSeventeenImageTwo);
    }

    private async Task AddApplicantSignatureImageForLegacyApplication(PermitApplication userApplication, Document mainDocument)
    {

        var fullFileName = BuildApplicantDocumentName(userApplication, "Signature");
        var imageBinaryData = await _documentService.GetApplicantImageAsync(fullFileName, cancellationToken: default);

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

        var pageSeventeenPositionOne = new ImagePosition()
        {
            Page = 17,
            Width = 200,
            Height = 22,
            Left = 108,
            Bottom = 325
        };

        var pageSeventeenImageOne = GetImageForImageData(imageData, pageSeventeenPositionOne);
        mainDocument.Add(pageSeventeenImageOne);

        var pageSeventeenPositionTwo = new ImagePosition()
        {
            Page = 17,
            Width = 200,
            Height = 22,
            Left = 108,
            Bottom = 75
        };

        var pageSeventeenImageTwo = GetImageForImageData(imageData, pageSeventeenPositionTwo);
        mainDocument.Add(pageSeventeenImageTwo);
    }

    private async Task AddProcessorsSignatureImageForLegacyApplication(string processorUserName, Document mainDocument)
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
        var personalInfo = userApplication.Application.PersonalInfo;
        return string.Join(" ", new[]
        {
        personalInfo?.LastName + ",",
        personalInfo?.FirstName,
        personalInfo?.MiddleName,
        personalInfo?.Suffix
        }.Where(name => !string.IsNullOrWhiteSpace(name)));
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

    private void AddLegacyAppendixPage(string header, string content, PdfAcroForm form, PdfDocument pdfDoc, bool userBorder = false)
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
        var font = form.GetField("form1[0].#subform[3].VIOLATION[1]").GetFont();

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
            Width = 177,
            Height = 14,
            Left = 185,
            Bottom = 17
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var secondPosition = new ImagePosition()
        {
            Page = 3,
            Width = 200,
            Height = 25,
            Left = 395,
            Bottom = 485
        };

        var secondCard = GetImageForImageData(imageData, secondPosition);
        mainDocument.Add(secondCard);

        var addendumPosition = new ImagePosition()
        {
            Page = 4,
            Width = 177,
            Height = 14,
            Left = 185,
            Bottom = 17
        };

        var addendumImage = GetImageForImageData(imageData, addendumPosition);
        mainDocument.Add(addendumImage);
    }

    private async Task AddApplicantSignatureImageForOfficial(PermitApplication userApplication, Document mainDocument)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "Signature");
        var imageData = await GetImageData(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 75,
            Height = 20,
            Left = 2,
            Bottom = 17
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 3,
            Width = 100,
            Height = 20,
            Left = 120,
            Bottom = 485
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);

        var addendumPosition= new ImagePosition()
        {
            Page = 4,
            Width = 75,
            Height = 20,
            Left = 2,
            Bottom = 17
        };

        var addendumImage = GetImageForImageData(imageData, addendumPosition);
        mainDocument.Add(addendumImage);
    }

    private async Task AddApplicantSignatureImageForLiveScan(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "Signature");
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
        var thumbprintFileName = BuildApplicantDocumentName(userApplication, "Thumbprint");
        var imageData = await GetImageData(thumbprintFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 2,
            Width = 60,
            Height = 60,
            Left = 12,
            Bottom = 8
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 3,
            Width = 150,
            Height = 90,
            Left = 135,
            Bottom = 215
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);
    }

    private async Task AddApplicantPhotoImageForOfficial(PermitApplication userApplication, Document mainDocument)
    {
        var portraitFileName = BuildApplicantDocumentName(userApplication, "Portrait");
        var imageData = await GetImageData(portraitFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 55,
            Height = 80,
            Left = 7,
            Bottom = 34
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        mainDocument.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 3,
            Width = 100,
            Height = 115,
            Left = 115,
            Bottom = 505
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        mainDocument.Add(rightImage);

        var addendumPosition = new ImagePosition()
        {
            Page = 4,
            Width = 55,
            Height = 80,
            Left = 7,
            Bottom = 34
        };

        var addendumImage = GetImageForImageData(imageData, addendumPosition);
        mainDocument.Add(addendumImage);
    }

    private async Task AddApplicantSignatureImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "Signature");
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

    private async Task AddApplicantSignatureImageForModification(PermitApplication userApplication, Document docFileAll)
    {
        var signatureFileName = BuildApplicantDocumentName(userApplication, "Signature");
        var imageData = await GetImageData(signatureFileName);

        var leftPosition = new ImagePosition()
        {
            Page = 2,
            Width = 185,
            Height = 40,
            Left = 110,
            Bottom = 117
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);
    }

    private async Task AddApplicantThumbprintImageForUnOfficial(PermitApplication userApplication, Document docFileAll)
    {
        var thumbprintFileName = BuildApplicantDocumentName(userApplication, "Thumbprint");
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
        var portraitFileName = BuildApplicantDocumentName(userApplication, "Portrait");
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

    private async Task AddSheriffLogoForOfficial(Document docFileAll)
    {
        var streamContent = await _documentService.GetSheriffLogoAsync(cancellationToken: default);

        var memoryStream = new MemoryStream();
        await streamContent.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var imageData = ImageDataFactory.Create(memoryStream.ToArray());

        var leftPosition = new ImagePosition()
        {
            Page = 1,
            Width = 22,
            Height = 46,
            Left = 2,
            Bottom = 126
        };

        var leftImage = GetImageForImageData(imageData, leftPosition);
        docFileAll.Add(leftImage);

        var rightPosition = new ImagePosition()
        {
            Page = 3,
            Width = 35,
            Height = 75,
            Left = 115,
            Bottom = 660
        };

        var rightImage = GetImageForImageData(imageData, rightPosition);
        docFileAll.Add(rightImage);

        var addendumPosition = new ImagePosition()
        {
            Page = 4,
            Width = 22,
            Height = 46,
            Left = 2,
            Bottom = 126
        };

        var addendumImage = GetImageForImageData(imageData, addendumPosition);
        docFileAll.Add(addendumImage);
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

    private string BuildApplicantDocumentName(PermitApplication userApplication, string documentType)
    {
        var matchingDocument = userApplication.Application.UploadedDocuments.FirstOrDefault(doc => doc.DocumentType == documentType);

        string fullFileName = $"{userApplication.UserId}_{matchingDocument.Name}";

        return fullFileName;
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
