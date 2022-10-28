<template>
  <v-container class="citizen-info-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Citizenship Information: ') }}
    </v-banner>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Citizen:') }}
          </v-chip>
          <div class="info-text">
            {{ props.citizenshipInfo.citizen }}
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
            {{ $t(' Military Status:') }}
          </v-chip>
          <div class="info-text">
            {{ props.citizenshipInfo.militaryStatus }}
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
            {{ $t('Immigrant Alien') }}
          </v-chip>
          <div class="info-text">
            {{ props.immigrantInfo.immigrantAlien }}
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
            {{ $t(' Non Immigrant Alien ') }}
          </v-chip>
          <div class="info-text">
            {{ props.immigrantInfo.nonImmigrantAlien }}
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
            {{ $t(' Country of Birth ') }}
          </v-chip>
          <div class="info-text">
            {{ props.immigrantInfo.countryOfBirth }}
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
            {{ $t(' Country of Citizenship ') }}
          </v-chip>
          <div class="info-text">
            {{ props.immigrantInfo.countryOfCitizenship }}
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import {
  CitizenshipType,
  ImmigrantInformation,
} from '@shared-utils/types/defaultTypes';

interface ICitizenShipInfoSectionProps {
  citizenshipInfo: CitizenshipType;
  immigrantInfo: ImmigrantInformation;
  color: string;
}

const props = defineProps<ICitizenShipInfoSectionProps>();
</script>

<style lang="scss">
.citizen-info-container {
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
