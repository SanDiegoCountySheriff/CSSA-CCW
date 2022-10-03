<template>
  <v-container class="confirm-info-section rounded">
    <v-card
      :color="props.color"
      class="rounded"
    >
      <v-card-title class="ml-3">
        {{ $t('Id Information') }}
      </v-card-title>
      <v-card-text class="m-5">
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Id number: ') }}</h3>
              <p class="info-text">
                {{ props.idInfo.idNumber }}
              </p>
            </div>
          </v-col>

          <v-col>
            <div class="info-row">
              <h3>{{ $t(' Issuing State: ') }}</h3>
              <p class="info-text">
                {{ props.idInfo.issuingState }}
              </p>
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { IdType } from '@shared-ui/types/defaultTypes';

interface IIdInfoSectionProps {
  idInfo: IdType;
  color: string;
}

const props = defineProps<IIdInfoSectionProps>();
</script>

<style lang="scss">
.confirm-info-section {
  width: 100%;
  padding: 0;
  margin: 0;
}

.info-row {
  display: flex;
  flex-direction: row;
  margin-left: 0.5rem;
}
.info-text {
  margin-left: 0.5rem;
}
</style>
