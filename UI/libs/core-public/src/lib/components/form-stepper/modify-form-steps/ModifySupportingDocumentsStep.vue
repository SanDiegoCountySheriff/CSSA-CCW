<template>
  <div>
    <v-card-title>Modify Supporting Documents</v-card-title>

    <v-card-text>
      <FileUploadContainer
        v-if="modifyingName"
        :uploaded-documents="application.application.uploadedDocuments"
        :accepted-formats="'image/png, image/jpeg, application/pdf'"
        :document-label="'Name Change Documents'"
        :is-loading="loadingStates.Names"
        :rules="nameValidationRules"
        :filter-document-type="'Name'"
        @file-opening="loadingStates.Names = true"
        @file-opened="loadingStates.Names = false"
        @upload-files="files => handleMultiInput(files, 'Name')"
        @delete-file="name => deleteFile(name)"
      />
    </v-card-text>

    <FormButtonContainer
      @continue="handleContinue"
      @save="handleSave"
      valid
    />
  </div>
</template>

<script lang="ts" setup>
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FileUploadContainer from '@core-public/components/containers/FileUploadContainer.vue'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { computed, reactive } from 'vue'

interface ModifyNameProps {
  application: CompleteApplication
  modifyingName: boolean
  modifyingAddress: boolean
  modifyingWeapons: boolean
}

const props = defineProps<ModifyNameProps>()

const loadingStates = reactive({
  Names: false,
})

const nameValidationRules = computed(() => {
  return [v => true]
})

function handleMultiInput(files, fileType) {}

function deleteFile(name) {}

function handleContinue() {}

function handleSave() {}
</script>
