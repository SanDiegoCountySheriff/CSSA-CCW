<template>
  <v-alert
    class="mt-5"
    type="error"
    elevation="3"
    dismissible
  >
    {{ $t(' Can not continue do to errors in the following fields') }}
    <span
      v-for="(error, i) in errors"
      :key="i"
    >
      {{ error }}
    </span>
  </v-alert>
</template>

<script setup lang="ts">
/**
 * Used to display an error message on an attempted form submission.
 * @param errors takes in an array of string and will display the strings in the error message.
 */

interface FormErrorAlertProps {
  errors?: Array<string>;
}

const props = withDefaults(defineProps<FormErrorAlertProps>(), {
  errors: () => [],
});
</script>
