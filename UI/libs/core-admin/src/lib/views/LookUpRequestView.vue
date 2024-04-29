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
          <!-- <v-toolbar>
            <v-text-field
              prepend-icon="mdi-magnify"
              class="mx-3"
              flat
              label="Search"
            ></v-text-field>
          </v-toolbar> -->
          <div>
            <v-toolbar tab>
              <v-text-field
                class="mx-3"
                flat
                label="Search"
                prepend-icon="mdi-magnify"
              ></v-text-field>

              <template v-slot:extension>
                <v-tabs
                  v-model="tab"
                  centered
                  :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
                  slider-color="primary"
                  @change="getPendingUsers"
                >
                  <v-tab> Potential Matches </v-tab>
                  <v-tab> Undo Matches </v-tab>
                </v-tabs>
              </template>
            </v-toolbar>
          </div>
          <!-- <v-card-title>
            <v-tabs
              v-model="tab"
              :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
              @change="handleTabChange"
              slider-color="primary"
            >
              <v-tab> Potential Matches </v-tab>
              <v-tab> Undo Matches </v-tab>
            </v-tabs>
          </v-card-title> -->

          <v-tabs-items v-model="tab">
            <v-tab-item>
              <LookUpRequestTemplate
                @on-upload-appointments="handleShowSnackbar"
              />
            </v-tab-item>
          </v-tabs-items>
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
      queryKey: ['getAllPendingReviewUsersApi'],
    })
  }
}

function getPendingUsers() {
  if (tab.value === 1) {
    queryClient.resetQueries({
      queryKey: ['getAllPendingReviewUsersApi'],
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
