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

<script lang="ts">
import FormStepHeader from '../form-stepper/FormStepHeader';
import FormStepItems from '../form-stepper/FormStepItems';
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'FormWrapper',
  components: { FormStepItems, FormStepHeader },
  data() {
    return {
      stepIndex: 1,
    };
  },
  methods: {
    handleNextSection() {
      this.stepIndex = this.stepIndex + 1;
    },
  },
});
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
