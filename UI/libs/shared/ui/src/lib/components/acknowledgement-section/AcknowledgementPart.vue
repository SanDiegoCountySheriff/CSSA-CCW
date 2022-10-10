<template>
  <v-container>
    <v-row>
      <v-col class="text-left acknowledgement-text">
        {{ $t(`${props.textBody}`) }}
      </v-col>
    </v-row>
    <div class="button-container">
      <AcknowledgementButtonContainer
        @accept="handleAccept"
        @decline="handleDecline"
      />
    </div>
  </v-container>
</template>

<script setup lang="ts">
import AcknowledgementButtonContainer from '@shared-ui/components/containers/AcknowledgementButtonContainer.vue';

interface IAcknowledgmentPartOneProps {
  handleAccept: CallableFunction;
  handleDecline: CallableFunction;
  textBody: string;
}
const props = defineProps<IAcknowledgmentPartOneProps>();
</script>

<style lang="scss">
.button-container {
  margin: 1.5rem;
}
.acknowledgement-text {
  color: #111111;
  font-size: 1.3rem;
}
</style>
