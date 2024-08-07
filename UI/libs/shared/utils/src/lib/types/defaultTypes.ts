import { DataOptions } from 'vuetify'
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
  eighteen: number
  nineteen: number
  twenty: number
  twentyone: number
}

export type AddressInfoType = {
  streetAddress: string
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
  isPaymentServiceAvailable: boolean
  environmentName: string
  refreshTime: number
  questions: QuestionsConfig
  payBeforeSubmit: boolean
  applicationInsightsConnectionString: string
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

export type UserType = {
  id: string
  email: string
  firstName: string
  lastName: string
  middleName: string
  dateOfBirth: string
  driversLicenseNumber: string
  permitNumber: string
  appointmentDate: string
  appointmentTime: string
  uploadedDocuments: Array<UploadedDocType>
  isPendingReview: boolean
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
    employment: number
  }
  renew: {
    standard: number
    judicial: number
    reserve: number
    employment: number
  }
  issuance: number
  modify: number
  creditFee: number
  convenienceFee: number
  standardLivescanFee?: number | null
  reserveLivescanFee?: number | null
  judicialLivescanFee?: number | null
  employmentLivescanFee?: number | null
}

export type DenialInfoType = {
  reason: string
  otherReason: string
  date: string
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
  restrictions: string
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
  expirationDate: string | null
  issueDate: string | null
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
  renewalExplanation: string
  updateInformation: boolean | null
}

export type QualifyingQuestionOne = {
  selected: boolean | null
  agency: string
  temporaryAgency: string
  issueDate: string
  temporaryIssueDate: string
  number: string
  temporaryNumber: string
  issuingState: string
  temporaryIssuingState: string
  updateInformation: boolean | null
}

export type QualifyingQuestionTwo = {
  selected: boolean | null
  agency: string
  temporaryAgency: string
  denialDate: string
  temporaryDenialDate: string
  denialReason: string
  temporaryDenialReason: string
  updateInformation: boolean | null
}

export type QualifyingQuestionTwelve = {
  selected: boolean | null
  trafficViolations: TrafficViolation[]
  temporaryTrafficViolations: TrafficViolation[]
  trafficViolationsExplanation: string
  updateInformation: boolean | null
}

export type QualifyingQuestionEight = {
  selected: boolean | null
  trafficViolations: TrafficViolation[]
  temporaryTrafficViolations: TrafficViolation[]
  trafficViolationsExplanation: string
  updateInformation: boolean | null
}

export type QualifyingQuestions = {
  questionOne: QualifyingQuestionOne
  questionTwo: QualifyingQuestionTwo
  questionThree: QualifyingQuestionStandard
  questionFour: QualifyingQuestionStandard
  questionFive: QualifyingQuestionStandard
  questionSix: QualifyingQuestionStandard
  questionSeven: QualifyingQuestionStandard
  questionEight: QualifyingQuestionStandard
  questionNine: QualifyingQuestionStandard
  questionTen: QualifyingQuestionStandard
  questionEleven: QualifyingQuestionStandard
  questionTwelve: QualifyingQuestionTwelve
  questionThirteen: QualifyingQuestionStandard
  questionFourteen: QualifyingQuestionStandard
  questionFifteen: QualifyingQuestionStandard
  questionSixteen: QualifyingQuestionStandard
  questionSeventeen: QualifyingQuestionStandard
  questionEighteen: QualifyingQuestionStandard
  questionNineteen: QualifyingQuestionStandard
  questionTwenty: QualifyingQuestionStandard
  questionTwentyOne: QualifyingQuestionStandard
}

