using CCW.Application.Models;

namespace CCW.Application.Extensions
{
    public static class PermitApplicationRequestModelMapper
    {
        public static PermitApplicationRequestModel ToNewUiModel(this PermitApplication permitApplication)
        {
            var newModel = new PermitApplicationRequestModel();
            newModel.id = permitApplication.id;

            newModel.application = new PermitApplicationRequestModel.Application()
            {
                aliases = MapNewUiAliases(permitApplication),
                applicationType = permitApplication.application.applicationType,
                citizenship = MapNewUiCitizenship(permitApplication),
                contact = MapNewUiContact(permitApplication),
                currentAddress = MapNewUiCurrentAddress(permitApplication.application.currentAddress),
                differentMailing = permitApplication.application.differentMailing,
                DOB = MapNewUiDOB(permitApplication),
                employment = permitApplication.application.employment,
                idInfo = MapNewUiIdInfo(permitApplication),
                mailingAddress = MapNewUiCurrentAddress(permitApplication.application.mailingAddress),
                personalInfo = MapNewUiPersonalInfo(permitApplication),
                physicalAppearance = MapNewUiPhysicalAppearance(permitApplication),
                previousAddresses = MapNewUiPreviousAddresses(permitApplication),
                weapons = MapNewUiWeapons(permitApplication),
            };

            return newModel;
        }

        private static PermitApplicationRequestModel.Alias[] MapNewUiAliases(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationRequestModel.Alias[permitApplication.application.aliases.Length];

            return MapAliases(permitApplication, newItem);
        }

        private static PermitApplicationRequestModel.Alias[] MapAliases(PermitApplication permitApplication, PermitApplicationRequestModel.Alias[] aliases)
        {
            if (permitApplication.application.aliases != null)
            {
                int count = permitApplication.application.aliases.Length;
                for (int i = 0; i < count; i++)
                {
                    aliases[i] = new PermitApplicationRequestModel.Alias()
                    {
                        prevLastName = permitApplication.application.aliases[i].prevLastName,
                        prevFirstName = permitApplication.application.aliases[i].prevFirstName,
                        prevMiddleName = permitApplication.application.aliases[i].prevMiddleName,
                        cityWhereChanged = permitApplication.application.aliases[i].cityWhereChanged,
                        stateWhereChanged = permitApplication.application.aliases[i].stateWhereChanged,
                        courtFileNumber = permitApplication.application.aliases[i].courtFileNumber,
                    };
                }
            }

            return aliases;
        }

        private static PermitApplicationRequestModel.Citizenship MapNewUiCitizenship(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationRequestModel.Citizenship();

            return MapCitizenship(permitApplication, newItem);
        }

        private static PermitApplicationRequestModel.Citizenship MapCitizenship(PermitApplication permitApplication, PermitApplicationRequestModel.Citizenship citizenship)
        {
            if (permitApplication.application.citizenship != null)
            {
                citizenship.citizen = permitApplication.application.citizenship.citizen;
                citizenship.militaryStatus = permitApplication.application.citizenship.militaryStatus;
            }

            return citizenship;
        }

        private static PermitApplicationRequestModel.Contact MapNewUiContact(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationRequestModel.Contact();

            return MapContact(permitApplication, newItem);
        }

        private static PermitApplicationRequestModel.Contact MapContact(PermitApplication permitApplication, PermitApplicationRequestModel.Contact contact)
        {
            if (permitApplication.application.contact != null)
            {
                contact.primaryPhoneNumber = permitApplication.application.contact.primaryPhoneNumber;
                contact.cellPhoneNumber = permitApplication.application.contact.cellPhoneNumber;
                contact.workPhoneNumber = permitApplication.application.contact.workPhoneNumber;
                contact.faxPhoneNumber = permitApplication.application.contact.faxPhoneNumber;
                contact.textMessageUpdates = permitApplication.application.contact.textMessageUpdates; 
            }

            return contact;
        }

        private static PermitApplicationRequestModel.Address MapNewUiCurrentAddress(PermitApplication.Address address)
        {
            var newItem = new PermitApplicationRequestModel.Address();

            return MapAddress(address, newItem);
        }

        private static PermitApplicationRequestModel.Address MapAddress(PermitApplication.Address dbAddress, PermitApplicationRequestModel.Address uiAddress)
        {
            uiAddress.addressLine1 = dbAddress.addressLine1;
            uiAddress.addressLine2 = dbAddress.addressLine2;
            uiAddress.city = dbAddress.city;
            uiAddress.county = dbAddress.county;
            uiAddress.state = dbAddress.state;
            uiAddress.zip = dbAddress.zip;
            uiAddress.country = dbAddress.country;

            return uiAddress;
        }

        private static PermitApplicationRequestModel.DOB MapNewUiDOB(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationRequestModel.DOB();

            return MapDOB(permitApplication, newItem);
        }

