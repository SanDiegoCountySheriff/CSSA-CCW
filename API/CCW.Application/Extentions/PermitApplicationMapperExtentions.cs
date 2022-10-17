using CCW.Application.Models;

namespace CCW.Application.Extensions
{
    public static class PermitApplicationMapperExtentions
    {
        public static PermitApplication ToNewDbModel(this PermitApplicationRequestModel permitApplicationRequestModel)
        {
            var newDbModel = new PermitApplication();
            newDbModel.id = Guid.NewGuid();
            newDbModel.application = new PermitApplication.Application()
            {
                aliases = MapAliases(permitApplicationRequestModel),
                applicationType = permitApplicationRequestModel.application.applicationType,
                citizenship = MapCitizenship(permitApplicationRequestModel, new PermitApplication.Citizenship()),
                contact = MapContact(permitApplicationRequestModel, new PermitApplication.Contact()),
                currentAddress = MapAddress(permitApplicationRequestModel.application.currentAddress, new PermitApplication.Address()),
                differentMailing = false,
                DOB = MapDOB(permitApplicationRequestModel, new PermitApplication.DOB()),
                employment = permitApplicationRequestModel.application.employment,
                idInfo = MapIdInfo(permitApplicationRequestModel, new PermitApplication.IdInfo()),
                mailingAddress = MapAddress(permitApplicationRequestModel.application.mailingAddress, new PermitApplication.Address()),
                personalInfo = MapPersonalInfo(permitApplicationRequestModel, new PermitApplication.PersonalInfo()),
                physicalAppearance = MapPhysicalAppearance(permitApplicationRequestModel, new PermitApplication.PhysicalAppearance()),
                previousAddresses = MapAddresses(permitApplicationRequestModel.application.previousAddresses),
                weapons = MapWeapons(permitApplicationRequestModel),
            };

            return newDbModel;
        }
        
        public static PermitApplication ToExistingDbModel(this PermitApplicationRequestModel permitApplicationRequestModel, PermitApplication permitApplication)
        {
            permitApplication.application.aliases = MapAliases(permitApplicationRequestModel);
            permitApplication.application.applicationType = permitApplicationRequestModel.application.applicationType;
            permitApplication.application.citizenship = MapCitizenship(permitApplicationRequestModel, permitApplication.application.citizenship);
            permitApplication.application.contact = MapContact(permitApplicationRequestModel, permitApplication.application.contact);
            permitApplication.application.currentAddress = MapAddress(permitApplicationRequestModel.application.currentAddress, permitApplication.application.currentAddress);
            permitApplication.application.differentMailing = permitApplicationRequestModel.application.differentMailing;
            permitApplication.application.DOB = MapDOB(permitApplicationRequestModel, permitApplication.application.DOB);
            permitApplication.application.employment = permitApplicationRequestModel.application.employment;
            permitApplication.application.idInfo = MapIdInfo(permitApplicationRequestModel, permitApplication.application.idInfo);
            permitApplication.application.mailingAddress = MapAddress(permitApplicationRequestModel.application.mailingAddress, permitApplication.application.mailingAddress);
            permitApplication.application.personalInfo = MapPersonalInfo(permitApplicationRequestModel, permitApplication.application.personalInfo);
            permitApplication.application.physicalAppearance = MapPhysicalAppearance(permitApplicationRequestModel, permitApplication.application.physicalAppearance);
            permitApplication.application.previousAddresses = MapAddresses(permitApplicationRequestModel.application.previousAddresses);
            permitApplication.application.weapons = MapWeapons(permitApplicationRequestModel);

            return permitApplication;
        }

        private static PermitApplication.Alias[] MapAliases(PermitApplicationRequestModel permitApplicationRequestModel)
        {
            if(permitApplicationRequestModel.application.aliases != null)
            {
                int count = permitApplicationRequestModel.application.aliases.Length;
                var newItem = new PermitApplication.Alias[count];
                for(int i = 0; i < count; i++)
                {
                    newItem[i] = MapAlias(permitApplicationRequestModel.application.aliases[i], new PermitApplication.Alias());
                }

                return newItem;
            }

            return new PermitApplication.Alias[0];
        }

        private static PermitApplication.Alias MapAlias(PermitApplicationRequestModel.Alias uiAlias, PermitApplication.Alias dbAlias)
        {
            dbAlias.prevLastName = uiAlias.prevLastName;
            dbAlias.prevFirstName = uiAlias.prevFirstName;
            dbAlias.prevMiddleName = uiAlias.prevMiddleName;
            dbAlias.cityWhereChanged = uiAlias.cityWhereChanged;
            dbAlias.stateWhereChanged = uiAlias.stateWhereChanged;
            dbAlias.courtFileNumber = uiAlias.courtFileNumber;

            return dbAlias;
        }

        private static PermitApplication.Citizenship MapCitizenship(PermitApplicationRequestModel permitApplicationRequestModel, PermitApplication.Citizenship dbCitizenship)
        {
            dbCitizenship.citizen = permitApplicationRequestModel.application.citizenship.citizen;
            dbCitizenship.militaryStatus = permitApplicationRequestModel.application.citizenship.militaryStatus;

            return dbCitizenship;
        }

        private static PermitApplication.Contact MapContact(PermitApplicationRequestModel permitApplicationRequestModel, PermitApplication.Contact dbContact)
        {
            if(permitApplicationRequestModel.application.contact != null)
            {
                dbContact.primaryPhoneNumber = permitApplicationRequestModel.application.contact.primaryPhoneNumber;
                dbContact.cellPhoneNumber = permitApplicationRequestModel.application.contact.cellPhoneNumber;
                dbContact.workPhoneNumber = permitApplicationRequestModel.application.contact.workPhoneNumber;
                dbContact.faxPhoneNumber = permitApplicationRequestModel.application.contact.faxPhoneNumber;
                dbContact.textMessageUpdates = permitApplicationRequestModel.application.contact.textMessageUpdates;
            }

            return dbContact;
        }

