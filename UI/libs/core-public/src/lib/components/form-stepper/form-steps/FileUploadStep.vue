<template>
  <div>
    <FormButtonContainer
      v-if="$vuetify.breakpoint.lgAndUp"
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
      v-on="$listeners"
    />

    <DocumentInfoSection />
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t('File Upload') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="12"
          lg="3"
          class="mb-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Drivers License'"
            :is-loading="loadingStates.DriverLicense"
            @file-opening="loadingStates.DriverLicense = true"
            @file-opened="loadingStates.DriverLicense = false"
            :rules="driverLicenseRules"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'DriverLicense'"
            @upload-files="files => handleMultiInput(files, 'DriverLicense')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>

        <v-col
          cols="12"
          lg="3"
          class="mb-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Proof of Residency'"
            :is-loading="loadingStates.ProofResidency"
            @file-opening="loadingStates.ProofResidency = true"
            @file-opened="loadingStates.ProofResidency = false"
            :rules="proofOfResidenceRules"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'ProofResidency'"
            @upload-files="files => handleMultiInput(files, 'ProofResidency')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>

        <v-col
          cols="12"
          lg="3"
          class="mb-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'2nd Proof of Residency'"
            :is-loading="loadingStates.ProofResidency2"
            @file-opening="loadingStates.ProofResidency2 = true"
            @file-opened="loadingStates.ProofResidency2 = false"
            :rules="proofOfResidence2Rules"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'ProofResidency2'"
            @upload-files="files => handleMultiInput(files, 'ProofResidency2')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>
        <v-col
          cols="12"
          lg="3"
          class="mb-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Military Documents'"
            :is-loading="loadingStates.MilitaryDoc"
            @file-opening="loadingStates.MilitaryDoc = true"
            @file-opened="loadingStates.MilitaryDoc = false"
            :rules="militaryDocRules"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'MilitaryDoc'"
            @upload-files="files => handleMultiInput(files, 'MilitaryDoc')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>
      </v-row>

      <v-divider />

      <v-row>
        <v-col
          cols="12"
          lg="3"
          class="mb-4 mt-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Citizenship Documents'"
            :is-loading="loadingStates.Citizenship"
            @file-opening="loadingStates.Citizenship = true"
            @file-opened="loadingStates.Citizenship = false"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'Citizenship'"
            @upload-files="files => handleMultiInput(files, 'Citizenship')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>
        <v-col
          cols="12"
          lg="3"
          class="mb-4 mt-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Supporting Documents'"
            :is-loading="loadingStates.Supporting"
            @file-opening="loadingStates.Supporting = true"
            @file-opened="loadingStates.Supporting = false"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'Supporting'"
            @upload-files="files => handleMultiInput(files, 'Supporting')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>
        <v-col
          cols="12"
          lg="3"
          class="mb-4 mt-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Legal Name Change Documents'"
            :is-loading="loadingStates.NameChange"
            @file-opening="loadingStates.NameChange = true"
            @file-opened="loadingStates.NameChange = false"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'NameChange'"
            @upload-files="files => handleMultiInput(files, 'NameChange')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>
        <v-col
          cols="12"
          lg="3"
          class="mb-4 mt-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Judicial Documents'"
            :is-loading="loadingStates.Judicial"
            @file-opening="loadingStates.Judicial = true"
            @file-opened="loadingStates.Judicial = false"
            :rules="judicialValidationRule"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'Judicial'"
            @upload-files="files => handleMultiInput(files, 'Judicial')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>
      </v-row>

      <v-divider />

      <v-row>
        <v-col
          cols="12"
          lg="3"
          class="mb-4 mt-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Reserve Documents'"
            :is-loading="loadingStates.Reserve"
            @file-opening="loadingStates.Reserve = true"
            @file-opened="loadingStates.Reserve = false"
            :rules="reserveValidationRule"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'Reserve'"
            @upload-files="files => handleMultiInput(files, 'Reserve')"
            @delete-file="name => deleteFile(name)"
          />
        </v-col>
        <v-col
          cols="12"
          lg="3"
          class="mb-4 mt-4"
        >
          <FileUploadContainer
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Firearm Safety Proficiency Certificate'"
            :is-loading="loadingStates.EightHourSafetyCourse"
            @file-opening="loadingStates.EightHourSafetyCourse = true"
            @file-opened="loadingStates.EightHourSafetyCourse = false"
            :rules="safetyCertificateRules"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'EightHourSafetyCourse'"
            @upload-files="
              files => handleMultiInput(files, 'EightHourSafetyCourse')
            "
            @delete-file="name => deleteFile(name)"
          />
        </v-col>
        <v-col
          cols="12"
          lg="3"
          class="mb-4 mt-4"
        >
          <FileUploadContainer
            v-if="brandStore.brand.employmentLicense"
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Employment Documents'"
            :is-loading="loadingStates.Employment"
            @file-opening="loadingStates.Employment = true"
            @file-opened="loadingStates.Employment = false"
            :rules="employmentValidationRule"
            :uploaded-documents="completeApplication.uploadedDocuments"
            :filter-document-type="'Employment'"
            @upload-files="files => handleMultiInput(files, 'Employment')"
            @delete-file="name => deleteFile(name)"
          />
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
      v-on="$listeners"
    />
  </div>
