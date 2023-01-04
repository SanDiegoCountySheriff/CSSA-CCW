<template>
  <v-container class="info-section-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Spouse Information: ') }}
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
          :label="$t('Last name')"
          :value="props.spouseInfo.lastName"
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
          class="pl-6"
          :label="$t('First name')"
          :value="props.spouseInfo.firstName"
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
          class="pl-6"
          :label="$t('Middle name')"
          :value="props.spouseInfo.middleName"
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
          class="pl-6"
          :label="$t('Maiden name')"
          :value="props.spouseInfo.maidenName"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { SpouseInfoType } from '@shared-utils/types/defaultTypes';

interface ISpouseInfoSectionProps {
  color: string;
  spouseInfo: SpouseInfoType;
}
const props = defineProps<ISpouseInfoSectionProps>();
</script>

<style lang="scss" scoped>
.info-section-container {
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
