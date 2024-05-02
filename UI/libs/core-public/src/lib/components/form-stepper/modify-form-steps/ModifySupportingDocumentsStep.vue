<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title>Modify Supporting Documents</v-card-title>

      <v-alert
        v-if="isNothingModified"
        type="warning"
        outlined
      >
        You haven't modified anything!
      </v-alert>

      <v-card-text>
        <v-row>
          <v-col
            v-if="modifyingName"
            cols="4"
          >
            <FileUploadContainer
              :uploaded-documents="application.application.uploadedDocuments"
              :accepted-formats="'image/png, image/jpeg, application/pdf'"
              :document-label="'Name Change Documents'"
              :is-loading="loadingStates.ModifyName"
              :rules="nameValidationRules"
              :filter-document-type="`ModifyName-${application.application.modificationNumber}`"
              @file-opening="loadingStates.ModifyName = true"
              @file-opened="loadingStates.ModifyName = false"
              @upload-files="
                files =>
                  handleMultiInput(
                    files,
                    `ModifyName-${application.application.modificationNumber}`
                  )
              "
              @delete-file="name => deleteFile(name)"
            />
          </v-col>

          <v-col
            v-if="modifyingAddress"
            cols="4"
          >
            <FileUploadContainer
              :uploaded-documents="application.application.uploadedDocuments"
              :accepted-formats="'image/png, image/jpeg, application/pdf'"
              :document-label="'Address Change Documents'"
              :is-loading="loadingStates.ModifyAddress"
              :rules="addressValidationRules"
              :filter-document-type="`ModifyAddress-${application.application.modificationNumber}`"
              @file-opening="loadingStates.ModifyAddress = true"
              @file-opened="loadingStates.ModifyAddress = false"
              @upload-files="
                files =>
                  handleMultiInput(
                    files,
                    `ModifyAddress-${application.application.modificationNumber}`
                  )
              "
              @delete-file="address => deleteFile(address)"
            />
          </v-col>

          <v-col
            v-if="modifyingWeapons"
            cols="4"
          >
            <FileUploadContainer
              :uploaded-documents="application.application.uploadedDocuments"
              :accepted-formats="'image/png, image/jpeg, application/pdf'"
              :document-label="'Weapon Safety Course Documents'"
              :is-loading="loadingStates.ModifyWeapons"
              :rules="weaponValidationRules"
              :filter-document-type="`ModifyWeapons-${application.application.modificationNumber}`"
              @file-opening="loadingStates.ModifyWeapons = true"
              @file-opened="loadingStates.ModifyWeapons = false"
              @upload-files="
                files =>
                  handleMultiInput(
                    files,
                    `ModifyWeapons-${application.application.modificationNumber}`
                  )
              "
              @delete-file="weapon => deleteFile(weapon)"
            />
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>

    <FormButtonContainer
      :valid="valid && !isNothingModified && isUpdatingAllStepsComplete"
      :is-final-step="true"
      :is-modification="true"
      @continue="handleContinue"
      @save="handleSave"
      v-on="$listeners"
    />
  </div>
</template>

<script lang="ts" setup>
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import FileUploadContainer from '@core-public/components/containers/FileUploadContainer.vue'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import {
  computed,
  inject,
  nextTick,
  onMounted,
  reactive,
  ref,
  watch,
} from 'vue'

interface ModifyNameProps {
  application: CompleteApplication
  modifyingName: boolean
  modifyingAddress: boolean
  modifyingWeapons: boolean
}

const props = defineProps<ModifyNameProps>()
const emit = defineEmits([
  'handle-continue',
  'handle-save',
  'update-step-four-valid',
])

const isUpdatingAllStepsComplete: boolean | undefined =
  inject('allStepsComplete')

const applicationStore = useCompleteApplicationStore()
const form = ref()
const valid = ref(false)

const loadingStates = reactive({
  ModifyName: false,
  ModifyAddress: false,
  ModifyWeapons: false,
})

const state = reactive({
  files: [] as Array<{ formData; target }>,
  name: '',
  address: '',
  weapons: '',
  uploadSuccessful: true,
})

onMounted(() => {
  if (form.value) {
    form.value.validate()
  }
})

