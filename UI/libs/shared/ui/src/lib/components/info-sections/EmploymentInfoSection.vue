<template>
  <v-container class="info-section-container rounded">
    <v-banner class="sub-header font-weight-bold text-left my-5">
      {{ $t('Employment Information: ') }}
      <template #actions>
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <v-btn
              v-if="
                applicationStore.completeApplication.application.status ==
                ApplicationStatus.Incomplete
              "
              icon
              @click="handleEditRequest"
              v-bind="attrs"
              v-on="on"
            >
              <v-icon :color="$vuetify.theme.dark ? 'info' : 'info'">
                mdi-square-edit-outline
              </v-icon>
            </v-btn>
          </template>
          {{ $t('Edit Section') }}
        </v-tooltip>
      </template>
    </v-banner>
    <div v-if="props.employmentInfo == 'Unemployed'">
      <v-banner
        rounded
        single-line
        class="text-left"
      >
        <v-icon
          left
          color="primary"
        >
          mdi-briefcase
        </v-icon>
        <strong>
          {{ $t('Employment Status: ') }}
        </strong>
        {{ props.employmentInfo }}
      </v-banner>
    </div>
    <div v-else>
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
              mdi-briefcase
            </v-icon>
            <strong>
              {{ $t('Employment Status: ') }}
            </strong>
            {{ props.employmentInfo }}
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
              mdi-briefcase
            </v-icon>
            <strong>
              {{ $t('Employment Name: ') }}
            </strong>
            {{ props.workInformation.employerName }}
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
              mdi-briefcase
            </v-icon>
            <strong>
              {{ $t('Address Line 1: ') }}
            </strong>
            {{ props.workInformation.employerAddressLine1 }}
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
              mdi-briefcase
            </v-icon>
            <strong>
              {{ $t('Address Line 2: ') }}
            </strong>
            {{ props.workInformation.employerAddressLine2 }}
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
              mdi-briefcase
            </v-icon>
            <strong>
              {{ $t(' Emplorer City: ') }}
            </strong>
            {{ props.workInformation.employerCity }}
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
              mdi-briefcase
            </v-icon>
            <strong>
              {{ $t(' Emplorer State: ') }}
            </strong>
            {{ props.workInformation.employerState }}
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
              mdi-briefcase
            </v-icon>
            <strong>
              {{ $t(' Emplorer Zip: ') }}
            </strong>
            {{ props.workInformation.employerZip }}
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
              mdi-briefcase
            </v-icon>
            <strong>
              {{ $t('Employment Phone number: ') }}
            </strong>
            {{ props.workInformation.employerPhone }}
          </v-banner>
        </v-col>
      </v-row>
    </div>
  </v-container>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { WorkInformationType } from '@shared-utils/types/defaultTypes'
import { useRouter } from 'vue-router/composables'

interface IEmploymentInfoSectionProps {
  employmentInfo: string
  workInformation: WorkInformationType
  color: string
}
const props = defineProps<IEmploymentInfoSectionProps>()
const router = useRouter()
const applicationStore = useCompleteApplicationStore()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 6
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete.toString(),
    },
  })
}
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
  padding-left: 1rem;
  padding-bottom: 0.5rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