</template>

<script setup lang="ts">
import DocumentInfoSection from '@shared-ui/components/info-sections/DocumentInfoSection.vue'
import Endpoints from '@shared-ui/api/endpoints'
import FileUploadContainer from '@core-public/components/containers/FileUploadContainer.vue'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import {
  ApplicationStatus,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import {
  ApplicationType,
  CompleteApplication,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, onMounted, reactive, ref, watch } from 'vue'

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
  employment: '',
  uploadSuccessful: true,
})

const judicialValidationRule = computed(() => {
  if (
    applicationType.value === ApplicationType.Judicial ||
    applicationType.value === ApplicationType['Renew Judicial']
  ) {
    const documentJudicial = completeApplication.uploadedDocuments.some(
      obj => obj.documentType === 'Judicial'
    )

    return [() => documentJudicial || 'Judicial Document is required']
  }

  return []
})

const reserveValidationRule = computed(() => {
  if (
    applicationType.value === ApplicationType.Reserve ||
    applicationType.value === ApplicationType['Renew Reserve']
  ) {
    const documentReserve = completeApplication.uploadedDocuments.some(
      obj => obj.documentType === 'Reserve'
    )

    return [() => documentReserve || 'Reserve Document is required']
  }

  return []
})

const employmentValidationRule = computed(() => {
  if (
    applicationType.value === ApplicationType.Employment ||
    applicationType.value === ApplicationType['Renew Employment']
  ) {
    const documentEmployment = completeApplication.uploadedDocuments.some(
      obj => obj.documentType === 'Employment'
    )

    return [() => documentEmployment || 'Employment Document is required']
  }

  return []
})

const form = ref()
const valid = ref(false)

const driverLicenseRules = computed(() => {
  const documentDriverLicense = completeApplication.uploadedDocuments.some(
    obj => obj.documentType === 'DriverLicense'
  )

  return [
    () =>
      documentDriverLicense ||
      completeApplication.status === ApplicationStatus['Permit Delivered'] ||
      "Driver's License is Required",
  ]
})

const proofOfResidenceRules = computed(() => {
  const proofOfResidence = completeApplication.uploadedDocuments.some(obj => {
    return obj.documentType === 'ProofResidency'
  })

  return [
    () =>
      proofOfResidence ||
      completeApplication.status === ApplicationStatus['Permit Delivered'] ||
      'Proof of Residency is Required',
  ]
})

const proofOfResidence2Rules = computed(() => {
  const proofOfResidence2 = completeApplication.uploadedDocuments.some(obj => {
    return obj.documentType === 'ProofResidency2'
  })

  return [
    () =>
      proofOfResidence2 ||
      completeApplication.status === ApplicationStatus['Permit Delivered'] ||
      '2nd Proof of Residency is Required',
  ]
})

const militaryDocRules = computed(() => {
  const militaryStatus = completeApplication.citizenship.militaryStatus
  const addressState = completeApplication.currentAddress.state
  const issuingState = completeApplication.idInfo.issuingState

  if (
    militaryStatus === 'Active' &&
    (addressState !== 'California' || issuingState !== 'California')
  ) {
    const documentMilitaryDocument = completeApplication.uploadedDocuments.some(
      obj => obj.documentType === 'MilitaryDoc'
    )

    return [() => documentMilitaryDocument || 'Military Documents are Required']
  }

  return [() => true]
})

const isRenew = computed(() => {
  const appType = model.value.application.applicationType

  return (
    appType === ApplicationType['Renew Standard'] ||
    appType === ApplicationType['Renew Reserve'] ||
    appType === ApplicationType['Renew Judicial'] ||
    appType === ApplicationType['Renew Employment']
  )
})

const safetyCertificateRules = computed(() => {
  if (isRenew.value) {
    const documentSafetyCertificate =
      completeApplication.uploadedDocuments.some(
        obj => obj.documentType === 'EightHourSafetyCourse'
      )

    return [
      () =>
        documentSafetyCertificate ||
        'Please upload front and back of certificate',
    ]
  }

  return [() => true]
})

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

    const name = isRenew.value
      ? `${target}_Renew-${
          applicationStore.completeApplication.application.renewalNumber
        }_${startIndex.toString()}`
      : `${target}_${startIndex.toString()}`

    const fileObject = {
      formData,
      target: name,
    }

    state.files.push(fileObject)
    startIndex++
  })
  fileMutation()
  validateForm()
}

function getNextFileIndex(target: string): number {
  const targetPrefix = `${target}_`

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
    try {
      await axios.post(
        `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${file.target}`,
        file.formData
      )

      const uploadDoc: UploadedDocType = {
        documentType: file.target.split('_').shift(),
        name: file.target,
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
