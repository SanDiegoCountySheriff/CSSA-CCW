<template>
  <v-container fluid>
    <v-card
      :loading="isLoading"
      flat
    >
      <v-card-title>Refund Requests</v-card-title>

      <v-card-text>
        <v-data-table
          :items="data"
          :headers="headers"
        >
          <template #[`item.orderId`]="props">
            <router-link
              :to="{
                name: 'PermitDetail',
                params: { orderId: props.item.orderId },
              }"
              style="text-decoration: underline; color: inherit"
            >
              {{ props.item.orderId }}
            </router-link>
          </template>

          <template #[`item.refundAmount`]="{ item }">
            ${{ item.refundAmount }}
          </template>

          <template #[`item.actions`]="{ item }">
            <v-btn
              @click="console.log(item)"
              color="success"
              icon
            >
              <v-icon>mdi-check-bold</v-icon>
            </v-btn>
          </template>
        </v-data-table>
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

const headers = [
  {
    text: 'Order ID',
    value: 'orderId',
  },
  {
    text: 'Transaction ID',
    value: 'transactionId',
  },
  {
    text: 'Refund Amount',
    value: 'refundAmount',
  },
  {
    text: 'Refund Reason',
    value: 'reason',
  },
  {
    text: 'Actions',
    value: 'actions',
  },
]
</script>
