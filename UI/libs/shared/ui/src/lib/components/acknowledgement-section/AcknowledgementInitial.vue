<template>
  <div>
    <v-card-text>{{ $t('acknowledgement-initial-part1') }}</v-card-text>
    <v-card-text>{{ $t('acknowledgement-initial-part2') }}</v-card-text>
    <v-card-text>{{ $t('acknowledgement-initial-part3') }}</v-card-text>
    <v-card-text>{{ $t('acknowledgement-initial-part4') }}</v-card-text>
    <v-card-text class="mb-5">
      {{ $t('acknowledgement-initial-part5') }}
    </v-card-text>

    <AcknowledgementButtonContainer
      @handle-accept="onAccept"
      @handle-decline="onDecline"
    />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementButtonContainer from '@shared-ui/components/containers/AcknowledgementButtonContainer.vue'

const emit = defineEmits(['handle-accept', 'handle-decline'])

function onAccept() {
  emit('handle-accept')
}

function onDecline() {
  emit('handle-decline')
}
</script>
