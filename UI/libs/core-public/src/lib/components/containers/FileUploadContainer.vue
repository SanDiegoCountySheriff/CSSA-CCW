<template>
  <v-card
    loading="isLoading"
    outlined
    elevation="1"
    :class="{
      'highlight-border': isDragging,
      'consistent-border': !isDragging && !hasError,
      'error-border': hasError && !isDragging,
    }"
    @dragover.prevent="dragOver"
    @dragleave.prevent="dragLeave"
    @drop.prevent="dropFiles"
  >
    <v-card-text class="card-text">
      <div
        class="upload-trigger"
        tabindex="0"
        @click="triggerFileDialog"
        @keydown.enter="triggerFileDialog"
      >
        <v-icon
          color="primary"
          class="mr-2"
        >
          mdi-cloud-upload-outline
        </v-icon>
        <span class="upload-text">{{ documentLabel }}</span>
      </div>
      <v-file-input
        v-show="false"
        :disabled="isLoading"
        :rules="rules || []"
        @change="handleFiles"
        multiple
        :accept="acceptedFormats"
        :label="documentLabel"
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
          x-small
        >
          {{ formatFileName(doc.name) }}
        </v-chip>
      </div>
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
  if (!props.rules || props.rules.length === 0) return false

  return props.rules.some(
    rule => typeof rule === 'function' && typeof rule(files.value) === 'string'
  )
})
</script>

<style scoped>
.card-text {
  height: 100px;
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
}

.upload-trigger {
  display: flex;
  align-items: center;
  gap: 8px;
}
</style>
