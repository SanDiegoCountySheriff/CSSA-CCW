<template>
  <v-card
    class="rounded elevation-2 form-card"
    :class="{ 'dark-card': $vuetify.theme.dark }"
  >
    <v-stepper
      alt-labels
      v-model="stepIndex.step"
    >
      <FormStepHeader :step-index="stepIndex.step" />
      <FormStepItems
        :step-index="stepIndex.step"
        :handle-next-section="handleNextSection"
        :handle-reset="handleResetForm"
      />
    </v-stepper>
  </v-card>
</template>

<script setup lang="ts">
import FormStepHeader from '../form-stepper/FormStepHeader.vue';
import FormStepItems from '../form-stepper/FormStepItems.vue';
import { useFormStep } from '@core-public/stores/formStep';
import { reactive } from 'vue';

const { getFormStep, setFormStep } = useFormStep();

const stepIndex = reactive({
  step: getFormStep,
});

function handleNextSection() {
  setFormStep(stepIndex.step + 1);
  stepIndex.step = stepIndex.step + 1;
}

function handleResetForm() {
  setFormStep(1);
  stepIndex.step = 1;
}
</script>

<style lang="scss" scoped>
.form-card {
  height: auto;
  min-height: 45vh;
}

.dark-card {
  background: #455a64;
}
</style>
