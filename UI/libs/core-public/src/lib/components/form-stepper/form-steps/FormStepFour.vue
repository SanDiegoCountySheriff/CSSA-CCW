<template>
  <div>
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Physical Appearance') }}
      </v-subheader>
      <v-row class="ml-5">
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Height feet')"
            :rules="[v => !!v || $t('Height feet is required')]"
            v-model="
              completeApplicationStore.completeApplication.physicalAppearance
                .heightFeet
            "
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

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Height inches')"
            :rules="[v => !!v || $t('Height inches is required')]"
            v-model="
              completeApplicationStore.completeApplication.physicalAppearance
                .heightInch
            "
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

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Weight')"
            :rules="[v => !!v || $t('Weight is required')]"
            v-model="
              completeApplicationStore.completeApplication.physicalAppearance
                .weight
            "
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
                completeApplicationStore.completeApplication.physicalAppearance.hairColor =
                  v.toLowerCase();
              }
            "
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-asterisk
              </v-icon>
            </template>
          </v-select>
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
                completeApplicationStore.completeApplication.physicalAppearance.eyeColor =
                  v.toLowerCase();
              }
            "
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-asterisk
              </v-icon>
            </template>
          </v-select>
        </v-col>
      </v-row>

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Gender ') }}
      </v-subheader>
      <v-row class="ml-5">
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
                completeApplicationStore.completeApplication.physicalAppearance.gender =
                  v;
              }
            "
          />
          <v-alert
            dense
            outlined
            type="error"
            v-if="
              !completeApplicationStore.completeApplication.physicalAppearance
                .gender
            "
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
            v-model="
              completeApplicationStore.completeApplication.physicalAppearance
                .physicalDesc
            "
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
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { eyeColors, hairColors } from '@shared-utils/lists/defaultConstants';

interface FormStepFourProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepFourProps>(), {
  handleNextSection: () => null,
});

const state = reactive({
  valid: false,
});

const completeApplicationStore = useCompleteApplicationStore();

function handleSubmit() {
  props.handleNextSection();
}

function handleSave() {}
</script>
