<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t('Character References:') }}
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
    <v-row
      v-for="(reference, refIndex) in $props.characterReferences"
      :key="refIndex"
    >
      <v-col
        cols="12"
        md="3"
        v-for="(field, fieldIndex) in [
          { key: 'name', label: 'Name' },
          { key: 'relationship', label: 'Relationship' },
          { key: 'phoneNumber', label: 'Phone Number' },
          { key: 'email', label: 'Email Address' },
        ]"
        :key="fieldIndex"
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
            mdi-numeric-{{ refIndex + 1 }}-circle
          </v-icon>
          <strong>{{ field.label + ': ' }}</strong> {{ reference[field.key] }}
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
  characterReferences: CharacterReferenceType[]
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
