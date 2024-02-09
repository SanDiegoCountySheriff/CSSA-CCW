<template>
  <div>
    <DocumentInfoSection />
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Currently uploaded files') }}
      </v-subheader>

      <v-row>
        <v-chip-group
          class="ml-5 mb-3"
          column
        >
          <v-chip
            v-for="(item, index) in completeApplication.uploadedDocuments"
            color="info"
            :key="index"
          >
            {{ item.documentType }}
          </v-chip>
        </v-chip-group>
      </v-row>

      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t('File Upload') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-card
            :loading="isLoading"
            class="dropzone-container"
            @drop="e => drop(e, 'DriverLicense')"
            @dragleave="dragLeave"
            @dragover="dragOver"
            v
            :style="
              isDragging
                ? 'border-color: green;'
                : isDriverLicenseRequired && !state.driverLicense
                ? 'border-color: red;'
                : state.driverLicense
                ? 'border-color: green'
                : ''
            "
          >
            <v-file-input
              class="hidden-input"
              id="driversLicenseInput"
              :loading="isLoading"
              outlined
              dense
              multiple
              ref="driver-license"
              show-size
              small-chips
              persistent-hint
              accept="image/png, image/jpeg, .pdf"
              :rules="driverLicenseRules"
              :label="$t('Driver License')"
              :hint="
                state.driverLicense
                  ? $t('Document has already been submitted')
                  : ''
              "
              @change="handleMultiInput($event, 'DriverLicense')"
              @input="fileMutation()"
              :prepend-icon="
                state.driverLicense
                  ? 'mdi-check-circle-outline'
                  : 'mdi-paperclip'
              "
            />
            <label
              for="driversLicenseInput"
              class="file-label"
            >
              <div v-if="isDragging">Release to drop files here.</div>
              <div v-else-if="state.driverLicense">
                Driver's License Uploaded Successfully
                <i class="mdi mdi-check-circle-outline"></i>
              </div>
              <div v-else>
                Drop Driver's License files here or <u>click here</u> to upload.
              </div>
            </label>
          </v-card>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-card
            :loading="isLoading"
            class="dropzone-container"
            @drop="e => drop(e, 'ProofResidency')"
            @dragleave="dragLeave"
            @dragover="dragOver"
            v
            :style="
              isDragging
                ? 'border-color: green;'
                : isDriverLicenseRequired && !state.proofResidence
                ? 'border-color: red;'
                : state.proofResidence
                ? 'border-color: green'
                : ''
            "
          >
            <v-file-input
              class="hidden-input"
              id="proofResidencyLabel"
              :loading="isLoading"
              outlined
              dense
              multiple
              show-size
              small-chips
              persistent-hint
              :rules="proofOfResidenceRules"
              :hint="
                state.proofResidence
                  ? $t('Document has already been submitted')
                  : ''
              "
              accept="image/png, image/jpeg, .pdf"
              :label="$t('Proof of Residence 1')"
              @change="handleMultiInput($event, 'ProofResidency')"
              @input="fileMutation()"
              :prepend-icon="
                state.proofResidence
                  ? 'mdi-check-circle-outline'
                  : 'mdi-paperclip'
              "
            />
            <label
              for="proofResidencyLabel"
              class="file-label"
            >
              <div v-if="isDragging">Release to drop files here.</div>
              <div v-else-if="state.proofResidence">
                Proof of Residency Uploaded Successfully
                <i class="mdi mdi-check-circle-outline"></i>
              </div>
              <div v-else>
                Drop Proof of Residency files here or <u>click here</u> to
                upload.
              </div>
            </label>
          </v-card>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            :loading="isLoading"
            outlined
            show-size
            dense
            multiple
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :rules="proofOfResidence2Rules"
            :label="$t('Proof of Residence 2')"
            :hint="
              state.proofResidence2
                ? $t('Document has already been submitted')
                : ''
            "
            @change="handleMultiInput($event, 'ProofResidency2')"
            @input="fileMutation()"
            :prepend-icon="
              state.proofResidence2
                ? 'mdi-check-circle-outline'
                : 'mdi-paperclip'
            "
          >
          </v-file-input>
        </v-col>
      </v-row>

      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Military') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            multiple
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Military Document')"
            :hint="
              state.military ? $t('Document has already been submitted') : ''
            "
            @change="handleMultiInput($event, 'MilitaryDoc')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.military"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>

      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Not born in US') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            multiple
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Citizenship Documents')"
            :hint="
              state.citizenship ? $t('Document has already been submitted') : ''
            "
            @change="handleMultiInput($event, 'Citizenship')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.citizenship"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>

      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Supporting Documents') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            dense
            show-size
            small-chips
            multiple
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :hint="
              state.supporting.length > 0
                ? $t('Documents has already been submitted')
                : ''
            "
            :label="$t('Supporting Documents')"
            @change="handleMultiInput($event, 'Supporting')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.supporting.length > 0"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>

      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Legal name change') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            multiple
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :hint="
              state.nameChange ? $t('Document has already been submitted') : ''
            "
            :label="$t('Name change documents')"
            @change="handleMultiInput($event, 'NameChange')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.nameChange"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>

      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' License Type Documents') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            dense
            multiple
            show-size
            small-chips
            persistent-hint
            :hint="
              state.judicial ? $t('Document has already been submitted') : ''
            "
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Judicial documents')"
            @change="handleMultiInput($event, 'Judicial')"
            :rules="judicialValidationRule"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.judicial"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            multiple
            small-chips
            persistent-hint
            :hint="
              state.reserve ? $t('Document has already been submitted') : ''
            "
            accept="image/png, image/jpeg "
            :label="$t('Reserve documents')"
            @change="handleMultiInput($event, 'Reserve')"
            :rules="reserveValidationRule"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.reserve"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>

      <v-divider />
    </v-form>

    <v-progress-circular
      v-if="!state.uploadSuccessful"
      color="primary"
      indeterminate
    />
    <FormButtonContainer
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import DocumentInfoSection from '@shared-ui/components/info-sections/DocumentInfoSection.vue'
import Endpoints from '@shared-ui/api/endpoints'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { computed, onMounted, reactive, ref, watch } from 'vue'

