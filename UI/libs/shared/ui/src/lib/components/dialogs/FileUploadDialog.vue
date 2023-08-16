<template>
  <div>
    <v-dialog
      width="600"
      v-model="state.dialog"
    >
      <template #activator="{ on, attrs }">
        <v-btn
          v-bind="attrs"
          v-on="on"
          color="primary"
        >
          <v-icon left> mdi-upload</v-icon>
          Upload
        </v-btn>
      </template>
      <v-card>
        <v-card-title>
          {{ $t('Upload Documents') }}
        </v-card-title>

        <v-card-text class="mt-6">
          <v-file-input
            dense
            outlined
            :label="$t('Select File')"
            prepend-icon=""
            prepend-inner-icon="mdi-file-document"
            accept="image/png, image/jpeg, image/bmp"
            @change="handleUpload"
          >
          </v-file-input>
        </v-card-text>

        <v-card-text>
          <v-select
            dense
            outlined
            :label="$t('File Type')"
            :items="userFileTypes"
            item-text="name"
            item-value="value"
            :rules="[v => !!v || 'Must select an option']"
            v-model="state.fileType"
          >
          </v-select>
        </v-card-text>

        <v-card-actions>
          <v-btn
            color="primary"
            text
            @click="state.dialog = false"
          >
            close
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            :disabled="!state.fileType"
            color="primary"
            text
            @click="handleSubmit"
          >
            submit
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'
import { userFileTypes } from '@shared-utils/lists/defaultConstants'

const emit = defineEmits(['on-file-submit'])
const state = reactive({
  dialog: false,
  fileType: '',
  file: {} as File,
})

function handleUpload(file) {
  state.file = file
}

function handleSubmit() {
  emit('on-file-submit', { file: state.file, fileType: state.fileType })
  state.dialog = false
}
</script>
