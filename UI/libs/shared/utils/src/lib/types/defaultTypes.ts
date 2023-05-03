import { VuetifyThemeItem } from 'vuetify/types/services/theme'

export type QuestionsConfig = {
  one: number
  two: number
  three: number
  four: number
  five: number
  six: number
  seven: number
  eight: number
  nine: number
  ten: number
  eleven: number
  twelve: number
  thirteen: number
  fourteen: number
  fifteen: number
  sixteen: number
  seventeen: number
}

export type AddressInfoType = {
  addressLine1: string
  addressLine2: string
  city: string
  county: string
  state: string
  zip: string
  country: string
}

export type AliasType = {
  prevLastName: string
  prevFirstName: string
  prevMiddleName: string
  cityWhereChanged: string
  stateWhereChanged: string
  courtFileNumber: string
}

export type AppConfigType = {
  apiBaseUrl: string
  adminApiBaseUrl: string
  applicationApiBaseUrl: string
  documentApiBaseUrl: string
  paymentApiBaseUrl: string
  scheduleApiBaseUrl: string
  userProfileApiBaseUrl: string
  apiSubscription: string
  authorityUrl: string
  knownAuthorities: Array<string>
  clientId: string
  defaultCounty: string
  displayDebugger: boolean
  environmentName: string
  refreshTime: number
  questions: QuestionsConfig
}

export type AppearanceInfoType = {
  gender: string
  heightFeet: string
  heightInch: string
  weight: string
  hairColor: string
  eyeColor: string
  physicalDesc: string
}

export type UploadedDocType = {
  name: string
  uploadedDateTimeUtc: string
  uploadedBy: string
  documentType: string
}

export type AdminUserType = {
  id?: string
  badgeNumber: string
  uploadedDocuments: Array<UploadedDocType>
}

export type AuthType = {
  id?: string
  userName: string
  userEmail: string
  jwtToken: string
  isAuthenticated: boolean
  isAdmin: boolean
  verifiedUser: boolean
  roles: Array<string>
  sessionStarted: string
  tokenExpired: boolean
}

export type CitizenshipType = {
  citizen: boolean
  militaryStatus: string
}

export type ContactInfoType = {
  primaryPhoneNumber: string
  cellPhoneNumber: string
  workPhoneNumber: string
  faxPhoneNumber: string
  textMessageUpdates: boolean
}

export type CostType = {
  new: {
    standard: number
    judicial: number
    reserve: number
  }
  renew: {
    standard: number
    judicial: number
    reserve: number
  }
  issuance: number
  modify: number
  creditFee: number
  convenienceFee: number
}

export type DOBType = {
  birthDate: string
  birthCity: string
  birthState: string
  birthCountry: string
}

export type HistoryType = {
  change: string
  changeDateTimeUtc: string
  changeMadeBy: string
}

export type IdType = {
  idNumber: string
  issuingState: string
}

export type ImmigrantInformation = {
  countryOfCitizenship: string
  countryOfBirth: string
  immigrantAlien: boolean
  nonImmigrantAlien: boolean
}

export type LicenseType = {
  permitNumber: string
  issuingCounty: string
  expirationDate: string
  issueDate: string
}

export type QualifyingQuestions = {
  questionOne: boolean | null
  questionOneAgency: string
  questionOneIssueDate: string
  questionOneNumber: string
  questionTwo: boolean | null
  questionTwoExp: string
  questionThree: boolean | null
  questionThreeExp: string
  questionFour: boolean | null
  questionFourExp: string
  questionFive: boolean | null
  questionFiveExp: string
  questionSix: boolean | null
  questionSixExp: string
  questionSeven: boolean | null
  questionSevenExp: string
  questionEight: boolean | null
  questionEightExp: string
  questionNine: boolean | null
  questionNineExp: string
  questionTen: boolean | null
  questionTenExp: string
  questionEleven: boolean | null
  questionElevenExp: string
  questionTwelve: boolean | null
  questionTwelveExp: string
  questionThirteen: boolean | null
  questionThirteenExp: string
  questionFourteen: boolean | null
  questionFourteenExp: string
  questionFifteen: boolean | null
  questionFifteenExp: string
  questionSixteen: boolean | null
  questionSixteenExp: string
  questionSeventeen: boolean | null
  questionSeventeenExp: string
}

