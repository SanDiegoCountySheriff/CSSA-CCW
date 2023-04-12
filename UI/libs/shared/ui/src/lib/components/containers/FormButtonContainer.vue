<template>
  <v-card-text>
    <v-btn
      class="mr-2"
      color="primary"
      @click="handleSubmit"
      :disabled="!props.valid"
      :loading="submitting"
    >
      {{ $t('Continue') }}
    </v-btn>
    <v-btn
      color="primary"
      @click="handleSave"
      :disabled="!props.valid"
      :loading="submitting"
    >
      {{ $t('Save and Exit') }}
    </v-btn>
  </v-card-text>
</template>

<script setup lang="ts">
interface FormButtonContainerProps {
  valid?: boolean;
  submitting?: boolean;
}

const props = withDefaults(defineProps<FormButtonContainerProps>(), {
  valid: false,
});

const emit = defineEmits(['submit', 'save', 'back', 'cancel']);

function handleSubmit() {
  emit('submit');
}

function handleSave() {
  emit('save');
}
</script>
