<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5">
      {{ $t('Weapons Information') }}
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
        <WeaponsTable
          :weapons="props.weapons"
          :delete-enabled="false"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'

interface IWeaponsInfoSectionProps {
  weapons: Array<WeaponInfoType>
}

const props = defineProps<IWeaponsInfoSectionProps>()
const applicationStore = useCompleteApplicationStore()
const router = useRouter()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 6
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete.toString(),
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
