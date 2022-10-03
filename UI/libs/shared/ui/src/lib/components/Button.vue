<template>
  <v-btn
    v-bind="$attrs"
    v-on="$listeners"
  >
    {{ $t(text) }}
  </v-btn>
</template>
<script setup lang="ts">
export interface ButtonProps {
  text?: string;
}

const props = withDefaults(defineProps<ButtonProps>(), {
  text: 'Button',
});
</script>
