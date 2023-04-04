<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container class="px-0 py-0">
    <v-card
      v-if="isLoading"
      height="80"
      outlined
    >
      <v-skeleton-loader type="list-item" />
    </v-card>
    <v-card
      v-else
      height="80"
      class="pt-2"
      outlined
    >
      <v-container>
        <v-row>
          <v-col
            cols="12"
            md="4"
            sm="12"
          >
            <div class="font-weight-bold">
              Application #{{ permitStore.getPermitDetail.application.orderId }}
            </div>
            <span class="body-2"> Submitted on {{ submittedDate }}</span>
          </v-col>
          <v-col
            cols="12"
            md="4"
            sm="12"
          >
            <v-row>
              <v-tooltip bottom>
                <template #activator="{ on: tooltipOn, attrs: tooltipattrs }">
                  <v-col
                    v-bind="tooltipattrs"
                    v-on="tooltipOn"
                  >
                    <v-menu offest-y>
                      <template #activator="{ on, attrs }">
                        <v-chip
                          :text-color="
                            $vuetify.theme.dark ? '' : 'grey darken-2'
                          "
                          v-bind="attrs"
                          v-on="on"
                        >
                          {{
                            capitalize(
                              permitStore.getPermitDetail.application
                                .applicationType
                            )
                          }}
                        </v-chip>
                      </template>
                      <v-list>
                        <v-list-item
                          v-for="(item, index) in items"
                          :key="index"
                          @click="
                            permitStore.getPermitDetail.application.applicationType =
                              item.value;
                            updateApplicationStatus(`Type to ${item.value}`);
                          "
                        >
                          <v-list-item-title>
                            {{ item.name }}
                          </v-list-item-title>
                        </v-list-item>
                      </v-list>
                    </v-menu>
                  </v-col>
                </template>
                {{ $t(' Click to change the Application Type') }}
              </v-tooltip>
              <v-col class="px-0">
                <v-chip
                  color=" green lighten-3"
                  text-color="green darken-4"
                >
                  {{
                    appStatus.find(
                      status =>
                        status.id ===
                        permitStore.getPermitDetail.application.status
                    )?.value
                  }}
                </v-chip>
              </v-col>
              <v-tooltip bottom>
                <template #activator="{ on, attrs }">
                  <v-col
                    v-bind="attrs"
                    v-on="on"
                    class="px-0"
                  >
                    <PaymentDialog />
                  </v-col>
                </template>
                {{ $t('Click to view and payment history') }}
              </v-tooltip>
            </v-row>
          </v-col>
          <v-col
            cols="12"
            md="4"
            sm="12"
          >
            <v-select
              ref="select"
              :items="appStatus"
              label="Application Status"
              item-text="value"
              item-value="id"
              v-model="permitStore.getPermitDetail.application.status"
              @change="$event => updateApplicationStatus($event)"
              dense
              outlined
            ></v-select>
          </v-col>
        </v-row>
      </v-container>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import PaymentDialog from '@core-admin/components/dialogs/PaymentDialog.vue';
import { capitalize } from '@shared-utils/formatters/defaultFormatters';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useRoute } from 'vue-router/composables';
import { computed, reactive } from 'vue';

const route = useRoute();
const permitStore = usePermitsStore();

const items = [
  { name: 'Standard', value: 'standard' },
  { name: 'Reserve', value: 'reserve' },
  { name: 'Judicial', value: 'judicial' },
  { name: 'Renew Standard', value: 'renew-standard' },
  { name: 'Renew Reserve', value: 'renew-reserve' },
  { name: 'Renew Judicial', value: 'renew-judicial' },
  { name: 'Modify Standard', value: 'modify-standard' },
  { name: 'Modify Reserve', value: 'modify-reserve' },
  { name: 'Modify Judicial', value: 'modify-judicial' },
  { name: 'Duplicate Standard', value: 'duplicate-standard' },
  { name: 'Duplicate Reserve', value: 'duplicate-reserve' },
  { name: 'Duplicate Judicial', value: 'duplicate-judicial' },
];

const state = reactive({
  update: '',
});

const appStatus = [
  {
    id: 0,
    value: 'None',
  },
  {
    id: 11,
    value: 'Approved',
  },
  {
    id: 4,
    value: 'Cancelled',
  },
  {
    id: 6,
    value: 'Complete',
  },
  {
    id: 14,
    value: 'Deny',
  },
  {
    id: 3,
    value: 'In Progress',
  },
  {
    id: 10,
    value: 'Pending Final Payment',
  },
  {
    id: 12,
    value: 'Permit Sent',
  },
  {
    id: 7,
    value: 'Refund',
  },
  {
    id: 5,
    value: 'Returned',
  },
  {
    id: 9,
    value: 'Revoke',
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
    id: 8,
    value: 'Suspend',
  },
  {
    id: 13,
    value: 'Withdraw',
  },
];

const { isLoading } = useQuery(['permitDetail', route.params.orderId], () =>
  permitStore.getPermitDetailApi(route.params.orderId)
);

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi(state.update),
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

function updateApplicationStatus(update: number) {
  switch (update) {
    case 0:
      state.update = 'Changed status to None';
      break;
    case 1:
      state.update = 'Changed status to Started';
      break;
    case 2:
      state.update = 'Changed status to Submitted';
      break;
    case 3:
      state.update = 'Changed status to In Progress';
      break;
    case 4:
      state.update = 'Changed status to Cancelled';
      break;
    case 5:
      state.update = 'Changed status to Returned';
      break;
    case 6:
      state.update = 'Changed status to Complete';
      break;
    case 7:
      state.update = 'Changed status to Refund';
      break;
    case 8:
      state.update = 'Changed status to Suspend';
      break;
    case 9:
      state.update = 'Changed status to Revoke';
      break;
    case 10:
      state.update = 'Changed status to Pending Final Payment';
      break;
    case 11:
      state.update = 'Changed status to Approved';
      break;
    case 12:
      state.update = 'Changed status to Permit Sent';
      break;
    case 13:
      state.update = 'Changed status to Withdraw';
      break;
    default:
      state.update = `Changed ${update}`;
      break;
  }

  updatePermitDetails();
}
</script>
