<template>
  <v-card
    :loading="isLoading"
    outlined
  >
    <v-tabs
      v-model="state.tab"
      :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
      center-active
      grow
    >
      <v-tabs-slider color="primary"></v-tabs-slider>
      <v-tab
        v-for="item in state.items"
        :key="item.tabName"
      >
        {{ item.tabName }}
      </v-tab>
    </v-tabs>

    <v-tabs-items v-model="state.tab">
      <v-tab-item>
        <BackgroundCheckTab />
      </v-tab-item>
      <v-tab-item>
        <HistoryTab />
      </v-tab-item>
    </v-tabs-items>
  </v-card>
</template>

<script setup lang="ts">
import BackgroundCheckTab from '../permit-detail/tabs/BackgroundCheckTab.vue'
import HistoryTab from '../permit-detail/tabs/HistoryTab.vue'
import { reactive } from 'vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import { useRoute } from 'vue-router/composables'
import { useThemeStore } from '@shared-ui/stores/themeStore'

const route = useRoute()
const permitStore = usePermitsStore()
const themeStore = useThemeStore()

const { isLoading } = useQuery(['permitDetail', route.params.orderId], () =>
  permitStore.getPermitDetailApi(route.params.orderId)
)

const state = reactive({
  tab: null,
  items: [
    { tabName: 'Checklist', component: 'BackgroundCheckTab' },
    { tabName: 'History', component: 'History' },
  ],
})
</script>
