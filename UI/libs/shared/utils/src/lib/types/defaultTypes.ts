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

export type AppearanceInfoType = {
  gender: string;
  heightFeet: number | null;
  heightInch: number | null;
  weight: number | null;
  hairColor: string;
  eyeColor: string;
  physicalDesc: string;
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
  currentAge: number | null;
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
}

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


export type CompleteApplication = {
  aliases: Array<AliasType>;
  applicationType: string;
  citizenship: {
    citizen: boolean;
    militaryStatus: string;
  };
  contact: ContactInfoType;
  currentAddress: AddressInfoType;
  differentMailing: boolean;
  DOB: DOBType;
  employment: string;
  id: IdType;
  immigrantInformation: ImmigrantInformation;
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
  spouseInformation: SpouseInfoType;
  weapons: Array<WeaponInfoType>;
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
