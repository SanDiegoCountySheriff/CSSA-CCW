<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Id Information') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <TextInput
            :label="'Id number'"
            :target="'idNumber'"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
            "
            :rules="[v => !!v || 'Id  number is required']"
            required
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <TextInput
            :label="' Issuing State'"
            :target="'issuingState'"
            :rules="[v => !!v || 'Issuing state is required']"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
            "
          />
        </v-col>
      </v-row>

      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Date of birth') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-date-picker
            v-model="DOBInfo.DOB"
            label="Date of birth"
          />
          <v-alert
            dense
            outlined
            type="error"
            v-if="!DOBInfo.DOB"
          >
            {{ $t('Date of birth cannot be blank!') }}
          </v-alert>
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <TextInput
            :label="'Current Age'"
            :target="'currentAge'"
            :rules="[v => !!v || 'Current age is required']"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
            "
          />
          <TextInput
            :label="'Birth city'"
            :target="'birthCity'"
            :rules="[v => !!v || 'Birth city cannot be blank']"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
            "
          />
          <TextInput
            :label="'Birth state'"
            :target="'birthState'"
            :rules="[v => !!v || 'Birth state cannot be blank']"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
            "
          />
          <TextInput
            :label="'Birth country'"
            :target="'birthCountry'"
            :rules="[v => !!v || 'Birth country cannot be blank']"
            @input="
              (v, t) => {
                handleInput(v, t);
              }
            "
          />
        </v-col>
      </v-row>

      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Citizenship Information') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
          class="pl-5"
        >
          <RadioGroupInput
            :label="'Citizen'"
            :layout="'row'"
            :options="[
              { label: 'Yes', value: true },
              { label: 'No', value: false },
            ]"
            :target="'citizen'"
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
            v-if="!citizenshipInfo.citizen"
          >
            {{ $t('Must select Yes or No!') }}
          </v-alert>
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-select
            v-model="citizenshipInfo.militaryStatus"
            :items="items"
            :label="$t('Military Status')"
          />

          <v-alert
            dense
            outlined
            type="error"
            v-if="!citizenshipInfo.militaryStatus"
          >
            {{ $t('Must select a status') }}
          </v-alert>
        </v-col>
      </v-row>
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="valid"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { getCurrentInstance, reactive, ref } from 'vue';
import { useIdStore } from '@shared-ui/stores/id';
import { useDOBStore } from '@shared-ui/stores/DOB';
import { useCitizenshipStore } from '@shared-ui/stores/citizenship';
import TextInput from '@shared-ui/components/inputs/TextInput.vue';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { CitizenshipType, DOBType, IdType } from '@shared-ui/types/defaultTypes';

interface FormStepOneProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepOneProps>(), {
  handleNextSection: () => null,
});

const id = reactive({} as IdType);
const DOBInfo = reactive({} as DOBType);
const citizenshipInfo = reactive({} as CitizenshipType);
const items = ref(['Active', 'Reserve', 'Discharged', 'Retired', 'N/A']);
const valid = ref(false);

const idStore = useIdStore();
const DOBStore = useDOBStore();
const citizenshipStore = useCitizenshipStore();

const instance = getCurrentInstance();

function handleInput(value, target) {
  switch (target) {
    case 'idNumber':
      id.idNumber = value;
      break;
    case 'issuingState':
      id.issuingState = value;
      break;
    case 'currentAge':
      DOBInfo.currentAge = value;
      break;
    case 'birthCity':
      DOBInfo.birthCity = value;
      break;
    case 'birthState':
      DOBInfo.birthState = value;
      break;
    case 'birthCountry':
      DOBInfo.birthCountry = value;
      break;
    case 'citizen':
      citizenshipInfo.citizen = value;
      instance?.proxy?.$forceUpdate();
      break;
    default:
      return;
  }
}

function handleSubmit() {
  idStore.setId(id);
  DOBStore.setDOBInfo(DOBInfo);
  citizenshipStore.setCitizenshipInfo(citizenshipInfo);
  props.handleNextSection();
}
</script>
