<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container class="brand-form-container">
    <v-snackbar
      :value="snackbar"
      :timeout="2000"
      app
      absolute
      bottom
      color="primary"
      left
      text
    >
      {{ $t('Updated settings') }} <strong>{{ $t('successfully.') }}</strong>
    </v-snackbar>
    <v-container
      v-if="isLoading && !isError"
      fluid
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
              divider, list-item-three-line,
              card-heading, image, image, image,
              image, actions"
      >
      </v-skeleton-loader>
    </v-container>
    <v-stepper
      v-else
      v-model="stepIndex"
      class="elevation-0 pb-0"
      vertical
    >
      <v-stepper-step
        :complete="stepIndex > 1"
        editable
        step="1"
      >
        Agency
      </v-stepper-step>

      <v-stepper-content step="1">
        <AgencyFormStep
          :handle-next-step="handleNextStep"
          :handle-back-step="handleBackStep"
          :handle-reset-step="handleResetStep"
        />
      </v-stepper-content>

      <v-stepper-step
        :complete="stepIndex > 2"
        editable
        step="2"
      >
        Assets
      </v-stepper-step>

      <v-stepper-content step="2">
        <AssetsFormStep
          :handle-next-step="handleNextStep"
          :handle-back-step="handleBackStep"
          :handle-reset-step="handleResetStep"
        />
      </v-stepper-content>

      <v-stepper-step
        :complete="stepIndex > 3"
        editable
        step="3"
      >
        Color Scheme
      </v-stepper-step>

      <v-stepper-content step="3">
        <ColorSchemeFormStep
          :handle-next-step="handleNextStep"
          :handle-back-step="handleBackStep"
          :handle-reset-step="handleResetStep"
        />
      </v-stepper-content>

      <v-stepper-step
        :complete="stepIndex > 4"
        editable
        step="4"
      >
        {{ $t('Configuration') }}
      </v-stepper-step>
      <v-stepper-content step="4">
        <ConfigurationFormStep
          :handle-next-step="handleNextStep"
          :handle-back-step="handleBackStep"
          :handle-reset-step="handleResetStep"
        />
      </v-stepper-content>

      <v-stepper-step
        editable
        step="5"
      >
        {{ $t('Fees') }}
      </v-stepper-step>
      <v-stepper-content step="5">
        <FeesFormStep
          :handle-next-step="handleNextStep"
          :handle-back-step="handleBackStep"
          :handle-reset-step="handleResetStep"
        />
      </v-stepper-content>
    </v-stepper>
  </v-container>
</template>

<script setup lang="ts">
import AgencyFormStep from './steps/AgencyFormStep.vue'
import AssetsFormStep from './steps/AssetsFormStep.vue'
import ColorSchemeFormStep from './steps/ColorSchemeFormStep.vue'
import ConfigurationFormStep from './steps/ConfigurationFormStep.vue'
import FeesFormStep from './steps/FeesFormStep.vue'
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'

const stepIndex = ref(1)
const snackbar = ref(false)

const brandStore = useBrandStore()
const { isLoading, isError } = useQuery(
  ['brandSetting'],
  brandStore.getBrandSettingApi
)

function handleNextStep() {
  snackbar.value = true
  stepIndex.value++
}

function handleBackStep() {
  stepIndex.value--
}

function handleResetStep() {
  stepIndex.value = 0
}
</script>

<style lang="scss" scoped>
.sub-header {
  font-size: 1.5rem;
}
</style>
