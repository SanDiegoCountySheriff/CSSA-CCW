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
      @accept="handleAccept"
      @decline="handleDecline"
    />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementButtonContainer from '@shared-ui/components/containers/AcknowledgementButtonContainer.vue'

interface IAcknowledgmentPartOneProps {
  handleAccept: CallableFunction
  handleDecline: CallableFunction
  textBody: string
  link?: string
}
const props = defineProps<IAcknowledgmentPartOneProps>()
</script>
