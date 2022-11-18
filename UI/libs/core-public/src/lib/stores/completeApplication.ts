import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defaultPermitState } from '@shared-utils/lists/defaultConstants';
import { defineStore } from 'pinia';
import { useAuthStore } from '@shared-ui/stores/auth';
import { computed, reactive } from 'vue';

export const useCompleteApplicationStore = defineStore(
  'completeApplicationStore',
  () => {
    const authStore = useAuthStore();
    const completeApplication =
      reactive<CompleteApplication>(defaultPermitState);
    /**
     * Get the complete application from the stored value
     * @type {ComputedRef<UnwrapRef<CompleteApplication>>}
     */
    const getCompleteApplication = computed(() => completeApplication);

    /**
     * Used to set the stored value from either the api call or the form
     * @param {CompleteApplication} payload
     */
    function setCompleteApplication(payload: CompleteApplication) {
      completeApplication.application = payload.application;
      completeApplication.id = payload.id;
    }

    //
    /**
     * Get the complete application from the backend
     */
    async function getCompleteApplicationFromApi(
      userEmail: string,
      isComplete: boolean
    ) {
      const res = await axios
        .get(Endpoints.GET_PERMIT_ENDPOINT, {
          params: {
            userEmailOrOrderId: userEmail,
            isOrderId: false,
            isComplete,
          },
        })

        .catch(err => {
          console.warn(err);
          Promise.reject();
        });

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
      completeApplication,
      createApplication,
      getCompleteApplication,
      setCompleteApplication,
      getCompleteApplicationFromApi,
      updateApplication,
    };
  }
);
