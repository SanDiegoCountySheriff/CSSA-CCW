using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToUserApplicationMapper : IMapper<PermitApplication, UserApplication>
{
    private readonly IMapper<PermitApplication, Alias[]> _aliasMapper;
    private readonly IMapper<PermitApplication, Address> _addressMapper;
    private readonly IMapper<PermitApplication, Citizenship> _citizenshipMapper;
    private readonly IMapper<PermitApplication, Contact> _contactMapper;
    private readonly IMapper<PermitApplication, DOB> _dobMapper;
    private readonly IMapper<PermitApplication, IdInfo> _idInfoMapper;
    private readonly IMapper<PermitApplication, PhysicalAppearance> _physicalAppearanceMapper;
    private readonly IMapper<PermitApplication, License> _licenseMapper;
    private readonly IMapper<PermitApplication, SpouseInformation> _spouseInfoMapper;
    private readonly IMapper<PermitApplication, WorkInformation> _workInfoMapper;
    private readonly IMapper<PermitApplication, PersonalInfo> _personalInfoMapper;
    private readonly IMapper<PermitApplication, MailingAddress?> _mailingAddressMapper;
    private readonly IMapper<PermitApplication, Address[]> _previousAddressMapper;
    private readonly IMapper<PermitApplication, ImmigrantInformation> _immigrationMapper;
    private readonly IMapper<PermitApplication, SpouseAddressInformation> _spouseAddressInfoMapper;
    private readonly IMapper<PermitApplication, Weapon[]> _weaponMapper;
    private readonly IMapper<PermitApplication, QualifyingQuestions> _qualifyingQuestionsMapper;
    private readonly IMapper<PermitApplication, UploadedDocument[]> _uploadedDocMapper;
    private readonly IMapper<PermitApplication, BackgroudCheck> _backgroundCheckMapper;

    public PermitApplicationToUserApplicationMapper(
        IMapper<PermitApplication, Alias[]> aliasMapper,
        IMapper<PermitApplication, Address> addressMapper,
        IMapper<PermitApplication, Citizenship> citizenshipMapper,
        IMapper<PermitApplication, Contact> contactMapper,
        IMapper<PermitApplication, DOB> dobMapper,
        IMapper<PermitApplication, IdInfo> idInfoMapper,
        IMapper<PermitApplication, PhysicalAppearance> physicalAppearanceMapper,
        IMapper<PermitApplication, License> licenseMapper,
        IMapper<PermitApplication, SpouseInformation> spouseInfoMapper,
        IMapper<PermitApplication, WorkInformation> workInfoMapper,
        IMapper<PermitApplication, PersonalInfo> personalInfoMapper,
        IMapper<PermitApplication, MailingAddress?> mailingAddressMapper,
        IMapper<PermitApplication, Address[]> previousAddressMapper,
        IMapper<PermitApplication, ImmigrantInformation> immigrationMapper,
        IMapper<PermitApplication, SpouseAddressInformation> spouseAddressInfoMapper,
        IMapper<PermitApplication, Weapon[]> weaponMapper,
        IMapper<PermitApplication, QualifyingQuestions> qualifyingQuestionsMapper,
        IMapper<PermitApplication, UploadedDocument[]> uploadedDocMapper,
        IMapper<PermitApplication, BackgroudCheck> backgroundCheckMapper)
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

    public UserApplication Map(PermitApplication source)
    {
        T? MapIfNotNull<T>(T? sourceType, Func<T> factoryFunc)
        {
            return sourceType == null ? sourceType : factoryFunc();
        }

        return new UserApplication
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
            AppointmentDateTime = source.Application.AppointmentDateTime,
            Status = source.Application.Status,
            OrderId = source.Application.OrderId,
            BackgroudCheck = MapIfNotNull(source.Application.BackgroudCheck, () => _backgroundCheckMapper.Map(source)),
            UploadedDocuments = MapIfNotNull(source.Application.UploadedDocuments, () => _uploadedDocMapper.Map(source)),
        };
    }
}