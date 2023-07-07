<template>
  <v-card-text>
    <v-btn
      class="mr-2"
      color="primary"
      @click="handleSubmit"
      :disabled="!props.valid || props.loading"
      style="width: 200px"
    >
      <v-progress-circular
        v-if="props.loading"
        indeterminate
        size="24"
        color="white"
      />
      <span v-else>{{ $t('Continue') }}</span>
    </v-btn>
    <v-btn
      color="primary"
      @click="handleSave"
      :disabled="!props.valid || props.loading"
      style="width: 200px"
    >
      {{ $t('Save and Exit') }}
    </v-btn>
  </v-card-text>
</template>

<script setup lang="ts">
interface FormButtonContainerProps {
  valid?: boolean
  loading?: boolean
}

const props = withDefaults(defineProps<FormButtonContainerProps>(), {
  valid: false,
  loading: false,
})

const emit = defineEmits(['submit', 'save'])

function handleSubmit() {
  emit('submit')
}

function handleSave() {
  emit('save')
}
</script>
