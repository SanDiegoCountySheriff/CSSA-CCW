import Endpoints from '@shared-ui/api/endpoints'
import { PermitsType } from '@core-admin/types'
import axios from 'axios'
import { defaultPermitState } from '@shared-utils/lists/defaultConstants'
import { defineStore } from 'pinia'
import { useAuthStore } from '@shared-ui/stores/auth'
import {
  AppointmentStatus,
  CompleteApplication,
  HistoryType,
} from '@shared-utils/types/defaultTypes'
import { computed, ref } from 'vue'
import {
  formatAddress,
  formatDate,
  formatInitials,
  formatName,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

export const usePermitsStore = defineStore('PermitsStore', () => {
  const authStore = useAuthStore()
  const permits = ref<Array<PermitsType>>()
  const openPermits = ref<number>(0)
  const permitDetail = ref<CompleteApplication>(defaultPermitState)
  const history = ref(defaultPermitState.history)
  const searchResults = ref([])

  const getPermits = computed(() => permits.value)
  const getSearchResults = computed(() => searchResults.value)
  const getOpenPermits = computed(() => openPermits.value)
  const getPermitDetail = computed(() => permitDetail.value)
  const getHistory = computed(() => history.value)

  const orderIds = new Map()

  function setPermits(payload: Array<PermitsType>) {
    permits.value = payload
  }

  function setOpenPermits(payload: number) {
    openPermits.value = payload
  }

  function setPermitDetail(payload: CompleteApplication) {
    permitDetail.value = payload
  }

  function setSearchResults(payload) {
    searchResults.value = payload
  }

  function setHistory(payload: Array<HistoryType>) {
    history.value = payload
  }

  async function getAllPermitsApi() {
    const res = await axios
      .get(Endpoints.GET_ALL_PERMITS_ENDPOINT)
      .catch(err => window.console.log(err))

    window.console.log(res?.data)

    const permitsData: Array<PermitsType> = res?.data?.map(data => ({
      ...data,
      status: 'New',
      appointmentStatus: AppointmentStatus[data.appointmentStatus],
      initials: formatInitials(data.firstName, data.lastName),
      name: formatName(data),
      address: formatAddress(data),
      rowClass: 'permits-table__row',
      appointmentDateTime: `${formatTime(
        data.appointmentDateTime
      )} on ${formatDate(data.appointmentDateTime)}`,
    }))

    setOpenPermits(permitsData.length)
    setPermits(permitsData)

    return permitsData
  }

  async function getPermitDetailApi(orderId: string) {
    let isComplete = false

    if (permits.value) {
      isComplete =
        permits.value.filter(item => item.orderId === orderId)[0]?.isComplete ||
        false
    }

    const res = await axios.get(
      `${Endpoints.GET_AGENCY_PERMIT_ENDPOINT}?userEmailOrOrderId=${orderId}&isOrderId=true&isComplete=${isComplete}`
    )

    orderIds.set(orderId, res?.data || {})
    setPermitDetail(res?.data)

    return res?.data || {}
  }

  async function getHistoryApi() {
    const orderId = permitDetail.value.application.orderId

    const res = await axios.get(
      `${Endpoints.GET_PERMIT_HISTORY_ENDPOINT}?applicationIdOrOrderId=${orderId}&isOrderId=true`
    )

    const array: HistoryType[] = res?.data
    const reorder = array.reverse()

    setHistory(reorder)

    return res?.data || {}
  }

  async function printApplicationApi() {
    const applicationId = permitDetail.value.id

    const res = await axios({
      url: `${Endpoints.GET_PRINT_APPLICATION_ENDPOINT}?applicationId=${applicationId}`,
      method: 'PUT',
      responseType: 'blob',
    })

    return res || {}
  }

  async function printOfficialLicenseApi() {
    const applicationId = permitDetail.value.id

    const res = await axios({
      // change to true if if need to download the pdf.
      url: `${Endpoints.GET_PRINT_OFFICIAL_LICENSE_ENDPOINT}?applicationId=${applicationId}&shouldAddDownloadFilename=false`,
      method: 'PUT',
      responseType: 'blob',
    })

    return res || {}
  }

  async function printUnofficialLicenseApi() {
    const applicationId = permitDetail.value.id

    const res = await axios({
      // change to true if if need to download the pdf.
      url: `${Endpoints.GET_PRINT_UNOFFICIAL_LICENSE_ENDPOINT}?applicationId=${applicationId}&shouldAddDownloadFilename=false`,
      method: 'PUT',
      responseType: 'blob',
    })

    return res || {}
  }

  async function printLiveScanApi() {
    const applicationId = permitDetail.value.id

    const res = await axios({
      // change to true if if need to download the pdf.
      url: `${Endpoints.GET_PRINT_LIVE_SCAN_ENDPOINT}?applicationId=${applicationId}&shouldAddDownloadFilename=false`,
      method: 'PUT',
      responseType: 'blob',
    })

    return res || {}
  }

  async function updatePermitDetailApi(item: string) {
    permitDetail.value.history.push({
      changeMadeBy: authStore.auth.userEmail,
      change: item,
      changeDateTimeUtc: new Date().toISOString(),
    })

    const res = await axios.put(
      Endpoints.PUT_UPDATE_AGENCY_PERMIT_ENDPOINT,
      permitDetail.value,
      {
        params: {
          updatedSection: item,
        },
      }
    )

    if (res?.data) setPermitDetail(res.data)

    return res?.data || {}
  }

  async function getPermitSearchApi(query) {
    const res = await axios.get(
      `${Endpoints.GET_AGENCY_SEARCH_ENDPOINT}?searchValue=${query}`
    )

    if (res?.data) setSearchResults(res.data)

    return res?.data || {}
  }

  async function getPermitSsn(id: string) {
    const res = await axios
      .get(Endpoints.GET_PERMIT_SSN_ENDPOINT, {
        params: {
          userId: id,
        },
      })
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  return {
    permits,
    searchResults,
    openPermits,
    permitDetail,
    getPermits,
    getOpenPermits,
    getPermitDetail,
    getSearchResults,
    getHistory,
    setPermits,
    setOpenPermits,
    setSearchResults,
    setPermitDetail,
    getAllPermitsApi,
    getPermitDetailApi,
    getPermitSearchApi,
    getHistoryApi,
    getPermitSsn,
    printApplicationApi,
    printOfficialLicenseApi,
    printUnofficialLicenseApi,
    printLiveScanApi,
    updatePermitDetailApi,
  }
})
