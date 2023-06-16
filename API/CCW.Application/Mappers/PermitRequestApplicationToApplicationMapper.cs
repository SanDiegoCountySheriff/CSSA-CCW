using CCW.Application.Entities;
using CCW.Application.Enum;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class PermitRequestApplicationToApplicationMapper : IMapper<PermitApplicationRequestModel, Entities.Application>
{
    private readonly IMapper<PermitApplicationRequestModel, Alias[]> _aliasMapper;
    private readonly IMapper<PermitApplicationRequestModel, Address> _addressMapper;
    private readonly IMapper<PermitApplicationRequestModel, Citizenship> _citizenshipMapper;
    private readonly IMapper<PermitApplicationRequestModel, Contact> _contactMapper;
    private readonly IMapper<PermitApplicationRequestModel, DOB> _dobMapper;
    private readonly IMapper<PermitApplicationRequestModel, IdInfo> _idInfoMapper;
    private readonly IMapper<PermitApplicationRequestModel, PhysicalAppearance> _physicalAppearanceMapper;
    private readonly IMapper<PermitApplicationRequestModel, License> _licenseMapper;
    private readonly IMapper<PermitApplicationRequestModel, SpouseInformation> _spouseInfoMapper;
    private readonly IMapper<PermitApplicationRequestModel, WorkInformation> _workInfoMapper;
    private readonly IMapper<PermitApplicationRequestModel, PersonalInfo> _personalInfoMapper;
    private readonly IMapper<PermitApplicationRequestModel, MailingAddress?> _mailingAddressMapper;
    private readonly IMapper<PermitApplicationRequestModel, Address[]> _previousAddressMapper;
    private readonly IMapper<PermitApplicationRequestModel, ImmigrantInformation> _immigrationMapper;
    private readonly IMapper<PermitApplicationRequestModel, SpouseAddressInformation> _spouseAddressInfoMapper;
    private readonly IMapper<PermitApplicationRequestModel, Weapon[]> _weaponMapper;
    private readonly IMapper<PermitApplicationRequestModel, QualifyingQuestions> _qualifyingQuestionsMapper;
    private readonly IMapper<PermitApplicationRequestModel, UploadedDocument[]> _uploadedDocMapper;
    private readonly IMapper<PermitApplicationRequestModel, BackgroundCheck> _backgroundCheckMapper;

    public PermitRequestApplicationToApplicationMapper(
            IMapper<PermitApplicationRequestModel, Alias[]> aliasMapper,
            IMapper<PermitApplicationRequestModel, Address> addressMapper,
            IMapper<PermitApplicationRequestModel, Citizenship> citizenshipMapper,
            IMapper<PermitApplicationRequestModel, Contact> contactMapper,
            IMapper<PermitApplicationRequestModel, DOB> dobMapper,
            IMapper<PermitApplicationRequestModel, IdInfo> idInfoMapper,
            IMapper<PermitApplicationRequestModel, PhysicalAppearance> physicalAppearanceMapper,
            IMapper<PermitApplicationRequestModel, License> licenseMapper,
            IMapper<PermitApplicationRequestModel, SpouseInformation> spouseInfoMapper,
            IMapper<PermitApplicationRequestModel, WorkInformation> workInfoMapper,
            IMapper<PermitApplicationRequestModel, PersonalInfo> personalInfoMapper,
            IMapper<PermitApplicationRequestModel, MailingAddress?> mailingAddressMapper,
            IMapper<PermitApplicationRequestModel, Address[]> previousAddressMapper,
            IMapper<PermitApplicationRequestModel, ImmigrantInformation> immigrationMapper,
            IMapper<PermitApplicationRequestModel, SpouseAddressInformation> spouseAddressInfoMapper,
            IMapper<PermitApplicationRequestModel, Weapon[]> weaponMapper,
            IMapper<PermitApplicationRequestModel, QualifyingQuestions> qualifyingQuestionsMapper,
            IMapper<PermitApplicationRequestModel, UploadedDocument[]> uploadedDocMapper,
            IMapper<PermitApplicationRequestModel, BackgroundCheck> backgroundCheckMapper)
    {
        _aliasMapper = aliasMapper;
        _addressMapper = addressMapper;
        _citizenshipMapper = citizenshipMapper;
        _contactMapper = contactMapper;
        _dobMapper = dobMapper;
        _idInfoMapper = idInfoMapper;
        _physicalAppearanceMapper = physicalAppearanceMapper;
        _licenseMapper = licenseMapper;
        _spouseInfoMapper = spouseInfoMapper;
        _workInfoMapper = workInfoMapper;
        _personalInfoMapper = personalInfoMapper;
        _mailingAddressMapper = mailingAddressMapper;
        _previousAddressMapper = previousAddressMapper;
        _immigrationMapper = immigrationMapper;
        _spouseAddressInfoMapper = spouseAddressInfoMapper;
        _weaponMapper = weaponMapper;
        _qualifyingQuestionsMapper = qualifyingQuestionsMapper;
        _uploadedDocMapper = uploadedDocMapper;
        _backgroundCheckMapper = backgroundCheckMapper;
    }

    public Entities.Application Map(PermitApplicationRequestModel source)
    {
        T? MapIfNotNull<T>(T? sourceType, Func<T> factoryFunc)
        {
            return sourceType == null ? sourceType : factoryFunc();
        }

        return new Entities.Application
        {
            Aliases = MapIfNotNull(source.Application.Aliases, () => _aliasMapper.Map(source)),
            ApplicationType = source.Application.ApplicationType,
            CurrentAddress = MapIfNotNull(source.Application.CurrentAddress, () => _addressMapper.Map(source)),
            Citizenship = MapIfNotNull(source.Application.Citizenship, () => _citizenshipMapper.Map(source)),
            Contact = MapIfNotNull(source.Application.Contact, () => _contactMapper.Map(source)),
            DOB = MapIfNotNull(source.Application.DOB, () => _dobMapper.Map(source)),
            Employment = source.Application.Employment,
            IdInfo = MapIfNotNull(source.Application.IdInfo, () => _idInfoMapper.Map(source)),
            PhysicalAppearance = MapIfNotNull(source.Application.PhysicalAppearance, () => _physicalAppearanceMapper.Map(source)),
            License = MapIfNotNull(source.Application.License, () => _licenseMapper.Map(source)),
            DifferentMailing = source.Application.DifferentMailing,
            DifferentSpouseAddress = source.Application.DifferentSpouseAddress,
            IsComplete = source.Application.IsComplete,
            ImmigrantInformation = MapIfNotNull(source.Application.ImmigrantInformation, () => _immigrationMapper.Map(source)),
            SpouseInformation = MapIfNotNull(source.Application.SpouseInformation, () => _spouseInfoMapper.Map(source)),
            WorkInformation = MapIfNotNull(source.Application.WorkInformation, () => _workInfoMapper.Map(source)),
            PersonalInfo = MapIfNotNull(source.Application.PersonalInfo, () => _personalInfoMapper.Map(source)),
            MailingAddress = MapIfNotNull(source.Application.MailingAddress, () => _mailingAddressMapper.Map(source)),
            PreviousAddresses = MapIfNotNull(source.Application.PreviousAddresses, () => _previousAddressMapper.Map(source)),
            SpouseAddressInformation = MapIfNotNull(source.Application.SpouseAddressInformation, () => _spouseAddressInfoMapper.Map(source)),
            UserEmail = source.Application.UserEmail,
            Weapons = MapIfNotNull(source.Application.Weapons, () => _weaponMapper.Map(source)),
            QualifyingQuestions = MapIfNotNull(source.Application.QualifyingQuestions, () => _qualifyingQuestionsMapper.Map(source)),
            CurrentStep = source.Application.CurrentStep,
            AppointmentStatus = source.Application.AppointmentStatus,
            SubmittedToLicensingDateTime = source.Application.SubmittedToLicensingDateTime,
            AppointmentDateTime = source.Application.AppointmentDateTime,
            AppointmentId = source.Application.AppointmentId,
            Status = source.Application.Status,
            OrderId = source.Application.OrderId,
            BackgroundCheck = MapIfNotNull(source.Application.BackgroundCheck, () => _backgroundCheckMapper.Map(source)),
            Comments = source.Application.Comments,
            PaymentStatus = source.Application.PaymentStatus,
            UploadedDocuments = MapIfNotNull(source.Application.UploadedDocuments, () => _uploadedDocMapper.Map(source)),
            StartOfNinetyDayCountdown = source.Application.StartOfNinetyDayCountdown,
            CiiNumber = source.Application.CiiNumber,
        };
    }
}
