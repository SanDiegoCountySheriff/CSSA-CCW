<template>
  <v-container fluid>
    <v-col>
      <v-row>
        <v-col>
          <v-expansion-panels
            class="my-4"
            inset
          >
            <v-expansion-panel
              v-for="(request, index) in userStore.allUsers"
              :key="index"
              title="Item"
            >
              <v-expansion-panel-header>
                <b> {{ request.firstName }} {{ request.lastName }}</b>
                Appointment Date: {{ request.appointmentDate }}
              </v-expansion-panel-header>
              <v-expansion-panel-content>
                <v-row>
                  <v-col>
                    <b>Driver License: </b> {{ request.driversLicenseNumber }}
                  </v-col>
                  <v-col>
                    <b>Agency License: </b> {{ request.permitNumber }}
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <b>Documents: </b> {{ request.driversLicenseNumber }}
                  </v-col>
                  <v-col> <b>Email:</b> {{ request.email }} </v-col>
                </v-row>
              </v-expansion-panel-content>
            </v-expansion-panel>
          </v-expansion-panels>
        </v-col>
      </v-row>
    </v-col>
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
  ['getAllPendingReviewUsersApi'],
  () => userStore.getAllPendingReviewUsersApi(),
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
