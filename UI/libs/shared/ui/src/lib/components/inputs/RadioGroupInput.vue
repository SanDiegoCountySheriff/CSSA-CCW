<template>
  <div class="radio-group">
    <v-radio-group
      v-if="layout === 'row'"
      row
      ref="radioGroup"
      v-model="value"
      :label="$t(label)"
      :hint="$t(hint)"
      persistent-hint
      @change="handleChange"
    >
      <v-radio
        class="radio-group text--black"
        v-for="(option, i) in options"
        :key="i"
        :label="option.label"
        :value="option.value"
        :color="option.color ? option.color : ''"
      />
    </v-radio-group>

    <v-radio-group
      v-if="layout !== 'row'"
      ref="radioGroup"
      v-model="value"
      :hint="$t(hint)"
      persistent-hint
      :label="$t(label)"
      @change="handleChange"
    >
      <v-radio
        class="radio-group"
        v-for="(option, i) in options"
        :key="i"
        :label="$t(option.label)"
        :value="option.value"
        :color="option.color ? option.color : ''"
      />
    </v-radio-group>
  </div>
</template>

<script setup lang="ts">
import { RadioOptionsType } from '@shared-utils/types/defaultTypes';
import { ref } from 'vue';

interface RadioGroupInputProps {
  options?: Array<RadioOptionsType>;
  label?: string;
  layout?: string;
  hint?: string;
}

const props = withDefaults(defineProps<RadioGroupInputProps>(), {
  options: () => [],
  label: '',
  layout: '',
  hint: '',
});

const emit = defineEmits(['input']);
const value = ref('');

function handleChange() {
  emit('input', value.value);
}
</script>

<style lang="scss">
.radio-group {
  color: #111;
}
</style>
