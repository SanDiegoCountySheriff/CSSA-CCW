import { defineStore } from 'pinia';
import { computed, reactive } from 'vue';
import { useAppConfigStore } from '@shared-ui/stores/appConfig';
import axios from 'axios';
import { CompleteApplication } from '@shared-utils/types/defaultTypes';

export const useCompleteApplicationStore = defineStore(
  'completeApplicationStore',
  () => {
    const appConfigStore = useAppConfigStore();

    let completeApplication = reactive<CompleteApplication>({
      personalInfo: {
        lastName: '',
        firstName: '',
        middleName: '',
        maidenName: '',
        noMiddleName: false,
        maritalStatus: '',
        ssn: '',
        suffix: '',
      },
      DOB: {
        birthDate: '',
        birthCity: '',
        birthCountry: '',
        birthState: '',
        currentAge: null,
      },
      aliases: [],
      applicationType: '',
      citizenship: { citizen: false, militaryStatus: '' },
      contact: {
        cellPhoneNumber: '',
        faxPhoneNumber: '',
        primaryPhoneNumber: '',
        textMessageUpdates: false,
        workPhoneNumber: '',
      },
      currentAddress: {
        addressLine1: '',
        addressLine2: '',
        city: '',
        country: '',
        county: '',
        state: '',
        zip: '',
      },
      differentMailing: false,
      employment: '',
      id: {
        idNumber: '',
        issuingState: '',
      },
      mailingAddress: {
        addressLine1: '',
        addressLine2: '',
        city: '',
        country: '',
        county: '',
        state: '',
        zip: '',
      },
      physicalAppearance: {
        eyeColor: '',
        gender: '',
        hairColor: '',
        heightFeet: null,
        heightInch: null,
        physicalDesc: '',
        weight: null,
      },
      previousAddress: [],
      weapons: [],
    } as CompleteApplication);

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
      completeApplication = payload;
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
      completeApplication,
      getCompleteApplication,
      setCompleteApplication,
      getCompleteApplicationFromApi,
      postCompleteApplicationFromApi,
    };
  }
);
