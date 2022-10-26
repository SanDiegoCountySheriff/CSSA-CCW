<template>
  <v-container class="payment-container">
    <v-row>
      <v-col>
        <PaymentWrapper
          v-if="state.payment.applicationCost"
          :payment="state.payment"
        />
      </v-col>
      <v-col>
        <PaymentButtonContainer :cash-payment="handleCashPayment" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useBrandStore } from '@core-public/stores/brandStore';
import { onMounted, reactive } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import PaymentWrapper from '@core-public/components/wrappers/PaymentWrapper.vue';
import PaymentButtonContainer from '@core-public/components/containers/PaymentButtonContainer.vue';

const brandStore = useBrandStore();
const application = useCompleteApplicationStore();

const state = reactive({
  payment: {
    applicationCost: 0,
    convenienceFee: 0,
    creditFee: 0,
    totalCost: 0,
  },
});

onMounted(() => {
  switch (application.completeApplication.applicationType) {
    case 'standard':
      state.payment.applicationCost = brandStore.brand.standardCost;
      break;
    case 'judicial':
      state.payment.applicationCost = brandStore.brand.judicialCost;
      break;
    case 'reserve':
      state.payment.applicationCost = brandStore.brand.reserveCost;
      break;
    default:
      return;
  }

  state.payment.convenienceFee = brandStore.brand.convenienceFee;
  state.payment.creditFee =
    state.payment.applicationCost * brandStore.brand.creditFee;
  state.payment.totalCost =
    state.payment.applicationCost +
    state.payment.convenienceFee +
    state.payment.creditFee;
});

function handleCashPayment() {
  state.payment.creditFee = 0;
  state.payment.totalCost =
    state.payment.applicationCost + state.payment.convenienceFee;
}
</script>

<style lang="scss" scoped>
.payment-container {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  border-radius: 0.5rem;
  background-color: #c9f36363;
}
</style>
