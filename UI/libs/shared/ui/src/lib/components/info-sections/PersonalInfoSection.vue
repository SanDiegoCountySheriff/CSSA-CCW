<template>
  <v-container class="info-section-container rounded">
    <v-card
      :color="props.color"
      class="rounded"
    >
      <v-card-title class="ml-3">
        {{ $t('Personal Information') }}
      </v-card-title>
      <v-card-text class="m-5">
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Last name:') }}</h3>
              <p class="info-text">
                {{ props.personalInfo.lastName }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('First name:') }}</h3>
              <p class="info-text">
                {{ props.personalInfo.firstName }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Middle name: ') }}
              </h3>
              <p class="info-text">
                {{ props.personalInfo.middleName }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('No middle name: ') }}
              </h3>
              <p class="info-text">
                {{ props.personalInfo.noMiddleName }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Maiden name: ') }}
              </h3>
              <p class="info-text">
                {{ props.personalInfo.maidenName }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Suffix name: ') }}
              </h3>
              <p class="info-text">
                {{ props.personalInfo.suffix }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Social Security number: ') }}
              </h3>
              <p class="info-text">
                {{ props.personalInfo.ssn }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Marital status: ') }}
              </h3>
              <p class="info-text">
                {{ props.personalInfo.maritalStatus }}
              </p>
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
/**
 * This Should be able to be used in both the public and the admin.
 * to change the size put this into a container in the parent component and change
 * the size of the container
 * Also the bg color can be changed in the props.
 */
import { PersonalInfoType } from '@shared-ui/types/defaultTypes';

interface personalInfoSectionProps {
  personalInfo: PersonalInfoType;
  color: string;
}
const props = defineProps<personalInfoSectionProps>();
</script>

<style lang="scss">
.info-section-container {
  width: 100%;
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
}
</style>
