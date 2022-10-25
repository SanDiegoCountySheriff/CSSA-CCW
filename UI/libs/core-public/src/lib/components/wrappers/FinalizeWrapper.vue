<template>
  <v-container
    class="carousel-container"
    fluid
  >
    <v-carousel
      class="carousel-body"
      v-model="currentInfoStore.state.selection"
    >
      <v-carousel-item>
        <div class="info-section">
          <PersonalInfoSection
            :color="'info'"
            :personal-info="applicationStore.completeApplication.personalInfo"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <SpouseInfoSection
            :color="'secondary'"
            :spouse-info="
              applicationStore.completeApplication.spouseInformation
            "
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <AliasInfoSection
            :color="'transparent'"
            :alias-info="applicationStore.completeApplication.aliases"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <IdInfoSection
            :color="'success'"
            :id-info="applicationStore.completeApplication.idInfo"
          />
          <DOBinfoSection
            :color="'info'"
            :d-o-b-info="applicationStore.completeApplication.DOB"
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <CitizenInfoSection
            :color="'warning'"
            :citizenship-info="applicationStore.completeApplication.citizenship"
            :immigrant-info="
              applicationStore.completeApplication.immigrantInformation
            "
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <AddressInfoSection
            :color="'primary'"
            :title="'Current Address'"
            :address-info="applicationStore.completeApplication.currentAddress"
          />
          <v-container class="different-mailing-container">
            <v-row class="ml-5">
              <v-col>
                <div class="info-row">
                  <v-chip
                    class="font-weight-bold"
                    label
                    color="primary"
                  >
                    {{ $t(' Different Mailing Address') }}
                  </v-chip>
                  <div class="info-text">
                    {{ applicationStore.completeApplication.differentMailing }}
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
              applicationStore.completeApplication.previousAddress
            "
            :color="'primary'"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <AddressInfoSection
            :title="'Mailing Address'"
            :address-info="applicationStore.completeApplication.mailingAddress"
            color="primary"
          />
        </div>
      </v-carousel-item>

      <v-carousel-item>
        <div class="info-section">
          <AppearanceInfoSection
            color="primary"
            :appearance-info="
              applicationStore.completeApplication.physicalAppearance
            "
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <ContactInfoSection
            :contact-info="applicationStore.completeApplication.contact"
            color="primary"
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <EmploymentInfoSection
            :employment-info="applicationStore.completeApplication.employment"
            color="primary"
            :work-information="
              applicationStore.completeApplication.workInformation
            "
          />
        </div>
      </v-carousel-item>
      <v-carousel-item>
        <div class="info-section">
          <WeaponInfoSection
            :weapons="applicationStore.completeApplication.weapons"
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
  background-color: #c3f3f3;
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
