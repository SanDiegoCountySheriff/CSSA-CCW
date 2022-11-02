import { BrandType } from '@core-public/types';
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
    primaryThemeColor: app.proxy.$vuetify.theme.themes.light.primary,
    secondaryThemeColor: app.proxy.$vuetify.theme.themes.light.secondary,
    agencyLogoDataURL: undefined,
    standardCost: 113,
    judicialCost: 135,
    reserveCost: 157,
    creditFee: 0.05,
    convenienceFee: 5,
    paymentURL: 'https://buy.stripe.com/test_fZe00ugG5guI0YU4gg',
  });

  const getBrand = computed(() => brand.value);

  function setBrand(payload: BrandType) {
    brand.value = payload;
  }

  async function getBrandSettingApi() {
    const res = await axios
      .get(Endpoints.GET_SETTINGS_ENDPOINT)
      .catch(err => window.console.log(err));

    setBrand(res.data);
    app.proxy.$vuetify.theme.themes.light.primary = res.data.primaryThemeColor;
    app.proxy.$vuetify.theme.themes.light.secondary =
      res.data.secondaryThemeColor;

    return res.data;
  }

  return {
    brand,
    getBrand,
    getBrandSettingApi,
  };
});
