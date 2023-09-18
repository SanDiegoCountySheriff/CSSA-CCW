<template>
  <div>
    <v-card-text>{{ $t(`${props.textBody}`) }}</v-card-text>
    <v-card-text>
      <a
        v-if="props.link"
        :href="props.link"
        target="_blank"
      >
        {{ $t('More Information') }}
      </a>
    </v-card-text>

    <AcknowledgementButtonContainer
      @handle-accept="onAccept"
      @handle-decline="onDecline"
      :is-loading="isLoading"
    />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementButtonContainer from '@shared-ui/components/containers/AcknowledgementButtonContainer.vue'

interface IAcknowledgmentPartOneProps {
  textBody: string
  link?: string
  isLoading?: boolean
}
const props = withDefaults(defineProps<IAcknowledgmentPartOneProps>(), {
  isLoading: false,
  link: '',
})

const emit = defineEmits(['handle-accept', 'handle-decline'])

function onAccept() {
  emit('handle-accept')
}

function onDecline() {
  emit('handle-decline')
}
</script>
