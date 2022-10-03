export type AliasType = {
  prevLastName: string;
  prevFirstName: string;
  prevMiddleName: string;
  cityWhereChanged: string;
  stateWhereChanged: string;
  courtFileNumber: string;
};

export type IdType = {
  idNumber: string;
  issuingState: string;
};

export type DOBType = {
  DOB: string;
  birthCity: string;
  birthState: string;
  birthCountry: string;
  currentAge: 0;
};

export type CitizenshipType = {
  citizen: boolean;
  militaryStatus: string;
};

export type PersonalInfoType = {
  lastName: string;
  firstName: string;
  middleName: string;
  noMiddleName: boolean;
  maidenName: string;
  suffix: string;
  ssn: string;
  maritalStatus: string;
};

export type AddressInfoType = {
  addressLine1: string;
  addressLine2: string;
  city: string;
  county: string;
  state: string;
  zip: string;
  country: string;
};

export type RadioOptionsType = {
  label: string;
  value: string | boolean;
};

export type AppearanceInfoType = {
  gender: string;
  heightFeet: number;
  heightInch: number;
  weight: number;
  hairColor: string;
  eyeColor: string;
  physicalDesc: string;
};

export type ContactInfoType = {
  primaryPhoneNumber: string;
  cellPhoneNumber: string;
  workPhoneNumber: string;
  faxPhoneNumber: string;
  textMessageUpdates: boolean;
};
