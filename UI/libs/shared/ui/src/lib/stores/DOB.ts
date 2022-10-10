import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { DOBType } from '@shared-utils/types/defaultTypes';

export const useDOBStore = defineStore('DOB', () => {
  const DOBInfo = ref<DOBType>({
    DOB: '',
    currentAge: 0,
    birthCity: '',
    birthState: '',
    birthCountry: '',
  });
  const getDOBInfo = computed(() => DOBInfo.value);

  function setDOBInfo(payload) {
    DOBInfo.value = payload;
  }

  function deleteDOBInfo() {
    DOBInfo.value = {
      DOB: '',
      currentAge: 0,
      birthCity: '',
      birthState: '',
      birthCountry: '',
    };
  }

  return {
    DOBInfo,
    getDOBInfo,
    setDOBInfo,
    deleteDOBInfo,
  };
});
