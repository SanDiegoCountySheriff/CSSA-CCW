<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/valid-v-slot -->
<!-- eslint-disable vue-a11y/no-autofocus -->
<template>
  <div class="permits-table">
    <v-data-table
      :headers="state.headers"
      :items="data"
      :search="state.search"
      :loading="isLoading && !isError"
      :loading-text="$t('Loading permit applications...')"
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
      item-key="orderID"
      item-class="rowClass"
      show-expand
    >
      <template #top>
        <v-toolbar flat>
          <v-toolbar-title
            class="text-no-wrap pr-4"
            style="text-overflow: clip"
          >
            {{ $t('Applications') }}
          </v-toolbar-title>
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
            </v-row>
          </v-container>
        </v-toolbar>
      </template>
      <template #item.orderID="props">
        <router-link
          :to="{
            name: 'PermitDetail',
            params: { orderId: props.item.orderID },
          }"
          style="text-decoration: none; color: inherit"
        >
          {{ props.item.orderID }}
        </router-link>
      </template>
      <template #item.name="props">
        <div v-if="props.item.initials.length !== 0">
          <v-avatar
            color="blue"
            size="30"
            class="mr-1"
          >
            <span class="white--text .text-xs-caption">
              {{ props.item.initials }}</span
            >
          </v-avatar>
          {{ props.item.name }}
        </div>
        <v-icon
          color="error"
          medium
          v-else
        >
          mdi-alert-octagon
        </v-icon>
      </template>
      <template #item.appointmentStatus="props">
        {{ props.item.appointmentStatus }}
      </template>
      <template #item.paymentStatus="props">
        {{ props.item.paymentStatus }}
      </template>
      <template #item.isComplete="props">
        <v-chip
          :color="props.item.isComplete ? 'blue' : 'error'"
          small
          label
          class="white--text"
        >
          {{ props.item.isComplete ? 'Ready for review' : 'Incomplete' }}
        </v-chip>
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
import { reactive } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const { getAllPermitsApi } = usePermitsStore();
const { isLoading, isError, data } = useQuery(['permits'], getAllPermitsApi);

const state = reactive({
  search: '',
  singleExpand: true,
  expanded: [],
  headers: [
    {
      text: 'ORDER ID',
      align: 'start',
      sortable: false,
      value: 'orderID',
    },
    { text: 'APPLICANT NAME', value: 'name' },
    { text: 'EMAIL', value: 'email' },
    { text: 'PAYMENT', value: 'status' },
    { text: 'APPOINTMENT STATUS', value: 'appointmentStatus' },
    { text: 'APPOINTMENT DATE/TIME', value: 'appointmentDateTime' },
    { text: 'APPLICATION STATUS', value: 'isComplete' },
    { text: '', value: '' },
  ],
});
</script>

<style lang="scss">
@media (min-width: 600px) {
  .permits-table {
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
        width: 20%;
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
