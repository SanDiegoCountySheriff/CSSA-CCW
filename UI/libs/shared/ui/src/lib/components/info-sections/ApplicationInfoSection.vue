<template>
  <div>
    <v-card-title>
      {{ $t('Application-Type-Header') }}
    </v-card-title>
    <p class="text-left ml-5">
      {{ $t('Application-Type-Subheader') }}
    </p>
    <v-card-text>
      <div class="text-line">
        <h3 class="text-label">
          {{ $t('STANDARD') }}
        </h3>
        <p class="text-info">
          {{ $t('Application-Standard') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('JUDICIAL-title') }}
        </h3>
        <p class="text-info">
          {{ $t('Application-Type-Judicial') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('RESERVE-title') }}
        </h3>
        <p class="text-info">
          {{ $t('Application-Type-Reserve') }}
        </p>
      </div>
      <div
        v-if="brandStore.brand.employmentLicense"
        class="text-line"
      >
        <h3 class="text-label">
          {{ 'EMPLOYMENT' }}
        </h3>
        <p class="text-info">
          {{ $t('Application-Type-Employment') }}
        </p>
      </div>
    </v-card-text>
  </div>
</template>

<script setup lang="ts">
import { useBrandStore } from '@shared-ui/stores/brandStore'

const brandStore = useBrandStore()
</script>

<style lang="scss" scoped>
.text {
  &-line {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
  }

  &-label {
    width: fit-content;
  }

  &-info {
    margin-left: 0.5rem;
  }
}
li {
  color: #111;
}
</style>
