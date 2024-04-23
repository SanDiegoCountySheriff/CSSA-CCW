<template>
  <v-dialog
    v-model="dialog"
    width="500"
  >
    <template #activator="{ on: dialogActivator, attrs }">
      <v-tooltip bottom>
        <template #activator="{ on: tooltip }">
          <v-btn
            v-if="!props.undoActive"
            v-bind="attrs"
            v-on="{ ...tooltip, ...dialogActivator }"
            color="success"
            class="mr-2"
            icon
          >
            <v-icon> mdi-check-bold </v-icon>
          </v-btn>
          <!-- <v-btn
          v-else
          v-bind="attrs"
          v-on="on"
          color="success"
          class="mr-2"
          icon
        >
          <v-icon> mdi-undo </v-icon>
        </v-btn> -->
        </template>
        <span v-if="!props.undoActive"> Check In </span>
        <span v-else>Undo Check In</span>
      </v-tooltip>
  
      <v-tooltip bottom>
        <template #activator="{ on: tooltip }">
          <v-btn
            v-if="!props.undoActive"
            v-bind="attrs"
            v-on="{ ...tooltip, ...dialogActivator }"
            color="error"
            class="mr-2"
            icon
          >
            <v-icon> mdi-close-thick </v-icon>
          </v-btn>
          <!-- <v-btn
          v-else
          v-bind="attrs"
          v-on="on"
          color="success"
          class="mr-2"
          icon
        >
          <v-icon> mdi-undo </v-icon>
        </v-btn> -->
        </template>
        <span v-if="!props.undoActive"> No Show </span>
        <span v-else>Undo No Show</span>
      </v-tooltip>
    </template>
    <!-- <v-tooltip bottom>
      <template #activator="{ on, attrs }">
        <v-btn
          v-if="!props.undoActive"
          v-bind="attrs"
          v-on="on"
          color="error"
          class="mr-2"
          icon
        >
          <v-icon> mdi-close-thick </v-icon>
        </v-btn>
        <v-btn
          v-else
          v-bind="attrs"
          v-on="on"
          color="error"
          class="mr-2"
          icon
        >
          <v-icon>mdi-undo</v-icon>
        </v-btn>
      </template>
      <span v-if="!props.undoActive">No Show</span>
      <span v-else>Undo No Show</span>
    </v-tooltip> -->

    <v-card>
      <v-card-title> {{ getTitle }} </v-card-title>

      <v-card-text> template test </v-card-text>

      <v-card-actions>
        <v-btn
          color="primary"
          text
        >
          Yes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts" setup>
import { computed, ref } from 'vue'

interface AppointmentActionConfirmationDialog {
  undoActive: boolean
  checkIn: boolean
  title: string
}

const props = defineProps<AppointmentActionConfirmationDialog>()

const dialog = ref(false)

const getTitle = computed(() => {
  return props.undoActive ? `Undo ${props.title}` : props.title
})
</script>
