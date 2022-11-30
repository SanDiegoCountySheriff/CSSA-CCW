import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defaultPermitState } from '@shared-utils/lists/defaultConstants';
import { defineStore } from 'pinia';
import { useAuthStore } from '@shared-ui/stores/auth';
import { computed, reactive, ref } from 'vue';

export const useCompleteApplicationStore = defineStore(
  'permitStore',
  () => {
    const authStore = useAuthStore();
    const completeApplication =
      reactive<CompleteApplication>(defaultPermitState);
    const allUserApplications =
      ref<Array<CompleteApplication>>(defaultPermitState);

    const getCompleteApplication = computed(() => completeApplication);
    const getAllUserApplications = computed(() => allUserApplications);

    /**
     * Used to set the stored value from either the api call or the form
     * @param {CompleteApplication} payload
     */
    function setCompleteApplication(payload: CompleteApplication) {
      completeApplication.application = payload.application;
      completeApplication.history = payload.history;
      completeApplication.id = payload.id;
    }

    function setAllUserApplications(payload: Array<CompleteApplication>) {
      allUserApplications.value = payload;
    }

    /**
     * Get the complete application from the backend
     */
    async function getCompleteApplicationFromApi(
      orderId: string,
      isComplete: boolean
    ) {
      const res = await axios
        .get(Endpoints.GET_PERMIT_ENDPOINT, {
          params: {
            userEmailOrOrderId: orderId,
            isOrderId: true,
            isComplete,
          },
        })

        .catch(err => {
          console.warn(err);
          Promise.reject();
        });

      return res?.data || {};
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
      });

      if (res?.data) setAllUserApplications(res.data);

      return res?.data;
    }

    async function createApplication() {
      const date = new Date(Date.now()).toISOString();

      completeApplication.history = [
        {
          change: 'Created application',
          changeDateTimeUtc: date,
          changeMadeBy: authStore.auth.userEmail,
        },
      ];

      await axios
        .put(Endpoints.PUT_CREATE_PERMIT_ENDPOINT, completeApplication)
        .then(res => {
          setCompleteApplication(res.data);
        })
        .catch(err => {
          window.console.log(err);

          return Promise.reject();
        });
    }

    async function updateApplication(changeMessage: string) {
      const date = new Date(Date.now()).toISOString();

      completeApplication.history = [
        {
          change: changeMessage,
          changeDateTimeUtc: date,
          changeMadeBy: authStore.auth.userEmail,
        },
      ];

      const res = await axios
        .put(Endpoints.PUT_UPDATE_PERMIT_ENDPOINT, completeApplication)
        .catch(err => {
          console.warn(err);

          return Promise.reject();
        });

      return res?.data;
    }

    return {
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
    };
  },
  {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    //@ts-ignore
    persist: true,
  }
);
