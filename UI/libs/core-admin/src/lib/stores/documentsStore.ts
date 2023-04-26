import Endpoints from '@shared-ui/api/endpoints'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { defineStore } from 'pinia'
import { usePermitsStore } from './permitsStore'
import { computed, ref } from 'vue'

export const useDocumentsStore = defineStore('DocumentsStore', () => {
  const documents = ref([])
  const getDocuments = computed(() => documents.value)
  const permitStore = usePermitsStore()

  function setDocuments(payload) {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-ignore
    documents.value.push(payload)
  }

  function formatName(name: string): string {
    if (name) {
      return `${Endpoints.GET_DOCUMENT_AGENCY_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${name}`
    }

    return ''
  }

  async function getApplicationDocumentApi(name: string) {
    const userName = `${permitStore.permitDetail.application.personalInfo.lastName}_${permitStore.permitDetail.application.personalInfo.firstName}`
    const res = await axios.get(
      `${Endpoints.GET_DOCUMENT_AGENCY_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${userName}_${name}`
    )

    setDocuments(res?.data)

    return res?.data || {}
  }

  async function postUploadAdminUserFile(data: FormData, target: string) {
    await axios.post(
      `${Endpoints.POST_UPLOAD_ADMIN_USER_FILE_ENDPOINT}?saveAsFileName=${target}.png`,
      data
    )
  }

  async function setUserApplicationFile(data, target) {
    const formData = new FormData()

    const userName = `${permitStore.permitDetail.application.personalInfo.lastName}_${permitStore.permitDetail.application.personalInfo.firstName}`

    const newFileName = `${permitStore.permitDetail.userId}_${userName}_${target}`

    formData.append('fileToUpload', data)
    const res = await axios.post(
      `${Endpoints.POST_AGENCY_DOCUMENT_FILE_ENDPOINT}?saveAsFileName=${newFileName}`,
      formData
    )

    if (res) {
      const uploadDoc: UploadedDocType = {
        documentType: target,
        name: `${userName}_${target}`,
        uploadedBy: permitStore.permitDetail.application.userEmail,
        uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
      }

      permitStore.permitDetail.application.uploadedDocuments.push(uploadDoc)
      permitStore.updatePermitDetailApi(
        `Uploaded new ${uploadDoc.documentType}`
      )
    }
  }

  async function getUserDocument(name) {
    const res = await axios
      .get(
        `${Endpoints.GET_DOCUMENT_AGENCY_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${name}`
      )
      .catch(err => {
        window.console.warn(err)
      })

    return res
  }

  return {
    documents,
    getDocuments,
    setDocuments,
    getApplicationDocumentApi,
    formatName,
    setUserApplicationFile,
    getUserDocument,
    postUploadAdminUserFile,
  }
})
