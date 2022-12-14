<template>
  <v-card elevation="0">
    <v-card-title class="subtitle-2">
      {{ $t('Citizenship Information:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-radio-group
          row
          v-model="permitStore.getPermitDetail.application.citizenship.citizen"
        >
          <template #label>
            <div>{{ $t('Citizen') }}</div>
          </template>
          <v-radio :value="true">
            <template #label>
              <div>{{ $t('Yes') }}</div>
            </template>
          </v-radio>
          <v-radio :value="false">
            <template #label>
              <div>{{ $t('No') }}</div>
            </template>
          </v-radio>
          <template #prepend>
            <v-icon
              x-small
              color="error"
            >
              mdi-star
            </v-icon>
          </template>
        </v-radio-group>
      </v-col>

      <v-col
        cols="12"
        md="5"
        sm="12"
        class="pl-8"
      >
        <v-select
          v-model="
            permitStore.getPermitDetail.application.citizenship.militaryStatus
          "
          :items="items"
          :label="$t('Military Status')"
        />

        <v-alert
          dense
          outlined
          type="error"
          v-if="
            !permitStore.getPermitDetail.application.citizenship.militaryStatus
          "
        >
          {{ $t('Must select a status') }}
        </v-alert>
        <v-alert
          dense
          outlined
          type="warning"
          v-if="
            permitStore.getPermitDetail.application.citizenship
              .militaryStatus === 'Discharged'
          "
        >
          {{ $t('discharged-disclaimer') }}
        </v-alert>
      </v-col>
    </v-row>
    <div v-if="!permitStore.getPermitDetail.application.citizenship.citizen">
      <v-divider class="mb-3" />
      <v-card-title class="subtitle-2">
        {{ $t('Immigrant Information:') }}
      </v-card-title>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-combobox
            dense
            outlined
            :items="countries"
            :label="$t('Country of Citizenship')"
            :rules="[v => !!v || $t('You must enter a country')]"
            autocomplete="nope"
            v-model="
              permitStore.getPermitDetail.application.immigrantInformation
                .countryOfCitizenship
            "
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-combobox>
        </v-col>
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-combobox
            dense
            outlined
            :items="countries"
            :label="$t('Country of Birth')"
            :rules="[v => !!v || $t('You must enter a country')]"
            autocomplete="nope"
            v-model="
              permitStore.getPermitDetail.application.immigrantInformation
                .countryOfBirth
            "
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-combobox>
        </v-col>
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-radio-group
            row
            v-model="
              permitStore.getPermitDetail.application.immigrantInformation
                .immigrantAlien
            "
          >
            <template #label>
              <div>{{ $t('Immigrant Alien') }}</div>
            </template>
            <v-radio :value="true">
              <template #label>
                <div>{{ $t('Yes') }}</div>
              </template>
            </v-radio>
            <v-radio :value="false">
              <template #label>
                <div>{{ $t('No') }}</div>
              </template>
            </v-radio>
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-radio-group>
        </v-col>
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-radio-group
            row
            v-model="
              permitStore.getPermitDetail.application.immigrantInformation
                .nonImmigrantAlien
            "
          >
            <template #label>
              <div>{{ $t('Non-Immigrant Alien') }}</div>
            </template>
            <v-radio :value="true">
              <template #label>
                <div>{{ $t('Yes') }}</div>
              </template>
            </v-radio>
            <v-radio :value="false">
              <template #label>
                <div>{{ $t('No') }}</div>
              </template>
            </v-radio>
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-radio-group>
        </v-col>
      </v-row>
    </div>
  </v-card>
</template>
<script setup lang="ts">
import { countries } from '@shared-utils/lists/defaultConstants';
import { ref } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const items = ref(['Active', 'Reserve', 'Discharged', 'Retired', 'N/A']);

const permitStore = usePermitsStore();
</script>
