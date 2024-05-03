<template>
  <v-container fluid>
    <v-card
      :loading="isLoading || isFetching"
      height="90vh"
      flat
    >
      <v-card-title>Link Existing Applications</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-data-table
              :items="unmatchedUsers"
              :headers="headers"
              single-expand
              show-expand
              @item-expanded="handleItemExpanded"
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title> Applicants </v-toolbar-title>
                </v-toolbar>
              </template>

              <template #[`item.name`]="{ item }">
                {{ item.firstName }} {{ item.lastName }}
              </template>

              <template #[`item.actions`]="{ item }">
                <v-btn
                  @click="handleMatch(item)"
                  color="primary"
                  icon
                >
                  <v-icon>mdi-check-bold</v-icon>
                </v-btn>
              </template>

              <template #expanded-item="{ item }">
                <td :colspan="headers.length">
                  <v-container>
                    <v-simple-table>
                      <tr class="text-left">
                        <th>Appointment</th>
                        <th>DOB</th>
                        <th>Permit Number</th>
                      </tr>
                      <tr class="text-left">
                        <td>
                          {{
                            new Date(item.appointmentDate).toLocaleDateString()
                          }}
                          {{ item.appointmentTime }}
                        </td>
                        <td>
                          {{ item.dateOfBirth }}
                        </td>
                        <td>
                          {{ item.permitNumber }}
                        </td>
                      </tr>
                      <tr>
                        <v-img
                          max-height="300"
                          max-width="300"
                          contain
                          :src="idImage"
                        ></v-img>
                      </tr>
                    </v-simple-table>
                  </v-container>
                </td>
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
import { PermitsType } from '@core-admin/types'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import { useUserStore } from '@shared-ui/stores/userStore'
import {
  ApplicationTableOptionsType,
  UserType,
} from '@shared-utils/types/defaultTypes'
import { ref, watch } from 'vue'

const userStore = useUserStore()
const permitStore = usePermitsStore()
const documentStore = useDocumentsStore()
const idImage = ref('')

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
    text: 'Name',
    value: 'name',
  },
  {
    text: 'ID Number',
    value: 'driversLicenseNumber',
  },
  {
    text: 'Email',
    value: 'email',
  },
  {
    text: 'Match',
    value: 'actions',
  },
]

const applicationHeaders = [{ text: 'Order ID', value: 'orderId' }]

function handleMatch(item) {
  window.console.log(item)
}

function handleItemExpanded({
  item,
  value,
}: {
  item: UserType
  value: boolean
}) {
  if (value) {
    const document = item.uploadedDocuments.find(d => {
      return d.documentType === 'DriverLicense'
    })

    if (document) {
      openPdf(`${item.id}_${document.name}`)
    }
  } else {
    idImage.value = ''
  }
}

async function openPdf(name: string) {
  documentStore
    .getUnmatchedUserDocument(name)
    .then(response => {
      if (response.type === 'application/pdf') {
        const pdfBlob = new Blob([response], { type: 'application/pdf' })

        // eslint-disable-next-line node/no-unsupported-features/node-builtins
        idImage.value = URL.createObjectURL(pdfBlob)
      } else if (response.type === 'text/plain') {
        response.text().then(base64String => {
          fetch(base64String)
            .then(res => res.blob())
            .then(blob => {
              // eslint-disable-next-line node/no-unsupported-features/node-builtins
              idImage.value = URL.createObjectURL(blob)
            })
        })
      }
    })
    .catch(error => {
      console.error('Error fetching the PDF:', error)
    })
}

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
