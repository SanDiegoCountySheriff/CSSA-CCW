<template>
  <v-card
    @click="triggerFileDialog"
    :loading="isLoading"
    :ripple="false"
    outlined
    elevation="2"
    :class="{
      'highlight-border': successBorder,
      'error-border': hasError && !isDragging,
      'consistent-border': !hasError && !isDragging && !isFileUploaded,
    }"
    :style="{
      borderColor:
        isDragging || hasError || isFileUploaded ? '' : 'var(--v-primary-base)',
    }"
    @dragover.prevent="dragOver"
    @dragleave.prevent="dragLeave"
    @drop.prevent="dropFiles"
  >
    <v-card-text class="card-text">
      <v-btn
        class="upload-trigger"
        text
      >
        <v-icon
          color="primary"
          class="mr-2"
        >
          mdi-cloud-upload-outline
        </v-icon>
        {{ documentLabel }}
      </v-btn>

      <v-file-input
        v-show="false"
        :disabled="isLoading"
        :rules="rules || []"
        @change="handleFiles"
        multiple
        :accept="acceptedFormats || ''"
        :label="documentLabel || ''"
        :show-size="true"
        outlined
        dense
        hide-details
      />

      <div
        class="file-chips"
        v-if="filteredDocuments.length > 0"
      >
        <v-chip
          v-for="(doc, index) in filteredDocuments"
          :key="index"
          class="file-chip"
          color="primary"
          @click.stop="
            openFile(doc.name), emit('file-opening', doc.documentType)
          "
          title="Click to view file"
        >
          {{ formatFileName(doc.name) }}

          <v-icon
            v-if="!isFileLoading"
            medium
            class="ml-3 delete-icon"
            @click.stop="confirmDelete(doc.name)"
          >
            mdi-delete
          </v-icon>
        </v-chip>
      </div>
      <v-dialog
        v-model="deleteDialog"
        max-width="600px"
      >
        <v-card>
          <v-card-title class="headline">Confirm Deletion</v-card-title>
          <v-card-text>Are you sure you want to delete this file?</v-card-text>
          <v-card-actions>
            <v-btn
              color="error"
              text
              @click="deleteDialog = false"
            >
              Cancel
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
              text
              @click="deleteConfirmed"
            >
              Delete
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-alert
        v-if="filteredDocuments.length === 0"
        :value="true"
        type="info"
        dense
        text
        color="primary"
        outlined
        class="mt-3"
      >
        Drop files here or click to upload
      </v-alert>
      <v-alert
        v-if="hasError"
        :value="true"
        type="error"
        dense
        text
        color="error"
        outlined
        class="mt-3"
      >
        {{ validationErrors.join(', ') }}
      </v-alert>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { computed, defineEmits, defineProps, ref } from 'vue'

interface Props {
  acceptedFormats?: string
  documentLabel?: string
  isLoading?: boolean
  rules?: Array<(value: File[]) => boolean | string>
  uploadedDocuments: Array<{ name: string; documentType: string }>
  filterDocumentType: string
}

const props = defineProps<Props>()
const emit = defineEmits([
  'upload-files',
  'fileNameSegment',
  'delete-file',
  'file-opening',
  'file-opened',
])
const files = ref<File[]>([])
const isDragging = ref(false)
const deleteDialog = ref(false)
const isFileLoading = ref(false)
let fileToDelete = ref('')

const filteredDocuments = computed(() => {
  return props.uploadedDocuments.filter(
    doc => doc.documentType === props.filterDocumentType
  )
})

function confirmDelete(fileName: string) {
  fileToDelete.value = fileName
  deleteDialog.value = true
}

function deleteConfirmed() {
  emit('delete-file', fileToDelete.value)
  deleteDialog.value = false
}

function dragOver() {
  isDragging.value = true
}

function dragLeave() {
  isDragging.value = false
}

