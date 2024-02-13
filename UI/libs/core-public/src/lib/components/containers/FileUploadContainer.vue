<template>
  <v-card
    loading="isLoading"
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
        @click="triggerFileDialog"
        tabindex="0"
        @keydown.enter="triggerFileDialog"
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
        ref="fileInput"
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
          small
        >
          {{ formatFileName(doc.name) }}
        </v-chip>
      </div>
      <v-alert
        v-if="hasError"
        :value="true"
        type="error"
        dense
        text
        color="error"
        outlined
        class="mt-4"
      >
        {{ validationErrors.join(', ') }}
      </v-alert>
      <v-alert
        v-else-if="filteredDocuments.length === 0"
        :value="true"
        type="info"
        dense
        text
        color="primary"
        outlined
        class="mt-4"
      >
        Drop files here
      </v-alert>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
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
const emit = defineEmits(['update:files', 'fileNameSegment'])
const files = ref<File[]>([])
const isDragging = ref(false)

const filteredDocuments = computed(() => {
  return props.uploadedDocuments.filter(
    doc => doc.documentType === props.filterDocumentType
  )
})

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

  const modifiedName = parts.slice(2).join('_')

  return modifiedName
}

function handleFiles(newFiles: File[] | FileList) {
  const newFilesArray = Array.from(newFiles)
  const existingFileNames = files.value.map(file => file.name)

  newFilesArray.forEach(newFile => {
    if (!existingFileNames.includes(newFile.name)) {
      files.value.push(newFile)
    }
  })

  emit('update:files', files.value)
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
  background-color: rgba(0, 0, 0, 0.1);
}
</style>
