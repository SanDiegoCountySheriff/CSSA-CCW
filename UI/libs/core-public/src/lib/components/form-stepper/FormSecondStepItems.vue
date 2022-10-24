<template>
  <v-stepper-items>
    <v-stepper-content step="1">
      <SecondFormStepOne :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="2">
      <SecondFormStepThree :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="3">
      <SecondFormStepTwo :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="4">
      <SecondFormStepFour :handle-next-section="handleNextSection" />
    </v-stepper-content>
  </v-stepper-items>
</template>

<script setup lang="ts">
import SecondFormStepOne from '@core-public/components/form-stepper/form-steps/SecondFormStepOne.vue';
import SecondFormStepTwo from '@core-public/components/form-stepper/form-steps/SecondFormStepTwo.vue';
import SecondFormStepThree from '@core-public/components/form-stepper/form-steps/SecondFormStepThree.vue';
import SecondFormStepFour from '@core-public/components/form-stepper/form-steps/SecondFormStepFour.vue';

interface IFormSecondStepItemsProps {
  handleNextSection: CallableFunction;
}
const props = withDefaults(defineProps<IFormSecondStepItemsProps>(), {
  handleNextSection: () => null,
});
</script>

<style lang="scss" scoped></style>
