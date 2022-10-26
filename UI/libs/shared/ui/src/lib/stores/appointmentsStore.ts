import { AppointmentType } from '@shared-utils/types/defaultTypes';
import axios from 'axios';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
// import { useAppConfigStore } from '@shared-ui/stores/appConfig';

export const useAppointmentsStore = defineStore('AppointmentsStore', () => {
  // const appConfigStore = useAppConfigStore();

  const appointments = ref<Array<AppointmentType>>([]);

  const getAppointments = computed(() => appointments.value);

  function setAppointments(payload: Array<AppointmentType>) {
    appointments.value = payload;
  }

  async function getAppointmentsApi() {
    const res = await axios
      .get(`https://apimocha.com/appointments/get`)
      .catch(err => window.window.console.log(err));

    const appointmentsData = res?.data?.appointments.map(data => ({
      ...data,
      rowClass: 'appointment-table__row',
    }));

    setAppointments(appointmentsData);

    return appointments;
  }

  return {
    appointments,
    getAppointments,
    setAppointments,
    getAppointmentsApi,
  };
});
