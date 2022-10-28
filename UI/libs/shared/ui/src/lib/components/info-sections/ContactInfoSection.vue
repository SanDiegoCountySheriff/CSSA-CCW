<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Contact Information: ') }}
    </v-banner>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Primary Phone number') }}
          </v-chip>
          <div class="info-text">
            {{ props.contactInfo.primaryPhoneNumber }}
          </div>
        </div>
      </v-col>
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Cell Phone number') }}
          </v-chip>
          <div class="info-text">
            {{ props.contactInfo.cellPhoneNumber }}
          </div>
        </div>
      </v-col>
    </v-row>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Fax Phone number') }}
          </v-chip>
          <div class="info-text">
            {{ props.contactInfo.faxPhoneNumber }}
          </div>
        </div>
      </v-col>
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Work Phone number') }}
          </v-chip>
          <div class="info-text">
            {{ props.contactInfo.workPhoneNumber }}
          </div>
        </div>
      </v-col>
    </v-row>
    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Text Message updates') }}
          </v-chip>
          <div class="info-text">
            {{ props.contactInfo.textMessageUpdates }}
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ContactInfoType } from '@shared-utils/types/defaultTypes';

interface IContactInfoSectionProps {
  contactInfo: ContactInfoType;
  color: string;
}

const props = defineProps<IContactInfoSectionProps>();
</script>

<style lang="scss" scoped>
.info-section-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
.info-row {
  display: flex;
  flex-direction: row;
  max-height: 2em;
  margin-left: 0.5rem;
}

.info-text {
  margin-left: 0.5rem;
  text-align: start;
  height: 1.8em;
  width: 50%;
  margin-bottom: 0.5rem;
  padding-left: 0.5rem;
  padding-top: 0.2rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
