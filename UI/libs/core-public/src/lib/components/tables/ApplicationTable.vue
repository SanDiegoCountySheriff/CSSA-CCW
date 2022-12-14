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
        color="accent"
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
          color="green"
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
        color="error"
        v-if="!props.item.application.appointmentStatus"
      >
        {{ $t('Not scheduled') }}
      </v-chip>
      <v-chip
        v-else
        small
        color="accent"
      >
        {{ $t('Scheduled') }}
      </v-chip>
    </template>
    <template #item.status="props">
      <v-chip
        small
        color="warning"
        v-if="props.item.application.status === 1"
      >
        {{ $t('Started') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === 2"
        small
        color="accent"
      >
        {{ $t('Submitted') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === 3"
        small
        color="primary"
      >
        {{ $t('In Progress') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === 4"
        small
        color="error"
      >
        {{ $t('Cancelled') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === 5"
        small
        color="warning"
      >
        {{ $t('Returned') }}
      </v-chip>
      <v-chip
        v-if="props.item.application.status === 6"
        small
        color="success"
      >
        {{ $t('Complete') }}
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
        v-if="props.item.application.status === 1"
        :delete-function="() => handleDelete(props.item)"
      />
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import DeleteDialog from '@shared-ui/components/dialogs/DeleteDialog.vue';

interface IProps {
  headers: Array<unknown>;
  items: Array<CompleteApplication>;
  isLoading: boolean;
}

const emit = defineEmits(['selected', 'delete']);

function handleDelete(item: CompleteApplication) {
  emit('delete', item.application.orderId);
}

const comProps = defineProps<IProps>();
</script>
