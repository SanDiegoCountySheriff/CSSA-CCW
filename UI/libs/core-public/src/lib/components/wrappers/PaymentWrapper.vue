<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container fluid>
    <v-row align="center">
      <v-col
        class="text-h5"
        cols="12"
      >
        {{ $t('Application / Processing Cost') }}
      </v-col>

      <v-col cols="6">
        {{ $t('Application Cost') }}
      </v-col>
      <v-col
        cols="6"
        class="text-right"
      >
        ${{ props.payment.applicationCost }}
      </v-col>

      <v-col cols="6">
        {{ $t('Credit Card Fee') }}
      </v-col>
      <v-col
        cols="6"
        class="text-right"
      >
        ${{ props.payment.creditFee }}
      </v-col>

      <v-col cols="6">
        {{ $t('Convenience Fee') }}
      </v-col>
      <v-col
        cols="6"
        class="text-right"
      >
        ${{ props.payment.convenienceFee }}
      </v-col>
    </v-row>

    <v-divider></v-divider>

    <v-row align="center mt-1">
      <v-col class="text-h5">
        {{ $t('Total Cost') }}
      </v-col>

      <v-col
        cols="6"
        class="text-right"
      >
        ${{ props.payment.totalCost }}
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { PaymentInfoType } from '@shared-utils/types/defaultTypes'

interface IPaymentWrapperProps {
  payment: PaymentInfoType
}
const props = defineProps<IPaymentWrapperProps>()
</script>
