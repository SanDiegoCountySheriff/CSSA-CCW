<template>
  <div>
    <v-card
      flat
      :loading="isLoading || isSaveHolidaysLoading"
    >
      <v-card-title>
        Which holidays does your organization observe?
      </v-card-title>

      <v-card-text>
        <v-checkbox
          v-for="(holiday, index) in holidays?.holidays"
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

      <v-card-actions>
        <v-btn
          @click="handleSaveHolidays"
          color="primary"
        >
          Save
        </v-btn>
      </v-card-actions>
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

const { data: holidays, isLoading } = useQuery(
  ['getHolidays'],
  appointmentsStore.getHolidays
)

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
