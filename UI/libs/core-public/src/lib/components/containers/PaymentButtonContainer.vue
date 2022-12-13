<template>
  <v-container>
    <v-row>
      <v-col>
        <v-btn
          class="payment-button"
          :color="$vuetify.theme.dark ? 'accent' : 'primary'"
          @click="handleCashPayment"
        >
          {{ $t('Pay in person') }}
        </v-btn>
      </v-col>
    </v-row>
    <v-row v-if="brand.paymentURL">
      <v-col>
        <v-btn
          class="payment-button"
          color="success"
          @click="onlinePayment"
        >
          {{ $t('Pay Online ') }}
        </v-btn>
      </v-col>
    </v-row>
    <v-row
      v-if="state.showInfo"
      class="mt-5"
    >
      <v-alert
        border="left"
        type="info"
        elevation="2"
        class="font-weight-bold"
      >
        {{
          $t(
            'Remember to bring the updated total to your appointment to pay for the application'
          )
        }}
      </v-alert>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useBrandStore } from '@shared-ui/stores/brandStore';

interface IPaymentButtonContainerProps {
  cashPayment: CallableFunction;
  onlinePayment: CallableFunction;
}

const { brand } = useBrandStore();
const props = defineProps<IPaymentButtonContainerProps>();
const state = reactive({
  showInfo: false,
});

function handleCashPayment() {
  props.cashPayment();
  state.showInfo = true;
}
</script>

<style lang="scss" scoped>
.payment-button {
  min-width: 50% !important;
}
</style>
