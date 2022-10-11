<template>
  <div>
    <v-card
      v-if="route.fullPath === '/form'"
      class="rounded elevation-2 form-card"
      :class="{ 'dark-card': $vuetify.theme.dark }"
    >
      <v-stepper
        alt-labels
        v-model="stepIndex.step"
      >
        <FormStepHeader
          :step-index="stepIndex.step"
          :step-names="formOneStepNames"
        />
        <FormStepItems
          :step-index="stepIndex.step"
          :handle-next-section="handleNextSection"
          :handle-reset="handleResetForm"
        />
      </v-stepper>
    </v-card>

    <v-card
      v-if="route.fullPath === '/form-2'"
      class="rounded elevation-2 form-card"
      :class="{ 'dark-card': $vuetify.theme.dark }"
    >
      <v-stepper
        alt-labels
        v-model="stepIndex.step"
      >
        <FormStepHeader
          :step-index="stepIndex.step"
          :step-names="formTwoStepName"
        />
        <FormSecondStepItems
          :step-index="stepIndex.step"
          :handle-next-section="handleNextSection"
          :handle-reset="handleResetForm"
        />
      </v-stepper>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import FormStepHeader from '../form-stepper/FormStepHeader.vue';
import FormStepItems from '../form-stepper/FormStepItems.vue';
import {
  formOneStepNames,
  formTwoStepName,
} from '@shared-utils/lists/defaultConstants';
import { useFormStep } from '@core-public/stores/formStep';
import { useRoute } from 'vue-router/composables';
import { reactive } from 'vue';
import FormSecondStepItems from '@core-public/components/form-stepper/FormSecondStepItems.vue';

const { getFormStep, setFormStep } = useFormStep();
const route = useRoute();

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
