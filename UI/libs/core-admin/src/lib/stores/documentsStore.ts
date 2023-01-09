import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import { usePermitsStore } from './permitsStore';
import { computed, ref } from 'vue';

export const useDocumentsStore = defineStore('DocumentsStore', () => {
  const documents = ref([]);
  const getDocuments = computed(() => documents.value);
  const permitStore = usePermitsStore();

  function setDocuments(payload) {
    documents.value.push(payload);
  }

  function formatName(name: string): string {
    if (name) {
      return `${Endpoints.GET_DOCUMENT_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${name}`;
    }

    return '';
  }

  async function getApplicationDocumentApi(name: string) {
    const res = await axios.get(
      `${Endpoints.GET_DOCUMENT_AGENCY_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${name}
      }`
    );

    setDocuments(res?.data);

    return res?.data || {};
  }

  return {
    documents,
    getDocuments,
    setDocuments,
    getApplicationDocumentApi,
    formatName,
  };
});
