import { computed, ref } from 'vue';
import { defineStore } from 'pinia';

export const useCurrentAddressStore = defineStore('currentAddress', () => {
  const currentAddress = ref({
    addressLine1: '',
    addressLine2: '',
    city: '',
    county: '',
    state: '',
    // need to decide if this is a string or a number
    zip: '',
    country: '',
  });
  const getCurrentAddress = computed(() => currentAddress.value);

  function setCurrentAddress(payload) {
    currentAddress.value = payload;
  }

  function deleteCurrentAddress() {
    currentAddress.value = {
      addressLine1: '',
      addressLine2: '',
      city: '',
      county: '',
      state: '',
      // need to decide if this is a string or a number
      zip: '',
      country: '',
    };
  }

  return {
    currentAddress,
    getCurrentAddress,
    setCurrentAddress,
    deleteCurrentAddress,
  };
});
