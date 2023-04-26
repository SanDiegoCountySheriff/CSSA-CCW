<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t(' Signature  ') }}
      <template #actions>
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <v-btn
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
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-draw
          </v-icon>
          <strong class="mr-3"> {{ $t('Signature Uploaded') }}: </strong>
          {{
            state.signature ? $t('Signature Uploaded') : $t('Missing Signature')
          }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import { onMounted, reactive } from 'vue'

const router = useRouter()
const applicationStore = useCompleteApplicationStore()

const state = reactive({
  signature: false,
})

onMounted(() => {
  applicationStore.completeApplication.application.uploadedDocuments.forEach(
    file => {
      if (file.documentType === 'signature') {
        state.signature = true
      }
    }
  )
})

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 10
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
.info-section-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
