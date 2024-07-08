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
        <v-card
          class="pa-5"
          outlined
        >
          <v-card-title class="headline mb-4">CSV Reports</v-card-title>
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
                <v-btn
                  @click="handleAppointmentDialog"
                  :color="$vuetify.theme.dark ? 'white' : 'primary'"
                  :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
                  :x-large="$vuetify.breakpoint.lgAndUp"
                  :small="$vuetify.breakpoint.smAndDown"
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
                </v-btn>
              </v-col>
              <v-col
                cols="12"
                md="6"
                class="d-flex justify-center"
              >
                <v-btn
                  @click="handleSIDDialog"
                  :color="$vuetify.theme.dark ? 'white' : 'primary'"
                  :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
                  :x-large="$vuetify.breakpoint.lgAndUp"
                  :small="$vuetify.breakpoint.smAndDown"
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
                </v-btn>
              </v-col>
            </v-row>
            <v-dialog
              v-model="appointmentDialog"
              max-width="600px"
            >
              <v-card>
                <v-card-title>Appointment Report</v-card-title>
                <v-card-text>
                  Select which day to generate an appointment report
                  <v-date-picker
                    v-model="selectedDate"
                    class="mt-3 mb-3 rounded-lg"
                    color="primary"
                    full-width
                  ></v-date-picker>
                </v-card-text>
                <v-card-actions>
                  <v-btn
                    color="primary"
                    text
                    @click="appointmentDialog = false"
                  >
                    Close
                  </v-btn>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="primary"
                    text
                    @click="handleAppointmentExport"
                  >
                    Generate Report
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
            <v-dialog
              v-model="sidDialog"
              max-width="600px"
            >
              <v-card>
                <v-card-title>SID Letter Report</v-card-title>
                <v-card-text>
                  Select which day to generate an SID Letter report
                  <v-date-picker
                    v-model="selectedDate"
                    class="mt-3 mb-3 rounded-lg"
                    color="primary"
                    full-width
                  ></v-date-picker>
                </v-card-text>
                <v-card-actions>
                  <v-btn
                    color="primary"
                    text
                    @click="sidDialog = false"
                  >
                    Close
                  </v-btn>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="primary"
                    text
                    @click="handleSIDExport"
                  >
                    Generate Report
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const permitStore = usePermitsStore()
const selectedDate = ref('')

const appointmentDialog = ref(false)
const sidDialog = ref(false)

async function handleAppointmentExport() {
  const permitsData = await permitStore.getPermitsByDate(selectedDate.value)

  exportAppointmentCsv(permitsData)
  appointmentDialog.value = false
}

async function handleSIDExport() {
  const permitsData = await permitStore.getPermitsByDate(selectedDate.value)

  exportSID(permitsData)
  sidDialog.value = false
}

function exportSID(data) {
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
    link.setAttribute('download', `APPOINTMENTS.csv`)
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  }
}

function exportAppointmentCsv(data) {
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
    link.setAttribute('download', `APPOINTMENTS.csv`)
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
