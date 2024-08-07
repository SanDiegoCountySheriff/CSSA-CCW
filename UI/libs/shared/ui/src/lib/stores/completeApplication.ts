import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import { defaultPermitState } from '@shared-utils/lists/defaultConstants'
import { defineStore } from 'pinia'
import { ref } from 'vue'
import axios, { AxiosError } from 'axios'

export const useCompleteApplicationStore = defineStore('permitStore', () => {
  const completeApplication = ref<CompleteApplication>(defaultPermitState)
  const publicUpdateFailed = ref(false)
  const publicUpdateFailedItem = ref('')

  async function addHistoricalApplicationPublic(
    application: CompleteApplication
  ) {
    await axios
      .put(
        Endpoints.PUT_ADD_HISTORICAL_APPLICATION_PUBLIC_ENDPOINT,
        application
      )
      .then(res => {
        return res.data
      })
  }

  async function getUserApplication() {
    const res = await axios.get(Endpoints.GET_ALL_BY_USER_ENDPOINT)

    if (res?.data) {
      completeApplication.value = res?.data
    }

    return res
  }

  async function createApplication() {
    await axios
      .put(Endpoints.PUT_CREATE_PERMIT_ENDPOINT, completeApplication.value)
      .then(res => {
        completeApplication.value = res.data
      })
  }

  async function getAgreementPdf(fileName: string) {
    const response = await axios.get(
      `${Endpoints.GET_AGREEMENT_PDF_ENDPOINT}?fileName=${fileName}`,
      {
        responseType: 'arraybuffer',
      }
    )
    const blob = new Blob([response.data], { type: 'application/pdf' })

    // eslint-disable-next-line node/no-unsupported-features/node-builtins
    const pdfUrl = URL.createObjectURL(blob)

    window.open(pdfUrl, '_blank')
  }

  async function updateApplication(updateReason: string) {
    await axios
      .put(
        `${Endpoints.PUT_UPDATE_PERMIT_ENDPOINT}?updateReason=${updateReason}`,
        completeApplication.value
      )
      .then(response => {
        completeApplication.value = response.data
      })
      .catch((error: AxiosError) => {
        if (error.response?.status === 412) {
          publicUpdateFailed.value = true
          publicUpdateFailedItem.value = updateReason
        }
      })
  }

  async function matchUserInformation(
    idNumber: string,
    dateOfBirth: string
  ): Promise<boolean> {
    const res = await axios.get(
      `${Endpoints.MATCH_USER_INFORMATION_ENDPOINT}?idNumber=${idNumber}&dateOfBirth=${dateOfBirth}`
    )

    if (res.data) {
      return res.data
    }

    return false
  }

  async function withdrawRenewal() {
    const res = await axios.post(Endpoints.WITHDRAW_RENEWAL_ENDPOINT)

    return res?.data
  }

  return {
    completeApplication,
    addHistoricalApplicationPublic,
    createApplication,
    getUserApplication,
    updateApplication,
    getAgreementPdf,
    matchUserInformation,
    withdrawRenewal,
  }
})
