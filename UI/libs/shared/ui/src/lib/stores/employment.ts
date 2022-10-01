import { computed, ref } from 'vue';
import { defineStore } from 'pinia';

export const useEmploymentStore = defineStore('employment', () => {
  const employment = ref('');
  const getEmployment = computed(() => employment.value);

  function setEmployment(payload) {
    employment.value = payload;
  }

  function deleteEmployment() {
    employment.value = '';
  }

  return {
    employment,
    getEmployment,
    setEmployment,
    deleteEmployment,
  };
});
