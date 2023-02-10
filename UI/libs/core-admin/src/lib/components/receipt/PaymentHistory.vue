<template>
  <v-container>
    <v-banner
      single-line
      class="text-left"
    >
      {{ $t('Payment History') }}
    </v-banner>
    <div
      v-if="permitStore.getPermitDetail.paymentHistory.length > 0"
      class="payment-history-container"
    >
      <v-card
        v-for="(item, index) in permitStore.getPermitDetail.paymentHistory"
        :key="index"
        elevation="0"
      >
        <v-card-title> Payment Type: {{ item.comments }} </v-card-title>
        <v-card-text>
          Total Amount Paid: $ {{ parseInt(item.amount).toFixed(2) }}
        </v-card-text>
        <v-card-text>
          Recorded by: {{ item.recordedBy }} on {{ item.paymentDateTimeUtc }}
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            icon
            color="info"
            @click="reprintReceipt(item)"
          >
            <v-icon
              small
              color="info"
            >
              mdi-printer
            </v-icon>
          </v-btn>
        </v-card-actions>
        <v-divider />
      </v-card>
    </div>
    <div v-if="permitStore.getPermitDetail.paymentHistory.length === 0">
      <v-card elevation="0">
        <v-card-title>
          {{ $t('No payment history found') }}
        </v-card-title>
      </v-card>
    </div>
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
            permitStore.getPermitDetail.application.applicationType
          "
          :order-id="permitStore.getPermitDetail.application.orderId"
        />
      </section>
    </vue-html2pdf>
  </v-container>
</template>

<script lang="ts" setup>
import { usePermitsStore } from "@core-admin/stores/permitsStore";
import Receipt from "@core-admin/components/receipt/Receipt.vue";
import VueHtml2pdf from "vue-html2pdf";
import { reactive, ref } from "vue";

const permitStore = usePermitsStore();
const html2Pdf = ref(null);

const state = reactive({
  paymentType: '',
  total: '',
  date: '',
});

function reprintReceipt(item) {
  state.date = new Date(item.paymentDateTimeUtc).toLocaleString();
  state.paymentType = item.comments;
  state.total = item.amount;
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  html2Pdf.value.generatePdf();
}
</script>

<style lang="scss" scoped>
.payment-history-container {
  overflow-y: auto;
  max-height: 85vh;
}
</style>
