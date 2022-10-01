import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { AddressInfoType } from '@shared-ui/types/defaultTypes';

export const usePreviousAddressesStore = defineStore(
  'previousAddresses',
  () => {
    const previousAddresses = ref<AddressInfoType>([]);
    const getPreviousAddresses = computed(() => previousAddresses.value);

    function setPreviousAddresses(payload: AddressInfoType) {
      previousAddresses.value = payload;
    }

    function addPreviousAddresses(previousAddress: oneOf<AddressInfoType>) {
      previousAddresses.unshift(previousAddress);
    }

    function updatePreviousAddresses(previousAddress: oneOf<AddressInfoType>) {
      const index = previousAddresses.value.find(previousAddress);
      previousAddresses.value.splice(index, 1, previousAddresses.value);
    }

    function deletePreviousAddresses(previousAddress: oneOf<AddressInfoType>) {
      const index = previousAddresses.value.find(previousAddress);
      previousAddresses.splice(index, 1);
    }

    return {
      previousAddresses,
      getPreviousAddresses,
      setPreviousAddresses,
      addPreviousAddresses,
      updatePreviousAddresses,
      deletePreviousAddresses,
    };
  }
);
