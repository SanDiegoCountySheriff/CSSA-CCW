import { VuetifyThemeItem } from 'vuetify/types/services/theme'
import {
  ApplicationStatus,
  ApplicationType,
  AppointmentStatus,
  CostType,
} from '@shared-utils/types/defaultTypes'

export type BrandType = {
  id?: string
  agencyName: string
  agencySheriffName: string
  chiefOfPoliceName: string
  agencyStreetAddress: string
  agencyTelephone: string
  agencyFax: string
  agencyEmail: string
  primaryThemeColor: string | VuetifyThemeItem
  secondaryThemeColor: string | VuetifyThemeItem
  cost: CostType
  paymentURL: string
  refreshTokenTime: number
  standardCost: number
  judicialCost: number
  reserveCost: number
  employmentCost: number
  creditFee: number
  convenienceFee: number
  ori: string
  courthouse: string
  employmentLicense: boolean
  localAgencyNumber: string
  agencyBillingNumber: string
  contactName: string
  contactNumber: string
  mailCode: string
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
}

export type AgencyDocumentsType = {
  agencyLogo: string | undefined
  agencyLandingPageImage: string | undefined
}

export type AdminAppConfigType = {
  apiBaseUrl: string
  apiSubscription: string
  authorityUrl: string
  knownAuthorities: Array<string>
  clientId: string
  defaultCounty: string
  displayDebugger: boolean
  environmentName: string
  loginType: string
  refreshTime: number
  applicationInsightsConnectionString: string
}

export type PermitsType = {
  id: string
  orderId: string
  name: string
  appointmentStatus: AppointmentStatus
  appointmentDateTime: string
  paid: boolean
  initials: string
  status: ApplicationStatus
  isComplete: boolean
  applicationType: ApplicationType
  assignedTo: string
  appointmentId: string
}

export type LegacyPermitsType = {
  id: string
  orderId: string
  name: string
  idNumber: string
  birthDate: string
  permitNumber: string
  email: string
  appointmentDateTime: string
  status: ApplicationStatus
  applicationType: ApplicationType
}

export type PdfValidationType = {
  BOF_4012_rev_01_2024: boolean
  BOF_4502_rev_09_2011: boolean
  BOF_1032_rev_01_2024: boolean
  BOF_1031_orig_01_2024: boolean
  BOF_8018_rev_01_2024: boolean
  BCIA_8016_rev_04_2020: boolean
  BOF_1027_rev_01_2024: boolean
  BOF_1034_orig_01_2024: boolean
  BCIA_8020_rev_01_2014: boolean
  Prohibiting_Categories_rev_01_2024: boolean
  Official_License: boolean
  Unofficial_License: boolean
  Conditions_for_Issuance: boolean
  False_Info: boolean
}
