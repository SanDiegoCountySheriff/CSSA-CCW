<template>
  <!--eslint-disable-next-line vue-a11y/click-events-have-key-events-->
  <button
    :class="{ loading: isLoading }"
    @click="clickButton"
    @keydown="clickButton"
  >
    <slot />
    {{ isLoading ? $t('- loading') : '' }}
  </button>
</template>
<script setup lang="ts">
interface ButtonProps {
  name: string;
  isLoading: boolean;
}

const props = withDefaults(defineProps<ButtonProps>(), {
  name: '',
  isLoading: true,
});

const emit = defineEmits(['buttonClicked']);

function clickButton() {
  emit('buttonClicked', `${props.name}-button clicked`);
}
</script>
