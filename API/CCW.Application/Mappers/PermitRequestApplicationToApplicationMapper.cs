using CCW.Application.Entities;
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
            IMapper<PermitApplicationRequestModel, QualifyingQuestions> qualifyingQuestionsMapper)
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
    }

    public Entities.Application Map(PermitApplicationRequestModel source)
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
            ImmigrantInformation = source.Application.ImmigrantInformation != null ? _immigrationMapper.Map(source) : null,
            SpouseInformation = source.Application.SpouseInformation != null ? _spouseInfoMapper.Map(source) : null,
            WorkInformation = source.Application.WorkInformation != null ? _workInfoMapper.Map(source) : null,
            PersonalInfo = source.Application.PersonalInfo != null ? _personalInfoMapper.Map(source) : null,
            MailingAddress = source.Application.MailingAddress != null ? _mailingAddressMapper.Map(source) : null,
            PreviousAddresses = source.Application.PreviousAddresses != null ? _previousAddressMapper.Map(source) : null,
            SpouseAddressInformation = source.Application.SpouseAddressInformation != null ? _spouseAddressInfoMapper.Map(source) : null,
            UserEmail = source.Application.UserEmail,
            Weapons = source.Application.Weapons != null ? _weaponMapper.Map(source) : null,
            QualifyingQuestions = source.Application.QualifyingQuestions != null ? _qualifyingQuestionsMapper.Map(source) : null,
            CurrentStep = source.Application.CurrentStep,
        };
    }
}
