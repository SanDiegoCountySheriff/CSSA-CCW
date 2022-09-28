<template>
  <v-checkbox
    v-model="value"
    :label="$t(label)"
    @change="handleChange"
  />
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'CheckboxInput',
  data: () => ({
    value: false,
  }),
  props: {
    label: {
      type: String,
      default: '',
    },
    target: {
      type: String,
      default: '',
    },
  },
  methods: {
    handleChange() {
      this.$emit('input', `${this.value}`, this.target);
    },
  },
});
</script>
