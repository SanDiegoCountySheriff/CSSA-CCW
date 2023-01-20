<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title class="subtitle-2">
      {{ $t('Demographic Information:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-radio-group
          row
          v-model="
            permitStore.getPermitDetail.application.physicalAppearance.gender
          "
        >
          <template #label>
            <div>{{ $t('Gender') }}</div>
          </template>
          <v-radio value="male">
            <template #label>
              <div>{{ $t('Male') }}</div>
            </template>
          </v-radio>
          <v-radio value="female">
            <template #label>
              <div>{{ $t('Female') }}</div>
            </template>
          </v-radio>
          <template #prepend>
            <v-icon
              x-small
              color="error"
            >
              mdi-star
            </v-icon>
          </template>
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
    </v-row>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          dense
          outlined
          :label="$t('Height feet')"
          type="number"
          :rules="[
            v =>
              (v > 0 && v < 10) ||
              $t('Height feet must be greater than 0 and less than 10'),
            v => !!v || $t('Height feet is required'),
          ]"
          v-model="
            permitStore.getPermitDetail.application.physicalAppearance
              .heightFeet
          "
        >
          <template #prepend>
            <v-icon
              x-small
              color="error"
            >
              mdi-star
            </v-icon>
          </template>
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
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          dense
          outlined
          type="number"
          :label="$t('Height inches')"
          :rules="[
            v => !!v || $t('Height inches is required'),
            v =>
              (v >= 0 && v < 12) ||
              $t('Height in inches must be 0 or greater and less than 11'),
          ]"
          v-model="
            permitStore.getPermitDetail.application.physicalAppearance
              .heightInch
          "
        >
          <template #prepend>
            <v-icon
              x-small
              color="error"
            >
              mdi-star
            </v-icon>
          </template>
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

      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          dense
          outlined
          type="number"
          :label="$t('Weight')"
          :rules="[
            v => !!v || $t('Weight is required'),
            v =>
              (v > 0 && v < 1500) ||
              $t('Weight must greater than 0 and less than 1500'),
          ]"
          v-model="
            permitStore.getPermitDetail.application.physicalAppearance.weight
          "
        >
          <template #prepend>
            <v-icon
              x-small
              color="error"
            >
              mdi-star
            </v-icon>
          </template>
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

      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-select
          :items="hairColors"
          :label="$t('Hair Color')"
          :rules="[v => !!v || $t(' Hair color is required')]"
          autocomplete="nope"
          dense
          outlined
          v-model="
            permitStore.getPermitDetail.application.physicalAppearance.hairColor
          "
        >
          <template #prepend>
            <v-icon
              x-small
              color="error"
            >
              mdi-star
            </v-icon>
          </template>
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
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-select
          :items="eyeColors"
          :label="$t('Eye Color')"
          :rules="[v => !!v || $t('Eye color is required')]"
          autocomplete="nope"
          dense
          outlined
          v-model="
            permitStore.getPermitDetail.application.physicalAppearance.eyeColor
          "
        >
          <template #prepend>
            <v-icon
              x-small
              color="error"
            >
              mdi-star
            </v-icon>
          </template>
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
      <v-col
        cols="12"
        md="5"
        sm="12"
        class="pl-8"
      >
        <v-textarea
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
  </v-card>
</template>
<script setup lang="ts">
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { eyeColors, hairColors } from '@shared-utils/lists/defaultConstants';

const permitStore = usePermitsStore();
</script>
