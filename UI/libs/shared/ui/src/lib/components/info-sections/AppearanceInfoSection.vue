<template>
  <v-container
    fluid
    class="appearance-info-container rounded mt-3"
  >
    <v-banner
      single-line
      class="sub-header font-weight-bold text-xl text-left mb-5"
    >
      {{ $t(' Physical Appearance Information ') }}
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          class="text-left"
          single-line
        >
          <v-icon
            left
            color="primary"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Height Feet: ') }}
          </strong>
          {{ props.appearanceInfo.heightFeet }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          class="text-left"
          single-line
        >
          <v-icon
            left
            color="primary"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Height Inches: ') }}
          </strong>
          {{ props.appearanceInfo.heightInch }}
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
          class="text-left"
          single-line
        >
          <v-icon
            left
            color="primary"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Weight (lbs): ') }}
          </strong>
          {{ props.appearanceInfo.weight }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          class="text-left"
          single-line
        >
          <v-icon
            left
            color="primary"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Eye Color: ') }}
          </strong>
          {{ props.appearanceInfo.eyeColor }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          class="text-left"
          single-line
        >
          <v-icon
            left
            color="primary"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Hair Color: ') }}
          </strong>
          {{ props.appearanceInfo.hairColor }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          class="text-left"
          single-line
        >
          <v-icon
            left
            color="primary"
          >
            mdi-account
          </v-icon>
          <strong>
            {{ $t('Gender: ') }}
          </strong>
          {{ props.appearanceInfo.gender }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { AppearanceInfoType } from '@shared-utils/types/defaultTypes'

interface IAppearanceInfoSectionProps {
  appearanceInfo: AppearanceInfoType
  color: string
}

const props = defineProps<IAppearanceInfoSectionProps>()
</script>

<style lang="scss" scoped>
.appearance-info-container {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
