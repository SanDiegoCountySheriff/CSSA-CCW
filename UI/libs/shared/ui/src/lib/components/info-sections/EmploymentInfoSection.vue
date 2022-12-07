<template>
  <v-container class="info-section-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Employment Information: ') }}
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
          :label="$t('Employment Status')"
          :value="props.employmentInfo"
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
          :label="$t('Employer name')"
          :value="props.workInformation.employerName"
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
          :label="$t('Employer phone')"
          :value="props.workInformation.employerPhone"
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
          :label="$t('Address Line 1')"
          :value="props.workInformation.employerAddressLine1"
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
          :label="$t('Address Line 2')"
          :value="props.workInformation.employerAddressLine2"
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
          :label="$t('City')"
          :value="props.workInformation.employerCity"
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
          :label="$t('State')"
          :value="props.workInformation.employerState"
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
          :label="$t('Zip')"
          :value="props.workInformation.employerZip"
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
          :label="$t('Country')"
          :value="props.workInformation.employerCountry"
        />
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
  max-height: 2vh;
  min-height: 1vh;
  margin-left: 0.5rem;
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
