<template>
  <v-stepper-items>
    <v-stepper-content step="1">
      <FormStepOne
        v-if="props.stepIndex === 1"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="2">
      <FormStepTwo
        v-if="props.stepIndex === 2"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="3">
      <FormStepThree
        v-if="props.stepIndex === 3"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="4">
      <FormStepFour
        v-if="props.stepIndex === 4"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="5">
      <FormStepFive v-if="props.stepIndex === 5" />
    </v-stepper-content>
  </v-stepper-items>
</template>

<script setup lang="ts">
import FormStepOne from './form-steps/FormStepOne.vue';
import FormStepTwo from './form-steps/FormStepTwo.vue';
import FormStepThree from './form-steps/FormStepThree.vue';
import FormStepFour from '@core-public/components/form-stepper/form-steps/FormStepFour.vue';
import FormStepFive from '@core-public/components/form-stepper/form-steps/FormStepFive.vue';

interface FormStepItemsProps {
  stepIndex?: number;
  handleNextSection?: CallableFunction;
  handleReset: CallableFunction;
}

const props = withDefaults(defineProps<FormStepItemsProps>(), {
  stepIndex: 1,
  handleNextSection: () => null,
  handleReset: () => null,
});
</script>
