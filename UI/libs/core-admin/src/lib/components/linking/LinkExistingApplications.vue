<template>
  <div>
    <v-card
      :loading="isMatchApplicationLoading || isUpdateUserLoading"
      height="90vh"
      flat
    >
      <v-card-title>
        <span class="mr-5"> Link Existing Applications </span>

        <ConfirmMatchApplicationDialog
          :disabled="!readyToMatch"
          :applicant-name="`${selectedUser[0]?.firstName} ${selectedUser[0]?.lastName}`"
          :application-name="selectedLegacyApplication[0]?.name"
          :applicant-email="selectedUser[0]?.email"
          :application-email="selectedLegacyApplication[0]?.email"
          :applicant-id-number="selectedUser[0]?.driversLicenseNumber"
          :application-id-number="selectedLegacyApplication[0]?.idNumber"
          @confirm="handleMatch"
        />

        <v-menu
          v-model="menu"
          :close-on-content-click="false"
          :return-value.sync="date"
          ref="menuComponent"
          transition="scale-transition"
          min-width="auto"
          offset-y
        >
          <template #activator="{ on, attrs }">
            <v-btn
              color="primary"
              class="ml-3"
              v-bind="attrs"
              v-on="on"
            >
              Appointment Date {{ permitStore.legacyOptions.selectedDate }}
            </v-btn>
          </template>

          <v-date-picker
            v-model="permitStore.legacyOptions.selectedDate"
            no-title
            scrollable
          >
            <v-btn
              @click="clearDate"
              text
              color="primary"
            >
              Clear
            </v-btn>

            <v-spacer />

            <v-btn
              @click="menu = false"
              text
              color="primary"
            >
              Cancel
            </v-btn>

            <v-btn
              @click="menu = false"
              text
              color="primary"
            >
              OK
            </v-btn>
          </v-date-picker>
        </v-menu>

        <v-spacer />

        <ConfirmUserNoApplicationDialog
          :disabled="!customerSelectedOnly"
          @confirm="handleCustomerNoApplications"
        />
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
              :options.sync="permitStore.legacyOptions.options"
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

                  <v-btn
                    :disabled="!permitStore.legacyOptions.selectedDate"
                    :loading="isGetEmailsFetching"
                    @click="handleCopyEmails"
                    color="primary"
                    class="ml-3"
                  >
                    <v-icon left>mdi-content-copy</v-icon>
                    Emails
                  </v-btn>

                  <v-spacer />

                  <v-text-field
                    v-model="permitStore.legacyOptions.applicationSearch"
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

              <template #[`item.status`]="{ item }">
                {{ ApplicationStatus[item.status] }}
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

    <v-snackbar
      v-model="snackbar"
      :timeout="10000"
      color="primary"
      centered
      multi-line
    >
      Emails have been copied to your clipboard

      <template #action="{ attrs }">
        <v-btn
          @click="snackbar = false"
          v-bind="attrs"
          color="white"
          text
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import ConfirmMatchApplicationDialog from '../dialogs/ConfirmMatchApplicationDialog.vue'
import ConfirmUserNoApplicationDialog from '../dialogs/ConfirmUserNoApplicationDialog.vue'
import { LegacyPermitsType } from '@core-admin/types'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useUserStore } from '@shared-ui/stores/userStore'
import { ApplicationStatus, UserType } from '@shared-utils/types/defaultTypes'
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
const menu = ref(false)
const date = ref('')
const snackbar = ref(false)

const readyToMatch = computed(() => {
  return (
    selectedUser.value.length > 0 && selectedLegacyApplication.value.length > 0
  )
})

const customerSelectedOnly = computed(() => {
  return (
    selectedUser.value.length > 0 &&
    selectedLegacyApplication.value.length === 0
  )
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

    if (permitStore.legacyOptions) {
      response = await permitStore.getAllLegacyApplications(signal)

      const totalPages = Math.ceil(
        response.total / permitStore.legacyOptions.options.itemsPerPage
      )

      let isBeyondLastPage =
        permitStore.legacyOptions.options.page > totalPages + 1

      while (isBeyondLastPage && permitStore.legacyOptions.options.page > 1) {
        permitStore.legacyOptions.options.page -= 1
        isBeyondLastPage = permitStore.legacyOptions.options.page > totalPages
      }

      return response
    }

    return response
  },
  {
    enabled: Boolean(permitStore.legacyOptions),
    initialData: { items: [], total: 0 },
  }
)

const { isFetching: isGetEmailsFetching, refetch } = useQuery(
  ['getEmails'],
  () => {
    return permitStore.getEmails()
  },
  {
    enabled: false,
    onSuccess: async response => {
      let emailString = ''

      for (let email of response) {
        emailString += `${email};`
      }

      await navigator.clipboard.writeText(emailString)

      snackbar.value = true
    },
  }
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

const { mutate: updateUser, isLoading: isUpdateUserLoading } = useMutation({
  mutationFn: (user: UserType) => {
    return userStore.updateUserProfileAdmin(user)
  },
  onSuccess: () => {
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
    sortable: false,
  },
]

const applicationHeaders = [
  { text: 'Name', value: 'name' },
  { text: 'ID Number', value: 'idNumber' },
  { text: 'Email', value: 'email' },
  { text: 'Status', value: 'status' },
  {
    text: 'Match',
    value: 'data-table-select',
  },
]

function clearDate() {
  permitStore.legacyOptions.selectedDate = ''
  menu.value = false
}

function handleMatch() {
  if (selectedUser.value[0].id && selectedLegacyApplication.value[0].id) {
    mutate({
      userId: selectedUser.value[0].id,
      applicationId: selectedLegacyApplication.value[0].id,
    })
  }
}

function handleCustomerNoApplications() {
  if (selectedUser.value[0].id) {
    selectedUser.value[0].isPendingReview = false
    updateUser(selectedUser.value[0])
  }
}

function handleCopyEmails() {
  refetch()
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

    permitStore.legacyOptions.applicationSearch = item.lastName

    if (idDocument) {
      openPdf(`${item.id}_${idDocument.name}`)
    }

    if (licenseDocument) {
      openPdf(`${item.id}_${licenseDocument.name}`)
    }
  } else {
    idImage.value = ''
    permitStore.legacyOptions.applicationSearch = ''
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
  () => permitStore.legacyOptions.applicationSearch,
  () => refetchApplications()
)

watch(
  permitStore.legacyOptions,
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
