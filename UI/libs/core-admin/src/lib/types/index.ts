import { VuetifyThemeItem } from 'vuetify/types/services/theme'
import { CompleteApplication, CostType } from '@shared-utils/types/defaultTypes'

export type BrandType = {
  id?: string
  agencyName: string
  agencySheriffName: string
  chiefOfPoliceName: string
  agencyAddress: string
  agencyTelephone: string
  agencyFax: string
  agencyEmail: string
  primaryThemeColor: string | VuetifyThemeItem
  secondaryThemeColor: string | VuetifyThemeItem
  cost: CostType
  paymentURL: string
  refreshTokenTime: number
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
