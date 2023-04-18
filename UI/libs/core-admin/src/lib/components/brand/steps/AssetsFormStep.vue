<template>
  <v-card
    color="lighten-1"
    class="mb-8 ml-2 mr-2 mt-6 elevation-3"
    height="100%"
  >
    <v-progress-linear
      :active="isLoading && isFetching"
      :indeterminate="isLoading && isFetching"
      absolute
      bottom
      color="primary"
    >
    </v-progress-linear>
    <v-form
      ref="form"
      class="ml-4"
      v-model="valid"
      lazy-validation
    >
      <v-row>
        <v-col
          cols="12"
          sm="6"
        >
          <v-file-input
            v-model="brandStore.getDocuments.agencyLogo"
            class="ml-5"
            :label="$t('Agency Logo')"
            :rules="[v => !!v || $t('Agency Logo is required')]"
            :show-size="1000"
            :hint="$t('Upload your Agency logo')"
            :placeholder="$t('Select your image')"
            append-icon="mdi-camera"
            prepend-icon=""
            accept="image/png, image/jpeg"
            @change="handleFileInput"
            truncate-length="50"
            counter
            required
          >
            <template #selection="{ index }">
              <v-chip
                v-if="index < 2"
                color="blue1"
                dark
                label
                small
              >
                {{ 'agency_logo' }}
              </v-chip>
            </template>
          </v-file-input>
        </v-col>
        <v-col>
          <img
            alt="Agency logo"
            :src="brandStore.documents.agencyLogo"
          />
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          sm="6"
        >
          <v-file-input
            v-model="brandStore.documents.agencyLandingPageImage"
            class="ml-5"
            :label="$t('Agency Landing page image')"
            :rules="[v => !!v || $t('Agency Landing page image is required')]"
            :show-size="1000"
            :hint="$t('Upload your Landing page image')"
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
                color="blue1"
                dark
                label
                small
              >
                {{ 'agency_landing_page_image' }}
              </v-chip>
            </template>
          </v-file-input>
        </v-col>
        <v-col>
          <img
            alt="Agency landing page image"
            :src="brandStore.getDocuments.agencyLandingPageImage"
          />
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          sm="6"
        >
          <v-file-input
            v-model="brandStore.documents.agencySheriffSignatureImage"
            class="ml-5"
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
                color="blue1"
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
          <img
            alt="Agency sheriff signature image"
            :src="brandStore.getDocuments.agencySheriffSignatureImage"
          />
        </v-col>
      </v-row>
      <v-row justify="space-between">
        <v-col
          cols="12"
          sm="6"
        >
          <v-btn @click="handleResetStep">
            {{ $t('Cancel') }}
          </v-btn>
          <v-btn @click="props.handleBackStep">
            {{ $t('Back') }}
          </v-btn>
        </v-col>
        <v-col
          cols="12"
          sm="6"
        >
          <v-btn
            color="primary"
            :disabled="!valid"
            @click="getFormValues"
          >
            {{ $t('Publish') }}
          </v-btn>
        </v-col>
      </v-row>
      <v-spacer></v-spacer>
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
import { ref } from 'vue';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { useQuery } from '@tanstack/vue-query';

interface IAssetsFormStepProps {
  handleNextStep: () => void;
  handleBackStep: () => void;
  handleResetStep: () => void;
}

const props = withDefaults(defineProps<IAssetsFormStepProps>(), {
  handleNextStep: () => null,
  handleBackStep: () => null,
  handleResetStep: () => null,
});

const allowedExtension = ['.png', '.jpeg', '.jpg'];

const valid = ref(false);
const snackbar = ref(false);
const text = 'Invalid file type provided.';
const brandStore = useBrandStore();

useQuery(
  ['sheriffSignatureImage'],
  brandStore.getAgencySheriffSignatureImageApi
);

const {
  isLoading,
  isFetching,
  refetch: queryLogo,
} = useQuery(['updateLogo'], brandStore.setAgencyLogoDocumentsApi, {
  enabled: false,
  onSuccess: () => {
    props.handleNextStep();
  },
});

const { refetch: queryLandingPageImage } = useQuery(
  ['updateLandingPageImage'],
  brandStore.setAgencyLandingPageImageApi,
  {
    enabled: false,
  }
);

const { refetch: querySheriffSignature } = useQuery(
  ['updateSheriffSignatureImage'],
  brandStore.setAgencySheriffSignatureImageApi,
  {
    enabled: false,
  }
);

async function getFormValues() {
  queryLogo();
  queryLandingPageImage();
  querySheriffSignature();
}

function handleFileInput(e) {
  if (!allowedExtension.some(ext => e.name.toLowerCase().endsWith(ext))) {
    snackbar.value = true;
  }
}
</script>
<style lang="scss" scoped>
img {
  max-width: 35%;
}
</style>
