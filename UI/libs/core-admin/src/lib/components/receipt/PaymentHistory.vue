<template>
  <v-container>
    <v-banner
      single-line
      class="text-left"
    >
      {{ $t('Payment History')}}
    </v-banner>
    <div
      v-if="permitStore.getPermitDetail.paymentHistory.length > 0"
      >
      <v-card
        v-for="(item, index) in permitStore.getPermitDetail.paymentHistory"
        :key="index"
        elevation="0"
      >
        <v-card-title> {{ item.comments }} </v-card-title>
        <v-card-text> {{ item.amount }} </v-card-text>
        <v-card-text>
          {{ item.recordedBy }} on {{ item.paymentDateTimeUtc }}
        </v-card-text>
      </v-card>
    </div>
    <div v-if="permitStore.getPermitDetail.paymentHistory.length === 0">
      <v-card
        elevation="0"
      >
        <v-card-title
        >
          {{ $t('No payment history found') }}
        </v-card-title>
      </v-card>
    </div>
  </v-container>
</template>

<script lang="ts" setup>
import { usePermitsStore } from "@core-admin/stores/permitsStore";

const permitStore = usePermitsStore();
</script>

<style lang="scss" scoped></style>
