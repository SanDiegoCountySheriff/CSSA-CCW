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
    >
      <v-carousel-item>
        <div class="info-section">
          <PersonalInfoSection
            :color="'info'"
            :personal-info="completeApplication.personalInfo"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <SpouseInfoSection
            :color="'info'"
            :spouse-info="
              completeApplication.spouseInformation
            "
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <AliasInfoSection
            :color="'transparent'"
            :alias-info="completeApplication.aliases"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <IdInfoSection
            :color="'info'"
            :id-info="completeApplication.idInfo"
          />
          <DOBinfoSection
            :color="'info'"
            :d-o-b-info="completeApplication.DOB"
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <CitizenInfoSection
            :color="'info'"
            :citizenship-info="completeApplication.citizenship"
            :immigrant-info="
              completeApplication.immigrantInformation
            "
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <AddressInfoSection
            :color="'info'"
            :title="'Current Address'"
            :address-info="completeApplication.currentAddress"
          />
          <v-container class="different-mailing-container">
            <v-row class="ml-5">
              <v-col>
                <div class="info-row">
                  <v-chip
                    class="font-weight-bold"
                    label
                    color="info"
                  >
                    {{ $t(' Different Mailing Address') }}
                  </v-chip>
                  <div class="info-text">
                    {{ completeApplication.differentMailing }}
                  </div>
                </div>
              </v-col>
            </v-row>
          </v-container>
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <PreviousAddressInfoSection
            :previous-address="
              completeApplication.previousAddress
            "
            :color="'info'"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <AddressInfoSection
            :title="'Mailing Address'"
            :address-info="completeApplication.mailingAddress"
            color="info"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <AppearanceInfoSection
            color="info"
            :appearance-info="
              completeApplication.physicalAppearance
            "
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <ContactInfoSection
            :contact-info="completeApplication.contact"
            color="info"
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <EmploymentInfoSection
            :employment-info="completeApplication.employment"
            color="info"
            :work-information="
              completeApplication.workInformation
            "
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <WeaponInfoSection
            :weapons="completeApplication.weapons"
          />
        </div>
      </v-carousel-item>
    </v-carousel>
  </v-container>
</template>

<script setup lang="ts">
import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue';
import { useCurrentInfoSection } from '@core-public/stores/currentInfoSection';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import SpouseInfoSection from '@shared-ui/components/info-sections/SpouseInfoSection.vue';
import AliasInfoSection from '@shared-ui/components/info-sections/AliasInfoSection.vue';
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue';
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue';
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue';
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue';
import PreviousAddressInfoSection from '@shared-ui/components/info-sections/PreviousAddressInfoSection.vue';
import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue';
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue';
import EmploymentInfoSection from '@shared-ui/components/info-sections/EmploymentInfoSection.vue';
import WeaponInfoSection from '@shared-ui/components/info-sections/WeaponsInfoSection.vue';

const currentInfoStore = useCurrentInfoSection();
const applicationStore = useCompleteApplicationStore();
const completeApplication = applicationStore.getCompleteApplication.application;
</script>

<style lang="scss" scoped>
.carousel-container {
  min-height: 100%;
}
.carousel-body {
  height: 40vh !important;
}

.different-mailing-container {
  width: 80%;
  height: 100%;
  margin: 1.5em;
  padding: 0;
}

.info-section {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.info-row {
  display: flex;
  flex-direction: row;
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
