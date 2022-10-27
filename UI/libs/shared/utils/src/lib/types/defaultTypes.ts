export type AddressInfoType = {
  addressLine1: string;
  addressLine2: string;
  city: string;
  county: string;
  state: string;
  zip: string;
  country: string;
};

export type AliasType = {
  prevLastName: string;
  prevFirstName: string;
  prevMiddleName: string;
  cityWhereChanged: string;
  stateWhereChanged: string;
  courtFileNumber: string;
};

export type AppConfigType = {
  apiBaseUrl: string;
  apiSubscription: string;
  authorityUrl: string;
  knownAuthorities: Array<string>;
  clientId: string;
  defaultCounty: string;
  displayDebugger: boolean;
  environmentName: string;
  loginType: string;
  refreshTime: number;
};

export type AppearanceInfoType = {
  gender: string;
  heightFeet: number | null;
  heightInch: number | null;
  weight: number | null;
  hairColor: string;
  eyeColor: string;
  physicalDesc: string;
};

export type AuthType = {
  id?: string;
  userName: string;
  userEmail: string;
  jwtToken: string;
  isAuthenticated: boolean;
  isAdmin: boolean;
  verifiedUser: boolean;
  roles: Array<string>;
};

export type CitizenshipType = {
  citizen: boolean;
  militaryStatus: string;
};

export type ContactInfoType = {
  primaryPhoneNumber: string;
  cellPhoneNumber: string;
  workPhoneNumber: string;
  faxPhoneNumber: string;
  textMessageUpdates: boolean;
};

export type DOBType = {
  birthDate: string;
  birthCity: string;
  birthState: string;
  birthCountry: string;
};

export type IdType = {
  idNumber: string;
  issuingState: string;
};

export type ImmigrantInformation = {
  countryOfCitizenship: string;
  countryOfBirth: string;
  immigrantAlien: boolean;
  nonImmigrantAlien: boolean;
};

export type LicenseType = {
  permitNumber: string;
  issuingCounty: string;
  expirationDate: string;
};
export type PaymentInfoType = {
  applicationCost: number;
  convenienceFee: number;
  creditFee: number;
  totalCost: number;
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

export type RadioOptionsType = {
  label: string;
  value: string | boolean;
  color?: string;
};

export type SpouseInfoType = {
  lastName: string;
  firstName: string;
  middleName: string;
  maidenName: string;
};

export type WeaponInfoType = {
  make: string;
  model: string;
  caliber: string;
  serialNumber: string;
};

export type WorkInformationType = {
  employerName: string;
  employerAddressLine1: string;
  employerAddressLine2: string;
  employerCity: string;
  employerState: string;
  employerZip: string;
  employerCountry: string;
  employerPhone: string;
};

export type AppointmentType = {
  status: string;
  name: string;
  permit: string;
  payment: string;
  date: string;
  time: string;
  rowClass?: string;
};

export type CompleteApplication = {
  id?: string;
  aliases: Array<AliasType>;
  applicationType: string;
  citizenship: {
    citizen: boolean;
    militaryStatus: string;
  };
  contact: ContactInfoType;
  currentAddress: AddressInfoType;
  differentMailing: boolean;
  differentSpouseAddress: boolean;
  DOB: DOBType;
  employment: string;
  idInfo: IdType;
  immigrantInformation: ImmigrantInformation;
  isComplete: boolean;
  license: LicenseType;
  mailingAddress: AddressInfoType;
  personalInfo: {
    lastName: string;
    firstName: string;
    middleName: string;
    noMiddleName: boolean;
    maidenName: string;
    suffix: string;
    ssn: string;
    maritalStatus: string;
  };
  physicalAppearance: AppearanceInfoType;
  previousAddress: Array<AddressInfoType>;
  spouseAddressInformation: AddressInfoType;
  spouseInformation: SpouseInfoType;
  userEmail: string;
  weapons: Array<WeaponInfoType>;
  workInformation: WorkInformationType;
};
