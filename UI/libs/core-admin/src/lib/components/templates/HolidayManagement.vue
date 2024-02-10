<template>
  <div>
    <v-card
      flat
      :loading="
        isLoading || isSaveHolidaysLoading || isGetOrganizationHolidaysLoading
      "
    >
      <v-card-title>
        Which holidays does your organization observe?

        <v-spacer />

        <v-btn
          @click="handleSaveHolidays"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>
          Save
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-checkbox
          v-for="(holiday, index) in holidays"
          v-model="selectedHolidays"
          :label="holiday"
          :value="holiday"
          :key="index"
          hide-details
        >
          <template #label>
            {{ holiday.split(',')[1] }} ---
            {{ holiday.split(',')[0] }}
          </template>
        </v-checkbox>
      </v-card-text>
    </v-card>

    <v-snackbar
      v-model="snackbar"
      color="primary"
    >
      Holidays were saved successfully
      <template #action="{ attrs }">
        <v-btn
          color="white"
          text
          v-bind="attrs"
          @click="snackbar = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { OrganizationalHolidaysRequestModel } from '@shared-utils/types/defaultTypes'
import { ref } from 'vue'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useMutation, useQuery } from '@tanstack/vue-query'

const appointmentsStore = useAppointmentsStore()
const selectedHolidays = ref<Array<string>>([])
const snackbar = ref(false)
const holidays = ref<Array<string>>([])

const { refetch, isLoading: isGetOrganizationHolidaysLoading } = useQuery({
  queryKey: ['getOrganizationHolidays'],
  queryFn: appointmentsStore.getOrganizationHolidays,
  onSuccess: data => {
    if (data) {
      selectedHolidays.value = data.holidays.map(holiday => {
        const matchingHoliday = holidays.value.find(h =>
          h.includes(holiday.name)
        )

        if (matchingHoliday) {
          return matchingHoliday
        }

        return `${holiday.name}, ${
          holiday.month < 10 ? `0${holiday.month}` : holiday.month
        }/${holiday.day < 10 ? `0${holiday.day}` : holiday.day}`
      })
    }
  },
  enabled: false,
})

const { isLoading } = useQuery({
  queryKey: ['getHolidays'],
  queryFn: appointmentsStore.getHolidays,
  onSuccess: data => {
    holidays.value = data.holidays
    refetch()
  },
  refetchOnMount: 'always',
})

const { isLoading: isSaveHolidaysLoading, mutate: saveHolidayMutation } =
  useMutation({
    mutationFn: () => {
      const holidayRequestModels = selectedHolidays.value.map(n => ({
        name: n.split(',')[0],
      }))
      const body: OrganizationalHolidaysRequestModel = {
        holidayRequestModels,
      }

      return appointmentsStore.saveHolidays(body)
    },
    onSuccess: () => {
      snackbar.value = true
    },
  })

function handleSaveHolidays() {
  saveHolidayMutation()
}
</script>
