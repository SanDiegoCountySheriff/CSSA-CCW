<template>
  <v-stepper-header>
    <v-stepper-step
      :complete="stepIndex > 1"
      step="1"
    > Personal </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 2"
      step="2"
    > Citizenship </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 3"
      step="3"
    > Appearance </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 4"
      step="4"
    > User </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 5"
      step="5"
    > Address </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 6"
      step="6"
    > Employment </v-stepper-step>
  </v-stepper-header>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'FormStepHeader',
  props: {
    stepIndex: Number,
  },
});
</script>

<style lang='scss' scoped></style>
