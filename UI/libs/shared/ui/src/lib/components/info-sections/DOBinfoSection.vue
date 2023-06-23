<template>
  <v-container class="DOB-info-container rounded mt-5">
    <v-banner
      as="h1"
      class="sub-header font-weight-bold text-left mb-5"
    >
      {{ $t('Birth Information: ') }}
      <template #actions>
        <v-btn
          v-if="
            applicationStore.completeApplication.application.status ==
            ApplicationStatus.Incomplete
          "
          icon
          @click="handleEditRequest"
        >
          <v-icon :color="$vuetify.theme.dark ? 'info' : 'info'">
            mdi-square-edit-outline
          </v-icon>
        </v-btn>
      </template>
    </v-banner>

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
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Birth Date: ') }}
          </strong>
          {{ props.DOBInfo.birthDate }}
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
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Birth City: ') }}
          </strong>
          {{ props.DOBInfo.birthCity }}
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
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Birth State: ') }}
          </strong>
          {{ props.DOBInfo.birthState }}
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
            mdi-card-account-details
          </v-icon>
          <strong>
            {{ $t('Birth Country : ') }}
          </strong>
          {{ props.DOBInfo.birthCountry }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { DOBType } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'

interface IDOBInfoSectionProps {
  // eslint-disable-next-line vue/prop-name-casing
  DOBInfo: DOBType
  color: string
}

const props = defineProps<IDOBInfoSectionProps>()

const applicationStore = useCompleteApplicationStore()
const router = useRouter()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 2
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  })
}
</script>

<style lang="scss" scoped>
.DOB-info-container {
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
