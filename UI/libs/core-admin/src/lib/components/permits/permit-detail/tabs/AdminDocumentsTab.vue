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
                    permitStore.getPermitDetail.application
                      .adminUploadedDocuments[index].documentType
                  "
                  single-line
                  outlined
                  dense
                  :menu-props="{ bottom: true, offsetY: true }"
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
    'UnofficialLicense',
    'OfficialLicense',
    'LiveScan',
    'Application',
  ],
})

async function openPdf($event, name) {
  $event.preventDefault()
  window.console.log('OpenPDF')

  documentStore.getAdminApplicationFile(name).then(res => {
    //   window.console.log('res: ', res)

    if (res) {
      //     window.console.log('made it to if(res)')

      //     return res.blob()
      //   }
      // })
      // .then(blob => {
      //   if (blob) {
      //     // eslint-disable-next-line node/no-unsupported-features/node-builtins
      //     let pdfBlobUrl = URL.createObjectURL(blob)

      //     window.open(pdfBlobUrl, '_blank')
      //   }
      // })
      //}
      const binaryString = window.atob(res.data.Content)
      const len = res.data.Content.length
      const bytes = new Uint8Array(len)

      for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i)
      }

      const blob = new Blob([bytes.buffer], { type: res.data.ContentType })

      // eslint-disable-next-line node/no-unsupported-features/node-builtins
      const url = URL.createObjectURL(blob)

      window.open(url)
      // let file = new Blob([res.data], { type: 'application/pdf' })
      // // eslint-disable-next-line node/no-unsupported-features/node-builtins
      // let fileURL = URL.createObjectURL(file)

      // window.open(fileURL)
    } else {
      let image = new Image()

      image.src = res.data
      let w = window.open('')

      if (w) {
        w.document.write(image.outerHTML)
      }
    }
  })
}

function handleSave() {
  emit('on-save', 'Documents')
}
</script>
