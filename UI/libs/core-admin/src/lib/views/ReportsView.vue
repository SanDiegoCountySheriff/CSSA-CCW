<template>
  <v-container fluid>
    <v-row
      justify="center"
      align="center"
      class="fill-height"
    >
      <v-col
        cols="12"
        md="8"
      >
        <template>
          <v-card-title class="headline mb-4 text-center d-flex justify-center">
            CSV Reports
          </v-card-title>

          <v-card-text>
            <v-row
              justify="center"
              align="center"
              class="mb-4"
            >
              <v-col
                cols="12"
                md="6"
                class="d-flex justify-center"
              >
                <v-card elevation="3">
                  <ReportsDialog
                    @click="handleAppointmentDialog"
                    @generate="handleAppointmentExport"
                    :button-color="$vuetify.theme.dark ? 'white' : 'primary'"
                    :button-height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
                    :button-x-large="$vuetify.breakpoint.lgAndUp"
                    :button-small="$vuetify.breakpoint.smAndDown"
                    :button-text="'APPOINTMENT REPORT'"
                    description="Select which day to generate an appointment report"
                    title="Appointment Report"
                    icon="mdi-calendar"
                    text
                  >
                    <v-container>
                      <v-row>
                        <v-col>
                          <v-icon x-large> mdi-calendar </v-icon>
                        </v-col>
                      </v-row>

                      <v-row>
                        <v-col>
                          {{ $t('APPOINTMENT REPORT') }}
                        </v-col>
                      </v-row>
                    </v-container>
                  </ReportsDialog>
                </v-card>
              </v-col>

              <v-col
                cols="12"
                md="6"
                class="d-flex justify-center"
              >
                <v-card>
                  <ReportsDialog
                    @click="handleSIDDialog"
                    @generate="handleSIDExport"
                    :button-color="$vuetify.theme.dark ? 'white' : 'primary'"
                    :button-height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
                    :button-x-large="$vuetify.breakpoint.lgAndUp"
                    :button-small="$vuetify.breakpoint.smAndDown"
                    :button-text="'SID LETTER REPORT'"
                    description="Select which day to generate an SID Letter Report"
                    title="SID Letter Report"
                    icon="mdi-file-chart-outline"
                    text
                  >
                    <v-container>
                      <v-row>
                        <v-col>
                          <v-icon x-large> mdi-file-chart-outline </v-icon>
                        </v-col>
                      </v-row>

                      <v-row>
                        <v-col>
                          {{ $t('SID LETTER REPORT') }}
                        </v-col>
                      </v-row>
                    </v-container>
                  </ReportsDialog>
                </v-card>
              </v-col>
            </v-row>

            <v-col>
              <v-row class="d-flex justify-center text-center">
                <v-alert
                  type="info"
                  outlined
                  class="mt-4"
                >
                  Click on the desired report and choose a date to generate it.
                </v-alert>
              </v-row>
            </v-col>
          </v-card-text>
        </template>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import ReportsDialog from '@core-admin/components/dialogs/ReportsDialog.vue'
import { ref } from 'vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const permitStore = usePermitsStore()

const appointmentDialog = ref(false)
const sidDialog = ref(false)

async function handleAppointmentExport(date: string) {
  const permitsData = await permitStore.getPermitsByDate(date)

  exportAppointmentCsv(permitsData, date)
  appointmentDialog.value = false
}

async function handleSIDExport(date: string) {
  const permitsData = await permitStore.getPermitsByDate(date)

  exportSID(permitsData, date)
  sidDialog.value = false
}

function exportSID(data, exportDate) {
  const csvRows: string[] = []

  const headers = [
    { csvHeader: 'LAST NAME', dataKey: 'lastName' },
    { csvHeader: 'FIRST NAME', dataKey: 'firstName' },
    { csvHeader: 'MIDDLE NAME', dataKey: 'middleName' },
    { csvHeader: 'SUFFIX', dataKey: 'suffix' },
    { csvHeader: 'DATE OF BIRTH', dataKey: 'birthDate' },
    { csvHeader: 'AKA', dataKey: 'aliases' },
  ]

  const headerRow = headers.map(header => header.csvHeader).join(',')

  csvRows.push(headerRow)

  data.sort((a, b) => {
    const lastNameA = a.lastName ? a.lastName.toUpperCase() : ''
    const lastNameB = b.lastName ? b.lastName.toUpperCase() : ''

    if (lastNameA < lastNameB) {
      return -1
    }

    if (lastNameA > lastNameB) {
      return 1
    }

    return 0
  })

  for (const row of data) {
    if (row.status === 4) {
      const values = headers
        .map(header => {
          let value = row[header.dataKey]

          if (value === null || value === undefined) {
            value = ''
          }

          if (header.dataKey === 'aliases') {
            value =
              Array.isArray(value) && value.length > 0
                ? value
                    .map(alias => {
                      const aliasParts = [
                        alias.prevLastName || '',
                        alias.prevFirstName || '',
                        alias.prevMiddleName || '',
                      ]
                        .filter(part => part)
                        .join(', ')

                      return aliasParts
                    })
                    .join('; ')
                : 'NONE'
          }

          if (typeof value === 'string') {
            value = value.toUpperCase()
          }

          return typeof value === 'string'
            ? `"${value.replace(/"/g, '""')}"`
            : value
        })
        .join(',')

      csvRows.push(values)
    }
  }

  const csvContent = csvRows.join('\n')
  const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' })

  if (typeof window !== 'undefined') {
    const link = document.createElement('a')

    // eslint-disable-next-line node/no-unsupported-features/node-builtins
    link.href = URL.createObjectURL(blob)
    link.setAttribute('download', `SID LETTER REPORT ${exportDate}.csv`)
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  }
}

function exportAppointmentCsv(data, exportDate) {
  const csvRows: string[] = []

  const headers = ['paid', 'lastName', 'firstName', 'appointmentDateTime']

  for (const row of data) {
    const values = headers
      .map(header => {
        let value = row[header]

        if (value === null || value === undefined) {
          value = ''
        }

        if (header === 'paid') {
          value = value ? 'Payment Received' : 'Requires Payment'
        }

        if (header === 'appointmentDateTime' && value) {
          const date = new Date(value)

          value = date.toLocaleTimeString([], {
            hour: '2-digit',
            minute: '2-digit',
            hour12: true,
          })
        }

        return typeof value === 'string'
          ? `"${value.replace(/"/g, '""')}"`
          : value
      })
      .join(',')

    csvRows.push(values)
  }

  const csvContent = csvRows.join('\n')
  const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' })

  if (typeof window !== 'undefined') {
    const link = document.createElement('a')

    // eslint-disable-next-line node/no-unsupported-features/node-builtins
    link.href = URL.createObjectURL(blob)
    link.setAttribute('download', `APPOINTMENTS ${exportDate}.csv`)
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  }
}

function handleAppointmentDialog() {
  appointmentDialog.value = true
}

function handleSIDDialog() {
  sidDialog.value = true
}
</script>
