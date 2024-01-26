import { VuetifyThemeItem } from 'vuetify/types/services/theme'
import {
  AddressInfoType,
  ApplicationStatus,
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
  creditFee: number
  convenienceFee: number
  ori: string
  courthouse: string
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
}

export type PermitsType = {
  orderId: string
  name: string
  currentAddress: AddressInfoType
  appointmentStatus: AppointmentStatus
  userEmail: string
  status: ApplicationStatus
  isComplete: boolean
  applicationType: string
  assignedTo: string
}

export type PdfValidationType = {
  BOF_4012_rev_01_2024: boolean
  'BOF 4502 (rev. 09/2011)': boolean
  'BOF 1032 (rev. 01/2024)': boolean
  'BOF 1031 (orig. 01/2024)': boolean
  'BOF 8018 (rev. 01/2024)': boolean
  'BCIA 8016 (rev. 04/2020)': boolean
  'Official License': boolean
  'Unofficial License': boolean
  'Conditions for Issuance': boolean
  'False Info': boolean
  'Good Moral Character': boolean
}
