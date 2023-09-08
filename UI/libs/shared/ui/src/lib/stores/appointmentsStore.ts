import { AppointmentWindowCreateRequestModel } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { defineStore } from 'pinia'
import {
  AppointmentManagement,
  AppointmentType,
  OrganizationalHolidaysRequestModel,
} from '@shared-utils/types/defaultTypes'
import { computed, ref } from 'vue'
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

export const useAppointmentsStore = defineStore('AppointmentsStore', () => {
  const appointments = ref<Array<AppointmentType>>([])
  const newAptCount = ref<number>(0)
  const newAppointmentsFile = ref<string | Blob>('')

  const getAppointments = computed(() => appointments.value)
  const getNewAptCount = computed(() => newAptCount.value)
  const getNewAppointmentsFile = computed(() => newAppointmentsFile.value)

  const currentAppointment = ref<AppointmentType>({} as AppointmentType)

  function setAppointments(payload: Array<AppointmentType>) {
    appointments.value = payload
  }

  function setCurrentAppointment(payload: AppointmentType) {
    currentAppointment.value = payload
  }

  function setNewAptCount(payload: number) {
    newAptCount.value = payload
  }

  function setNewAppointmentsFile(payload) {
    newAppointmentsFile.value = payload
  }

  async function getAppointmentsApi(): Promise<Array<AppointmentType>> {
    const res = await axios
      .get(Endpoints.GET_APPOINTMENTS_ENDPOINT)
      .catch(err => window.console.log(err))

    const appointmentsData: Array<AppointmentType> = res?.data?.map(data => ({
      ...data,
      date: formatDate(data.start),
      time: formatTime(data.start),
    }))

    setNewAptCount(appointmentsData?.length)
    setAppointments(appointmentsData)

    return appointmentsData
  }

  async function uploadAppointmentsApi() {
    const formData = new FormData()

    formData.append('fileToPersist', getNewAppointmentsFile.value)
    const res = await axios.post(
      `${Endpoints.POST_UPLOAD_APPOINTMENTS_ENDPOINT}`,
      formData
    )

    return res?.data
  }

  async function createNewAppointments(body: AppointmentManagement) {
    const res = await axios
      .post(Endpoints.CREATE_APPOINTMENTS_ENDPOINT, body)
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  async function getAvailableAppointments() {
    const res = await axios
      .get(Endpoints.GET_AVAILABLE_APPOINTMENTS_ENDPOINT)
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  async function setAppointmentPublic(body: AppointmentType) {
    const res = await axios
      .put(Endpoints.PUT_PUBLIC_APPOINTMENT_ENDPOINT, body)
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  async function sendAppointmentCheck(body: AppointmentType) {
    const res = await axios
      .put(Endpoints.PUT_APPOINTMENTS_ENDPOINT, body)
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  async function rescheduleAppointment(body: AppointmentType) {
    const res = await axios
      .put(Endpoints.PUT_RESCHEDULE_APPOINTMENT_ENDPOINT, body)
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  async function getSingleAppointment(id: string) {
    const res = await axios
      .get(Endpoints.GET_SINGLE_APPOINTMENT, {
        params: {
          applicationId: id,
        },
      })
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function deleteUserFromAppointment(id: string) {
    const res = await axios
      .delete(Endpoints.DELETE_USER_FROM_APPOINTMENT, {
        params: {
          appointmentId: id,
        },
      })
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function deleteAppointmentsByDate(date: Date | undefined) {
    const res = await axios
      .delete(Endpoints.DELETE_APPOINTMENTS_BY_DATE, {
        params: {
          date,
        },
      })
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function deleteAppointmentsByTimeSlot(date: Date | undefined) {
    const res = await axios
      .delete(Endpoints.DELETE_APPOINTMENTS_BY_TIME_SLOT, {
        params: {
          date,
        },
      })
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function deleteSlotByApplicationId(applicationId: string) {
    const res = await axios
      .delete(
        `${Endpoints.DELETE_APPOINTMENT_BY_APPLICATION_ID}?applicationId=${applicationId}`
      )
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function putReopenSlotByApplicationId(applicationId: string) {
    const res = await axios
      .put(
        `${Endpoints.REOPEN_APPOINTMENT_BY_APPLICATION_ID}?applicationId=${applicationId}`
      )
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function putRemoveApplicationFromAppointment(
    appointmentId: string | null
  ) {
    const res = await axios
      .put(
        `${Endpoints.REMOVE_APPLICATION_FROM_APPOINTMENT}?appointmentId=${appointmentId}`
      )
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function putCreateManualAppointment(
    appointment: AppointmentWindowCreateRequestModel
  ) {
    const res = await axios
      .put(Endpoints.PUT_CREATE_MANUAL_APPOINTMENT_ENDPOINT, appointment)
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function putCheckInAppointment(appointmentId: string) {
    const res = await axios
      .put(
        `${Endpoints.PUT_CHECK_IN_APPOINTMENT_BY_APPOINTMENT_ID}?appointmentId=${appointmentId}`
      )
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function putNoShowAppointment(appointmentId: string) {
    const res = await axios
      .put(
        `${Endpoints.PUT_NO_SHOW_APPOINTMENT_BY_APPOINTMENT_ID}?appointmentId=${appointmentId}`
      )
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function putSetAppointmentScheduled(appointmentId: string) {
    const res = await axios
      .put(
        `${Endpoints.PUT_SET_APPOINTMENT_SCHEDULED_BY_APPOINTMENT_ID}?appointmentId=${appointmentId}`
      )
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function getNumberOfNewAppointmentsByNumberOfDays(
    numberOfDays: number
  ) {
    const res = await axios
      .get(
        `${Endpoints.GET_NUMBER_OF_NEW_APPOINTMENTS_ENDPOINT}?numberOfDays=${numberOfDays}`
      )
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function getNextAvailableAppointment() {
    const res = await axios
      .get(`${Endpoints.GET_NEXT_AVAILABLE_APPOINTMENT_ENDPOINT}`)
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function getHolidays() {
    const res = await axios
      .get(`${Endpoints.GET_HOLIDAYS_ENDPOINT}`)
      .catch(err => {
        window.console.warn(err)
        Promise.reject()
      })

    return res?.data
  }

  async function saveHolidays(body: OrganizationalHolidaysRequestModel) {
    const res = await axios
      .post(Endpoints.SAVE_HOLIDAYS_ENDPOINT, body)
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  return {
    appointments,
    currentAppointment,
    newAptCount,
    newAppointmentsFile,
    getAppointments,
    getNewAptCount,
    getNewAppointmentsFile,
    setAppointments,
    setAppointmentPublic,
    setCurrentAppointment,
    setNewAptCount,
    setNewAppointmentsFile,
    getAppointmentsApi,
    getAvailableAppointments,
    getSingleAppointment,
    sendAppointmentCheck,
    rescheduleAppointment,
    uploadAppointmentsApi,
    deleteUserFromAppointment,
    deleteAppointmentsByDate,
    deleteAppointmentsByTimeSlot,
    createNewAppointments,
    deleteSlotByApplicationId,
    putReopenSlotByApplicationId,
    putRemoveApplicationFromAppointment,
    putCreateManualAppointment,
    putCheckInAppointment,
    putNoShowAppointment,
    putSetAppointmentScheduled,
    getNumberOfNewAppointmentsByNumberOfDays,
    getNextAvailableAppointment,
    getHolidays,
    saveHolidays,
  }
})
