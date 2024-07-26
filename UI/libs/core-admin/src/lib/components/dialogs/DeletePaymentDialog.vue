<template>
  <v-dialog
    v-if="item.paymentStatus === 1"
    v-model="dialog"
    max-width="600px"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        :disabled="loading"
        v-on="on"
        v-bind="attrs"
        color="primary"
        small
      >
        <v-icon left>mdi-delete</v-icon>
        Delete
      </v-btn>
    </template>

    <v-card>
      <v-card-title> Delete Transaction {{ item.transactionId }} </v-card-title>

      <v-card-text>
        Are you sure you wish to delete transaction
        {{ item.transactionId }} for ${{ item.amount }}? This cannot be undone.
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = !dialog"
          color="error"
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          @click="handleDeleteTransaction()"
          color="primary"
        >
          Yes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>

  <v-dialog
    v-else-if="item.paymentStatus === 2 && !item.verified"
    v-model="dialog"
    max-width="600px"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        :disabled="loading"
        v-on="on"
        v-bind="attrs"
        color="primary"
        small
      >
        <v-icon left>mdi-delete</v-icon>
        Delete
      </v-btn>
    </template>

    <v-card>
      <v-card-title> Delete Transaction {{ item.transactionId }} </v-card-title>

      <v-card-text>
        Are you sure you wish to delete this unverified Heartland transaction
        for ${{ item.amount }}? This cannot be undone.
      </v-card-text>

      <v-col>
        <v-checkbox
          class="ml-3"
          v-model="acknowledgeLookup"
          label="I have looked this payment up on Heartland and it doesn't exist"
        ></v-checkbox>

        <v-checkbox
          class="ml-3"
          v-model="acknowledgeTimeline"
          label="I have waited 2-3 days and confirm this transaction doesn't exist"
        ></v-checkbox>

        <v-checkbox
          class="ml-3"
          v-model="acknowledgeDeletion"
          label="I acknowledge this action cannot be undone"
        ></v-checkbox>
      </v-col>

      <v-card-actions>
        <v-btn
          @click="dialog = !dialog"
          color="error"
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          :disabled="
            !acknowledgeLookup || !acknowledgeTimeline || !acknowledgeDeletion
          "
          @click="handleDeleteHeartlandTransaction()"
          color="primary"
        >
          Yes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { PaymentHistoryType } from '@shared-utils/types/defaultTypes'
import { ref } from 'vue'

interface DeleteDialogProps {
  item: PaymentHistoryType
  index: number
  loading: boolean
}

const props = defineProps<DeleteDialogProps>()

const emit = defineEmits(['confirm', 'confirm-heartland'])

const dialog = ref(false)
const acknowledgeLookup = ref(false)
const acknowledgeTimeline = ref(false)
const acknowledgeDeletion = ref(false)

function handleDeleteTransaction() {
  emit('confirm', props.index)
  dialog.value = false
}

function handleDeleteHeartlandTransaction() {
  emit('confirm-heartland', props.index)
  dialog.value = false

  acknowledgeLookup.value = false
  acknowledgeTimeline.value = false
  acknowledgeDeletion.value = false
}
</script>
