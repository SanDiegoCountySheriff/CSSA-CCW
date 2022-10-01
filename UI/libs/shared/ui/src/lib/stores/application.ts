import { computed, ref } from 'vue';
import { defineStore } from 'pinia';

export const useApplicationStore = defineStore('application', () => {
  const applicationType = ref('');
  const getApplicationType = computed(() => applicationType.value);

  function setApplicationType(payload) {
    applicationType.value = payload;
  }

  function deleteApplicationType() {
    applicationType.value = '';
  }

  return {
    applicationType,
    getApplicationType,
    setApplicationType,
    deleteApplicationType,
  };
});
