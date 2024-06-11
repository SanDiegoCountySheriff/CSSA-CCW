<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t('Contact Information: ') }}
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
            mdi-card-account-phone
          </v-icon>
          <strong>
            {{ $t('Primary Phone Number: ') }}
          </strong>
          {{ props.contactInfo.primaryPhoneNumber }}
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
            mdi-card-account-phone
          </v-icon>
          <strong>
            {{ $t('Cell Phone Number: ') }}
          </strong>
          {{ props.contactInfo.cellPhoneNumber }}
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
            mdi-card-account-phone
          </v-icon>
          <strong>
            {{ $t('Work Phone Number: ') }}
          </strong>
          {{ props.contactInfo.workPhoneNumber }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ContactInfoType } from '@shared-utils/types/defaultTypes'

interface IContactInfoSectionProps {
  contactInfo: ContactInfoType
  color: string
}

const props = defineProps<IContactInfoSectionProps>()
</script>

<style lang="scss" scoped>
.info-section-container {
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
  margin-left: 0.5rem;
}

.info-text {
  margin-left: 0.5rem;
  text-align: start;
  height: 1.8em;
  width: 50%;
  margin-bottom: 0.5rem;
  padding-bottom: 0.5rem;
  padding-left: 1rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
