<template>
  <v-card :loading="loading.value">
    <v-form
      ref="form"
      v-model="valid"
      lazy-validation
    >
      <v-card-title>
        Agency Image Assets
        <v-spacer />
        <v-btn
          @click="save"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>Save
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col cols="6">
            <v-file-input
              v-model="agencyLogo"
              :label="$t('Agency Logo')"
              @change="handleFileInput"
              accept="image/png, image/jpeg"
              outlined
            />
          </v-col>

          <v-col cols="6">
            <v-img
              alt="Agency logo"
              :src="brandStore.documents.agencyLogo"
              max-height="400"
              contain
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-file-input
              v-model="agencySignature"
              :label="$t('Agency Head Signature')"
              @change="handleFileInput"
              accept="image/png, image/jpeg"
              outlined
            />
          </v-col>

          <v-col>
            <v-img
              alt="Agency head signature image"
              :src="brandStore.getDocuments.agencySheriffSignatureImage"
              max-height="200"
              contain
            />
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>

    <v-snackbar v-model="snackbar">
      {{ text }}
      <template #action="{ attrs }">
        <v-btn
          color="red"
          text
          v-bind="attrs"
          @click="snackbar = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'
import { useTanstack } from '@shared-ui/composables/useTanstack'

const allowedExtension = ['.png', '.jpeg', '.jpg']

const agencyLogo = ref<File>()
const agencySignature = ref<File>()
const valid = ref(false)
const snackbar = ref(false)
const text = 'Invalid file type provided.'
const brandStore = useBrandStore()

useQuery(
  ['sheriffSignatureImage'],
  brandStore.getAgencySheriffSignatureImageApi
)

const { loading, setAgencyLogo, setAgencySignature } = useTanstack()

async function save() {
  if (agencyLogo.value) {
    const logoFormData = new FormData()

    logoFormData.append('fileToUpload', agencyLogo.value)
    setAgencyLogo(logoFormData)
  }

  if (agencySignature.value) {
    const signatureFormData = new FormData()

    signatureFormData.append('fileToUpload', agencySignature.value)
    setAgencySignature(signatureFormData)
  }
}

function handleFileInput(e) {
  if (!allowedExtension.some(ext => e.name.toLowerCase().endsWith(ext))) {
    snackbar.value = true
  }
}
</script>
