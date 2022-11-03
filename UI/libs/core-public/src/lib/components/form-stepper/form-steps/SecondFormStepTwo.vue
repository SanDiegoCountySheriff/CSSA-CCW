<template>
  <div>
    <DocumentInfoSection />
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('File Upload') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            accept=".png, .jpeg"
            :label="$t('Driver License')"
            @change="handleFileUpload($event, 'driver-license')"
          />
        </v-col>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            accept=".pdf, .doc, .dox"
            :label="$t('Proof of Residence 1')"
            @change="handleFileUpload"
          />
        </v-col>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            accept=".pdf, .doc, .dox"
            :label="$t('Proof of Residence 2')"
            @change="handleFileUpload"
          />
        </v-col>
      </v-row>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Military') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            accept=".pdf, .doc, .dox"
            :label="$t('Military Document')"
            @change="handleFileUpload"
          />
        </v-col>
      </v-row>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Not born in US') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            accept=".pdf, .doc, .dox"
            :label="$t('CitizenShip Documents')"
            @change="handleFileUpload"
          />
        </v-col>
      </v-row>

      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Supporting Documents') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            multiple
            :label="$t('Supporting Documents')"
            @change="handleFileUpload"
          />
        </v-col>
      </v-row>
      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Legal name change') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            accept=".pdf, .doc, .dox"
            :label="$t('Name change documents')"
            @change="handleFileUpload"
          />
        </v-col>
      </v-row>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' License Type Documents') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            accept=".pdf, .doc, .dox"
            :label="$t('Judicial documents')"
            @change="handleFileUpload"
          />
        </v-col>
        <v-col
          cols="4"
          lg="4"
          sm="1"
        >
          <v-file-input
            show-size
            small-chips
            accept=".pdf, .doc, .dox"
            :label="$t('Reserve documents')"
            @change="handleFileUpload"
          />
        </v-col>
      </v-row>
      <v-divider />
    </v-form>
    <FormButtonContainer
      :valid="state.valid"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import DocumentInfoSection from '@shared-ui/components/info-sections/DocumentInfoSection.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';

const applicationStore = useCompleteApplicationStore();
const completeApplication = applicationStore.completeApplication.application;

interface ISecondFormStepTwoProps {
  handleNextSection: CallableFunction;
}

const props = defineProps<ISecondFormStepTwoProps>();

const state = reactive({
  files: [] as Array<File>,
  valid: false,
});

function handleFileUpload(event, target: string) {
  // need to add the application id to this.
  const newFileName = `${applicationStore.completeApplication.id}-${completeApplication.personalInfo.lastName}-${completeApplication.personalInfo.firstName}_${target}`;
  const newFile = new File([event], newFileName, event.type);

  state.files.push(newFile);
}

function handleSubmit() {
  // TODO: Create another function to loop throught the files and call the api call
  // Function will also need to set a loading state till the call is finished.

  props.handleNextSection();
}
</script>

<style lang="scss" scoped>
.text-line {
  display: flex;
  flex-direction: row;
}
</style>
