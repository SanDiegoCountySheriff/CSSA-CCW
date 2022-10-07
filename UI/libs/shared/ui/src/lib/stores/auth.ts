import { ref } from 'vue';
import { defineStore } from 'pinia';

export const useAuthStore = defineStore('AuthStore', () => {
  const userName = ref('');
  const jwtToken = ref('');
  const isAuthenticated = ref(false);
  const isAdmin = ref(false);

  const setUser = name => {
    userName.value = name;
  };
  const setToken = token => {
    jwtToken.value = token;
  };

  const setIsAuthenticated = value => {
    isAuthenticated.value = value;
  };

  return {
    isAuthenticated,
    isAdmin,
    userName,
    jwtToken,
    setUser,
    setToken,
    setIsAuthenticated,
  };
});
