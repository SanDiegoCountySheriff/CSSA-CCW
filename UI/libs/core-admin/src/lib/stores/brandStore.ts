// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import { BrandType } from '@core-admin/types';
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import { computed, getCurrentInstance, ref } from 'vue';

export const useBrandStore = defineStore('BrandStore', () => {
  const app = getCurrentInstance();

  const brand = ref<BrandType>({
    agencyName: '',
    agencySheriffName: '',
    chiefOfPoliceName: '',
    primaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.primary,
    secondaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.primary,
    agencyLogoDataURL: undefined,
  });

  const getBrand = computed(() => brand.value);

  function setBrand(payload: BrandType) {
    brand.value = payload;
  }

  function setLogoDataURL(payload) {
    brand.value.agencyLogoDataURL = payload;
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

    await axios
      .put(Endpoints.PUT_SETTINGS_ENDPOINT, data.value)
      .catch(err => window.console.log(err));
  }

  return {
    brand,
    getBrand,
    getBrandSettingApi,
    setBrandSettingApi,
    setLogoDataURL,
    setBrand,
  };
});
