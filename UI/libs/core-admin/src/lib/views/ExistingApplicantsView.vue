<template>
  <v-container fluid>
    <v-card :loading="isLoading || isFetching">
      <v-card-title>Link Existing Applications</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-data-table
              :items="unmatchedUsers"
              :headers="headers"
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title> Applicants </v-toolbar-title>
                </v-toolbar>
              </template>
            </v-data-table>
          </v-col>

          <v-col>
            <v-data-table
              :headers="applicationHeaders"
              :items="data.items"
              :server-items-length="data.total"
              :options.sync="options.options"
              :loading-text="$t('Loading permit applications...')"
              :items-per-page="10"
              :footer-props="{
                'items-per-page-options': [10, 25, 50, 100],
              }"
              item-key="orderId"
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title> Legacy Applications </v-toolbar-title>
                </v-toolbar>
              </template>
            </v-data-table>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ApplicationTableOptionsType } from '@shared-utils/types/defaultTypes'
import { PermitsType } from '@core-admin/types'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import { useUserStore } from '@shared-ui/stores/userStore'
import { ref, watch } from 'vue'

const userStore = useUserStore()
const permitStore = usePermitsStore()

const options = ref<ApplicationTableOptionsType>({
  options: {
    page: 1,
    itemsPerPage: 10,
    sortBy: [],
    sortDesc: [],
    groupBy: [],
    groupDesc: [],
    multiSort: false,
    mustSort: false,
  },
  statuses: [],
  search: '',
  paid: false,
  appointmentStatuses: [],
  applicationTypes: [],
  showingTodaysAppointments: false,
  selectedDate: '',
})

const {
  data: unmatchedUsers,
  isLoading,
  isFetching,
} = useQuery(['getUnmatchedUsers'], userStore.getUnmatchedUsers)

const {
  data,
  isLoading: isLegacyApplicationLoading,
  isFetching: isLegacyApplicationFetching,
  refetch,
} = useQuery(
  ['getAllLegacyApplications'],
  async ({ signal }) => {
    let response: {
      items: Array<PermitsType>
      total: number
    } = { items: [], total: 0 }

    if (options.value) {
      response = await permitStore.getAllLegacyApplications(
        options.value,
        signal
      )

      const totalPages = Math.ceil(
        response.total / options.value.options.itemsPerPage
      )

      let isBeyondLastPage = options.value.options.page > totalPages + 1

      while (isBeyondLastPage && options.value.options.page > 1) {
        options.value.options.page -= 1
        isBeyondLastPage = options.value.options.page > totalPages
      }

      return response
    }

    return response
  },
  { enabled: Boolean(options.value), initialData: { items: [], total: 0 } }
)

const headers = [
  {
    text: 'Email',
    value: 'email',
  },
]

const applicationHeaders = [{ text: 'Order ID', value: 'orderId' }]

watch(
  options,
  newVal => {
    if (newVal) {
      refetch()
    }
  },
  { deep: true }
)
</script>
