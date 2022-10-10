import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { IdType } from '@shared-utils/types/defaultTypes';

export const useIdStore = defineStore('id', () => {
  const id = ref<IdType>({
    idNumber: '',
    issuingState: '',
  });
  const getId = computed(() => id.value);

  function setId(payload) {
    id.value = payload;
  }

  function deleteId() {
    id.value = {
      idNumber: '',
      issuingState: '',
    };
  }

  return {
    id,
    getId,
    setId,
    deleteId,
  };
});
