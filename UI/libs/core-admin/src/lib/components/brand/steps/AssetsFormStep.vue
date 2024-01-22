<template>
  <v-card :loading="isLoading && isFetching">
    <v-form
      ref="form"
      v-model="valid"
      lazy-validation
    >
      <v-card-title>
        Agency Image Assets
        <v-spacer />
        <v-btn
          @click="getFormValues"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>Save
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col cols="6">
            <v-file-input
              v-model="brandStore.getDocuments.agencyLogo"
              :label="$t('Agency Logo')"
              :rules="[v => !!v || $t('Agency Logo is required')]"
              :hint="$t('Upload your Agency logo')"
              :placeholder="$t('Select your image')"
              append-icon="mdi-camera"
              prepend-icon=""
              accept="image/png, image/jpeg"
              @change="handleFileInput"
              required
            >
              <template #selection="{ index }">
                <v-chip
                  v-if="index < 2"
                  color="primary"
                  dark
                  label
                  small
                >
                  {{ 'agency_logo' }}
                </v-chip>
              </template>
            </v-file-input>
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
              v-model="brandStore.documents.agencySheriffSignatureImage"
              :label="$t('Agency Sheriff signature')"
              :rules="[v => !!v || $t('Agency Sheriff signature is required')]"
              :show-size="1000"
              :hint="$t('Upload Sheriff signature image')"
              :placeholder="$t('Select your image')"
              append-icon="mdi-camera"
              prepend-icon=""
              accept="image/png, image/jpeg"
              truncate-length="50"
              @change="handleFileInput"
              counter
              required
            >
              <template #selection="{ index }">
                <v-chip
                  v-if="index < 2"
                  color="primary"
                  dark
                  label
                  small
                >
                  {{ 'agency_sheriff_signature_image' }}
                </v-chip>
              </template>
            </v-file-input>
          </v-col>

          <v-col>
            <v-img
              alt="Agency sheriff signature image"
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

const allowedExtension = ['.png', '.jpeg', '.jpg']

const valid = ref(false)
const snackbar = ref(false)
const text = 'Invalid file type provided.'
const brandStore = useBrandStore()

useQuery(
  ['sheriffSignatureImage'],
  brandStore.getAgencySheriffSignatureImageApi
)

useQuery(['agencyHomePageImage'], brandStore.getAgencyHomePageImageApi)

const {
  isLoading,
  isFetching,
  refetch: queryLogo,
} = useQuery(['updateLogo'], brandStore.setAgencyLogoDocumentsApi, {
  enabled: false,
})

const { refetch: queryLandingPageImage } = useQuery(
  ['updateLandingPageImage'],
  brandStore.setAgencyLandingPageImageApi,
  {
    enabled: false,
  }
)

const { refetch: queryHomePageImage } = useQuery(
  ['updateHomePageImage'],
  brandStore.setAgencyHomePageImageApi,
  {
    enabled: false,
  }
)

const { refetch: querySheriffSignature } = useQuery(
  ['updateSheriffSignatureImage'],
  brandStore.setAgencySheriffSignatureImageApi,
  {
    enabled: false,
  }
)

async function getFormValues() {
  queryLogo()
  queryLandingPageImage()
  querySheriffSignature()
  queryHomePageImage()
}

function handleFileInput(e) {
  if (!allowedExtension.some(ext => e.name.toLowerCase().endsWith(ext))) {
    snackbar.value = true
  }
}
</script>
