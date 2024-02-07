<template>
  <div>
    <template v-if="permitStore.getPermitDetail.paymentHistory.length > 0">
      <v-card flat>
        <v-card-title> {{ $t('Payment History') }} </v-card-title>

        <v-divider />

        <v-card-text
          v-for="(
            item, index
          ) in permitStore.getPermitDetail.paymentHistory.filter(
            ph => ph.successful === true
          )"
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

          <v-card-actions>
            <RefundDialog
              :application-id="permitStore.getPermitDetail.id"
              :payment="item"
              :loading="loading"
              v-on="$listeners"
            />

            <v-spacer />

            <v-dialog
              v-if="item.paymentStatus === 1"
              v-model="dialog"
              max-width="600px"
            >
              <template #activator="{ on, attrs }">
                <v-btn
                  v-on="on"
                  v-bind="attrs"
                  color="primary"
                  small
                >
                  <v-icon left>mdi-delete</v-icon>
                  Delete
                </v-btn>
              </template>

              <v-card>
                <v-card-title>
                  Delete Transaction {{ item.transactionId }}
                </v-card-title>

                <v-card-text>
                  Are you sure you wish to delete transaction
                  {{ item.transactionId }} for ${{ item.amount }}? This cannot
                  be undone.
                </v-card-text>

                <v-card-actions>
                  <v-btn
                    @click="dialog = !dialog"
                    color="error"
                  >
                    Cancel
                  </v-btn>

                  <v-spacer />

                  <v-btn
                    @click="handleDeleteTransaction(item)"
                    color="primary"
                  >
                    Yes
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>

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
import RefundDialog from '@core-admin/components/dialogs/RefundDialog.vue'
import VueHtml2pdf from 'vue-html2pdf'
import { capitalize } from '@shared-utils/formatters/defaultFormatters'
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
const emit = defineEmits(['delete-transaction'])

const permitStore = usePermitsStore()
const html2Pdf = ref(null)
const dialog = ref(false)

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

function handleDeleteTransaction(paymentHistory: PaymentHistoryType) {
  emit('delete-transaction', paymentHistory)
}
</script>
