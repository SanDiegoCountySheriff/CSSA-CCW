<template>
  <v-container>
    <v-row>
      <v-col
        cols="8"
        lg="4"
      >
        <v-expansion-panel
          v-for="(profile, index) in userProfiles"
          :key="index"
        >
          <v-expansion-panel-header>
            {{ profile.firstName }} {{ profile.lastName }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-row> Search Potential Matches</v-row>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-col>
      <v-col
        cols="1"
        lg="8"
      >
        <v-card
          outlined
          color="grey"
        >
          <v-card-title :color="$vuetify.theme.dark ? 'white' : 'primary'">
            Look Up Results
          </v-card-title>
          <v-btn
            color="primary"
            @click="getAllUsers"
            >API</v-btn
          >
          <v-card-header :color="$vuetify.theme.dark ? 'white' : 'primary'">
            Please select a request to perform look up
          </v-card-header>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import {
  LicenseType,
  AppointmentManagement,
  LookUpRequestType,
  PersonalInfoType,
  UserType,
} from '@shared-utils/types/defaultTypes'

import { useUserStore } from '@shared-ui/stores/userStore'
import { computed, onMounted, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'
const userStore = useUserStore()
const user = computed(() => userStore.userProfile)
const userProfiles = ref<UserType[]>([])

const { isFetching: isAllUsersLoading } = useQuery(
  ['getAllUsersApi'],
  () => userStore.getAllUsersApi(),
  {}
)

const { mutate: getAllUsers } = useMutation(
  ['getAllUserApi'],
  async () => {
    // Fetch user profiles from the API
    const data = await userStore.getAllUsersApi()

    // Update the userProfiles variable with the fetched data
    userProfiles.value = data
  },
  {
    onSuccess: async () => {
      // Optionally, you can perform additional actions after successful mutation
      window.console.log('All user profiles fetched successfully!')
    },
  }
)
</script>

<style lang="scss">
::-webkit-calendar-picker-indicator {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  width: auto;
  height: auto;
  color: transparent;
  background: transparent;
}
input::-webkit-date-and-time-value {
  text-align: left;
}

.v-calendar-daily__scroll-area {
  overflow-y: hidden !important;
}

.v-calendar-daily__head {
  margin-right: 0px !important;
}

.theme--dark.v-calendar-daily .v-calendar-daily__intervals-body {
  border-bottom: #9e9e9e 1px solid;
}

.theme--light.v-calendar-daily .v-calendar-daily__intervals-body {
  border-bottom: #e0e0e0 1px solid;
}

.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
