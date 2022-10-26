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
        <FormStepItems
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
import { formOneStepNames } from '@shared-utils/lists/defaultConstants';
import { reactive } from 'vue';

const stepIndex = reactive({
  step: 1,
});

function handleNextSection() {
  stepIndex.step += 1;
}

function handleResetForm() {
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
