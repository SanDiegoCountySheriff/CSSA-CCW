<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div class="payment-wrapper">
    <v-banner class="mb-5 text-xl font-weight-bold">
      {{ $t('Application / Processing Cost') }}
    </v-banner>

    <v-card-text>
      <div class="payment-info-row">
        <div class="payment-info-label">
          <h3
            class="font-weight-bold"
            label
            color="success"
          >
            {{ $t('Application Cost') }}
          </h3>
        </div>
        <div class="payment-inf-value">
          <h3>${{ props.payment.applicationCost }}</h3>
        </div>
      </div>

      <div class="payment-info-row">
        <div class="payment-info-label">
          <h3>{{ $t('Credit Card Fee') }}</h3>
        </div>
        <div class="payment-info-value">
          <h3>${{ props.payment.creditFee }}</h3>
        </div>
      </div>

      <div class="payment-info-row">
        <div class="payment-info-label">
          <h3>{{ $t('Convenience Fee') }}</h3>
        </div>
        <div class="payment-info-value">
          <h3>${{ props.payment.convenienceFee }}</h3>
        </div>
      </div>

      <v-divider class="my-4"></v-divider>

      <div class="payment-info-row">
        <div class="payment-info-label">
          <h3>{{ $t('Total Cost') }}</h3>
        </div>
        <div class="payment-info-value">
          <h3>${{ props.payment.totalCost }}</h3>
        </div>
      </div>
    </v-card-text>
  </div>
</template>

<script setup lang="ts">
import { PaymentInfoType } from '@shared-utils/types/defaultTypes'

interface IPaymentWrapperProps {
  payment: PaymentInfoType
}
const props = defineProps<IPaymentWrapperProps>()
</script>

<style scoped>
.payment-wrapper {
  margin: 0 auto;
  max-width: 500px;
  padding: 20px;
}

.payment-info-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.payment-info-label {
  flex: 1;
}

.payment-info-value {
  flex: 1;
  text-align: right;
}
</style>
