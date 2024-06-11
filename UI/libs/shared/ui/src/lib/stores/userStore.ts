import Endpoints from '@shared-ui/api/endpoints'
import { UserType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export const useUserStore = defineStore('UserStore', () => {
  const userProfile = ref<UserType>({} as UserType)
  const unmatchedUsersCount = ref(0)

  const getUserState = computed(() => userProfile.value)

  const setUser = (user: UserType) => {
    userProfile.value = user
  }

  async function getUser() {
    const res = await axios.get(Endpoints.GET_USER_ENDPOINT)

    if (res?.data) {
      setUser(res.data)
    }

    return res
  }

  async function putCreateUser() {
    const res = await axios.put(Endpoints.PUT_CREATE_USER_ENDPOINT)

    if (res?.data) {
      setUser(res.data)
    }

    return res?.data || {}
  }

  async function updateUserProfile(user: UserType) {
    const res = await axios.post(Endpoints.UPDATE_USER_PROFILE_ENDPOINT, user)

    if (res?.data) {
      setUser(res.data)
    }

    return res?.data || {}
  }

  async function updateUserProfileAdmin(user: UserType) {
    const res = await axios.post(
      Endpoints.UPDATE_USER_PROFILE_ADMIN_ENDPOINT,
      user
    )

    if (res?.data) {
      setUser(res.data)
    }

    return res?.data || {}
  }

  async function getUnmatchedUsers() {
    const res = await axios.get(Endpoints.GET_UNMATCHED_USERS_ENDPOINT)

    if (res?.data) {
      unmatchedUsersCount.value = res.data.length

      return res.data
    }
  }

  return {
    userProfile,
    unmatchedUsersCount,
    setUser,
    getUserState,
    getUser,
    putCreateUser,
    getUnmatchedUsers,
    updateUserProfile,
    updateUserProfileAdmin,
  }
})
