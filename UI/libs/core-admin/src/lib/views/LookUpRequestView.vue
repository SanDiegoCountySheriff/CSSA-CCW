<template>
  <v-container fluid>
    <div class="mb-5">
      <v-card color="#EEEEEE">
        <v-card-title> Match Legacy Applications</v-card-title></v-card
      >
    </div>
    <v-row>
      <v-col
        col="4"
        lg="4"
      >
        <v-card height="100%">
          <v-tabs
            v-model="tab"
            :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
            @change="handleTabChange"
          >
            <v-tabs-slider color="primary"></v-tabs-slider>

            <v-tab> Look-Up Requests </v-tab>
            <v-tab> Upcoming Appointments </v-tab>

            <v-tabs-items v-model="tab">
              <v-tab-item>
                <LookUpRequestTemplate
                  @on-upload-appointments="handleShowSnackbar"
                />
              </v-tab-item>
            </v-tabs-items>
          </v-tabs>
        </v-card>
      </v-col>
      <v-col
        col="4"
        lg="8"
      >
        <v-card height="900">
          <v-card-title>
            <v-tabs
              v-model="tab"
              :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
              @change="handleTabChange"
            >
              <v-tabs-slider color="primary"></v-tabs-slider>

              <v-tab> Potential Matches </v-tab>
              <v-tab-item> Potential Matches </v-tab-item>
              <v-tab> Undo Matches </v-tab>
              <v-tab-item>Undo Matches </v-tab-item>

              <v-tabs-items v-model="tab"> </v-tabs-items>
            </v-tabs>
          </v-card-title>
        </v-card>
      </v-col>
    </v-row>
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
