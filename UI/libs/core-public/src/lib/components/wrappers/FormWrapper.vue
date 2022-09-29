<template>
  <v-card
    class="rounded elevation-2 form-card"
    :class="{ 'dark-card': $vuetify.theme.dark }"
  >
    <v-stepper
      alt-labels
      v-model="stepIndex"
    >
      <FormStepHeader :step-index="stepIndex" />
      <FormStepItems
        :step-index="stepIndex"
        :handle-next-section="handleNextSection"
      />
    </v-stepper>
  </v-card>
</template>

<script setup lang="ts">
import FormStepHeader from '../form-stepper/FormStepHeader.vue';
import FormStepItems from '../form-stepper/FormStepItems.vue';
import { ref } from 'vue';

const stepIndex = ref(5);

function handleNextSection() {
  stepIndex.value = stepIndex.value + 1;
  console.log(stepIndex.value);
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