        private static PermitApplication.Address[] MapAddresses(PermitApplicationRequestModel.Address[] addresses)
        {
            if (addresses != null)
            {
                int count = addresses.Length;
                var newItem = new PermitApplication.Address[count];
                for (int i = 0; i < count; i++)
                {
                    newItem[i] = MapAddress(addresses[i], new PermitApplication.Address());
                }

                return newItem;
            }

            return new PermitApplication.Address[0];
        }

        private static PermitApplication.Address MapAddress(PermitApplicationRequestModel.Address uiAddress, PermitApplication.Address dbAddress)
        { 
            if(uiAddress != null)
            {
                dbAddress.addressLine1 = uiAddress.addressLine1;
                dbAddress.addressLine2 = uiAddress.addressLine2;
                dbAddress.city = uiAddress.city;
                dbAddress.county = uiAddress.county;
                dbAddress.state = uiAddress.state;
                dbAddress.zip = uiAddress.zip;
                dbAddress.country = uiAddress.country;
            }

            return dbAddress;
        }

        private static PermitApplication.DOB MapDOB(PermitApplicationRequestModel permitApplicationRequestModel, PermitApplication.DOB dbDOB)
        { 
            if(permitApplicationRequestModel.application.DOB != null)
            {
                dbDOB.birthDate = permitApplicationRequestModel.application.DOB.birthDate;
                dbDOB.birthCity = permitApplicationRequestModel.application.DOB.birthCity;
                dbDOB.birthState = permitApplicationRequestModel.application.DOB.birthState;
                dbDOB.birthCountry = permitApplicationRequestModel.application.DOB.birthCountry;
            }

            return dbDOB;
        }

        private static PermitApplication.IdInfo MapIdInfo(PermitApplicationRequestModel permitApplicationRequestModel, PermitApplication.IdInfo dbIdInfo)
        { 
            if(permitApplicationRequestModel.application.idInfo != null)
            {
                dbIdInfo.idNumber = permitApplicationRequestModel.application.idInfo.idNumber;
                dbIdInfo.issuingState = permitApplicationRequestModel.application.idInfo.issuingState;
            }

            return dbIdInfo;
        }
        
        private static PermitApplication.PersonalInfo MapPersonalInfo(PermitApplicationRequestModel permitApplicationRequestModel, PermitApplication.PersonalInfo dbPersonalinfo)
        { 
            if(permitApplicationRequestModel.application.personalInfo != null)
            {
                dbPersonalinfo.lastName = permitApplicationRequestModel.application.personalInfo.lastName;
                dbPersonalinfo.firstName = permitApplicationRequestModel.application.personalInfo.firstName;
                dbPersonalinfo.middleName = permitApplicationRequestModel.application.personalInfo.middleName;
                dbPersonalinfo.noMiddleName = permitApplicationRequestModel.application.personalInfo.noMiddleName;
                dbPersonalinfo.maidenName = permitApplicationRequestModel.application.personalInfo.maidenName;
                dbPersonalinfo.suffix = permitApplicationRequestModel.application.personalInfo.suffix;
                dbPersonalinfo.ssn = permitApplicationRequestModel.application.personalInfo.ssn;
                dbPersonalinfo.maritalStatus = permitApplicationRequestModel.application.personalInfo.maritalStatus;
            }

            return dbPersonalinfo;
        }
        
        private static PermitApplication.PhysicalAppearance MapPhysicalAppearance(PermitApplicationRequestModel permitApplicationRequestModel, PermitApplication.PhysicalAppearance dbPhysicalAppearance)
        {
            if(permitApplicationRequestModel.application.physicalAppearance != null)
            {
                dbPhysicalAppearance.gender = permitApplicationRequestModel.application.physicalAppearance.gender;
                dbPhysicalAppearance.heightFeet = permitApplicationRequestModel.application.physicalAppearance.heightFeet;
                dbPhysicalAppearance.heightInch = permitApplicationRequestModel.application.physicalAppearance.heightInch;
                dbPhysicalAppearance.weight = permitApplicationRequestModel.application.physicalAppearance.weight;
                dbPhysicalAppearance.hairColor = permitApplicationRequestModel.application.physicalAppearance.hairColor;
                dbPhysicalAppearance.eyeColor = permitApplicationRequestModel.application.physicalAppearance.eyeColor;
                dbPhysicalAppearance.physicalDesc = permitApplicationRequestModel.application.physicalAppearance.physicalDesc;
            }

            return dbPhysicalAppearance;
        }

        private static PermitApplication.Weapon[] MapWeapons(PermitApplicationRequestModel permitApplicationRequestModel)
        { 
            if(permitApplicationRequestModel.application.weapons != null)
            {
                int count = permitApplicationRequestModel.application.weapons.Length;
                var newItem = new PermitApplication.Weapon[count];
                for (int i = 0; i < count; i++)
                {
                    newItem[i] = MapWeapon(permitApplicationRequestModel.application.weapons[i], new PermitApplication.Weapon());
                }

                return newItem;
            }

            return new PermitApplication.Weapon[0];
        }

        private static PermitApplication.Weapon MapWeapon(PermitApplicationRequestModel.Weapon uiWeapon, PermitApplication.Weapon dbWeapon)
        {
            dbWeapon.make = uiWeapon.make;
            dbWeapon.model = uiWeapon.model;
            dbWeapon.caliber = uiWeapon.caliber;
            dbWeapon.serialNumber = uiWeapon.serialNumber;

            return dbWeapon;
        }
    }
}
