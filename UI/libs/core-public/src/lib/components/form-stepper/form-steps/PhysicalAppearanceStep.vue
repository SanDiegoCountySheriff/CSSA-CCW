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
          cols="12"
          lg="6"
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
            v-model="completeApplication.physicalAppearance.heightFeet"
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-text-field>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            dense
            outlined
            type="number"
            :label="$t('Height inches')"
            :rules="[
              v => !!v || $t('Height inches is required'),
              v => v < 12 || $t('Height inches must be less than 11'),
            ]"
            v-model="completeApplication.physicalAppearance.heightInch"
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-text-field>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            dense
            outlined
            persistent-hint
            type="number"
            :hint="$t('Enter weight in pounds(lbs)')"
            :label="$t('Weight')"
            :rules="[
              v => !!v || $t('Weight is required'),
              v =>
                (v > 0 && v < 2000) ||
                $t('Weight must greater than 0 and less than 2000'),
            ]"
            v-model="completeApplication.physicalAppearance.weight"
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-text-field>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-select
            dense
            outlined
            v-model="completeApplication.physicalAppearance.hairColor"
            :value="completeApplication.physicalAppearance.hairColor"
            :items="hairColors"
            :label="$t('Hair Color')"
            :rules="[v => !!v || $t(' Hair color is required')]"
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-select>
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-select
            dense
            outlined
            v-model="completeApplication.physicalAppearance.eyeColor"
            :value="completeApplication.physicalAppearance.eyeColor"
            :items="eyeColors"
            :label="$t('Eye Color')"
            :rules="[v => !!v || $t('Eye color is required')]"
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
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
          cols="12"
          lg="6"
        >
          <v-radio-group
            :label="'Gender'"
            v-model="completeApplication.physicalAppearance.gender"
            row
          >
            <v-radio
              :label="'Male'"
              :value="'male'"
            />
            <v-radio
              :label="'Female'"
              :value="'female'"
            />
          </v-radio-group>
          <v-alert
            dense
            outlined
            type="error"
            v-if="!completeApplication.physicalAppearance.gender"
          >
            {{ $t('Must select a gender') }}
          </v-alert>
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-textarea
            dense
            outlined
            counter
            maxlength="1000"
            v-model="completeApplication.physicalAppearance.physicalDesc"
            :label="$t('Physical Description')"
            :rules="[
              v =>
                !v ||
                (v && v.length <= 1000) ||
                $t('Maximum 1000 characters are allowed'),
            ]"
            clearable
          />
        </v-col>
      </v-row>
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="state.valid"
      @submit="handleSubmit"
      @save="saveMutation.mutate"
      @back="handlePreviousSection"
      @cancel="router.push('/')"
    />
    <v-snackbar
      :value="state.snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
    <v-snackbar
      :value="state.formError"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('You must select a gender') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import { eyeColors, hairColors } from '@shared-utils/lists/defaultConstants';

interface FormStepFourProps {
  handleNextSection: () => void;
  handlePreviousSection: CallableFunction;
}

const props = withDefaults(defineProps<FormStepFourProps>(), {
  handleNextSection: () => null,
});

const state = reactive({
  valid: false,
  snackbar: false,
  formError: false,
});
const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;
const router = useRouter();

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    completeApplication.currentStep = 5;
    props.handleNextSection();
  },
  onError: () => {
    state.snackbar = true;
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    state.snackbar = true;
  },
});

function handleSubmit() {
  if (!completeApplication.physicalAppearance.gender) {
    state.formError = true;
  } else {
    updateMutation.mutate();
  }
}
</script>
