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
        private readonly IMapper<PermitApplicationRequestModel, SpouseAddressInformation> _spouseAddressInfoMapper;
        private readonly IMapper<PermitApplicationRequestModel, Weapon[]> _weaponMapper;

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
            IMapper<PermitApplicationRequestModel, SpouseAddressInformation> spouseAddressInfoMapper,
            IMapper<PermitApplicationRequestModel, Weapon[]> weaponMapper)
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
        }

        public Entities.Application Map(PermitApplicationRequestModel source)
        {
            return new Entities.Application
            {
                Aliases = _aliasMapper.Map(source),
                ApplicationType = source.Application.ApplicationType,
                CurrentAddress = _addressMapper.Map(source),
                Citizenship = _citizenshipMapper.Map(source),
                Contact = _contactMapper.Map(source),
                DOB = _dobMapper.Map(source),
                Employment = source.Application.Employment,
                IdInfo = _idInfoMapper.Map(source),
                PhysicalAppearance = _physicalAppearanceMapper.Map(source),
                License = _licenseMapper.Map(source),
                DifferentMailing = source.Application.DifferentMailing,
                DifferentSpouseAddress = source.Application.DifferentSpouseAddress,
                IsComplete = source.Application.IsComplete,
                SpouseInformation = _spouseInfoMapper.Map(source),
                WorkInformation = _workInfoMapper.Map(source),
                PersonalInfo = _personalInfoMapper.Map(source),
                MailingAddress = _mailingAddressMapper.Map(source),
                PreviousAddresses = _previousAddressMapper.Map(source),
                SpouseAddressInformation = _spouseAddressInfoMapper.Map(source),
                UserEmail = source.Application.UserEmail,
                Weapons = _weaponMapper.Map(source),
            };
        }
    }
