<template>
  <v-card
    flat
    :loading="isLoading"
  >
    <v-card-title>Which holidays does your organization observe?</v-card-title>

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

    <v-card-actions>
      <v-btn
        @click="handleSaveHolidays"
        color="primary"
      >
        Save
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useQuery } from '@tanstack/vue-query'

const appointmentsStore = useAppointmentsStore()
const selectedHolidays = ref([])

const { data: holidays, isLoading } = useQuery(
  ['getHolidays'],
  appointmentsStore.getHolidays
)

function handleSaveHolidays() {
  window.console.log(selectedHolidays.value)
}
</script>
