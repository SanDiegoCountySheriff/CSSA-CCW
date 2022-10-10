<template>
  <v-container>
    <v-card
      v-if="step.index === 0"
      class="acknowledgement-card"
    >
      <v-card-title> </v-card-title>
      <v-card-text>
        <AcknowledgementInitial
          :handle-accept="handleAccept"
          :handle-decline="handleDecline"
        />
      </v-card-text>
    </v-card>

    <v-card
      v-if="step.index === 1"
      class="acknowledgement-card"
    >
      <v-card-title> {{ $t('acknowledgement-title-part2') }} </v-card-title>
      <v-card-text>
        <AcknowledgementPart
          :handle-accept="handleAccept"
          :handle-decline="handleDecline"
          :text-body="'acknowledgement-part1'"
        />
      </v-card-text>
    </v-card>

    <v-card
      v-if="step.index === 2"
      class="acknowledgement-card"
    >
      <v-card-title> {{ $t('acknowledgement-title-part3') }} </v-card-title>
      <v-card-text>
        <AcknowledgementPart
          :handle-accept="handleAccept"
          :handle-decline="handleDecline"
          :text-body="'acknowledgement-part2'"
        />
      </v-card-text>
    </v-card>

    <v-card
      v-if="step.index === 3"
      class="acknowledgement-card"
    >
      <v-card-title> {{ $t('acknowledgement-title-part4') }} </v-card-title>
      <v-card-text>
        <AcknowledgementPart
          :handle-accept="handleAccept"
          :handle-decline="handleDecline"
          :text-body="'acknowledgement-part3'"
        />
      </v-card-text>
    </v-card>

    <v-card
      v-if="step.index === 4"
      class="acknowledgement-card"
    >
      <v-card-title> {{ $t('acknowledgement-title-part5') }} </v-card-title>
      <v-card-text>
        <AcknowledgementPart
          :handle-accept="handleAccept"
          :handle-decline="handleDecline"
          :text-body="'acknowledgement-part4'"
        />
      </v-card-text>
    </v-card>

    <v-card
      v-if="step.index === 5"
      class="acknowledgement-card"
    >
      <v-card-title> {{ $t('acknowledgement-title-part6') }} </v-card-title>
      <v-card-text>
        <AcknowledgementPart
          :handle-accept="handleAccept"
          :handle-decline="handleDecline"
          :text-body="'acknowledgement-part5'"
        />
      </v-card-text>
    </v-card>

    <v-card
      v-if="step.index === 6"
      class="acknowledgement-card"
    >
      <v-card-title> {{ $t('acknowledgement-title-part7') }} </v-card-title>
      <v-card-text>
        <AcknowledgementPart
          :handle-accept="handleAccept"
          :handle-decline="handleDecline"
          :text-body="'acknowledgement-part6'"
        />
      </v-card-text>
    </v-card>

    <v-card
      v-if="step.index === 7"
      class="acknowledgement-card"
    >
      <v-card-title> {{ $t('acknowledgement-title-part8') }} </v-card-title>
      <v-card-text>
        <AcknowledgementPart
          :handle-accept="handleFinalAccept"
          :handle-decline="handleDecline"
          :text-body="'acknowledgement-part7'"
        />
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router/composables';
import { reactive } from 'vue';
import AcknowledgementInitial from '@shared-ui/components/acknowledgement-section/AcknowledgementInitial.vue';
import AcknowledgementPart from '@shared-ui/components/acknowledgement-section/AcknowledgementPart.vue';

const router = useRouter();
const step = reactive({
  index: 0,
});

function handleAccept() {
  step.index = step.index + 1;
}

function handleDecline() {
  step.index = 0;
}

function handleFinalAccept() {
  router.push('form');
}
</script>

<style lang="scss" scoped>
.acknowledgement {
  &-card {
    border-radius: 0.5rem;
    border: 0.75rem solid rgba(255, 255, 148, 0.57);
  }
}
</style>