function dropFiles(event: DragEvent) {
  event.preventDefault()

  if (event.dataTransfer) {
    const droppedFiles = Array.from(event.dataTransfer.files)

    handleFiles(droppedFiles)
  }

  isDragging.value = false
}

function formatFileName(fileName: string): string {
  const parts = fileName.split('_')

  if (
    parts.length === 4 &&
    /^[A-Za-z]+_[A-Za-z]+_[A-Za-z]+\d*_\d+$/.test(fileName)
  ) {
    const modifiedName = parts.slice(2).join('_')

    return modifiedName
  }

  return fileName
}

function handleFiles(newFiles: File[] | FileList) {
  const acceptedFormatsArray = props.acceptedFormats
    ?.split(',')
    .map(format => format.trim())
  const newFilesArray = Array.from(newFiles)

  const filteredFiles = newFilesArray.filter(file =>
    acceptedFormatsArray?.includes(file.type)
  )

  files.value = []

  files.value = [...files.value, ...filteredFiles]

  emit('upload-files', files.value)
}

async function openFile(fileName: string) {
  const response = await axios.get(
    `${Endpoints.GET_DOCUMENT_FILE_ENDPOINT}?applicantFileName=${fileName}`,
    { responseType: 'blob' }
  )

  if (response.data.type === 'application/pdf') {
    const pdfBlob = new Blob([response.data], { type: 'application/pdf' })
    // eslint-disable-next-line node/no-unsupported-features/node-builtins
    const pdfUrl = URL.createObjectURL(pdfBlob)
    const newWindow = window.open(pdfUrl, '_blank')

    if (newWindow) {
      // eslint-disable-next-line node/no-unsupported-features/node-builtins
      URL.revokeObjectURL(pdfUrl)
      emit('file-opened')
    } else {
      alert(
        'The PDF could not be opened in a new window. Please check your pop-up blocker settings.'
      )
    }
  } else if (response.data.type === 'text/plain') {
    response.data.text().then(base64String => {
      fetch(base64String)
        .then(res => res.blob())
        .then(blob => {
          // eslint-disable-next-line node/no-unsupported-features/node-builtins
          const imgUrl = URL.createObjectURL(blob)

          window.open(imgUrl, '_blank')
          emit('file-opened')
        })
    })
  }
}

function triggerFileDialog() {
  const input = document.createElement('input')

  input.type = 'file'
  input.multiple = true
  input.accept = props.acceptedFormats || ''
  input.addEventListener('change', event => {
    handleFiles((event.target as HTMLInputElement).files || [])
  })
  input.click()
}

const hasError = computed(() => {
  if (!props.rules || !Array.isArray(props.rules) || props.rules.length === 0)
    return false

  return props.rules.some(
    rule => typeof rule === 'function' && typeof rule(files.value) === 'string'
  )
})

const validationErrors = computed(() => {
  if (!props.rules || props.rules.length === 0) return []

  return props.rules
    .filter(rule => typeof rule === 'function')
    .map(rule => rule(files.value))
})

const isFileUploaded = computed(() => {
  return filteredDocuments.value.length > 0
})

const successBorder = computed(() => {
  return isDragging.value || (isFileUploaded.value && !hasError.value)
})
</script>

<style scoped>
.card-text {
  height: 200px;
  overflow-y: auto;
}

.consistent-border {
  transition: border-color 0.2s;
}

.highlight-border {
  border: 2px solid green;
}

.error-border {
  border: 2px solid red;
}

.file-chips {
  margin-top: 8px;
}

.file-chip {
  height: 24px;
  line-height: 24px;
  margin-right: 8px;
  margin-top: 4px;
  margin-bottom: 4px;
}
.upload-trigger {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 24px;
  border-radius: 12px;
  cursor: pointer;
}

.upload-trigger:hover {
  pointer-events: none;
}

.v-card--link:before {
  background: none;
}

.delete-icon:hover {
  background-color: red;
}
</style>
