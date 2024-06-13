<template>
  <v-dialog
    v-model="dialog"
    width="600"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        :loading="loading"
        color="white"
        text
      >
        <v-icon left> mdi-file-upload-outline</v-icon>
        Upload Files
      </v-btn>
    </template>

    <v-sheet
      color="primary"
      width="600"
      outlined
      rounded
    >
      <v-card outlined>
        <v-card-title>
          {{ $t('Upload Files') }}
        </v-card-title>

        <v-card-text class="mt-6">
          <v-file-input
            :label="$t('Select File')"
            @change="handleUpload"
            prepend-icon=""
            prepend-inner-icon="mdi-file-document"
            accept="image/png, image/jpeg, image/bmp"
            outlined
            dense
          >
          </v-file-input>
        </v-card-text>

        <v-card-text>
          <v-select
            v-model="fileType"
            :label="$t('File Type')"
            :items="adminFileTypes"
            :rules="[v => !!v || 'Must select an option']"
            item-text="name"
            item-value="value"
            outlined
            dense
          >
          </v-select>
        </v-card-text>

        <v-card-actions>
          <v-btn
            color="error"
            text
            @click="dialog = false"
          >
            Cancel
          </v-btn>

          <v-spacer> </v-spacer>

          <v-btn
            :disabled="!fileType"
            color="primary"
            text
            @click="handleSubmit"
          >
            Submit
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-sheet>
  </v-dialog>
</template>

<script lang="ts" setup>
import { adminFileTypes } from '@shared-utils/lists/defaultConstants'
import { ref } from 'vue'

interface FileUploadDialogProps {
  loading: boolean
}

defineProps<FileUploadDialogProps>()

const emit = defineEmits(['get-file-from-dialog'])

const dialog = ref(false)
const fileType = ref('')
const file = ref<File>()

function handleUpload(input: File) {
  file.value = input
}

function handleSubmit() {
  emit('get-file-from-dialog', { file: file.value, target: fileType.value })
  dialog.value = false
  file.value = undefined
  fileType.value = ''
}
</script>
