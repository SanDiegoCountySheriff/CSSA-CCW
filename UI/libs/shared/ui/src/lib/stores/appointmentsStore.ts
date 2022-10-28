import { AppointmentType } from '@shared-utils/types/defaultTypes';
import axios from 'axios';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
// import { useAppConfigStore } from '@shared-ui/stores/appConfig';

export const useAppointmentsStore = defineStore('AppointmentsStore', () => {
  // const appConfigStore = useAppConfigStore();

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
      .get(`https://apimocha.com/appointments/get`)
      .catch(err => window.window.console.log(err));

    const appointmentsData: Array<AppointmentType> =
      res?.data?.appointments.map(data => ({
        ...data,
        rowClass: 'appointment-table__row',
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
