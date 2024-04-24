<template>
  <div>
    <v-container fluid>
      <v-row
        align="center"
        justify="center"
      >
        <v-col
          cols="12"
          md="10"
          lg="8"
        >
          <v-card
            class="pa-5"
            elevation="2"
          >
            <v-form
              ref="form"
              v-model="valid"
            >
              <v-row>
                <v-col>
                  <div class="display-1 text-center mb-4">Existing License</div>
                </v-col>
              </v-row>

              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    v-model="user.firstName"
                    label="First Name"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    v-model="user.lastName"
                    label="Last Name"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
              </v-row>

              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    v-model="user.middleName"
                    label="Middle Name"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    v-model="user.dateOfBirth"
                    :label="$t('Date of birth')"
                    type="date"
                    append-icon="mdi-calendar"
                    persistent-hint
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
              </v-row>

              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    v-model="user.driversLicenseNumber"
                    label="Drivers License Number"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>

                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    v-model="user.permitNumber"
                    label="Optional CCW Permit Number"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                  class="mb-4"
                >
                  <FileUploadContainer
                    :accepted-formats="'image/png, image/jpeg, application/pdf'"
                    :document-label="'Photo of Drivers License'"
                    :is-loading="loadingStates.DriverLicense"
                    @file-opening="loadingStates.DriverLicense = true"
                    @file-opened="loadingStates.DriverLicense = false"
                    :rules="driverLicenseRules"
                    :uploaded-documents="completeApplication.uploadedDocuments"
                    :filter-document-type="'DriverLicense'"
                    @upload-files="
                      files => handleMultiInput(files, 'DriverLicense')
                    "
                    @delete-file="name => deleteFile(name)"
                  />
                </v-col>
                <v-col
                  cols="12"
                  sm="6"
                  class="mb-4"
                >
                  <FileUploadContainer
                    :accepted-formats="'image/png, image/jpeg, application/pdf'"
                    :document-label="'Photo of CCW Permit'"
                    :is-loading="loadingStates.DriverLicense"
                    @file-opening="loadingStates.DriverLicense = true"
                    @file-opened="loadingStates.DriverLicense = false"
                    :rules="ccwPermitRules"
                    :uploaded-documents="completeApplication.uploadedDocuments"
                    :filter-document-type="'CCWPermit'"
                    @upload-files="
                      files => handleMultiInput(files, 'DriverLicense')
                    "
                    @delete-file="name => deleteFile(name)"
                  />
                </v-col>
              </v-row>
              <v-row class="justify-center">
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    v-model="user.appointmentDate"
                    :label="$t('Appointment Date')"
                    type="date"
                    append-icon="mdi-calendar"
                    persistent-hint
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
              </v-row>

              <v-container class="mb-10">
                <v-text>
                  Application look-up may take some time, please check back
                  soon! We highly recommend giving us information about your
                  appointment date if you have one so we can expedite the
                  process for you.
                </v-text>
              </v-container>

              <v-row class="justify-center">
                <v-card-actions class="d-flex flex-column align-center">
                  <v-container
                    class="px-0"
                    fluid
                  >
                    <v-row>
                      <v-col
                        cols="12"
                        sm="8"
                        md="6"
                      >
                        <v-btn
                          color="error"
                          @click="cancelForm"
                          block
                        >
                          Cancel
                        </v-btn>
                      </v-col>
                      <v-col
                        cols="12"
                        sm="8"
                        md="6"
                      >
                        <v-btn
                          color="primary"
                          @click="handleLinkRequest"
                          block
                        >
                          Submit for Review
                        </v-btn>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-actions>
              </v-row>
              <!-- 
              <FormButtonContainer
                :valid="valid"
                @continue="handleContinue"
                @save="handleSave"
              /> -->
            </v-form>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import DocumentInfoSection from '@shared-ui/components/info-sections/DocumentInfoSection.vue'
import Endpoints from '@shared-ui/api/endpoints'
import FileUploadContainer from '@core-public/components/containers/FileUploadContainer.vue'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useUserStore } from '@shared-ui/stores/userStore'
import {
  ApplicationType,
  CompleteApplication,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, onMounted, reactive, ref, watch } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'

const authStore = useAuthStore()
const userStore = useUserStore()
const user = computed(() => userStore.userProfile)
const applicationStore = useCompleteApplicationStore()
const completeApplication = applicationStore.completeApplication.application

interface ISecondFormStepTwoProps {
  value: CompleteApplication
}
const props = defineProps<ISecondFormStepTwoProps>()
const brandStore = useBrandStore()
const emit = defineEmits([
  'input',
  'handle-continue',
  'handle-save',
  'update-step-six-valid',
])

