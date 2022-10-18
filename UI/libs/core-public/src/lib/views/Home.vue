<template>
  <div class="home">
    <!-- TODO: Make this the custom logo -->
    <img
      v-if="store.getBrand.agencyLogoDataURL"
      alt="Application logo"
      :src="store.getBrand.agencyLogoDataURL"
    />
    <v-container>
      <div class="option-section">
        <v-btn
          outlined
          color="primary"
          class="option-button"
          @click="handleRoute('/application')"
        >
          <div class="option-inner">
            <v-icon
              x-large
              color="primary"
              class="mb-3"
            >
              mdi-card-account-details
            </v-icon>
            {{ $t('NEW/EDIT CCW License') }}
          </div>
        </v-btn>
        <v-btn
          color="primary"
          class="option-button"
          outlined
          @click="handleRoute('/renew-application')"
        >
          <div class="option-inner">
            <v-icon
              x-large
              color="primary"
              class="mb-3"
            >
              mdi-card-account-details-star
            </v-icon>
            {{ $t('RENEW CCW License') }}
          </div>
        </v-btn>
      </div>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { useBrandStore } from '@core-public/stores/brandStore';
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
.option {
  &-inner {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
  &-button {
    height: 6rem !important;
    padding: 0.5rem !important;
    margin: 0.5rem 0 !important;
    min-width: 16rem !important;
  }
  &-section {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
}
</style>