const applicationStore = useCompleteApplicationStore()
const completeApplication = applicationStore.completeApplication.application

interface ISecondFormStepTwoProps {
  value: CompleteApplication
}

const props = defineProps<ISecondFormStepTwoProps>()
const emit = defineEmits([
  'input',
  'handle-continue',
  'handle-save',
  'update-step-six-valid',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const applicationType = computed(() => model.value.application.applicationType)

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
  uploadSuccessful: true,
})

const judicialValidationRule = computed(() => {
  if (applicationType.value === 'judicial') {
    return [
      v =>
        Boolean(v) || state.judicial.length || 'Judicial Document is required',
    ]
  }

  return []
})

const reserveValidationRule = computed(() => {
  if (applicationType.value === 'reserve') {
    return [
      v => Boolean(v) || state.reserve.length || 'Reserve Document is required',
    ]
  }

  return []
})

const form = ref()
const valid = ref(false)
const isDragging = ref(false)

const driverLicenseRules = computed(() => {
  const documentDriverLicense = completeApplication.uploadedDocuments.some(
    obj => {
      return obj.documentType === 'DriverLicense'
    }
  )

  return [documentDriverLicense || "Driver's license is required"]
})

const isDriverLicenseRequired = computed(() => {
  return driverLicenseRules[0] !== true
})

const proofOfResidenceRules = computed(() => {
  const proofOfResidence = completeApplication.uploadedDocuments.some(obj => {
    return obj.documentType === 'ProofResidency'
  })

  return [proofOfResidence || 'Proof of Residence is required']
})

const proofOfResidence2Rules = computed(() => {
  const proofOfResidence2 = completeApplication.uploadedDocuments.some(obj => {
    return obj.documentType === 'ProofResidency2'
  })

  return [proofOfResidence2 || 'Proof of Residence is required']
})

const { isLoading, mutate: fileMutation } = useMutation({
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
        case 'signature':
          break
        default:
          break
      }
    }

    state.files = []
  },
})

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
  state.files.forEach(file => {
    const newFileName = `${completeApplication.personalInfo.lastName}_${completeApplication.personalInfo.firstName}_${file.target}`

    axios
      .post(
        `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
        file.formData
      )
      .catch(e => {
        window.console.warn(e)
        Promise.reject()
      })

    const uploadDoc: UploadedDocType = {
      documentType: file.target.split('_').shift(),
      name: `${newFileName}`,
      uploadedBy: completeApplication.userEmail,
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    }

    completeApplication.uploadedDocuments.push(uploadDoc)
    updateMutation()
  })
}

function drop(e, inputName) {
  e.preventDefault()
  const files = Array.from(e.dataTransfer.files)

  handleMultiInput(files, inputName)
  isDragging.value = false
}

function dragOver(e) {
  e.preventDefault()
  isDragging.value = true
}

function dragLeave(e) {
  isDragging.value = false
}

function handleContinue() {
  fileMutation()
  emit('update-step-six-valid', valid.value)
  emit('handle-continue')
}

function handleSave() {
  fileMutation()
  emit('update-step-six-valid', valid.value)
  emit('handle-save')
}

onMounted(() => {
  state.driverLicense = ''
  state.proofResidence = ''
  state.proofResidence2 = ''
  state.military = ''
  state.citizenship = ''
  state.supporting = []
  state.nameChange = ''
  state.judicial = ''
  state.reserve = ''

  for (let item of completeApplication.uploadedDocuments) {
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
      case 'signature':
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

<style>
.dropzone-container {
  width: 95%;
  height: 42px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  background: #f2f2f2;
  border: 2px solid #c7c7c7;
  border-radius: 8px;
  margin-left: 10px;
}

.hidden-input {
  opacity: 0;
  overflow: hidden;
  position: absolute;
  width: 1px;
  height: 1px;
}

.file-label {
  font-size: 20px;
  display: block;
  cursor: pointer;
}
</style>
