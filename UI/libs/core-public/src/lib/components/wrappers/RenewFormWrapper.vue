<template>
  <div>
    <v-card
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
        <RenewFormStepItems
          :step-index="stepIndex.step"
          :handle-next-section="handleNextSection"
          :handle-reset="handleResetForm"
        />
      </v-stepper>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { formOneStepNames } from '@shared-utils/lists/defaultConstants';
import { reactive } from 'vue';
import FormStepHeader from '@core-public/components/form-stepper/FormStepHeader.vue';
import RenewFormStepItems from '@core-public/components/form-stepper/RenewFormStepItems.vue';

const stepIndex = reactive({
  step: 1,
});
function handleNextSection() {
  stepIndex.step = stepIndex.step + 1;
}

function handleResetForm() {
  stepIndex.step = 1;
}
</script>

<style lang="scss">
.form-card {
  height: auto;
  min-height: 45vh;
}

.dark-card {
  background: #455a64;
}
</style>
