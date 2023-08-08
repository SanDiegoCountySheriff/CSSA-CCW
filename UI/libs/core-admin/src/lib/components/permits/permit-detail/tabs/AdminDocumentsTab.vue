<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Admin Attached Documents:') }}
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
            <template v-for="documentType in state.documentTypes">
              <tr
                v-if="
                  state.documents.filter(
                    doc => doc.documentType === documentType
                  ).length > 0
                "
                :key="documentType"
              >
                <td
                  colspan="4"
                  style="font-weight: bold"
                >
                  {{ documentType }}
                </td>
              </tr>
              <tr
                v-for="item in state.documents.filter(
                  doc => doc.documentType === documentType
                )"
                :key="`${item.name}-${item.documentType}`"
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
                <td>{{ item.documentType }}</td>
                <td>{{ item.uploadedBy }}</td>
                <td>
                  {{ formatDate(item.uploadedDateTimeUtc) }}&nbsp;{{
                    formatTime(item.uploadedDateTimeUtc)
                  }}
                </td>
              </tr>
            </template>
          </tbody>
        </template>
      </v-simple-table>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import { reactive } from 'vue'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()
const documentStore = useDocumentsStore()

const state = reactive({
  documents: permitStore.getPermitDetail.application.adminUploadedDocuments,
  documentTypes: [
    'Unofficial_License',
    'Official_License',
    'Live_Scan',
    'Application',
  ],
})

async function openPdf($event, name) {
  $event.preventDefault()

  documentStore
    .getAdminApplicationFile(name)
    .then(response => {
      if (response.type === 'application/pdf') {
        const pdfBlob = new Blob([response], { type: 'application/pdf' })
        // eslint-disable-next-line node/no-unsupported-features/node-builtins
        const pdfUrl = URL.createObjectURL(pdfBlob)
        const newWindow = window.open(pdfUrl, '_blank')

        if (newWindow) {
          // eslint-disable-next-line node/no-unsupported-features/node-builtins
          URL.revokeObjectURL(pdfUrl)
        } else {
          alert(
            'The PDF could not be opened in a new window. Please check your pop-up blocker settings.'
          )
        }
      } else {
        const imgBlob = new Blob([response], { type: 'image/jpeg' })
        // eslint-disable-next-line node/no-unsupported-features/node-builtins
        const imgUrl = URL.createObjectURL(imgBlob)

        const img = new Image()

        img.onload = () => {
          const w = window.open('')

          if (w) {
            w.document.write(img.outerHTML)
          }

          // eslint-disable-next-line node/no-unsupported-features/node-builtins
          URL.revokeObjectURL(imgUrl)
        }
        img.src = imgUrl
      }
    })
    .catch(error => {
      console.error('Error fetching the PDF:', error)
    })
}

function handleSave() {
  emit('on-save', 'Documents')
}
</script>
