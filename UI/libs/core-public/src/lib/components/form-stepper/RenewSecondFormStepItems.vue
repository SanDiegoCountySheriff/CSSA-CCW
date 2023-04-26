<template>
  <v-stepper-items>
    <v-stepper-content step="6">
      <SecondFormStepOne
        v-if="props.stepIndex === 6"
        :handle-previous-section="handlePreviousSection"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="7">
      <RenewSecondFormStepThree
        v-if="props.stepIndex === 7"
        :handle-previous-section="handlePreviousSection"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="8">
      <SecondFormStepTwo
        v-if="props.stepIndex === 8"
        :handle-previous-section="handlePreviousSection"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="9">
      <SecondFormStepFour
        v-if="props.stepIndex === 9"
        :handle-previous-section="handlePreviousSection"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>
  </v-stepper-items>
</template>

<script setup lang="ts">
import RenewSecondFormStepThree from '@core-public/components/form-stepper/form-steps/RenewApplicationTypeStep.vue'
import SecondFormStepFour from '@core-public/components/form-stepper/form-steps/SignatureStep.vue'
import SecondFormStepOne from '@core-public/components/form-stepper/form-steps/WorkInfoStep.vue'
import SecondFormStepTwo from '@core-public/components/form-stepper/form-steps/FileUploadStep.vue'

interface IFormSecondStepItemsProps {
  handleNextSection: CallableFunction
  handlePreviousSection: CallableFunction
  stepIndex: number
}

const props = withDefaults(defineProps<IFormSecondStepItemsProps>(), {
  handleNextSection: () => null,
  stepIndex: 6,
})
</script>
