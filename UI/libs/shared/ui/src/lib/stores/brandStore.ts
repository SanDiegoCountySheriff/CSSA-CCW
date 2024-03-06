// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { defineStore } from 'pinia'
import {
  AgencyDocumentsType,
  BrandType,
} from '@shared-utils/types/defaultTypes'
import { computed, getCurrentInstance, ref } from 'vue'

export const useBrandStore = defineStore('BrandStore', () => {
  const app = getCurrentInstance()
  const defaultEyeColors = [
    { name: 'Black' },
    { name: 'Brown' },
    { name: 'Blue' },
    { name: 'Green' },
    { name: 'Multicolor' },
  ]
  const defaultHairColors = [
    { name: 'Black' },
    { name: 'Brown' },
    { name: 'Blonde' },
    { name: 'Grey' },
    { name: 'Red' },
    { name: 'Bald' },
    { name: 'Multicolor' },
  ]

  const brand = ref<BrandType>({
    id: '00000000-0000-0000-0000-000000000000',
    agencyName: '',
    agencyTelephone: '',
    agencyFax: '',
    agencyEmail: '',
    agencySheriffName: '',
    chiefOfPoliceName: '',
    agencyStreetAddress: '',
    agencyAptBuildingNumber: '',
    agencyCity: '',
    agencyState: '',
    agencyZip: '',
    agencyCounty: '',
    agencyShippingStreetAddress: '',
    agencyShippingAptBuildingNumber: '',
    agencyShippingCity: '',
    agencyShippingState: '',
    agencyShippingZip: '',
    agencyShippingCounty: '',
    agencyBillingNumber: '',
    contactName: '',
    contactNumber: '',
    mailCode: '',
    primaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.primary,
    secondaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.secondary,
    cost: {
      new: {
        standard: 20,
        judicial: 20,
        reserve: 20,
        employment: 20,
      },
      renew: {
        standard: 77,
        judicial: 99,
        reserve: 121,
        employment: 55,
      },
      issuance: 1,
      modify: 10,
      creditFee: 3,
      convenienceFee: 5,
    },
    paymentURL: 'https://www.google.com',
    refreshTokenTime: 30,
    ori: '',
    courthouse: '',
    employmentLicense: false,
    localAgencyNumber: '',
    expiredApplicationRenewalPeriod: 0,
    archivedApplicationRetentionPeriod: 0,
    agencyHairColors: [],
    agencyEyeColors: [],
  })

  const documents = ref<AgencyDocumentsType>({
    agencyLogo: undefined,
    agencyLandingPageImage: undefined,
    agencySheriffSignatureImage: undefined,
    agencyGoodMoralPDF: undefined,
    agencyConditionsForIssuancePDF: undefined,
    agencyFalseInfoPDF: undefined,
    agencyHomePageImage: undefined,
  })

  const getBrand = computed(() => brand.value)
  const getDocuments = computed(() => documents.value)

  function setBrand(payload: BrandType) {
    brand.value = payload

    if (
      brand.value.agencyEyeColors?.length === 0 ||
      !brand.value.agencyEyeColors
    ) {
      brand.value.agencyEyeColors = defaultEyeColors
    }

    if (
      brand.value.agencyHairColors?.length === 0 ||
      !brand.value.agencyHairColors
    ) {
      brand.value.agencyHairColors = defaultHairColors
    }
  }

  function setAgencyLogo(payload) {
    documents.value.agencyLogo = payload
  }

  function setAgencySheriffSignatureImage(payload) {
    documents.value.agencySheriffSignatureImage = payload
  }

  async function getBrandSettingApi() {
    const res = await axios.get(Endpoints.GET_SETTINGS_ENDPOINT).catch(() => {
      brand.value.agencyEyeColors = defaultEyeColors
      brand.value.agencyHairColors = defaultHairColors
    })

    if (res?.data) {
      setBrand(res.data)
      app.proxy.$vuetify.theme.themes.light.primary = res.data.primaryThemeColor
      app?.proxy.$vuetify.theme.themes.dark.primary =
        res.data.secondaryThemeColor
      app.proxy.$vuetify.theme.themes.light.secondary =
        res.data.secondaryThemeColor
      app.proxy.$vuetify.theme.themes.dark.secondary =
        res.data.secondaryThemeColor
    }

    return res?.data
  }

  async function setBrandSettingApi() {
    const data = getBrand

    await axios
      .put(Endpoints.PUT_SETTINGS_ENDPOINT, data.value)
      .catch(err => window.console.log(err))
  }

  async function getAgencyLogoDocumentsApi() {
    const res = await axios
      .get(`${Endpoints.GET_DOCUMENT_AGENCY_ENDPOINT}`)
      .catch(err => window.console.log(err))

    if (res?.data) setAgencyLogo(res.data)

    return res?.data
  }

  async function setAgencyDocument(document: FormData, documentName: string) {
    await axios
      .post(
        `${Endpoints.POST_DOCUMENT_AGENCY_ENDPOINT}?saveAsFileName=${documentName}`,
        document
      )
      .catch(err => window.console.log(err))
  }

  async function getAgencySheriffSignatureImageApi() {
    const res = await axios
      .get(`${Endpoints.GET_DOCUMENT_AGENCY_SIGNATURE_ENDPOINT}`)
      .catch(err => window.console.log(err))

    if (res?.data) setAgencySheriffSignatureImage(res.data)

    return res?.data
  }

  return {
    brand,
    documents,
    getBrand,
    getDocuments,
    setAgencyLogo,
    getBrandSettingApi,
    setBrandSettingApi,
    getAgencyLogoDocumentsApi,
    getAgencySheriffSignatureImageApi,
    setBrand,
    setAgencyDocument,
  }
})
