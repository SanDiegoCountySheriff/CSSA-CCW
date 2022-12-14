<template>
  <v-card elevation="0">
    <v-card-title class="subtitle-2">
      {{ $t('Attached Documents:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col cols="12">
        <v-simple-table>
          <template #default>
            <thead>
              <tr>
                <th
                  scope="col"
                  class="text-left"
                >
                  {{ $t('Name') }}
                </th>
                <th
                  scope="col"
                  class="text-left"
                >
                  {{ $t('Document Type') }}
                </th>
                <th
                  scope="col"
                  class="text-left"
                >
                  {{ $t('Uploaded By') }}
                </th>
                <th
                  scope="col"
                  class="text-left"
                >
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
                    :href="formatName(item.name)"
                    @click="openPdf"
                    @keydown="openPdf"
                  >
                    <v-icon> mdi-download </v-icon>{{ item.name }}
                  </a>
                </td>
                <td>{{ item.documentType }}</td>
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
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import axios from 'axios';
import { reactive } from 'vue';
import { useDocumentsStore } from '@core-admin/stores/documentsStore';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters';

const permitStore = usePermitsStore();
const { formatName } = useDocumentsStore();

const state = reactive({
  documents: permitStore.getPermitDetail.application.uploadedDocuments,
});

function openPdf($event) {
  // Prevent default behavior when clicking a link
  $event.preventDefault();

  // Get filename from href
  let filename = $event.target.href;

  axios.get(filename, { responseType: 'arraybuffer' }).then(res => {
    let file = new Blob([res.data], { type: 'application/pdf' });
    // eslint-disable-next-line node/no-unsupported-features/node-builtins
    let fileURL = URL.createObjectURL(file);

    window.open(fileURL);

    /*  TODO: for png/jpeg
    let image = new Image();
    image.src = res.data;
    let w = window.open('');
    w.document.write(image.outerHTML);
    */
  });
}
</script>
<style lang="scss">
.v-data-table > .v-data-table__wrapper > table > tbody > tr > td {
  text-align: left !important;
}
</style>
