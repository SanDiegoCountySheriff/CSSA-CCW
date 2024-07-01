<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Admin Attached Documents:') }}
      <v-spacer></v-spacer>
      <SaveButton
        :disabled="readonly"
        @on-save="handleSave"
      />
    </v-card-title>

    <v-card-text>
      <v-form v-model="valid">
        <v-data-table
          :headers="headers"
          :items="
            permitStore.getPermitDetail.application.adminUploadedDocuments
          "
          class="elevation-0"
          :editable="true"
        >
          <template #[`item.name`]="{ item }">
            <v-row>
              <v-col>
                <td style="font-size: 12px">
                  {{ item.name }}
                </td>
              </v-col>
              <v-col>
                <v-tooltip bottom>
                  <template #activator="{ on, attrs }">
                    <v-icon
                      v-on="on"
                      v-bind="attrs"
                      color="primary"
                      class="float-right"
                      @click="editDialog(item)"
                    >
                      mdi-rename-box-outline
                    </v-icon>
                  </template>
                  <span>{{ $t('Rename file') }}</span>
                </v-tooltip>
              </v-col>
            </v-row>
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
              class="ml-5"
            >
              mdi-delete
            </v-icon>
          </template>
        </v-data-table>
      </v-form>
      <v-dialog
        v-model="showDeleteDialog"
        max-width="600px"
      >
        <v-card>
          <v-card-title class="headline">Confirm Delete</v-card-title>
          <v-card-text>
            Are you sure you want to delete:
            {{ itemToDelete ? itemToDelete.name : '' }}?
          </v-card-text>
          <v-card-actions>
            <v-btn
              color="error"
              text
              @click="showDeleteDialog = false"
            >
              Cancel
            </v-btn>

            <v-spacer />

            <v-btn
              color="primary"
              text
              @click="deletePdf()"
            >
              Delete
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-dialog
        v-model="showEditDialog"
        max-width="600px"
      >
        <v-card outlined>
          <v-card-title class="headline">
            Rename {{ itemToEdit?.name }}
          </v-card-title>

          <v-card-text>
            <v-row>
              <v-col>
                <v-text-field
                  v-model="editedFileName"
                  :rules="fileNameRules"
                  :label="'New file name'"
                  outlined
                  dense
                >
                </v-text-field>
              </v-col>
            </v-row>
          </v-card-text>

          <v-card-actions>
            <v-btn
              color="error"
              text
              @click="showEditDialog = false"
            >
              Cancel
            </v-btn>

            <v-spacer />

            <v-btn
              :disabled="!valid"
              color="primary"
              text
              @click="onNameEdit()"
            >
              Confirm Edit
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { computed, inject, ref } from 'vue'
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()
const documentStore = useDocumentsStore()
const readonly = inject<boolean>('readonly')
const valid = ref(false)
const editedFileName = ref('')
const showEditDialog = ref(false)
const showDeleteDialog = ref(false)
const itemToDelete = ref<UploadedDocType | null>(null)
const itemToEdit = ref<UploadedDocType | null>(null)

const headers = [
  { text: 'DOCUMENT NAME', value: 'name' },
  { text: 'DOCUMENT TYPE', value: 'documentType', width: '200px' },
  { text: 'UPLOADED BY', value: 'uploadedBy' },
  { text: 'UPLOADED DATE', value: 'uploadedDateTimeUtc' },
  { text: 'ACTIONS', value: 'actions' },
]

const fileNameRules = computed(() => {
  return [
    v => !showEditDialog.value || Boolean(v) || 'File name is required',
    v =>
      !showEditDialog.value ||
      !/[#%&{}/\\<>*$'":@+`|=~?!]/.test(v) ||
      'Special characters are not allowed',
    v =>
      !showEditDialog.value || !/\s/.test(v) || 'Blank spaces are not allowed',
    v =>
      !showEditDialog.value ||
      !isDuplicateFileName(v) ||
      'File name already exists',
  ]
})

function isDuplicateFileName(name) {
  const ignoreNames = ['Signature', 'Thumbprint', 'Portrait']

  const lowerCaseName = name.toLowerCase()

  if (ignoreNames.map(n => n.toLowerCase()).includes(lowerCaseName)) {
    return false
  }

  if (
    itemToEdit.value &&
    itemToEdit.value.name.toLowerCase() === lowerCaseName
  ) {
    return true
  }

  return permitStore.getPermitDetail.application.adminUploadedDocuments.some(
    doc => doc.name.toLowerCase() === lowerCaseName && doc !== itemToEdit.value
  )
}

async function onNameEdit() {
  if (itemToEdit.value) {
    let oldName = itemToEdit.value.name
    let name = editedFileName.value

    let oldNameWithId = `${permitStore.getPermitDetail.id}_${oldName}`
    let newName = `${permitStore.getPermitDetail.id}_${name}`

    documentStore.editAdminApplicationFileName(oldNameWithId, newName)

    const index =
      permitStore.getPermitDetail.application.adminUploadedDocuments.findIndex(
        doc => doc.name === oldName
      )

    if (index !== -1) {
      permitStore.getPermitDetail.application.adminUploadedDocuments[
        index
      ].name = name
    }

    permitStore.updatePermitDetailApi(
      `Updated name of document ${oldName} to ${name}`
    )
  }

  showEditDialog.value = false
  itemToEdit.value = null
}

async function openPdf(item) {
  documentStore
    .getAdminApplicationFile(item.name)
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
  if (itemToDelete.value) {
    documentStore.deleteAdminApplicationFile(itemToDelete.value?.name)
    const index =
      permitStore.getPermitDetail.application.adminUploadedDocuments.indexOf(
        itemToDelete.value
      )

    if (index > -1) {
      permitStore.getPermitDetail.application.adminUploadedDocuments.splice(
        index,
        1
      )
      permitStore.updatePermitDetailApi(
        `Deleted document: ${itemToDelete.value.name}`
      )
    }
  }

  showDeleteDialog.value = false
  itemToDelete.value = null
}

async function confirmDelete(item) {
  itemToDelete.value = item
  showDeleteDialog.value = true
}

async function editDialog(item) {
  itemToEdit.value = item
  showEditDialog.value = true
  editedFileName.value = ''
}

function handleSave() {
  emit('on-save', 'Documents')
}
</script>
