<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card class="mr-8">
    <v-tabs
      v-model="state.tab"
      class="fixed-side-tabs-bar"
      center-active
      color="blue1"
      grow
    >
      <v-tabs-slider color="blue1"></v-tabs-slider>
      <v-tab
        v-for="item in state.items"
        class="nav_tab"
        :key="item.tabName"
      >
        {{ item.tabName }}
      </v-tab>
      <v-progress-linear
        :active="isLoading"
        :indeterminate="isLoading"
        absolute
        bottom
        color="primary"
      >
      </v-progress-linear>
    </v-tabs>
    <div v-if="!isLoading && !isError">
      <v-tabs-items v-model="state.tab">
        <v-tab-item
          v-for="item in state.items"
          :key="item.tabName"
        >
          <v-container style="height: 100vh">
            <v-row dense>
              <v-col cols="12">
                <component :is="renderTabs(item.component)" />
              </v-col>
            </v-row>
          </v-container>
        </v-tab-item>
      </v-tabs-items>
    </div>
    <v-alert
      v-if="!isLoading && isError"
      border="right"
      colored-border
      type="error"
      class="grey--text"
      dense
    >
      {{ $t('No data available') }}
    </v-alert>
    <v-alert
      v-if="isLoading && !isError"
      class="grey--text"
      dense
    >
      {{ $t('Loading application detail') }}
    </v-alert>
  </v-card>
</template>

<script setup lang="ts">
import BackgroundCheckTab from '../permit-detail/tabs/BackgroundCheckTab.vue';
import CommentsTab from '../permit-detail/tabs/CommentsTab.vue';
import HistoryTab from '../permit-detail/tabs/HistoryTab.vue';
import { reactive } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useRoute } from 'vue-router/composables';

const route = useRoute();
const permitStore = usePermitsStore();

const { isLoading, isError } = useQuery(
  ['permitDetail', route.params.orderId],
  () => permitStore.getPermitDetailApi(route.params.orderId)
);

const state = reactive({
  tab: null,
  items: [
    { tabName: 'Checklist', component: 'BackgroundCheckTab' },
    { tabName: 'Comments', component: 'Comments' },
    { tabName: 'History', component: 'History' },
  ],
});

const renderTabs = item => {
  switch (item) {
    case 'History':
      return HistoryTab;
    case 'Comments':
      return CommentsTab;
    case 'BackgroundCheckTab':
      return BackgroundCheckTab;
  }
};
</script>

<style lang="scss">
.fixed-side-tabs-bar {
  position: -webkit-sticky;
  position: sticky;
  top: 8.5rem;
  z-index: 7;
}
</style>
