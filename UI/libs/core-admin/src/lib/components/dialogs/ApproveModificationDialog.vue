<template>
  <div>
    <v-dialog
      v-model="state.dialog"
      persistent
      max-width="600"
    >
      <v-card>
        <v-card-title>Approve Modification</v-card-title>
        <v-card-text>
          <v-row>
            <v-col>
              <v-alert
                type="warning"
                dense
                outlined
              >
                Are you sure you want to approve the modification? You will not
                be able to make any changes to the weapons once the modification
                has been approved.
              </v-alert>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-btn
            color="error"
            text
            @click="cancelDialog"
          >
            Cancel
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            color="primary"
            text
            @click="handleApprove"
          >
            Approve Modification
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'

interface ApproveModificationDialog {
  showDialog: boolean
}

const emits = defineEmits(['cancel', 'approved'])

const props = defineProps<ApproveModificationDialog>()

const state = reactive({
  dialog: props.showDialog,
})

function cancelDialog() {
  state.dialog = false
  emits('cancel')
}

function handleApprove() {
  state.dialog = false
  emits('approved')
}
</script>