export type LegacyQualifyingQuestions = {
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

export type RefundRequest = {
  id: string | null | undefined
  orderId: string | null | undefined
  transactionId: string
  applicationId: string
  refundAmount: number
  reason: string | null | undefined
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

export type RevocationInfoType = {
  reason: string
  otherReason: string
  date: string
}

export type SpouseInfoType = {
  lastName: string
  firstName: string
  middleName: string
  maidenName: string
  phoneNumber: string
}

export enum PaymentStatus {
  'None',
  'In Person',
  'Online Submitted',
  'Refunded',
  'Partially Refunded',
}

export type PaymentHistoryType = {
  amount: string
  refundAmount: string
  paymentDateTimeUtc: string
  paymentType: number
  recordedBy: string
  transactionId: string
  vendorInfo: string
  successful: boolean
  paymentStatus: PaymentStatus
  modificationNumber: number | null
  verified: boolean | null
}

export type WeaponInfoType = {
  make: string
  model: string
  caliber: string
  serialNumber: string
  added?: boolean
  deleted?: boolean
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
  employerStreetAddress: string
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
  slots?: number | null
}

export type HolidayRequestModel = {
  name: string
}

export type OrganizationalHolidaysRequestModel = {
  holidayRequestModels: HolidayRequestModel[]
}

export type BackgroundCheckType = {
  proofOfID: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  proofOfResidency: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  ncicWantsWarrants: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  locals: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  probations: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  dmvRecord: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  akasChecked: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  crimeTracer: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  trafficCourtPortal: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  dojApprovalLetter: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  ciiNumber: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  doj: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  fbi: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  livescan: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  sR14: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  firearms: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  sidLettersReceived: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  safetyCertificate: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
  restrictions: {
    changeDateTimeUtc: string | null
    changeMadeBy: string | null
    value: boolean | null
  }
}

export type CommentType = {
  text: string
  commentDateTimeUtc: string
  commentMadeBy: string
}

export type LiveScanInfoType = {
  atiNumber: string
  date: string
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
  'Modification Approved',
  'Renewal Approved',
  'Modification Denied',
}

export enum PaymentType {
  'CCW Application Initial Payment',
  'CCW Application Initial Judicial Payment',
  'CCW Application Initial Reserve Payment',
  'CCW Application Initial Employment Payment',
  'CCW Application Modification Payment',
  'CCW Application Modification Judicial Payment',
  'CCW Application Modification Reserve Payment',
  'CCW Application Modification Employment Payment',
  'CCW Application Renewal Payment',
  'CCW Application Renewal Judicial Payment',
  'CCW Application Renewal Reserve Payment',
  'CCW Application Renewal Employment Payment',
  'CCW Application Issuance Payment',
  'CCW Application Standard Livescan Payment',
  'CCW Application Judicial Livescan Payment',
  'CCW Application Reserve Livescan Payment',
  'CCW Application Employment Livescan Payment',
}

export enum ApplicationType {
  'None',
  'Standard',
  'Reserve',
  'Judicial',
  'Employment',
  'Renew Standard',
  'Renew Reserve',
  'Renew Judicial',
  'Renew Employment',
  'Modify Standard',
  'Modify Reserve',
  'Modify Judicial',
  'Modify Employment',
  'Duplicate Standard',
  'Duplicate Reserve',
  'Duplicate Judicial',
  'Duplicate Employment',
}

export type HistoricalApplicationSummary = {
  id: string
  historicalDate: string | null
  applicationType: ApplicationType
}

export type AssignedApplicationSummary = {
  orderId: string
  name: string
  status: ApplicationStatus
  appointmentStatus: AppointmentStatus
}

export type ApplicationSummaryCount = {
  standardType: number
  reserveType: number
  judicialType: number
  employmentType: number
  suspendedStatus: number
  revokedStatus: number
  deniedStatus: number
  activeStandardStatus: number
  activeReserveStatus: number
  activeJudicialStatus: number
  activeEmploymentStatus: number
  submittedStatus: number
}

export type ApplicationTableOptionsType = {
  options: DataOptions
  search: string
  applicationSearch: string | null
  statuses: ApplicationStatus[]
  paid: boolean
  appointmentStatuses: AppointmentStatus[]
  applicationTypes: ApplicationType[]
  showingTodaysAppointments: boolean
  selectedDate: string
  matchedApplications: boolean
}

export type CompleteApplication = {
  application: {
    aliases: Array<AliasType>
    applicationType: ApplicationType
    citizenship: {
      citizen: boolean
      militaryStatus: string
    }
    characterReferences: CharacterReferences
    comments: Array<CommentType>
    contact: ContactInfoType
    currentAddress: AddressInfoType
    modifiedAddress: AddressInfoType
    modifiedAddressComplete: boolean | null
    denialInfo: DenialInfoType
    differentMailing: boolean
    differentSpouseAddress: boolean
    dob: DOBType
    employment: string
    idInfo: IdType
    immigrantInformation: ImmigrantInformation
    isComplete: boolean
    isUpdatingApplication: boolean
    license: LicenseType
    liveScanInfo: LiveScanInfoType
    mailingAddress: AddressInfoType
    paymentStatus: number
    personalInfo: {
      lastName: string
      firstName: string
      middleName: string
      modifiedFirstName: string
      modifiedMiddleName: string
      modifiedLastName: string
      noMiddleName: boolean
      maidenName: string
      suffix: string
      ssn: string
      maritalStatus: string
    }
    modifiedNameComplete: boolean | null
    physicalAppearance: AppearanceInfoType
    previousAddresses: Array<AddressInfoType>
    qualifyingQuestions?: QualifyingQuestions
    legacyQualifyingQuestions?: LegacyQualifyingQuestions
    referenceNotes: string
    revocationInfo: RevocationInfoType
    spouseAddressInformation: {
      streetAddress: string
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
    modifyDeleteWeapons: Array<WeaponInfoType>
    modifyAddWeapons: Array<WeaponInfoType>
    modifiedWeaponComplete: boolean | null
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
    modificationSubmittedToLicensingDateTime: string | null
    assignedTo: string | null
    flaggedForCustomerReview: boolean | null
    flaggedForLicensingReview: boolean | null
    agreements: {
      conditionsForIssuanceAgreed: boolean
      conditionsForIssuanceAgreedDate: string | null
      falseInfoAgreed: boolean
      falseInfoAgreedDate: string | null
    }
    readyForInitialPayment: boolean
    readyForRenewalPayment: boolean
    readyForModificationPayment: boolean
    readyForIssuancePayment: boolean
    modificationNumber: number
    renewalNumber: number
    isRenewingWithLegacyQuestions: boolean
  }
  history: Array<HistoryType>
  paymentHistory: Array<PaymentHistoryType>
  historicalDate: string | null | undefined
  userId: string
  id: string
  isMatchUpdated: boolean | null
}

export type ThemeConfigType = {
  isDark: boolean
}

export type StausType = {
  isOnline: boolean
}

export type HairColor = {
  name: string
}

export type EyeColor = {
  name: string
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
  employmentCost: number
  creditFee: number
  convenienceFee: number
  paymentURL: string
  refreshTokenTime: number
  ori: string
  courthouse: string
  employmentLicense: boolean
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
  agencyHairColors: HairColor[]
  agencyEyeColors: EyeColor[]
  daysBeforeActiveRenewal: number
  numberOfModificationsBetweenRenewals: number
  licensingManager: string
}

export type AgencyDocumentsType = {
  agencyLogo: string | undefined
  agencyLandingPageImage: string | undefined
  agencySheriffSignatureImage: string | undefined
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
  breakLength: number | undefined | null
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
