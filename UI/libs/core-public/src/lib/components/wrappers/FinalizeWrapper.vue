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

    <template v-if="!state.isLoading && !state.isError">
      <v-card>
        <v-card-title
          class="d-flex justify-center align-center"
          @click="state.open = !state.open"
        >
          <div class="flex-grow-1 text-center">
            {{ $t('Application Review') }}
          </div>
          <v-icon>
            {{ state.open ? 'mdi-menu-up' : 'mdi-menu-down' }}
          </v-icon>
        </v-card-title>
        <v-card-subtitle
          class="text-center mr-3"
          v-if="!isRenew"
        >
          {{ $t('Click to review your application') }}
        </v-card-subtitle>
        <v-card-text v-if="state.open">
          <div class="info-section">
            <PersonalInfoSection
              :color="'info'"
              :personal-info="
                applicationStore.completeApplication.application.personalInfo
              "
            />
          </div>

          <div
            class="info-section"
            v-if="
              applicationStore.completeApplication.application.personalInfo
                .maritalStatus == 'Married'
            "
          >
            <SpouseInfoSection
              :color="'info'"
              :spouse-info="
                applicationStore.completeApplication.application
                  .spouseInformation
              "
            />
          </div>

          <div
            class="info-section"
            v-if="
              applicationStore.completeApplication.application
                .differentSpouseAddress
            "
          >
            <SpouseAddressInfoSection
              :title="$t('Different Spouse Address').toString()"
              :color="'info'"
              :spouse-address="
                applicationStore.completeApplication.application
                  .spouseAddressInformation
              "
            />
          </div>

          <div
            class="info-section"
            v-if="
              applicationStore.completeApplication.application.aliases.length >
              0
            "
          >
            <AliasInfoSection
              :color="'transparent'"
              :alias-info="
                applicationStore.completeApplication.application.aliases
              "
            />
          </div>

          <div class="info-section">
            <IdInfoSection
              :color="'info'"
              :id-info="applicationStore.completeApplication.application.idInfo"
            />
          </div>

          <div class="info-section">
            <DOBinfoSection
              :color="'info'"
              :birth-info="applicationStore.completeApplication.application.dob"
            />
          </div>

          <div class="info-section">
            <CitizenInfoSection
              :color="'info'"
              :citizenship-info="
                applicationStore.completeApplication.application.citizenship
              "
              :immigrant-info="
                applicationStore.completeApplication.application
                  .immigrantInformation
              "
            />
          </div>

          <div class="info-section">
            <AddressInfoSection
              :color="'info'"
              :title="'Current Address'"
              :address-info="
                applicationStore.completeApplication.application.currentAddress
              "
            />
          </div>

          <div
            class="info-section"
            v-if="
              applicationStore.completeApplication.application.previousAddresses
                .length > 0
            "
          >
            <PreviousAddressInfoSection
              :previous-address="
                applicationStore.completeApplication.application
                  .previousAddresses
              "
              :color="'info'"
            />
          </div>

          <div
            class="info-section"
            v-if="
              applicationStore.completeApplication.application.differentMailing
            "
          >
            <AddressInfoSection
              :title="'Mailing Address'"
              :address-info="
                applicationStore.completeApplication.application.mailingAddress
              "
              color="info"
            />
          </div>

          <div class="info-section">
            <AppearanceInfoSection
              color="info"
              :appearance-info="
                applicationStore.completeApplication.application
                  .physicalAppearance
              "
            />
          </div>

          <div class="info-section">
            <CharacterReferenceInfoSection
              :character-references="
                applicationStore.completeApplication.application
                  .characterReferences
              "
              color="info"
            />
          </div>

          <div class="info-section">
            <ContactInfoSection
              :contact-info="
                applicationStore.completeApplication.application.contact
              "
              color="info"
            />
          </div>

          <div class="info-section">
            <EmploymentInfoSection
              :employment-info="
                applicationStore.completeApplication.application.employment
              "
              color="info"
              :work-information="
                applicationStore.completeApplication.application.workInformation
              "
            />
          </div>

          <div
            class="info-section"
            v-if="
              applicationStore.completeApplication.application.weapons.length >
              0
            "
          >
            <WeaponInfoSection
              :weapons="
                applicationStore.completeApplication.application.weapons
              "
            />
          </div>

          <div class="info-section">
            <ApplicationTypeInfoSection />
          </div>

          <div class="info-section">
            <FileUploadInfoSection
              :color="'primary'"
              :uploaded-documents="
                applicationStore.completeApplication.application
                  .uploadedDocuments
              "
              :enable-button="false"
              :enable-eight-hour-safety-course-button="false"
            />
          </div>

          <div class="info-section">
            <QualifyingQuestionsInfoSection
              :color="'primary'"
              :qualifying-questions-info="
                applicationStore.completeApplication.application
                  .qualifyingQuestions
              "
            />
          </div>

          <div class="info-section">
            <SignatureInfoSection />
          </div>
        </v-card-text>
      </v-card>
    </template>
  </div>
</template>

<script setup lang="ts">
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue'
import AliasInfoSection from '@shared-ui/components/info-sections/AliasInfoSection.vue'
import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue'
import { ApplicationType } from '@shared-utils/types/defaultTypes'
import ApplicationTypeInfoSection from '@shared-ui/components/info-sections/ApplicationTypeInfoSection.vue'
import CharacterReferenceInfoSection from '@shared-ui/components/info-sections/CharacterReferenceInfoSection.vue'
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue'
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue'
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue'
import EmploymentInfoSection from '@shared-ui/components/info-sections/EmploymentInfoSection.vue'
import FileUploadInfoSection from '@shared-ui/components/info-sections/FileUploadInfoSection.vue'
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue'
import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue'
import PreviousAddressInfoSection from '@shared-ui/components/info-sections/PreviousAddressInfoSection.vue'
import QualifyingQuestionsInfoSection from '@shared-ui/components/info-sections/QualifyingQuestionsInfoSection.vue'
import SignatureInfoSection from '@shared-ui/components/info-sections/SignatureInfoSection.vue'
import SpouseAddressInfoSection from '@shared-ui/components/info-sections/SpouseAddressInfoSection.vue'
import SpouseInfoSection from '@shared-ui/components/info-sections/SpouseInfoSection.vue'
import WeaponInfoSection from '@shared-ui/components/info-sections/WeaponsInfoSection.vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { computed, onMounted, reactive } from 'vue'

const applicationStore = useCompleteApplicationStore()

const state = reactive({
  isLoading: true,
  isError: false,
  open: false,
})

const isRenew = computed(() => {
  const applicationType =
    applicationStore.completeApplication.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

onMounted(() => {
  // TODO: this should be a query
  if (!applicationStore.completeApplication.application.orderId) {
    applicationStore
      // TODO: check if this works
      .getUserApplication()
      .then(() => {
        state.isLoading = false
      })
      .catch(() => {
        state.isError = true
      })
  } else {
    state.isLoading = false
  }

  if (isRenew.value) {
    state.open = true
  }
})
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
