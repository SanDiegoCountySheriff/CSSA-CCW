<template>
  <div class="file-dialog-container">
    <v-dialog
      width="600"
      v-model="state.dialog"
    >
      <template #activator="{ on, attrs }">
        <v-btn
          small
          :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
          class="mr-1 w-auto"
          v-bind="attrs"
          v-on="on"
        >
          <v-icon> {{ props.icon }}</v-icon>
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
            :items="adminFileTypes"
            item-text="name"
            item-value="value"
            :rules="[v => !!v || 'Must select an option']"
            v-model="state.fileType"
          >
          </v-select>
        </v-card-text>
        <v-card-actions>
          <v-spacer> </v-spacer>
          <v-btn
            :disabled="!state.fileType"
            color="info"
            text
            @click="handleSubmit"
          >
            submit
          </v-btn>
          <v-btn
            color="error"
            text
            @click="state.dialog = false"
          >
            close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script lang="ts" setup>
import { adminFileTypes } from '@shared-utils/lists/defaultConstants';
import { reactive } from 'vue';
interface FileUploadDialogProps {
  icon: string;
  defaultSelection?: string;
  getFileFromDialog: (file, target) => void;
}
const props = defineProps<FileUploadDialogProps>();
const state = reactive({
  dialog: false,
  fileType: props.defaultSelection ? `${props.defaultSelection}` : '',
  file: {} as File,
});

function handleUpload(file) {
  state.file = file;
}

function handleSubmit() {
  props.getFileFromDialog(state.file, state.fileType);
  state.dialog = false;
}
</script>

<style lang="scss">
.file-dialog-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 2px 0;
}
</style>
