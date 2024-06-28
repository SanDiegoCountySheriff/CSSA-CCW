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
            <td>
              {{ item.name }}
            </td>
            <v-dialog
              v-model="state.showEditDialog"
              max-width="600px"
            >
              <v-card outlined>
                <v-card-title class="headline">
                  Rename {{ item.name }}
                </v-card-title>

                <v-card-text>
                  <v-row>
                    <v-col>
                      <v-text-field
                        v-model="editedFileName"
                        :rules="fileNameRules"
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
                    @click="state.showEditDialog = false"
                  >
                    Cancel
                  </v-btn>

                  <v-spacer />

                  <v-btn
                    :disabled="!valid"
                    color="primary"
                    text
                    @click="onNameEdit(item, editedFileName)"
                  >
                    Confirm Edit
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
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
              @click="editDialog(item)"
              class="mx-1"
              color="primary"
            >
              mdi-pencil
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
      </v-form>
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
  showEditDialog: false,
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
    v => Boolean(v) || 'File name is required',
    v => !/[#%&{}/\\<>*$'":@+`|=~?!]/.test(v) || 'File name is invalid',
    v => !/\s/.test(v) || 'File name is invalid',
  ]
})

function onNameEdit(item, name) {
  let oldName = item.name

  item.name = name
  let oldNameWithId = `${permitStore.getPermitDetail.userId}_${oldName}`
  let newName = `${permitStore.getPermitDetail.userId}_${name}`

  documentStore.editApplicationFileName(oldNameWithId, newName)

  permitStore.updatePermitDetailApi(
    `Updated name of document ${oldName} to ${name}`
  )

  state.showEditDialog = false
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
  state.showEditDialog = true
}

function handleSave() {
  emit('on-save', 'Documents')
}
</script>
