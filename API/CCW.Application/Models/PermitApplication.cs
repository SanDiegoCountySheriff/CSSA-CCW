
public class PermitApplication
{
    public Application application { get; set; }

    public Guid id { get; set; }

    public class Application
    {
        public Alias[] aliases { get; set; }
        public string applicationType { get; set; }
        public Citizenship citizenship { get; set; }
        public Contact contact { get; set; }
        public Address currentAddress { get; set; }
        public bool differentMailing { get; set; }
        public bool differentSpouseAddress { get; set; }
        public DOB DOB { get; set; }
        public string Employment { get; set; }
        public IdInfo idInfo { get; set; }
        public bool isComplete { get; set; }
        public License License { get; set; }
        public Address? mailingAddress { get; set; }
        public PersonalInfo personalInfo { get; set; }
        public PhysicalAppearance physicalAppearance { get; set; }
        public Address[] previousAddresses { get; set; }
        public SpouseInformation SpouseInformation { get; set; }
        public SpouseAddressInformation SpouseAddressInformation { get; set; }
        public string userEmail { get; set; }
        public Weapon[] weapons { get; set; }
        public WorkInformation WorkInformation { get; set; }
    }

    public class Address
    {
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }

    public class Alias
    {
        public string prevLastName { get; set; }
        public string prevFirstName { get; set; }
        public string prevMiddleName { get; set; }
        public string cityWhereChanged { get; set; }
        public string stateWhereChanged { get; set; }
        public string courtFileNumber { get; set; }
    }

    public class Citizenship
    {
        public string citizen { get; set; }
        public string militaryStatus { get; set; }
    }

    public class Contact
    {
        public string primaryPhoneNumber { get; set; }
        public string cellPhoneNumber { get; set; }
        public string workPhoneNumber { get; set; }
        public string faxPhoneNumber { get; set; }
        public string textMessageUpdates { get; set; }
    }

    public class DOB
    {
        public string birthDate { get; set; }
        public string birthCity { get; set; }
        public string birthState { get; set; }
        public string birthCountry { get; set; }
    }

    public class IdInfo
    {
        public string idNumber { get; set; }
        public string issuingState { get; set; }
    }

    public class License
    {
        public string permitNumber { get; set; }
        public string issuingCounty { get; set; }
        public string expirationDate { get; set; }
    }

    public class PersonalInfo
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public bool noMiddleName { get; set; }
        public string maidenName { get; set; }
        public string suffix { get; set; }
        public string ssn { get; set; }
        public string maritalStatus { get; set; }
    }

    public class PhysicalAppearance
    {
        public string gender { get; set; }
        public string heightFeet { get; set; }
        public string heightInch { get; set; }
        public string weight { get; set; }
        public string hairColor { get; set; }
        public string eyeColor { get; set; }
        public string physicalDesc { get; set; }
    }

    public class SpouseInformation
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string maidenName { get; set; }
    }

    public class SpouseAddressInformation
    {
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string county { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }


    public class Weapon
    {
        public string make { get; set; }
        public string model { get; set; }
        public string caliber { get; set; }
        public string serialNumber { get; set; }
    }

    public class WorkInformation
    {
        public string employerName { get; set; }
        public string employerAddressLine1 { get; set; }
        public string employerAddressLine2 { get; set; }
        public string employerCity { get; set; }
        public string employerState { get; set; }
        public string employerCountry { get; set; }
        public string employerPhone { get; set; }
        public string employerZip { get; set; }
    }

}