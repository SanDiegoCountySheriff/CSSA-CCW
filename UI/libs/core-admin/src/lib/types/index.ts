import { VuetifyThemeItem } from 'vuetify/types/services/theme'
import { CompleteApplication, CostType } from '@shared-utils/types/defaultTypes'

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
  orderID: string
  name: string
  address: CompleteApplication.currentAddress
  appointmentStatus: boolean
  email: string
  status: number
  isComplete: boolean
}
