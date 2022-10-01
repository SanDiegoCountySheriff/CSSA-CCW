import { computed, ref } from 'vue';
import { defineStore } from 'pinia';

export const useDifferentMailingAddressStore = defineStore(
  'differentMailingAddress',
  () => {
    const differentMailingAddress = ref(false);
    const getDifferentMailingAddress = computed(
      () => differentMailingAddress.value
    );

    function setDifferentMailingAddress(payload) {
      differentMailingAddress.value = payload;
    }

    function deleteDifferentMailingAddress() {
      differentMailingAddress.value = false;
    }

    return {
      differentMailingAddress,
      getDifferentMailingAddress,
      setDifferentMailingAddress,
      deleteDifferentMailingAddress,
    };
  }
);
