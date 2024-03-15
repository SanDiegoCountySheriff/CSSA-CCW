<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title>Modify Supporting Documents</v-card-title>

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
              :filter-document-type="'ModifyName'"
              @file-opening="loadingStates.ModifyName = true"
              @file-opened="loadingStates.ModifyName = false"
              @upload-files="files => handleMultiInput(files, 'ModifyName')"
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
              :filter-document-type="'ModifyAddress'"
              @file-opening="loadingStates.ModifyAddress = true"
              @file-opened="loadingStates.ModifyAddress = false"
              @upload-files="files => handleMultiInput(files, 'ModifyAddress')"
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
              :filter-document-type="'ModifyWeapons'"
              @file-opening="loadingStates.ModifyWeapons = true"
              @file-opened="loadingStates.ModifyWeapons = false"
              @upload-files="files => handleMultiInput(files, 'ModifyWeapons')"
              @delete-file="weapon => deleteFile(weapon)"
            />
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>

    <FormButtonContainer
      @continue="handleContinue"
      @save="handleSave"
      :valid="valid"
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
import { computed, nextTick, onMounted, reactive, ref } from 'vue'

interface ModifyNameProps {
  application: CompleteApplication
  modifyingName: boolean
  modifyingAddress: boolean
  modifyingWeapons: boolean
}

const props = defineProps<ModifyNameProps>()
const emit = defineEmits(['handle-continue', 'handle-save'])

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
      return obj.documentType === 'ModifyName'
    }
  )

  return [() => modifyName || 'Proof of name change is required']
})

const addressValidationRules = computed(() => {
  const modifyName = props.application.application.uploadedDocuments.some(
    obj => {
      return obj.documentType === 'ModifyAddress'
    }
  )

  return [() => modifyName || 'Proof of address change is required']
})

const weaponValidationRules = computed(() => {
  const modifyName = props.application.application.uploadedDocuments.some(
    obj => {
      return obj.documentType === 'ModifyWeapons'
    }
  )

  return [() => modifyName || 'Updated Weapons Safety documents are required']
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
        case 'modifyname':
          state.name = item.name
          break
        case 'modifyaddress':
          state.address = item.name
          break
        case 'modifyweapon':
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
  const targetPrefix = `${props.application.application.personalInfo.lastName}_${props.application.application.personalInfo.firstName}_${target}_`

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

  const documentType = documentToDelete.documentType

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

  documentTypes.forEach(type => (loadingStates[type] = true))

  for (let file of state.files) {
    const newFileName = `${props.application.application.personalInfo.lastName}_${props.application.application.personalInfo.firstName}_${file.target}`

    try {
      await axios.post(
        `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
        file.formData
      )

      const uploadDoc: UploadedDocType = {
        documentType: file.target.split('_').shift(),
        name: `${newFileName}`,
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

  documentTypes.forEach(type => (loadingStates[type] = false))

  updateMutation()
}
</script>
