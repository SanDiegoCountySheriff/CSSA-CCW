<template>
  <div>
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Contact information') }}
      </v-subheader>
      <v-row class="ml-5">
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Primary phone number')"
            :rules="phoneRuleSet"
            v-model="completeApplication.contact.primaryPhoneNumber"
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
            :label="$t('Cell phone number')"
            :hint="$t('Only numbers no spaces or dashes')"
            v-model="completeApplication.contact.cellPhoneNumber"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Work phone number')"
            :hint="$t('Only numbers no spaces or dashes')"
            v-model="completeApplication.contact.workPhoneNumber"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Fax number')"
            :hint="$t('Only numbers no spaces or dashes')"
            v-model="completeApplication.contact.faxPhoneNumber"
          />
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <CheckboxInput
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
      @submit="handleSubmit"
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import CheckboxInput from '@shared-ui/components/inputs/CheckboxInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { formatPhoneNumber } from '@shared-utils/formatters/defaultFormatters';
import { i18n } from '@shared-ui/plugins';
import { phoneRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';

const router = useRouter();

const state = reactive({
  valid: false,
});

const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Step five complete');
  },
  onSuccess: () => {
    router.push('/form-2');
  },
  onError: error => {
    alert(error);
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Save and quit');
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    alert(i18n.t('Save unsuccessful, please try again'));
  },
});

async function handleSubmit() {
  formatInputs();
  updateMutation.mutate();
}

async function handleSave() {
  formatInputs();
  saveMutation.mutate();
}

function formatInputs() {
  completeApplication.contact.primaryPhoneNumber = formatPhoneNumber(
    completeApplication.contact.primaryPhoneNumber
  );

  completeApplication.contact.cellPhoneNumber = formatPhoneNumber(
    completeApplication.contact.cellPhoneNumber
  );

  completeApplication.contact.faxPhoneNumber = formatPhoneNumber(
    completeApplication.contact.faxPhoneNumber
  );

  completeApplication.contact.workPhoneNumber = formatPhoneNumber(
    completeApplication.contact.workPhoneNumber
  );
}
</script>
