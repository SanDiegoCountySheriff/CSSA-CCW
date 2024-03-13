<template>
  <div>
    <v-card-title>Modify Supporting Documents</v-card-title>

    <v-card-text>
      <v-row>
        <v-col>
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
        </v-col>

        <v-col>
          <FileUploadContainer
            v-if="modifyingAddress"
            :uploaded-documents="application.application.uploadedDocuments"
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Address Change Documents'"
            :is-loading="loadingStates.Address"
            :rules="addressValidationRules"
            :filter-document-type="'Address'"
            @file-opening="loadingStates.Address = true"
            @file-opened="loadingStates.Address = false"
            @upload-files="files => handleMultiInput(files, 'Address')"
            @delete-file="address => deleteFile(address)"
          />
        </v-col>

        <v-col>
          <FileUploadContainer
            v-if="modifyingWeapons"
            :uploaded-documents="application.application.uploadedDocuments"
            :accepted-formats="'image/png, image/jpeg, application/pdf'"
            :document-label="'Weapon Safety Course Documents'"
            :is-loading="loadingStates.Weapon"
            :rules="weaponValidationRules"
            :filter-document-type="'Weapon'"
            @file-opening="loadingStates.Weapon = true"
            @file-opened="loadingStates.Weapon = false"
            @upload-files="files => handleMultiInput(files, 'Weapon')"
            @delete-file="weapon => deleteFile(weapon)"
          />
        </v-col>
      </v-row>
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
  Address: false,
  Weapon: false,
})

const nameValidationRules = computed(() => {
  return [v => Boolean(v) || 'we need it']
})

const addressValidationRules = computed(() => {
  return [v => Boolean(v) || 'we need it']
})

const weaponValidationRules = computed(() => {
  return [v => Boolean(v) || 'we need it']
})

function handleMultiInput(files, fileType) {}

function deleteFile(name) {}

function handleContinue() {}

function handleSave() {}
</script>
