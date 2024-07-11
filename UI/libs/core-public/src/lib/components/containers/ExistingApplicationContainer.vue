<template>
  <v-container>
    <v-card>
      <v-form
        ref="form"
        v-model="valid"
      >
        <v-card-title class="justify-center">
          Please fill in this information to help us match your
          application/permit.
        </v-card-title>

        <v-card-text>
          <v-row>
            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="user.firstName"
                :rules="[v => !!v || $t('First Name cannot be blank')]"
                label="First Name"
                color="primary"
                outlined
              />
            </v-col>

            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="user.lastName"
                :rules="[v => !!v || $t('Last Name cannot be blank')]"
                label="Last Name"
                color="primary"
                outlined
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="user.middleName"
                label="Middle Name"
                color="primary"
                outlined
              />
            </v-col>

            <v-col
              cols="12"
              md="6"
            >
              <v-menu
                v-model="menu"
                :close-on-content-click="false"
                transition="scale-transition"
                offset-y
                min-width="auto"
              >
                <template #activator="{ on, attrs }">
                  <v-text-field
                    v-model="user.dateOfBirth"
                    :label="$t('Date of Birth')"
                    :rules="[v => !!v || $t('Date of birth is required')]"
                    hint="Click the month and year at the top of the calendar to change the year"
                    prepend-inner-icon="mdi-calendar"
                    color="primary"
                    persistent-hint
                    v-bind="attrs"
                    v-on="on"
                    outlined
                    readonly
                  />
                </template>

                <v-date-picker
                  v-model="user.dateOfBirth"
                  color="primary"
                  no-title
                  scrollable
                />
              </v-menu>
            </v-col>
          </v-row>

          <v-row>
            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="user.driversLicenseNumber"
                :rules="[
                  v => !!v || $t('Drivers License Number cannot be blank'),
                ]"
                label="Drivers License Number"
                color="primary"
                outlined
              />
            </v-col>

            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="user.permitNumber"
                label="Optional Agency License Number"
                color="primary"
                outlined
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-alert
                type="warning"
                outlined
              >
                <span
                  :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
                >
                  Please upload only the front side of your drivers license. If
                  you're also uploading your permit, include only the front
                  side.
                </span>
              </v-alert>
            </v-col>
          </v-row>

          <v-row class="mb-6">
            <v-col
              cols="12"
              md="6"
            >
              <FileUploadContainer
                :accepted-formats="'image/png, image/jpeg'"
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
              md="6"
            >
              <FileUploadContainer
                :accepted-formats="'image/png, image/jpeg'"
                :document-label="'Photo of CCW Permit'"
                :is-loading="loadingStates.CCWPermit"
                @file-opening="loadingStates.CCWPermit = true"
                @file-opened="loadingStates.CCWPermit = false"
                :uploaded-documents="user.uploadedDocuments"
                :filter-document-type="'CCWPermit'"
                @upload-files="files => handleMultiInput(files, 'CCWPermit')"
                @delete-file="name => deleteFile(name)"
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-alert
                outlined
                type="info"
              >
                <span
                  :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
                >
                  If you have an existing appointment and know your appointment
                  date and time please fill out the information below. This
                  information will only be used to assist licensing in matching
                  your information and does not schedule an appointment that
                  does not already exist.
                </span>
              </v-alert>
            </v-col>
          </v-row>

          <v-row>
            <v-col
              cols="12"
              md="6"
            >
              <v-menu
                v-model="appointmentMenu"
                :close-on-content-click="false"
                transition="scale-transition"
                offset-y
                min-width="auto"
              >
                <template #activator="{ on, attrs }">
                  <v-text-field
                    v-model="user.appointmentDate"
                    :label="$t('Appointment Date')"
                    prepend-inner-icon="mdi-calendar"
                    color="primary"
                    v-bind="attrs"
                    v-on="on"
                    outlined
                    readonly
                  />
                </template>

                <v-date-picker
                  v-model="user.appointmentDate"
                  color="primary"
                  no-title
                  scrollable
                />
              </v-menu>
            </v-col>

            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="user.appointmentTime"
                append-icon="mdi-clock-time-four-outline"
                label="Appointment Time"
                type="time"
                color="primary"
                clearable
                outlined
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-alert
                outlined
                type="info"
              >
                <span
                  :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
                >
                  Application lookup may take some time, please check back soon!
                </span>
              </v-alert>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-actions>
          <v-spacer />

          <v-btn
            @click="$router.push(Routes.HOME_ROUTE_PATH)"
            color="error"
          >
            Cancel
          </v-btn>

          <v-btn
            @click="handleLinkRequest"
            :disabled="!valid"
            color="primary"
          >
            Submit for Review
          </v-btn>

          <v-spacer />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Endpoints from '@shared-ui/api/endpoints'
import FileUploadContainer from '@core-public/components/containers/FileUploadContainer.vue'
import Routes from '@core-public/router/routes'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { useUserStore } from '@shared-ui/stores/userStore'
import { computed, nextTick, onMounted, reactive, ref } from 'vue'

const userStore = useUserStore()
const themeStore = useThemeStore()
const router = useRouter()
const user = computed(() => userStore.userProfile)
const form = ref()
const valid = ref(false)
const menu = ref(false)
const appointmentMenu = ref(false)

const { mutate: updateUser } = useMutation(
  ['updateUserProfile'],
  async () => await userStore.updateUserProfile(user.value),
  {
    onSuccess: async () => {
      await userStore.getUser()
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
