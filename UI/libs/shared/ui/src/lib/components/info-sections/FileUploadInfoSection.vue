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
          <ModifySignatureDialog
            v-if="!uploadedDocuments.some(x => x.documentType === 'Signature')"
            v-on="$listeners"
            title="Add Signature"
          ></ModifySignatureDialog>
          <FileUploadDialog
            v-on="$listeners"
            title="Upload"
            :eight-hour-safety-input="false"
            :enable-button="enableButton"
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
            <td>
              <v-container
                ml-12
                v-if="item.documentType === 'Signature'"
              >
                <ModifySignatureDialog
                  title="Modify Signature"
                  v-on="$listeners"
                ></ModifySignatureDialog>
              </v-container>
            </td>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts" setup>
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
// eslint-disable-next-line sort-imports
import FileUploadDialog from '@shared-ui/components/dialogs/FileUploadDialog.vue'
import ModifySignatureDialog from '@shared-ui/components/dialogs/ModifySignatureDialog.vue'
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

const applicationStore = useCompleteApplicationStore()

withDefaults(defineProps<IFileUploadInfoSection>(), {
  enableEightHourSafetyCourseButton: false,
})

const headers = [
  { text: 'Name', value: 'name' },
  { text: 'Document Type', value: 'documentType' },
  {
    text: 'Uploaded On',
    value: 'uploadedDateTimeUtc',
  },
]
</script>
