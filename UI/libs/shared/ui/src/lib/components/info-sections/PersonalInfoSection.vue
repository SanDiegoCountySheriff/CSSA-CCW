<template>
  <v-container class="info-section-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Personal Information: ') }}
    </v-banner>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Last name:') }}
          </v-chip>
          <div class="info-text">
            {{ props.personalInfo.lastName }}
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
            {{ $t('First name:') }}
          </v-chip>
          <div class="info-text">
            {{ props.personalInfo.firstName }}
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
            {{ $t('Middle name: ') }}
          </v-chip>
          <div class="info-text">
            {{ props.personalInfo.middleName }}
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
            {{ $t('No middle name: ') }}
          </v-chip>
          <div class="info-text">
            {{ props.personalInfo.noMiddleName }}
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
            {{ $t('Maiden name: ') }}
          </v-chip>
          <div class="info-text">
            {{ props.personalInfo.maidenName }}
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
            {{ $t('Suffix name: ') }}
          </v-chip>
          <div class="info-text">
            {{ props.personalInfo.suffix }}
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
            {{ $t('Social Security number: ') }}
          </v-chip>
          <div class="info-text">
            {{ props.personalInfo.ssn }}
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
            {{ $t('Marital status: ') }}
          </v-chip>
          <div class="info-text">
            {{ props.personalInfo.maritalStatus }}
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
/**
 * This Should be able to be used in both the public and the admin.
 * to change the size put this into a container in the parent component and change
 * the size of the container
 * Also the bg color can be changed in the props.
 */
import { PersonalInfoType } from '@shared-utils/types/defaultTypes';

interface personalInfoSectionProps {
  personalInfo: PersonalInfoType;
  color: string;
}
const props = defineProps<personalInfoSectionProps>();
</script>

<style lang="scss">
.info-section-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
.info-row {
  display: flex;
  flex-direction: row;
  max-height: 2em;
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
