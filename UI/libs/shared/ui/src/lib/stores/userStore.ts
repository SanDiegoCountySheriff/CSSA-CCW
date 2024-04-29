import { UserType } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export const useUserStore = defineStore('UserStore', () => {
  const userProfile = ref<UserType>({} as UserType)
  const allUsers = ref<Array<UserType>>()
  const pendingUsers = ref<Array<UserType>>()
  const validUser = ref(true)
  const pendingReviewCount = ref<number>(0)

  const getUserState = computed(() => userProfile.value)

  const setUser = (user: UserType) => {
    userProfile.value = user
  }

  const setValidUser = (value: boolean) => {
    validUser.value = value
  }

  const setAllUsers = (value: Array<UserType>) => {
    allUsers.value = value
  }

  const setPendingUsers = (value: Array<UserType>) => {
    pendingUsers.value = value
  }

  const setPendingReviewCount = (value: number) => {
    pendingReviewCount.value = value
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

  async function getAllUsersApi() {
    const res = await axios.get(Endpoints.GET_ALL_USERS_ENDPOINT)

    if (res?.data) setAllUsers(res.data)

    return res?.data || {}
  }

  async function getAllPendingReviewUsersApi() {
    const res = await axios.get(Endpoints.GET_ALL_USERS_ENDPOINT)

    if (res?.data) {
      const users = res?.data

      const pending = users.filter(
        pendingUser => pendingUser.isPendingReview === true
      )

      setPendingUsers(pending)
      setPendingReviewCount(pending.length || 0)
    }

    return (
      res?.data.filter(pendingUser => pendingUser.isPendingReview === true) ||
      {}
    )
  }

  async function getPendingReviewCountApi() {
    return pendingReviewCount || 0
  }

  return {
    userProfile,
    validUser,
    allUsers,
    pendingUsers,
    getUserState,
    pendingReviewCount,
    getUserApi,
    putCreateUserApi,
    getAllUsersApi,
    setValidUser,
    getAllPendingReviewUsersApi,
    getPendingReviewCountApi,
  }
})
