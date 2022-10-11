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
          <v-text-field
            v-model="id.idNumber"
            :label="$t('Id number')"
            :rules="[v => !!v || $t('Id  number is required')]"
            required
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t(' Issuing State')"
            :rules="[v => !!v || $t('Issuing state is required')]"
            v-model="id.issuingState"
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
          <v-text-field
            :label="$t('Current Age')"
            :rules="[v => !!v || $t('Current age is required')]"
            v-model="DOBInfo.currentAge"
          />
          <v-text-field
            :label="$t('Birth city')"
            :rules="[v => !!v || $t('Birth city cannot be blank')]"
            v-model="DOBInfo.birthCity"
          />
          <v-text-field
            :label="$t('Birth state')"
            :rules="[v => !!v || $t('Birth state cannot be blank')]"
            v-model="DOBInfo.birthState"
          />
          <v-text-field
            :label="$t('Birth country')"
            :rules="[v => !!v || $t('Birth country cannot be blank')]"
            v-model="DOBInfo.birthCountry"
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
            @input="
              v => {
                citizenshipInfo.citizen = v;
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
import { reactive, ref } from 'vue';
import { useIdStore } from '@shared-ui/stores/id';
import { useDOBStore } from '@shared-ui/stores/DOB';
import { useCitizenshipStore } from '@shared-ui/stores/citizenship';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import {
  CitizenshipType,
  DOBType,
  IdType,
} from '@shared-utils/types/defaultTypes';

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

function handleSubmit() {
  idStore.setId(id);
  DOBStore.setDOBInfo(DOBInfo);
  citizenshipStore.setCitizenshipInfo(citizenshipInfo);
  props.handleNextSection();
}
</script>
