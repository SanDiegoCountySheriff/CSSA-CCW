<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-container>
        <v-row justify="center">
          <v-alert
            v-if="paymentComplete"
            :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
            color="primary"
            type="info"
            outlined
          >
            Modification Payment is complete.
          </v-alert>
        </v-row>
      </v-container>

      <v-alert
        v-if="isNothingModified"
        type="warning"
        outlined
      >
        You haven't modified anything! Please go back and modify a value to
        continue.
      </v-alert>

      <template>
        <v-container v-if="!paymentComplete">
          <v-row justify="center">
            <v-card-title>
              Modification Payment of ${{ brandStore.brand.cost.modify }} is
              required
            </v-card-title>
          </v-row>

          <v-row
            v-if="brandStore.brand.cost.convenienceFee > 0"
            justify="center"
            class="text-center"
          >
            <v-card-text>
              In order to pay online with a credit card a convenience fee of
              {{ brandStore.brand.cost.convenienceFee }}% will be added to the
              transaction.
            </v-card-text>
          </v-row>

          <v-row justify="center">
            <v-btn
              color="primary"
              :loading="isLoading"
              :disabled="isNothingModified"
              @click="makePayment"
            >
              Pay Now
            </v-btn>
          </v-row>
        </v-container>

        <v-container>
          <v-row justify="center">
            <v-card-title>
              {{ $t('Please Sign Here') }}
            </v-card-title>

            <v-col
              cols="12"
              class="d-flex align-center justify-center"
            >
              <v-card
                light
                flat
                :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
                height="100px"
                outlined
                style="border: solid 2px black"
              >
                <canvas
                  :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
                  height="100px"
                  id="signature"
                  class="signature"
                ></canvas>
              </v-card>
            </v-col>

            <v-col
              cols="12"
              class="text-center"
            >
              <v-btn
                color="primary"
                text
                @click="handleClearSignature"
              >
                {{ $t('Clear Signature') }}
              </v-btn>
            </v-col>
          </v-row>

          <v-row justify="center">
            <v-btn
              :disabled="
                isSignaturePadEmpty || !paymentComplete || isNothingModified
              "
              @click="handleSubmit"
              color="primary"
            >
              Submit
            </v-btn>
          </v-row>
        </v-container>
      </template>
    </v-form>

    <v-snackbar
      v-model="paymentSnackbar"
      :timeout="-1"
      color="primary"
      persistent
    >
      {{ $t('There was a problem processing the payment, please try again.') }}
      <v-btn
        @click="paymentSnackbar = !paymentSnackbar"
        icon
      >
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-snackbar>
  </div>
</template>

<script lang="ts" setup>
import { PaymentType } from '@shared-utils/types/defaultTypes'
import SignaturePad from 'signature_pad'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'

import { computed, nextTick, onMounted, ref } from 'vue'

interface ModifyFinalizeProps {
  modifyingName: boolean
  modifyingAddress: boolean
  modifyingWeapons: boolean
  paymentComplete: boolean
}

const props = withDefaults(defineProps<ModifyFinalizeProps>(), {
  modifyingName: true,
  modifyingAddress: true,
  modifyingWeapons: true,
})

const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const brandStore = useBrandStore()
const form = ref()
const valid = ref(false)
const paymentSnackbar = ref(false)
const signaturePad = ref<SignaturePad>()
const isSignaturePadEmpty = computed(() => {
  return signaturePad.value?.isEmpty()
})

const isNothingModified = computed(() => {
  return (
    !props.modifyingName && !props.modifyingAddress && !props.modifyingWeapons
  )
})

onMounted(() => {
  nextTick(() => {
    const canvas = document.getElementById('signature') as HTMLCanvasElement

    signaturePad.value = new SignaturePad(canvas, {
      backgroundColor: 'rgba(255, 255, 255, 0)',
    })
  })
})

const { mutate: makePayment, isLoading } = useMutation({
  mutationFn: () => {
    const cost = brandStore.brand.cost.modify
    let paymentType: string

    switch (applicationStore.completeApplication.application.applicationType) {
      case 'standard':
        paymentType =
          PaymentType['CCW Application Modification Payment'].toString()
        break
      case 'judicial':
        paymentType =
          PaymentType[
            'CCW Application Modification Judicial Payment'
          ].toString()
        break
      case 'reserve':
        paymentType =
          PaymentType['CCW Application Modification Reserve Payment'].toString()
        break
      default:
        paymentType =
          PaymentType['CCW Application Modification Payment'].toString()
    }

    return paymentStore.getPayment(
      applicationStore.completeApplication.id,
      cost,
      applicationStore.completeApplication.application.orderId,
      paymentType
    )
  },
  onError: () => {
    paymentSnackbar.value = true
  },
})

function handleSubmit() {
  // update signature
  // set status
  // redirect to submission confirmation
}

function handleClearSignature() {
  signaturePad.value?.clear()
}
</script>
