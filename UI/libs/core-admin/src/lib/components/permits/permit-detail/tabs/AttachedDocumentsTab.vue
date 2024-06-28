<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Attached Documents:') }}
      <v-spacer></v-spacer>
      <SaveButton
        :disabled="readonly || !valid"
        @on-save="handleSave"
      />
    </v-card-title>

    <v-card-text>
      <v-form v-model="valid">
        <v-data-table
          :headers="state.headers"
          :items="state.documents"
          class="elevation-0"
          :editable="true"
        >
          <template #[`item.name`]="{ item }">
            <v-row>
              <v-col>
                <td>
                  {{ item.name }}
                </td>
              </v-col>
              <v-col>
                <v-icon
                  @click="editDialog(item)"
                  class="float-right"
                  color="primary"
                >
                  mdi-pencil-box-outline
                </v-icon>
              </v-col>
            </v-row>
          </template>
          <template #[`item.documentType`]="{ item }">
            <v-select
              v-model="item.documentType"
              :items="documentTypeSelections"
              item-text="name"
              item-value="value"
              style="width: 200px"
            ></v-select>
          </template>
          <template #[`item.uploadedDateTimeUtc`]="{ item }">
            <td>
              {{ formatDate(item.uploadedDateTimeUtc) }}&nbsp;{{
                formatTime(item.uploadedDateTimeUtc)
              }}
            </td>
          </template>
          <template #[`item.actions`]="{ item }">
            <v-icon
              @click="openPdf(item)"
              class="mx-1"
            >
              mdi-download
            </v-icon>
            <v-icon
              @click="confirmDelete(item)"
              color="red"
              class="mx-1"
            >
              mdi-delete
            </v-icon>
          </template>
        </v-data-table>

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
                color="error"
                text
                @click="state.showDeleteDialog = false"
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
              Rename {{ state.itemToEdit?.name }}
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
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import { openPdf } from '@core-admin/components/composables/openDocuments'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { computed, inject, reactive, ref } from 'vue'
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

const state = reactive({
  documents: permitStore.getPermitDetail.application.uploadedDocuments || [],
  documentTypes: [
    { value: 'DriverLicense', name: "Driver's License" },
    { value: 'ProofResidency', name: 'Proof of Residency' },
    { value: 'ProofResidency2', name: 'Proof of Residency 2' },
    { value: 'MilitaryDoc', name: 'Military Document' },
    { value: 'Citizenship', name: 'Citizenship Document' },
    { value: 'Supporting', name: 'Supporting Document' },
    { value: 'NameChange', name: 'Name Change Document' },
    { value: 'Judicial', name: 'Judicial Document' },
    { value: 'Reserve', name: 'Reserve Document' },
    { value: 'Employment', name: 'Employment Document' },
    { value: 'Signature', name: 'Signature Document' },
    { value: 'EightHourSafetyCourse', name: 'Eight Hour Safety Course' },
    { value: 'Portrait', name: 'Portrait' },
    { value: 'Thumbprint', name: 'Thumbprint' },
    { value: 'Signature', name: 'Signature' },
    { value: 'ModifyAddress', name: 'Modify Address' },
    { value: 'ModifyWeapons', name: 'Modify Weapons' },
    { value: 'ModifyName', name: 'Modify Name' },
  ],
  headers: [
    { text: 'DOCUMENT NAME', value: 'name' },
    { text: 'DOCUMENT TYPE', value: 'documentType', width: '200px' },
    { text: 'UPLOADED BY', value: 'uploadedBy' },
    { text: 'UPLOADED DATE', value: 'uploadedDateTimeUtc' },
    { text: 'ACTIONS', value: 'actions' },
  ],
  showDeleteDialog: false,
  itemToDelete: null as UploadedDocType | null,
  itemToEdit: null as UploadedDocType | null,
})

const documentTypeSelections = computed(() => {
  return state.documentTypes.map(type => ({
    value: type.value,
    name: type.name,
  }))
})

const fileNameRules = computed(() => {
  return [
    v => !showEditDialog.value || Boolean(v) || 'File name is required',
    v =>
      !/[#%&{}/\\<>*$'":@+`|=~?!]/.test(v) ||
      'Special characters are not allowed',
    v => !/\s/.test(v) || 'Blank spaces are not allowed',
    v =>
      !showEditDialog.value ||
      !isDuplicateFileName(v) ||
      'File name already exists',
  ]
})

function isDuplicateFileName(name) {
  const ignoreNames = ['Signature', 'Thumbprint', 'Portrait']

  if (ignoreNames.includes(name)) {
    return false
  }

  return state.documents.some(
    doc => doc.name === name && doc !== state.itemToEdit
  )
}

function onNameEdit() {
  if (state.itemToEdit) {
    let oldName = state.itemToEdit.name
    let name = editedFileName.value

    let oldNameWithId = `${permitStore.getPermitDetail.userId}_${oldName}`
    let newName = `${permitStore.getPermitDetail.userId}_${name}`

    const index = state.documents.findIndex(doc => doc.name === oldName)

    if (index !== -1) {
      state.documents[index].name = name
    }

    documentStore.editApplicationFileName(oldNameWithId, newName)

    permitStore.updatePermitDetailApi(
      `Updated name of document ${oldName} to ${name}`
    )
  }

  showEditDialog.value = false
  state.itemToEdit = null
}

async function deletePdf() {
  if (state.itemToDelete) {
    documentStore.deleteApplicationFile(state.itemToDelete.name)
    const index = state.documents.indexOf(state.itemToDelete)

    if (index > -1) {
      state.documents.splice(index, 1)
      permitStore.updatePermitDetailApi(
        `Deleted document: ${state.itemToDelete.name}`
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

async function editDialog(item) {
  state.itemToEdit = item
  showEditDialog.value = true
  editedFileName.value = ''
}

function handleSave() {
  emit('on-save', 'Documents')
}
</script>
