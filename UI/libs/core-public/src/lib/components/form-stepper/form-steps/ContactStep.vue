<template>
  <div>
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Contact Information') }}
      </v-subheader>
      <v-row class="ml-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            dense
            maxlength="10"
            counter
            outlined
            :hint="$t('Only numbers no spaces or dashes')"
            :label="$t('Primary phone number')"
            :rules="phoneRuleSet"
            v-model="completeApplication.contact.primaryPhoneNumber"
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
            :label="$t('Cell phone number')"
            maxlength="10"
            counter
            :rules="notRequiredPhoneRuleSet"
            :hint="$t('Only numbers no spaces or dashes')"
            v-model="completeApplication.contact.cellPhoneNumber"
          />
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            dense
            outlined
            maxlength="10"
            counter
            class="pl-6"
            :label="$t('Work phone number')"
            :rules="notRequiredPhoneRuleSet"
            :hint="$t('Only numbers no spaces or dashes')"
            v-model="completeApplication.contact.workPhoneNumber"
          />
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            dense
            outlined
            maxlength="10"
            counter
            :label="$t('Fax number')"
            :rules="notRequiredPhoneRuleSet"
            :hint="$t('Only numbers no spaces or dashes')"
            v-model="completeApplication.contact.faxPhoneNumber"
          />
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <CheckboxInput
            v-if="!hidden"
            class="pl-6"
            :label="'Text message updates'"
            :target="'textMessageUpdates'"
            @input="
              v => {
                completeApplication.contact.textMessageUpdates = v;
              }
            "
          />
        </v-col>
      </v-row>
    </v-form>
    <FormButtonContainer
      :valid="state.valid"
      :submitting="state.submited"
      @submit="handleSubmit"
      @save="handleSave"
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
  </div>
</template>

<script setup lang="ts">
import CheckboxInput from '@shared-ui/components/inputs/CheckboxInput.vue';
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import {
  notRequiredPhoneRuleSet,
  phoneRuleSet,
} from '@shared-ui/rule-sets/ruleSets';

interface IProps {
  handleNextSection: CallableFunction;
  handlePreviousSection: CallableFunction;
}

const props = defineProps<IProps>();

const router = useRouter();

const state = reactive({
  valid: false,
  snackbar: false,
  submited: false,
});

// Remove this to implement the text updateds checkbox
const hidden = true;

const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    completeApplication.currentStep = 6;
    props.handleNextSection();
  },
  onError: () => {
    state.submited = false;
    state.valid = true;
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

async function handleSubmit() {
  state.valid = false;
  state.submited = true;
  updateMutation.mutate();
}

async function handleSave() {
  saveMutation.mutate();
}
</script>
