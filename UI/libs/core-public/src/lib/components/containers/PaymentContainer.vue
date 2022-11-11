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
        <PaymentButtonContainer
          :cash-payment="handleCashPayment"
          :online-payment="handleOnlinePayment"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import PaymentButtonContainer from '@core-public/components/containers/PaymentButtonContainer.vue';
import PaymentWrapper from '@core-public/components/wrappers/PaymentWrapper.vue';
import { useBrandStore } from '@core-public/stores/brandStore';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { onMounted, reactive } from 'vue';

interface IProps {
  togglePayment: CallableFunction;
}

const brandStore = useBrandStore();
const application = useCompleteApplicationStore();

const props = defineProps<IProps>();

const state = reactive({
  payment: {
    applicationCost: 0,
    convenienceFee: 0,
    creditFee: 0,
    totalCost: 0,
  },
});

onMounted(() => {
  window.console.log(
    application.completeApplication.application.applicationType
  );

  switch (application.completeApplication.application.applicationType) {
    case 'standard':
      state.payment.applicationCost = brandStore.brand.cost.new.standard;
      break;
    case 'judicial':
      state.payment.applicationCost = brandStore.brand.cost.new.judicial;
      break;
    case 'reserve':
      state.payment.applicationCost = brandStore.brand.cost.new.reserve;
      break;
    case 'modify-standard' || 'modify-judicial' || 'modify-reserve':
      state.payment.applicationCost = brandStore.brand.cost.modify;
      break;
    case 'renew-standard':
      state.payment.applicationCost = brandStore.brand.cost.renew.standard;
      break;
    case 'renew-judicial':
      state.payment.applicationCost = brandStore.brand.cost.renew.judicial;
      break;
    case 'renew-reserve':
      state.payment.applicationCost = brandStore.brand.cost.renew.reserve;
      break;
    default:
      return;
  }

  state.payment.convenienceFee = brandStore.brand.cost.convenienceFee;
  state.payment.creditFee =
    state.payment.applicationCost * brandStore.brand.cost.creditFee;
  state.payment.totalCost =
    state.payment.applicationCost +
    state.payment.convenienceFee +
    state.payment.creditFee;
});

function handleCashPayment() {
  state.payment.creditFee = 0;
  state.payment.totalCost =
    state.payment.applicationCost + state.payment.convenienceFee;
  props.togglePayment();
}

// TODO: update with a confirmation somehow and update a payment field in the application object.
function handleOnlinePayment() {
  window.open(brandStore.brand.paymentURL, '_blank');
  props.togglePayment();
}
</script>

<style lang="scss" scoped>
.payment-container {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  border-radius: 0.5rem;
  background-color: rgba(201, 243, 99, 0.19);
}
</style>