const nameValidationRules = computed(() => {
  const modifyName = props.application.application.uploadedDocuments.some(
    obj => {
      return (
        obj.documentType ===
        `ModifyName-${props.application.application.modificationNumber}`
      )
    }
  )

  return [() => modifyName || 'Proof of name change is required']
})

const addressValidationRules = computed(() => {
  const modifyName = props.application.application.uploadedDocuments.some(
    obj => {
      return (
        obj.documentType ===
        `ModifyAddress-${props.application.application.modificationNumber}`
      )
    }
  )

  return [() => modifyName || 'Proof of address change is required']
})

const weaponValidationRules = computed(() => {
  const modifyName = props.application.application.uploadedDocuments.some(
    obj => {
      return (
        obj.documentType ===
        `ModifyWeapons-${props.application.application.modificationNumber}`
      )
    }
  )

  return [() => modifyName || 'Updated Weapons Safety documents are required']
})

const isNothingModified = computed(() => {
  return (
    !props.modifyingName && !props.modifyingAddress && !props.modifyingWeapons
  )
})

const { mutate: fileMutation } = useMutation({
  mutationFn: handleFileUpload,
})

const { mutate: updateMutation } = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication()
  },
  onSuccess: () => {
    for (let item of props.application.application.uploadedDocuments) {
      switch (item.documentType.toLowerCase()) {
        case `modifyname-${props.application.application.modificationNumber}`:
          state.name = item.name
          break
        case `modifyaddress-${props.application.application.modificationNumber}`:
          state.address = item.name
          break
        case `modifyweapon-${props.application.application.modificationNumber}`:
          state.weapons = item.name
          break
        default:
          break
      }
    }

    state.files = []

    validateForm()
  },
})

function getNextFileIndex(target: string): number {
  const targetPrefix = `${target}_`

  const indexes = props.application.application.uploadedDocuments
    .filter(doc => doc.name.startsWith(targetPrefix))
    .map(doc => {
      const parts = doc.name.split('_')

      return parseInt(parts[parts.length - 1], 10)
    })

  if (!indexes.length) return 1

  const maxIndex = Math.max(...indexes)

  return maxIndex + 1
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

async function deleteFile(name) {
  const documentToDelete = props.application.application.uploadedDocuments.find(
    doc => doc.name === name
  )

  if (!documentToDelete) {
    return
  }

  const documentType = documentToDelete.documentType.split('-')[0]

  if (documentType && loadingStates[documentType] !== undefined) {
    loadingStates[documentType] = true
  }

  try {
    await axios.delete(
      `${Endpoints.DELETE_DOCUMENT_FILE_PUBLIC_ENDPOINT}?applicantFileName=${name}`
    )

    const updatedDocuments =
      props.application.application.uploadedDocuments.filter(
        doc => doc.name !== name
      )

    applicationStore.completeApplication.application.uploadedDocuments =
      updatedDocuments

    updateMutation()

    validateForm()
  } finally {
    if (documentType && loadingStates[documentType] !== undefined) {
      loadingStates[documentType] = false
    }
  }
}

function handleContinue() {
  fileMutation()
  emit('handle-continue')
}

function handleSave() {
  fileMutation()
  emit('handle-save')
}

function validateForm() {
  nextTick(() => {
    if (form.value) {
      form.value.validate()
    }
  })
}

async function handleFileUpload() {
  const documentTypes = new Set(
    state.files.map(file => file.target.split('_').shift())
  )

  documentTypes.forEach(type => (loadingStates[type.split('-')[0]] = true))

  for (let file of state.files) {
    try {
      await axios.post(
        `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${file.target}`,
        file.formData
      )

      const uploadDoc: UploadedDocType = {
        documentType: file.target.split('_').shift(),
        name: file.target,
        uploadedBy: props.application.application.userEmail,
        uploadedDateTimeUtc: new Date().toISOString(),
      }

      applicationStore.completeApplication.application.uploadedDocuments.push(
        uploadDoc
      )
    } catch (e) {
      window.console.warn(e)
    }
  }

  documentTypes.forEach(type => (loadingStates[type.split('-')[0]] = false))

  updateMutation()
}

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-four-valid', newValue)
  }
})
</script>
