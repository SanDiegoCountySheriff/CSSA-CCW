<!-- eslint-disable vue-a11y/form-has-label -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/valid-v-slot -->
<!-- eslint-disable vue-a11y/no-autofocus -->
<template>
  <div class="appointment-table">
    <v-data-table
      :headers="state.headers"
      :items="data"
      :search="state.search"
      :loading="isLoading && !isError"
      :loading-text="$t('Loading appointment schedules...')"
      :single-expand="state.singleExpand"
      :expanded.sync="state.expanded"
      :items-per-page="15"
      :footer-props="{
        showCurrentPage: true,
        showFirstLastPage: true,
        firstIcon: 'mdi-skip-backward',
        lastIcon: 'mdi-skip-forward',
        prevIcon: 'mdi-minus',
        nextIcon: 'mdi-plus',
      }"
      item-key="id"
      item-class="rowClass"
      show-expand
    >
      <template #top>
        <v-toolbar flat>
          <v-toolbar-title>{{ $t('Appointments') }}</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-container class="appointment-table__header__container">
            <v-row justify="end">
              <v-col md="4">
                <v-text-field
                  v-model="state.search"
                  prepend-icon="mdi-filter"
                  label="Filter"
                  placeholder="Start typing to filter"
                  single-line
                  hide-details
                >
                </v-text-field>
              </v-col>
              <v-col
                md="4"
                class="mr-1"
              >
                <!-- 1. Create the button that will be clicked to select a file -->
                <v-btn
                  color="primary"
                  :loading="state.isSelecting"
                  @click="handleFileImport"
                >
                  Upload New Appointments
                </v-btn>

                <!-- Create a File Input that will be hidden but triggered with JavaScript -->
                <input
                  ref="uploader"
                  label="Upload New Appointments"
                  class="d-none"
                  type="file"
                  @change="onFileChanged"
                />
              </v-col>
            </v-row>
          </v-container>
        </v-toolbar>
      </template>
      <template #item.status="props">
        <v-chip
          v-if="props.item.status.length !== 0"
          :color="getColor(props.item.status)"
          :text-color="getTextColor(props.item.status)"
          class="ma-0 font-weight-regular"
          small
        >
          {{ props.item.status }}
        </v-chip>
        <v-icon
          color="error"
          medium
          v-else
        >
          mdi-alert-octagon
        </v-icon>
      </template>
      <template #item.name="props">
        <v-avatar
          color="blue"
          size="30"
          class="mr-1"
        >
          <span class="white--text .text-xs-caption"> {{ $t('JS') }}</span>
        </v-avatar>
        {{ props.item.name }}
      </template>
      <template #item.permit="props">
        <v-chip
          v-if="props.item.permit.length !== 0"
          :color="getColor(props.item.permit)"
          :text-color="getTextColor(props.item.permit)"
          class="ma-0"
          small
        >
          {{ props.item.permit }}
        </v-chip>
        <v-icon
          color="error"
          medium
          v-else
        >
          mdi-alert-octagon
        </v-icon>
      </template>
      <template #item.payment="props">
        <v-chip
          v-if="props.item.payment.length !== 0"
          :color="getColor(props.item.payment)"
          :text-color="getTextColor(props.item.payment)"
          class="ma-0"
          small
        >
          {{ props.item.payment }}
        </v-chip>
        <v-icon
          color="error"
          medium
          v-else
        >
          mdi-alert-octagon
        </v-icon>
      </template>
      <template #expanded-item="{ item }">
        <td colspan="2">
          {{ $t(`More info about ${item.name}`) }}
        </td>
      </template>
    </v-data-table>
  </div>
</template>

<script setup lang="ts">
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';
import { useQuery } from '@tanstack/vue-query';
import { reactive, ref } from 'vue';

const appointmentsStore = useAppointmentsStore();
const { isLoading, isError, data } = useQuery(
  ['appointments'],
  appointmentsStore.getAppointmentsApi
);

const uploader = ref(null);

const state = reactive({
  isSelecting: false,
  search: '',
  singleExpand: true,
  expanded: [],
  snack: false,
  snackColor: '',
  snackText: '',
  pagination: {},
  headers: [
    {
      text: 'STATUS',
      align: 'start',
      sortable: false,
      value: 'status',
    },
    { text: 'APPLICANT NAME', value: 'name' },
    { text: 'PERMIT', value: 'permit' },
    { text: 'PAYMENT', value: 'payment' },
    { text: 'DATE', value: 'date' },
    { text: 'TIME', value: 'time' },
    { text: '', value: '' },
  ],
});

function handleFileImport() {
  state.isSelecting = true;

  // After obtaining the focus when closing the FilePicker, return the button state to normal
  window.addEventListener(
    'focus',
    () => {
      state.isSelecting = false;
    },
    { once: true }
  );

  uploader.value.click();
}

function onFileChanged(e) {
  appointmentsStore.newAppointmentsFile = e.target.files[0];
}

function getColor(name) {
  if (name === 'New' || name.match(/^\d/)) return '#eff8ff';

  if (name === 'cash' || name === 'card') return '#ecfdf3';

  if (name === 'Set') return '#fffaeb';

  return '#f2f4f7';
}

function getTextColor(name) {
  if (name === 'New' || name.match(/^\d/)) return '#2e90fa';

  if (name === 'cash' || name === 'card') return '#12B76A';

  if (name === 'Set') return '#F79009';

  return '#667085';
}
</script>

<style lang="scss">
@media (min-width: 600px) {
  .appointment-table {
    &__header {
      &__container {
        max-width: 800px;
      }
    }
    .v-card {
      &__title {
        font-size: 20px;
      }
    }

    &__row {
      height: 56px;

      td:first-child {
        width: 5%;
      }

      td:nth-child(2) {
        width: 9.1%;
      }

      td:nth-child(3) {
        width: 32%;
      }
      td:nth-child(4) {
        width: 12%;
      }
      td:nth-child(5) {
        width: 12%;
      }
      td:nth-child(6) {
        width: 12%;
      }
      td:nth-child(7) {
        width: 12%;
      }
    }
  }
}

#app {
  .theme--light {
    thead > tr {
      background: #f2f4f7;
    }
  }
  .theme--dark {
    thead > tr > th {
      color: white !important;
    }
  }
}
.appointment-table {
  .v-text-field {
    max-width: 320px;
  }
}
</style>
