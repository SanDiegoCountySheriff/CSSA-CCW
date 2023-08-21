<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container fluid>
    <v-card outlined>
      <v-card-title>
        {{ $t('Application / Processing Cost') }}
      </v-card-title>

      <v-simple-table>
        <template #default>
          <thead>
            <tr>
              <th class="text-left body-1">{{ $t('Details') }}</th>
              <th class="text-right body-1">{{ $t('Cost') }}</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>{{ $t('Application Cost') }}</td>
              <td class="text-right body-1">
                ${{ props.payment.applicationCost }}
              </td>
            </tr>
            <tr>
              <td>{{ $t('Credit Card Fee') }}</td>
              <td class="text-right body-1">${{ props.payment.creditFee }}</td>
            </tr>
            <tr>
              <td>{{ $t('Convenience Fee') }}</td>
              <td class="text-right body-1">
                ${{ props.payment.convenienceFee }}
              </td>
            </tr>
            <tr>
              <td class="body-1 font-weight-bold">{{ $t('Total Cost') }}</td>
              <td class="text-right body-1 font-weight-bold">
                ${{ props.payment.totalCost }}
              </td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { PaymentInfoType } from '@shared-utils/types/defaultTypes'

interface IPaymentWrapperProps {
  payment: PaymentInfoType
}
const props = defineProps<IPaymentWrapperProps>()
</script>
