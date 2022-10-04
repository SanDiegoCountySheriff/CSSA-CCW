<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-card
      :color="props.color"
      class="rounded"
    >
      <v-card-title class="ml-3">
        {{ $t('Contact Information') }}
      </v-card-title>
      <v-card-text class="m-5">
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Primary phone number') }}</h3>
              <p class="info-text">
                {{ props.contactInfo.primaryPhoneNumber }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Cell phone number') }}</h3>
              <p class="info-text">
                {{ props.contactInfo.cellPhoneNumber }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Work phone number') }}</h3>
              <p class="info-text">
                {{ props.contactInfo.workPhoneNumber }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>{{ $t(' Fax number') }}</h3>
              <p class="info-text">
                {{ props.contactInfo.faxPhoneNumber }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>{{ $t('Text message update') }}</h3>
              <p class="info-text">
                {{ props.contactInfo.textMessageUpdates }}
              </p>
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ContactInfoType } from '@shared-ui/types/defaultTypes';

interface IContactInfoSectionProps {
  contactInfo: ContactInfoType;
  color: string;
}

const props = defineProps<IContactInfoSectionProps>();
</script>

<style lang="scss" scoped></style>
