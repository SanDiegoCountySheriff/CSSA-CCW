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
            text
            @click="handleSubmit"
          >
            Submit
          </v-btn>
          <v-btn
            color="error"
            text
            @click="state.dialog = false"
          >
            Cancel
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts" setup>
import { AppointmentType } from '@shared-utils/types/defaultTypes';
import { reactive } from 'vue';
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';

interface AppointmentDeleteDialog {
  appointment: AppointmentType;
  refetch: CallableFunction;
}

const props = defineProps<AppointmentDeleteDialog>();

const appointmentStore = useAppointmentsStore();

const state = reactive({
  dialog: false,
  enteredName: '',
});

function handleSubmit() {
  appointmentStore.deleteUserFromAppointment(props.appointment.id).then(() => {
    props.refetch();
    state.dialog = false;
  });
}
</script>
