<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card class="payment-wrapper">
    <v-banner class="mb-5 text-xl font-weight-bold">
      {{ $t('Application / Processing Cost') }}
    </v-banner>

    <v-card-text>
      <v-row class="m-5">
        <v-col>
          <h3
            class="font-weight-bold"
            label
            color="success"
          >
            {{ $t('Application Cost') }}
          </h3>
        </v-col>
        <v-col>
          <h3>$ {{ props.payment.applicationCost }}</h3>
        </v-col>
      </v-row>
      <v-row class="m-5">
        <v-col>
          <h3>
            {{ $t('Credit Card fee') }}
          </h3>
        </v-col>
        <v-col>
          <h3>$ {{ props.payment.creditFee }}</h3>
        </v-col>
      </v-row>
      <v-row class="m-5">
        <v-col>
          <h3>
            {{ $t(' Convenience fee') }}
          </h3>
        </v-col>
        <v-col>
          <h3>$ {{ props.payment.convenienceFee }}</h3>
        </v-col>
      </v-row>

      <v-banner class="mb-5 text-xl font-weight-bold">
        {{ $t('Total cost') }}
      </v-banner>

      <v-row class="m-5">
        <v-col>
          <h3>
            {{ $t(' Total Cost') }}
          </h3>
        </v-col>
        <v-col>
          <h3>$ {{ props.payment.totalCost }}</h3>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { PaymentInfoType } from '@shared-utils/types/defaultTypes'

interface IPaymentWrapperProps {
  payment: PaymentInfoType
}
const props = defineProps<IPaymentWrapperProps>()
</script>

<style lang="scss" scoped>
.payment-wrapper {
  width: 100%;
  margin-bottom: 1rem;
}
</style>
