<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title class="subtitle-2">
      <v-spacer></v-spacer>
      <v-btn @click="update"> Refresh </v-btn>
    </v-card-title>
    <v-row>
      <v-col cols="12">
        <v-card-text>
          <v-timeline
            dense
            class="history-container"
          >
            <v-slide-x-reverse-transition
              group
              origin
              leave-absolute
            >
              <v-timeline-item
                v-for="(item, index) in state.history"
                :key="index"
                color="primary"
                class="mb-3 pr-4"
                small
                dot
              >
                <v-row justify="center">
                  <v-col
                    class="text-left small-text"
                    cols="8"
                  >
                    {{ item.changeMadeBy }}
                  </v-col>
                  <v-col
                    class="text-right small-text"
                    cols="4"
                  >
                    {{ formatDate(item.changeDateTimeUtc) }}
                  </v-col>
                </v-row>
                <v-row>
                  <v-col
                    class="text-left small-text"
                    cols="8"
                  >
                    {{ item.change }}
                  </v-col>
                  <v-col
                    class="text-right small-text"
                    cols="4"
                  >
                    {{ formatTime(item.changeDateTimeUtc) }}
                  </v-col>
                </v-row>
              </v-timeline-item>
            </v-slide-x-reverse-transition>
          </v-timeline>
        </v-card-text>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import {
  formatDate,
  formatInitials,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'
import { onBeforeUnmount, reactive } from 'vue'

const permitStore = usePermitsStore()

const state = reactive({
  interval: null,
  history: permitStore.getPermitDetail.history.reverse(),
  nonce: 2,
  initials: formatInitials(
    permitStore.getPermitDetail.application.personalInfo.firstName,
    permitStore.getPermitDetail.application.personalInfo.lastName
  ),
})

const { refetch } = useQuery(['history'], permitStore.getHistoryApi, {
  enabled: false,
  onSuccess: data => {
    state.history = data
  },
})

onBeforeUnmount(() => {
  stop()
})

function update() {
  refetch()
}
</script>

<style lang="scss" scoped>
.history-container {
  max-height: 100vh;
  overflow-y: auto;
  overflow-x: hidden;
}
.small-text {
  font-size: 0.7rem !important;
  padding: 2px;
}
</style>
