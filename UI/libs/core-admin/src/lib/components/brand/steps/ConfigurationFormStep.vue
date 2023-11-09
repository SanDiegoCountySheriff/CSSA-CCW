<template>
  <div>
    <v-card-text>
      <v-form
        ref="form"
        v-model="valid"
      >
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
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
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
            ></v-text-field>
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
                  <v-spacer></v-spacer>
                  <v-dialog v-model="hairDialog">
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
                        <v-container>
                          <v-row>
                            <v-col
                              cols="12"
                              sm="6"
                              md="4"
                            >
                              <v-text-field
                                v-model="editedHairColor.name"
                                label="Hair color"
                              >
                              </v-text-field>
                            </v-col>
                          </v-row>
                        </v-container>
                      </v-card-text>

                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                          color="primary"
                          text
                          @click="closeHairColor"
                        >
                          Cancel
                        </v-btn>
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
                      <v-card-title class="text-h5"
                        >Are you sure you want to delete
                        {{ editedHairColor.name }}?</v-card-title
                      >
                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                          color="primary"
                          text
                          @click="closeDeleteHairColor"
                          >Cancel</v-btn
                        >
                        <v-btn
                          color="primary"
                          text
                          @click="deleteHairColorConfirm"
                          >OK</v-btn
                        >
                        <v-spacer></v-spacer>
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
                  mdi-pencil
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
        </v-row>
        <v-row>
          <v-col>
            <v-data-table
              :items="eyeColors"
              :headers="headers"
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title>Edit Eye Colors</v-toolbar-title>
                  <v-spacer></v-spacer>
                  <v-dialog v-model="eyeDialog">
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
                        <v-container>
                          <v-row>
                            <v-col
                              cols="12"
                              sm="6"
                              md="4"
                            >
                              <v-text-field
                                v-model="editedEyeColor.name"
                                label="Eye color"
                              >
                              </v-text-field>
                            </v-col>
                          </v-row>
                        </v-container>
                      </v-card-text>

                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                          color="primary"
                          text
                          @click="closeEyeColor"
                        >
                          Cancel
                        </v-btn>
                        <v-btn
                          color="primary"
                          text
                          @click="saveEyeColor"
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
                      <v-card-title class="text-h5"
                        >Are you sure you want to delete
                        {{ editedEyeColor.name }}?</v-card-title
                      >
                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                          color="primary"
                          text
                          @click="closeDeleteEyeColor"
                          >Cancel</v-btn
                        >
                        <v-btn
                          color="primary"
                          text
                          @click="deleteEyeColorConfirm"
                          >OK</v-btn
                        >
                        <v-spacer></v-spacer>
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
                  mdi-pencil
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
      </v-form>
    </v-card-text>
    <v-card-actions>
      <v-btn
        @click="handleResetStep"
        color="primary"
      >
        {{ $t('Cancel') }}
      </v-btn>
      <v-btn
        @click="props.handleBackStep"
        color="primary"
      >
        {{ $t('Back') }}
      </v-btn>
      <v-btn
        :disabled="!valid"
        @click="setFormValues"
        color="primary"
      >
        {{ $t('Publish') }}
      </v-btn>
    </v-card-actions>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref, nextTick, computed, set } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useMutation } from '@tanstack/vue-query'
import { closestIndexTo } from 'date-fns'
// import { hairColors } from '@shared-utils/lists/defaultConstants'

interface IAgencyFormStepProps {
  handleNextStep: () => void
  handleBackStep: () => void
  handleResetStep: () => void
}

const brandStore = useBrandStore()
const hairColors = ref([...brandStore.brand.agencyHairColors])
const hairDialog = ref(false)
const hairDialogDelete = ref(false)
const eyeColors = ref([...brandStore.brand.agencyEyeColors])
const eyeDialog = ref(false)
const eyeDialogDelete = ref(false)
const editedHairIndex = ref(-1)
const editedEyeIndex = ref(-1)
const editedEyeColor = ref({ name: '' })
const editedHairColor = ref({ name: '' })
const defaultHairColor = ref({ name: '' })
const defaultEyeColor = ref({ name: '' })

const headers = [
  {
    text: 'Color',
    value: 'name',
  },
  {
    text: 'Actions',
    value: 'actions',
  },
]

const props = withDefaults(defineProps<IAgencyFormStepProps>(), {
  handleNextStep: () => null,
  handleBackStep: () => null,
  handleResetStep: () => null,
})

const valid = ref(false)

const setBrandSettings = useMutation({
  mutationFn: () => brandStore.setBrandSettingApi(),
  onSuccess: () => props.handleNextStep(),
})

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
  closeDeleteHairColor()
}

function deleteEyeColorConfirm() {
  eyeColors.value.splice(editedEyeIndex.value, 1)
  closeDeleteEyeColor()
}

function closeDeleteHairColor() {
  hairDialogDelete.value = false
  editedHairColor.value = { ...defaultHairColor.value }
  editedHairIndex.value = -1
}

function closeDeleteEyeColor() {
  eyeDialogDelete.value = false
  editedEyeColor.value = { ...defaultEyeColor.value }
  editedEyeIndex.value = -1
}

function closeHairColor() {
  hairDialog.value = false
  editedHairColor.value = { ...defaultHairColor.value }
  editedHairIndex.value = -1
}

function closeEyeColor() {
  eyeDialog.value = false
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

  closeHairColor()
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

  closeEyeColor()
}

async function setFormValues() {
  setBrandSettings.mutate()
}
</script>
