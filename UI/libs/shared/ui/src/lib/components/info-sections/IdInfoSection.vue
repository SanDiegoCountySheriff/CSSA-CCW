<template>
  <v-container class="confirm-info-section rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Id Information: ') }}
    </v-banner>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Id Number') }}
          </v-chip>
          <div class="info-text">
            {{ props.idInfo.idNumber }}
          </div>
        </div>
      </v-col>

      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Issuing State ') }}
          </v-chip>
          <div class="info-text">
            {{ props.idInfo.issuingState }}
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { IdType } from '@shared-utils/types/defaultTypes';

interface IIdInfoSectionProps {
  idInfo: IdType;
  color: string;
}

const props = defineProps<IIdInfoSectionProps>();
</script>

<style lang="scss">
.confirm-info-section {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}

.info-row {
  display: flex;
  flex-direction: row;
  margin-left: 0.5rem;
}
.info-text {
  margin-left: 0.5rem;
  text-align: start;
  height: 1.8em;
  width: 50%;
  margin-bottom: 0.5rem;
  padding-left: 0.5rem;
  padding-top: 0.2rem;
  background-color: #c3f3f3;
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
