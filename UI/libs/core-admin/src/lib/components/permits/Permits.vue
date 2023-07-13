<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/valid-v-slot -->
<!-- eslint-disable vue-a11y/no-autofocus -->
<template>
  <v-container fluid>
    <v-data-table
      :headers="state.headers"
      :items="data"
      :search="state.search"
      :loading="isLoading && !isError"
      :loading-text="$t('Loading permit applications...')"
      :single-expand="state.singleExpand"
      :expanded.sync="state.expanded"
      :items-per-page="14"
      show-select
      v-model="state.selected"
      :footer-props="{
        showCurrentPage: true,
        showFirstLastPage: true,
        firstIcon: 'mdi-skip-backward',
        lastIcon: 'mdi-skip-forward',
        prevIcon: 'mdi-skip-previous',
        nextIcon: 'mdi-skip-next',
      }"
      item-key="orderId"
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
          <v-container>
            <v-row justify="end">
              <v-col md="6">
                <v-menu offset-y>
                  <template #activator="{ on }">
                    <v-btn
                      color="primary"
                      dark
                      v-on="on"
                    >
                      <div>
                        {{ 'Assign User' }}
                      </div>
                    </v-btn>
                  </template>
                  <v-list>
                    <v-list-item
                      v-for="(adminUser, index) in adminUserStore.allAdminUsers"
                      :key="index"
                      @click="handleAdminUserSelect(adminUser.name)"
                    >
                      <v-list-item-title>{{
                        adminUser.name
                      }}</v-list-item-title>
                    </v-list-item>
                  </v-list>
                </v-menu>
              </v-col>
              <v-col>
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
          style="text-decoration: underline; color: inherit"
        >
          {{ props.item.orderId }}
        </router-link>
      </template>
      <template #item.name="props">
        <div v-if="props.item.initials.length !== 0">
          <v-avatar
            color="primary"
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
          :color="$vuetify.theme.dark ? '' : 'error'"
          medium
          v-else
        >
          mdi-alert-octagon
        </v-icon>
      </template>
      <template #item.applicationStatus="props">
        <v-chip
          small
          color="grey"
          label
          class="white--text"
        >
          {{ props.item.applicationStatus }}
        </v-chip>
      </template>
      <template #item.isComplete="props">
        <v-chip
          :color="props.item.isComplete ? 'primary' : 'error'"
          small
          label
        >
          {{ props.item.isComplete ? 'Ready for review' : 'Incomplete' }}
        </v-chip>
      </template>
    </v-data-table>

    <v-dialog
      v-model="state.assignDialog"
      persistent
      max-width="500"
    >
      <v-card>
        <v-card-title>Assign User</v-card-title>
        <v-card-text>
          Are you sure you want to assign
          {{ state.selected.length }} applications to:
          {{ state.selectedAdminUser }}
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="error"
            @click="state.assignDialog = false"
          >
            No
          </v-btn>
          <v-btn
            rounded
            color="primary"
            @click="handleAssignMultipleApplications"
          >
            Yes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { PermitsType } from '@core-admin/types'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const { getAllPermitsApi } = usePermitsStore()
const { isLoading, isError, data } = useQuery(['permits'], getAllPermitsApi, {
  refetchOnMount: 'always',
})

const state = reactive({
  search: '',
  singleExpand: true,
  expanded: [],
  selected: [] as PermitsType[],
  selectedAdminUser: '',
  assignDialog: false,
  headers: [
    {
      text: 'ORDER ID',
      align: 'start',
      sortable: false,
      value: 'orderId',
    },
    { text: 'APPLICANT NAME', value: 'name' },
    { text: 'APPLICATION TYPE', value: 'applicationType' },
    { text: 'EMAIL', value: 'userEmail' },
    { text: 'PAYMENT', value: 'status' },
    { text: 'APPOINTMENT STATUS', value: 'appointmentStatus' },
    { text: 'APPOINTMENT DATE/TIME', value: 'appointmentDateTime' },
    { text: 'APPLICATION STATUS', value: 'isComplete' },
  ],
})
const permitStore = usePermitsStore()
const adminUserStore = useAdminUserStore()
const changed = ref('')

const { mutate: updateMultiplePermitDetailsApi } = useMutation({
  mutationFn: (orderIds: string[]) =>
    permitStore.updateMultiplePermitDetailsApi(
      orderIds,
      state.selectedAdminUser
    ),
})

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi(`Updated ${changed.value}`),
  {
    enabled: false,
  }
)

function handleAssignApplications() {
  changed.value = 'Assigned User to Applications'
  updatePermitDetails()
}

function handleAdminUserSelect(adminUser) {
  state.selectedAdminUser = adminUser
  state.assignDialog = true
}

async function handleAssignMultipleApplications() {
  const orderIds = state.selected.map(element => element.orderId)

  if (state.selectedAdminUser) {
    updateMultiplePermitDetailsApi(orderIds)
  }

  state.assignDialog = false
}
</script>
