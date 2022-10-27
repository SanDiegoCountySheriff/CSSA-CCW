<!-- eslint-disable vue/valid-v-slot -->
<!-- eslint-disable vue-a11y/no-autofocus -->
<template>
  <div class="appointment-table">
    <div>
      <v-card-title>
        {{ $t('Appointments') }}
        <v-spacer></v-spacer>
        <v-text-field
          v-model="state.search"
          append-icon="mdi-magnify"
          label="Search"
          single-line
          hide-details
        ></v-text-field>
      </v-card-title>
      <v-data-table
        :headers="state.headers"
        :items="data"
        :search="state.search"
        :loading="isLoading && !isError"
        :loading-text="$t('Loading appointment schedules...')"
        items-per-page="15"
        item-class="rowClass"
      >
        <template #item.status="props">
          <v-edit-dialog
            :return-value.sync="props.item.status"
            @save="save"
            @cancel="cancel"
            @open="open"
            @close="close"
          >
            <v-chip
              :color="getColor(props.item.status)"
              :text-color="getTextColor(props.item.status)"
            >
              {{ props.item.status }}
            </v-chip>
            <template #input>
              <v-text-field
                v-model="props.item.status"
                :rules="[state.max25chars]"
                label="Edit"
                single-line
                counter
              >
              </v-text-field>
            </template>
          </v-edit-dialog>
        </template>
        <template #item.date="props">
          <v-edit-dialog
            :return-value.sync="props.item.date"
            large
            persistent
            @save="save"
            @cancel="cancel"
            @open="open"
            @close="close"
          >
            <div>{{ props.item.date }}</div>
            <template #input>
              <div class="mt-4 text-h6">
                {{ $t('Update Date') }}
              </div>
              <v-text-field
                v-model="props.item.date"
                :rules="[state.max25chars]"
                label="Edit"
                single-line
                counter
                autofocus
              >
              </v-text-field>
            </template>
          </v-edit-dialog>
        </template>
        <template #item.time="props">
          <v-edit-dialog
            :return-value.sync="props.item.time"
            large
            persistent
            @save="save"
            @cancel="cancel"
            @open="open"
            @close="close"
          >
            <div>{{ props.item.time }}</div>
            <template #input>
              <div class="mt-4 text-h6">
                {{ $t('Update Time') }}
              </div>
              <v-text-field
                v-model="props.item.time"
                :rules="[state.max25chars]"
                label="Edit"
                single-line
                counter
                autofocus
              >
              </v-text-field>
            </template>
          </v-edit-dialog>
        </template>
      </v-data-table>

      <v-snackbar
        v-model="state.snack"
        :timeout="3000"
        :color="state.snackColor"
      >
        {{ state.snackText }}

        <template #action="{ attrs }">
          <v-btn
            v-bind="attrs"
            text
            @click="state.snack = false"
          >
            {{ $t('Close') }}
          </v-btn>
        </template>
      </v-snackbar>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useQuery } from '@tanstack/vue-query';
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';

const { getAppointmentsApi } = useAppointmentsStore();
const { isLoading, isError, data } = useQuery(
  ['appointments'],
  getAppointmentsApi
);

const state = reactive({
  search: '',
  snack: false,
  snackColor: '',
  snackText: '',
  max25chars: v => v.length <= 25 || 'Input too long!',
  pagination: {},
  headers: [
    {
      text: 'Status',
      align: 'start',
      sortable: false,
      value: 'status',
    },
    { text: 'Applicant Name', value: 'name' },
    { text: 'Permit', value: 'permit' },
    { text: 'Payment', value: 'payment' },
    { text: 'Date', value: 'date' },
    { text: 'Time', value: 'time' },
    { text: '', value: '' },
  ],
});

function save() {
  state.snack = true;
  state.snackColor = 'success';
  state.snackText = 'Data saved';
}

function cancel() {
  state.snack = true;
  state.snackColor = 'error';
  state.snackText = 'Canceled';
}

function open() {
  state.snack = true;
  state.snackColor = 'info';
  state.snackText = 'Dialog opened';
}

function close() {
  window.console.log('Dialog closed');
}

function getColor(name) {
  if (name === 'New') return '#eff8ff';

  return '#f2f4f7';
}

function getTextColor(name) {
  if (name === 'New') return '#2e90fa';

  return '#667085';
}
</script>

<style lang="scss">
@media (min-width: 600px) {
  .appointment-table {
    .v-card {
      &__title {
        font-size: 20px;
      }
    }

    &__row {
      height: 60px;

      td:first-child {
        width: 9.1%;
      }

      td:nth-child(2) {
        width: 32%;
      }

      td:nth-child(3) {
        width: 10%;
      }
      td:nth-child(4) {
        width: 10%;
      }
      td:nth-child(5) {
        width: 9%;
      }
      td:nth-child(6) {
        width: 12%;
      }
    }
  }
}

.appointment-table {
  thead > tr {
    background: #f2f4f7;
  }
}
</style>
