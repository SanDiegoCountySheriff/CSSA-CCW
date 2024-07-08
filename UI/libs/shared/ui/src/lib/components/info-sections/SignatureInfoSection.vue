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
            <v-btn
              v-if="state.signature"
              color="primary"
              tonal
              @click="handleModifyDocument"
            >
              {{ $t('Edit Signature') }}
            </v-btn>
          </v-container>
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { onMounted, reactive } from 'vue'

const applicationStore = useCompleteApplicationStore()

const state = reactive({
  signature: false,
  signatureName: '',
  application: [applicationStore.completeApplication],
  files: [] as Array<{ formData; target }>,
})

const router = useRouter()

const { mutate: updateMutation } = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication()
  },
  onSuccess: () => {
    for (let item of applicationStore.completeApplication.application
      .uploadedDocuments) {
      switch (item.documentType.toLowerCase()) {
        case 'signature':
          state.signatureName = item.name
          break
        default:
          break
      }
    }

    state.files = []
  },
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

function handleModifyDocument() {
  applicationStore.completeApplication.application.uploadedDocuments.forEach(
    file => {
      if (file.documentType === 'Signature') {
        deleteFile(file.name).then(() => {
          viewSignatureSection()
        })
      }
    }
  )
}

function viewSignatureSection() {
  router.push({
    path: `/form`,
    query: {
      applicationId: state.application[0].id,
      isComplete: state.application[0].application.isComplete.toString(),
    },
  })
}

async function deleteFile(name) {
  const documentToDelete =
    applicationStore.completeApplication.application.uploadedDocuments.find(
      doc => doc.name === name
    )

  if (!documentToDelete) {
    return
  }

  try {
    await axios
      .delete(
        `${Endpoints.DELETE_DOCUMENT_FILE_PUBLIC_ENDPOINT}?applicantFileName=${name}`
      )
      .then(() => {
        applicationStore.completeApplication.application.uploadedDocuments.pop()
      })

    const updatedDocuments =
      applicationStore.completeApplication.application.uploadedDocuments.filter(
        doc => doc.name !== name
      )

    applicationStore.completeApplication.application.uploadedDocuments =
      updatedDocuments

    updateMutation()
    // eslint-disable-next-line no-empty
  } finally {
  }
}
</script>

<style lang="scss">
.info-section-container {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
