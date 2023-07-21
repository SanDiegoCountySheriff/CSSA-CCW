<template>
  <div>
    <v-card-title>
      {{ $t('Payment History') }}
    </v-card-title>

    <v-divider></v-divider>

    <template v-if="permitStore.getPermitDetail.paymentHistory.length > 0">
      <v-card
        v-for="(item, index) in permitStore.getPermitDetail.paymentHistory"
        :key="index"
        elevation="0"
      >
        <v-card-title> Payment Type: {{ item.paymentType }} </v-card-title>

        <v-card-text>
          Total Amount Paid: $ {{ parseInt(item.amount).toFixed(2) }}
          <br />
          Recorded by: {{ item.recordedBy }} on {{ item.paymentDateTimeUtc }}
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn
            @click="handleRefund(item)"
            color="primary"
          >
            <v-icon left>mdi-credit-card-refund</v-icon>
            Refund
          </v-btn>

          <v-btn
            @click="reprintReceipt(item)"
            color="primary"
          >
            <v-icon left> mdi-printer </v-icon>
            Print Receipt
          </v-btn>
        </v-card-actions>

        <v-divider />
      </v-card>
    </template>

    <template v-if="permitStore.getPermitDetail.paymentHistory.length === 0">
      <v-card elevation="0">
        <v-card-title>
          {{ $t('No payment history found') }}
        </v-card-title>
      </v-card>
    </template>

    <vue-html2pdf
      :show-layout="false"
      :float-layout="true"
      :enable-download="false"
      :preview-modal="true"
      filename="receipt"
      :paginate-elements-by-height="1400"
      :pdf-quality="2"
      :manual-pagination="false"
      pdf-format="a4"
      :pdf-margin="[20, 20, 20, 20]"
      pdf-orientation="portrait"
      pdf-content-width="400px"
      ref="html2Pdf"
    >
      <section slot="pdf-content">
        <Receipt
          :date="state.date"
          :name="`${permitStore.getPermitDetail.application.personalInfo.lastName}, ${permitStore.getPermitDetail.application.personalInfo.firstName}`"
          :payment-type="state.paymentType"
          :total="state.total"
          :application-type="
            capitalize(permitStore.getPermitDetail.application.applicationType)
          "
          :order-id="permitStore.getPermitDetail.application.orderId"
          :vendor-info="state.vendor"
          :auth="state.auth"
          :transaction-id="state.transactionId"
        />
      </section>
    </vue-html2pdf>
  </div>
</template>

<script lang="ts" setup>
import Receipt from '@core-admin/components/receipt/Receipt.vue'
import VueHtml2pdf from 'vue-html2pdf'
import { capitalize } from '@shared-utils/formatters/defaultFormatters'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { reactive, ref } from 'vue'

const permitStore = usePermitsStore()
const paymentStore = usePaymentStore()
const html2Pdf = ref(null)

const state = reactive({
  paymentType: '',
  total: '',
  date: '',
  auth: '',
  vendor: '',
  transactionId: '',
})

function reprintReceipt(item) {
  state.date = new Date(item.paymentDateTimeUtc).toLocaleString()
  state.paymentType = item.paymentType
  state.total = item.amount
  state.transactionId = item.transactionId
  state.vendor = item.vendorInfo
  state.auth = item.recordedBy
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  html2Pdf.value.generatePdf()
}

async function handleRefund(transaction) {
  await paymentStore.refundPayment(
    transaction.transactionId,
    transaction.amount,
    permitStore.getPermitDetail.id
  )
}
</script>
