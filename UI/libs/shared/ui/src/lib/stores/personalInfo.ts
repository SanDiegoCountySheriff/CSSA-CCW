import { computed, ref } from 'vue';
import { defineStore } from 'pinia';

export const usePersonalInfoStore = defineStore('personalInfo', () => {
  const personalInfo = ref({
    lastName: '',
    firstName: '',
    middleName: '',
    maidenName: '',
    suffix: '',
    ssn: '',
    maritalStatus: '',
  });
  const getPersonalInfo = computed(() => personalInfo.value);

  function setPersonalInfo(payload) {
    personalInfo.value = payload;
  }

  function deletePersonalInfo() {
    personalInfo.value = {
      lastName: '',
      firstName: '',
      middleName: '',
      maidenName: '',
      suffix: '',
      ssn: '',
      maritalStatus: '',
    };
  }

  return {
    personalInfo,
    getPersonalInfo,
    setPersonalInfo,
    deletePersonalInfo,
  };
});
