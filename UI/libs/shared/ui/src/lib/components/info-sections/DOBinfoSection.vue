<template>
  <v-container
    fluid
    class="DOB-info-container rounded"
  >
    <v-card
      :color="props.color"
      class="rounded"
    >
      <v-card-title>
        {{ $t(' DOB Information') }}
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('DOB:') }}
              </h3>
              <p class="info-text">
                {{ props.DOBInfo.birthDate }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Current Age: ') }}
              </h3>
              <p class="info-text">
                {{ props.DOBInfo.currentAge }}
              </p>
            </div>
          </v-col>
          <v-col> </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Birth city: ') }}
              </h3>
              <p class="info-text">
                {{ props.DOBInfo.birthCity }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Birth State: ') }}
              </h3>
              <p class="info-text">
                {{ props.DOBInfo.birthState }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Birth country: ') }}
              </h3>
              <p class="info-text">
                {{ props.DOBInfo.birthCountry }}
              </p>
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { DOBType } from '@shared-utils/types/defaultTypes';

interface IDOBInfoSectionProps {
  // eslint-disable-next-line vue/prop-name-casing
  DOBInfo: DOBType;
  color: string;
}

const props = defineProps<IDOBInfoSectionProps>();
</script>

<style lang="scss" scoped>
.DOB-info-container {
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
