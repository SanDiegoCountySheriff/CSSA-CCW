<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    elevation="0"
    :loading="isLoading"
  >
    <v-card-title>
      <v-spacer></v-spacer>
      <v-btn
        @click="update"
        color="primary"
        small
      >
        <v-icon left> mdi-refresh </v-icon>
        Refresh
      </v-btn>
    </v-card-title>

    <v-card-text>
      <v-timeline dense>
        <v-timeline-item
          v-for="(item, index) in state.history"
          :key="index"
          color="primary"
          small
          dot
        >
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title class="text-wrap mb-1">
                {{ item.changeMadeBy }}
              </v-list-item-title>
              <v-list-item-title class="text-wrap">
                {{ item.change }}
              </v-list-item-title>
              <v-list-item-subtitle class="text-wrap">
                {{ new Date(item.changeDateTimeUtc).toLocaleString() }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-timeline-item>
      </v-timeline>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { formatInitials } from '@shared-utils/formatters/defaultFormatters'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
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

const { refetch, isLoading } = useQuery(
  ['history'],
  permitStore.getHistoryApi,
  {
    enabled: false,
    onSuccess: data => {
      state.history = data
    },
  }
)

onBeforeUnmount(() => {
  stop()
})

function update() {
  refetch()
}
</script>
