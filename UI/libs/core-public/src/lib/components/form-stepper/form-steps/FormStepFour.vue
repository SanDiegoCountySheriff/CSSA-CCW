<template>
  <div>
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Physical Appearance') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Height feet')"
            :rules="[v => !!v || 'Height feet is required']"
            v-model="state.appearance.heightFeet"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Height inches')"
            :rules="[v => !!v || 'Height incheds is required']"
            v-model="state.appearance.heightInch"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Weight')"
            :rules="[v => !!v || 'Weight is required']"
            v-model="state.appearance.weight"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-select
            :items="hairColors"
            :label="$t('Hair Color')"
            :rules="[v => !!v || $t(' Hair color is required')]"
            @change="
              v => {
                state.appearance.hairColor = v.toLowerCase();
              }
            "
          />
        </v-col>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-select
            :items="eyeColors"
            :label="$t('Eye Color')"
            :rules="[v => !!v || $t('Eye color is required')]"
            @change="
              v => {
                state.appearance.eyeColor = v.toLowerCase();
              }
            "
          />
        </v-col>
      </v-row>

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Gender ') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <RadioGroupInput
            :label="'Gender'"
            :layout="'row'"
            :options="[
              { label: 'Male', value: 'male' },
              { label: 'Female', value: 'female' },
            ]"
            @input="
              v => {
                state.appearance.gender = v;
              }
            "
          />
          <v-alert
            dense
            outlined
            type="error"
            v-if="!state.appearance.gender"
          >
            {{ $t('Must select a gender') }}
          </v-alert>
        </v-col>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-textarea
            v-model="state.appearance.physicalDesc"
            :label="$t('Physical Description')"
            clearable
          />
        </v-col>
      </v-row>
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="state.valid"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { usePhysicalAppearanceStore } from '@shared-ui/stores/physicalAppearance';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { AppearanceInfoType } from '@shared-utils/types/defaultTypes';
import { eyeColors, hairColors } from '@shared-utils/lists/defaultList';

interface FormStepFourProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepFourProps>(), {
  handleNextSection: () => null,
});

const state = reactive({
  appearance: {} as AppearanceInfoType,
  valid: false,
});

const physicalAppearanceStore = usePhysicalAppearanceStore();

function handleSubmit() {
  physicalAppearanceStore.setPhysicalAppearance(state.appearance);
  props.handleNextSection();
}
</script>
