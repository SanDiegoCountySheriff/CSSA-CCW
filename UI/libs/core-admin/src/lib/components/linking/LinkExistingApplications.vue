<template>
  <div>
    <v-card
      :loading="isMatchApplicationLoading"
      height="90vh"
      flat
    >
      <v-card-title>
        Link Existing Applications

        <v-btn
          @click="handleMatch"
          :disabled="!readyToMatch"
          color="primary"
          class="ml-4"
        >
          Match Applicant/Application
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-data-table
              v-model="selectedUser"
              :items="unmatchedUsers"
              :headers="headers"
              :loading="isFetching || isLoading || pdfLoading"
              :search="applicantSearch"
              @item-expanded="handleItemExpanded"
              single-expand
              show-expand
              show-select
              single-select
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title> Applicants </v-toolbar-title>

                  <v-spacer />

                  <v-text-field
                    v-model="applicantSearch"
                    label="Search"
                    color="primary"
                    hide-details
                    outlined
                    clearable
                  ></v-text-field>
                </v-toolbar>
              </template>

              <template #[`item.name`]="{ item }">
                {{ item.firstName }} {{ item.lastName }}
              </template>

              <template #expanded-item="{ item }">
                <td :colspan="headers.length">
                  <v-container>
                    <v-simple-table>
                      <tr class="text-left">
                        <th>Appointment Date</th>
                        <th>Appointment Time</th>
                        <th>DOB</th>
                        <th>Permit Number</th>
                      </tr>
                      <tr class="text-left">
                        <td>
                          {{
                            item.appointmentDate
                              ? new Date(
                                  item.appointmentDate
                                ).toLocaleDateString()
                              : 'User did not enter'
                          }}
                        </td>
                        <td>
                          {{ item.appointmentTime ?? 'User did not enter' }}
                        </td>
                        <td>
                          {{ item.dateOfBirth }}
                        </td>
                        <td>
                          {{ item.permitNumber ?? 'User did not enter' }}
                        </td>
                      </tr>

                      <tr>
                        <td colspan="2">
                          <v-img
                            :src="idImage"
                            @click="idDialog = true"
                            class="hover-zoom-in"
                            max-height="200"
                            max-width="200"
                            contain
                          ></v-img>
                        </td>

                        <td colspan="2">
                          <v-img
                            :src="licenseImage"
                            @click="licenseDialog = true"
                            class="hover-zoom-in"
                            max-height="200"
                            max-width="200"
                            contain
                          ></v-img>
                        </td>
                      </tr>
                    </v-simple-table>
                  </v-container>
                </td>
              </template>
            </v-data-table>
          </v-col>

          <v-col>
            <v-data-table
              v-model="selectedLegacyApplication"
              :headers="applicationHeaders"
              :items="data.items"
              :server-items-length="data.total"
              :options.sync="options.options"
              :loading="
                isLegacyApplicationLoading || isLegacyApplicationFetching
              "
              :loading-text="$t('Loading permit applications...')"
              :items-per-page="10"
              :footer-props="{
                'items-per-page-options': [10, 25, 50, 100],
              }"
              item-key="orderId"
              single-select
              single-expand
              show-select
              show-expand
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title> Legacy Applications </v-toolbar-title>

                  <v-spacer />

                  <v-text-field
                    v-model="options.applicationSearch"
                    label="Search"
                    color="primary"
                    hide-details
                    outlined
                    clearable
                  ></v-text-field>
                </v-toolbar>
              </template>

              <template #[`item.name`]="{ item }">
                <router-link
                  :to="{
                    name: 'PermitDetail',
                    params: { orderId: item.orderId, isLegacy: true },
                  }"
                  style="text-decoration: underline; color: inherit"
                >
                  {{ item.name }}
                </router-link>
              </template>

              <template #expanded-item="{ item }">
                <td :colspan="headers.length">
                  <v-container>
                    <v-simple-table>
                      <tr class="text-left">
                        <th>Appointment Date</th>
                        <th>DOB</th>
                        <th>Permit Number</th>
                      </tr>
                      <tr class="text-left">
                        <td>
                          {{ item.appointmentDateTime }}
                        </td>
                        <td>
                          {{ item.birthDate }}
                        </td>
                        <td>
                          {{ item.permitNumber }}
                        </td>
                      </tr>
                    </v-simple-table>
                  </v-container>
                </td>
              </template>
            </v-data-table>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>

    <v-dialog
      v-model="idDialog"
      max-width="90vh"
    >
      <v-img
        @click="idDialog = false"
        :src="idImage"
        class="hover-zoom-out"
        max-width="90vh"
        contain
      ></v-img>
    </v-dialog>

    <v-dialog
      v-model="licenseDialog"
      max-width="90vh"
    >
      <v-img
        @click="licenseDialog = false"
        :src="licenseImage"
        class="hover-zoom-out"
        max-width="90vh"
        contain
      ></v-img>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { LegacyPermitsType } from '@core-admin/types'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useUserStore } from '@shared-ui/stores/userStore'
