<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t(' Signature  ') }}
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
            color="primary"
          >
            mdi-draw
          </v-icon>
          <strong class="mr-3"> {{ $t('Signature Uploaded') }}: </strong>
          {{
            state.signature ? $t('Signature Uploaded') : $t('Missing Signature')
          }}
          <v-container ml-12>
            <ModifySignatureDialog
              v-if="state.signature"
            ></ModifySignatureDialog>
          </v-container>
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import ModifySignatureDialog from '@shared-ui/components/dialogs/ModifySignatureDialog.vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { onMounted, reactive } from 'vue'

const applicationStore = useCompleteApplicationStore()

const state = reactive({
  signature: false,
  signatureName: '',
  application: [applicationStore.completeApplication],
  files: [] as Array<{ formData; target }>,
})

onMounted(() => {
  applicationStore.completeApplication.application.uploadedDocuments.forEach(
    file => {
      if (file.documentType === 'Signature') {
        state.signature = true
      }
    }
  )
})
</script>

<style lang="scss">
.info-section-container {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
