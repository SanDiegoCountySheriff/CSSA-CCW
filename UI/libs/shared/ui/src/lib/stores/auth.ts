import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import { AdminUserType, AuthType } from '@shared-utils/types/defaultTypes';
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
      sessionStarted: '',
      tokenExpired: false,
      adminUser: {} as AdminUserType,
      adminUserSignature: '',
      validAdminUser: true,
    });

    const getAuthState = computed(() => auth.value);

    const setUser = name => {
      auth.value.userName = name;
    };

    const setAdminUser = (user: AdminUserType) => {
      auth.value.adminUser = user;
    };

    const setAdminUserSignature = file => {
      auth.value.adminUserSignature = file;
    };

    const setUserEmail = email => {
      auth.value.userEmail = email;
    };

    const setVerifiedUser = value => {
      auth.value.verifiedUser = value;
    };

    const setValidAdminUser = value => {
      auth.value.validAdminUser = value;
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

    const setSessionStarted = value => {
      auth.value.sessionStarted = value;
    };

    const setTokenExpired = value => {
      auth.value.tokenExpired = value;
    };

    const resetStore = () => {
      auth.value.userName = '';
      auth.value.userEmail = '';
      auth.value.jwtToken = '';
      auth.value.isAuthenticated = false;
      auth.value.isAdmin = false;
      auth.value.verifiedUser = false;
      auth.value.roles = [];
      auth.value.sessionStarted = '';
      auth.value.tokenExpired = false;
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

    async function getAdminUserApi() {
      const res = await axios.get(Endpoints.GET_ADMIN_USER_ENDPOINT);
      const imageResponse = await axios.get(
        `${Endpoints.GET_ADMIN_USER_FILE_ENDPOINT}?adminUserFileName=signature.png`
      );

      if (imageResponse?.data) setAdminUserSignature(imageResponse.data);

      if (res?.data) setAdminUser(res.data);

      if (!res?.data.badgeNumber && res?.data.uploadedDocuments.length === 0) {
        setValidAdminUser(false);
      }

      return res?.data || {};
    }

    async function putCreateAdminUserApi(adminUser: AdminUserType) {
      const res = await axios.put(
        Endpoints.PUT_CREATE_ADMIN_USER_ENDPOINT,
        adminUser
      );

      if (res?.data) setAdminUser(res.data);

      return res?.data || {};
    }

    return {
      auth,
      getAuthState,
      setUser,
      setUserEmail,
      setToken,
      setIsAuthenticated,
      setRoles,
      setSessionStarted,
      setTokenExpired,
      resetStore,
      postVerifyUserApi,
      putCreateUserApi,
      getAdminUserApi,
      putCreateAdminUserApi,
      setAdminUserSignature,
      setValidAdminUser,
    };
  },
  {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    //@ts-ignore
    persist: true,
  }
);
