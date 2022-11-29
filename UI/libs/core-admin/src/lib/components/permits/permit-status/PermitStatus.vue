<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card>
    <v-tabs
      v-model="state.tab"
      class="fixed-side-tabs-bar"
      grow
    >
      <span
        v-for="(item, index) in state.items"
        :key="index"
      >
        <v-tab
          @click="$vuetify.goTo('#side_sec_' + index)"
          class="nav_tab"
        >
          {{ item }}
        </v-tab>
      </span>
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
      <div
        v-for="(item, index) in state.items"
        :key="index"
      >
        <v-container>
          <v-row dense>
            <v-col cols="12">
              <div :id="'side_sec_' + index">
                <span :id="'side_span_' + index"></span>
                {{ item }}

                <component :is="renderTabs(item)" />
              </div>
            </v-col>
          </v-row>
        </v-container>
      </div>
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
import { useQuery } from '@tanstack/vue-query';

const { isLoading, isError } = useQuery(['permitDetail']);

const state = reactive({
  tab: 0,
  items: ['Checklist', 'Comments', 'History'],
});

const renderTabs = item => {
  switch (item) {
    case 'History':
      return HistoryTab;
    case 'Comments':
      return CommentsTab;
    default:
      return BackgroundCheckTab;
  }
};
</script>

<style lang="scss">
.fixed-side-tabs-bar.theme--light.v-tabs > .v-tabs-bar {
  background-color: #f2f2f2 !important;
}

.fixed-side-tabs-bar {
  position: -webkit-sticky;
  position: sticky;
  top: 4rem;
  z-index: 999;

  .v-tabs-bar__content {
    padding-top: 15px;
  }
}
</style>
