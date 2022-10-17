<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container
    v-if="isLoading && !isError"
    fluid
  >
    <v-skeleton-loader
      fluid
      class="fill-height"
      type="list-item, 
              divider, list-item-three-line, 
              card-heading, image, image, image,
              image, actions"
    >
    </v-skeleton-loader>
  </v-container>
  <v-container
    v-else
    class="brand-form-container"
    fluid
    style="height: 100vh"
  >
    <v-layout
      justify-center
      fill-height
    >
      <v-flex
        xs12
        sm10
        md11
        lg8
      >
        <v-form
          ref="form"
          v-model="valid"
          lazy-validation
        >
          <v-subheader class="sub-header font-weight-bold">
            {{ $t('BRAND SETTINGS') }}
          </v-subheader>
          <v-card class="mt-5">
            <v-card-text>
              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-file-input
                    v-model="agencyLogo"
                    :label="$t('Agency Logo')"
                    :rules="[v => !!v || 'Agency Logo is required']"
                    :show-size="1000"
                    @change="readFileAsync"
                    placeholder="Select your file"
                    prepend-icon="mdi-camera"
                    accept="image/png, image/jpeg"
                    truncate-length="50"
                    counter
                    required
                  >
                    <template #selection="{ index, text }">
                      <v-chip
                        v-if="index < 2"
                        color="deep-purple accent-4"
                        dark
                        label
                        small
                      >
                        {{ text }}
                      </v-chip>
                    </template>
                    <template #prepend>
                      <v-icon
                        x-small
                        color="error"
                      >
                        mdi-asterisk
                      </v-icon>
                    </template>
                  </v-file-input>
                </v-col>
                <v-col>
                  <img
                    alt="Application logo"
                    :src="brandStore.getBrand.agencyLogoDataURL"
                  />
                </v-col>
              </v-row>
              <v-row>
                <v-col
                  cols="12"
                  md="6"
                >
                  <v-text-field
                    :label="$t('Agency Name')"
                    :rules="[v => !!v || 'Agency Name is required']"
                    v-model="brandStore.getBrand.agencyName"
                    required
                  >
                    <template #prepend>
                      <v-icon
                        x-small
                        color="error"
                      >
                        mdi-asterisk
                      </v-icon>
                    </template>
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col
                  cols="12"
                  md="6"
                >
                  <v-text-field
                    :label="$t('Agency Sheriff Name')"
                    :rules="[v => !!v || 'Agency Sheriff Name is required']"
                    v-model="brandStore.getBrand.agencySheriffName"
                    required
                  >
                    <template #prepend>
                      <v-icon
                        x-small
                        color="error"
                      >
                        mdi-asterisk
                      </v-icon>
                    </template>
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col
                  cols="12"
                  md="6"
                >
                  <v-text-field
                    :label="$t('Chief of Police Name')"
                    :rules="[v => !!v || 'Chief of Police name is required']"
                    v-model="brandStore.getBrand.chiefOfPoliceName"
                  >
                    <template #prepend>
                      <v-icon
                        x-small
                        color="error"
                        v-icon
                      >
                      </v-icon>
                    </template>
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col
                  cols="12"
                  md="6"
                >
                  <v-text-field
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
                    v-mask="'#XXXXXX'"
                    placeholder="#XXXXXX"
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
                        mdi-asterisk
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
                    v-mask="'#XXXXXX'"
                    placeholder="#XXXXXX"
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
                        mdi-asterisk
                      </v-icon>
                    </template>
                  </v-text-field>
                </v-col>
              </v-row>
            </v-card-text>
            <v-card-actions>
              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-btn>
                    {{ $t('Cancel') }}
                  </v-btn>
                </v-col>
              </v-row>
              <v-spacer></v-spacer>
              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                  class="ml-auto"
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
            </v-card-actions>
          </v-card>
        </v-form>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { useQuery } from '@tanstack/vue-query';
import { useBrandStore } from '@core-admin/stores/brandStore';

const brandStore = useBrandStore();
const valid = ref(false);
const agencyLogo = ref<Blob | null>(null);
const primaryMenu = ref(false);
const secondaryMenu = ref(false);

const primarySwatchStyle = computed(() => {
  return {
    backgroundColor: brandStore.getBrand.primaryThemeColor,
    cursor: 'pointer',
    height: '30px',
    width: '30px',
    marginBottom: '2px',
    borderRadius: primaryMenu.value ? '50%' : '4px',
    transition: 'border-radius 200ms ease-in-out',
  };
});

const secondarySwatchStyle = computed(() => {
  return {
    backgroundColor: brandStore.getBrand.secondaryThemeColor,
    cursor: 'pointer',
    height: '30px',
    width: '30px',
    marginBottom: '2px',
    borderRadius: primaryMenu.value ? '50%' : '4px',
    transition: 'border-radius 200ms ease-in-out',
  };
});

const { isLoading, isError } = useQuery(
  ['brand'],
  brandStore.getBrandSettingApi
);

function readFileAsync() {
  return new Promise((resolve, reject) => {
    let file = agencyLogo.value;
    let reader = new FileReader();

    if (file) {
      reader.readAsDataURL(file);
      reader.onload = () => {
        resolve(brandStore.setLogoDataURL(reader.result));
      };
      reader.onerror = reject;
    }
  });
}

async function getFormValues() {
  brandStore.setBrandSettingApi();
}
</script>

<style lang="scss" scoped>
.sub-header {
  font-size: 1.5rem;
}

.brand-form-container {
  min-height: 100vh;

  img {
    max-width: 35%;
  }
}
</style>
