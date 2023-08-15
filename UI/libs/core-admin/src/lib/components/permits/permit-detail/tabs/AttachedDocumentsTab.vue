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
      <template>
        <v-data-table
          :headers="state.headers"
          :items="state.documents"
          class="elevation-0"
          :editable="true"
        >
          <template #[`item.name`]="{ item }">
            <v-text-field
              :value="item.name"
              @change="onNameEdit(item, $event)"
              style="font-size: 12px"
            ></v-text-field>
          </template>
          <template #[`item.uploadedDateTimeUtc`]="{ item }">
            <td>
              {{ formatDate(item.uploadedDateTimeUtc) }}&nbsp;{{
                formatTime(item.uploadedDateTimeUtc)
              }}
            </td>
          </template>
          <template #[`item.actions`]="{ item }">
            <v-icon @click="openPdf(item)">mdi-download</v-icon>
            <v-icon
              @click="confirmDelete(item)"
              color="red"
            >
              mdi-delete
            </v-icon>
          </template>
        </v-data-table>
      </template>
      <v-dialog
        v-model="state.showDeleteDialog"
        max-width="600px"
      >
        <v-card>
          <v-card-title class="headline">Confirm Delete</v-card-title>
          <v-card-text>
            Are you sure you want to delete:
            {{ state.itemToDelete ? state.itemToDelete.name : '' }}?
          </v-card-text>
          <v-card-actions>
            <v-btn
              color="primary"
              text
              @click="state.showDeleteDialog = false"
              >Cancel</v-btn
            >
            <v-btn
              color="red"
              text
              @click="deletePdf()"
              >Delete</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-dialog>
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
import { UploadedDocType } from '@shared-utils/types/defaultTypes'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()
const documentStore = useDocumentsStore()

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
  headers: [
    { text: 'DOCUMENT NAME', value: 'name' },
    { text: 'DOCUMENT TYPE', value: 'documentType' },
    { text: 'UPLOADED BY', value: 'uploadedBy' },
    { text: 'UPLOADED DATE', value: 'uploadedDateTimeUtc' },
    { text: 'ACTIONS', value: 'actions' },
  ],
  showDeleteDialog: false,
  itemToDelete: null as UploadedDocType | null,
})

function onNameEdit(item, name) {
  let oldName = item.name

  item.name = name
  let oldNameWithId = `${permitStore.getPermitDetail.userId}_${oldName}`
  let newName = `${permitStore.getPermitDetail.userId}_${name}`

  documentStore.editApplicationFileName(oldNameWithId, newName)

  permitStore.updatePermitDetailApi('updated name')
}

async function openPdf(item) {
  documentStore
    .getUserDocument(item.name)
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

async function deletePdf() {
  if (state.itemToDelete) {
    documentStore.deleteApplicationFile(state.itemToDelete.name)
    const index = state.documents.indexOf(state.itemToDelete)

    if (index > -1) {
      state.documents.splice(index, 1)
      permitStore.updatePermitDetailApi(
        `'Deleted document: '${state.itemToDelete.name}`
      )
    }
  }

  state.showDeleteDialog = false
  state.itemToDelete = null
}

async function confirmDelete(item) {
  state.itemToDelete = item
  state.showDeleteDialog = true
}

function handleSave() {
  emit('on-save', 'Documents')
}
</script>
