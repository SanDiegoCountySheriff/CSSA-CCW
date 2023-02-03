import Endpoints from '@shared-ui/api/endpoints';
import { UploadedDocType } from '@shared-utils/types/defaultTypes';
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
      return `${Endpoints.GET_DOCUMENT_AGENCY_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${name}`;
    }

    return '';
  }

  async function getApplicationDocumentApi(name: string) {
    const res = await axios.get(
      `${Endpoints.GET_DOCUMENT_AGENCY_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${name}`
    );

    setDocuments(res?.data);

    return res?.data || {};
  }

  async function setUserApplicationFile(data, target) {
    const formData = new FormData();

    const newFileName = `${permitStore.permitDetail.userId}_${target}`;

    formData.append('fileToUpload', data);
    const res = await axios.post(
      `${Endpoints.POST_AGENCY_DOCUMENT_FILE_ENDPOINT}?saveAsFileName=${newFileName}`,
      formData
    );

    if (res) {
      const uploadDoc: UploadedDocType = {
        documentType: target,
        name: `${newFileName}`,
        uploadedBy: permitStore.permitDetail.application.userEmail,
        uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
      };

      permitStore.permitDetail.application.uploadedDocuments.push(uploadDoc);
      permitStore.updatePermitDetailApi();
    }
  }

  return {
    documents,
    getDocuments,
    setDocuments,
    getApplicationDocumentApi,
    formatName,
    setUserApplicationFile,
  };
});
