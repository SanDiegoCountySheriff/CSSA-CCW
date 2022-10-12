import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import axios from 'axios';
import { useAppConfigStore } from '@shared-ui/stores/appConfig';

export const useAuthStore = defineStore(
  'AuthStore',
  () => {
    const appConfigStore = useAppConfigStore();

    const userName = ref('');
    const userEmail = ref('');
    const jwtToken = ref('');
    const isAuthenticated = ref(false);
    const isAdmin = ref(false);
    const verifiedUser = ref(false);

    const getEmail = computed(() => userEmail.value);

    const setUser = name => {
      userName.value = name;
    };

    const setUserEmail = email => {
      userEmail.value = email;
    };

    const setVerifiedUser = value => {
      verifiedUser.value = value;
    };

    const setToken = token => {
      jwtToken.value = token;
    };

    const setIsAuthenticated = value => {
      isAuthenticated.value = value;
    };

    const resetStore = () => {
      userName.value = '';
      userEmail.value = '';
      jwtToken.value = '';
      isAuthenticated.value = false;
      isAdmin.value = false;
    };

    async function postVerifyUserApi() {
      const res = await axios
        .post(
          `${appConfigStore.getAppConfig.apiBaseUrl}/v1/User/verifyEmail?userEmail=${getEmail.value}`
        )
        .catch(err => console.warn(err));
      setVerifiedUser(res?.data.isSuccessStatusCode);
      return res?.data;
    }

    return {
      isAuthenticated,
      isAdmin,
      userName,
      jwtToken,
      verifiedUser,
      setUser,
      setUserEmail,
      setToken,
      setIsAuthenticated,
      resetStore,
      postVerifyUserApi,
    };
  },
  {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    //@ts-ignore
    persist: true,
  }
);
