<template>
  <v-container>
    <v-row>
      <v-col class="text-left acknowledgement-text">
        {{ $t('acknowledgement-initial-part1') }}
      </v-col>
    </v-row>
    <v-row>
      <v-col class="text-left acknowledgement-text">
        {{ $t('acknowledgement-initial-part2') }}
      </v-col>
    </v-row>
    <v-row>
      <v-col class="text-left acknowledgement-text">
        {{ $t('acknowledgement-initial-part3') }}
      </v-col>
    </v-row>
    <v-row>
      <v-col class="text-left acknowledgement-text">
        {{ $t('acknowledgement-initial-part4') }}
      </v-col>
    </v-row>

    <v-row>
      <v-col class="text-left acknowledgement-text">
        {{ $t('acknowledgement-initial-part5') }}
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

interface IAcknowledgementInitialProps {
  handleAccept: CallableFunction;
  handleDecline: CallableFunction;
}

const props = defineProps<IAcknowledgementInitialProps>();
</script>

<style lang="scss" scoped>
.button-container {
  margin: 1.5rem;
}

.acknowledgement-text {
  color: #111111;
  font-size: 1.3rem;
}
</style>
