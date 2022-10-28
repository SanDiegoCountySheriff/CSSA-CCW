<template>
  <v-container class="DOB-info-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Birth Information: ') }}
    </v-banner>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Birth Date') }}
          </v-chip>
          <div class="info-text">
            {{ props.DOBInfo.birthDate }}
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
            {{ $t('Birth City') }}
          </v-chip>
          <div class="info-text">
            {{ props.DOBInfo.birthCity }}
          </div>
        </div>
      </v-col>
    </v-row>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Birth State') }}
          </v-chip>
          <div class="info-text">
            {{ props.DOBInfo.birthState }}
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
            {{ $t('Birth Country') }}
          </v-chip>
          <div class="info-text">
            {{ props.DOBInfo.birthCountry }}
          </div>
        </div>
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
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
