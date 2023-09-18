<template>
  <v-card-actions>
    <v-btn
      @click="onDecline"
      color="error"
      text
      absolute
      bottom
    >
      {{ $t('Decline') }}
    </v-btn>
    <v-btn
      :disabled="isLoading"
      @click="onAccept"
      color="primary"
      text
      absolute
      bottom
      right
    >
      {{ $t('Accept') }}
    </v-btn>
  </v-card-actions>
</template>

<script setup lang="ts">
interface IAcknowledgmentButtonContainerProps {
  isLoading?: boolean
}
const props = withDefaults(defineProps<IAcknowledgmentButtonContainerProps>(), {
  isLoading: false,
})

const emit = defineEmits(['handle-accept', 'handle-decline'])

function onAccept() {
  emit('handle-accept')
}

function onDecline() {
  emit('handle-decline')
}
</script>
