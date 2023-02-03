import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defaultPermitState } from '@shared-utils/lists/defaultConstants';
import { defineStore } from 'pinia';
import { useAuthStore } from '@shared-ui/stores/auth';
import { computed, reactive, ref } from 'vue';

export const useCompleteApplicationStore = defineStore('permitStore', () => {
  const blankApplication = JSON.parse(JSON.stringify(defaultPermitState));
  const authStore = useAuthStore();
  const completeApplication = reactive<CompleteApplication>(blankApplication);
  const allUserApplications = ref<Array<CompleteApplication>>();

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
    applicationId: string,
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

  async function updateApplication() {
    const res = await axios
      .put(Endpoints.PUT_UPDATE_PERMIT_ENDPOINT, completeApplication)
      .catch(err => {
        console.warn(err);

        return Promise.reject();
      });

    return res?.data;
  }

  async function deleteApplication(applicationId: string) {
    const res = await axios
      .put(
        Endpoints.DELETE_PERMIT_ENDPOINT,
        {},
        {
          params: {
            applicationId,
          },
        }
      )
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
    deleteApplication,
  };
});
