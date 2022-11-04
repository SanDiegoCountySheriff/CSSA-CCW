/* eslint-disable prettier/prettier */
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import { AgencyDocumentsType, BrandType } from '@core-admin/types';
import { computed, getCurrentInstance, ref } from 'vue';

export const useBrandStore = defineStore('BrandStore', () => {
  const app = getCurrentInstance();

  const brand = ref<BrandType>({
    agencyName: '',
    agencySheriffName: '',
    chiefOfPoliceName: '',
    primaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.primary,
    secondaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.secondary,
    standardCost: 113.0,
    judicialCost: 135.0,
    reserveCost: 157.0,
    creditFee: 0.05,
    convenienceFee: 5.0,
    paymentURL: 'https://buy.stripe.com/test_fZe00ugG5guI0YU4gg',
    refreshTokenTime: 30,
  });

  const documents = ref<AgencyDocumentsType>({
    agencyLogo: undefined,
    agencyLandingPageImage: undefined,
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

  async function getBrandSettingApi() {
    const res = await axios
      .get(Endpoints.GET_SETTINGS_ENDPOINT)
      .catch(err => window.console.log(err));

    if (res.data) setBrand(res.data);

    return res?.data;
  }

  async function setBrandSettingApi() {
    const data = getBrand;

    const res = await axios
      .put(Endpoints.PUT_SETTINGS_ENDPOINT, data.value)
      .catch(err => window.console.log(err));

    return res?.data;
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

    formData.append('fileToPersist', getDocuments.value.agencyLogo);
    const res = await axios
      .post(
        `${Endpoints.POST_DOCUMENT_AGENCY_ENDPOINT}?saveAsFileName=agency_logo`,
        formData
      )
      .catch(err => window.console.log(err));

    return res?.data;
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

    formData.append('fileToPersist', getDocuments.value.agencyLandingPageImage);
    const res = await axios
      .post(
        `${Endpoints.POST_DOCUMENT_AGENCY_ENDPOINT}?saveAsFileName=agency_landing_page_image`,
        formData
      )
      .catch(err => window.console.log(err));

    return res?.data || {};
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
    setBrand,
  };
});
