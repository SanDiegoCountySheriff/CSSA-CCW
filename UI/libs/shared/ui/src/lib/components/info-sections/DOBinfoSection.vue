<template>
  <v-container class="DOB-info-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Birth Information: ') }}
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :label="$t('Birth Date')"
          :value="props.DOBInfo.birthDate"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :label="$t('Birth City')"
          :value="props.DOBInfo.birthCity"
        />
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :label="$t('Birth State')"
          :value="props.DOBInfo.birthState"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :label="$t('Birth State')"
          :value="props.DOBInfo.birthCountry"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { DOBType } from '@shared-utils/types/defaultTypes';

interface IDOBInfoSectionProps {
  // eslint-disable-next-line vue/prop-name-casing
  DOBInfo: DOBType;
  color: string;
}

const props = defineProps<IDOBInfoSectionProps>();
</script>

<style lang="scss" scoped>
.DOB-info-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}

.info-row {
  display: flex;
  flex-direction: row;
  max-height: 2vh;
  min-height: 1vh;
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
