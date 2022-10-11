import { defineStore } from 'pinia';
import { getCurrentInstance, ref, computed } from 'vue';
import { BrandType } from '@core-admin/types';
import { useAdminAppConfigStore } from '@core-admin/stores/adminAppConfig';
import axios from 'axios';

export const useBrandStore = defineStore('BrandStore', () => {
  const adminConfigStore = useAdminAppConfigStore();
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

  function setLogoDataURL(payload) {
    brand.value.agencyLogoDataURL = payload;
  }

  async function getBrandSettingApi() {
    const res = await axios
      .get(
        `${adminConfigStore.getAdminAppConfig.apiBaseUrl}/SystemSettings/get?applicationId=efd83276-ee14-4532-ad27-85915f5ea88f`
      )
      .catch(err => console.log(err));

    setBrand(res.data);

    return res.data;
  }

  async function setBrandSettingApi() {
    const data = getBrand;
    data.value.id = 'efd83276-ee14-4532-ad27-85915f5ea88f';

    await axios
      .put(
        `${adminConfigStore.getAdminAppConfig.apiBaseUrl}/SystemSettings/create`,
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
