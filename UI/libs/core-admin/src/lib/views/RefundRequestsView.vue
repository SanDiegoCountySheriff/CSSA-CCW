<template>
  <v-container fluid>
    <v-card :loading="isLoading">
      <v-card-title>Refund Requests</v-card-title>

      <v-card-text>
        {{ data }}
        <v-data-table> </v-data-table>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { useQuery } from '@tanstack/vue-query'

const paymentStore = usePaymentStore()

const { isLoading, data } = useQuery(
  ['getAllRefundRequests'],
  paymentStore.getAllRefundRequests
)
</script>
