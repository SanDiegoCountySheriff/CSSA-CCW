<template>
  <div>
    <template v-if="permitStore.getPermitDetail.paymentHistory.length > 0">
      <v-card flat>
        <v-card-title> {{ $t('Payment History') }} </v-card-title>

        <v-divider />

        <v-card-text
          v-for="(item, index) in permitStore.getPermitDetail.paymentHistory"
          :key="index"
        >
          Initial Payment: $ {{ Number.parseFloat(item.amount).toFixed(2) }}
          <br />
          Refunded Amount: $
          {{ Number.parseFloat(item.refundAmount).toFixed(2) }}
          <br />
          Total Payment: $
          {{
            (
              Number.parseFloat(item.amount) -
              Number.parseFloat(item.refundAmount)
            ).toFixed(2)
          }}
          <br />
          Vendor: {{ item.vendorInfo }} <br />
          Recorded By: {{ item.recordedBy ?? 'Customer' }} <br />
          Date: {{ new Date(item.paymentDateTimeUtc).toLocaleString() }}
          <br />
          Payment Type: {{ PaymentType[item.paymentType] }}
          <br />
          Submitted: {{ paymentStatuses[item.paymentStatus].name }}
          <br />
          Verified:
          {{
            item.verified
              ? 'Transaction ID is verified with vendor'
              : 'Payment status is not verified, look up on vendor website to verify'
          }}

          <v-card-actions>
            <RefundDialog
              :application-id="permitStore.getPermitDetail.id"
              :disabled="
                Number.parseFloat(item.amount).toFixed(2) ===
                Number.parseFloat(item.refundAmount).toFixed(2)
              "
              :payment="item"
              :loading="loading"
              v-on="$listeners"
            />

            <VerifyTransactionDialog
              v-if="!item.verified"
              @confirm="handleConfirm(item, $event)"
            />

            <v-spacer />

            <DeletePaymentDialog
              :item="item"
              :index="index"
              :loading="loading"
              @confirm="handleDeleteTransaction"
              @confirm-heartland="handleDeleteHeartlandTransaction"
            />

            <v-btn
              :disabled="loading"
              @click="reprintReceipt(item)"
              color="primary"
              small
            >
              <v-icon left> mdi-printer </v-icon>
              Print Receipt
            </v-btn>
          </v-card-actions>

          <v-divider />
        </v-card-text>

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
            ApplicationType[
              permitStore.getPermitDetail.application.applicationType
            ].toString()
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
import { ApplicationType } from '@shared-utils/types/defaultTypes'
import DeletePaymentDialog from '@core-admin/components/dialogs/DeletePaymentDialog.vue'
import Receipt from '@core-admin/components/receipt/Receipt.vue'
import RefundDialog from '@core-admin/components/dialogs/RefundDialog.vue'
import VerifyTransactionDialog from '@core-admin/components/dialogs/VerifyTransactionDialog.vue'
import VueHtml2pdf from 'vue-html2pdf'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  PaymentHistoryType,
  PaymentType,
} from '@shared-utils/types/defaultTypes'
import { reactive, ref } from 'vue'

interface PaymentHistoryProps {
  loading: boolean
}

defineProps<PaymentHistoryProps>()
const emit = defineEmits([
  'delete-transaction',
  'verify-transaction',
  'delete-heartland-transaction',
])

const permitStore = usePermitsStore()
const html2Pdf = ref(null)

const state = reactive({
  paymentType: '',
  total: '',
  date: '',
  auth: '',
  vendor: '',
  transactionId: '',
})

const paymentStatuses = [
  { name: 'None', value: 0 },
  { name: 'In Person', value: 1 },
  { name: 'Submitted Online', value: 2 },
]

function reprintReceipt(item) {
  state.date = new Date(item.paymentDateTimeUtc).toLocaleString()
  state.paymentType = PaymentType[item.paymentType]
  state.total = item.amount
  state.transactionId = item.transactionId
  state.vendor = item.vendorInfo
  state.auth = item.recordedBy
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  html2Pdf.value.generatePdf()
}

function handleDeleteTransaction(index: number) {
  emit('delete-transaction', index)
}

function handleDeleteHeartlandTransaction(index: number) {
  const paymentType = permitStore.permitDetail.paymentHistory[index].paymentType

  if (
    paymentType === PaymentType['CCW Application Initial Payment'] ||
    paymentType === PaymentType['CCW Application Initial Judicial Payment'] ||
    paymentType === PaymentType['CCW Application Initial Reserve Payment'] ||
    paymentType === PaymentType['CCW Application Initial Employent Payment']
  ) {
    permitStore.permitDetail.application.readyForInitialPayment = true
  }

  if (
    paymentType === PaymentType['CCW Application Modification Payment'] ||
    paymentType ===
      PaymentType['CCW Application Modification Judicial Payment'] ||
    paymentType ===
      PaymentType['CCW Application Modification Reserve Payment'] ||
    paymentType ===
      PaymentType['CCW Application Modification Employment Payment']
  ) {
    permitStore.permitDetail.application.readyForModificationPayment = true
  }

  if (
    paymentType === PaymentType['CCW Application Renewal Payment'] ||
    paymentType === PaymentType['CCW Application Renewal Reserve Payment'] ||
    paymentType === PaymentType['CCW Application Renewal Judicial Payment'] ||
    paymentType === PaymentType['CCW Application Renewal Employment Payment']
  ) {
    permitStore.permitDetail.application.readyForRenewalPayment = true
  }

  if (paymentType === PaymentType['CCW Application Issuance Payment']) {
    permitStore.permitDetail.application.readyForIssuancePayment = true
  }

  emit('delete-heartland-transaction', index)
}

function handleConfirm(
  paymentHistory: PaymentHistoryType,
  transactionId: string
) {
  paymentHistory.transactionId = transactionId
  paymentHistory.verified = true

  if (
    paymentHistory.paymentType ===
      PaymentType['CCW Application Initial Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Initial Judicial Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Initial Reserve Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Initial Employent Payment']
  ) {
    permitStore.permitDetail.application.readyForInitialPayment = false
  }

  if (
    paymentHistory.paymentType ===
      PaymentType['CCW Application Modification Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Modification Judicial Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Modification Reserve Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Modification Employment Payment']
  ) {
    permitStore.permitDetail.application.readyForModificationPayment = false
  }

  if (
    paymentHistory.paymentType ===
      PaymentType['CCW Application Renewal Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Renewal Reserve Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Renewal Judicial Payment'] ||
    paymentHistory.paymentType ===
      PaymentType['CCW Application Renewal Employment Payment']
  ) {
    permitStore.permitDetail.application.readyForRenewalPayment = false
  }

  if (
    paymentHistory.paymentType ===
    PaymentType['CCW Application Issuance Payment']
  ) {
    permitStore.permitDetail.application.readyForIssuancePayment = false
  }

  emit('verify-transaction')
}
</script>
