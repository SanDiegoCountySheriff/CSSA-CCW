<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-dialog
        width="600"
        v-model="state.dialog"
      >
        <v-card>
          <v-card-title>
            {{ $t('Deny Application') }}
          </v-card-title>

          <v-card-text class="mt-2">
            <v-row>
              <v-col cols="12">
                <v-select
                  v-model="
                    permitStore.permitDetail.application.denialInfo.reason
                  "
                  label="Reason"
                  outlined
                  required
                  :items="['Disqualified Person (PC § 26202)', 'Other']"
                  :rules="[v => !!v || 'Reason is required']"
                ></v-select>
              </v-col>
            </v-row>
            <v-row>
              <v-col cols="12">
                <v-menu
                  v-model="state.menu"
                  :close-on-content-click="false"
                  transition="scale-transition"
                  offset-y
                  min-width="auto"
                >
                  <template #activator="{ on }">
                    <v-text-field
                      v-model="
                        permitStore.permitDetail.application.denialInfo.date
                      "
                      label="Revocation Date"
                      outlined
                      append-icon="mdi-calendar"
                      readonly
                      required
                      v-on="on"
                    ></v-text-field>
                  </template>
                  <v-date-picker
                    v-model="
                      permitStore.permitDetail.application.denialInfo.date
                    "
                    color="primary"
                    no-title
                    @input="state.menu = false"
                    scrollable
                    style="max-width: 290px"
                    required
                  >
                  </v-date-picker>
                </v-menu>
              </v-col>
            </v-row>

            <v-row
              v-if="
                permitStore.permitDetail.application.denialInfo.reason ===
                'Other'
              "
            >
              <v-col cols="12">
                <v-text-field
                  v-model="
                    permitStore.permitDetail.application.denialInfo.otherReason
                  "
                  label="Other Reason"
                  outlined
                  required
                  :rules="[v => !!v || 'Other Reason is required']"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-card-text>

          <v-card-actions>
            <v-btn
              text
              color="error"
              @click="cancelDialog"
            >
              Cancel
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn
              text
              color="primary"
              @click="submitRevocation"
              :disabled="!valid"
            >
              Submit
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-form>
  </div>
</template>

<script lang="ts" setup>
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { reactive, ref } from 'vue'

const permitStore = usePermitsStore()

interface ApprovedEmailApplicantDialog {
  showDialog: boolean
}

const emits = defineEmits(['cancel'])
const valid = ref(false)

const props = defineProps<ApprovedEmailApplicantDialog>()

const state = reactive({
  dialog: props.showDialog,
  menu: false,
  loading: false,
})

function submitRevocation() {
  permitStore.printRevocationLetterApi()
  state.dialog = false
}

function cancelDialog() {
  state.dialog = false
  emits('cancel')
}
</script>
