<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t('Character References: ') }}
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
    <v-row>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-1-circle
          </v-icon>
          <strong>
            {{ $t('Name: ') }}
          </strong>
          {{ props.characterReferences[0].name }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-1-circle
          </v-icon>
          <strong>
            {{ $t('Relationship: ') }}
          </strong>
          {{ props.characterReferences[0].relationship }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-1-circle
          </v-icon>
          <strong>
            {{ $t('Phone Number: ') }}
          </strong>
          {{ props.characterReferences[0].phoneNumber }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-1-circle
          </v-icon>
          <strong>
            {{ $t('Email Address: ') }}
          </strong>
          {{ props.characterReferences[0].email }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-2-circle
          </v-icon>
          <strong>
            {{ $t('Name:') }}
          </strong>
          {{ props.characterReferences[1].name }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-2-circle
          </v-icon>
          <strong>
            {{ $t('Relationship: ') }}
          </strong>
          {{ props.characterReferences[1].relationship }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-2-circle
          </v-icon>
          <strong>
            {{ $t('Phone Number:') }}
          </strong>
          {{ props.characterReferences[1].phoneNumber }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-2-circle
          </v-icon>
          <strong>
            {{ $t('Email Address: ') }}
          </strong>
          {{ props.characterReferences[1].email }}
        </v-banner>
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-3-circle
          </v-icon>
          <strong>
            {{ $t('Name:') }}
          </strong>
          {{ props.characterReferences[2].name }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-3-circle
          </v-icon>
          <strong>
            {{ $t('Relationship: ') }}
          </strong>
          {{ props.characterReferences[2].relationship }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-3-circle
          </v-icon>
          <strong>
            {{ $t('Phone Number:') }}
          </strong>
          {{ props.characterReferences[2].phoneNumber }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        md="3"
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
            mdi-numeric-3-circle
          </v-icon>
          <strong>
            {{ $t('Email Address: ') }}
          </strong>
          {{ props.characterReferences[2].email }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { CharacterReferenceType } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'

interface ICharacterReferenceInfoSectionProps {
  characterReferences: CharacterReferenceType
  color: string
}

const props = defineProps<ICharacterReferenceInfoSectionProps>()
const router = useRouter()
const applicationStore = useCompleteApplicationStore()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 1
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete:
        applicationStore.completeApplication.application.isComplete.toString(),
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
  padding-bottom: 0.5rem;
  padding-left: 1rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
