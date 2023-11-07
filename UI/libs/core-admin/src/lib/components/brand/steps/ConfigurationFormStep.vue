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
                                v-model="editedItem.name"
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
                          @click="close"
                        >
                          Cancel
                        </v-btn>
                        <v-btn
                          color="primary"
                          text
                          @click="save"
                        >
                          Save
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-dialog>
                  <v-dialog
                    v-model="dialogDelete"
                    max-width="500px"
                  >
                    <v-card>
                      <v-card-title class="text-h5"
                        >Are you sure you want to delete
                        {{ editedItem.name }}?</v-card-title
                      >
                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                          color="primary"
                          text
                          @click="closeDelete"
                          >Cancel</v-btn
                        >
                        <v-btn
                          color="primary"
                          text
                          @click="deleteValueConfirm"
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
                  @click="editItem(item)"
                >
                  mdi-pencil
                </v-icon>
                <v-icon
                  small
                  @click="deleteValue(item)"
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
import { reactive, ref, nextTick, computed } from 'vue'
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
const dialogDelete = ref(false)
const editedIndex = ref(-1)
const editedItem = ref({ name: '' })
const defaultItem = ref({ name: '' })

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

function editItem(value) {
  editedIndex.value = hairColors.value.indexOf(value)
  editedItem.value = { ...value }
  hairDialog.value = true
}

function deleteValue(value) {
  editedIndex.value = hairColors.value.indexOf(value)
  editedItem.value = { ...value }
  dialogDelete.value = true
}

function deleteValueConfirm() {
  hairColors.value.splice(editedIndex.value, 1)
  closeDelete()
}

function closeDelete() {
  dialogDelete.value = false
  editedItem.value = {...defaultItem.value }
  editedIndex.value = -1
}

function close() {
  hairDialog.value = false
  editedItem.value = { ...defaultItem.value }
  editedIndex.value = -1
}

function save() {
  if (editedIndex.value > -1) {
    Object.assign(hairColors[editedIndex.value], editedItem)
  } else {
    brandStore.brand.agencyHairColors.push({ name: editedItem.value.name })
    hairColors.value.push({ name: editedItem.value.name })
  }

  close()
}

async function setFormValues() {
  setBrandSettings.mutate()
}
</script>
