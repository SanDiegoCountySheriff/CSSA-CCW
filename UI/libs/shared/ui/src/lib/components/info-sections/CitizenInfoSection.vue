<template>
  <v-container class="citizen-info-container rounded">
    <v-card
      :color="props.color"
      class="rounded"
    >
      <v-card-title class="ml-3">
        {{ $t(' Citizenship Information') }}
      </v-card-title>

      <v-card-text class="m-5">
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Citizen: ') }}</h3>
              <p class="info-text">
                {{ props.citizenshipInfo.citizen }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Military Status: ') }}</h3>
              <p class="info-text">
                {{ props.citizenshipInfo.militaryStatus }}
              </p>
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { CitizenshipType } from '@shared-ui/types/defaultTypes';

interface ICitizenShipInfoSectionProps {
  citizenshipInfo: CitizenshipType;
  color: string;
}

const props = defineProps<ICitizenShipInfoSectionProps>();
</script>

<style lang="scss">
.citizen-info-container {
  width: 100%;
  padding: 0;
  margin: 0;
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
