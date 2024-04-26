<template>
  <v-container>
    <v-row>
      <v-col
        cols="8"
        lg="4"
      >
        <v-expansion-panels
          class="my-4"
          variant="inset"
        >
          <v-expansion-panel
            v-for="(request, index) in userStore.allUsers"
            :key="index"
            text="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
            title="Item"
          >
            <v-expansion-panel-header>
              <b> {{ request.firstName }} {{ request.lastName }}</b> Appointment
              Date: {{ request.appointmentDate }}
            </v-expansion-panel-header>
            <v-expansion-panel-content>
              <v-row>
                <v-col>
                  <b>Driver License: {{ request.driversLicenseNumber }}</b>
                </v-col>
                <v-col>
                  <b>Agency License: {{ request.permitNumber }}</b>
                </v-col>
              </v-row>
            </v-expansion-panel-content>
          </v-expansion-panel>
        </v-expansion-panels>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { UserType } from '@shared-utils/types/defaultTypes'
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
