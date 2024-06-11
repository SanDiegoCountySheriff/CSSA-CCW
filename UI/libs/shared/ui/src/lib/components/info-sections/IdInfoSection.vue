<template>
  <v-container
    fluid
    class="confirm-info-section rounded mt-5"
  >
    <v-banner
      single-line
      class="sub-header font-weight-bold text-xl text-left my-5"
    >
      {{ $t('Id Information: ') }}
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="4"
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
            {{ $t('Id Number: ') }}
          </strong>
          {{ props.idInfo.idNumber }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="4"
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
            {{ $t('Issuing State: ') }}
          </strong>
          {{ props.idInfo.issuingState }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        lg="4"
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
            {{ $t('Restrictions: ') }}
          </strong>
          {{ props.idInfo.restrictions }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { IdType } from '@shared-utils/types/defaultTypes'

interface IIdInfoSectionProps {
  idInfo: IdType
  color: string
}

const props = defineProps<IIdInfoSectionProps>()
</script>

<style lang="scss" scoped>
.confirm-info-section {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}

.info-row {
  display: flex;
  flex-direction: row;
  min-height: 1vh;
  max-height: 2vh;
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
