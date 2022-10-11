import { defineStore } from 'pinia';
import { getCurrentInstance, ref, computed } from 'vue';
import { BrandType } from '@core-public/types';
import { usePublicAppConfigStore } from '@core-public/stores/publicAppConfig';
import axios from 'axios';

export const useBrandStore = defineStore('BrandStore', () => {
  const publicConfigStore = usePublicAppConfigStore();
  const app = getCurrentInstance();

  const brand = ref<BrandType>({
    agencyName: '',
    agencySheriffName: '',
    chiefOfPoliceName: '',
    primaryThemeColor: app.proxy.$vuetify.theme.themes.light.primary,
    secondaryThemeColor: app.proxy.$vuetify.theme.themes.light.primary,
    agencyLogoDataURL: null,
  });

  const getBrand = computed(() => brand.value);

  function setBrand(payload: BrandType) {
    brand.value = payload;
  }

  async function getBrandSettingApi() {
    const res = await axios
      .get(
        `${publicConfigStore.getPublicAppConfig.apiBaseUrl}/SystemSettings/get?applicationId=efd83276-ee14-4532-ad27-85915f5ea88f`
      )
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
