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
      <div class="text-left text-line">
        <h4>{{ $t('NOTE: ') }}</h4>
        <p class="ml-3">
          {{ $t('Rename') }}
        </p>
      </div>
      <v-file-input
        show-size
        small-chips
        multiple
        v-model="state.files"
        @change="handleFileUpload"
      />
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="state.valid"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import DocumentInfoSection from '@shared-ui/components/info-sections/DocumentInfoSection.vue';
import { reactive } from 'vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';

interface ISecondFormStepTwoProps {
  handleNextSection: CallableFunction;
}

const props = defineProps<ISecondFormStepTwoProps>();

const state = reactive({
  files: [],
  valid: false,
});
function handleFileUpload() {
  if (state.files) {
    state.valid = true;
  }
}

function handleSubmit() {
  console.log(state.files);
  props.handleNextSection();
}
</script>

<style lang="scss" scoped>
.text-line {
  display: flex;
  flex-direction: row;
}
</style>
