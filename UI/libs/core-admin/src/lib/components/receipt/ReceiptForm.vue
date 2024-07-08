<template>
  <div>
    <v-card flat>
      <v-card-title>
        Add Transaction {{ currentDate.toLocaleString() }}
      </v-card-title>

      <v-card-text>
        <v-form v-model="valid">
          <v-row>
            <v-col>
              <v-text-field
                v-model="permitStore.getPermitDetail.application.orderId"
                :rules="[v => !!v || 'Order ID is required']"
                label="Order Id"
                readonly
                outlined
                dense
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-select
                v-model="paymentType"
                :items="paymentOptions"
                :rules="[v => !!v || 'Payment type is required.']"
                item-text="text"
                item-value="value"
                label="Payment Type"
                outlined
                dense
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-text-field
                v-model="total"
                :rules="[v => !!v || 'Total amount is required']"
                type="number"
                label="Total Amount"
                outlined
                dense
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-text-field
                v-model="transactionId"
                :rules="[v => !!v || 'Transaction ID is required.']"
                label="Transaction Id"
                outlined
                dense
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-text-field
                v-model="vendorInfo"
                :rules="[v => !!v || 'Vendor Information is required.']"
                label="Vendor Information"
                hint="Type in Vendor name or 'cash' for cash payments"
                persistent-hint
                outlined
                dense
              />
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-btn
          :disabled="loading || !valid"
          @click="submitAndPrint"
          color="primary"
        >
          <v-icon left>mdi-account-credit-card</v-icon>
          Submit
        </v-btn>
      </v-card-actions>
    </v-card>

    <v-snackbar
      v-model="snackbar"
      :timeout="4000"
      color="error"
      class="font-weight-bold"
    >
      {{ $t('Payment History failed to update. Please try again') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationType,
  PaymentHistoryType,
} from '@shared-utils/types/defaultTypes'

interface PaymentHistoryProps {
  loading: boolean
}

const props = defineProps<PaymentHistoryProps>()

const permitStore = usePermitsStore()
const authStore = useAuthStore()
const valid = ref(false)
const paymentType = ref(null)
const total = ref(0)
const snackbar = ref(false)
const transactionId = ref('')
const vendorInfo = ref('')

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
  { text: 'CCW Application Issuance Payment', value: 12 },
  { text: 'CCW Application Standard Livescan Payment', value: 13 },
  { text: 'CCW Application Judicial Livescan Payment', value: 14 },
  { text: 'CCW Application Reserve Livescan Payment', value: 15 },
  { text: 'CCW Application Employment Livescan Payment', value: 16 },
]

const currentDate = new Date(Date.now())

function submitAndPrint() {
  const body: PaymentHistoryType = {
    amount: total.value.toString(),
    paymentDateTimeUtc: currentDate.toISOString(),
    recordedBy: authStore.getAuthState.userName,
    paymentType: paymentType.value || 0,
    transactionId: transactionId.value,
    vendorInfo: vendorInfo.value,
    successful: true,
    paymentStatus: 1,
    refundAmount: '0',
    verified: true,
    modificationNumber: getModificationNumber(),
  }

  permitStore.permitDetail.paymentHistory.push(body)

  if (permitStore.permitDetail.application.readyForInitialPayment) {
    permitStore.permitDetail.application.readyForInitialPayment = false
  }

  if (permitStore.permitDetail.application.readyForRenewalPayment) {
    permitStore.permitDetail.application.readyForRenewalPayment = false
  }

  if (permitStore.permitDetail.application.readyForModificationPayment) {
    permitStore.permitDetail.application.readyForModificationPayment = false
  }

  permitStore.updatePermitDetailApi('Payment History added').catch(() => {
    snackbar.value = true
  })
}

function getModificationNumber() {
  if (
    permitStore.permitDetail.application.applicationType ===
      ApplicationType['Modify Standard'] ||
    permitStore.permitDetail.application.applicationType ===
      ApplicationType['Modify Judicial'] ||
    permitStore.permitDetail.application.applicationType ===
      ApplicationType['Modify Reserve'] ||
    permitStore.permitDetail.application.applicationType ===
      ApplicationType['Modify Employment']
  ) {
    return permitStore.permitDetail.application.modificationNumber
  }

  return null
}
</script>
