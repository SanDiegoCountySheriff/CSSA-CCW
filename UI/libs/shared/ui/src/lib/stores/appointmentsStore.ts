import { AppointmentType } from '@shared-utils/types/defaultTypes';
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters';

export const useAppointmentsStore = defineStore('AppointmentsStore', () => {
  // TODO: Create available appointments state;
  const appointments = ref<Array<AppointmentType>>([]);
  const newAptCount = ref<number>(0);

  const getAppointments = computed(() => appointments.value);
  const getNewAptCount = computed(() => newAptCount.value);

  function setAppointments(payload: Array<AppointmentType>) {
    appointments.value = payload;
  }

  function setNewAptCount(payload: number) {
    newAptCount.value = payload;
  }

  async function getAppointmentsApi() {
    const res = await axios
      .get(Endpoints.GET_APPOINTMENTS_ENDPOINT)
      .catch(err => window.window.console.log(err));

    const appointmentsData: Array<AppointmentType> = res?.data?.map(data => ({
      ...data,
      rowClass: 'appointment-table__row',
      date: formatDate(data.start),
      time: formatTime(data.start),
    }));

    setNewAptCount(
      appointmentsData?.filter(apt => apt.status === 'New').length
    );
    setAppointments(appointmentsData);

    return appointments;
  }

  // TODO: create a get availableAppointmentsApi

  return {
    appointments,
    newAptCount,
    getAppointments,
    getNewAptCount,
    setAppointments,
    setNewAptCount,
    getAppointmentsApi,
  };
});
