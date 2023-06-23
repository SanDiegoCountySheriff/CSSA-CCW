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
      class="pt-2 fill-height"
      outlined
    >
      <v-container>
        <v-row>
          <v-col
            cols="12"
            lg="4"
          >
            <div class="font-weight-bold">
              Application #{{ permitStore.getPermitDetail.application.orderId }}
            </div>
            <span class="body-2"> Submitted on {{ submittedDate }}</span>
          </v-col>
          <v-col
            cols="12"
            lg="4"
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
                              item.value
                            updateApplicationStatus(`Type to ${item.value}`)
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
            lg="4"
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
import PaymentDialog from '@core-admin/components/dialogs/PaymentDialog.vue'
import { capitalize } from '@shared-utils/formatters/defaultFormatters'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import { useRoute } from 'vue-router/composables'
import { computed, reactive } from 'vue'

const route = useRoute()
const permitStore = usePermitsStore()

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
]

const state = reactive({
  update: '',
})

const appStatus = [
  {
    id: 0,
    value: 'None',
  },
  {
    id: 1,
    value: 'Incomplete',
  },
  {
    id: 2,
    value: 'Submitted',
  },
  {
    id: 3,
    value: 'Ready for Appointment',
  },
  {
    id: 4,
    value: 'Appointment Complete',
  },
  {
    id: 5,
    value: 'Background in Progress',
  },
  {
    id: 6,
    value: 'Contingently Approved',
  },
  {
    id: 7,
    value: 'Approved',
  },
  {
    id: 8,
    value: 'Permit Delivered',
  },
  {
    id: 9,
    value: 'Suspend',
  },
  {
    id: 10,
    value: 'Revoke',
  },
  {
    id: 11,
    value: 'Cancelled',
  },
  {
    id: 12,
    value: 'Denied',
  },
  {
    id: 13,
    value: 'Withdrawn',
  },
]

const { isLoading } = useQuery(['permitDetail', route.params.orderId], () =>
  permitStore.getPermitDetailApi(route.params.orderId)
)

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi(state.update),
  {
    enabled: false,
  }
)

const submittedDate = computed(
  () =>
    new Date(
      permitStore.getPermitDetail?.history[0]?.changeDateTimeUtc
    )?.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    }) || ''
)

function updateApplicationStatus(update: number) {
  switch (update) {
    case 0:
      state.update = 'Changed status to None'
      break
    case 1:
      state.update = 'Changed status to Incomplete'
      break
    case 2:
      state.update = 'Changed status to Submitted'
      break
    case 3:
      state.update = 'Changed status to Ready for Appointment'
      break
    case 4:
      state.update = 'Changed status to Appointment Complete'
      break
    case 5:
      state.update = 'Changed status to Background in Progress'
      break
    case 6:
      state.update = 'Changed status to Contingently Approved'
      break
    case 7:
      state.update = 'Changed status to Approved'
      break
    case 8:
      state.update = 'Changed status to Permit Delivered'
      break
    case 9:
      state.update = 'Changed status to Suspend'
      break
    case 10:
      state.update = 'Changed status to Revoke'
      break
    case 11:
      state.update = 'Changed status to Cancelled'
      break
    case 12:
      state.update = 'Changed status to Denied'
      break
    case 13:
      state.update = 'Changed status to Withdrawn'
      break
    default:
      state.update = `Changed ${update}`
      break
  }

  updatePermitDetails()
}
</script>
