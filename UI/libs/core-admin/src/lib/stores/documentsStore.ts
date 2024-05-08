import Endpoints from '@shared-ui/api/endpoints'
import { PdfValidationType } from '@core-admin/types'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { defineStore } from 'pinia'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from './permitsStore'
import { computed, ref } from 'vue'

export const useDocumentsStore = defineStore('DocumentsStore', () => {
  const documents = ref([])
  const getDocuments = computed(() => documents.value)
  const permitStore = usePermitsStore()
  const authStore = useAuthStore()

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

  async function getUserPortrait() {
    const res = await axios.get(
      `${Endpoints.GET_USER_PORTRAIT_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_Portrait`
    )

    return res.data
  }

  async function postUploadAdminUserFile(data: FormData, target: string) {
    await axios.post(
      `${Endpoints.POST_UPLOAD_ADMIN_USER_FILE_ENDPOINT}?saveAsFileName=${target}.png`,
      data
    )
  }

  async function setUserApplicationFile(data, target) {
    const formData = new FormData()

    const newFileName = `${permitStore.permitDetail.userId}_${target}`

    formData.append('fileToUpload', data)
    const res = await axios.post(
      `${Endpoints.POST_AGENCY_DOCUMENT_FILE_ENDPOINT}?saveAsFileName=${newFileName}`,
      formData
    )

    if (res) {
      const uploadDoc: UploadedDocType = {
        documentType: target,
        name: target,
        uploadedBy: authStore.getAuthState.userEmail,
        uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
      }

      permitStore.permitDetail.application.uploadedDocuments.push(uploadDoc)
      permitStore.updatePermitDetailApi(
        `Uploaded new ${uploadDoc.documentType}`
      )
    }
  }

  async function getAdminApplicationFile(name: string) {
    const res = await axios.get(
      `${Endpoints.GET_ADMIN_APPLICATION_FILE_ENDPOINT}?adminApplicationFileName=${permitStore.permitDetail.id}_${name}`,
      { responseType: 'blob' }
    )

    setDocuments(res?.data)

    return res?.data || {}
  }

  async function deleteAdminApplicationFile(name: string) {
    await axios.delete(
      `${Endpoints.DELETE_ADMIN_APPLICATION_FILE_ENDPOINT}?adminApplicationFileName=${permitStore.permitDetail.id}_${name}`
    )
  }

  async function deleteApplicationFile(name: string) {
    await axios.delete(
      `${Endpoints.DELETE_DOCUMENT_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${name}`
    )
  }

  async function getUserDocument(name) {
    const res = await axios
      .get(
        `${Endpoints.GET_DOCUMENT_AGENCY_FILE_ENDPOINT}?applicantFileName=${permitStore.permitDetail.userId}_${name}`,
        { responseType: 'blob' }
      )

      .catch(err => {
        window.console.warn(err)
      })

    return res?.data || {}
  }

  async function editAdminApplicationFileName(oldName, newName) {
    try {
      await axios.post(
        `${Endpoints.POST_ADMIN_APPLICATION_FILE_RENAME_ENDPOINT}?oldName=${oldName}&newName=${newName}`,
        null
      )
    } catch (error) {
      console.error(error)
    }
  }

  async function editApplicationFileName(oldName, newName) {
    try {
      await axios.post(
        `${Endpoints.POST_APPLICATION_FILE_RENAME_ENDPOINT}?oldName=${oldName}&newName=${newName}`,
        null
      )
    } catch (error) {
      console.error(error)
    }
  }

  async function getPdfFormValidation(): Promise<PdfValidationType> {
    const res = await axios.get(Endpoints.GET_PDF_VALIDATION_ENDPOINT)

    return res.data as PdfValidationType
  }

  async function uploadAgencyFile(file: FormData, fileName: string) {
    try {
      await axios.post(
        `${Endpoints.POST_DOCUMENT_AGENCY_FILE_ENDPOINT}?saveAsFileName=${fileName}`,
        file
      )
    } catch (error) {
      console.error(error)
    }
  }

  return {
    documents,
    getDocuments,
    formatName,
    setUserApplicationFile,
    getUserDocument,
    getAdminApplicationFile,
    postUploadAdminUserFile,
    deleteAdminApplicationFile,
    deleteApplicationFile,
    editAdminApplicationFileName,
    editApplicationFileName,
    getUserPortrait,
    getPdfFormValidation,
    uploadAgencyFile,
  }
})
