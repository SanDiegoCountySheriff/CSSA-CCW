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
  <v-form
    v-else
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
            {{ $t('Brand Template') }}
          </v-subheader>
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
                  v-model="brandStore.getBrand.agencyName"
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
                  v-model="brandStore.getBrand.agencySheriffName"
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
                  v-model="brandStore.getBrand.chiefOfPoliceName"
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
                  v-model="brandStore.getBrand.primaryThemeColor"
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
                  v-model="brandStore.getBrand.secondaryThemeColor"
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
import { ref } from 'vue';
import { useQuery } from '@tanstack/vue-query';
import { useBrandStore } from '@core-admin/stores/brandStore';

const brandStore = useBrandStore();
const valid = ref(false);
const agencyLogo = ref<Blob | null>(null);

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
  await readFileAsync();
  brandStore.setBrandSettingApi();
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
