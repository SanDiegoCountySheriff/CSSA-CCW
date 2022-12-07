<template>
  <v-container class="info-section-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Personal Information: ') }}
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
          :label="$t('Last name')"
          :value="props.personalInfo.lastName"
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
          :label="$t('First Name')"
          :value="props.personalInfo.firstName"
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
          :label="$t('Middle Name')"
          :value="props.personalInfo.middleName"
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
          :label="$t('Maiden Name')"
          :value="props.personalInfo.maidenName"
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
          :label="$t('Suffix')"
          :value="props.personalInfo.suffix"
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
          :label="$t('SSN')"
          :value="props.personalInfo.ssn"
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
          :label="$t('Marital status')"
          :value="props.personalInfo.maritalStatus"
        />
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
