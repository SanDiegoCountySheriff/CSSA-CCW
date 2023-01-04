<template>
  <v-container class="confirm-info-section rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Id Information: ') }}
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          class="pl-6"
          readonly
          :label="$t('Id number')"
          :value="props.idInfo.idNumber"
        />
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          class="pl-6"
          readonly
          :label="$t('Issuing State')"
          :value="props.idInfo.issuingState"
        />
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

<style lang="scss" scoped>
.confirm-info-section {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}

.info-row {
  display: flex;
  flex-direction: row;
  min-height: 1vh;
  max-height: 2vh;
}
.info-text {
  margin-left: 0.5rem;
  text-align: start;
  height: 1.8em;
  width: 50%;
  margin-bottom: 0.5rem;
  padding-left: 1rem;
  padding-bottom: 0.5rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
