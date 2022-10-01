import { computed, ref } from 'vue';
import { defineStore } from 'pinia';

export const useUserStore = defineStore('user', () => {
  const user = ref({
    email: '',
    // Need a way to secure this, {should not store this}
    password: '',
  });
  const getUser = computed(() => user.value);

  function setUser(payload) {
    user.value = payload;
  }

  function deleteUser() {
    user.value = {
      email: '',
      // Need a way to secure this, {should not store this}
      password: '',
    };
  }

  return {
    user,
    getUser,
    setUser,
    deleteUser,
  };
});
