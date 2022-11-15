using CCW.Application.Entities;
using CCW.Application.Mappers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Reflection;
using Fare;
using System.Diagnostics.Metrics;
using CCW.Application.Models;

namespace CCW.Application.Tests;

internal class MapperTests
{
    internal class EntityToPermitApplicationResponseMapperTests
    {
        //private readonly Mock<IMapper<PermitApplication, Entities.Application>> _applicationMapperMock;
        //private readonly Mock<IMapper<PermitApplication, History[]>> _historyMapperMock;

        //private EntityToPermitApplicationResponseMapperTests()
        //{
        //    _applicationMapperMock = new Mock<IMapper<PermitApplication, Entities.Application>>();
        //    _historyMapperMock = new Mock<IMapper<PermitApplication, History[]>>();
        //}

        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            [Frozen] History[] history,
            [Frozen] Entities.Application application,
            [Frozen] PermitApplication permitApplication,
            EntityToPermitApplicationResponseMapper sut
        )
        {
            var request = new Fixture().Build<PermitApplication>()
                .With(x => x.Application, application)
                .With(x => x.History, history)
                .Create();

            var _applicationMapperMock = new Mock<IMapper<PermitApplication, Entities.Application>>();
            var _historyMapperMock = new Mock<IMapper<PermitApplication, History[]>>();

            _applicationMapperMock.Setup(x => x.Map(permitApplication)).Returns(permitApplication.Application);
            _historyMapperMock.Setup(x => x.Map(permitApplication)).Returns(permitApplication.History);

            var result = sut.Map(permitApplication);

            result.Application.Should().BeEquivalentTo(application);
            result.Id.Should().Be(permitApplication.Id);
            result.History.Should().BeEquivalentTo(permitApplication.History);
        }
    }

    internal class EntityToSummarizedPermitApplicationModelMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            SummarizedPermitApplication request,
            EntityToSummarizedPermitApplicationModelMapper sut
        )
        {
            var result = sut.Map(request);

            result.FirstName.Should().Be(request.FirstName);
            result.LastName.Should().Be(request.LastName);
            result.Address.Should().BeEquivalentTo(request.CurrentAddress);
            result.Status.Should().Be(Convert.ToInt16(request.Status));
            result.ApplicationID.Should().Be(request.id);
            result.AppointmentStatus.Should().Be(request.AppointmentStatus);
            result.Email.Should().Be(request.UserEmail);
            result.OrderID.Should().Be(request.OrderId);
        }
    }

    internal class PermitApplicationToAddressMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToAddressMapper sut
        )
        {
            var result = sut.Map(request);

            result.AddressLine1.Should().Be(request.Application.CurrentAddress?.AddressLine1);
            result.AddressLine2.Should().Be(request.Application.CurrentAddress?.AddressLine2);
            result.City.Should().Be(request.Application.CurrentAddress?.City);
            result.County.Should().Be(request.Application.CurrentAddress?.County);
            result.State.Should().Be(request.Application.CurrentAddress?.State);
            result.Zip.Should().Be(request.Application.CurrentAddress?.Zip);
            result.Country.Should().Be(request.Application.CurrentAddress?.Country);
        }
    }

    internal class PermitApplicationToAliasMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToAliasMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.Application.Aliases?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.Application.Aliases?[i]);
            }
        }
    }

    internal class PermitApplicationToApplicationMapperTests
    {
        private readonly Mock<IMapper<PermitApplication, Alias[]>> _aliasMapper;
        private readonly Mock<IMapper<PermitApplication, Address>> _addressMapper;
        private readonly Mock<IMapper<PermitApplication, Citizenship>> _citizenshipMapper;
        private readonly Mock<IMapper<PermitApplication, Contact>> _contactMapper;
        private readonly Mock<IMapper<PermitApplication, DOB>> _dobMapper;
        private readonly Mock<IMapper<PermitApplication, IdInfo>> _idInfoMapper;
        private readonly Mock<IMapper<PermitApplication, PhysicalAppearance>> _physicalAppearanceMapper;
        private readonly Mock<IMapper<PermitApplication, License>> _licenseMapper;
        private readonly Mock<IMapper<PermitApplication, SpouseInformation>> _spouseInfoMapper;
        private readonly Mock<IMapper<PermitApplication, WorkInformation>> _workInfoMapper;
        private readonly Mock<IMapper<PermitApplication, PersonalInfo>> _personalInfoMapper;
        private readonly Mock<IMapper<PermitApplication, MailingAddress?>> _mailingAddressMapper;
        private readonly Mock<IMapper<PermitApplication, Address[]>> _previousAddressMapper;
        private readonly Mock<IMapper<PermitApplication, ImmigrantInformation>> _immigrationMapper;
        private readonly Mock<IMapper<PermitApplication, SpouseAddressInformation>> _spouseAddressInfoMapper;
        private readonly Mock<IMapper<PermitApplication, Weapon[]>> _weaponMapper;
        private readonly Mock<IMapper<PermitApplication, QualifyingQuestions>> _qualifyingQuestionsMapper;
        private readonly Mock<IMapper<PermitApplication, UploadedDocument[]>> _uploadedDocMapper;

        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToApplicationMapper sut
        )
        {
            //TODO: OP fix test
            var result = sut.Map(request);

           // result.Aliases.Should().BeEquivalentTo(_aliasMapper.Map(request));
            //result.ApplicationType = source.Application.ApplicationType,
            //result.CurrentAddress = MapIfNotNull(source.Application.CurrentAddress, () => _addressMapper.Map(source)),
            //result.Citizenship = MapIfNotNull(source.Application.Citizenship, () => _citizenshipMapper.Map(source)),
            //result.Contact = MapIfNotNull(source.Application.Contact, () => _contactMapper.Map(source)),
            //result.DOB = MapIfNotNull(source.Application.DOB, () => _dobMapper.Map(source)),
            //result.Employment = source.Application.Employment,
            //result.IdInfo = MapIfNotNull(source.Application.IdInfo, () => _idInfoMapper.Map(source)),
            //result.PhysicalAppearance = MapIfNotNull(source.Application.PhysicalAppearance, () => _physicalAppearanceMapper.Map(source)),
            //result.License = MapIfNotNull(source.Application.License, () => _licenseMapper.Map(source)),
            //result.DifferentMailing = source.Application.DifferentMailing,
            //result.DifferentSpouseAddress = source.Application.DifferentSpouseAddress,
            //result.IsComplete = source.Application.IsComplete,
            //result.ImmigrantInformation = MapIfNotNull(source.Application.ImmigrantInformation, () => _immigrationMapper.Map(source)),
            //result.SpouseInformation = MapIfNotNull(source.Application.SpouseInformation, () => _spouseInfoMapper.Map(source)),
            //result.WorkInformation = MapIfNotNull(source.Application.WorkInformation, () => _workInfoMapper.Map(source)),
            //result.PersonalInfo = MapIfNotNull(source.Application.PersonalInfo, () => _personalInfoMapper.Map(source)),
            //result.MailingAddress = MapIfNotNull(source.Application.MailingAddress, () => _mailingAddressMapper.Map(source)),
            //result.PreviousAddresses = MapIfNotNull(source.Application.PreviousAddresses, () => _previousAddressMapper.Map(source)),
            //result.SpouseAddressInformation = MapIfNotNull(source.Application.SpouseAddressInformation, () => _spouseAddressInfoMapper.Map(source)),
            //result.UserEmail = source.Application.UserEmail,
            //result.Weapons = MapIfNotNull(source.Application.Weapons, () => _weaponMapper.Map(source)),
            //result.QualifyingQuestions = MapIfNotNull(source.Application.QualifyingQuestions, () => _qualifyingQuestionsMapper.Map(source)),
            //result.CurrentStep = source.Application.CurrentStep,
            //result.AppointmentStatus = source.Application.AppointmentStatus,
            //result.Status = source.Application.Status,
            //result.OrderId = source.Application.OrderId,
            //result.UploadedDocuments = MapIfNotNull(source.Application.UploadedDocuments, () => _uploadedDocMapper.Map(source)),
        }
    }

    internal class PermitApplicationToCitizenshipMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToCitizenshipMapper sut
        )
        {
            var result = sut.Map(request);

            result.Citizen.Should().Be(request.Application.Citizenship.Citizen);
            result.MilitaryStatus.Should().Be(request.Application.Citizenship.MilitaryStatus);
        }
    }

    internal class PermitApplicationToContactMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToContactMapper sut
        )
        {
            var result = sut.Map(request);

            result.PrimaryPhoneNumber.Should().Be(request.Application.Contact?.PrimaryPhoneNumber);
            result.CellPhoneNumber.Should().Be(request.Application.Contact?.CellPhoneNumber);
            result.WorkPhoneNumber.Should().Be(request.Application.Contact?.WorkPhoneNumber);
            result.FaxPhoneNumber.Should().Be(request.Application.Contact?.FaxPhoneNumber);
            result.TextMessageUpdates.Should().Be(request.Application.Contact.TextMessageUpdates);
        }
    }

    internal class PermitApplicationToDOBMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToDOBMapper sut
        )
        {
            var result = sut.Map(request);

            result.BirthDate.Should().Be(request.Application.DOB.BirthDate);
            result.BirthCity.Should().Be(request.Application.DOB.BirthCity);
            result.BirthState.Should().Be(request.Application.DOB.BirthState);
            result.BirthCountry.Should().Be(request.Application.DOB.BirthCountry);
        }
    }

    internal class PermitApplicationToHistoryMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToHistoryMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.History?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.History?[i]);
            }
        }
    }

    internal class PermitApplicationToIdInfoMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToIdInfoMapper sut
        )
        {
            var result = sut.Map(request);

            result.IdNumber.Should().Be(request.Application.IdInfo?.IdNumber);
            result.IssuingState.Should().Be(request.Application.IdInfo?.IssuingState);
        }
    }

    internal class PermitApplicationToImmigrantInformationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToImmigrantInformationMapper sut
        )
        {
            var result = sut.Map(request);

            result.CountryOfBirth.Should().Be(request.Application.ImmigrantInformation?.CountryOfBirth);
            result.CountryOfCitizenship.Should().Be(request.Application.ImmigrantInformation?.CountryOfCitizenship);
            result.ImmigrantAlien.Should().Be(request.Application.ImmigrantInformation?.ImmigrantAlien);
            result.NonImmigrantAlien.Should().Be(request.Application.ImmigrantInformation?.NonImmigrantAlien);
        }
    }

    internal class PermitApplicationToLicenseMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToLicenseMapper sut
        )
        {
            var result = sut.Map(request);

            result.ExpirationDate.Should().Be(request.Application.License.ExpirationDate);
            result.IssuingCounty.Should().Be(request.Application.License.IssuingCounty);
            result.PermitNumber.Should().Be(request.Application.License.PermitNumber);
        }
    }

    internal class PermitApplicationToMailingAddressMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToMailingAddressMapper sut
        )
        {
            var result = sut.Map(request);

            result.AddressLine1.Should().Be(request.Application.MailingAddress.AddressLine1);
            result.AddressLine2.Should().Be(request.Application.MailingAddress.AddressLine2);
            result.City.Should().Be(request.Application.MailingAddress.City);
            result.County.Should().Be(request.Application.MailingAddress.County);
            result.State.Should().Be(request.Application.MailingAddress.State);
            result.Zip.Should().Be(request.Application.MailingAddress.Zip);
            result.Country.Should().Be(request.Application.MailingAddress.Country);
        }
    }

    internal class PermitApplicationToPersonalInfoMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToPersonalInfoMapper sut
        )
        {
            var result = sut.Map(request);

            result.LastName.Should().Be(request.Application.PersonalInfo.LastName);
            result.FirstName.Should().Be(request.Application.PersonalInfo.FirstName);
            result.MiddleName.Should().Be(request.Application.PersonalInfo.MiddleName);
            result.NoMiddleName.Should().Be(request.Application.PersonalInfo.NoMiddleName);
            result.MaidenName.Should().Be(request.Application.PersonalInfo.MaidenName);
            result.Suffix.Should().Be(request.Application.PersonalInfo.Suffix);
            result.Ssn.Should().Be(request.Application.PersonalInfo.Ssn);
            result.MaritalStatus.Should().Be(request.Application.PersonalInfo.MaritalStatus);
        }
    }

    internal class PermitApplicationToPhysicalAppearanceMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToPhysicalAppearanceMapper sut
        )
        {
            var result = sut.Map(request);

            result.Gender.Should().Be(request.Application.PhysicalAppearance?.Gender);
            result.HeightFeet.Should().Be(request.Application.PhysicalAppearance?.HeightFeet);
            result.HeightInch.Should().Be(request.Application.PhysicalAppearance?.HeightInch);
            result.Weight.Should().Be(request.Application.PhysicalAppearance?.Weight);
            result.HairColor.Should().Be(request.Application.PhysicalAppearance?.HairColor);
            result.EyeColor.Should().Be(request.Application.PhysicalAppearance?.EyeColor);
            result.PhysicalDesc.Should().Be(request.Application.PhysicalAppearance?.PhysicalDesc);
        }
    }

    internal class PermitApplicationToPreviousAddressesMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToPreviousAddressesMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.Application.PreviousAddresses?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.Application.PreviousAddresses?[i]);
            }
        }
    }

    internal class PermitApplicationToQualifyingQuestionsMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToQualifyingQuestionsMapper sut
        )
        {
            var result = sut.Map(request);

            result.QuestionOne.Should().Be(request.Application.QualifyingQuestions.QuestionOne);
            result.QuestionOneExp.Should().Be(request.Application.QualifyingQuestions.QuestionOneExp);
            result.QuestionTwo.Should().Be(request.Application.QualifyingQuestions.QuestionTwo);
            result.QuestionTwoExp.Should().Be(request.Application.QualifyingQuestions.QuestionTwoExp);
            result.QuestionThree.Should().Be(request.Application.QualifyingQuestions.QuestionThree);
            result.QuestionThreeExp.Should().Be(request.Application.QualifyingQuestions.QuestionThreeExp);
            result.QuestionFour.Should().Be(request.Application.QualifyingQuestions.QuestionFour);
            result.QuestionFourExp.Should().Be(request.Application.QualifyingQuestions.QuestionFourExp);
            result.QuestionFive.Should().Be(request.Application.QualifyingQuestions.QuestionFive);
            result.QuestionFiveExp.Should().Be(request.Application.QualifyingQuestions.QuestionFiveExp);
            result.QuestionSix.Should().Be(request.Application.QualifyingQuestions.QuestionSix);
            result.QuestionSixExp.Should().Be(request.Application.QualifyingQuestions.QuestionSixExp);
            result.QuestionSeven.Should().Be(request.Application.QualifyingQuestions.QuestionSeven);
            result.QuestionSevenExp.Should().Be(request.Application.QualifyingQuestions.QuestionSevenExp);
            result.QuestionEight.Should().Be(request.Application.QualifyingQuestions.QuestionEight);
            result.QuestionEightExp.Should().Be(request.Application.QualifyingQuestions.QuestionEightExp);
            result.QuestionNine.Should().Be(request.Application.QualifyingQuestions.QuestionNine);
            result.QuestionNineExp.Should().Be(request.Application.QualifyingQuestions.QuestionNineExp);
            result.QuestionTen.Should().Be(request.Application.QualifyingQuestions.QuestionTen);
            result.QuestionTenExp.Should().Be(request.Application.QualifyingQuestions.QuestionTenExp);
            result.QuestionEleven.Should().Be(request.Application.QualifyingQuestions.QuestionEleven);
            result.QuestionElevenExp.Should().Be(request.Application.QualifyingQuestions.QuestionElevenExp);
            result.QuestionTwelve.Should().Be(request.Application.QualifyingQuestions.QuestionTwelve);
            result.QuestionTwelveExp.Should().Be(request.Application.QualifyingQuestions.QuestionTwelveExp);
            result.QuestionThirteen.Should().Be(request.Application.QualifyingQuestions.QuestionThirteen);
            result.QuestionThirteenExp.Should().Be(request.Application.QualifyingQuestions.QuestionThirteenExp);
            result.QuestionFourteen.Should().Be(request.Application.QualifyingQuestions.QuestionFourteen);
            result.QuestionFourteenExp.Should().Be(request.Application.QualifyingQuestions.QuestionFourteenExp);
            result.QuestionFifteen.Should().Be(request.Application.QualifyingQuestions.QuestionFifteen);
            result.QuestionFifteenExp.Should().Be(request.Application.QualifyingQuestions.QuestionFifteenExp);
            result.QuestionSixteen.Should().Be(request.Application.QualifyingQuestions.QuestionSixteen);
            result.QuestionSixteenExp.Should().Be(request.Application.QualifyingQuestions.QuestionSixteenExp);
            result.QuestionSeventeen.Should().Be(request.Application.QualifyingQuestions.QuestionSeventeen);
            result.QuestionSeventeenExp.Should().Be(request.Application.QualifyingQuestions.QuestionSeventeenExp);
        }
    }

    internal class PermitApplicationToSpouseAddressInformationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToSpouseAddressInformationMapper sut
        )
        {
            var result = sut.Map(request);

            result.AddressLine1.Should().Be(request.Application.SpouseAddressInformation.AddressLine1);
            result.AddressLine2.Should().Be(request.Application.SpouseAddressInformation.AddressLine2);
            result.City.Should().Be(request.Application.SpouseAddressInformation.City);
            result.County.Should().Be(request.Application.SpouseAddressInformation.County);
            result.State.Should().Be(request.Application.SpouseAddressInformation.State);
            result.Zip.Should().Be(request.Application.SpouseAddressInformation.Zip);
            result.Country.Should().Be(request.Application.SpouseAddressInformation.Country);
        }
    }

    internal class PermitApplicationToSpouseInformationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToSpouseInformationMapper sut
        )
        {
            var result = sut.Map(request);

            result.FirstName.Should().Be(request.Application.SpouseInformation.FirstName);
            result.LastName.Should().Be(request.Application.SpouseInformation.LastName);
            result.MaidenName.Should().Be(request.Application.SpouseInformation.MaidenName);
            result.MiddleName.Should().Be(request.Application.SpouseInformation.MiddleName);
        }
    }

    internal class PermitApplicationToUploadDocumentMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToUploadDocumentMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.Application.UploadedDocuments?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.Application.UploadedDocuments?[i]);
            }
        }
    }

    internal class PermitApplicationToWeaponMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToWeaponMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.Application.Weapons?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.Application.Weapons?[i]);
            }
        }
    }

    internal class PermitApplicationToWorkInformationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplication request,
            PermitApplicationToWorkInformationMapper sut
        )
        {
            var result = sut.Map(request);

            result.EmployerAddressLine1.Should().Be(request.Application.WorkInformation?.EmployerAddressLine1);
            result.EmployerAddressLine2.Should().Be(request.Application.WorkInformation?.EmployerAddressLine2);
            result.EmployerCity.Should().Be(request.Application.WorkInformation?.EmployerCity);
            result.EmployerCountry.Should().Be(request.Application.WorkInformation?.EmployerCountry);
            result.EmployerPhone.Should().Be(request.Application.WorkInformation?.EmployerPhone);
            result.EmployerName.Should().Be(request.Application.WorkInformation?.EmployerName);
            result.EmployerState.Should().Be(request.Application.WorkInformation?.EmployerState);
            result.EmployerZip.Should().Be(request.Application.WorkInformation?.EmployerZip);
            result.Occupation.Should().Be(request.Application.WorkInformation?.Occupation);
        }
    }

    internal class PermitRequestApplicationToApplicationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            PermitRequestApplicationToApplicationMapper sut
        )
        {
            //TODO: OP fix test
            var result = sut.Map(request);

            //Aliases = MapIfNotNull(source.Application.Aliases, () => _aliasMapper.Map(source)),
            //ApplicationType = source.Application.ApplicationType,
            //CurrentAddress = MapIfNotNull(source.Application.CurrentAddress, () => _addressMapper.Map(source)),
            //Citizenship = MapIfNotNull(source.Application.Citizenship, () => _citizenshipMapper.Map(source)),
            //Contact = MapIfNotNull(source.Application.Contact, () => _contactMapper.Map(source)),
            //DOB = MapIfNotNull(source.Application.DOB, () => _dobMapper.Map(source)),
            //Employment = source.Application.Employment,
            //IdInfo = MapIfNotNull(source.Application.IdInfo, () => _idInfoMapper.Map(source)),
            //PhysicalAppearance = MapIfNotNull(source.Application.PhysicalAppearance, () => _physicalAppearanceMapper.Map(source)),
            //License = MapIfNotNull(source.Application.License, () => _licenseMapper.Map(source)),
            //DifferentMailing = source.Application.DifferentMailing,
            //DifferentSpouseAddress = source.Application.DifferentSpouseAddress,
            //IsComplete = source.Application.IsComplete,
            //ImmigrantInformation = MapIfNotNull(source.Application.ImmigrantInformation, () => _immigrationMapper.Map(source)),
            //SpouseInformation = MapIfNotNull(source.Application.SpouseInformation, () => _spouseInfoMapper.Map(source)),
            //WorkInformation = MapIfNotNull(source.Application.WorkInformation, () => _workInfoMapper.Map(source)),
            //PersonalInfo = MapIfNotNull(source.Application.PersonalInfo, () => _personalInfoMapper.Map(source)),
            //MailingAddress = MapIfNotNull(source.Application.MailingAddress, () => _mailingAddressMapper.Map(source)),
            //PreviousAddresses = MapIfNotNull(source.Application.PreviousAddresses, () => _previousAddressMapper.Map(source)),
            //SpouseAddressInformation = MapIfNotNull(source.Application.SpouseAddressInformation, () => _spouseAddressInfoMapper.Map(source)),
            //UserEmail = source.Application.UserEmail,
            //Weapons = MapIfNotNull(source.Application.Weapons, () => _weaponMapper.Map(source)),
            //QualifyingQuestions = MapIfNotNull(source.Application.QualifyingQuestions, () => _qualifyingQuestionsMapper.Map(source)),
            //CurrentStep = source.Application.CurrentStep,
            //AppointmentStatus = source.Application.AppointmentStatus,
            //Status = source.Application.Status,
            //OrderId = source.Application.OrderId,
            //UploadedDocuments = MapIfNotNull(source.Application.UploadedDocuments, () => _uploadedDocMapper.Map(source)),
        }
    }

    internal class RequestPermitApplicationModelToEntityMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            bool isNewApplication,
            PermitApplicationRequestModel request,
            RequestPermitApplicationModelToEntityMapper sut
        )
        {
            //TODO: OP fix test
            var result = sut.Map(isNewApplication, request);

            //Aliases = MapIfNotNull(source.Application.Aliases, () => _aliasMapper.Map(source)),
            //ApplicationType = source.Application.ApplicationType,
            //CurrentAddress = MapIfNotNull(source.Application.CurrentAddress, () => _addressMapper.Map(source)),
            //Citizenship = MapIfNotNull(source.Application.Citizenship, () => _citizenshipMapper.Map(source)),
            //Contact = MapIfNotNull(source.Application.Contact, () => _contactMapper.Map(source)),
            //DOB = MapIfNotNull(source.Application.DOB, () => _dobMapper.Map(source)),
            //Employment = source.Application.Employment,
            //IdInfo = MapIfNotNull(source.Application.IdInfo, () => _idInfoMapper.Map(source)),
            //PhysicalAppearance = MapIfNotNull(source.Application.PhysicalAppearance, () => _physicalAppearanceMapper.Map(source)),
            //License = MapIfNotNull(source.Application.License, () => _licenseMapper.Map(source)),
            //DifferentMailing = source.Application.DifferentMailing,
            //DifferentSpouseAddress = source.Application.DifferentSpouseAddress,
            //IsComplete = source.Application.IsComplete,
            //ImmigrantInformation = MapIfNotNull(source.Application.ImmigrantInformation, () => _immigrationMapper.Map(source)),
            //SpouseInformation = MapIfNotNull(source.Application.SpouseInformation, () => _spouseInfoMapper.Map(source)),
            //WorkInformation = MapIfNotNull(source.Application.WorkInformation, () => _workInfoMapper.Map(source)),
            //PersonalInfo = MapIfNotNull(source.Application.PersonalInfo, () => _personalInfoMapper.Map(source)),
            //MailingAddress = MapIfNotNull(source.Application.MailingAddress, () => _mailingAddressMapper.Map(source)),
            //PreviousAddresses = MapIfNotNull(source.Application.PreviousAddresses, () => _previousAddressMapper.Map(source)),
            //SpouseAddressInformation = MapIfNotNull(source.Application.SpouseAddressInformation, () => _spouseAddressInfoMapper.Map(source)),
            //UserEmail = source.Application.UserEmail,
            //Weapons = MapIfNotNull(source.Application.Weapons, () => _weaponMapper.Map(source)),
            //QualifyingQuestions = MapIfNotNull(source.Application.QualifyingQuestions, () => _qualifyingQuestionsMapper.Map(source)),
            //CurrentStep = source.Application.CurrentStep,
            //AppointmentStatus = source.Application.AppointmentStatus,
            //Status = source.Application.Status,
            //OrderId = source.Application.OrderId,
            //UploadedDocuments = MapIfNotNull(source.Application.UploadedDocuments, () => _uploadedDocMapper.Map(source)),
        }
    }

    internal class RequestPermitApplicationToAddressMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToAddressMapper sut
        )
        {
            var result = sut.Map(request);

            result.AddressLine1.Should().Be(request.Application.CurrentAddress?.AddressLine1);
            result.AddressLine2.Should().Be(request.Application.CurrentAddress?.AddressLine2);
            result.City.Should().Be(request.Application.CurrentAddress?.City);
            result.County.Should().Be(request.Application.CurrentAddress?.County);
            result.State.Should().Be(request.Application.CurrentAddress?.State);
            result.Zip.Should().Be(request.Application.CurrentAddress?.Zip);
            result.Country.Should().Be(request.Application.CurrentAddress?.Country);
        }
    }

    internal class RequestPermitApplicationToAliasMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToAliasMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.Application.Aliases?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.Application.Aliases?[i]);
            }
        }
    }

    internal class RequestPermitApplicationToCitizenshipMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToCitizenshipMapper sut
        )
        {
            var result = sut.Map(request);

            result.Citizen.Should().Be(request.Application.Citizenship.Citizen);
            result.MilitaryStatus.Should().Be(request.Application.Citizenship.MilitaryStatus);
        }
    }

    internal class RequestPermitApplicationToContactMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToContactMapper sut
        )
        {
            var result = sut.Map(request);

            result.PrimaryPhoneNumber.Should().Be(request.Application.Contact?.PrimaryPhoneNumber);
            result.CellPhoneNumber.Should().Be(request.Application.Contact?.CellPhoneNumber);
            result.WorkPhoneNumber.Should().Be(request.Application.Contact?.WorkPhoneNumber);
            result.FaxPhoneNumber.Should().Be(request.Application.Contact?.FaxPhoneNumber);
            result.TextMessageUpdates.Should().Be(request.Application.Contact.TextMessageUpdates);
        }
    }

    internal class RequestPermitApplicationToDOBMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToDOBMapper sut
        )
        {
            var result = sut.Map(request);

            result.BirthDate.Should().Be(request.Application.DOB.BirthDate);
            result.BirthCity.Should().Be(request.Application.DOB.BirthCity);
            result.BirthState.Should().Be(request.Application.DOB.BirthState);
            result.BirthCountry.Should().Be(request.Application.DOB.BirthCountry);
        }
    }

    internal class RequestPermitApplicationToHistoryMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToHistoryMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.History?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.History?[i]);
            }
        }
    }

    internal class RequestPermitApplicationToIdInfoMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToIdInfoMapper sut
        )
        {
            var result = sut.Map(request);

            result.IdNumber.Should().Be(request.Application.IdInfo?.IdNumber);
            result.IssuingState.Should().Be(request.Application.IdInfo?.IssuingState);
        }
    }

    internal class RequestPermitApplicationToImmigrantInformationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToImmigrantInformationMapper sut
        )
        {
            var result = sut.Map(request);

            result.CountryOfBirth.Should().Be(request.Application.ImmigrantInformation?.CountryOfBirth);
            result.CountryOfCitizenship.Should().Be(request.Application.ImmigrantInformation?.CountryOfCitizenship);
            result.ImmigrantAlien.Should().Be(request.Application.ImmigrantInformation?.ImmigrantAlien);
            result.NonImmigrantAlien.Should().Be(request.Application.ImmigrantInformation?.NonImmigrantAlien);
        }
    }


    internal class RequestPermitApplicationToLicenseMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToLicenseMapper sut
        )
        {
            var result = sut.Map(request);

            result.ExpirationDate.Should().Be(request.Application.License.ExpirationDate);
            result.IssuingCounty.Should().Be(request.Application.License.IssuingCounty);
            result.PermitNumber.Should().Be(request.Application.License.PermitNumber);
        }
    }

    internal class RequestPermitApplicationToMailingAddressMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToMailingAddressMapper sut
        )
        {
            var result = sut.Map(request);

            result.AddressLine1.Should().Be(request.Application.MailingAddress.AddressLine1);
            result.AddressLine2.Should().Be(request.Application.MailingAddress.AddressLine2);
            result.City.Should().Be(request.Application.MailingAddress.City);
            result.County.Should().Be(request.Application.MailingAddress.County);
            result.State.Should().Be(request.Application.MailingAddress.State);
            result.Zip.Should().Be(request.Application.MailingAddress.Zip);
            result.Country.Should().Be(request.Application.MailingAddress.Country);
        }
    }

    internal class RequestPermitApplicationToPersonalInfoMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToPersonalInfoMapper sut
        )
        {
            var result = sut.Map(request);

            result.LastName.Should().Be(request.Application.PersonalInfo.LastName);
            result.FirstName.Should().Be(request.Application.PersonalInfo.FirstName);
            result.MiddleName.Should().Be(request.Application.PersonalInfo.MiddleName);
            result.NoMiddleName.Should().Be(request.Application.PersonalInfo.NoMiddleName);
            result.MaidenName.Should().Be(request.Application.PersonalInfo.MaidenName);
            result.Suffix.Should().Be(request.Application.PersonalInfo.Suffix);
            result.Ssn.Should().Be(request.Application.PersonalInfo.Ssn);
            result.MaritalStatus.Should().Be(request.Application.PersonalInfo.MaritalStatus);
        }
    }

    internal class RequestPermitApplicationToPhysicalAppearanceMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToPhysicalAppearanceMapper sut
        )
        {
            var result = sut.Map(request);

            result.Gender.Should().Be(request.Application.PhysicalAppearance?.Gender);
            result.HeightFeet.Should().Be(request.Application.PhysicalAppearance?.HeightFeet);
            result.HeightInch.Should().Be(request.Application.PhysicalAppearance?.HeightInch);
            result.Weight.Should().Be(request.Application.PhysicalAppearance?.Weight);
            result.HairColor.Should().Be(request.Application.PhysicalAppearance?.HairColor);
            result.EyeColor.Should().Be(request.Application.PhysicalAppearance?.EyeColor);
            result.PhysicalDesc.Should().Be(request.Application.PhysicalAppearance?.PhysicalDesc);
        }
    }

    internal class RequestPermitApplicationToPreviousAddressesMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToPreviousAddressesMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.Application.PreviousAddresses?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.Application.PreviousAddresses?[i]);
            }
        }
    }

    internal class RequestPermitApplicationToQualifyingQuestionsMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToQualifyingQuestionsMapper sut
        )
        {
            var result = sut.Map(request);

            result.QuestionOne.Should().Be(request.Application.QualifyingQuestions.QuestionOne);
            result.QuestionOneExp.Should().Be(request.Application.QualifyingQuestions.QuestionOneExp);
            result.QuestionTwo.Should().Be(request.Application.QualifyingQuestions.QuestionTwo);
            result.QuestionTwoExp.Should().Be(request.Application.QualifyingQuestions.QuestionTwoExp);
            result.QuestionThree.Should().Be(request.Application.QualifyingQuestions.QuestionThree);
            result.QuestionThreeExp.Should().Be(request.Application.QualifyingQuestions.QuestionThreeExp);
            result.QuestionFour.Should().Be(request.Application.QualifyingQuestions.QuestionFour);
            result.QuestionFourExp.Should().Be(request.Application.QualifyingQuestions.QuestionFourExp);
            result.QuestionFive.Should().Be(request.Application.QualifyingQuestions.QuestionFive);
            result.QuestionFiveExp.Should().Be(request.Application.QualifyingQuestions.QuestionFiveExp);
            result.QuestionSix.Should().Be(request.Application.QualifyingQuestions.QuestionSix);
            result.QuestionSixExp.Should().Be(request.Application.QualifyingQuestions.QuestionSixExp);
            result.QuestionSeven.Should().Be(request.Application.QualifyingQuestions.QuestionSeven);
            result.QuestionSevenExp.Should().Be(request.Application.QualifyingQuestions.QuestionSevenExp);
            result.QuestionEight.Should().Be(request.Application.QualifyingQuestions.QuestionEight);
            result.QuestionEightExp.Should().Be(request.Application.QualifyingQuestions.QuestionEightExp);
            result.QuestionNine.Should().Be(request.Application.QualifyingQuestions.QuestionNine);
            result.QuestionNineExp.Should().Be(request.Application.QualifyingQuestions.QuestionNineExp);
            result.QuestionTen.Should().Be(request.Application.QualifyingQuestions.QuestionTen);
            result.QuestionTenExp.Should().Be(request.Application.QualifyingQuestions.QuestionTenExp);
            result.QuestionEleven.Should().Be(request.Application.QualifyingQuestions.QuestionEleven);
            result.QuestionElevenExp.Should().Be(request.Application.QualifyingQuestions.QuestionElevenExp);
            result.QuestionTwelve.Should().Be(request.Application.QualifyingQuestions.QuestionTwelve);
            result.QuestionTwelveExp.Should().Be(request.Application.QualifyingQuestions.QuestionTwelveExp);
            result.QuestionThirteen.Should().Be(request.Application.QualifyingQuestions.QuestionThirteen);
            result.QuestionThirteenExp.Should().Be(request.Application.QualifyingQuestions.QuestionThirteenExp);
            result.QuestionFourteen.Should().Be(request.Application.QualifyingQuestions.QuestionFourteen);
            result.QuestionFourteenExp.Should().Be(request.Application.QualifyingQuestions.QuestionFourteenExp);
            result.QuestionFifteen.Should().Be(request.Application.QualifyingQuestions.QuestionFifteen);
            result.QuestionFifteenExp.Should().Be(request.Application.QualifyingQuestions.QuestionFifteenExp);
            result.QuestionSixteen.Should().Be(request.Application.QualifyingQuestions.QuestionSixteen);
            result.QuestionSixteenExp.Should().Be(request.Application.QualifyingQuestions.QuestionSixteenExp);
            result.QuestionSeventeen.Should().Be(request.Application.QualifyingQuestions.QuestionSeventeen);
            result.QuestionSeventeenExp.Should().Be(request.Application.QualifyingQuestions.QuestionSeventeenExp);
        }
    }

    internal class RequestPermitApplicationToSpouseAddressInformationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToSpouseAddressInformationMapper sut
        )
        {
            var result = sut.Map(request);

            result.AddressLine1.Should().Be(request.Application.SpouseAddressInformation.AddressLine1);
            result.AddressLine2.Should().Be(request.Application.SpouseAddressInformation.AddressLine2);
            result.City.Should().Be(request.Application.SpouseAddressInformation.City);
            result.County.Should().Be(request.Application.SpouseAddressInformation.County);
            result.State.Should().Be(request.Application.SpouseAddressInformation.State);
            result.Zip.Should().Be(request.Application.SpouseAddressInformation.Zip);
            result.Country.Should().Be(request.Application.SpouseAddressInformation.Country);
        }
    }

    internal class RequestPermitApplicationToSpouseInformationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToSpouseInformationMapper sut
        )
        {
            var result = sut.Map(request);

            result.FirstName.Should().Be(request.Application.SpouseInformation.FirstName);
            result.LastName.Should().Be(request.Application.SpouseInformation.LastName);
            result.MaidenName.Should().Be(request.Application.SpouseInformation.MaidenName);
            result.MiddleName.Should().Be(request.Application.SpouseInformation.MiddleName);
        }
    }

    internal class RequestPermitApplicationToUploadDocumentMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToUploadDocumentMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.Application.UploadedDocuments?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.Application.UploadedDocuments?[i]);
            }
        }
    }

    internal class RequestPermitApplicationToWeaponMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToWeaponMapper sut
        )
        {
            var result = sut.Map(request);

            result.Length.Should().Be(request.Application.Weapons?.Length);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Should().BeEquivalentTo(request.Application.Weapons?[i]);
            }
        }
    }

    internal class RequestPermitApplicationToWorkInformationMapperTests
    {
        [Test]
        [AutoMoqData]
        public void AllValuesMap(
            PermitApplicationRequestModel request,
            RequestPermitApplicationToWorkInformationMapper sut
        )
        {
            var result = sut.Map(request);

            result.EmployerAddressLine1.Should().Be(request.Application.WorkInformation?.EmployerAddressLine1);
            result.EmployerAddressLine2.Should().Be(request.Application.WorkInformation?.EmployerAddressLine2);
            result.EmployerCity.Should().Be(request.Application.WorkInformation?.EmployerCity);
            result.EmployerCountry.Should().Be(request.Application.WorkInformation?.EmployerCountry);
            result.EmployerPhone.Should().Be(request.Application.WorkInformation?.EmployerPhone);
            result.EmployerName.Should().Be(request.Application.WorkInformation?.EmployerName);
            result.EmployerState.Should().Be(request.Application.WorkInformation?.EmployerState);
            result.EmployerZip.Should().Be(request.Application.WorkInformation?.EmployerZip);
            result.Occupation.Should().Be(request.Application.WorkInformation?.Occupation);
        }
    }
}