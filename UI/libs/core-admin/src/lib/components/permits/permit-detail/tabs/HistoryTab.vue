<template>
  <v-card :loading="isLoading">
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
import { reactive } from 'vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'

const permitStore = usePermitsStore()

const state = reactive({
  history: permitStore.getPermitDetail.history.sort((a, b) => {
    if (new Date(a.changeDateTimeUtc) < new Date(b.changeDateTimeUtc)) {
      return 1
    }

    if (new Date(a.changeDateTimeUtc) > new Date(b.changeDateTimeUtc)) {
      return -1
    }

    return 0
  }),
})

const { refetch, isLoading } = useQuery(
  ['history'],
  permitStore.getHistoryApi,
  {
    onSuccess: data => {
      state.history = data
    },
  }
)

function update() {
  refetch()
}
</script>
