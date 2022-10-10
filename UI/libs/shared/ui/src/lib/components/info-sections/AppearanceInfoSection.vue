<template>
  <v-container
    fluid
    class="appearance-info-container rounded"
  >
    <v-card
      :color="props.color"
      class="rounded"
    >
      <v-card-title class="ml-3">
        {{ $t('Physical Appearance Information') }}
      </v-card-title>
      <v-card-text class="m-5">
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Height in feet: ') }}</h3>
              <p class="info-text">
                {{ props.appearanceInfo.heightFeet }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Height in inches: ') }}</h3>
              <p class="info-text">
                {{ props.appearanceInfo.heightInch }}
              </p>
            </div>
          </v-col>

          <v-col>
            <div class="info-row">
              <h3>{{ $t('Weight: ') }}</h3>
              <p class="info-text">
                {{ props.appearanceInfo.weight }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Hair color: ') }}</h3>
              <p class="info-text">
                {{ props.appearanceInfo.hairColor }}
              </p>
            </div>
          </v-col>

          <v-col>
            <div class="info-row">
              <h3>{{ $t('Eye color: ') }}</h3>
              <p class="info-text">
                {{ props.appearanceInfo.eyeColor }}
              </p>
            </div>
          </v-col>

          <v-col>
            <div class="info-row">
              <h3>{{ $t('Gender: ') }}</h3>
              <p class="info-text">
                {{ props.appearanceInfo.gender }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Physical Description: ') }}</h3>
              <p class="info-text">
                {{ props.appearanceInfo.physicalDesc }}
              </p>
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { AppearanceInfoType } from '@shared-utils/types/defaultTypes';

interface IAppearanceInfoSectionProps {
  appearanceInfo: AppearanceInfoType;
  color: string;
}

const props = defineProps<IAppearanceInfoSectionProps>();
</script>

<style lang="scss" scoped>
.appearance-info-container {
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
