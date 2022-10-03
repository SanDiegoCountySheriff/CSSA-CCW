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
import { useActions, useGetters } from 'vuex-composition-helpers';
import { reactive } from 'vue';

const { getIndex } = useGetters(['getIndex']);
const { updateIndex } = useActions(['updateIndex']);
const stepIndex = reactive(getIndex);

function handleNextSection() {
  const currentStep = stepIndex.value;
  updateIndex(currentStep + 1);
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
