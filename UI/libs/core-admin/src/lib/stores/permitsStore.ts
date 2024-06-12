import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { defaultPermitState } from '@shared-utils/lists/defaultConstants'
import { defineStore } from 'pinia'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import {
  ApplicationStatus,
  ApplicationTableOptionsType,
  ApplicationType,
  AssignedApplicationSummary,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'
import {
  ApplicationSummaryCount,
  AppointmentStatus,
  CompleteApplication,
  HistoryType,
} from '@shared-utils/types/defaultTypes'
import { LegacyPermitsType, PermitsType } from '@core-admin/types'
import { computed, ref } from 'vue'
import {
  formatDate,
  formatInitials,
  formatName,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

export const usePermitsStore = defineStore('PermitsStore', () => {
  const authStore = useAuthStore()
  const permits = ref<Array<PermitsType>>()
  const summaryCount = ref<ApplicationSummaryCount>()
  const assignedApplicationsSummary = ref<AssignedApplicationSummary[]>()
  const openPermits = ref<number>(0)
  const permitDetail = ref<CompleteApplication>(defaultPermitState)
  const history = ref(defaultPermitState.history)
  const viewingPermitDetail = ref(false)
  const searchResults = ref([])
  const brandStore = useBrandStore()

  const getPermits = computed(() => permits.value)
  const getSearchResults = computed(() => searchResults.value)
  const getOpenPermits = computed(() => openPermits.value)
  const getPermitDetail = computed(() => permitDetail.value)
  const getHistory = computed(() => history.value)
  const options = ref<ApplicationTableOptionsType>({
    options: {
      page: 1,
      itemsPerPage: 10,
      sortBy: [],
      sortDesc: [],
      groupBy: [],
      groupDesc: [],
      multiSort: false,
      mustSort: false,
    },
    statuses: [2],
    search: '',
    paid: false,
    appointmentStatuses: [],
    applicationTypes: [],
    showingTodaysAppointments: false,
    selectedDate: '',
    applicationSearch: null,
    matchedApplications: false,
  })
  const legacyOptions = ref<ApplicationTableOptionsType>({
    options: {
      page: 1,
      itemsPerPage: 10,
      sortBy: [],
      sortDesc: [],
      groupBy: [],
      groupDesc: [],
      multiSort: false,
      mustSort: false,
    },
    statuses: [],
    search: '',
    paid: false,
    appointmentStatuses: [],
    applicationTypes: [],
    showingTodaysAppointments: false,
    selectedDate: '',
    applicationSearch: null,
    matchedApplications: false,
  })
  const undoOptions = ref<ApplicationTableOptionsType>({
    options: {
      page: 1,
      itemsPerPage: 10,
      sortBy: [],
      sortDesc: [],
      groupBy: [],
      groupDesc: [],
      multiSort: false,
      mustSort: false,
    },
    statuses: [],
    search: '',
    paid: false,
    appointmentStatuses: [],
    applicationTypes: [],
    showingTodaysAppointments: false,
    selectedDate: '',
    applicationSearch: null,
    matchedApplications: true,
  })

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

    const permitsData: Array<PermitsType> = res?.data?.map(data => {
      const permitsType: PermitsType = {
        id: data.id,
        orderId: data.orderId,
        status: ApplicationStatus[ApplicationStatus[data.status]],
        applicationType: ApplicationType[ApplicationType[data.applicationType]],
        appointmentStatus:
          AppointmentStatus[AppointmentStatus[data.appointmentStatus]],
        paid: data.paid,
        initials: formatInitials(data.firstName, data.lastName),
        name: formatName(data),
        assignedTo: data.assignedTo,
        appointmentDateTime: data.appointmentDateTime
          ? `${formatDate(data.appointmentDateTime)} at ${formatTime(
              data.appointmentDateTime
            )}`
          : 'None',
        isComplete: data.isComplete,
        appointmentId: data.appointmentId,
      }

      return permitsType
    })

    setOpenPermits(permitsData.length)
    setPermits(permitsData)

    return permitsData
  }

  async function getApplicationSummaryCount() {
    const res = await axios.get(
      Endpoints.GET_APPLICATION_SUMMARY_COUNT_ENDPOINT
    )

    summaryCount.value = { ...res?.data }

    return res?.data
  }

  async function getAssignedApplicationsSummary() {
    const res = await axios.get(Endpoints.GET_ASSIGNED_APPLICATIONS_ENDPOINT)

    assignedApplicationsSummary.value = res?.data

    return res?.data
  }

  async function getAllPermitsSummary(
    signal: AbortSignal | undefined
  ): Promise<{
    items: Array<PermitsType>
    total: number
  }> {
    const res = await axios.get(Endpoints.GET_ALL_PERMITS_SUMMARY_ENDPOINT, {
      signal,
      params: {
        page: options.value.options.page,
        itemsPerPage: options.value.options.itemsPerPage,
        sortBy: options.value.options.sortBy,
        sortDesc: options.value.options.sortDesc,
        groupBy: options.value.options.groupBy,
        groupDesc: options.value.options.groupDesc,
        statuses: options.value.statuses,
        appointmentStatuses: options.value.appointmentStatuses,
        applicationTypes: options.value.applicationTypes,
        search: options.value.search,
        showingTodaysAppointments: options.value.showingTodaysAppointments,
        selectedDate: options.value.selectedDate
          ? new Date(options.value.selectedDate).toISOString()
          : '',
        matchedApplications: options.value.matchedApplications,
      },
      paramsSerializer: {
        indexes: null,
      },
    })

    const permitsData: Array<PermitsType> = res?.data?.items.map(data => {
      const permitsType: PermitsType = {
        id: data.id,
        orderId: data.orderId,
        status: ApplicationStatus[ApplicationStatus[data.status]],
        applicationType: ApplicationType[ApplicationType[data.applicationType]],
        appointmentStatus:
          AppointmentStatus[AppointmentStatus[data.appointmentStatus]],
        paid: data.paid,
        initials: formatInitials(data.firstName, data.lastName),
        name: formatName(data),
        assignedTo: data.assignedTo,
        appointmentDateTime: data.appointmentDateTime
          ? `${formatDate(data.appointmentDateTime)} at ${formatTime(
              data.appointmentDateTime
            )}`
          : 'None',
        isComplete: data.isComplete,
        appointmentId: data.appointmentId,
      }

      return permitsType
    })

    res.data.items = permitsData

    return res.data
  }

  async function getUndoPermitsSummary(
    signal: AbortSignal | undefined
  ): Promise<{
    items: Array<PermitsType>
    total: number
  }> {
    const res = await axios.get(Endpoints.GET_ALL_PERMITS_SUMMARY_ENDPOINT, {
      signal,
      params: {
        page: undoOptions.value.options.page,
        itemsPerPage: undoOptions.value.options.itemsPerPage,
        sortBy: undoOptions.value.options.sortBy,
        sortDesc: undoOptions.value.options.sortDesc,
        groupBy: undoOptions.value.options.groupBy,
        groupDesc: undoOptions.value.options.groupDesc,
        statuses: undoOptions.value.statuses,
        appointmentStatuses: undoOptions.value.appointmentStatuses,
        applicationTypes: undoOptions.value.applicationTypes,
        search: undoOptions.value.search,
        showingTodaysAppointments: undoOptions.value.showingTodaysAppointments,
        selectedDate: undoOptions.value.selectedDate
          ? new Date(undoOptions.value.selectedDate).toISOString()
          : '',
        matchedApplications: undoOptions.value.matchedApplications,
      },
      paramsSerializer: {
        indexes: null,
      },
    })

    const permitsData: Array<PermitsType> = res?.data?.items.map(data => {
      const permitsType: PermitsType = {
        id: data.id,
        orderId: data.orderId,
        status: ApplicationStatus[ApplicationStatus[data.status]],
        applicationType: ApplicationType[ApplicationType[data.applicationType]],
        appointmentStatus:
          AppointmentStatus[AppointmentStatus[data.appointmentStatus]],
        paid: data.paid,
        initials: formatInitials(data.firstName, data.lastName),
        name: formatName(data),
        assignedTo: data.assignedTo,
        appointmentDateTime: data.appointmentDateTime
          ? `${formatDate(data.appointmentDateTime)} at ${formatTime(
              data.appointmentDateTime
            )}`
          : 'None',
        isComplete: data.isComplete,
        appointmentId: data.appointmentId,
      }

      return permitsType
    })

    res.data.items = permitsData

    return res.data
  }

  async function getAllLegacyApplications(
    signal: AbortSignal | undefined
  ): Promise<{
    items: Array<LegacyPermitsType>
    total: number
  }> {
    const res = await axios.get(
      Endpoints.GET_ALL_LEGACY_APPLICATIONS_ENDPOINT,
      {
        signal,
        params: {
          page: legacyOptions.value.options.page,
          itemsPerPage: legacyOptions.value.options.itemsPerPage,
          sortBy: legacyOptions.value.options.sortBy,
          sortDesc: legacyOptions.value.options.sortDesc,
          groupBy: legacyOptions.value.options.groupBy,
          groupDesc: legacyOptions.value.options.groupDesc,
          statuses: legacyOptions.value.statuses,
          appointmentStatuses: legacyOptions.value.appointmentStatuses,
          applicationTypes: legacyOptions.value.applicationTypes,
          search: legacyOptions.value.search,
          applicationSearch: legacyOptions.value.applicationSearch,
          showingTodaysAppointments:
            legacyOptions.value.showingTodaysAppointments,
          selectedDate: legacyOptions.value.selectedDate
            ? new Date(legacyOptions.value.selectedDate).toISOString()
            : '',
        },
        paramsSerializer: {
          indexes: null,
        },
      }
    )

    const permitsData: Array<LegacyPermitsType> = res?.data?.items.map(data => {
      const permitsType: LegacyPermitsType = {
        orderId: data.orderId,
        status: ApplicationStatus[ApplicationStatus[data.status]],
        applicationType: ApplicationType[ApplicationType[data.applicationType]],
        name: formatName(data),
        appointmentDateTime: data.appointmentDateTime
          ? `${formatDate(data.appointmentDateTime)} at ${formatTime(
              data.appointmentDateTime
            )}`
          : 'None',
        idNumber: data.idNumber,
        birthDate: data.birthDate,
        permitNumber: data.permitNumber,
        email: data.email,
        id: data.id,
      }

      return permitsType
    })

    res.data.items = permitsData

    return res.data
  }

  async function getEmails(): Promise<string[]> {
    const res = await axios.get(Endpoints.GET_EMAILS_ENDPOINT, {
      params: {
        page: legacyOptions.value.options.page,
        itemsPerPage: legacyOptions.value.options.itemsPerPage,
        sortBy: legacyOptions.value.options.sortBy,
        sortDesc: legacyOptions.value.options.sortDesc,
        groupBy: legacyOptions.value.options.groupBy,
        groupDesc: legacyOptions.value.options.groupDesc,
        statuses: legacyOptions.value.statuses,
        appointmentStatuses: legacyOptions.value.appointmentStatuses,
        applicationTypes: legacyOptions.value.applicationTypes,
        search: legacyOptions.value.search,
        applicationSearch: legacyOptions.value.applicationSearch,
        showingTodaysAppointments:
          legacyOptions.value.showingTodaysAppointments,
        selectedDate: legacyOptions.value.selectedDate
          ? new Date(legacyOptions.value.selectedDate).toISOString()
          : '',
      },
      paramsSerializer: {
        indexes: null,
      },
    })

    if (res?.data) {
      return res.data
    }

    return []
  }

  async function getPermitDetailApi(orderId: string, isLegacy = 'false') {
    const isComplete = true

    const res = await axios.get(
      `${Endpoints.GET_AGENCY_PERMIT_ENDPOINT}?userEmailOrOrderId=${orderId}&isOrderId=true&isComplete=${isComplete}&isLegacy=${isLegacy}`
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

  async function matchApplication(userId: string, applicationId: string) {
    await axios.post(Endpoints.MATCH_APPLICATION_ENDPOINT, {
      userId,
      applicationId,
    })
  }

  async function undoMatchApplication(applicationId: string) {
    await axios.post(
      `${Endpoints.UNDO_MATCH_APPLICATION_ENDPOINT}?applicationId=${applicationId}`
    )
  }

  async function addHistoricalApplication(application: CompleteApplication) {
    await axios
      .put(Endpoints.PUT_ADD_HISTORICAL_APPLICATION_ENDPOINT, application)
      .then(res => {
        return res.data
      })
      .catch(err => {
        window.console.log(err)

        return Promise.reject()
      })
  }

  async function printRevocationLetterApi() {
    const applicationId = permitDetail.value.id
    const formattedDateTime = formatDateTimeNow()
    const fileName = `${applicationId}_${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_RevocationLetter_${formattedDateTime}`
    const user = authStore.auth.userName

    const res = await axios({
      url: `${Endpoints.GET_PRINT_REVOCATION_LETTER_ENDPOINT}?applicationId=${applicationId}&fileName=${fileName}&user=${user}`,
      method: 'PUT',
      responseType: 'blob',
    })

    const uploadAdminDoc: UploadedDocType = {
      documentType: 'RevocationLetter',
      name: `${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_RevocationLetter_${formattedDateTime}`,
      uploadedBy: authStore.auth.userEmail,
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    }

    permitDetail.value.application.adminUploadedDocuments.push(uploadAdminDoc)
    updatePermitDetailApi(`Uploaded new ${uploadAdminDoc.documentType}`)

    return res || {}
  }

  async function printApplicationApi() {
    const applicationId = permitDetail.value.id
    const formattedDateTime = formatDateTimeNow()
    const fileName = `${applicationId}_${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_Application_${formattedDateTime}`

    const res = await axios({
      url: `${Endpoints.GET_PRINT_APPLICATION_ENDPOINT}?applicationId=${applicationId}&fileName=${fileName}`,
      method: 'PUT',
      responseType: 'blob',
    })

    const uploadAdminDoc: UploadedDocType = {
      documentType: 'Application',
      name: `${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_Application_${formattedDateTime}`,
      uploadedBy: authStore.auth.userEmail,
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    }

    permitDetail.value.application.adminUploadedDocuments.push(uploadAdminDoc)
    updatePermitDetailApi(`Uploaded new ${uploadAdminDoc.documentType}`)

    return res || {}
  }

  async function printModificationApi() {
    const applicationId = permitDetail.value.id
    const formattedDateTime = formatDateTimeNow()
    const fileName = `${applicationId}_${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_Modification_${formattedDateTime}`

    const res = await axios({
      url: `${Endpoints.GET_PRINT_MODIFICATION_ENDPOINT}?applicationId=${applicationId}&fileName=${fileName}`,
      method: 'PUT',
      responseType: 'blob',
    })

    const uploadAdminDoc: UploadedDocType = {
      documentType: 'Modification',
      name: `${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_Modification_${formattedDateTime}`,
      uploadedBy: authStore.auth.userEmail,
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    }

    permitDetail.value.application.adminUploadedDocuments.push(uploadAdminDoc)
    updatePermitDetailApi(`Uploaded new ${uploadAdminDoc.documentType}`)

    return res || {}
  }

  function formatDateTimeNow() {
    const date = new Date(Date.now())
    const month =
      date.getMonth() + 1 < 10 ? `0${date.getMonth() + 1}` : date.getMonth() + 1
    const day = date.getDate() < 10 ? `0${date.getDate()}` : date.getDate()
    const year = date.getFullYear()
    const hours = date.getHours() < 10 ? `0${date.getHours()}` : date.getHours()
    const minutes =
      date.getMinutes() < 10 ? `0${date.getMinutes()}` : date.getMinutes()
    const formattedDate = `${month}-${day}-${year}_${hours}-${minutes}`

    return formattedDate
  }

  async function printOfficialLicenseApi() {
    const applicationId = permitDetail.value.id
    const formattedDateTime = formatDateTimeNow()
    const fileName = `${applicationId}_${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_Official_License_${formattedDateTime}`

    let issueDate: Date
    let expDate: Date

    const license = permitDetail.value.application.license
    const applicationType = permitDetail.value.application.applicationType

    if (license && license.issueDate && license.expirationDate) {
      issueDate = new Date(license.issueDate)
      expDate = new Date(license.expirationDate)
    } else {
      issueDate = new Date()
      expDate = new Date()

      switch (applicationType) {
        case ApplicationType.Reserve:
        case ApplicationType['Renew Reserve']:
          expDate.setUTCFullYear(expDate.getUTCFullYear() + 4)
          break
        case ApplicationType.Judicial:
        case ApplicationType['Renew Judicial']:
          expDate.setUTCFullYear(expDate.getUTCFullYear() + 3)
          break
        case ApplicationType.Employment:
        case ApplicationType['Renew Employment']:
          expDate.setUTCDate(expDate.getUTCDate() + 90)
          break
        default:
          expDate.setUTCFullYear(expDate.getUTCFullYear() + 2)
          break
      }
    }

    const issueDateISO = issueDate.toISOString()
    const expDateISO = expDate.toISOString()

    permitDetail.value.application.license.issueDate = issueDateISO
    permitDetail.value.application.license.expirationDate = expDateISO

    try {
      await axios.put(
        Endpoints.PUT_UPDATE_AGENCY_PERMIT_ENDPOINT,
        permitDetail.value
      )

      const res = await axios({
        url: `${Endpoints.GET_PRINT_OFFICIAL_LICENSE_ENDPOINT}?applicationId=${applicationId}&fileName=${fileName}`,
        method: 'PUT',
        responseType: 'blob',
      })

      const uploadAdminDoc: UploadedDocType = {
        documentType: 'Official_License',
        name: `${permitDetail.value.application.personalInfo.lastName}_${
          permitDetail.value.application.personalInfo.firstName
        }_${'Official_License'}_${formattedDateTime}`,
        uploadedBy: authStore.auth.userEmail,
        uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
      }

      permitDetail.value.application.adminUploadedDocuments.push(uploadAdminDoc)

      const historyMessage = `Uploaded new ${uploadAdminDoc.documentType}`

      await updatePermitDetailApi(historyMessage)

      return res || {}
    } catch (error) {
      console.error('Error occurred:', error)

      return {}
    }
  }

  async function printUnofficialLicenseApi() {
    const applicationId = permitDetail.value.id
    const formattedDateTime = formatDateTimeNow()
    const fileName = `${applicationId}_${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_Unofficial_License_${formattedDateTime}`

    const res = await axios({
      url: `${Endpoints.GET_PRINT_UNOFFICIAL_LICENSE_ENDPOINT}?applicationId=${applicationId}&fileName=${fileName}`,
      method: 'PUT',
      responseType: 'blob',
    })
    const uploadAdminDoc: UploadedDocType = {
      documentType: 'Unofficial_License',
      name: `${permitDetail.value.application.personalInfo.lastName}_${
        permitDetail.value.application.personalInfo.firstName
      }_${'Unofficial_License'}_${formattedDateTime}`,
      uploadedBy: authStore.auth.userEmail,
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    }

    permitDetail.value.application.adminUploadedDocuments.push(uploadAdminDoc)
    updatePermitDetailApi(`Uploaded new ${uploadAdminDoc.documentType}`)

    return res || {}
  }

  async function printLiveScanApi() {
    const applicationId = permitDetail.value.id
    const formattedDateTime = formatDateTimeNow()
    const fileName = `${applicationId}_${permitDetail.value.application.personalInfo.lastName}_${permitDetail.value.application.personalInfo.firstName}_Live_Scan_${formattedDateTime}`

    const res = await axios({
      url: `${Endpoints.GET_PRINT_LIVE_SCAN_ENDPOINT}?applicationId=${applicationId}&fileName=${fileName}`,
      method: 'PUT',
      responseType: 'blob',
    })
    const uploadAdminDoc: UploadedDocType = {
      documentType: 'Live_Scan',
      name: `${permitDetail.value.application.personalInfo.lastName}_${
        permitDetail.value.application.personalInfo.firstName
      }_${'Live_Scan'}_${formattedDateTime}`,
      uploadedBy: authStore.auth.userEmail,
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    }

    permitDetail.value.application.adminUploadedDocuments.push(uploadAdminDoc)
    updatePermitDetailApi(`Uploaded new ${uploadAdminDoc.documentType}`)

    return res || {}
  }

  async function updateMultiplePermitDetailsApi(ids, assignedAdminUser) {
    await axios.put(
      `${Endpoints.PUT_UPDATE_MULTIPLE_PERMITS_ENDPOINT}?assignedAdminUser=${assignedAdminUser}`,
      ids
    )
  }

  async function updatePermitDetailApi(item: string) {
    if (permitDetail.value.application.cost === null) {
      permitDetail.value.application.cost = brandStore.brand.cost
    }

    if (permitDetail.value.application.cost?.standardLivescanFee === null) {
      permitDetail.value.application.cost.standardLivescanFee =
        brandStore.brand.cost.standardLivescanFee
    }

    if (permitDetail.value.application.cost?.judicialLivescanFee === null) {
      permitDetail.value.application.cost.judicialLivescanFee =
        brandStore.brand.cost.judicialLivescanFee
    }

    if (permitDetail.value.application.cost?.reserveLivescanFee === null) {
      permitDetail.value.application.cost.reserveLivescanFee =
        brandStore.brand.cost.reserveLivescanFee
    }

    if (permitDetail.value.application.cost?.employmentLivescanFee === null) {
      permitDetail.value.application.cost.employmentLivescanFee =
        brandStore.brand.cost.employmentLivescanFee
    }

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
    options,
    legacyOptions,
    undoOptions,
    permits,
    searchResults,
    openPermits,
    permitDetail,
    getPermits,
    getOpenPermits,
    getPermitDetail,
    getSearchResults,
    getHistory,
    summaryCount,
    assignedApplicationsSummary,
    viewingPermitDetail,
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
    printModificationApi,
    printOfficialLicenseApi,
    printUnofficialLicenseApi,
    printLiveScanApi,
    printRevocationLetterApi,
    updatePermitDetailApi,
    updateMultiplePermitDetailsApi,
    getAllPermitsSummary,
    getApplicationSummaryCount,
    getAssignedApplicationsSummary,
    addHistoricalApplication,
    getAllLegacyApplications,
    getUndoPermitsSummary,
    matchApplication,
    undoMatchApplication,
    getEmails,
  }
})
