import {
  CompleteApplication,
  HistoryType,
} from '@shared-utils/types/defaultTypes';
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import { computed, reactive } from 'vue';
import { useAuthStore } from '@shared-ui/stores/auth';

export const useCompleteApplicationStore = defineStore(
  'completeApplicationStore',
  () => {
    const authStore = useAuthStore();
    let completeApplication = reactive<CompleteApplication>({
      id: '',
      DOB: {
        birthDate: '',
        birthCity: '',
        birthCountry: '',
        birthState: '',
      },
      aliases: [
        {
          prevLastName: '',
          prevFirstName: '',
          prevMiddleName: '',
          cityWhereChanged: '',
          stateWhereChanged: '',
          courtFileNumber: '',
        },
      ],
      applicationType: '',
      citizenship: { citizen: true, militaryStatus: '' },
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
      history: [
        {
          change: '',
          dateTime: '',
          changeMadeBy: '',
        },
      ],
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
      personalInfo: {
        lastName: '',
        firstName: '',
        middleName: '',
        noMiddleName: false,
        maidenName: '',
        suffix: '',
        ssn: '',
        maritalStatus: '',
      },
      physicalAppearance: {
        eyeColor: '',
        gender: '',
        hairColor: '',
        heightFeet: '',
        heightInch: '',
        physicalDesc: '',
        weight: '',
      },
      previousAddresses: [
        {
          addressLine1: '',
          addressLine2: '',
          city: '',
          country: '',
          county: '',
          state: '',
          zip: '',
        },
      ],
      qualifyingQuestions: {
        questionOne: false,
        questionOneExp: '',
        questionTwo: false,
        questionTwoExp: '',
        questionThree: false,
        questionThreeExp: '',
        questionFour: false,
        questionFourExp: '',
        questionFive: false,
        questionFiveExp: '',
        questionSix: false,
        questionSixExp: '',
        questionSeven: false,
        questionSevenExp: '',
        questionEight: false,
        questionEightExp: '',
        questionNine: false,
        questionNineExp: '',
        questionTen: false,
        questionTenExp: '',
        questionEleven: false,
        questionElevenExp: '',
        questionTwelve: false,
        questionTwelveExp: '',
        questionThirteen: false,
        questionThirteenExp: '',
        questionFourteen: false,
        questionFourteenExp: '',
        questionFifteen: false,
        questionFifteenExp: '',
        questionSixteen: false,
        questionSixteenExp: '',
        questionSeventeen: false,
        questionSeventeenExp: '',
      },
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
    async function getCompleteApplicationFromApi(
      userEmail: string,
      isComplete: boolean
    ) {
      const res = await axios
        .get(Endpoints.GET_PERMIT_ENDPOINT, {
          params: { userEmail, isComplete },
        })

        .catch(err => console.warn(err));

      //TODO: add back in once the api is corrected.
      //setCompleteApplication(res?.data);
      return res?.data;
    }

    async function createApplication() {
      const applicationId = completeApplication.id;
      const date = new Date(Date.now()).toUTCString();
      const historyLog: HistoryType = {
        change: 'Created application',
        dateTime: date,
        changeMadeBy: authStore.auth.userEmail,
      };

      completeApplication.history.push(historyLog);
      const body = {
        application: completeApplication,
        id: applicationId,
      };

      await axios.put(Endpoints.PUT_CREATE_PERMIT_ENDPOINT, body).catch(err => {
        window.console.log(err);

        return Promise.reject();
      });
    }

    async function updateApplication(changeMessage: string) {
      const date = new Date(Date.now()).toUTCString();
      const historyLog: HistoryType = {
        change: changeMessage,
        dateTime: date,
        changeMadeBy: authStore.auth.userEmail,
      };

      completeApplication.history.push(historyLog);
      const application = {
        application: completeApplication,
        id: completeApplication.id,
      };

      const res = await axios
        .put(Endpoints.PUT_UPDATE_PERMIT_ENDPOINT, application)
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
