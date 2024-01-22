<template>
  <v-card :loading="isLoading && isFetching">
    <v-form
      ref="form"
      v-model="valid"
      lazy-validation
    >
      <v-card-title>
        Agency Color Settings
        <v-spacer />

        <v-btn
          @click="getFormValues"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>
          Save
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col cols="6">
            <v-text-field
              v-model="brandStore.getBrand.primaryThemeColor"
              :label="$t('Color Theme Light Mode')"
              :rules="[
                v => !!v || $t('Color theme is required'),
                v =>
                  (v && v.length <= 7) ||
                  $t('Color theme must be less than 7 characters'),
                v =>
                  (v && v.length > 0 && v.startsWith('#')) ||
                  $t('Color theme must start with #'),
              ]"
              hint="This color will apply to various components when in light mode."
              color="primary"
              outlined
              required
            >
              <template #append>
                <v-menu
                  v-model="primaryMenu"
                  bottom
                  :close-on-content-click="false"
                >
                  <template #activator="{ on }">
                    <v-btn
                      color="primary"
                      v-on="on"
                    >
                      Color Picker
                    </v-btn>
                  </template>

                  <v-card>
                    <v-card-text class="pa-0">
                      <v-color-picker
                        v-model="brandStore.getBrand.primaryThemeColor"
                        flat
                      />
                    </v-card-text>
                  </v-card>
                </v-menu>
              </template>
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="6">
            <v-text-field
              v-model="brandStore.getBrand.secondaryThemeColor"
              :label="$t('Color Theme Dark Mode')"
              :rules="[
                v => !!v || $t('Color theme is required'),
                v =>
                  (v && v.length <= 7) ||
                  $t('Color theme must be less than 7 characters'),
                v =>
                  (v && v.length > 0 && v.startsWith('#')) ||
                  $t('Color theme must start with #'),
              ]"
              hint="This color will apply to various components when in dark mode."
              color="primary"
              required
              outlined
            >
              <template #append>
                <v-menu
                  v-model="secondaryMenu"
                  top
                  nudge-bottom="105"
                  nudge-left="16"
                  :close-on-content-click="false"
                >
                  <template #activator="{ on }">
                    <v-btn
                      color="secondary"
                      v-on="on"
                    >
                      Color Picker
                    </v-btn>
                  </template>

                  <v-card>
                    <v-card-text class="pa-0">
                      <v-color-picker
                        v-model="brandStore.getBrand.secondaryThemeColor"
                        flat
                      />
                    </v-card-text>
                  </v-card>
                </v-menu>
              </template>
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-btn color="primary"> Example Button </v-btn>
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { BrandType } from '@shared-utils/types/defaultTypes'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'
import { getCurrentInstance, ref, watch } from 'vue'

const app = getCurrentInstance()

const valid = ref(false)
const primaryMenu = ref(false)
const secondaryMenu = ref(false)
const brandStore = useBrandStore()

const {
  isLoading,
  isFetching,
  refetch: queryBrandSettings,
} = useQuery(['setBrandSettings'], brandStore.setBrandSettingApi, {
  enabled: false,
})

async function getFormValues() {
  queryBrandSettings()
}

watch(brandStore.brand, (newVal: BrandType) => {
  if (app) {
    app.proxy.$vuetify.theme.themes.light.primary = newVal.primaryThemeColor
  }
})

watch(brandStore.brand, (newVal: BrandType) => {
  if (app) {
    app.proxy.$vuetify.theme.themes.dark.primary = newVal.secondaryThemeColor
  }
})
</script>
