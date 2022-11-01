import { AuthType } from '@shared-utils/types/defaultTypes';
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useAuthStore = defineStore(
  'AuthStore',
  () => {
    const auth = ref<AuthType>({
      userName: '',
      userEmail: '',
      jwtToken: '',
      isAuthenticated: false,
      isAdmin: false,
      verifiedUser: false,
      roles: [],
    });

    const getAuthState = computed(() => auth.value);

    const setUser = name => {
      auth.value.userName = name;
    };

    const setUserEmail = email => {
      auth.value.userEmail = email;
    };

    const setVerifiedUser = value => {
      auth.value.verifiedUser = value;
    };

    const setToken = token => {
      auth.value.jwtToken = token;
    };

    const setIsAuthenticated = value => {
      auth.value.isAuthenticated = value;
    };

    const setRoles = value => {
      auth.value.roles = value;
    };

    const resetStore = () => {
      auth.value.userName = '';
      auth.value.userEmail = '';
      auth.value.jwtToken = '';
      auth.value.isAuthenticated = false;
      auth.value.isAdmin = false;
      auth.value.verifiedUser = false;
      auth.value.roles = [];
    };

    async function postVerifyUserApi() {
      const res = await axios
        .post(
          `${Endpoints.POST_VERIFY_USER_ENDPOINT}?userEmail=${getAuthState.value.userEmail}`
        )
        .catch(err => console.warn(err));

      if (res?.data.statusCode === 404) {
        await putCreateUserApi();
      }

      setVerifiedUser(res?.data.isSuccessStatusCode);

      return res?.data;
    }

    async function putCreateUserApi() {
      const payload = { emailAddress: getAuthState.value.userEmail };
      const res = await axios
        .put(Endpoints.PUT_CREATE_USER_ENDPOINT, payload)
        .catch(err => console.warn(err));

      setVerifiedUser(Boolean(res?.data.id));

      return res?.data;
    }

    return {
      auth,
      getAuthState,
      setUser,
      setUserEmail,
      setToken,
      setIsAuthenticated,
      setRoles,
      resetStore,
      postVerifyUserApi,
      putCreateUserApi,
    };
  },
  {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    //@ts-ignore
    persist: true,
  }
);
