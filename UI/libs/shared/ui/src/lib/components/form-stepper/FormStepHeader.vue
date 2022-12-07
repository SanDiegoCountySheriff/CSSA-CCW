<template>
  <v-stepper-header v-if="!props.smallSize">
    <div class="step-heading">
      <div
        class="step-section"
        v-for="(name, index) in stepNames"
        :key="index"
      >
        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex > startingStep + index"
          :step="startingStep + index"
        >
          {{ $t(`${name}`) }}
        </v-stepper-step>
        <v-divider
          :class="{ 'step-transparent': index + 1 === stepNames.length }"
        />
      </div>
    </div>
  </v-stepper-header>
</template>

<script setup lang="ts">
interface FormStepHeaderProps {
  stepIndex: number;
  startingStep: number;
  previousIndex: number;
  stepNames: Array<string>;
  smallSize?: boolean;
}

const props = withDefaults(defineProps<FormStepHeaderProps>(), {
  stepIndex: 1,
  startingStep: 1,
  stepNames: () => [],
  smallSize: false,
});
</script>

<style scoped lang="scss">
.step-container {
  width: 95%;
  display: flex;
  justify-content: center;
  align-items: center;
}
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
.step-transparent {
  border-color: transparent !important;
}
</style>
