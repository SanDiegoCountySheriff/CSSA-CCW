<template>
  <v-container class="payment-container">
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <PaymentWrapper
          v-if="state.payment.applicationCost"
          :payment="state.payment"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
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
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { usePaymentStore } from '@core-public/stores/paymentStore';
import { onMounted, reactive } from 'vue';

interface IProps {
  togglePayment: CallableFunction;
}

const brandStore = useBrandStore();
const application = useCompleteApplicationStore();
const paymentStore = usePaymentStore();

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
    case 'duplicate-standard' || 'duplicate-judicial' || 'duplicate-reserve':
      state.payment.applicationCost = brandStore.brand.cost.modify;
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
  paymentStore.setPaymentType('cash');
  props.togglePayment();
}

function handleOnlinePayment() {
  window.open(brandStore.brand.paymentURL, '_blank');
  paymentStore.setPaymentType('card');
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
