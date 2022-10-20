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
            :rules="[
              v => !!v || $t('Primary phone number cannot be blank'),
              v => !!v.match(/^[0-9]+$/) || $t('Must only contain numbers'),
            ]"
            v-model="
              completeApplicationStore.completeApplication.contact
                .primaryPhoneNumber
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
            :label="$t('Cell phone number')"
            :rules="[
              v => !!v.match(/^[0-9]+$/) || $t('Must only contain numbers'),
            ]"
            v-model="
              completeApplicationStore.completeApplication.contact
                .cellPhoneNumber
            "
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Work phone number')"
            :rules="[
              v => !!v.match(/^[0-9]+$/) || $t('Must only contain numbers'),
            ]"
            v-model="
              completeApplicationStore.completeApplication.contact
                .workPhoneNumber
            "
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Fax number')"
            :rules="[
              v => !!v.match(/^[0-9]+$/) || $t('Must only contain numbers'),
            ]"
            v-model="
              completeApplicationStore.completeApplication.contact
                .faxPhoneNumber
            "
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
                completeApplicationStore.completeApplication.contact.textMessageUpdates =
                  v;
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
import { reactive } from 'vue';
import { formatPhoneNumber } from '@shared-utils/formatters/defaultFormatters';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import CheckboxInput from '@shared-ui/components/inputs/CheckboxInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';

interface FormStepFiveProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepFiveProps>(), {
  handleNextSection: () => null,
});

const state = reactive({
  valid: false,
});

const completeApplicationStore = useCompleteApplicationStore();

function handleSubmit() {
  formatInputs();
  props.handleNextSection();
}
function handleSave() {
  formatInputs();
  completeApplicationStore.postCompleteApplicationFromApi;
}

function formatInputs() {
  completeApplicationStore.completeApplication.contact.primaryPhoneNumber =
    formatPhoneNumber(
      completeApplicationStore.completeApplication.contact.primaryPhoneNumber
    );

  completeApplicationStore.completeApplication.contact.cellPhoneNumber =
    formatPhoneNumber(
      completeApplicationStore.completeApplication.contact.cellPhoneNumber
    );

  completeApplicationStore.completeApplication.contact.faxPhoneNumber =
    formatPhoneNumber(
      completeApplicationStore.completeApplication.contact.faxPhoneNumber
    );

  completeApplicationStore.completeApplication.contact.workPhoneNumber =
    formatPhoneNumber(
      completeApplicationStore.completeApplication.contact.workPhoneNumber
    );
}
</script>
