<template>
  <div>
    <!-- need to create sections for the info -->
    <v-row>
      <div class="info-card">
        <PersonalInfoSection
          :color="'teal lighten-5'"
          :personal-info="state.personalInfo"
        />
      </div>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { useGetters } from 'vuex-composition-helpers';
import { reactive } from 'vue';
import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue';

const {
  currentAddress,
  aliases,
  physicalAppearanceInfo,
  citizenshipInfo,
  contactInfo,
  DOBInfo,
  id,
  personalInfo,
  previousAddresses,
} = useGetters([
  'currentAddress',
  'aliases',
  'physicalAppearanceInfo',
  'citizenshipInfo',
  'contactInfo',
  'DOBInfo',
  'id',
  'personalInfo',
  'previousAddresses',
]);

const state = reactive({
  address: currentAddress.value,
  aliases: aliases.value,
  appearance: physicalAppearanceInfo.value,
  citizenshipInfo: citizenshipInfo.value,
  contact: contactInfo.value,
  DOBInfo: DOBInfo.value,
  id: id.value,
  personalInfo: personalInfo.value,
  previousAddress: previousAddresses.value,
});
</script>

<style lang="scss" scoped>
.info-card {
  width: 80%;
}
</style>
