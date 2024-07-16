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
                <ModifySignatureDialog></ModifySignatureDialog>
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
import { reactive } from 'vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
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

const state = reactive({
  files: [] as Array<{ formData; target }>,
  driverLicense: '',
  proofResidence: '',
  proofResidence2: '',
  military: '',
  citizenship: '',
  supporting: [] as Array<string>,
  nameChange: '',
  judicial: '',
  reserve: '',
  employment: '',
  eightHourSafetyCourse: '',
  signature: '',
  uploadSuccessful: true,
  application: [applicationStore.completeApplication],
})

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

const { mutate: updateMutation } = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication()
  },
  onSuccess: () => {
    for (let item of applicationStore.completeApplication.application
      .uploadedDocuments) {
      switch (item.documentType.toLowerCase()) {
        case 'driverlicense':
          state.driverLicense = item.name
          break
        case 'proofresidency':
          state.proofResidence = item.name
          break
        case 'proofresidency2':
          state.proofResidence2 = item.name
          break
        case 'militarydoc':
          state.military = item.name
          break
        case 'citizenship':
          state.citizenship = item.name
          break
        case 'supporting':
          state.supporting.push(item.name)
          break
        case 'namechange':
          state.nameChange = item.name
          break
        case 'judicial':
          state.judicial = item.name
          break
        case 'reserve':
          state.reserve = item.name
          break
        case 'employment':
          state.employment = item.name
          break
        case 'eighthoursafetycourse':
          state.eightHourSafetyCourse = item.name
          break
        case 'signature':
          state.signature = item.name
          break
        default:
          break
      }
    }

    state.files = []
  },
})
</script>
