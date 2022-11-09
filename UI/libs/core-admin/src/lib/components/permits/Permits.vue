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
        showFirstLastPage: true,
        firstIcon: 'mdi-arrow-collapse-left',
        lastIcon: 'mdi-arrow-collapse-right',
        prevIcon: 'mdi-minus',
        nextIcon: 'mdi-plus',
      }"
      item-key="id"
      item-class="rowClass"
      show-expand
    >
      <template #top>
        <v-toolbar flat>
          <v-toolbar-title>{{ $t('Applications') }}</v-toolbar-title>
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
      <template #item.orderId="props">
        <router-link
          :to="{
            name: 'PermitDetail',
            params: { orderId: props.item.orderId },
          }"
          tag="a"
          target="_blank"
          style="text-decoration: none; color: inherit"
        >
          {{ props.item.orderId }}
        </router-link>
      </template>
      <template #item.name="props">
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
      </template>
      <template #item.address="props">
        {{ props.item.address }}
      </template>
      <template #item.appointmentStatus="props">
        {{ props.item.application.appointmentStatus }}
      </template>
      <template #item.paymentStatus="props">
        {{ props.item.application.paymentStatus }}
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
      text: 'Order Id',
      align: 'start',
      sortable: false,
      value: 'orderId',
    },
    { text: 'Applicant Name', value: 'name' },
    { text: 'Email', value: 'application.userEmail' },
    { text: 'Address', value: 'address' },
    { text: 'Payment Status', value: 'application.paymentStatus' },
    { text: 'Appointment Status', value: 'application.appointmentStatus' },
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

  thead > tr > th {
    font-size: 18px !important;
    line-height: 30px;
    font-weight: 500;
    color: #344054 !important;
  }
}
</style>
