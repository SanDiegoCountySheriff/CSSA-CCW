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
              label="Name"
              v-model="state.name"
              outlined
            >
            </v-text-field>
          </v-col>

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
            <v-text-field
              dense
              readonly
              label="Application Type"
              :value="
                capitalize(
                  permitStore.getPermitDetail.application.applicationType
                )
              "
              outlined
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-select
              dense
              :items="paymentOptions"
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
              label=" Vendor Information "
              v-model="state.vendorInfo"
              outlined
            >
            </v-text-field>
          </v-col>

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
import { capitalize } from '@shared-utils/formatters/defaultFormatters'
import { reactive } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

interface PaymentHistoryProps {
  loading: boolean
}

const props = defineProps<PaymentHistoryProps>()

const permitStore = usePermitsStore()
const authStore = useAuthStore()

const paymentOptions = ['Initial', 'Final', 'Refund']

const state = reactive({
  name: `${permitStore.getPermitDetail.application.personalInfo.lastName}, ${permitStore.getPermitDetail.application.personalInfo.firstName}`,
  applicationType: permitStore.getPermitDetail.application.applicationType,
  paymentType: '',
  vendorInfo: '',
  total: '',
  transactionId: '',
  snackbar: false,
})

const currentDate = new Date(Date.now())

function submitAndPrint() {
  const body: PaymentHistoryType = {
    amount: state.total,
    paymentDateTimeUtc: currentDate.toISOString(),
    recordedBy: authStore.getAuthState.userName,
    paymentType: state.paymentType,
    transactionId: state.transactionId,
    vendorInfo: state.vendorInfo,
    successful: true,
    paymentStatus: 1,
  }

  permitStore.permitDetail.paymentHistory.push(body)

  permitStore.updatePermitDetailApi('Payment History added').catch(() => {
    state.snackbar = true
  })
}
</script>
