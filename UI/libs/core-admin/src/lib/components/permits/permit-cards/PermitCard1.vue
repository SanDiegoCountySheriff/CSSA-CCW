<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    class="mt-6 ml-8 mr-8 pt-2 fixed-permit-card"
    elevation="2"
  >
    <v-container
      v-if="isLoading"
      fluid
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,divider,list-item"
      >
      </v-skeleton-loader>
    </v-container>
    <v-row
      class="ml-1"
      v-else
    >
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          elevation="0"
          class="text-left pt-3"
        >
          <div class="font-weight-bold grey--text text--darken-3">
            Application #{{ permitStore.getPermitDetail.application.orderId }}
          </div>
          <span class="grey--text body-2">
            Submitted on {{ submittedDate }}</span
          >
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          elevation="0"
          class="mt-2 pt-3"
        >
          <v-chip
            class="ml-4"
            :color="$vuetify.theme.dark ? '' : 'blue lighten-4'"
            :text-color="$vuetify.theme.dark ? '' : 'blue'"
          >
            {{
              permitStore.getPermitDetail.application.applicationType.startsWith(
                'renew'
              )
                ? 'Renew'
                : permitStore.getPermitDetail.application.applicationType.startsWith(
                    'modify'
                  )
                ? 'Modify'
                : 'New'
            }}
          </v-chip>
          <v-chip
            class="ml-4"
            :text-color="$vuetify.theme.dark ? '' : 'grey darken-2'"
          >
            {{
              capitalize(
                permitStore.getPermitDetail.application.applicationType
              )
            }}
          </v-chip>
          <v-chip
            class="ml-4"
            :color="$vuetify.theme.dark ? '' : 'green lighten-3'"
            :text-color="$vuetify.theme.dark ? '' : 'green darken-4'"
          >
            {{
              appStatus.find(
                status =>
                  status.id === permitStore.getPermitDetail.application.status
              )?.value
            }}
          </v-chip>
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          elevation="0"
          class="text-right mr-4 mt-2 pl-12 pt-3"
        >
          <v-select
            :items="appStatus"
            label="Application Status"
            item-text="value"
            item-value="id"
            v-model="permitStore.getPermitDetail.application.status"
            @change="updateApplicationStatus()"
            dense
            outlined
          ></v-select>
        </v-card>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import { capitalize } from '@shared-utils/formatters/defaultFormatters';
import { computed } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useRoute } from 'vue-router/composables';

const route = useRoute();
const permitStore = usePermitsStore();

const appStatus = [
  {
    id: 0,
    value: 'None',
  },
  {
    id: 1,
    value: 'Started',
  },
  {
    id: 2,
    value: 'Submitted',
  },
  {
    id: 3,
    value: 'In Progress',
  },
  {
    id: 4,
    value: 'Cancelled',
  },
  {
    id: 5,
    value: 'Returned',
  },
  {
    id: 6,
    value: 'Complete',
  },
  {
    id: 7,
    value: 'Refund',
  },
  {
    id: 8,
    value: 'Suspend',
  },
  {
    id: 9,
    value: 'Revoke',
  },
  {
    id: 10,
    value: 'Pending Final Payment',
  },
  {
    id: 12,
    value: 'Approved',
  },
  {
    id: 12,
    value: 'Permit Sent',
  },
];

const { isLoading } = useQuery(['permitDetail', route.params.orderId], () =>
  permitStore.getPermitDetailApi(route.params.orderId)
);

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  permitStore.updatePermitDetailApi,
  {
    enabled: false,
  }
);

const submittedDate = computed(
  () =>
    new Date(
      permitStore.getPermitDetail?.history[0]?.changeDateTimeUtc
    )?.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    }) || ''
);

function updateApplicationStatus() {
  updatePermitDetails();
}
</script>
<style lang="scss" scoped>
.fixed-permit-card {
  position: -webkit-sticky;
  position: sticky;
  top: 4rem;
  z-index: 7;

  .v-tabs-bar__content {
    padding-top: 15px;
  }
}
</style>
