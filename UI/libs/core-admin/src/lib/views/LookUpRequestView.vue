<template>
  <v-container>
    <v-tabs
      v-model="tab"
      :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
      @change="handleTabChange"
    >
      <v-tabs-slider color="primary"></v-tabs-slider>
      <v-tab> Look-Up Requests </v-tab>

      <v-tabs-items v-model="tab">
        <v-tab-item>
          <LookUpRequestTemplate @on-upload-appointments="handleShowSnackbar" />
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
import LookUpRequestTemplate from '@core-admin/components/templates/LookUpRequestTemplate.vue'
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
