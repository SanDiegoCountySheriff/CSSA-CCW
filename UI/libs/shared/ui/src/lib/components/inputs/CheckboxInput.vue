<template>
  <v-checkbox
    v-model="value.value"
    :label="$t(label)"
    @click="handleChange"
  />
</template>

<script setup lang="ts">
import { reactive } from 'vue';

interface CheckboxInputProps {
  label: string;
  target: string;
}

const props = withDefaults(defineProps<CheckboxInputProps>(), {
  label: '',
  target: '',
});

const emit = defineEmits(['input']);
const value = reactive({
  value: false,
});

function handleChange() {
  emit('input', value.value, props.target);
}
</script>