import {
  ApplicationTableOptionsType,
  UserType,
} from '@shared-utils/types/defaultTypes'
import { computed, ref, watch } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const userStore = useUserStore()
const permitStore = usePermitsStore()
const documentStore = useDocumentsStore()
const idImage = ref('')
const licenseImage = ref('')
const pdfLoading = ref(false)
const idDialog = ref(false)
const licenseDialog = ref(false)
const selectedLegacyApplication = ref<Array<LegacyPermitsType>>([])
const selectedUser = ref<Array<UserType>>([])
const applicantSearch = ref('')

const readyToMatch = computed(() => {
  return (
    selectedUser.value.length > 0 && selectedLegacyApplication.value.length > 0
  )
})

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
  applicationSearch: '',
})

const {
  data: unmatchedUsers,
  isLoading,
  isFetching,
  refetch: refetchUsers,
} = useQuery(['getUnmatchedUsers'], userStore.getUnmatchedUsers)

const {
  data,
  isLoading: isLegacyApplicationLoading,
  isFetching: isLegacyApplicationFetching,
  refetch: refetchApplications,
} = useQuery(
  ['getAllLegacyApplications'],
  async ({ signal }) => {
    let response: {
      items: Array<LegacyPermitsType>
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

const { mutate, isLoading: isMatchApplicationLoading } = useMutation({
  mutationFn: ({
    userId,
    applicationId,
  }: {
    userId: string
    applicationId: string
  }) => permitStore.matchApplication(userId, applicationId),
  onSuccess: () => {
    refetchApplications()
    refetchUsers()
  },
})

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
    value: 'data-table-select',
  },
]

const applicationHeaders = [
  { text: 'Name', value: 'name' },
  { text: 'ID Number', value: 'idNumber' },
  { text: 'Email', value: 'email' },
  {
    text: 'Match',
    value: 'data-table-select',
  },
]

function handleMatch() {
  if (selectedUser.value[0].id && selectedLegacyApplication.value[0].id) {
    mutate({
      userId: selectedUser.value[0].id,
      applicationId: selectedLegacyApplication.value[0].id,
    })
  }
}

function handleItemExpanded({
  item,
  value,
}: {
  item: UserType
  value: boolean
}) {
  if (value) {
    const idDocument = item.uploadedDocuments.find(d => {
      return d.documentType === 'DriverLicense'
    })

    const licenseDocument = item.uploadedDocuments.find(d => {
      return d.documentType === 'CCWPermit'
    })

    options.value.applicationSearch = item.lastName

    if (idDocument) {
      openPdf(`${item.id}_${idDocument.name}`)
    }

    if (licenseDocument) {
      openPdf(`${item.id}_${licenseDocument.name}`)
    }
  } else {
    idImage.value = ''
    options.value.applicationSearch = ''
  }
}

async function openPdf(name: string) {
  pdfLoading.value = true

  documentStore
    .getUnmatchedUserDocument(name)
    .then(async response => {
      response.text().then(base64String => {
        fetch(base64String)
          .then(res => res.blob())
          .then(blob => {
            if (name.includes('DriverLicense')) {
              // eslint-disable-next-line node/no-unsupported-features/node-builtins
              idImage.value = URL.createObjectURL(blob)
            }

            if (name.includes('CCWPermit')) {
              // eslint-disable-next-line node/no-unsupported-features/node-builtins
              licenseImage.value = URL.createObjectURL(blob)
            }
          })
      })

      pdfLoading.value = false
    })
    .catch(error => {
      console.error('Error fetching the PDF:', error)
    })
}

watch(
  () => options.value.applicationSearch,
  () => refetchApplications()
)

watch(
  options,
  newVal => {
    if (newVal) {
      refetchApplications()
    }
  },
  { deep: true }
)
</script>

<style>
.hover-zoom-in {
  cursor: zoom-in;
}

.hover-zoom-out {
  cursor: zoom-out;
}
</style>
