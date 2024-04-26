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
                  <div class="display-1 text-center mb-4">Link Application</div>
                </v-col>
              </v-row>

              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    v-model="user.firstName"
                    :rules="[v => !!v || $t('First Name cannot be blank')]"
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
                    :rules="[v => !!v || $t('Last Name cannot be blank')]"
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
                    :rules="[v => !!v || $t('Date of birth is required')]"
                    :label="$t('Date of birth')"
                    type="date"
                    append-icon="mdi-calendar"
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
                    :rules="[
                      v => !!v || $t('Drivers License Number cannot be blank'),
                    ]"
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
                    label="Optional Agency License Number"
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
                    :uploaded-documents="user.uploadedDocuments"
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
                    :is-loading="loadingStates.CCWPermit"
                    @file-opening="loadingStates.CCWPermit = true"
                    @file-opened="loadingStates.CCWPermit = false"
                    :uploaded-documents="user.uploadedDocuments"
                    :filter-document-type="'CCWPermit'"
                    @upload-files="
                      files => handleMultiInput(files, 'CCWPermit')
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
                Application look-up may take some time, please check back soon!
                We highly recommend giving us information about your appointment
                date if you have one so we can expedite the process for you.
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
                          @click="$router.push(Routes.HOME_ROUTE_PATH)"
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
                          :disabled="!valid"
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
            </v-form>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import Endpoints from '@shared-ui/api/endpoints'
import FileUploadContainer from '@core-public/components/containers/FileUploadContainer.vue'
import Routes from '@core-public/router/routes'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { useUserStore } from '@shared-ui/stores/userStore'
import { computed, nextTick, onMounted, reactive, ref } from 'vue'

const userStore = useUserStore()
const router = useRouter()
const user = computed(() => userStore.userProfile)
const form = ref()
const valid = ref(false)

const { mutate: updateUser } = useMutation(
  ['createUserProfile'],
  async () => await userStore.putCreateUserApi(user.value),
  {
    onSuccess: async () => {
      await userStore.getUserApi()
    },
  }
)

function handleLinkRequest() {
  user.value.isPendingReview = true
  updateUser()
  router.push({
    path: Routes.HOME_ROUTE_PATH,
  })
}

const state = reactive({
  files: [] as Array<{ formData; target }>,
  driverLicense: '',
  CCWPermit: '',
})

const driverLicenseRules = computed(() => {
  const documentDriverLicense = user.value.uploadedDocuments.some(
    obj => obj.documentType === 'DriverLicense'
  )

  return [() => documentDriverLicense || "Driver's License is Required"]
})

const loadingStates = reactive({
  DriverLicense: false,
  CCWPermit: false,
})

const { mutate: fileMutation } = useMutation({
  mutationFn: handleFileUpload,
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
  const targetPrefix = `${target}_`

  const indexes = user.value.uploadedDocuments
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
    const newFileName = `${file.target}`

    try {
      await axios.post(
        `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
        file.formData
      )

      const uploadDoc: UploadedDocType = {
        documentType: file.target.split('_').shift(),
        name: `${newFileName}`,
        uploadedBy: user.value.email,
        uploadedDateTimeUtc: new Date().toISOString(),
      }

      user.value.uploadedDocuments.push(uploadDoc)
    } catch (e) {
      window.console.warn(e)
    }
  }

  documentTypes.forEach(type => (loadingStates[type] = false))

  validateForm()
}

async function deleteFile(name) {
  const documentToDelete = user.value.uploadedDocuments.find(
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

    const updatedDocuments = user.value.uploadedDocuments.filter(
      doc => doc.name !== name
    )

    user.value.uploadedDocuments = updatedDocuments

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

  for (let item of user.value.uploadedDocuments) {
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
})
</script>
