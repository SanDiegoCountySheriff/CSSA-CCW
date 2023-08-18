<template>
  <v-container fluid>
    <v-card flat>
      <v-card-title>
        Uploaded Documents
        <v-spacer></v-spacer>

        <template #actions>
          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-btn
                v-if="
                  applicationStore.completeApplication.application.status ===
                  ApplicationStatus.Incomplete
                "
                icon
                @click="handleEditRequest"
                v-bind="attrs"
                v-on="on"
                :disable="!props.enableButton"
              >
                <v-icon color="primary"> mdi-square-edit-outline </v-icon>
              </v-btn>
            </template>
            {{ $t('Edit Section') }}
          </v-tooltip>
        </template>

        <template
          v-if="
            applicationStore.completeApplication.application.status !==
            ApplicationStatus.Incomplete
          "
        >
          <FileUploadDialog
            v-on="$listeners"
            :enable-button="enableButton"
          />
        </template>
      </v-card-title>

      <v-card-text>
        <v-data-table
          :items="props.uploadedDocuments"
          :headers="headers"
        >
          <template #[`item.uploadedDateTimeUtc`]="{ item }">
            <td>
              {{ formatDate(item.uploadedDateTimeUtc) }}&nbsp;{{
                formatTime(item.uploadedDateTimeUtc)
              }}
            </td>
          </template></v-data-table
        >
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts" setup>
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import FileUploadDialog from '@shared-ui/components/dialogs/FileUploadDialog.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

interface IFileUploadInfoSection {
  uploadedDocuments: UploadedDocType[]
  color: string
  enableButton: boolean
}

const props = defineProps<IFileUploadInfoSection>()
const router = useRouter()
const applicationStore = useCompleteApplicationStore()
const headers = [
  { text: 'Name', value: 'name' },
  { text: 'Document Type', value: 'documentType' },
  {
    text: 'Uploaded On',
    value: 'uploadedDateTimeUtc',
  },
]

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 8
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
