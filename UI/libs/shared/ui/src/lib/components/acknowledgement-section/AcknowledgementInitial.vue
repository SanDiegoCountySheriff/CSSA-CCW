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
      @accept="handleAccept"
      @decline="handleDecline"
    />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementButtonContainer from '@shared-ui/components/containers/AcknowledgementButtonContainer.vue'

interface IAcknowledgementInitialProps {
  handleAccept: CallableFunction
  handleDecline: CallableFunction
}

const props = defineProps<IAcknowledgementInitialProps>()
</script>
