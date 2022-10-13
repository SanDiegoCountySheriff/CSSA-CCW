// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import { defineStore } from 'pinia';
import { getCurrentInstance, ref, computed } from 'vue';
import { BrandType } from '@core-admin/types';
import { useAppConfigStore } from '@shared-ui/stores/appConfig';
import axios from 'axios';

export const useBrandStore = defineStore('BrandStore', () => {
  const appConfigStore = useAppConfigStore();
  const app = getCurrentInstance();

  const brand = ref<BrandType>({
    agencyName: '',
    agencySheriffName: '',
    chiefOfPoliceName: '',
    primaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.primary,
    secondaryThemeColor: app?.proxy?.$vuetify.theme.themes.light.primary,
    agencyLogoDataURL: null,
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
      .get(`${appConfigStore.getAppConfig.apiBaseUrl}/SystemSettings/get`)
      .catch(err => console.log(err));

    setBrand(res?.data);

    return res?.data;
  }

  async function setBrandSettingApi() {
    const data = getBrand;

    await axios
      .put(
        `${appConfigStore.getAppConfig.apiBaseUrl}/SystemSettings/update`,
        data.value
      )
      .catch(err => console.log(err));
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
