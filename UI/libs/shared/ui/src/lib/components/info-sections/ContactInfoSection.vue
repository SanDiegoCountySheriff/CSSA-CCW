<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Contact Information: ') }}
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :value="props.contactInfo.primaryPhoneNumber"
          :label="$t('Primary phone number')"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :value="props.contactInfo.cellPhoneNumber"
          :label="$t('Cell phone number')"
        />
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :value="props.contactInfo.faxPhoneNumber"
          :label="$t('Fax phone number')"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :value="props.contactInfo.workPhoneNumber"
          :label="$t('Work phone number')"
        />
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          :value="props.contactInfo.textMessageUpdates"
          :label="$t('Text message updates')"
        />
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
