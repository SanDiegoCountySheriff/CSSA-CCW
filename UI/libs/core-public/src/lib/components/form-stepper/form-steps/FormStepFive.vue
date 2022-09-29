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
          <TextInput
            :label="' Primary phone number'"
            :target="'primaryPhoneNumber'"
            :rules="[v => !!v || $t('Primary phone number cannot be blank')]"
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
            :label="' Cell phone number'"
            :target="'cellPhoneNumber'"
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
            :label="' Work phone number'"
            :target="'workPhoneNumber'"
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
            :label="' Fax phone number'"
            :target="'faxPhoneNumber'"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
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
              (v, t) => {
                handleInput(v, t);
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
import { getCurrentInstance, reactive } from 'vue';
import { ContactInfoType } from '@shared-ui/types/defaultTypes';
import { useActions } from 'vuex-composition-helpers';
import TextInput from '@shared-ui/components/inputs/TextInput.vue';
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

const { addContactInfo } = useActions(['addContactInfo']);
const instance = getCurrentInstance();

function handleInput(value, target) {
  switch (target) {
    case 'primaryPhoneNumber':
      state.contact.primaryPhoneNumber = value;
      break;
    case 'cellPhoneNumber':
      state.contact.cellPhoneNumber = value;
      break;
    case 'workPhoneNumber':
      state.contact.workPhoneNumber = value;
      break;
    case 'faxPhoneNumber':
      state.contact.faxPhoneNumber = value;
      break;
    case 'textMessageUpdates':
      state.contact.textMessageUpdates = value;
      instance?.proxy?.$forceUpdate();
      break;
    default:
      return;
  }
}
function handleSubmit() {
  addContactInfo(state.contact);
  props.handleNextSection();
}
</script>
