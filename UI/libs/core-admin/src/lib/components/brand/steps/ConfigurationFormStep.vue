<template>
  <v-card :loading="loading.value">
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title>
        Agency Configuration Settings
        <v-spacer />

        <v-btn
          :disabled="!valid"
          @click="save"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>Save
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col
            sm="6"
            cols="12"
          >
            <v-text-field
              v-model.number="brandStore.brand.expiredApplicationRenewalPeriod"
              label="Expired application renewal grace period, days"
              hint="Expired applications will have this many days after expiration to submit their renewal"
              :rules="[
                v =>
                  (v !== undefined && v !== null) ||
                  v === 0 ||
                  'An expiration renewal period is required',
              ]"
              type="number"
              color="primary"
              outlined
            />
          </v-col>

          <v-col
            sm="6"
            cols="12"
          >
            <v-text-field
              v-model.number="
                brandStore.brand.archivedApplicationRetentionPeriod
              "
              label="Archived application retention period, years"
              hint="Applications that are inactive will be retained for this many years"
              :rules="[
                v =>
                  (v !== undefined && v !== null) ||
                  v === 0 ||
                  'An expiration renewal period is required',
              ]"
              type="number"
              color="primary"
              outlined
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col
            sm="6"
            cols="12"
          >
            <v-text-field
              v-model.number="brandStore.brand.daysBeforeActiveRenewal"
              label="Renewal threshold, days"
              hint="Applicants will be able to renew this many days before license expiration date"
              :rules="[
                v =>
                  (v !== undefined && v !== null) ||
                  v >= 0 ||
                  'An expiration renewal period is required',
              ]"
              type="number"
              color="primary"
              outlined
            />
          </v-col>

          <v-col>
            <v-text-field
              v-model.number="
                brandStore.brand.numberOfModificationsBetweenRenewals
              "
              :rules="[
                v =>
                  (v !== undefined && v !== null) ||
                  v >= 0 ||
                  'Number of modifications between renewals is required',
              ]"
              label="Number of modifications between renewals"
              hint="This does not include address changes, only names and weapons"
              type="number"
              color="primary"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-toolbar-title>Use Employment License</v-toolbar-title>
            <v-radio-group v-model="brandStore.brand.employmentLicense">
              <v-radio
                label="Yes"
                :value="true"
              ></v-radio>
              <v-radio
                label="No"
                :value="false"
              ></v-radio>
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-data-table
              :items="hairColors"
              :headers="headers"
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title>Edit Hair Colors</v-toolbar-title>

                  <v-spacer />

                  <v-dialog
                    v-model="hairDialog"
                    max-width="600px"
                  >
                    <template #activator="{ on, attrs }">
                      <v-btn
                        color="primary"
                        dark
                        class="mb-2"
                        v-bind="attrs"
                        v-on="on"
                      >
                        New Item
                      </v-btn>
                    </template>

                    <v-card>
                      <v-card-text>
                        <v-card-title> New Item</v-card-title>

                        <v-text-field
                          v-model="editedHairColor.name"
                          label="Hair color"
                          outlined
                        >
                        </v-text-field>
                      </v-card-text>

                      <v-card-actions>
                        <v-btn
                          color="primary"
                          text
                          @click="closeDialog"
                        >
                          Cancel
                        </v-btn>

                        <v-spacer />

                        <v-btn
                          color="primary"
                          text
                          @click="saveHairColor"
                        >
                          Save
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-dialog>

                  <v-dialog
                    v-model="hairDialogDelete"
                    max-width="500px"
                  >
                    <v-card>
                      <v-card-title class="text-h5">
                        Are you sure you want to delete
                        {{ editedHairColor.name }}?
                      </v-card-title>

                      <v-card-actions>
                        <v-spacer />

                        <v-btn
                          color="primary"
                          text
                          @click="closeDialog"
                        >
                          Cancel
                        </v-btn>

                        <v-spacer />

                        <v-btn
                          color="primary"
                          text
                          @click="deleteHairColorConfirm"
                        >
                          OK
                        </v-btn>

                        <v-spacer />
                      </v-card-actions>
                    </v-card>
                  </v-dialog>
                </v-toolbar>
              </template>

              <template #[`item.actions`]="{ item }">
                <v-icon
                  small
                  class="mr-2"
                  @click="editHairColor(item)"
                >
                  mdi-pencil-box-outline
                </v-icon>

                <v-icon
                  small
                  @click="deleteHairColor(item)"
                >
                  mdi-delete
                </v-icon>
              </template>
            </v-data-table>
          </v-col>

          <v-col>
            <v-data-table
              :items="eyeColors"
              :headers="headers"
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title>Edit Eye Colors</v-toolbar-title>

                  <v-spacer />

                  <v-dialog
                    v-model="eyeDialog"
                    max-width="600px"
                  >
                    <template #activator="{ on, attrs }">
                      <v-btn
                        color="primary"
                        dark
                        class="mb-2"
                        v-bind="attrs"
                        v-on="on"
                      >
                        New Item
                      </v-btn>
                    </template>

                    <v-card>
                      <v-card-title> New Item</v-card-title>

                      <v-card-text>
                        <v-text-field
                          v-model="editedEyeColor.name"
                          label="Eye color"
                          outlined
                        >
                        </v-text-field>
                      </v-card-text>

                      <v-card-actions>
                        <v-btn
                          @click="closeDialog"
                          color="primary"
                          text
                        >
                          Cancel
                        </v-btn>

                        <v-spacer />

                        <v-btn
                          @click="saveEyeColor"
                          color="primary"
                          text
                        >
                          Save
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-dialog>

                  <v-dialog
                    v-model="eyeDialogDelete"
                    max-width="500px"
                  >
                    <v-card>
                      <v-card-title class="text-h5">
                        Are you sure you want to delete
                        {{ editedEyeColor.name }}?
                      </v-card-title>

                      <v-card-actions>
                        <v-spacer />

                        <v-btn
                          color="primary"
                          text
                          @click="closeDialog"
                        >
                          Cancel
                        </v-btn>

                        <v-spacer />

                        <v-btn
                          color="primary"
                          text
                          @click="deleteEyeColorConfirm"
                        >
                          OK
                        </v-btn>

                        <v-spacer />
                      </v-card-actions>
                    </v-card>
                  </v-dialog>
                </v-toolbar>
              </template>

              <template #[`item.actions`]="{ item }">
                <v-icon
                  small
                  class="mr-2"
                  @click="editEyeColor(item)"
                >
                  mdi-pencil-box-outline
                </v-icon>

                <v-icon
                  small
                  @click="deleteEyeColor(item)"
                >
                  mdi-delete
                </v-icon>
              </template>
            </v-data-table>
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useTanstack } from '@shared-ui/composables/useTanstack'
import { ref, set } from 'vue'

