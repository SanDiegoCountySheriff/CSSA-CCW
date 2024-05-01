import Endpoints from '@shared-ui/api/endpoints'
import { UserType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export const useUserStore = defineStore('UserStore', () => {
  const userProfile = ref<UserType>({} as UserType)

  const getUserState = computed(() => userProfile.value)

  const setUser = (user: UserType) => {
    userProfile.value = user
  }

  async function getUser() {
    const res = await axios.get(Endpoints.GET_USER_ENDPOINT)

    if (res?.data) setUser(res.data)

    return res?.data || {}
  }

  async function putCreateUser(user) {
    const res = await axios.put(Endpoints.PUT_CREATE_USER_ENDPOINT, user)

    if (res?.data) setUser(res.data)

    return res?.data || {}
  }

  return {
    userProfile,
    setUser,
    getUserState,
    getUser,
    putCreateUser,
  }
})
