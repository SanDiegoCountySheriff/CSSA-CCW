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
                  <a :href="formatName(item.name)">
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
</script>
<style lang="scss">
.v-data-table > .v-data-table__wrapper > table > tbody > tr > td {
  text-align: left !important;
}
</style>
