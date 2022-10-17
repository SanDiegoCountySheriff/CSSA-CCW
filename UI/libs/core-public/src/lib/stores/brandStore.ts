import { defineStore } from 'pinia';
import { getCurrentInstance, ref, computed } from 'vue';
import { BrandType } from '@core-public/types';
import { useAppConfigStore } from '@shared-ui/stores/appConfig';
import axios from 'axios';

export const useBrandStore = defineStore('BrandStore', () => {
  const appConfigStore = useAppConfigStore();
  const app = getCurrentInstance();

  const brand = ref<BrandType>({
    agencyName: '',
    agencySheriffName: '',
    chiefOfPoliceName: '',
    primaryThemeColor: app.proxy.$vuetify.theme.themes.light.primary,
    secondaryThemeColor: app.proxy.$vuetify.theme.themes.light.primary,
    agencyLogoDataURL: undefined,
  });

  const getBrand = computed(() => brand.value);

  function setBrand(payload: BrandType) {
    brand.value = payload;
  }

  async function getBrandSettingApi() {
    const res = await axios
      .get(`${appConfigStore.getAppConfig.apiBaseUrl}/SystemSettings/get`)
      .catch(err => console.log(err));

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
