<template>
  <v-container class="citizen-info-container rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Citizenship Information: ') }}
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
          :label="$t('Citizen')"
          :value="props.citizenshipInfo.citizen"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          class="pl-6"
          readonly
          :label="$t('Military status')"
          :value="props.citizenshipInfo.militaryStatus"
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
          class="pl-6"
          readonly
          :label="$t('Immigrant Alien')"
          :value="props.immigrantInfo.immigrantAlien"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          class="pl-6"
          readonly
          :label="$t('Non Immigrant Alien')"
          :value="props.immigrantInfo.nonImmigrantAlien"
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
          class="pl-6"
          readonly
          :label="$t('Country of Birth')"
          :value="props.immigrantInfo.countryOfBirth"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          class="pl-6"
          readonly
          :label="$t('Country of Citizenship')"
          :value="props.immigrantInfo.countryOfCitizenship"
        />
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

<style lang="scss" scoped>
.citizen-info-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
