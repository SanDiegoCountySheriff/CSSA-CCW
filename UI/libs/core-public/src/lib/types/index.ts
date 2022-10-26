import { VuetifyThemeItem } from 'vuetify/types/services/theme';

export type BrandType = {
  id?: string;
  agencyName: string;
  agencySheriffName: string;
  chiefOfPoliceName: string;
  primaryThemeColor: string | VuetifyThemeItem;
  secondaryThemeColor: string | VuetifyThemeItem;
  agencyLogoDataURL: string | undefined;
  standardCost: number;
  judicialCost: number;
  reserveCost: number;
  creditFee: number;
  convenienceFee: number;
};

export type PublicAppConfigType = {
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
