<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t('File Upload Information') }}
      <template #actions>
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <v-btn
              v-if="
                applicationStore.completeApplication.application.status == 1
              "
              icon
              @click="handleEditRequest"
              v-bind="attrs"
              v-on="on"
            >
              <v-icon color="info"> mdi-square-edit-outline </v-icon>
            </v-btn>
          </template>
          {{ $t('Edit Section') }}
        </v-tooltip>
      </template>
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="12"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-file
          </v-icon>
          <strong>
            {{ $t(' Uploaded Files: ') }}
          </strong>
          <v-chip
            v-for="(file, index) in props.uploadedDocuments"
            :key="index"
            color="info"
            small
            class="ml-2"
          >
            {{ file.documentType }}
          </v-chip>
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import { useRouter } from 'vue-router/composables'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'

interface IFileUploadInfoSection {
  uploadedDocuments: UploadedDocType[]
  color: string
}

const props = defineProps<IFileUploadInfoSection>()
const router = useRouter()
const applicationStore = useCompleteApplicationStore()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 8
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
  margin: 0;
  padding: 0;
}
</style>
