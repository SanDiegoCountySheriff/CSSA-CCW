<template>
  <v-container fluid>
    <v-card flat>
      <v-card-title>
        Uploaded Documents
        <v-spacer></v-spacer>

        <template
          v-if="
            applicationStore.completeApplication.application.status ===
            ApplicationStatus.Approved
          "
        >
          <FileUploadDialog
            v-on="$listeners"
            title="Upload 8 Hour Safety Course"
            :enable-button="enableEightHourSafetyCourseButton"
            :eight-hour-safety-input="true"
          ></FileUploadDialog>
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
            :eight-hour-safety-input="false"
            title="Upload"
            class="ml-4"
          />
        </template>
      </v-card-title>

      <v-card-text>
        <v-data-table
          :items="uploadedDocuments"
          :headers="headers"
        >
          <template #[`item.uploadedDateTimeUtc`]="{ item }">
            <td>
              {{ formatDate(item.uploadedDateTimeUtc) }}&nbsp;{{
                formatTime(item.uploadedDateTimeUtc)
              }}
            </td>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts" setup>
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import FileUploadDialog from '@shared-ui/components/dialogs/FileUploadDialog.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

interface IFileUploadInfoSection {
  uploadedDocuments: UploadedDocType[]
  color: string
  enableButton: boolean
  enableEightHourSafetyCourseButton: boolean
}

withDefaults(defineProps<IFileUploadInfoSection>(), {
  enableEightHourSafetyCourseButton: false,
})
const applicationStore = useCompleteApplicationStore()
const headers = [
  { text: 'Name', value: 'name' },
  { text: 'Document Type', value: 'documentType' },
  {
    text: 'Uploaded On',
    value: 'uploadedDateTimeUtc',
  },
]
</script>
