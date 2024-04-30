<template>
  <v-container fluid>
    <v-card
      :loading="isLoading || isRefundLoading || isFetching"
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
            <RefundRequestConfirmationDialog
              v-model="refundAmount"
              :refund-request="item"
              @confirm="handleConfirmRefundRequest(item)"
            />
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { RefundRequest } from '@shared-utils/types/defaultTypes'
import RefundRequestConfirmationDialog from '@core-admin/components/dialogs/RefundRequestConfirmationDialog.vue'
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { useMutation, useQuery } from '@tanstack/vue-query'

const paymentStore = usePaymentStore()
const brandStore = useBrandStore()

const { isLoading, data, refetch, isFetching } = useQuery(
  ['getAllRefundRequests'],
  paymentStore.getAllRefundRequests
)

const refundAmount = ref(0)

const { isLoading: isRefundLoading, mutateAsync: refundPayment } = useMutation({
  mutationFn: (item: RefundRequest) => {
    item.refundAmount = refundAmount.value

    return paymentStore.refundPayment(
      item,
      brandStore.brand.cost.creditFee / 100
    )
  },
})

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

async function handleConfirmRefundRequest(item: RefundRequest) {
  await refundPayment(item)
  refetch()
}
</script>
