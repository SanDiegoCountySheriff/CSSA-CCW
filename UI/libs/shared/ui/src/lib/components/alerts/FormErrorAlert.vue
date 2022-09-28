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

<script lang="ts">
import Vue from 'vue';

/**
 * Used to display an error message on an attempted form submission.
 * @param errors takes in an array of string and will display the strings in the error message.
 */
export default Vue.extend({
  name: 'FormErrorAlert',
  props: {
    errors: {
      type: Array<string>,
      default: () => [],
    },
  },
});
</script>
