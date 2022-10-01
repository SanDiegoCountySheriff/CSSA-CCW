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
          <TextInput
            :label="' Height feet'"
            :target="'heightFeet'"
            :rules="[v => !!v || 'Height feet is required']"
            :type="'number'"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
            "
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <TextInput
            :label="' Height inches'"
            :target="'heightInch'"
            :rules="[v => !!v || 'Height inches is required']"
            :type="'number'"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
            "
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <TextInput
            :label="'Weight'"
            :target="'weight'"
            :rules="[v => !!v || 'Weight is required']"
            :type="'number'"
            @input="
              (v, t) => {
                handleInput(v, t);
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
            :items="hairColors"
            :label="$t('Hair Color')"
            :rules="[v => !!v || $t(' Hair color is require')]"
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
            :target="'gender'"
            :layout="'row'"
            :options="[
              { label: 'Male', value: 'male' },
              { label: 'Female', value: 'female' },
            ]"
            @input="
              (v, t) => {
                handleInput(v, t);
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
import { getCurrentInstance, reactive } from 'vue';
import { usePhysicalAppearanceStore } from '@shared-ui/stores/physicalAppearance';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { AppearanceInfoType } from '@shared-ui/types/defaultTypes';
import { eyeColors, hairColors } from '@shared-utils/lists/defaultList';
import TextInput from '@shared-ui/components/inputs/TextInput.vue';

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
const instance = getCurrentInstance();

function handleInput(value, target) {
  switch (target) {
    case 'gender':
      state.appearance.gender = value;
      instance?.proxy?.$forceUpdate();
      break;
    case 'heightFeet':
      state.appearance.heightFeet = value;
      break;
    case 'heightInch':
      state.appearance.heightInch = value;
      break;
    case 'weight':
      state.appearance.weight = value;
      break;
    case 'hairColor':
      state.appearance.hairColor = value;
      break;
    case 'eyeColor':
      state.appearance.eyeColor = value;
      break;
    case 'physicalDesc':
      state.appearance.physicalDesc = value;
      break;
    default:
      return;
  }
}

function handleSubmit() {
  physicalAppearanceStore.setPhysicalAppearance(state.appearance);
  props.handleNextSection();
}
</script>

<style lang="scss" scoped></style>
