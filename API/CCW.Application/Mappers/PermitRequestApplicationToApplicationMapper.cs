using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class PermitRequestApplicationToApplicationMapper : IMapper<PermitApplicationRequestModel, Entities.Application>
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

        public PermitRequestApplicationToApplicationMapper(
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
            IMapper<PermitApplication, Weapon[]> weaponMapper)
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
               // Aliases = _aliasMapper.Map(source),
                ApplicationType = source.Application.ApplicationType,
                //CurrentAddress = _addressMapper.Map(source),
                //Citizenship = _citizenshipMapper.Map(source),
                //Contact = _contactMapper.Map(source),
                //DOB = _dobMapper.Map(source),
                //Employment = source.Application.Employment,
                //IdInfo = _idInfoMapper.Map(source),
                //PhysicalAppearance = _physicalAppearanceMapper.Map(source),
                //License = _licenseMapper.Map(source),
                //DifferentMailing = source.Application.DifferentMailing,
                //DifferentSpouseAddress = source.Application.DifferentSpouseAddress,
                //IsComplete = source.Application.IsComplete,
                //SpouseInformation = _spouseInfoMapper.Map(source),
                //WorkInformation = _workInfoMapper.Map(source),
                //PersonalInfo = _personalInfoMapper.Map(source),
                //MailingAddress = _mailingAddressMapper.Map(source),
                //PreviousAddresses = _previousAddressMapper.Map(source),
                //SpouseAddressInformation = _spouseAddressInfoMapper.Map(source),
                //UserEmail = source.Application.UserEmail,
                //Weapons = _weaponMapper.Map(source),
            };
        }
    }
}
