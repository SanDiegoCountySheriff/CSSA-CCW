<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    class="mt-2 ml-8 mr-8"
    elevation="2"
  >
    <v-row class="ml-5">
      <v-col cols="4">
        <v-card
          elevation="0"
          class="text-left"
        >
          <div class="font-weight-bold grey--text text--darken-3">
            Application #{{ permitStore.getPermitDetail.application.orderId }}
          </div>
          <span class="grey--text body-2"> Submitted on November 11, 2021</span>
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
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const permitStore = usePermitsStore();
</script>
