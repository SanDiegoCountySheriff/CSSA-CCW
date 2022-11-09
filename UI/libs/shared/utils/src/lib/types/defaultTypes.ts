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
  adminApiBaseUrl: string;
  applicationApiBaseUrl: string;
  documentApiBaseUrl: string;
  paymentApiBaseUrl: string;
  scheduleApiBaseUrl: string;
  userProfileApiBaseUrl: string;
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
  heightFeet: string;
  heightInch: string;
  weight: string;
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

export type EventType = {
  name: string;
  start: Date;
  end: Date;
  color: string;
  timed: boolean;
};

export type HistoryType = {
  change: string;
  changeDateTimeUtc: string;
  changeMadeBy: string;
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

export type QualifyingQuestions = {
  questionOne: boolean;
  questionOneExp: string;
  questionTwo: boolean;
  questionTwoExp: string;
  questionThree: boolean;
  questionThreeExp: string;
  questionFour: boolean;
  questionFourExp: string;
  questionFive: boolean;
  questionFiveExp: string;
  questionSix: boolean;
  questionSixExp: string;
  questionSeven: boolean;
  questionSevenExp: string;
  questionEight: boolean;
  questionEightExp: string;
  questionNine: boolean;
  questionNineExp: string;
  questionTen: boolean;
  questionTenExp: string;
  questionEleven: boolean;
  questionElevenExp: string;
  questionTwelve: boolean;
  questionTwelveExp: string;
  questionThirteen: boolean;
  questionThirteenExp: string;
  questionFourteen: boolean;
  questionFourteenExp: string;
  questionFifteen: boolean;
  questionFifteenExp: string;
  questionSixteen: boolean;
  questionSixteenExp: string;
  questionSeventeen: boolean;
  questionSeventeenExp: string;
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
  occupation: string;
};

export type AppointmentType = {
  id: string;
  applicantId: string;
  status: string;
  name: string;
  permit: string;
  payment: string;
  date: string;
  time: string;
  start: string;
  end: string;
  isManuallyCreated: boolean;
};

export type UploadedDocType = {
  name: string;
  uploadedDateTimeUtc: string;
  uploadedBy: string;
  DocumentType: string;
};

export type CompleteApplication = {
  application: {
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
    dob: DOBType;
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
    previousAddresses: Array<AddressInfoType>;
    qualifyingQuestions: QualifyingQuestions;
    spouseAddressInformation: AddressInfoType;
    spouseInformation: SpouseInfoType;
    userEmail: string;
    weapons: Array<WeaponInfoType>;
    workInformation: WorkInformationType;
    currentStep: number;
    status: number;
    appointmentStatus: number;
    orderId: string;
    uploadedDocuments: Array<UploadedDocType>;
  };
  history: Array<HistoryType>;
  id: string;
};

export type ThemeConfigType = {
  isDark: boolean;
};

export type StatusType = {
  isOnline: boolean;
};
