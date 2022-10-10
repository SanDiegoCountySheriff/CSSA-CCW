<template>
  <v-stepper-header>
    <div class="step-heading">
      <div
        class="step-section"
        v-for="(name, index) in stepNames"
        :key="index"
      >
        <v-stepper-step
          :complete="stepIndex > index + 1"
          :step="index + 1"
        >
          {{ $t(`${name}`) }}
        </v-stepper-step>
        <v-divider v-if="index + 1 < stepNames.length" />
      </div>
    </div>
  </v-stepper-header>
</template>

<script setup lang="ts">
interface FormStepHeaderProps {
  stepIndex?: number;
  stepNames: Array<string>;
}

const props = withDefaults(defineProps<FormStepHeaderProps>(), {
  stepIndex: 1,
  stepNames: () => [],
});

console.log(props.stepNames);
</script>

<style scoped lang="scss">
.step-heading {
  display: flex;
  flex-direction: row;
  justify-content: center;
  width: 100%;
  padding-left: 4%;
}
.step-section {
  display: flex;
  flex-direction: row;
  justify-content: center;
  width: 100%;
}
</style>
