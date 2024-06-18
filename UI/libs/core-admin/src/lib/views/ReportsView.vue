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
          <v-card-title class="headline mb-4">Export CSV Reports</v-card-title>
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
                        {{ $t('Appointment Reports') }}
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
                  large
                  color="primary"
                  class="mb-4 mx-2"
                  elevation="12"
                  rounded
                >
                  <v-icon
                    left
                    large
                  >
                    mdi-file-document-outline
                  </v-icon>
                  SID Letter Report
                </v-btn>
              </v-col>
            </v-row>
            <v-dialog
              v-model="appointmentDialog"
              max-width="600px"
            >
              <v-card>
                <v-card-title>Appointment Packet Report</v-card-title>
                <v-card-text>
                  Select the day you would like to generate a report for
                  appointments
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
                    @click="handleExport"
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

async function handleExport() {
  const permitsData = await permitStore.getPermitsByDate(selectedDate.value)

  exportToCsv(permitsData)
  appointmentDialog.value = false
}

function exportToCsv(data) {
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
    link.setAttribute('download', `MAIL MERGE MONDAY APPOINTMENTS.csv`)
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  }
}

function handleAppointmentDialog() {
  appointmentDialog.value = true
}
</script>
