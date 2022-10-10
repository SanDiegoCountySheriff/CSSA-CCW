<template>
  <v-btn
    v-bind="$attrs"
    v-on="$listeners"
  >
    <slot />
    {{ $t(text) }}
  </v-btn>
</template>

<script setup lang="ts">
interface ButtonProps {
  text?: string;
}

const props = withDefaults(defineProps<ButtonProps>(), {
  text: 'Button',
});
</script>
