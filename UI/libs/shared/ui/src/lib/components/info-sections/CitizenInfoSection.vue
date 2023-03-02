<template>
  <v-container class="citizen-info-container rounded">
    <v-banner class="sub-header font-weight-bold text-xl text-left my-5">
      {{ $t('Citizenship Information: ') }}
      <template #actions>
        <v-btn
          icon
          @click="handleEditRequest"
        >
          <v-icon :color="$vuetify.theme.dark ? 'info' : 'info'">
            mdi-square-edit-outline
          </v-icon>
        </v-btn>
      </template>
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Citizen: ') }}
          </strong>
          {{ props.citizenshipInfo.citizen }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Military Status: ') }}
          </strong>
          {{ props.citizenshipInfo.militaryStatus }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Immigrant Alien: ') }}
          </strong>
          {{ props.immigrantInfo.immigrantAlien }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Non Immigrant Alien: ') }}
          </strong>
          {{ props.immigrantInfo.nonImmigrantAlien }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Country of Birth: ') }}
          </strong>
          {{ props.immigrantInfo.countryOfBirth }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Country of Citizen: ') }}
          </strong>
          {{ props.immigrantInfo.countryOfCitizenship }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import {
  CitizenshipType,
  ImmigrantInformation,
} from '@shared-utils/types/defaultTypes';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useRouter } from 'vue-router/composables';

interface ICitizenShipInfoSectionProps {
  citizenshipInfo: CitizenshipType;
  immigrantInfo: ImmigrantInformation;
  color: string;
}

const props = defineProps<ICitizenShipInfoSectionProps>();
const applicationStore = useCompleteApplicationStore();
const router = useRouter();

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 2;
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  });
}
</script>

<style lang="scss" scoped>
.citizen-info-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
