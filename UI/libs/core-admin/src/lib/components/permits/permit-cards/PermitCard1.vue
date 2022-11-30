<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    class="mt-2 ml-8 mr-8"
    elevation="2"
  >
    <v-container
      v-if="isLoading"
      fluid
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,divider, list-item"
      >
      </v-skeleton-loader>
    </v-container>
    <v-row
      class="ml-5"
      v-else
    >
      <v-col cols="4">
        <v-card
          elevation="0"
          class="text-left"
        >
          <div class="font-weight-bold grey--text text--darken-3">
            Application #{{ permitStore.getPermitDetail.application.orderId }}
          </div>
          <span class="grey--text body-2">
            Submitted on {{ submittedDate }}</span
          >
        </v-card>
      </v-col>
      <v-col cols="4">
        <v-card elevation="0">
          <v-chip
            class="ml-4"
            color="blue lighten-4"
            text-color="blue"
          >
            New ...
          </v-chip>
          <v-chip
            class="ml-4"
            text-color="grey darken-2"
          >
            2 year ...
          </v-chip>
          <v-chip
            class="ml-4"
            color="green lighten-3"
            text-color="green darken-4"
          >
            <v-icon
              left
              small
            >
              mdi-checkbox-marked-circle
            </v-icon>
            Paid ...
          </v-chip>
        </v-card>
      </v-col>
      <v-col cols="4">
        <v-card
          elevation="0"
          class="text-right mr-4"
        >
          <v-chip
            class="ml-4"
            color="warning lighten-5"
            text-color="warning"
            label
          >
            {{ $t(' Withdraw') }}
          </v-chip>
          <v-chip
            class="ml-4"
            color="error lighten-5"
            text-color="error"
            label
          >
            {{ $t('Deny') }}
          </v-chip>
          <v-chip
            class="ml-4"
            color="green lighten-4"
            text-color="green darken-4"
            label
          >
            {{ $t('Approve') }}
          </v-chip>
        </v-card>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const { isLoading } = useQuery(['permitDetail']);

window.console.log(isLoading);

const permitStore = usePermitsStore();
const submittedDate = ref(
  new Date(
    permitStore.getPermitDetail.history[0]?.changeDateTimeUtc
  ).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  }) || ''
);
</script>
