<template>
  <v-container>
    <v-tabs
      v-model="tab"
      :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
      @change="handleTabChange"
    >
      <v-tabs-slider color="primary"></v-tabs-slider>
      <v-tab> Appointment Template </v-tab>
      <v-tab> Appointment Management </v-tab>

      <v-tabs-items v-model="tab">
        <v-tab-item>
          <AppointmentTemplate @on-upload-appointments="handleShowSnackbar" />
        </v-tab-item>
        <v-tab-item>
          <AppointmentManagement @on-delete-appointments="handleShowSnackbar" />
        </v-tab-item>
      </v-tabs-items>
    </v-tabs>

    <v-snackbar
      v-model="snackbar"
      color="primary"
    >
      {{ message }}
      <template #action="{ attrs }">
        <v-btn
          text
          v-bind="attrs"
          @click="snackbar = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import AppointmentManagement from '@core-admin/components/templates/AppointmentManagement.vue'
import AppointmentTemplate from '@core-admin/components/templates/AppointmentTemplate.vue'
import { ref } from 'vue'
import { useQueryClient } from '@tanstack/vue-query'
import { useThemeStore } from '@shared-ui/stores/themeStore'

const queryClient = useQueryClient()
const tab = ref(null)
const themeStore = useThemeStore()
const snackbar = ref(false)
const message = ref('')

function handleTabChange() {
  if (tab.value === 1) {
    queryClient.resetQueries({
      queryKey: ['getAppointments'],
    })
  }
}

function handleShowSnackbar(notification) {
  message.value = notification
  snackbar.value = true
}
</script>

<style lang="scss">
.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
