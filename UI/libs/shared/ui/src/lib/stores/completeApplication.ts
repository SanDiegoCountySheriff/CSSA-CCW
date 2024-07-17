import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { defaultPermitState } from '@shared-utils/lists/defaultConstants'
import { defineStore } from 'pinia'
import { useAuthStore } from '@shared-ui/stores/auth'
import { computed, reactive, ref } from 'vue'

export const useCompleteApplicationStore = defineStore('permitStore', () => {
  const blankApplication = JSON.parse(JSON.stringify(defaultPermitState))
  const authStore = useAuthStore()
  const completeApplication = reactive<CompleteApplication>(blankApplication)
  const allUserApplications = ref<Array<CompleteApplication>>()

  const getCompleteApplication = computed(() => completeApplication)
  const getAllUserApplications = computed(() => allUserApplications)

  /**
   * Used to set the stored value from either the api call or the form
   * @param {CompleteApplication} payload
   */
  function setCompleteApplication(payload: CompleteApplication) {
    completeApplication.application = payload.application
    completeApplication.history = payload.history
    completeApplication.id = payload.id
    completeApplication.paymentHistory = payload.paymentHistory
    completeApplication.isMatchUpdated = payload.isMatchUpdated
  }

  function setAllUserApplications(payload: Array<CompleteApplication>) {
    allUserApplications.value = payload
  }

  /**
   * Get the complete application from the backend
   */
  async function getCompleteApplicationFromApi(
    applicationId: string | null,
    isComplete: boolean
  ) {
    const res = await axios
      .get(Endpoints.GET_PERMIT_ENDPOINT, {
        params: {
          applicationId,
          isComplete,
        },
      })
      .catch(err => {
        console.warn(err)
        Promise.reject()
      })

    return res?.data || {}
  }

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
      .catch(err => {
        window.console.log(err)

        return Promise.reject()
      })
  }

  /**
   * Get all applications by the user
   * @param userEmail
   * @returns {Promise<void>}
   */
  async function getAllUserApplicationsApi() {
    const res = await axios.get(Endpoints.GET_ALL_BY_USER_ENDPOINT, {
      params: {
        userEmail: authStore.auth.userEmail,
      },
    })

    if (res?.data) setAllUserApplications(res.data)

    return res
  }

  async function createApplication() {
    await axios
      .put(Endpoints.PUT_CREATE_PERMIT_ENDPOINT, completeApplication)
      .then(res => {
        setCompleteApplication(res.data)
      })
      .catch(err => {
        window.console.log(err)

        return Promise.reject()
      })
  }

  async function getAgreementPdf(fileName: string) {
    try {
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
    } catch (error) {
      console.error('Error getting PDF:', error)
    }
  }

  async function updateApplication(updateReason: string) {
    const res = await axios
      .put(
        `${Endpoints.PUT_UPDATE_PERMIT_ENDPOINT}?updateReason=${updateReason}`,
        completeApplication
      )
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  async function matchUserInformation(
    idNumber: string,
    dateOfBirth: string
  ): Promise<boolean> {
    const res = await axios
      .get(
        `${Endpoints.MATCH_USER_INFORMATION_ENDPOINT}?idNumber=${idNumber}&dateOfBirth=${dateOfBirth}`
      )
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

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
    addHistoricalApplicationPublic,
    allUserApplications,
    completeApplication,
    createApplication,
    getCompleteApplication,
    getAllUserApplications,
    setCompleteApplication,
    setAllUserApplications,
    getCompleteApplicationFromApi,
    getAllUserApplicationsApi,
    updateApplication,
    getAgreementPdf,
    matchUserInformation,
    withdrawRenewal,
  }
})
