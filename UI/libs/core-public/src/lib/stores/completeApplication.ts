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
      id: '',
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
      differentSpouseAddress: false,
      employment: '',
      idInfo: {
        idNumber: '',
        issuingState: '',
      },
      immigrantInformation: {
        countryOfCitizenship: '',
        countryOfBirth: '',
        immigrantAlien: false,
        nonImmigrantAlien: false,
      },
      isComplete: false,
      license: {
        permitNumber: '',
        issuingCounty: '',
        expirationDate: '',
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
      spouseInformation: {
        lastName: '',
        firstName: '',
        middleName: '',
        maidenName: '',
      },
      spouseAddressInformation: {
        addressLine1: '',
        addressLine2: '',
        city: '',
        country: '',
        county: '',
        state: '',
        zip: '',
      },
      userEmail: '',
      weapons: [],
      workInformation: {
        employerName: '',
        employerAddressLine1: '',
        employerAddressLine2: '',
        employerCity: '',
        employerCountry: 'United States',
        employerPhone: '',
        employerState: '',
        employerZip: '',
      },
    } as CompleteApplication);

    const url = `${appConfigStore.getAppConfig.apiBaseUrl}/v1/PermitApplication`;
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
      const res = await axios.get(`${url}`).catch(err => console.warn(err));
      //TODO: add back in once the api is corrected.
      //setCompleteApplication(res?.data);
      console.log(res?.data);
    }

    async function postCompleteApplicationFromApi(
      payload: CompleteApplication
    ) {
      if (payload.id) {
        const res = await axios
          .put(`${url}/update`, payload)
          .catch(err => console.warn(err));
        return res?.data;
      } else {
        const res = await axios
          .put(`${url}/create`, payload)
          .catch(err => console.warn(err));
        return res?.data;
      }
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
