<template>
  <v-form
    ref="form"
    v-model="valid"
    lazy-validation
  >
    <v-layout justify-center>
      <v-flex
        xs12
        sm10
        md8
        lg6
      >
        <v-card class="mt-5">
          <v-subheader class="sub-header font-weight-bold">
            {{ $t('Brandable Template') }}
          </v-subheader>
          <v-card-text>
            <v-row>
              <v-col
                cols="12"
                sm="6"
              >
                <v-file-input
                  v-model="state.agencyLogo"
                  :label="$t('Agency Logo')"
                  :rules="[v => !!v || 'Agency Logo is required']"
                  :show-size="1000"
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
                </v-file-input>
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
                  v-model="state.agencyName"
                  required
                />
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
                  v-model="state.agencySheriffName"
                  required
                />
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
                  v-model="state.chiefOfPoliceName"
                />
              </v-col>
            </v-row>
            <v-row>
              <v-col
                cols="12"
                md="6"
              >
                <v-text-field
                  :label="$t('Primary Theme Color')"
                  :rules="[v => !!v || 'Primary Theme color is required']"
                  v-model="state.primaryThemeColor"
                  required
                />
              </v-col>
            </v-row>
            <v-row>
              <v-col
                cols="12"
                md="6"
              >
                <v-text-field
                  :label="$t('Secondary Theme Color')"
                  :rules="[v => !!v || 'Secondary Theme color is required']"
                  v-model="state.secondaryThemeColor"
                  required
                />
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
              >
                <v-btn
                  color="primary"
                  class="mr-4"
                  :disabled="!valid"
                  @click="getFormValues"
                >
                  {{ $t('Publish') }}
                </v-btn>
              </v-col>
            </v-row>
          </v-card-actions>
        </v-card>
      </v-flex>
    </v-layout>
  </v-form>
</template>

<script setup lang="ts">
import { getCurrentInstance, ref } from 'vue';
import { useBrandStore } from '@core-admin/stores/brand';
import { BrandType } from '@core-admin/types';

const app = getCurrentInstance();

const valid = ref(false);

const state = ref<BrandType>({
  agencyName: '',
  agencySheriffName: '',
  chiefOfPoliceName: '',
  primaryThemeColor: app.proxy.$vuetify.theme.themes.light.primary,
  secondaryThemeColor: app.proxy.$vuetify.theme.themes.light.secondary,
  agencyLogo: null,
  agencyLogoDataURL: null,
});

const store = useBrandStore();

function readFile() {
  let file = state.value.agencyLogo;
  let reader = new FileReader();

  if (file) {
    reader.readAsDataURL(file);

    reader.onload = function () {
      state.value.agencyLogoDataURL = reader.result;
    };

    reader.onerror = function () {
      console.log(reader.error);
    };
  }
}

function getFormValues() {
  // Push model JSON to API endpoint for Public App consumption"
  readFile();
  store.setBrand(state.value);
}
</script>

<style lang="scss" scoped>
.sub-header {
  font-size: 1.5rem;
}

.form-container {
  min-height: 50%;
}
</style>
