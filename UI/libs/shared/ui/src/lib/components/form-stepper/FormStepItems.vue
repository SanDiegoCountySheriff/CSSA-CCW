<template>
  <v-stepper-items>
    <v-stepper-content step="1">
      <FormStepOne :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="2">
      <v-btn color="secondary" @click="handleNextSection"> continue </v-btn>
    </v-stepper-content>

    <v-stepper-content step="3">
      <v-btn color="secondary" @click="handleNextSection"> continue </v-btn>
    </v-stepper-content>

    <v-stepper-content step="4">
      <v-btn color="secondary" @click="handleNextSection"> continue </v-btn>
    </v-stepper-content>

    <v-stepper-content step="5">
      <v-btn color="secondary" @click="handleNextSection"> continue </v-btn>
    </v-stepper-content>

    <v-stepper-content step="6">
      <v-btn color="secondary" @click="handleNextSection"> continue </v-btn>
    </v-stepper-content>

    <v-stepper-content step="7">
      <v-btn color="secondary" @click="handleNextSection"> continue </v-btn>
    </v-stepper-content>

    <v-stepper-content step="8">
      <v-btn color="secondary" @click="handleNextSection"> continue </v-btn>
    </v-stepper-content>

    <v-stepper-content step="9">
      <v-btn color="secondary" @click="handleNextSection"> continue </v-btn>
    </v-stepper-content>
  </v-stepper-items>
</template>

<script lang="ts">
import FormStepOne from '../form-stepper/form-steps/FormStepOne';
export default {
  name: 'FormStepItems',
  components: { FormStepOne },
  props: {
    stepIndex: Number,
    handleNextSection: Function,
  },
};
</script>
