<template>
  <div class="home">
    <img
      :class="{ dark: $vuetify.theme.dark }"
      v-if="store.getDocuments.agencyLandingPageImage"
      alt="Application logo"
      :src="store.getDocuments.agencyLandingPageImage"
    />
    <v-container fluid>
      <v-row>
        <v-col
          cols="12"
          lg="2"
        >
          <div class="option-section">
            <v-btn
              :color="$vuetify.theme.dark ? 'info' : 'primary'"
              class="option-button"
              outlined
              @click="handleRoute(Routes.APPLICATION_STATUS_PATH)"
            >
              <div class="option-inner">
                <v-icon
                  x-large
                  :color="$vuetify.theme.dark ? 'info' : 'primary'"
                  class="mb-3"
                >
                  mdi-card-account-details-outline
                </v-icon>
                {{ $t('View Applications') }}
              </div>
            </v-btn>
          </div>
        </v-col>
        <v-col
          cols="12"
          lg="5"
        >
          <GeneralInfoWrapper />
        </v-col>
        <v-col
          cols="12"
          lg="5"
        >
          <PriceInfoWrapper />
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import GeneralInfoWrapper from '@core-public/components/wrappers/GeneralInfoWrapper.vue';
import PriceInfoWrapper from '@core-public/components/wrappers/PriceInfoWrapper.vue';
import Routes from '@core-public/router/routes';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { useRouter } from 'vue-router/composables';

const store = useBrandStore();
const route = useRouter();

function handleRoute(path) {
  route.push(path);
}
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}

img.dark {
  background-color: #bbb;
  border-radius: 5px;
  padding: 5px;
}
.option {
  &-inner {
    background: transparent;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
  &-button {
    height: 6rem !important;
    padding: 0.5rem !important;
    margin: 0 1rem 1rem 1rem !important;
    min-width: 14rem !important;
  }
  &-section {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    padding-top: 0;
  }
}
</style>
