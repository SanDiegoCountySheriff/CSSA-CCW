<template>
  <v-text-field
    v-model="value"
    :label="$t(label)"
    :rules="rules"
    :type="type"
    :name="name"
    @blur="handleBlur"
  />
</template>

<script setup lang="ts">
import { ref } from 'vue';

interface TextInputProps {
  label?: string;
  name?: string;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  rules?: Array<any>;
  target?: string;
  type?: string;
}

const props = withDefaults(defineProps<TextInputProps>(), {
  label: '',
  name: '',
  rules: () => [],
  target: '',
  type: 'text',
});

const emit = defineEmits(['input']);

function handleBlur(event) {
  emit('input', event.target.value, props.target);
}

const value = ref('');
</script>