export type PaymentInfoType = {
  applicationCost: number
  convenienceFee: number
  creditFee: number
  totalCost: number
}

export type PersonalInfoType = {
  lastName: string
  firstName: string
  middleName: string
  noMiddleName: boolean
  maidenName: string
  suffix: string
  ssn: string
  maritalStatus: string
}

export type SpouseInfoType = {
  lastName: string
  firstName: string
  middleName: string
  maidenName: string
  phoneNumber: string
}

export type PaymentHistoryType = {
  amount: string
  paymentDateTimeUtc: string
  paymentType: string
  recordedBy: string
  transactionId: string
  vendorInfo: string
}

export type WeaponInfoType = {
  make: string
  model: string
  caliber: string
  serialNumber: string
}

export type WorkInformationType = {
  employerName: string
  employerAddressLine1: string
  employerAddressLine2: string
  employerCity: string
  employerState: string
  employerZip: string
  employerCountry: string
  employerPhone: string
  occupation: string
}

export type AppointmentType = {
  id: string
  applicationId: string | null
  status: string
  name: string
  permit: string
  payment: string
  date: string
  time: string
  start: string
  end: string
  isManuallyCreated: boolean
}

export type BackgroundCheckType = {
  proofOfID: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  proofOfResidency: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  ncicWantsWarrants: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  locals: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  probations: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  dmvRecord: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  akSsChecked: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  coplink: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  trafficCourtPortal: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  propertyAssesor: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  voterRegistration: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  dojApprovalLetter: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  ciiNumber: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  doj: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  fbi: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  sR14: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  firearmsReg: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  allDearChiefLTRsRCRD: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  safetyCertificate: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  restrictions: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
}

export type CompleteApplication = {
  application: {
    aliases: Array<AliasType>
    applicationType: string
    citizenship: {
      citizen: boolean
      militaryStatus: string
    }
    comments: string
    contact: ContactInfoType
    currentAddress: AddressInfoType
    differentMailing: boolean
    differentSpouseAddress: boolean
    dob: DOBType
    employment: string
    idInfo: IdType
    immigrantInformation: ImmigrantInformation
    isComplete: boolean
    license: LicenseType
    mailingAddress: AddressInfoType
    paymentStatus: number
    personalInfo: {
      lastName: string
      firstName: string
      middleName: string
      noMiddleName: boolean
      maidenName: string
      suffix: string
      ssn: string
      maritalStatus: string
    }
    physicalAppearance: AppearanceInfoType
    previousAddresses: Array<AddressInfoType>
    qualifyingQuestions: QualifyingQuestions
    spouseAddressInformation: AddressInfoType
    spouseInformation: SpouseInfoType
    userEmail: string
    weapons: Array<WeaponInfoType>
    workInformation: WorkInformationType
    currentStep: number
    status: number
    appointmentStatus: boolean
    appointmentDateTime: string
    orderId: string
    uploadedDocuments: Array<UploadedDocType>
    backgroundCheck: BackgroundCheckType
    startOfNinetyDayCountdown: string
  }
  history: Array<HistoryType>
  paymentHistory: Array<PaymentHistoryType>
  userId: string
  id: string
}

export type ThemeConfigType = {
  isDark: boolean
}

export type StatusType = {
  isOnline: boolean
}

export type BrandType = {
  id?: string
  agencyName: string
  agencyAddress: string
  agencyTelephone: string
  agencyFax: string
  agencyEmail: string
  agencySheriffName: string
  chiefOfPoliceName: string
  liveScanURL: string
  primaryThemeColor: string | VuetifyThemeItem
  secondaryThemeColor: string | VuetifyThemeItem
  standardCost: number
  judicialCost: number
  reserveCost: number
  creditFee: number
  convenienceFee: number
  paymentURL: string
  refreshTokenTime: number
  ori: string
  courthouse: string
  localAgencyNumber: string
  cost: CostType
}

export type AgencyDocumentsType = {
  agencyLogo: string | undefined
  agencyLandingPageImage: string | undefined
  agencySheriffSignatureImage: string | undefined
}

export type AppointmentManagement = {
  daysOfTheWeek: Array<string>
  firstAppointmentStartTime: string
  lastAppointmentStartTime: string
  numberOfSlotsPerAppointment: number
  appointmentLength: number
  numberOfWeeksToCreate: number
  breakLength: number | undefined
  breakStartTime: string | undefined
}
