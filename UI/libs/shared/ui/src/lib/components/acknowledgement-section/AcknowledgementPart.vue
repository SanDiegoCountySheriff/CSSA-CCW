<template>
  <v-container>
    <v-row>
      <v-col class="text-left acknowledgement-text">
        {{ $t(`${props.textBody}`) }}
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <a
          v-if="props.link"
          :href="props.link"
          target="_blank"
        >
          {{ $t('More Information') }}
        </a>
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
  link?: string;
}
const props = defineProps<IAcknowledgmentPartOneProps>();
</script>

<style lang="scss">
.button-container {
  margin: 1.5rem;
}

.acknowledgement-text {
  font-size: 1.3rem;
}
</style>
