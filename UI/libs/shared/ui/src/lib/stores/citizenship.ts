import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { CitizenshipType } from '@shared-utils/types/defaultTypes';

export const useCitizenshipStore = defineStore('citizenship', () => {
  const citizenshipInfo = ref<CitizenshipType>({
    citizen: false,
    militaryStatus: '',
  });
  const getCitizenshipInfo = computed(() => citizenshipInfo.value);

  function setCitizenshipInfo(payload) {
    citizenshipInfo.value = payload;
  }

  function deleteCitizenshipInfo() {
    citizenshipInfo.value = {
      citizen: false,
      militaryStatus: '',
    };
  }

  return {
    citizenshipInfo,
    getCitizenshipInfo,
    setCitizenshipInfo,
    deleteCitizenshipInfo,
  };
});
