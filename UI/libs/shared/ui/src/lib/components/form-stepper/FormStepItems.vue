<template>
  <v-stepper-items>
    <v-stepper-content step="1">
      <FormStepOne :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="2">
      <FormStepTwo :handle-next-section="handleNextSection" />
    </v-stepper-content>

    <v-stepper-content step="3">
      <FormStepThree :handle-next-section='handleNextSection'/>
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
import FormStepTwo from '@shared-ui/components/form-stepper/form-steps/FormStepTwo.vue';
import FormStepThree from '@shared-ui/components/form-stepper/form-steps/FormStepThree.vue';

export default {
  name: 'FormStepItems',
  components: { FormStepThree, FormStepOne, FormStepTwo },
  props: {
    stepIndex: Number,
    handleNextSection: Function,
  },
};
</script>

<style lang='scss' scoped></style>
