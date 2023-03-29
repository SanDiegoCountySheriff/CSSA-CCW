/* eslint-disable prettier/prettier */
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import {
  AgencyDocumentsType,
  BrandType,
} from '@shared-utils/types/defaultTypes';
import { computed, getCurrentInstance, ref } from 'vue';

export const useBrandStore = defineStore('BrandStore', () => {
  const app = getCurrentInstance();

  const brand = ref<BrandType>({
    id: '00000000-0000-0000-0000-000000000000',
    agencyName: 'XXXXXXXXXXX',
    agencyAddress: 'XXXXXXXXXXX',
    agencyTelephone: 'XXXXXXXXXXX',
    agencyFax: 'XXXXXXXXXXX',
    agencyEmail: 'XXXXXXXXXXX',
    agencySheriffName: 'XXXXXXXXXXX',
    chiefOfPoliceName: 'XXXXXXXXXXX',
    primaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.primary,
    secondaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.secondary,
    liveScanURL:
      'https://www.ocsheriff.gov/sites/ocsd/files/2022-08/Livescan_Form_Fillable.pdf',
    cost: {
      new: {
        standard: 20,
        judicial: 20,
        reserve: 20,
      },
      renew: {
        standard: 77,
        judicial: 99,
        reserve: 121,
      },
      issuance: 1,
      modify: 10,
      creditFee: 3,
      convenienceFee: 5,
    },
    paymentURL: 'https://www.google.com',
    refreshTokenTime: 30,
    ori: 'XXXXXXXXXXX',
    courthouse: 'XXXXXXXXXXX',
    localAgencyNumber: 'XXXXXXXXXXX',
  });

  const documents = ref<AgencyDocumentsType>({
    agencyLogo: undefined,
    agencyLandingPageImage: undefined,
    agencySheriffSignatureImage: undefined,
  });

  const getBrand = computed(() => brand.value);
  const getDocuments = computed(() => documents.value);

  function setBrand(payload: BrandType) {
    brand.value = payload;
  }

  function setAgencyLogo(payload) {
    documents.value.agencyLogo = payload;
  }

  function setAgencyLandingPageImage(payload) {
    documents.value.agencyLandingPageImage = payload;
  }

  function setAgencySheriffSignatureImage(payload) {
    documents.value.agencySheriffSignatureImage = payload;
  }

  async function getBrandSettingApi() {
    const res = await axios
      .get(Endpoints.GET_SETTINGS_ENDPOINT)
      .catch(err => window.console.log(err));

    if (res.data) {
      setBrand(res.data);
      app.proxy.$vuetify.theme.themes.light.primary =
        res.data.primaryThemeColor;
      app?.proxy.$vuetify.theme.themes.dark.primary = '#005B96';
      app.proxy.$vuetify.theme.themes.light.secondary =
        res.data.secondaryThemeColor;
      app.proxy.$vuetify.theme.themes.dark.secondary =
        res.data.secondaryThemeColor;
    }

    return res?.data;
  }

  async function setBrandSettingApi() {
    const data = getBrand;

    await axios
      .put(Endpoints.PUT_SETTINGS_ENDPOINT, data.value)
      .catch(err => window.console.log(err));
  }

  async function getAgencyLogoDocumentsApi() {
    const res = await axios
      .get(
        `${Endpoints.GET_DOCUMENT_AGENCY_ENDPOINT}?agencyLogoName=agency_logo`
      )
      .catch(err => window.console.log(err));

    if (res.data) setAgencyLogo(res.data);

    return res?.data;
  }

  async function setAgencyLogoDocumentsApi() {
    const formData = new FormData();

    formData.append('fileToUpload', getDocuments.value.agencyLogo);
    await axios
      .post(
        `${Endpoints.POST_DOCUMENT_AGENCY_ENDPOINT}?saveAsFileName=agency_logo`,
        formData
      )
      .catch(err => window.console.log(err));
  }

  async function getAgencyLandingPageImageApi() {
    const res = await axios
      .get(
        `${Endpoints.GET_DOCUMENT_AGENCY_ENDPOINT}?agencyLogoName=agency_landing_page_image`
      )
      .catch(err => window.console.log(err));

    if (res.data) setAgencyLandingPageImage(res.data);

    return res?.data;
  }

  async function setAgencyLandingPageImageApi() {
    const formData = new FormData();

    formData.append('fileToUpload', getDocuments.value.agencyLandingPageImage);
    await axios
      .post(
        `${Endpoints.POST_DOCUMENT_AGENCY_ENDPOINT}?saveAsFileName=agency_landing_page_image`,
        formData
      )
      .catch(err => window.console.log(err));
  }

  async function getAgencySheriffSignatureImageApi() {
    const res = await axios
      .get(
        `${Endpoints.GET_DOCUMENT_AGENCY_ENDPOINT}?agencyLogoName=agency_sheriff_signature_image`
      )
      .catch(err => window.console.log(err));

    if (res.data) setAgencySheriffSignatureImage(res.data);

    return res?.data;
  }

  async function setAgencySheriffSignatureImageApi() {
    const formData = new FormData();

    formData.append(
      'fileToUpload',
      getDocuments.value.agencySheriffSignatureImage
    );
    await axios
      .post(
        `${Endpoints.POST_DOCUMENT_AGENCY_ENDPOINT}?saveAsFileName=agency_sheriff_signature_image`,
        formData
      )
      .catch(err => window.console.log(err));
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
    setAgencyLogoDocumentsApi,
    getAgencyLandingPageImageApi,
    setAgencyLandingPageImageApi,
    getAgencySheriffSignatureImageApi,
    setAgencySheriffSignatureImageApi,
    setBrand,
  };
});
