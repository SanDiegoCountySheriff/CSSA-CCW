<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable vue-a11y/form-has-label -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/require-v-for-key -->
<template>
  <div
    class="receipt-container"
    id="print-container"
  >
    <v-row class="payment-row ma-2 text-left">
      <v-col
        cols="12"
        lg="2"
      >
        {{ $t('Date: ') }}
      </v-col>
      <v-col
        cols="12"
        lg="8"
        class="text-left"
      >
        {{ currentDate.toLocaleString() }}
      </v-col>
    </v-row>
    <v-row class="payment-row ma-2 text-left">
      <v-text-field
        dense
        readonly
        label="Name"
        v-model="state.name"
        outlined
      >
      </v-text-field>
    </v-row>
    <v-row class="payment-row ma-2 text-left">
      <v-text-field
        dense
        readonly
        label="Order Id"
        v-model="permitStore.getPermitDetail.application.orderId"
        outlined
      >
      </v-text-field>
    </v-row>
    <v-row class="payment-row ma-2 text-left">
      <v-text-field
        dense
        readonly
        label="Application Type"
        :value="
          capitalize(permitStore.getPermitDetail.application.applicationType)
        "
        outlined
      >
      </v-text-field>
    </v-row>
    <v-row class="payment-row ma-2 text-left">
      <v-select
        dense
        :items="paymentOptions"
        label="Payment Type"
        v-model="state.paymentType"
        outlined
      >
      </v-select>
    </v-row>
    <v-row class="payment-row ma-2 text-left">
      <v-text-field
        dense
        label=" Vendor Information "
        v-model="state.vendorInfo"
        outlined
      >
      </v-text-field>
    </v-row>
    <v-row class="payment-row ma-2 text-left">
      <v-text-field
        dense
        type="number"
        label="Total Amount"
        v-model="state.total"
        outlined
      >
      </v-text-field>
    </v-row>
    <v-row class="payment-row ma-2 text-left">
      <v-text-field
        dense
        label="Transaction Id"
        v-model="state.transactionId"
        outlined
      >
      </v-text-field>
    </v-row>
    <v-row class="payment-row ma-2">
      <v-spacer />
      <v-btn
        color="accent"
        class="mr-5"
        @click="submitAndPrint"
      >
        Submit
      </v-btn>
    </v-row>
    <vue-html2pdf
      :show-layout="false"
      :float-layout="true"
      :enable-download="false"
      :preview-modal="true"
      :paginate-elements-by-height="1400"
      filename="receipt"
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
          :date="currentDate.toLocaleString()"
          :name="state.name"
          :payment-type="state.paymentType"
          :total="state.total"
          :vendor-info="state.vendorInfo"
          :application-type="state.applicationType"
          :order-id="permitStore.getPermitDetail.application.orderId"
          :transaction-id="state.transactionId"
          :auth="authStore.getAuthState.userName"
        />
      </section>
    </vue-html2pdf>
    <v-snackbar
      v-model="state.snackbar"
      :timeout="4000"
      color="error"
      class="font-weight-bold"
    >
      {{ $t('Payment History failed to update. Please try again') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { capitalize } from '@shared-utils/formatters/defaultFormatters';
import { PaymentHistoryType } from '@shared-utils/types/defaultTypes';
import Receipt from '@core-admin/components/receipt/Receipt.vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import VueHtml2pdf from 'vue-html2pdf';
import { useAuthStore } from '@shared-ui/stores/auth';
import { reactive, ref } from 'vue';

interface ReceiptFormProps {
  updatePayment: any;
}

const permitStore = usePermitsStore();
const authStore = useAuthStore();

const paymentOptions = ['Initial', 'Final', 'Refund'];

const state = reactive({
  name: `${permitStore.getPermitDetail.application.personalInfo.lastName}, ${permitStore.getPermitDetail.application.personalInfo.firstName}`,
  applicationType: permitStore.getPermitDetail.application.applicationType,
  paymentType: '',
  vendorInfo: '',
  total: '',
  transactionId: '',
  date: '',
  auth: '',
  snackbar: false,
});

const props = defineProps<ReceiptFormProps>();

const html2Pdf = ref(null);
const currentDate = new Date(Date.now());

function submitAndPrint() {
  const body: PaymentHistoryType = {
    amount: state.total,
    paymentDateTimeUtc: currentDate.toISOString(),
    recordedBy: authStore.getAuthState.userName,
    paymentType: state.paymentType,
    transactionId: state.transactionId,
    vendorInfo: state.vendorInfo,
  };

  permitStore.permitDetail.paymentHistory.push(body);

  permitStore
    .updatePermitDetailApi('Payment History added')
    .then(() => {
      props.updatePayment;
      // eslint-disable-next-line @typescript-eslint/ban-ts-comment
      //@ts-ignore
      html2Pdf.value.generatePdf();
    })
    .catch(() => {
      state.snackbar = true;
    });
}
</script>

<style lang="scss" scoped>
.receipt-container {
  display: flex;
  flex-direction: column;
  justify-content: start;
  align-items: start;
  width: 60%;
}

.payment-row {
  width: 100%;
}
</style>
