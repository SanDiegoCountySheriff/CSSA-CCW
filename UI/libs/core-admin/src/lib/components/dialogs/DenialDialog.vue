<template>
  <div>
    <v-dialog
      width="800"
      v-model="state.dialog"
    >
      <v-card>
        <v-card-title>
          {{ $t('Denied') }}
        </v-card-title>

        <v-card-text>
          <v-row>
            <v-col cols="8">
              <v-select
                v-model="permitStore.permitDetail.application.denialInfo.reason"
                label="Reason"
                required
                :items="['Disqualified Person (PC § 26202)', 'Other']"
                :rules="[v => !!v || 'Reason is required']"
              ></v-select>
            </v-col>
            <v-col cols="4">
              <v-menu
                v-model="state.menu"
                :close-on-content-click="false"
                transition="scale-transition"
                offset-y
                min-width="auto"
              >
                <template #activator="{ on, attrs }">
                  <v-text-field
                    v-model="
                      permitStore.permitDetail.application.denialInfo.date
                    "
                    :label="$t('Date Revoked')"
                    hint="YYYY-MM-DD format"
                    persistent-hint
                    v-bind="attrs"
                    v-on="on"
                    outlined
                    dense
                    :rules="[v => !!v || 'Date is required']"
                  >
                    <template #append>
                      <v-icon> mdi-calendar </v-icon>
                      <v-icon
                        color="error"
                        medium
                        v-if="
                          !permitStore.permitDetail.application.denialInfo.date
                        "
                      >
                        mdi-alert-octagon
                      </v-icon>
                    </template>
                  </v-text-field>
                </template>
                <v-date-picker
                  v-model="permitStore.permitDetail.application.denialInfo.date"
                  color="primary"
                  no-title
                  @input="state.menu = false"
                  scrollable
                  required
                >
                </v-date-picker>
              </v-menu>
            </v-col>
          </v-row>
          <v-row
            v-if="
              permitStore.permitDetail.application.denialInfo.reason === 'Other'
            "
          >
            <v-col cols="12">
              <v-text-field
                v-model="
                  permitStore.permitDetail.application.denialInfo.otherReason
                "
                label="Other Reason"
                required
                :rules="[v => !!v || 'Other Reason is required']"
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row> </v-row>
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
          >
            Submit
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const permitStore = usePermitsStore()

interface ApprovedEmailApplicantDialog {
  showDialog: boolean
}

const emits = defineEmits(['cancel'])

const props = defineProps<ApprovedEmailApplicantDialog>()

const state = reactive({
  dialog: props.showDialog,
  menu: false,
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
