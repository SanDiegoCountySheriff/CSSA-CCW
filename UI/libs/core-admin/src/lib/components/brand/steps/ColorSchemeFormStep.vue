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
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Primary Theme Color')"
            :rules="[
              v => !!v || $t('Primary Theme color is required'),
              v =>
                (v && v.length <= 7) ||
                $t('Primary Theme must be less than 7 characters'),
              v =>
                (v && v.length > 0 && v.startsWith('#')) ||
                $t('Primary Theme must start with #'),
            ]"
            v-model="brandStore.getBrand.primaryThemeColor"
            hint="Enter or pick primary color to apply across the application"
            placeholder="#XXXXXX"
            color="blue1"
            required
          >
            <template #append>
              <v-menu
                v-model="primaryMenu"
                top
                nudge-bottom="105"
                nudge-left="16"
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
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
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
            dense
            filled
            :label="$t('Secondary Theme Color')"
            :rules="[
              v => !!v || $t('Secondary Theme color is required'),
              v =>
                (v && v.length <= 7) ||
                $t('Secondary Theme must be less than 7 characters'),
              v =>
                (v && v.length > 0 && v.startsWith('#')) ||
                $t('Secondary Theme must start with #'),
            ]"
            v-model="brandStore.getBrand.secondaryThemeColor"
            placeholder="#XXXXXX"
            hint="Enter or pick secondary color to apply across the application"
            color="blue1"
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
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-text-field>
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
  </v-card>
</template>
<script setup lang="ts">
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'
import { computed, ref } from 'vue'

interface IColorSchemeFormStepProps {
  handleNextStep: () => void
  handleBackStep: () => void
  handleResetStep: () => void
}

const props = withDefaults(defineProps<IColorSchemeFormStepProps>(), {
  handleNextStep: () => null,
  handleBackStep: () => null,
  handleResetStep: () => null,
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

const {
  isLoading,
  isFetching,
  refetch: queryBrandSettings,
} = useQuery(['setBrandSettings'], brandStore.setBrandSettingApi, {
  enabled: false,
  onSuccess: () => {
    props.handleNextStep()
  },
})

async function getFormValues() {
  queryBrandSettings()
}
</script>
