<template>
  <v-stepper-items>
    <v-stepper-content step="6">
      <SecondFormStepOne
        v-if="props.stepIndex === 6"
        :handle-next-section="handleNextSection"
        :handle-previous-section="handlePreviousSection"
      />
    </v-stepper-content>

    <v-stepper-content step="7">
      <SecondFormStepThree
        v-if="props.stepIndex === 7"
        :handle-next-section="handleNextSection"
        :handle-previous-section="handlePreviousSection"
      />
    </v-stepper-content>

    <v-stepper-content step="8">
      <SecondFormStepTwo
        v-if="props.stepIndex === 8"
        :handle-next-section="handleNextSection"
        :handle-previous-section="handlePreviousSection"
      />
    </v-stepper-content>

    <v-stepper-content step="9">
      <SecondFormStepFour
        v-if="props.stepIndex === 9"
        :handle-next-section="handleNextSection"
        :handle-previous-section="handlePreviousSection"
      />
    </v-stepper-content>
  </v-stepper-items>
</template>

<script setup lang="ts">
import SecondFormStepOne from '@shared-ui/components/form-stepper/form-steps/WorkInfoStep.vue';
import SecondFormStepTwo from '@shared-ui/components/form-stepper/form-steps/FileUploadStep.vue';
import SecondFormStepThree from '@shared-ui/components/form-stepper/form-steps/ApplicationTypeStep.vue';
import SecondFormStepFour from '@shared-ui/components/form-stepper/form-steps/SignatureStep.vue';

interface IFormSecondStepItemsProps {
  stepIndex?: number;
  handleNextSection: CallableFunction;
  handlePreviousSection: CallableFunction;
}
const props = withDefaults(defineProps<IFormSecondStepItemsProps>(), {
  handleNextSection: () => null,
  stepIndex: 6,
});
</script>
