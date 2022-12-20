using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;
public class UserPermitRequestApplicationToApplicationMapper : IMapper<string, UserPermitApplicationRequestModel, Entities.Application>
{
    private readonly IMapper<UserPermitApplicationRequestModel, Alias[]> _aliasMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, Address> _addressMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, Citizenship> _citizenshipMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, Contact> _contactMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, DOB> _dobMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, IdInfo> _idInfoMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, PhysicalAppearance> _physicalAppearanceMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, License> _licenseMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, SpouseInformation> _spouseInfoMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, WorkInformation> _workInfoMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, PersonalInfo> _personalInfoMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, MailingAddress?> _mailingAddressMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, Address[]> _previousAddressMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, ImmigrantInformation> _immigrationMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, SpouseAddressInformation> _spouseAddressInfoMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, Weapon[]> _weaponMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, QualifyingQuestions> _qualifyingQuestionsMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, UploadedDocument[]> _uploadedDocMapper;
    private readonly IMapper<UserPermitApplicationRequestModel, BackgroudCheck> _backgroundCheckMapper;


    public UserPermitRequestApplicationToApplicationMapper(
            IMapper<UserPermitApplicationRequestModel, Alias[]> aliasMapper,
            IMapper<UserPermitApplicationRequestModel, Address> addressMapper,
            IMapper<UserPermitApplicationRequestModel, Citizenship> citizenshipMapper,
            IMapper<UserPermitApplicationRequestModel, Contact> contactMapper,
            IMapper<UserPermitApplicationRequestModel, DOB> dobMapper,
            IMapper<UserPermitApplicationRequestModel, IdInfo> idInfoMapper,
            IMapper<UserPermitApplicationRequestModel, PhysicalAppearance> physicalAppearanceMapper,
            IMapper<UserPermitApplicationRequestModel, License> licenseMapper,
            IMapper<UserPermitApplicationRequestModel, SpouseInformation> spouseInfoMapper,
            IMapper<UserPermitApplicationRequestModel, WorkInformation> workInfoMapper,
            IMapper<UserPermitApplicationRequestModel, PersonalInfo> personalInfoMapper,
            IMapper<UserPermitApplicationRequestModel, MailingAddress?> mailingAddressMapper,
            IMapper<UserPermitApplicationRequestModel, Address[]> previousAddressMapper,
            IMapper<UserPermitApplicationRequestModel, ImmigrantInformation> immigrationMapper,
            IMapper<UserPermitApplicationRequestModel, SpouseAddressInformation> spouseAddressInfoMapper,
            IMapper<UserPermitApplicationRequestModel, Weapon[]> weaponMapper,
            IMapper<UserPermitApplicationRequestModel, QualifyingQuestions> qualifyingQuestionsMapper,
            IMapper<UserPermitApplicationRequestModel, UploadedDocument[]> uploadedDocMapper,
            IMapper<UserPermitApplicationRequestModel, BackgroudCheck> backgroundCheckMapper)
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


    public Entities.Application Map(string comments, UserPermitApplicationRequestModel source)
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
            AppointmentDateTime = source.Application.AppointmentDateTime,
            Status = source.Application.Status,
            OrderId = source.Application.OrderId,
            BackgroudCheck = MapIfNotNull(source.Application.BackgroudCheck, () => _backgroundCheckMapper.Map(source)),
            Comments = comments,
            UserId = source.Application.UserId,
            PaymentStatus = source.Application.PaymentStatus,
            UploadedDocuments = MapIfNotNull(source.Application.UploadedDocuments, () => _uploadedDocMapper.Map(source)),
        };
    }
}
