<template>
  <v-container
    fluid
    class="DOB-info-container rounded mt-5"
  >
    <v-banner
      as="h1"
      class="sub-header font-weight-bold text-left mb-5"
    >
      {{ $t('Birth Information: ') }}
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
            color="primary"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Birth Date: ') }}
          </strong>
          {{ props.birthInfo.birthDate }}
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
            color="primary"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Birth City: ') }}
          </strong>
          {{ props.birthInfo.birthCity }}
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
            color="primary"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Birth State: ') }}
          </strong>
          {{ props.birthInfo.birthState }}
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
            color="primary"
          >
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Birth Country : ') }}
          </strong>
          {{ props.birthInfo.birthCountry }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { DOBType } from '@shared-utils/types/defaultTypes'

interface IDOBInfoSectionProps {
  birthInfo: DOBType
}

const props = defineProps<IDOBInfoSectionProps>()
</script>

<style lang="scss" scoped>
.DOB-info-container {
  width: 100%;
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
