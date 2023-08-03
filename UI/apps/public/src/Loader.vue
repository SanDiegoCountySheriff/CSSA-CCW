<template>
  <div class="text-center mt-16">
    <v-row>
      <v-col>
        <v-progress-circular
          size="300"
          color="primary"
          indeterminate
        >
          <v-row>
            <v-col>
              <v-avatar size="200">
                <v-img
                  :src="brandStore.getDocuments.agencyLogo"
                  alt="Image"
                  contain
                />
              </v-avatar>
            </v-col>
          </v-row>
        </v-progress-circular>
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        Loading the {{ brandStore.brand.agencyName }} CCW Application
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { useBrandStore } from '@shared-ui/stores/brandStore'

const brandStore = useBrandStore()
</script>
