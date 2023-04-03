<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Attached Documents:') }}
      <v-spacer></v-spacer>
      <SaveButton
        :disabled="false"
        @on-save="handleSave"
      />
    </v-card-title>

    <v-card-text>
      <v-simple-table>
        <template #default>
          <thead>
            <tr>
              <th class="text-left">
                {{ $t('Name') }}
              </th>
              <th class="text-left">
                {{ $t('Document Type') }}
              </th>
              <th class="text-left">
                {{ $t('Uploaded By') }}
              </th>
              <th class="text-left">
                {{ $t('Upload Date & Time') }}
              </th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(item, index) in state.documents"
              :key="index"
            >
              <td>
                <a
                  :href="documentStore.formatName(item.name)"
                  @click="openPdf($event, item.name)"
                  @keydown="openPdf($event, item.name)"
                >
                  <v-icon class="mr-2"> mdi-download </v-icon>{{ item.name }}
                </a>
              </td>
              <td>
                <v-select
                  :items="state.documentTypes"
                  :label="item.documentType"
                  v-model="
                    permitStore.getPermitDetail.application.uploadedDocuments[
                      index
                    ].documentType
                  "
                  single-line
                  dense
                ></v-select>
              </td>
              <td>{{ item.uploadedBy }}</td>
              <td>
                {{ formatDate(item.uploadedDateTimeUtc) }}
                &nbsp;
                {{ formatTime(item.uploadedDateTimeUtc) }}
              </td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue';
import { reactive } from 'vue';
import { useDocumentsStore } from '@core-admin/stores/documentsStore';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters';

const emit = defineEmits(['on-save']);
const permitStore = usePermitsStore();
const documentStore = useDocumentsStore();

const state = reactive({
  documents: permitStore.getPermitDetail.application.uploadedDocuments,
  documentTypes: [
    'DriverLicense',
    'ProofResidency',
    'ProofResidency2',
    'MilitaryDoc',
    'Citizenship',
    'Supporting',
    'NameChange',
    'Judicial',
    'Reserve',
    'Signature',
  ],
});

async function openPdf($event, name) {
  $event.preventDefault();

  documentStore.getUserDocument(name).then(res => {
    if (res) {
      if (res.headers['content-type'] === 'application/pdf') {
        let file = new Blob([res.data], { type: 'application/pdf' });
        // eslint-disable-next-line node/no-unsupported-features/node-builtins
        let fileURL = URL.createObjectURL(file);

        window.open(fileURL);
      } else {
        let image = new Image();

        image.src = res.data;
        let w = window.open('');

        if (w) {
          w.document.write(image.outerHTML);
        }
      }
    }
  });
}

function handleSave() {
  emit('on-save', 'Documents');
}
</script>
