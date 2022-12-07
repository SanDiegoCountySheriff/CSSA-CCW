<template>
  <v-stepper-items>
    <v-stepper-content step="1">
      <RenewFormStepOne
        v-if="props.stepIndex === 1"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="2">
      <FormStepTwo
        v-if="props.stepIndex === 2"
        :handle-previous-section="handlePreviousSection"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="3">
      <FormStepThree
        v-if="props.stepIndex === 3"
        :handle-previous-section="handlePreviousSection"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="4">
      <FormStepFour
        v-if="props.stepIndex === 4"
        :handle-previous-section="handlePreviousSection"
        :handle-next-section="handleNextSection"
      />
    </v-stepper-content>

    <v-stepper-content step="5">
      <RenewFormStepFive
        v-if="props.stepIndex === 5"
        :handle-previous-section="handlePreviousSection"
      />
    </v-stepper-content>
  </v-stepper-items>
</template>

<script setup lang="ts">
import FormStepFour from '@shared-ui/components/form-stepper/form-steps/PhysicalAppearanceStep.vue';
import FormStepThree from './form-steps/AddressInfoStep.vue';
import FormStepTwo from './form-steps/IdBirthInfoStep.vue';
import RenewFormStepFive from '@shared-ui/components/form-stepper/form-steps/RenewContactInfoStep.vue';
import RenewFormStepOne from './form-steps/RenewPersonalInfoStep.vue';

interface RenewFormStepItemsProps {
  stepIndex?: number;
  handleNextSection?: CallableFunction;
  handlePreviousSection: CallableFunction;
}

const props = withDefaults(defineProps<RenewFormStepItemsProps>(), {
  stepIndex: 1,
  handleNextSection: () => null,
});
</script>
