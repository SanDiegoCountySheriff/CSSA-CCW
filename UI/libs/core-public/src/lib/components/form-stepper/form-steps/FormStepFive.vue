<template>
  <div>
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Contact information') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Primary phone number')"
            :rules="[v => !!v || $t('Primary phone number cannot be blank')]"
            :v-model="state.contact.primaryPhoneNumber"
          />
        </v-col>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Cell phone number')"
            v-model="state.contact.cellPhoneNumber"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Work phone number')"
            v-model="state.contact.workPhoneNumber"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Fax number')"
            v-model="state.contact.faxPhoneNumber"
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
                state.contact.textMessageUpdates = v;
              }
            "
          />
        </v-col>
      </v-row>
    </v-form>
    <FormButtonContainer
      :valid="state.valid"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useContactStore } from '@shared-ui/stores/contact';
import { ContactInfoType } from '@shared-utils/types/defaultTypes';
import CheckboxInput from '@shared-ui/components/inputs/CheckboxInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';

interface FormStepFiveProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepFiveProps>(), {
  handleNextSection: () => null,
});

const state = reactive({
  contact: {
    textMessageUpdates: false,
  } as ContactInfoType,
  valid: false,
});

const contactInfoStore = useContactStore();

function handleSubmit() {
  contactInfoStore.setContactInfo(state.contact);
  props.handleNextSection();
}
</script>
