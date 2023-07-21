<template>
  <v-dialog
    v-model="state.dialog"
    max-width="800"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        color="primary"
        small
        block
      >
        <v-icon left>mdi-clipboard-account</v-icon>
        Assign Application
      </v-btn>
    </template>

    <v-card>
      <v-card-title>
        Assign Application To User
        <v-spacer></v-spacer>
        <v-btn
          v-if="permitStore.getPermitDetail.application.assignedTo"
          @click="handleUnassignApplication"
          color="primary"
        >
          Unassign
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col> </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-autocomplete
              v-model="permitStore.getPermitDetail.application.assignedTo"
              :items="adminUserStore.allAdminUsers"
              @input="handleAssignApplication"
              label="Assign Application"
              item-text="name"
              item-value="name"
              clearable
              outlined
              dense
              color="primary"
            ></v-autocomplete>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          color="primary"
          @click="state.dialog = !state.dialog"
        >
          Close
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useMutation } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const permitStore = usePermitsStore()
const adminUserStore = useAdminUserStore()

const state = reactive({
  dialog: false,
  changed: '',
})

const { mutate: updatePermitDetails } = useMutation({
  mutationFn: () =>
    permitStore.updatePermitDetailApi(`Updated ${state.changed}`),
})

function handleUnassignApplication() {
  state.changed = 'Removed Assigned User from Application'
  permitStore.getPermitDetail.application.assignedTo = ''
  updatePermitDetails()
  state.dialog = false
}

function handleAssignApplication() {
  state.changed = 'Assigned User to Application'
  updatePermitDetails()
  state.dialog = false
}
</script>