const { mutate: createUser } = useMutation(
  ['createUserProfile'],
  async () => await userStore.putCreateUserApi(user.value),
  {
    onSuccess: async () => {
      await userStore.getUserApi()
    },
  }
)

function handleLinkRequest() {
  createUser()
}

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const state = reactive({
  files: [] as Array<{ formData; target }>,
  driverLicense: '',
  CCWPermit: '',
})

const form = ref()
const valid = ref(false)

const driverLicenseRules = computed(() => {
  const documentDriverLicense = completeApplication.uploadedDocuments.some(
    obj => obj.documentType === 'DriverLicense'
  )

  return [() => documentDriverLicense || "Driver's License is Required"]
})

const ccwPermitRules = computed(() => {
  const documentCCWPermit = completeApplication.uploadedDocuments.some(obj => {
    return obj.documentType === 'CCWPermit'
  })

  return [() => documentCCWPermit]
})

const loadingStates = reactive({
  DriverLicense: false,
  CCWPermit: false,
})

const { mutate: fileMutation } = useMutation({
  mutationFn: handleFileUpload,
})

const { mutate: updateMutation } = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication()
  },
  onSuccess: () => {
    for (let item of completeApplication.uploadedDocuments) {
      switch (item.documentType.toLowerCase()) {
        case 'driverlicense':
          state.driverLicense = item.name
          break
        case 'ccwPermit':
          break
        default:
          break
      }
    }

    state.files = []
    validateForm()
  },
})

function validateForm() {
  nextTick(() => {
    if (form.value) {
      form.value.validate()
    }
  })
}

function handleMultiInput(event, target: string) {
  if (!event || event.length === 0) {
    return
  }

  state.files = []

  let startIndex = getNextFileIndex(target)

  event.forEach((file: File) => {
    const formData = new FormData()

    formData.append('fileToUpload', file)
    const fileObject = {
      formData,
      target: `${target}_${startIndex.toString()}`,
    }

    state.files.push(fileObject)
    startIndex++
  })
  fileMutation()
  validateForm()
}

function getNextFileIndex(target: string): number {
  const targetPrefix = `${completeApplication.personalInfo.lastName}_${completeApplication.personalInfo.firstName}_${target}_`

  const indexes = completeApplication.uploadedDocuments
    .filter(doc => doc.name.startsWith(targetPrefix))
    .map(doc => {
      const parts = doc.name.split('_')

      return parseInt(parts[parts.length - 1], 10)
    })

  if (!indexes.length) return 1

  const maxIndex = Math.max(...indexes)

  return maxIndex + 1
}

async function handleFileUpload() {
  const documentTypes = new Set(
    state.files.map(file => file.target.split('_').shift())
  )

  documentTypes.forEach(type => (loadingStates[type] = true))

  for (let file of state.files) {
    const newFileName = `${completeApplication.personalInfo.lastName}_${completeApplication.personalInfo.firstName}_${file.target}`

    try {
      await axios.post(
        `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
        file.formData
      )

      const uploadDoc: UploadedDocType = {
        documentType: file.target.split('_').shift(),
        name: `${newFileName}`,
        uploadedBy: completeApplication.userEmail,
        uploadedDateTimeUtc: new Date().toISOString(),
      }

      completeApplication.uploadedDocuments.push(uploadDoc)
    } catch (e) {
      window.console.warn(e)
    }
  }

  documentTypes.forEach(type => (loadingStates[type] = false))

  updateMutation()
}

async function deleteFile(name) {
  const documentToDelete = completeApplication.uploadedDocuments.find(
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
    await axios.delete(
      `${Endpoints.DELETE_DOCUMENT_FILE_PUBLIC_ENDPOINT}?applicantFileName=${name}`
    )

    const updatedDocuments = completeApplication.uploadedDocuments.filter(
      doc => doc.name !== name
    )

    completeApplication.uploadedDocuments = updatedDocuments

    updateMutation()

    validateForm()
  } finally {
    if (documentType && loadingStates[documentType] !== undefined) {
      loadingStates[documentType] = false
    }
  }
}

onMounted(() => {
  state.driverLicense = ''
  state.CCWPermit = ''

  for (let item of completeApplication.uploadedDocuments) {
    switch (item.documentType.toLowerCase()) {
      case 'driverlicense':
        state.driverLicense = item.name
        break
      case 'CCWPermit':
        state.CCWPermit = item.name
        break
      default:
        break
    }
  }

  if (form.value) {
    form.value.validate()
  }

  emit('update-step-six-valid', valid.value)
})

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-six-valid', newValue)
  }
})
</script>
