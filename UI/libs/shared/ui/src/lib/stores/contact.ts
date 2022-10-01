import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { ContactInfoType } from '@shared-ui/types/defaultTypes';

export const useContactStore = defineStore('contact', () => {
  const contact = ref<ContactInfoType>({
    primaryPhone: '',
    cellPhone: '',
    workPhone: '',
    faxNumber: '',
    textMessageUpdates: false,
  });
  const getContactInfo = computed(() => contact.value);

  function setContactInfo(payload: ContactInfoType) {
    contact.value = payload;
  }

  function deleteContactInfo() {
    contact.value = {
      primaryPhone: '',
      cellPhone: '',
      workPhone: '',
      faxNumber: '',
      textMessageUpdates: false,
    };
  }

  return {
    contact,
    getContactInfo,
    setContactInfo,
    deleteContactInfo,
  };
});
