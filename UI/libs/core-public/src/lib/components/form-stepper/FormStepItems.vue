<template>
  <v-stepper-items>
    <v-stepper-content step="1">
      <FormStepOne :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="2">
      <FormStepTwo :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="3">
      <FormStepThree :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="4">
      <FormStepFour :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="5">
      <FormStepFive :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="6">
      <FormStepSix
        v-if="props.stepIndex === 6"
        :reset-form="handleReset"
      />
    </v-stepper-content>
  </v-stepper-items>
</template>

<script setup lang="ts">
import FormStepOne from './form-steps/FormStepOne.vue';
import FormStepTwo from './form-steps/FormStepTwo.vue';
import FormStepThree from './form-steps/FormStepThree.vue';
import FormStepFour from '@core-public/components/form-stepper/form-steps/FormStepFour.vue';
import FormStepFive from '@core-public/components/form-stepper/form-steps/FormStepFive.vue';
import FormStepSix from '@core-public/components/form-stepper/form-steps/FormStepSix.vue';

interface FormStepItemsProps {
  stepIndex?: number;
  handleNextSection?: () => void;
  handleReset: CallableFunction;
}

const props = withDefaults(defineProps<FormStepItemsProps>(), {
  stepIndex: 1,
  handleNextSection: () => null,
  handleReset: () => null,
});
</script>
