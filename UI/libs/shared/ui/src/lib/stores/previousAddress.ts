import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { AddressInfoType } from '@shared-ui/types/defaultTypes';

export const usePreviousAddressesStore = defineStore(
  'previousAddresses',
  () => {
    const previousAddresses = ref<Array<AddressInfoType>>([]);
    const getPreviousAddresses = computed(() => previousAddresses.value);

    function setPreviousAddresses(payload: Array<AddressInfoType>) {
      previousAddresses.value = payload;
    }

    function updatePreviousAddresses(previousAddress: AddressInfoType) {
      const index = previousAddresses.value.indexOf(previousAddress);
      previousAddresses.value.splice(index, 1, previousAddresses.value);
    }

    function deletePreviousAddresses(previousAddress: AddressInfoType) {
      const index = previousAddresses.value.find(previousAddress);
      previousAddresses.splice(index, 1);
    }

    return {
      previousAddresses,
      getPreviousAddresses,
      setPreviousAddresses,
      updatePreviousAddresses,
      deletePreviousAddresses,
    };
  }
);
