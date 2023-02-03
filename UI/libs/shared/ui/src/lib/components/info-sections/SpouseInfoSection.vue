<template>
  <v-container class="info-section-container rounded mt-3">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Spouse Information: ') }}
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Last Name:') }}
          </strong>
          {{ props.spouseInfo.lastName }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('First Name:') }}
          </strong>
          {{ props.spouseInfo.firstName }}
        </v-banner>
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Middle Name:') }}
          </strong>
          {{ props.spouseInfo.middleName }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Maiden Name:') }}
          </strong>
          {{ props.spouseInfo.maidenName }}
        </v-banner>
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
