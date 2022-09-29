<template>
  <v-stepper-header>
    <v-stepper-step
      :complete="stepIndex > 1"
      step="1"
    >
      {{ $t('Personal') }}
    </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 2"
      step="2"
    >
      {{ $t('Citizenship') }}
    </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 3"
      step="3"
    >
      {{ $t('Address') }}
    </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 4"
      step="4"
    >
      {{ $t('Appearance') }}
    </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 5"
      step="5"
    >
      {{ $t('Contact') }}
    </v-stepper-step>
    <v-divider />

    <v-stepper-step
      :complete="stepIndex > 6"
      step="6"
    >
      {{ $t('Confirmation') }}
    </v-stepper-step>
  </v-stepper-header>
</template>

<script setup lang="ts">
interface FormStepHeaderProps {
  stepIndex?: number;
}

const props = withDefaults(defineProps<FormStepHeaderProps>(), {
  stepIndex: 1,
});
</script>
