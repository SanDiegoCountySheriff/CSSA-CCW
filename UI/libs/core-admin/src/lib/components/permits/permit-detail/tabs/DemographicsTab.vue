<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="3">
    <v-card-title class="text-h5">
      {{ $t('Demographic Information:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="3"
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
            <v-icon
              v-else
              medium
              class="mr-3"
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-radio-group>
      </v-col>
    </v-row>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-text-field
          dense
          filled
          :label="$t('Height feet')"
          :rules="[v => !!v || $t('Height feet is required')]"
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
            <v-icon
              v-else
              medium
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-text-field
          dense
          filled
          :label="$t('Height inches')"
          :rules="[v => !!v || $t('Height inches is required')]"
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
            <v-icon
              v-else
              medium
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-text-field>
      </v-col>

      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-text-field
          dense
          filled
          :label="$t('Weight')"
          :rules="[v => !!v || $t('Weight is required')]"
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
            <v-icon
              v-else
              medium
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-text-field>
      </v-col>

      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-select
          :items="hairColors"
          :label="$t('Hair Color')"
          :rules="[v => !!v || $t(' Hair color is required')]"
          autocomplete="nope"
          dense
          filled
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
            <v-icon
              v-else
              medium
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-select>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-select
          :items="eyeColors"
          :label="$t('Eye Color')"
          :rules="[v => !!v || $t('Eye color is required')]"
          autocomplete="nope"
          dense
          filled
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
            <v-icon
              v-else
              medium
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-select>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
        class="pl-8"
      >
        <v-textarea
          v-model="
            permitStore.getPermitDetail.application.physicalAppearance
              .physicalDesc
          "
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
