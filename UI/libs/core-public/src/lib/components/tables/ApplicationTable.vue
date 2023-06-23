<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/valid-v-slot -->
<!-- eslint-disable vue-a11y/no-autofocus -->
<template>
  <v-data-table
    :headers="comProps.headers"
    :items="comProps.items"
    :item-key="'orderId'"
    :loading="!comProps.isLoading"
    :loading-text="$t('Loading applications')"
  >
    <template #item.orderId="props">
      <v-btn
        color="primary"
        :loading="!isLoading"
        small
        text
        @click="emit('selected', props.item)"
      >
        {{ props.item.application.orderId }}
      </v-btn>
    </template>
    <template #item.completed="props">
      <div v-if="props.item.application.isComplete">
        <v-icon
          medium
          color="success"
        >
          mdi-check-circle
        </v-icon>
        <span class="ml-3">{{ $t('Completed') }}</span>
      </div>
      <div v-else>
        <v-icon
          color="error"
          medium
        >
          mdi-alert-octagon
        </v-icon>
        <span class="ml-3">{{ $t('Not Completed') }}</span>
      </div>
    </template>

    <template #item.appointmentStatus="props">
      <v-chip
        small
        color="primary"
      >
        {{ AppointmentStatus[props.item.application.appointmentStatus] }}
      </v-chip>
    </template>
    <template #item.status="props">
      <v-chip
        small
        color="warning"
        v-if="props.item.application.status === ApplicationStatus.Incomplete"
      >
        {{ $t('Incomplete') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === ApplicationStatus.Submitted"
        small
        color="primary"
      >
        {{ $t('Submitted') }}
      </v-chip>
      <v-chip
        v-if="
          props.item.application.status ===
          ApplicationStatus['Ready For Appointment']
        "
        small
        color="primary"
      >
        {{ $t('Ready For Appointment') }}
      </v-chip>
      <v-chip
        v-if="
          props.item.application.status ===
          ApplicationStatus['Appointment Complete']
        "
        small
        color="error"
      >
        {{ $t('Appointment Complete') }}
      </v-chip>
      <v-chip
        v-if="
          props.item.application.status ===
          ApplicationStatus['Background In Progress']
        "
        small
        color="warning"
      >
        {{ $t('Background In Progress') }}
      </v-chip>
      <v-chip
        v-if="
          props.item.application.status ===
          ApplicationStatus['Contingently Approved']
        "
        small
        color="success"
      >
        {{ $t('Complete') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === ApplicationStatus.Approved"
        small
        color="success"
      >
        {{ $t('Approved') }}
      </v-chip>
      <v-chip
        v-if="
          props.item.application.status ===
          ApplicationStatus['Permit Delivered']
        "
        small
        color="success"
      >
        {{ $t('Permit Delivered') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === ApplicationStatus.Suspended"
        small
        color="warning"
      >
        {{ $t('Suspended') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === ApplicationStatus.Revoked"
        small
        color="warning"
      >
        {{ $t('Revoked') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === ApplicationStatus.Cancelled"
        small
        color="warning"
      >
        {{ $t('Cancelled') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === ApplicationStatus.Denied"
        small
        color="warning"
      >
        {{ $t('Denied') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === ApplicationStatus.Withdrawn"
        small
        color="warning"
      >
        {{ $t('WithDrawn') }}
      </v-chip>
    </template>
    <template #item.appointmentDate="props">
      {{
        new Date(props.item.application.appointmentDateTime).toISOString() >
        new Date(Date.now()).toISOString()
          ? new Date(
              props.item.application.appointmentDateTime
            ).toLocaleString()
          : $t('Not Scheduled')
      }}
    </template>
    <template #item.step="props">
      {{ props.item.application.currentStep }}
    </template>
    <template #item.type="props">
      {{ props.item.application.applicationType }}
    </template>
    <template #item.delete="props">
      <DeleteDialog
        v-if="props.item.application.status === ApplicationStatus.Incomplete"
        :delete-function="() => handleDelete(props.item)"
      />
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import {
  ApplicationStatus,
  AppointmentStatus,
} from '@shared-utils/types/defaultTypes'
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import DeleteDialog from '@shared-ui/components/dialogs/DeleteDialog.vue'

interface IProps {
  headers: Array<unknown>
  items: Array<CompleteApplication>
  isLoading: boolean
}

const emit = defineEmits(['selected', 'delete'])

function handleDelete(item: CompleteApplication) {
  emit('delete', item.id)
}

const comProps = defineProps<IProps>()
</script>
