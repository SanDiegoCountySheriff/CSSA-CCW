<template>
  <v-stepper-items>
    <v-stepper-content step="1">
      <SecondFormStepOne :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="2">
      <SecondFormStepTwo :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="3">
</v-stepper-content>
  </v-stepper-items>
</template>

<script setup lang="ts">
import SecondFormStepOne from '@core-public/components/form-stepper/form-steps/SecondFormStepOne.vue';
import SecondFormStepTwo from '@core-public/components/form-stepper/form-steps/SecondFormStepTwo.vue';

interface IFormSecondStepItemsProps {
  stepIndex: number;
  handleNextSection: CallableFunction;
  handleReset: CallableFunction;
}
const props = defineProps<IFormSecondStepItemsProps>();
</script>

<style lang="scss" scoped></style>