const { loading, setBrandSettings } = useTanstack()

const brandStore = useBrandStore()
const valid = ref(false)

function save() {
  setBrandSettings()
}

const hairColors = ref([...brandStore.brand.agencyHairColors])
const hairDialog = ref(false)
const hairDialogDelete = ref(false)
const editedHairColor = ref({ name: '' })
const defaultHairColor = ref({ name: '' })
const editedHairIndex = ref(-1)
const eyeColors = ref([...brandStore.brand.agencyEyeColors])
const eyeDialog = ref(false)
const eyeDialogDelete = ref(false)
const editedEyeColor = ref({ name: '' })
const defaultEyeColor = ref({ name: '' })
const editedEyeIndex = ref(-1)

const headers = [
  {
    text: 'Color',
    value: 'name',
  },
  {
    text: 'Actions',
    value: 'actions',
    width: '10%',
  },
]

function editHairColor(value) {
  editedHairIndex.value = hairColors.value.indexOf(value)
  editedHairColor.value = { ...value }
  hairDialog.value = true
}

function editEyeColor(value) {
  editedEyeIndex.value = eyeColors.value.indexOf(value)
  editedEyeColor.value = { ...value }
  eyeDialog.value = true
}

function deleteHairColor(value) {
  editedHairIndex.value = hairColors.value.indexOf(value)
  editedHairColor.value = { ...value }
  hairDialogDelete.value = true
}

function deleteEyeColor(value) {
  editedEyeIndex.value = eyeColors.value.indexOf(value)
  editedEyeColor.value = { ...value }
  eyeDialogDelete.value = true
}

function deleteHairColorConfirm() {
  hairColors.value.splice(editedHairIndex.value, 1)
  brandStore.brand.agencyHairColors.splice(editedHairIndex.value, 1)
  hairDialogDelete.value = false
  editedHairColor.value = { ...defaultHairColor.value }
  editedHairIndex.value = -1
}

function deleteEyeColorConfirm() {
  eyeColors.value.splice(editedEyeIndex.value, 1)
  brandStore.brand.agencyEyeColors.splice(editedEyeIndex.value, 1)
  eyeDialogDelete.value = false
  editedEyeColor.value = { ...defaultEyeColor.value }
  editedEyeIndex.value = -1
}

function closeDialog() {
  hairDialog.value = false
  hairDialogDelete.value = false
  editedHairColor.value = { ...defaultHairColor.value }
  editedHairIndex.value = -1
  eyeDialog.value = false
  eyeDialogDelete.value = false
  editedEyeColor.value = { ...defaultEyeColor.value }
  editedEyeIndex.value = -1
}

function saveHairColor() {
  if (editedHairIndex.value > -1) {
    set(hairColors.value, editedHairIndex.value, { ...editedHairColor.value })
    set(brandStore.brand.agencyHairColors, editedHairIndex.value, {
      ...editedHairColor.value,
    })
  } else {
    brandStore.brand.agencyHairColors.push({ ...editedHairColor.value })
    hairColors.value.push({ ...editedHairColor.value })
  }

  closeDialog()
}

function saveEyeColor() {
  if (editedEyeIndex.value > -1) {
    set(eyeColors.value, editedEyeIndex.value, { ...editedEyeColor.value })
    set(brandStore.brand.agencyEyeColors, editedEyeIndex.value, {
      ...editedEyeColor.value,
    })
  } else {
    brandStore.brand.agencyEyeColors.push({ ...editedEyeColor.value })
    eyeColors.value.push({ ...editedEyeColor.value })
  }

  closeDialog()
}
</script>
