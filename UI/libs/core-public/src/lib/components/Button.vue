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
<script lang="ts">
export default {
  name: 'MyButton',
  props: {
    name: { type: String, required: true },
    isLoading: { type: Boolean, default: false },
  },
  methods: {
    clickButton() {
      this.$emit('buttonClicked', `${this.name}-button clicked`);
    },
  },
};
</script>
