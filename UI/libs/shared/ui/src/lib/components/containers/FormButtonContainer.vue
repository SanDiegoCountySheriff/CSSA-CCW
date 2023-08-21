<template>
  <v-card-actions>
    <v-btn
      :disabled="!props.valid || props.loading || !props.allStepsComplete"
      :loading="props.loading"
      @click="handleSubmit"
      color="primary"
    >
      {{ $t('Continue') }}
    </v-btn>
    <v-btn
      color="primary"
      @click="handleSave"
      :disabled="props.loading"
      :loading="props.loading"
    >
      {{ $t('Save and Exit') }}
    </v-btn>
  </v-card-actions>
</template>

<script setup lang="ts">
interface FormButtonContainerProps {
  valid?: boolean
  loading?: boolean
  allStepsComplete?: boolean
}

const props = withDefaults(defineProps<FormButtonContainerProps>(), {
  valid: false,
  loading: false,
  allStepsComplete: true,
})

const emit = defineEmits(['submit', 'save'])

function handleSubmit() {
  emit('submit')
}

function handleSave() {
  emit('save')
}
</script>
