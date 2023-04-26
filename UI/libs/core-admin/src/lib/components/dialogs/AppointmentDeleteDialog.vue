<template>
  <div>
    <v-dialog
      width="600"
      v-model="state.dialog"
    >
      <template #activator="{ on, attrs }">
        <v-icon
          medium
          color="error"
          v-bind="attrs"
          v-on="on"
        >
          mdi-delete-empty
        </v-icon>
      </template>
      <v-card>
        <v-card-title>
          {{ $t('Confirm Delete') }}
        </v-card-title>
        <v-card-text>
          <v-banner>
            {{ $t(' To delete applicant from this appointment slot enter: ') }}
            {{ props.appointment.name }}
          </v-banner>
          <v-text-field
            outlined
            dense
            :label="$t('Confirm Name')"
            :rules="[
              v =>
                v === props.appointment.name.trim() ||
                $t('Entered text must match applicants name'),
            ]"
            v-model="state.enteredName"
          >
          </v-text-field>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            :disabled="state.enteredName !== props.appointment.name.trim()"
            color="info"
            :loading="state.loading"
            text
            @click="handleSubmit"
          >
            Submit
          </v-btn>
          <v-btn
            color="error"
            text
            :loading="state.loading"
            @click="state.dialog = false"
          >
            Cancel
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-snackbar
      color="error"
      v-model="state.error"
      :timeout="5000"
      class="font-weight-bold"
    >
      {{ $t('Error deleting applicant from appointment') }}
    </v-snackbar>
    <v-snackbar
      color="success"
      v-model="state.success"
      :timeout="5000"
      class="font-weight-bold"
    >
      {{ $t('Applicatant deleted from this appointment slot') }}
    </v-snackbar>
  </div>
</template>

<script lang="ts" setup>
import { AppointmentType } from '@shared-utils/types/defaultTypes'
import { reactive } from 'vue'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'

interface AppointmentDeleteDialog {
  appointment: AppointmentType
  refetch: CallableFunction
}

const props = defineProps<AppointmentDeleteDialog>()

const appointmentStore = useAppointmentsStore()

const state = reactive({
  dialog: false,
  enteredName: '',
  loading: false,
  success: false,
  error: false,
})

function handleSubmit() {
  state.loading = true
  appointmentStore
    .deleteUserFromAppointment(props.appointment.id)
    .then(() => {
      state.loading = false
      state.success = true
      props.refetch()
      state.dialog = false
    })
    .catch(err => {
      state.loading = false
      state.error = true
    })
}
</script>
