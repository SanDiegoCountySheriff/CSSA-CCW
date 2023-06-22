<template>
  <v-container class="confirm-info-section rounded mt-5">
    <v-banner class="sub-header font-weight-bold text-xl text-left mb-5">
      {{ $t(' Alias Information: ') }}
      <template #actions>
        <v-btn
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
        <AliasTable
          :enable-delete="false"
          :color="props.color"
          :aliases="props.aliasInfo"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import AliasTable from '@shared-ui/components/tables/AliasTable.vue'
import { AliasType } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'

interface IAliasInfoSectionProps {
  aliasInfo: Array<AliasType>
  color: string
}

const applicationStore = useCompleteApplicationStore()
const router = useRouter()
const props = defineProps<IAliasInfoSectionProps>()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 1
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  })
}
</script>

<style lang="scss">
.confirm-info-section {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
