<template>
  <v-container class="info-section-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Spouse Information: ') }}
    </v-banner>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Spouse last name:') }}
          </v-chip>
          <div class="info-text">
            {{ props.spouseInfo.lastName }}
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
            {{ $t('Spouse first name:') }}
          </v-chip>
          <div class="info-text">
            {{ props.spouseInfo.firstName }}
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
            {{ $t(' Spouse middle name: ') }}
          </v-chip>
          <div class="info-text">
            {{ props.spouseInfo.middleName }}
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
            {{ $t('Spouse maiden name: ') }}
          </v-chip>
          <div class="info-text">
            {{ props.spouseInfo.maidenName }}
          </div>
        </div>
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

<style lang="scss" scoped></style>
