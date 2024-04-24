import { UserType } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useUserStore = defineStore('UserStore', () => {
  const userProfile = ref<UserType>({} as UserType)
  const allUsers = ref<Array<UserType>>()
  const validUser = ref(true)

  const setUser = (user: UserType) => {
    userProfile.value = user
  }

  const setValidUser = (value: boolean) => {
    validUser.value = value
  }

  async function getUserApi() {
    const res = await axios.get(Endpoints.GET_USER_ENDPOINT)

    if (res?.data) setUser(res.data)

    return res?.data || {}
  }

  async function putCreateUserApi(user) {
    const res = await axios.put(Endpoints.PUT_CREATE_USER_ENDPOINT, user)

    if (res?.data) setUser(res.data)

    return res?.data || {}
  }

  return {
    userProfile,
    validUser,
    allUsers,
    getUserApi,
    putCreateUserApi,
    setValidUser,
  }
})
