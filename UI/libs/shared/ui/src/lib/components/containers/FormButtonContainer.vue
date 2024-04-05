<template>
  <v-card-actions>
    <v-btn
      v-if="
        !props.isFinalStep ||
        (!applicationStore.completeApplication.application
          .isUpdatingApplication &&
          !props.loading)
      "
      :disabled="!props.valid || props.loading || !props.allStepsComplete"
      :loading="props.loading"
      @click="handleContinue"
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
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'

const applicationStore = useCompleteApplicationStore()

interface FormButtonContainerProps {
  valid?: boolean
  loading?: boolean
  allStepsComplete?: boolean
  isFinalStep?: boolean
}

const props = withDefaults(defineProps<FormButtonContainerProps>(), {
  valid: false,
  loading: false,
  allStepsComplete: true,
  isFinalStep: false,
})

const emit = defineEmits(['continue', 'save'])

function handleContinue() {
  emit('continue')
}

function handleSave() {
  emit('save')

  if (applicationStore.completeApplication.application.isUpdatingApplication) {
    applicationStore.completeApplication.application.isUpdatingApplication =
      false
  }
}
</script>
