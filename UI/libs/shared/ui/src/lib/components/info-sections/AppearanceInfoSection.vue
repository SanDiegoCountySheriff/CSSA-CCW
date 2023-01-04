<template>
  <v-container
    fluid
    class="appearance-info-container rounded"
  >
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t(' Physical Appearance Information ') }}
    </v-banner>
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Height feet')"
          :value="props.appearanceInfo.heightFeet"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Height inches')"
          :value="props.appearanceInfo.heightInch"
        />
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Weight')"
          :value="props.appearanceInfo.weight"
        />
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Hair color')"
          :value="props.appearanceInfo.hairColor"
        />
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Eye color')"
          :value="props.appearanceInfo.eyeColor"
        />
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Gender')"
          :value="props.appearanceInfo.gender"
        />
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-textarea
          outlined
          readonly
          dense
          class="pl-6"
          :label="$t('Physical Description')"
          :value="props.appearanceInfo.physicalDesc"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { AppearanceInfoType } from '@shared-utils/types/defaultTypes';

interface IAppearanceInfoSectionProps {
  appearanceInfo: AppearanceInfoType;
  color: string;
}

const props = defineProps<IAppearanceInfoSectionProps>();
</script>

<style lang="scss" scoped>
.appearance-info-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
