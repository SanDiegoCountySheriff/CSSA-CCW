<template>
  <div>
    <v-container
      v-if="state.isLoading && !state.isError"
      fluid
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
        divider, list-item-three-line,
        card-heading, image, image, image,
        image, actions"
      >
      </v-skeleton-loader>
    </v-container>
    <v-container
      v-if="!state.isLoading && !state.isError"
      class="carousel-container"
    >
      <v-card>
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <v-card-title
              v-bind="attrs"
              v-on="on"
            >
              {{ $t(' Application Review ') }}
              <v-icon
                class="ml-3"
                @click="state.open = !state.open"
              >
                {{ state.open ? 'mdi-menu-down' : 'mdi-menu-up' }}
              </v-icon>
            </v-card-title>
          </template>
          {{ $t(' Click to review your application') }}
        </v-tooltip>
        <v-card-text v-if="state.open">
          <div class="info-section">
            <PersonalInfoSection
              :color="'info'"
              :personal-info="state.completeApplication.personalInfo"
            />
          </div>

          <div class="info-section">
            <SpouseInfoSection
              :color="'info'"
              :spouse-info="state.completeApplication.spouseInformation"
            />
          </div>

          <div class="info-section">
            <SpouseAddressInfoSection
              :title="$t('Different Spouse Address').toString()"
              :color="'info'"
              :spouse-address="
                state.completeApplication.spouseAddressInformation
              "
            />
          </div>

          <div class="info-section">
            <AliasInfoSection
              :color="'transparent'"
              :alias-info="state.completeApplication.aliases"
            />
          </div>

          <div class="info-section">
            <IdInfoSection
              :color="'info'"
              :id-info="state.completeApplication.idInfo"
            />
          </div>

          <div class="info-section">
            <DOBinfoSection
              :color="'info'"
              :d-o-b-info="state.completeApplication.dob"
            />
          </div>

          <div class="info-section">
            <CitizenInfoSection
              :color="'info'"
              :citizenship-info="state.completeApplication.citizenship"
              :immigrant-info="state.completeApplication.immigrantInformation"
            />
          </div>

          <div class="info-section">
            <AddressInfoSection
              :color="'info'"
              :title="'Current Address'"
              :address-info="state.completeApplication.currentAddress"
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
                    :value="state.completeApplication.differentMailing"
                  />
                </v-col>
              </v-row>
            </v-container>
          </div>
          <div class="info-section">
            <PreviousAddressInfoSection
              :previous-address="state.completeApplication.previousAddresses"
              :color="'info'"
            />
          </div>

          <div class="info-section">
            <AddressInfoSection
              :title="'Mailing Address'"
              :address-info="state.completeApplication.mailingAddress"
              color="info"
            />
          </div>

          <div class="info-section">
            <AppearanceInfoSection
              color="info"
              :appearance-info="state.completeApplication.physicalAppearance"
            />
          </div>

          <div class="info-section">
            <ContactInfoSection
              :contact-info="state.completeApplication.contact"
              color="info"
            />
          </div>

          <div class="info-section">
            <EmploymentInfoSection
              :employment-info="state.completeApplication.employment"
              color="info"
              :work-information="state.completeApplication.workInformation"
            />
          </div>

          <div class="info-section">
            <WeaponInfoSection :weapons="state.completeApplication.weapons" />
          </div>
        </v-card-text>
      </v-card>
    </v-container>
  </div>
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
import SpouseAddressInfoSection from '@shared-ui/components/info-sections/SpouseAddressInfoSection.vue';
import SpouseInfoSection from '@shared-ui/components/info-sections/SpouseInfoSection.vue';
import WeaponInfoSection from '@shared-ui/components/info-sections/WeaponsInfoSection.vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useCurrentInfoSection } from '@core-public/stores/currentInfoSection';
import { onMounted, reactive } from 'vue';
import { useRoute } from 'vue-router/composables';

const currentInfoStore = useCurrentInfoSection();
const applicationStore = useCompleteApplicationStore();

const route = useRoute();

const state = reactive({
  isLoading: true,
  isError: false,
  completeApplication: applicationStore.getCompleteApplication.application,
  open: false,
});

onMounted(() => {
  if (!applicationStore.completeApplication.application.orderId) {
    applicationStore
      .getCompleteApplicationFromApi(
        route.query.orderId,
        route.query.isComplete
      )
      .then(res => {
        state.completeApplication = res.application;
        state.isLoading = false;
        window.console.log(state.completeApplication);
      })
      .catch(() => {
        state.isError = true;
      });
  } else {
    state.isLoading = false;
  }
});
</script>

<style lang="scss" scoped>
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
