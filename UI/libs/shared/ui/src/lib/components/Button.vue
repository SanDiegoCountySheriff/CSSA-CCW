<template>
  <v-btn
    :class="{ loading: isLoading }"
    :color="props.color"
    @click="clickButton"
    @keydown="clickButton"
     v-bind="$attrs"
    v-on="$listeners"
  >
  
    <slot />
    {{ isLoading ? $t('- loading') : '' }}

    {{ $t(text) }}

  </v-btn>
</template>

<script setup lang="ts">

interface ButtonProps {
  name: string;
  isLoading: boolean;
    text?: string;
}

const props = withDefaults(defineProps<ButtonProps>(), {
  name: '',
  isLoading: true,
  color: '',
  text: 'Button',
});
</script>
