<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-alert
        v-if="isNothingModified"
        type="warning"
        outlined
      >
        You haven't modified anything! Please go back and modify a value to
        continue.
      </v-alert>
      <template v-else>
        <v-container>
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
            <v-btn color="primary">Make Payment</v-btn>
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
              :disabled="isSignaturePadEmpty || !isPaymentComplete"
              @click="handleSubmit"
              color="primary"
            >
              Submit
            </v-btn>
          </v-row>
        </v-container>
      </template>
    </v-form>
  </div>
</template>

<script lang="ts" setup>
import SignaturePad from 'signature_pad'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { computed, nextTick, onMounted, ref } from 'vue'

interface ModifyFinalizeProps {
  modifyingName: boolean
  modifyingAddress: boolean
  modifyingWeapons: boolean
}

const props = defineProps<ModifyFinalizeProps>()

const brandStore = useBrandStore()
const form = ref()
const valid = ref(false)
const signaturePad = ref<SignaturePad>()
const isSignaturePadEmpty = computed(() => {
  return signaturePad.value?.isEmpty()
})

const isPaymentComplete = computed(() => {
  return false
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

function handleSubmit() {}

function handleClearSignature() {
  signaturePad.value?.clear()
}
</script>
