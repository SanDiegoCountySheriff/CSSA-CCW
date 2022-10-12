import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
import { useAppConfigStore } from '@shared-ui/stores/appConfig';
import axios from 'axios';
import { CompleteApplication } from '@shared-utils/types/defaultTypes';

export const useCompleteApplicationStore = defineStore(
  'completeApplicationStore',
  () => {
    const appConfigStore = useAppConfigStore();
    // TODO: Create the complete Application type
    const completeApplication = ref<CompleteApplication>(
      {} as CompleteApplication
    );

    /**
     * Get the complete application from the stored value
     * @type {ComputedRef<UnwrapRef<CompleteApplication>>}
     */
    const getCompleteApplication = computed(() => completeApplication.value);

    /**
     * Used to set the stored value from either the api call or the form
     * @param {CompleteApplication} payload
     */
    function setCompleteApplication(payload: CompleteApplication) {
      completeApplication.value = payload;
    }

    //
    /**
     * Get the complete application from the backend
     */
    async function getCompleteApplicationFromApi() {
      const res = await axios
        .get(`${appConfigStore.getAppConfig.apiBaseUrl}`)
        .catch(err => console.warn(err));
      setCompleteApplication(res?.data);
      return res?.data;
    }

    async function postCompleteApplicationFromApi(
      payload: CompleteApplication
    ) {
      const res = await axios
        .put(`${appConfigStore.getAppConfig.apiBaseUrl}`, payload)
        .catch(err => console.warn(err));
      return res?.data;
    }
    return {
      getCompleteApplication,
      setCompleteApplication,
      getCompleteApplicationFromApi,
      postCompleteApplicationFromApi,
    };
  }
);