        private static PermitApplicationRequestModel.DOB MapDOB(PermitApplication permitApplication, PermitApplicationRequestModel.DOB dob)
        {
            if (permitApplication.application.DOB != null)
            {
                dob.birthDate = permitApplication.application.DOB.birthDate;
                dob.birthCity = permitApplication.application.DOB.birthCity;
                dob.birthState = permitApplication.application.DOB.birthState;
                dob.birthCountry = permitApplication.application.DOB.birthCountry; 
            }

            return dob;
        }

        private static PermitApplicationRequestModel.IdInfo MapNewUiIdInfo(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationRequestModel.IdInfo();

            return MapIdInfo(permitApplication, newItem);
        }

        private static PermitApplicationRequestModel.IdInfo MapIdInfo(PermitApplication permitApplication, PermitApplicationRequestModel.IdInfo idInfo)
        {
            if (permitApplication.application.idInfo != null)
            {
                idInfo.idNumber = permitApplication.application.idInfo.idNumber;
                idInfo.issuingState = permitApplication.application.idInfo.issuingState; 
            }

            return idInfo;
        }

        private static PermitApplicationRequestModel.PersonalInfo MapNewUiPersonalInfo(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationRequestModel.PersonalInfo();

            return MapPersonalInfo(permitApplication, newItem);
        }

        private static PermitApplicationRequestModel.PersonalInfo MapPersonalInfo(PermitApplication permitApplication, PermitApplicationRequestModel.PersonalInfo personalInfo)
        {
            if (permitApplication.application.personalInfo != null)
            {
                personalInfo.lastName = permitApplication.application.personalInfo.lastName;
                personalInfo.firstName = permitApplication.application.personalInfo.firstName;
                personalInfo.middleName = permitApplication.application.personalInfo.middleName;
                personalInfo.noMiddleName = permitApplication.application.personalInfo.noMiddleName;
                personalInfo.maidenName = permitApplication.application.personalInfo.maidenName;
                personalInfo.suffix = permitApplication.application.personalInfo.suffix;
                personalInfo.ssn = permitApplication.application.personalInfo.ssn;
                personalInfo.maritalStatus = permitApplication.application.personalInfo.maritalStatus; 
            }

            return personalInfo;
        }

        private static PermitApplicationRequestModel.PhysicalAppearance MapNewUiPhysicalAppearance(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationRequestModel.PhysicalAppearance();

            return MapPhysicalAppearance(permitApplication, newItem);
        }

        private static PermitApplicationRequestModel.PhysicalAppearance MapPhysicalAppearance(PermitApplication permitApplication, PermitApplicationRequestModel.PhysicalAppearance physicalAppearance)
        {
            if (permitApplication.application.physicalAppearance != null)
            {
                physicalAppearance.gender = permitApplication.application.physicalAppearance.gender;
                physicalAppearance.heightFeet = permitApplication.application.physicalAppearance.heightFeet;
                physicalAppearance.heightInch = permitApplication.application.physicalAppearance.heightInch;
                physicalAppearance.weight = permitApplication.application.physicalAppearance.weight;
                physicalAppearance.hairColor = permitApplication.application.physicalAppearance.hairColor;
                physicalAppearance.eyeColor = permitApplication.application.physicalAppearance.eyeColor;
                physicalAppearance.physicalDesc = permitApplication.application.physicalAppearance.physicalDesc; 
            }

            return physicalAppearance;
        }

        private static PermitApplicationRequestModel.Address[] MapNewUiPreviousAddresses(PermitApplication permitApplication)
        {
            if (permitApplication.application.previousAddresses != null)
            {
                var newItem = new PermitApplicationRequestModel.Address[permitApplication.application.previousAddresses.Length];

                return MapPreviousAddresses(permitApplication, newItem);
            }

            return new PermitApplicationRequestModel.Address[0];
        }

        private static PermitApplicationRequestModel.Address[] MapPreviousAddresses(PermitApplication permitApplication, PermitApplicationRequestModel.Address[] addresses)
        {
            int count = permitApplication.application.previousAddresses.Length;
            for (int i = 0; i < count; i++)
            {
                addresses[i] = MapAddress(permitApplication.application.previousAddresses[i], new PermitApplicationRequestModel.Address());
            }

            return addresses;
        }

        private static PermitApplicationRequestModel.Weapon[] MapNewUiWeapons(PermitApplication permitApplication)
        {
            if (permitApplication.application.weapons != null)
            {
                var newItem = new PermitApplicationRequestModel.Weapon[permitApplication.application.weapons.Length];

                return MapWeapon(permitApplication, newItem);
            }

            return new PermitApplicationRequestModel.Weapon[0];
        }

        private static PermitApplicationRequestModel.Weapon[] MapWeapon(PermitApplication permitApplication, PermitApplicationRequestModel.Weapon[] weapons)
        {
            int count = permitApplication.application.previousAddresses.Length;
            for (int i = 0; i < count; i++)
            {
                weapons[i] = new PermitApplicationRequestModel.Weapon()
                {
                    make = permitApplication.application.weapons[i].make,
                    model = permitApplication.application.weapons[i].model,
                    caliber = permitApplication.application.weapons[i].caliber,
                    serialNumber = permitApplication.application.weapons[i].serialNumber,
                };
            }

            return weapons;
        }
    }
}
