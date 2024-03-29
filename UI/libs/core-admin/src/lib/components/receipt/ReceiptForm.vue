<template>
  <div>
    <v-card flat>
      <v-card-title>
        Add Transaction {{ currentDate.toLocaleString() }}
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              dense
              readonly
              label="Order Id"
              v-model="permitStore.getPermitDetail.application.orderId"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-select
              dense
              :items="paymentOptions"
              item-text="text"
              item-value="value"
              label="Payment Type"
              v-model="state.paymentType"
              outlined
            >
            </v-select>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              dense
              type="number"
              label="Total Amount"
              v-model="state.total"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              dense
              label="Transaction Id"
              v-model="state.transactionId"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-actions>
        <v-btn
          :disabled="loading"
          @click="submitAndPrint"
          color="primary"
        >
          <v-icon left>mdi-account-credit-card</v-icon>
          Submit
        </v-btn>
      </v-card-actions>
    </v-card>

    <v-snackbar
      v-model="state.snackbar"
      :timeout="4000"
      color="error"
      class="font-weight-bold"
    >
      {{ $t('Payment History failed to update. Please try again') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { PaymentHistoryType } from '@shared-utils/types/defaultTypes'
import { reactive } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

interface PaymentHistoryProps {
  loading: boolean
}

const props = defineProps<PaymentHistoryProps>()

const permitStore = usePermitsStore()
const authStore = useAuthStore()

const paymentOptions = [
  { text: 'CCW Application Initial Payment', value: 0 },
  { text: 'CCW Application Initial Judicial Payment', value: 1 },
  { text: 'CCW Application Initial Reserve Payment', value: 2 },
  { text: 'CCW Application Initial Employment Payment', value: 3 },
  { text: 'CCW Application Modification Payment', value: 4 },
  { text: 'CCW Application Modification Judicial Payment', value: 5 },
  { text: 'CCW Application Modification Reserve Payment', value: 6 },
  { text: 'CCW Application Modification Employment Payment', value: 7 },
  { text: 'CCW Application Renewal Payment', value: 8 },
  { text: 'CCW Application Renewal Judicial Payment', value: 9 },
  { text: 'CCW Application Renewal Reserve Payment', value: 10 },
  { text: 'CCW Application Renewal Employment Payment', value: 11 },
]

const state = reactive({
  paymentType: 0,
  total: 0,
  transactionId: '',
  snackbar: false,
})

const currentDate = new Date(Date.now())

function submitAndPrint() {
  const body: PaymentHistoryType = {
    amount: state.total.toString(),
    paymentDateTimeUtc: currentDate.toISOString(),
    recordedBy: authStore.getAuthState.userName,
    paymentType: state.paymentType,
    transactionId: state.transactionId,
    vendorInfo: 'Manually entered',
    successful: true,
    paymentStatus: 1,
    refundAmount: '0',
  }

  permitStore.permitDetail.paymentHistory.push(body)

  permitStore.updatePermitDetailApi('Payment History added').catch(() => {
    state.snackbar = true
  })
}
</script>
