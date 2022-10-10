import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { BrandType } from '@core-admin/types';

export const useBrandStore = defineStore('BrandStore', () => {
  const brand = ref<BrandType>({
    agencyName: '',
    agencySheriffName: '',
    chiefOfPoliceName: '',
    primaryThemeColor: '',
    secondaryThemeColor: '',
    agencyLogo: null,
    agencyLogoDataURL: null,
  });
  const getBrand = computed(() => brand.value);

  function setBrand(payload: BrandType) {
    brand.value = payload;
  }

  return { brand, getBrand, setBrand };
});
