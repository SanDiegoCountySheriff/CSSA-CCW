<template>
  <v-container
    class="carousel-container"
    fluid
  >
    <v-carousel
      hide-delimiter-background
      hide-delimiters
      class="carousel-body"
      v-model="currentInfoStore.state.selection"
      :style="{
        backgroundColor: $vuetify.theme.dark ? '#333' : 'white',
        borderRadius: '5px',
      }"
    >
      <v-carousel-item class="item">
        <div class="info-section">
          <PersonalInfoSection
            :color="'info'"
            :personal-info="completeApplication.personalInfo"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item class="item">
        <div class="info-section">
          <SpouseInfoSection
            :color="'info'"
            :spouse-info="completeApplication.spouseInformation"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item class="item">
        <div class="info-section">
          <AliasInfoSection
            :color="'transparent'"
            :alias-info="completeApplication.aliases"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item class="item">
        <div class="info-section">
          <IdInfoSection
            :color="'info'"
            :id-info="completeApplication.idInfo"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <DOBinfoSection
            :color="'info'"
            :d-o-b-info="completeApplication.dob"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item class="item">
        <div class="info-section">
          <CitizenInfoSection
            :color="'info'"
            :citizenship-info="completeApplication.citizenship"
            :immigrant-info="completeApplication.immigrantInformation"
          />
        </div>
      </v-carousel-item>
      <v-carousel-item class="item">
        <div class="info-section">
          <AddressInfoSection
            :color="'info'"
            :title="'Current Address'"
            :address-info="completeApplication.currentAddress"
          />
          <v-container class="different-mailing-container">
            <v-row>
              <v-col
                cols="12"
                lg="6"
              >
                <v-text-field
                  outlined
                  dense
                  class="pl-6"
                  :label="$t('Different Mailing Address')"
                  :value="completeApplication.differentMailing"
                />
              </v-col>
            </v-row>
          </v-container>
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <PreviousAddressInfoSection
            :previous-address="completeApplication.previousAddresses"
            :color="'info'"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item class="item">
        <div class="info-section">
          <AddressInfoSection
            :title="'Mailing Address'"
            :address-info="completeApplication.mailingAddress"
            color="info"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item class="item">
        <div class="info-section">
          <AppearanceInfoSection
            color="info"
            :appearance-info="completeApplication.physicalAppearance"
          />
        </div>
      </v-carousel-item>
      <v-carousel-item class="item">
        <div class="info-section">
          <ContactInfoSection
            :contact-info="completeApplication.contact"
            color="info"
          />
        </div>
      </v-carousel-item>
      <v-carousel-item class="item">
        <div class="info-section">
          <EmploymentInfoSection
            :employment-info="completeApplication.employment"
            color="info"
            :work-information="completeApplication.workInformation"
          />
        </div>
      </v-carousel-item>
      <v-carousel-item class="item">
        <div class="info-section">
          <WeaponInfoSection :weapons="completeApplication.weapons" />
        </div>
      </v-carousel-item>
    </v-carousel>
  </v-container>
</template>

<script setup lang="ts">
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue';
import AliasInfoSection from '@shared-ui/components/info-sections/AliasInfoSection.vue';
import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue';
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue';
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue';
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue';
import EmploymentInfoSection from '@shared-ui/components/info-sections/EmploymentInfoSection.vue';
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue';
import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue';
import PreviousAddressInfoSection from '@shared-ui/components/info-sections/PreviousAddressInfoSection.vue';
import SpouseInfoSection from '@shared-ui/components/info-sections/SpouseInfoSection.vue';
import WeaponInfoSection from '@shared-ui/components/info-sections/WeaponsInfoSection.vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useCurrentInfoSection } from '@core-public/stores/currentInfoSection';

const currentInfoStore = useCurrentInfoSection();
const applicationStore = useCompleteApplicationStore();
const completeApplication = applicationStore.getCompleteApplication.application;
</script>

<style lang="scss" scoped>
.carousel-container {
  min-height: 100%;
}
.carousel-body {
  height: auto !important;
}

.different-mailing-container {
  width: 80%;
  height: 100%;
  margin: 1.5em 0;
  padding: 0;
}

.info-section {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  overflow-y: scroll;
}

.info-row {
  display: flex;
  flex-direction: row;
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
  font-size: 1em;
  font-weight: bold;
}

.item {
  height: 100%;
  overflow-y: scroll;
}
</style>
