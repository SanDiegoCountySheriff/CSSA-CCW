using CCW.Application.Models;

namespace CCW.Application.Extensions
{
    public static class PermitApplicationResponseModelMapper
    {
        public static PermitApplicationResponseModel ToUiResponseModel(this PermitApplication permitApplication)
        {
            var newModel = new PermitApplicationResponseModel();
            newModel.id = permitApplication.id;

            newModel.application = new PermitApplicationResponseModel.Application()
            {
                aliases = MapNewUiAliases(permitApplication),
                applicationType = permitApplication.application.applicationType,
                citizenship = MapNewUiCitizenship(permitApplication),
                contact = MapNewUiContact(permitApplication),
                currentAddress = MapNewUiCurrentAddress(permitApplication.application.currentAddress),
                differentMailing = permitApplication.application.differentMailing,
                DOB = MapNewUiDOB(permitApplication),
               // employment = permitApplication.application.employment,
                idInfo = MapNewUiIdInfo(permitApplication),
                mailingAddress = MapNewUiCurrentAddress(permitApplication.application.mailingAddress),
                personalInfo = MapNewUiPersonalInfo(permitApplication),
                physicalAppearance = MapNewUiPhysicalAppearance(permitApplication),
                previousAddresses = MapNewUiPreviousAddresses(permitApplication),
                weapons = MapNewUiWeapons(permitApplication),
            };

            return newModel;
        }

        private static PermitApplicationResponseModel.Alias[] MapNewUiAliases(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationResponseModel.Alias[permitApplication.application.aliases.Length];

            return MapAliases(permitApplication, newItem);
        }

        private static PermitApplicationResponseModel.Alias[] MapAliases(PermitApplication permitApplication, PermitApplicationResponseModel.Alias[] aliases)
        {
            if (permitApplication.application.aliases != null)
            {
                int count = permitApplication.application.aliases.Length;
                for (int i = 0; i < count; i++)
                {
                    aliases[i] = new PermitApplicationResponseModel.Alias()
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

        private static PermitApplicationResponseModel.Citizenship MapNewUiCitizenship(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationResponseModel.Citizenship();

            return MapCitizenship(permitApplication, newItem);
        }

        private static PermitApplicationResponseModel.Citizenship MapCitizenship(PermitApplication permitApplication, PermitApplicationResponseModel.Citizenship citizenship)
        {
            if (permitApplication.application.citizenship != null)
            {
                citizenship.citizen = permitApplication.application.citizenship.citizen;
                citizenship.militaryStatus = permitApplication.application.citizenship.militaryStatus;
            }

            return citizenship;
        }

        private static PermitApplicationResponseModel.Contact MapNewUiContact(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationResponseModel.Contact();

            return MapContact(permitApplication, newItem);
        }

        private static PermitApplicationResponseModel.Contact MapContact(PermitApplication permitApplication, PermitApplicationResponseModel.Contact contact)
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

        private static PermitApplicationResponseModel.Address MapNewUiCurrentAddress(PermitApplication.Address address)
        {
            var newItem = new PermitApplicationResponseModel.Address();

            return MapAddress(address, newItem);
        }

        private static PermitApplicationResponseModel.Address MapAddress(PermitApplication.Address dbAddress, PermitApplicationResponseModel.Address uiAddress)
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

        private static PermitApplicationResponseModel.DOB MapNewUiDOB(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationResponseModel.DOB();

            return MapDOB(permitApplication, newItem);
        }

        private static PermitApplicationResponseModel.DOB MapDOB(PermitApplication permitApplication, PermitApplicationResponseModel.DOB dob)
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

        private static PermitApplicationResponseModel.IdInfo MapNewUiIdInfo(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationResponseModel.IdInfo();

            return MapIdInfo(permitApplication, newItem);
        }

        private static PermitApplicationResponseModel.IdInfo MapIdInfo(PermitApplication permitApplication, PermitApplicationResponseModel.IdInfo idInfo)
        {
            if (permitApplication.application.idInfo != null)
            {
                idInfo.idNumber = permitApplication.application.idInfo.idNumber;
                idInfo.issuingState = permitApplication.application.idInfo.issuingState; 
            }

            return idInfo;
        }

        private static PermitApplicationResponseModel.PersonalInfo MapNewUiPersonalInfo(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationResponseModel.PersonalInfo();

            return MapPersonalInfo(permitApplication, newItem);
        }

        private static PermitApplicationResponseModel.PersonalInfo MapPersonalInfo(PermitApplication permitApplication, PermitApplicationResponseModel.PersonalInfo personalInfo)
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

        private static PermitApplicationResponseModel.PhysicalAppearance MapNewUiPhysicalAppearance(PermitApplication permitApplication)
        {
            var newItem = new PermitApplicationResponseModel.PhysicalAppearance();

            return MapPhysicalAppearance(permitApplication, newItem);
        }

        private static PermitApplicationResponseModel.PhysicalAppearance MapPhysicalAppearance(PermitApplication permitApplication, PermitApplicationResponseModel.PhysicalAppearance physicalAppearance)
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

        private static PermitApplicationResponseModel.Address[] MapNewUiPreviousAddresses(PermitApplication permitApplication)
        {
            if (permitApplication.application.previousAddresses != null)
            {
                var newItem = new PermitApplicationResponseModel.Address[permitApplication.application.previousAddresses.Length];

                return MapPreviousAddresses(permitApplication, newItem);
            }

            return new PermitApplicationResponseModel.Address[0];
        }

        private static PermitApplicationResponseModel.Address[] MapPreviousAddresses(PermitApplication permitApplication, PermitApplicationResponseModel.Address[] addresses)
        {
            int count = permitApplication.application.previousAddresses.Length;
            for (int i = 0; i < count; i++)
            {
                addresses[i] = MapAddress(permitApplication.application.previousAddresses[i], new PermitApplicationResponseModel.Address());
            }

            return addresses;
        }

        private static PermitApplicationResponseModel.Weapon[] MapNewUiWeapons(PermitApplication permitApplication)
        {
            if (permitApplication.application.weapons != null)
            {
                var newItem = new PermitApplicationResponseModel.Weapon[permitApplication.application.weapons.Length];

                return MapWeapon(permitApplication, newItem);
            }

            return new PermitApplicationResponseModel.Weapon[0];
        }

        private static PermitApplicationResponseModel.Weapon[] MapWeapon(PermitApplication permitApplication, PermitApplicationResponseModel.Weapon[] weapons)
        {
            int count = permitApplication.application.weapons.Length;
            for (int i = 0; i < count; i++)
            {
                weapons[i] = new PermitApplicationResponseModel.Weapon()
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
