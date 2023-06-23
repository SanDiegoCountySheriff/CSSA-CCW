<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-xl text-left mb-5">
      {{ $t(' Previous Addresses: ') }}
      <template #actions>
        <v-btn
          v-if="
            applicationStore.completeApplication.application.status ==
            ApplicationStatus.Incomplete
          "
          icon
          @click="handleEditRequest"
        >
          <v-icon :color="$vuetify.theme.dark ? 'info' : 'info'">
            mdi-square-edit-outline
          </v-icon>
        </v-btn>
      </template>
    </v-banner>
    <v-row>
      <v-col>
        <AddressTable
          :enable-delete="false"
          :addresses="props.previousAddress"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes'
import AddressTable from '../tables/AddressTable.vue'
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'

interface PreviousAddressInfoSectionProps {
  previousAddress: Array<AddressInfoType>
  color: string
}

const props = defineProps<PreviousAddressInfoSectionProps>()
const applicationStore = useCompleteApplicationStore()
const router = useRouter()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 3
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  })
}
</script>

<style lang="scss" scoped>
.info-section-container {
  width: 100%;
  height: 100%;
  margin: 0.5em 0;
  padding: 0;
}
</style>
