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
              <v-btn
                v-if="item.documentType === 'Signature'"
                color="primary"
                text
                @click="handleModifyDocument"
              >
                {{ $t('Edit Signature') }}
              </v-btn>
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
import { useRouter } from 'vue-router/composables'
// eslint-disable-next-line sort-imports
import axios from 'axios'
// eslint-disable-next-line sort-imports
import Endpoints from '@shared-ui/api/endpoints'
import FileUploadDialog from '@shared-ui/components/dialogs/FileUploadDialog.vue'
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'
// eslint-disable-next-line @nrwl/nx/enforce-module-boundaries, no-restricted-imports, sort-imports
import Routes from '@core-public/router/routes'

interface IFileUploadInfoSection {
  uploadedDocuments: UploadedDocType[]
  color: string
  enableButton: boolean
  enableEightHourSafetyCourseButton: boolean
}

const loadingStates = reactive({
  DriverLicense: false,
  ProofResidency: false,
  ProofResidency2: false,
  MilitaryDoc: false,
  Citizenship: false,
  Supporting: false,
  NameChange: false,
  Judicial: false,
  Reserve: false,
  Employment: false,
  EightHourSafetyCourse: false,
  Signature: false,
})

const router = useRouter()
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

function handleModifyDocument() {
  applicationStore.completeApplication.application.uploadedDocuments.forEach(
    file => {
      if (file.documentType === 'Signature') {
        deleteFile(file.name).then(() => {
          viewSignatureSection()
        })
      }
    }
  )
}

function viewSignatureSection() {
  router.push({
    path: Routes.FORM_ROUTE_PATH,
    query: {
      applicationId: state.application[0].id,
      isComplete: state.application[0].application.isComplete.toString(),
    },
  })
}

async function deleteFile(name) {
  const documentToDelete =
    applicationStore.completeApplication.application.uploadedDocuments.find(
      doc => doc.name === name
    )

  if (!documentToDelete) {
    return
  }

  const documentType = documentToDelete.documentType

  if (documentType && loadingStates[documentType] !== undefined) {
    loadingStates[documentType] = true
  }

  try {
    await axios
      .delete(
        `${Endpoints.DELETE_DOCUMENT_FILE_PUBLIC_ENDPOINT}?applicantFileName=${name}`
      )
      .then(() => {
        applicationStore.completeApplication.application.uploadedDocuments.pop()
      })

    const updatedDocuments =
      applicationStore.completeApplication.application.uploadedDocuments.filter(
        doc => doc.name !== name
      )

    applicationStore.completeApplication.application.uploadedDocuments =
      updatedDocuments

    updateMutation()
  } finally {
    if (documentType && loadingStates[documentType] !== undefined) {
      loadingStates[documentType] = false
    }
  }
}
</script>
