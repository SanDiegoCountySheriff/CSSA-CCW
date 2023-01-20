import { VuetifyThemeItem } from 'vuetify/types/services/theme';

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
  sessionStarted: string;
  tokenExpired: boolean;
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

export type CostType = {
  new: {
    standard: number;
    judicial: number;
    reserve: number;
  };
  renew: {
    standard: number;
    judicial: number;
    reserve: number;
  };
  issuance: number;
  modify: number;
  creditFee: number;
  convenienceFee: number;
};

export type DOBType = {
  birthDate: string;
  birthCity: string;
  birthState: string;
  birthCountry: string;
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
  questionOne: boolean | null;
  questionOneExp: string;
  questionTwo: boolean | null;
  questionTwoExp: string;
  questionThree: boolean | null;
  questionThreeExp: string;
  questionFour: boolean | null;
  questionFourExp: string;
  questionFive: boolean | null;
  questionFiveExp: string;
  questionSix: boolean | null;
  questionSixExp: string;
  questionSeven: boolean | null;
  questionSevenExp: string;
  questionEight: boolean | null;
  questionEightExp: string;
  questionNine: boolean | null;
  questionNineExp: string;
  questionTen: boolean | null;
  questionTenExp: string;
  questionEleven: boolean | null;
  questionElevenExp: string;
  questionTwelve: boolean | null;
  questionTwelveExp: string;
  questionThirteen: boolean | null;
  questionThirteenExp: string;
  questionFourteen: boolean | null;
  questionFourteenExp: string;
  questionFifteen: boolean | null;
  questionFifteenExp: string;
  questionSixteen: boolean | null;
  questionSixteenExp: string;
  questionSeventeen: boolean | null;
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

export type SpouseInfoType = {
  lastName: string;
  firstName: string;
  middleName: string;
  maidenName: string;
  phoneNumber: string;
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
  applicationId: string | null;
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
  documentType: string;
};

export type BackgroundCheckType = {
  proofOfID: boolean | null;
  proofOfResidency: boolean | null;
  NCICWantsWarrants: boolean | null;
  locals: boolean | null;
  probations: boolean | null;
  DMVRecord: boolean | null;
  AKSsChecked: boolean | null;
  coplink: boolean | null;
  trafficCourtPortal: boolean | null;
  propertyAssesor: boolean | null;
  voterRegistration: boolean | null;
  DOJApprovalLetter: boolean | null;
  CIINumber: boolean | null;
  DOJ: boolean | null;
  FBI: boolean | null;
  SR14: boolean | null;
  firearmsReg: boolean | null;
  allDearChiefLTRsRCRD: boolean | null;
  safetyCertificate: boolean | null;
  restrictions: boolean | null;
};

export type CompleteApplication = {
  application: {
    aliases: Array<AliasType>;
    applicationType: string;
    citizenship: {
      citizen: boolean;
      militaryStatus: string;
    };
    comments: string;
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
    paymentStatus: number;
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
    appointmentStatus: boolean;
    appointmentDateTime: string;
    orderId: string;
    uploadedDocuments: Array<UploadedDocType>;
    backgroudCheck: BackgroundCheckType;
  };
  history: Array<HistoryType>;
  userId: string;
  id: string;
};

export type ThemeConfigType = {
  isDark: boolean;
};

export type StatusType = {
  isOnline: boolean;
};

export type BrandType = {
  id?: string;
  agencyName: string;
  agencyAddress: string;
  agencyTelephone: string;
  agencyFax: string;
  agencyEmail: string;
  agencySheriffName: string;
  chiefOfPoliceName: string;
  liveScanURL: string;
  primaryThemeColor: string | VuetifyThemeItem;
  secondaryThemeColor: string | VuetifyThemeItem;
  standardCost: number;
  judicialCost: number;
  reserveCost: number;
  creditFee: number;
  convenienceFee: number;
  paymentURL: string;
  refreshTokenTime: number;
  cost: CostType;
};

export type AgencyDocumentsType = {
  agencyLogo: string | undefined;
  agencyLandingPageImage: string | undefined;
  agencySheriffSignatureImage: string | undefined;
};
