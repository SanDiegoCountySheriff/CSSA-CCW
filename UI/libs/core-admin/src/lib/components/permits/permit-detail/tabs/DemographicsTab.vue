<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Demographic Information') }}
      <v-spacer></v-spacer>
      <SaveButton
        :disabled="!valid"
        @on-save="handleSave"
      />
    </v-card-title>
    <v-card-text>
      <v-form v-model="valid">
        <v-row>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.physicalAppearance
                  .gender
              "
              :label="$t('Gender:')"
              row
            >
              <v-radio
                value="male"
                :label="$t('Male')"
              >
              </v-radio>
              <v-radio
                value="female"
                :label="$t('Female')"
              >
              </v-radio>
              <template #append>
                <v-icon
                  color="error"
                  class="mr-3"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.physicalAppearance
                      .gender
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-radio-group>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.physicalAppearance
                  .heightFeet
              "
              :label="$t('Height feet')"
              :rules="[
                v =>
                  (v > 0 && v < 10) ||
                  $t('Height feet must be greater than 0 and less than 10'),
                v => !!v || $t('Height feet is required'),
              ]"
              type="number"
              outlined
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.physicalAppearance
                      .heightFeet
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.physicalAppearance
                  .heightInch
              "
              :label="$t('Height inches')"
              :rules="[
                v => !!v || $t('Height inches is required'),
                v =>
                  (v >= 0 && v < 12) ||
                  $t('Height in inches must be 0 or greater and less than 11'),
              ]"
              type="number"
              outlined
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.physicalAppearance
                      .heightInch
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.physicalAppearance
                  .weight
              "
              :label="$t('Weight')"
              :rules="[
                v => !!v || $t('Weight is required'),
                v =>
                  (v > 0 && v < 1500) ||
                  $t('Weight must greater than 0 and less than 1500'),
              ]"
              type="number"
              outlined
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.physicalAppearance
                      .weight
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col>
            <v-select
              v-model="
                permitStore.getPermitDetail.application.physicalAppearance
                  .hairColor
              "
              :items="hairColors"
              :label="$t('Hair Color')"
              :rules="[v => !!v || $t(' Hair color is required')]"
              outlined
              dense
              :menu-props="{ bottom: true, offsetY: true }"
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.physicalAppearance
                      .hairColor
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-select>
          </v-col>
          <v-col>
            <v-select
              v-model="
                permitStore.getPermitDetail.application.physicalAppearance
                  .eyeColor
              "
              :items="eyeColors"
              :label="$t('Eye Color')"
              :rules="[v => !!v || $t('Eye color is required')]"
              outlined
              dense
              :menu-props="{ bottom: true, offsetY: true }"
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.physicalAppearance
                      .eyeColor
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-select>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="6">
            <v-textarea
              outlined
              v-model="
                permitStore.getPermitDetail.application.physicalAppearance
                  .physicalDesc
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
              :label="$t('Physical Description')"
            />
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import { ref } from 'vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { eyeColors, hairColors } from '@shared-utils/lists/defaultConstants'

const permitStore = usePermitsStore()
const emit = defineEmits(['on-save'])
const valid = ref(false)

function handleSave() {
  emit('on-save', 'Demographics Information')
}
</script>
