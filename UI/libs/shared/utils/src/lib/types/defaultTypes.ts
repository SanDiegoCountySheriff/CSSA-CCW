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
  reasonForNameChange: string
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
  name: string
  jobTitle: string
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
  handlingRedirectPromise: boolean
}

export type CitizenshipType = {
  citizen: boolean
  militaryStatus: string
}

export type ContactInfoType = {
  primaryPhoneNumber: string
  cellPhoneNumber: string
  workPhoneNumber: string
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

export type TrafficViolation = {
  date: string
  violation: string
  agency: string
  citationNumber: string
}

export type QualifyingQuestionStandard = {
  selected: boolean | null
  explanation: string
  temporaryExplanation: string
}

export type QualifyingQuestionOne = {
  selected: boolean | null
  agency: string
  temporaryAgency: string
  issueDate: string
  temporaryIssueDate: string
  number: string
  temporaryNumber: string
}

export type QualifyingQuestionTwo = {
  selected: boolean | null
  agency: string
  temporaryAgency: string
  denialDate: string
  temporaryDenialDate: string
  denialReason: string
  temporaryDenialReason: string
}

export type QualifyingQuestionEight = {
  selected: boolean | null
  trafficViolations: TrafficViolation[]
  temporaryTrafficViolations: TrafficViolation[]
}

export type QualifyingQuestions = {
  questionOne: QualifyingQuestionOne
  questionTwo: QualifyingQuestionTwo
  questionThree: QualifyingQuestionStandard
  questionFour: QualifyingQuestionStandard
  questionFive: QualifyingQuestionStandard
  questionSix: QualifyingQuestionStandard
  questionSeven: QualifyingQuestionStandard
  questionEight: QualifyingQuestionEight
  questionNine: QualifyingQuestionStandard
  questionTen: QualifyingQuestionStandard
  questionEleven: QualifyingQuestionStandard
  questionTwelve: QualifyingQuestionStandard
  questionThirteen: QualifyingQuestionStandard
  questionFourteen: QualifyingQuestionStandard
  questionFifteen: QualifyingQuestionStandard
  questionSixteen: QualifyingQuestionStandard
  questionSeventeen: QualifyingQuestionStandard
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
  successful: boolean
  paymentStatus: number
}

export type WeaponInfoType = {
  make: string
  model: string
  caliber: string
  serialNumber: string
}

export type CharacterReferenceType = {
  name: string
  relationship: string
  phoneNumber: string
  email: string
}

export type CharacterReferences = CharacterReferenceType[]

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

export enum AppointmentStatus {
  'Available',
  'Not Scheduled',
  'Scheduled',
  'Checked In',
  'No Show',
}

export type AppointmentType = {
  id: string
  applicationId: string | null
  status: AppointmentStatus
  name: string
  permit: string
  payment: string
  date: string
  time: string
  start: string
  appointmentCreatedDate: string
  end: string
  isManuallyCreated: boolean
  userId: string
}

export type HolidayRequestModel = {
  name: string
}

export type OrganizationalHolidaysRequestModel = {
  holidayRequestModels: HolidayRequestModel[]
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
  akasChecked: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  crimeTracer: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  trafficCourtPortal: {
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
  firearms: {
    changeDateTimeUtc: null
    changeMadeBy: null
    value: boolean | null
  }
  sidLettersReceived: {
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

export type CommentType = {
  text: string
  commentDateTimeUtc: string
  commentMadeBy: string
}

export enum ApplicationStatus {
  'None',
  'Incomplete',
  'Submitted',
  'Ready For Appointment',
  'Appointment Complete',
  'Background In Progress',
  'Contingently Approved',
  'Approved',
  'Permit Delivered',
  'Suspended',
  'Revoked',
  'Canceled',
  'Denied',
  'Withdrawn',
  'Flagged For Review',
  'Appointment No Show',
  'Contingently Denied',
  'Ready To Issue',
  'Waiting For Customer',
}

export enum PaymentType {
  'CCW Application Initial Payment',
  'CCW Application Initial Judicial Payment',
  'CCW Application Initial Reserve Payment',
  'CCW Application Modification Payment',
  'CCW Application Modification Judicial Payment',
  'CCW Application Modification Reserve Payment',
  'CCW Application Renewal Payment',
  'CCW Application Renewal Judicial Payment',
  'CCW Application Renewal Reserve Payment',
}

export type CompleteApplication = {
  application: {
    aliases: Array<AliasType>
    applicationType: string
    citizenship: {
      citizen: boolean
      militaryStatus: string
    }
    characterReferences: CharacterReferences
    comments: Array<CommentType>
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
    referenceNotes: string
    spouseAddressInformation: {
      addressLine1: string
      addressLine2: string
      city: string
      county: string
      state: string
      zip: string
      country: string
      reason: string
    }
    spouseInformation: SpouseInfoType
    userEmail: string
    weapons: Array<WeaponInfoType>
    workInformation: WorkInformationType
    currentStep: number
    status: ApplicationStatus
    originalStatus: ApplicationStatus
    appointmentStatus: AppointmentStatus | null
    appointmentDateTime: string | null
    appointmentId: string | null
    orderId: string
    uploadedDocuments: Array<UploadedDocType>
    adminUploadedDocuments: Array<UploadedDocType>
    backgroundCheck: BackgroundCheckType
    startOfNinetyDayCountdown: string | null
    ninetyDayCountdownPaused: boolean
    ninetyDayCountdownPausedDate: string | null
    ciiNumber: string
    cost: CostType
    submittedToLicensingDateTime: string | null
    assignedTo: string | null
    flaggedForCustomerReview: boolean | null
    flaggedForLicensingReview: boolean | null
    agreements: {
      goodMoralCharacterAgreed: boolean
      goodMoralCharacterAgreedDate: string | null
      conditionsForIssuanceAgreed: boolean
      conditionsForIssuanceAgreedDate: string | null
      falseInfoAgreed: boolean
      falseInfoAgreedDate: string | null
    }
  }
  history: Array<HistoryType>
  paymentHistory: Array<PaymentHistoryType>
  userId: string
  id: string
}

export type ThemeConfigType = {
  isDark: boolean
}

export type StausType = {
  isOnline: boolean
}

export type BrandType = {
  id?: string
  agencyName: string
  agencyTelephone: string
  agencyFax: string
  agencyEmail: string
  agencySheriffName: string
  chiefOfPoliceName: string
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
  agencyBillingNumber: string
  contactName: string
  contactNumber: string
  mailCode: string
  agencyStreetAddress: string
  agencyAptBuildingNumber: string
  agencyCity: string
  agencyState: string
  agencyZip: string
  agencyCounty: string
  agencyShippingStreetAddress: string
  agencyShippingAptBuildingNumber: string
  agencyShippingCity: string
  agencyShippingState: string
  agencyShippingZip: string
  agencyShippingCounty: string
  expiredApplicationRenewalPeriod: number
  archivedApplicationRetentionPeriod: number
}

export type AgencyDocumentsType = {
  agencyLogo: string | undefined
  agencyLandingPageImage: string | undefined
  agencySheriffSignatureImage: string | undefined
  agencyGoodMoralPDF: string | undefined
  agencyConditionsForIssuancePDF: string | undefined
  agencyFalseInfoPDF: string | undefined
  agencyHomePageImage: string | undefined
}

export type AppointmentManagement = {
  daysOfTheWeek: Array<string>
  firstAppointmentStartTime: string
  lastAppointmentStartTime: string
  numberOfSlotsPerAppointment: number
  appointmentLength: number
  numberOfWeeksToCreate: number
  breakLength: number | undefined
  breakStartTime: string | null
  startDate: string
}

export type AppointmentWindowCreateRequestModel = {
  start: string
  end: string
  applicationId: string | null
  userId: string
  status: AppointmentStatus | null
  name: string | null
  permit: string | null
  payment: string | null
  isManuallyCreated: boolean
  appointmentCreatedDate: string | null
}
