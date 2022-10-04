<template>
  <v-btn
    :class="{ loading: isLoading }"
    :color="props.color"
    @click="clickButton"
    @keydown="clickButton"
  >
    <slot />
    {{ isLoading ? $t('- loading') : '' }}
  </v-btn>
</template>

<script setup lang="ts">
interface ButtonProps {
  name: string;
  isLoading: boolean;
}

const props = withDefaults(defineProps<ButtonProps>(), {
  name: '',
  isLoading: true,
  color: '',
});

const emit = defineEmits(['buttonClicked']);

function clickButton() {
  emit('buttonClicked', `${props.name}-button clicked`);
}
</script>
