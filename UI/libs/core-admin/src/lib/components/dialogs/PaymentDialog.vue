<template>
  <v-dialog
    fullscreen
    hide-overlay
    transition="dialog-bottom-transition"
    v-model="state.dialog"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        small
        block
        color="primary"
        v-bind="attrs"
        v-on="on"
      >
        <v-icon left>mdi-currency-usd</v-icon>
        {{ $t('Payments') }}
      </v-btn>
    </template>

    <v-card>
      <v-toolbar
        color="primary"
        dense
        dark
      >
        <v-btn
          icon
          @click="state.dialog = false"
        >
          <v-icon> mdi-close </v-icon>
        </v-btn>

        <v-toolbar-title color="primary">
          Financial Transactions
        </v-toolbar-title>
      </v-toolbar>

      <v-card-title>
        <v-progress-linear
          v-if="isRefundPaymentLoading"
          indeterminate
        ></v-progress-linear>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col
            cols="12"
            lg="6"
            md="6"
          >
            <PaymentHistory
              :loading="isRefundPaymentLoading || isLoading"
              @refund="handleRefund"
            />
          </v-col>

          <v-col
            cols="12"
            lg="6"
            md="6"
          >
            <ReceiptForm :loading="isRefundPaymentLoading || isLoading" />
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script lang="ts" setup>
import PaymentHistory from '@core-admin/components/receipt/PaymentHistory.vue'
import ReceiptForm from '@core-admin/components/receipt/ReceiptForm.vue'
import { RefundRequest } from '@shared-utils/types/defaultTypes'
import { reactive } from 'vue'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useMutation, useQuery } from '@tanstack/vue-query'

const state = reactive({
  dialog: false,
})

const paymentStore = usePaymentStore()
const permitStore = usePermitsStore()

const { isLoading, refetch } = useQuery(
  ['permitDetail'],
  () =>
    permitStore.getPermitDetailApi(
      permitStore.permitDetail.application.orderId
    ),
  {
    enabled: false,
  }
)

const { mutate: refundPayment, isLoading: isRefundPaymentLoading } =
  useMutation({
    mutationFn: (refundRequest: RefundRequest) =>
      paymentStore.refundPayment(refundRequest).then(() => {
        refetch()
      }),
  })

async function handleRefund(refundRequest: RefundRequest) {
  refundPayment(refundRequest)
}
</script>
