<template>
  <v-stepper-items>
    <v-stepper-content step="1">
      <RenewFormStepOne :handle-next-section="handleNextSection" />
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
      <FormStepFive />
    </v-stepper-content>
  </v-stepper-items>
</template>

<script setup lang="ts">
import RenewFormStepOne from './form-steps/RenewFormStepOne.vue';
import FormStepTwo from './form-steps/FormStepTwo.vue';
import FormStepThree from './form-steps/FormStepThree.vue';
import FormStepFour from '@core-public/components/form-stepper/form-steps/FormStepFour.vue';
import FormStepFive from '@core-public/components/form-stepper/form-steps/FormStepFive.vue';

interface RenewFormStepItemsProps {
  stepIndex?: number;
  handleNextSection?: CallableFunction;
  handleReset: CallableFunction;
}

const props = withDefaults(defineProps<RenewFormStepItemsProps>(), {
  stepIndex: 1,
  handleNextSection: () => null,
  handleReset: () => null,
});
</script>

<style lang="scss" scoped></style>
