<template>
  <div>
    <v-dialog
      width="800"
      v-model="state.dialog"
    >
      <v-card>
        <v-card-title>
          {{ $t('Revoked / Canceled / Denied') }}
        </v-card-title>

        <v-card-text>
          <v-row>
            <v-col cols="8">
              <v-text-field
                v-model="state.revocationLetterReason"
                label="Reason"
                required
              ></v-text-field>
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
                    v-model="state.revocationDate"
                    :label="$t('Date Revoked')"
                    hint="YYYY-MM-DD format"
                    persistent-hint
                    v-bind="attrs"
                    v-on="on"
                    outlined
                    dense
                  >
                    <template #append>
                      <v-icon> mdi-calendar </v-icon>
                      <v-icon
                        color="error"
                        medium
                        v-if="!state.revocationDate"
                      >
                        mdi-alert-octagon
                      </v-icon>
                    </template>
                  </v-text-field>
                </template>
                <v-date-picker
                  v-model="state.revocationDate"
                  color="primary"
                  no-title
                  @input="state.menu = false"
                  scrollable
                >
                </v-date-picker>
              </v-menu>
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
  revocationLetterReason: '',
  revocationDate: '',
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
