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
            outlined
            dense
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
            outlined
            dense
            :label="$t('Cell phone number')"
            :hint="$t('Only numbers no spaces or dashes')"
            v-model="completeApplication.contact.cellPhoneNumber"
          />
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            class="pl-6"
            :label="$t('Work phone number')"
            :hint="$t('Only numbers no spaces or dashes')"
            v-model="completeApplication.contact.workPhoneNumber"
          />
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            :label="$t('Fax number')"
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
            :label="'Text message updates'"
            :target="'textMessageUpdates'"
            class="pl-6"
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
import { phoneRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useRouter } from 'vue-router/composables';
import { useMutation } from '@tanstack/vue-query';

const router = useRouter();

const state = reactive({
  valid: false,
  snackbar: false,
});

interface IProps {
  routes: any;
  handleNextSection: CallableFunction;
  handlePreviousSection: CallableFunction;
}
const props = defineProps<IProps>();

const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Step five complete');
  },
  onSuccess: () => {
    completeApplication.currentStep = 6;
    props.handleNextSection();
  },
  onError: () => {
    state.snackbar = true;
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Save and quit');
  },
  onSuccess: () => {
    router.push(props.routes.HOME_ROUTE_PATH);
  },
  onError: () => {
    state.snackbar = true;
  },
});

function handleSubmit() {
  updateMutation.mutate();
}

function handleSave() {
  saveMutation.mutate();
}
</script>
