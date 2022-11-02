using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToApplicationMapper : IMapper<PermitApplication, Entities.Application>
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
    private readonly IMapper<PermitApplication, SpouseAddressInformation> _spouseAddressInfoMapper;
    private readonly IMapper<PermitApplication, Weapon[]> _weaponMapper;
    private readonly IMapper<PermitApplication, QualifyingQuestions> _qualifyingQuestionsMapper;
    private readonly IMapper<PermitApplication, History[]> _historyMapper;

    public PermitApplicationToApplicationMapper(
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
        IMapper<PermitApplication, SpouseAddressInformation> spouseAddressInfoMapper,
        IMapper<PermitApplication, Weapon[]> weaponMapper,
        IMapper<PermitApplication, QualifyingQuestions> qualifyingQuestionsMapper,
        IMapper<PermitApplication, History[]> historyMapper)
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
        _spouseAddressInfoMapper = spouseAddressInfoMapper;
        _weaponMapper = weaponMapper;
        _qualifyingQuestionsMapper = qualifyingQuestionsMapper;
        _historyMapper = historyMapper;
    }

    public Entities.Application Map(PermitApplication source)
    {
        return new Entities.Application
        {
            Aliases = source.Application.Aliases != null ? _aliasMapper.Map(source) : null,
            ApplicationType = source.Application.ApplicationType ?? null,
            CurrentAddress = source.Application.CurrentAddress != null ? _addressMapper.Map(source) : null,
            Citizenship = source.Application.Citizenship != null ? _citizenshipMapper.Map(source) : null,
            Contact = source.Application.Contact != null ? _contactMapper.Map(source) : null,
            DOB = source.Application.DOB != null ? _dobMapper.Map(source) : null,
            Employment = source.Application.Employment ?? null,
            IdInfo = source.Application.IdInfo != null ? _idInfoMapper.Map(source) : null,
            PhysicalAppearance = source.Application.PhysicalAppearance != null ? _physicalAppearanceMapper.Map(source) : null,
            License = source.Application.License != null ? _licenseMapper.Map(source) : null,
            DifferentMailing = source.Application.DifferentMailing ?? null,
            DifferentSpouseAddress = source.Application.DifferentSpouseAddress ?? null,
            IsComplete = source.Application.IsComplete,
            SpouseInformation = source.Application.SpouseInformation != null ? _spouseInfoMapper.Map(source) : null,
            WorkInformation = source.Application.WorkInformation != null ? _workInfoMapper.Map(source) : null,
            PersonalInfo = source.Application.PersonalInfo != null ? _personalInfoMapper.Map(source) : null,
            MailingAddress = source.Application.MailingAddress != null ? _mailingAddressMapper.Map(source) : null,
            PreviousAddresses = source.Application.PreviousAddresses != null ? _previousAddressMapper.Map(source) : null,
            SpouseAddressInformation = source.Application.SpouseAddressInformation != null ? _spouseAddressInfoMapper.Map(source) : null,
            UserEmail = source.Application.UserEmail,
            Weapons = source.Application.Weapons != null ? _weaponMapper.Map(source) : null,
            QualifyingQuestions = source.Application.QualifyingQuestions != null ? _qualifyingQuestionsMapper.Map(source) : null,
            History = _historyMapper.Map(source),
        };
    }
}