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
          <v-timeline dense>
            <v-slide-x-reverse-transition
              group
              origin
              leave-absolute
            >
              <v-timeline-item
                v-for="(item, index) in state.history"
                :key="index"
                color="info"
                class="mb-4"
                medium
                fill-dot
              >
                <template #icon>
                  <span class="white--text">{{ state.initials }}</span>
                </template>
                <v-row justify="space-between">
                  <v-col cols="6">
                    {{ item.change }}
                  </v-col>
                  <v-col
                    class="text-right"
                    cols="6"
                  >
                    {{ formatDate(item.changeDateTimeUtc) }}
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
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import {
  formatDate,
  formatInitials,
} from '@shared-utils/formatters/defaultFormatters';
import { onBeforeUnmount, reactive } from 'vue';

const permitStore = usePermitsStore();

const state = reactive({
  interval: null,
  history: permitStore.getPermitDetail.history,
  nonce: 2,
  initials: formatInitials(
    permitStore.getPermitDetail.application.personalInfo.firstName,
    permitStore.getPermitDetail.application.personalInfo.lastName
  ),
});

const { refetch } = useQuery(['history'], permitStore.getHistoryApi, {
  enabled: false,
  onSuccess: data => {
    state.history = data;
  },
});

onBeforeUnmount(() => {
  stop();
});

function update() {
  refetch();
}
</script>
