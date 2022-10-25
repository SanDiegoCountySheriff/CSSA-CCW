<template>
  <v-container class="info-section-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Employment Information: ') }}
    </v-banner>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Employment Status:') }}
          </v-chip>
          <div class="info-text">
            {{ props.employmentInfo }}
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
            {{ $t('Employer name:') }}
          </v-chip>
          <div class="info-text">
            {{ props.workInformation.employerName }}
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
            {{ $t('Employer Phone') }}
          </v-chip>
          <div class="info-text">
            {{ props.workInformation.employerPhone }}
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
            {{ $t('Address Line 1') }}
          </v-chip>
          <div class="info-text">
            {{ props.workInformation.employerAddressLine1 }}
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
            {{ $t('Address Line 2') }}
          </v-chip>
          <div class="info-text">
            {{ props.workInformation.employerAddressLine2 }}
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
            {{ $t('City') }}
          </v-chip>
          <div class="info-text">
            {{ props.workInformation.employerCity }}
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
            {{ $t('State') }}
          </v-chip>
          <div class="info-text">
            {{ props.workInformation.employerState }}
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
            {{ $t('Zip') }}
          </v-chip>
          <div class="info-text">
            {{ props.workInformation.employerZip }}
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
            {{ $t('Country') }}
          </v-chip>
          <div class="info-text">
            {{ props.workInformation.employerCountry }}
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { WorkInformationType } from '@shared-utils/types/defaultTypes';

interface IEmploymentInfoSectionProps {
  employmentInfo: string;
  workInformation: WorkInformationType;
  color: string;
}
const props = defineProps<IEmploymentInfoSectionProps>();
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
  background-color: #c3f3f3;
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
