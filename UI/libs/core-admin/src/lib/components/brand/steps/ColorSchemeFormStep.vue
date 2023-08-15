<template>
  <v-form
    ref="form"
    class="ml-4 mt-4"
    v-model="valid"
    lazy-validation
  >
    <v-row>
      <v-col
        cols="12"
        md="6"
      >
        <v-text-field
          outlined
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
          v-model="brandStore.getBrand.primaryThemeColor"
          hint="This color will apply to various components when in light mode."
          placeholder="#XXXXXX"
          color="primary"
          required
        >
          <template #append>
            <v-menu
              v-model="primaryMenu"
              bottom
              :close-on-content-click="false"
            >
              <template #activator="{ on }">
                <div
                  :style="primarySwatchStyle"
                  v-on="on"
                />
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
      <v-col
        cols="12"
        md="6"
        class="shrink"
        style="min-width: 220px"
      >
        <v-text-field
          outlined
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
          v-model="brandStore.getBrand.secondaryThemeColor"
          placeholder="#XXXXXX"
          hint="This color will apply to various components when in dark mode."
          color="primary"
          required
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
                <div
                  :style="secondarySwatchStyle"
                  v-on="on"
                />
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
    <v-row justify="space-between">
      <v-col
        cols="12"
        sm="6"
      >
        <v-btn
          @click="props.handleBackStep"
          color="primary"
        >
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
  </v-form>
</template>

<script setup lang="ts">
import { BrandType } from '@shared-utils/types/defaultTypes'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'
import { computed, getCurrentInstance, ref, watch } from 'vue'

const app = getCurrentInstance()

interface IColorSchemeFormStepProps {
  handleNextStep: () => void
  handleBackStep: () => void
}

const props = withDefaults(defineProps<IColorSchemeFormStepProps>(), {
  handleNextStep: () => null,
  handleBackStep: () => null,
})

const valid = ref(false)
const primaryMenu = ref(false)
const secondaryMenu = ref(false)
const brandStore = useBrandStore()

const primarySwatchStyle = computed(() => {
  return {
    backgroundColor: brandStore.getBrand.primaryThemeColor,
    cursor: 'pointer',
    height: '30px',
    width: '30px',
    marginBottom: '2px',
    borderRadius: primaryMenu.value ? '50%' : '4px',
    transition: 'border-radius 200ms ease-in-out',
  }
})

const secondarySwatchStyle = computed(() => {
  return {
    backgroundColor: brandStore.getBrand.secondaryThemeColor,
    cursor: 'pointer',
    height: '30px',
    width: '30px',
    marginBottom: '2px',
    borderRadius: primaryMenu.value ? '50%' : '4px',
    transition: 'border-radius 200ms ease-in-out',
  }
})

const { refetch: queryBrandSettings } = useQuery(
  ['setBrandSettings'],
  brandStore.setBrandSettingApi,
  {
    enabled: false,
    onSuccess: () => {
      props.handleNextStep()
    },
  }
)

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
