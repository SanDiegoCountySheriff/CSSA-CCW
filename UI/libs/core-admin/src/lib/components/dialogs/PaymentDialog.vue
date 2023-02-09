<template>
  <div>
    <v-dialog
      fullscreen
      hide-overlay
      transition="dialog-bottom-transition"
      v-model="state.dialog"
    >
      <template #activator="{ on, attrs }">
        <v-chip
          color="blue lighten-3"
          text-color="blue darken-4"
          v-bind="attrs"
          v-on="on"
        >
          {{ mostRecentPayment ? mostRecentPayment : $t('No payment history') }}
        </v-chip>
      </template>
      <v-card class="payment-container">
        <v-toolbar
          dense
        >
          <v-spacer> </v-spacer>
          <v-btn
            icon
            color="error"
            @click="state.dialog = false"
          >
            <v-icon color="error">
              mdi-close
            </v-icon>
          </v-btn>
        </v-toolbar>
        <v-row class="payment-row">
          <v-col
            cols="12"
            lg="3"
            md="3"
            class="payment-section"
          >
            <PaymentHistory />
          </v-col>
          <v-col
            cols="12"
            lg="9"
            md="9"
            class="payment-section"
          >
            <ReceiptForm />
          </v-col>
        </v-row>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts" setup>
import { computed, reactive } from "vue";
import { usePermitsStore } from "@core-admin/stores/permitsStore";
import PaymentHistory from "@core-admin/components/receipt/PaymentHistory.vue";
import ReceiptForm from "@core-admin/components/receipt/ReceiptForm.vue";

const permitStore = usePermitsStore();

const state = reactive({
  dialog: false,
});

const mostRecentPayment = computed(
  () => permitStore.getPermitDetail?.paymentHistory[-1]?.comments
);
</script>

<style lang="scss" scoped>
.payment- {
  &container {
    height: 50vh;
  }

  &section {
    height: 100%;
    border-right-color: #616161;
    border-right-width: 1px;
    border-right-style: solid;
  }

  &row {
    height: 100%;
  }
}
</style>
