<template>
  <div class="text-left">
    <v-container fluid>
      <v-stepper
        vertical
        :non-linear="props.admin"
        v-model="stepIndex.step"
      >
        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :editable="props.admin"
          :complete="stepIndex.step > 1"
          step="1"
        >
          {{ $t('Personal') }}
        </v-stepper-step>
        <v-stepper-content step="1">
          <PersonalInfoStep
            v-if="stepIndex.step === 1"
            :handle-next-section="handleNextSection"
          />
        </v-stepper-content>

        <v-stepper-step
          :editable="props.admin"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="2"
          :complete="stepIndex.step > 2"
        >
          {{ $t('Citizenship') }}
        </v-stepper-step>
        <v-stepper-content step="2">
          <IdBirthInfoStep
            v-if="stepIndex.step === 2"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="3"
          :complete="stepIndex.step > 3"
        >
          {{ $t('Address') }}
        </v-stepper-step>
        <v-stepper-content step="3">
          <AddressInfoStep
            v-if="stepIndex.step === 3"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="4"
          :complete="stepIndex.step > 4"
        >
          {{ $t('Appearance') }}
        </v-stepper-step>
        <v-stepper-content step="4">
          <PhysicalAppearanceStep
            v-if="stepIndex.step === 4"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="5"
          :complete="stepIndex.step > 5"
        >
          {{ $t('Contact') }}
        </v-stepper-step>
        <v-stepper-content step="5">
          <ContactStep
            v-if="stepIndex.step === 5"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>
        <v-stepper-step
          step="6"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex.step > 6"
        >
          {{ $t(' Employment & Weapons') }}
        </v-stepper-step>
        <v-stepper-content step="6">
          <WorkInfoStep
            v-if="stepIndex.step === 6"
            :routes="routes"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          step="7"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex.step > 7"
        >
          {{ $t('Application Type') }}
        </v-stepper-step>
        <v-stepper-content step="7">
          <ApplicationTypeStep
            v-if="stepIndex.step === 7"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          step="8"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex.step > 8"
        >
          {{ $t(' Upload Files') }}
        </v-stepper-step>
        <v-stepper-content step="8">
          <FileUploadStep
            v-if="stepIndex.step === 8"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          step="9"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex.step > 9"
        >
          {{ $t('Signature') }}
        </v-stepper-step>
        <v-stepper-content step="9">
          <SignatureStep
            v-if="stepIndex.step === 9"
            :routes="routes"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>
      </v-stepper>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import AddressInfoStep from '@shared-ui/components/form-stepper/form-steps/AddressInfoStep.vue';
import ApplicationTypeStep from '@shared-ui/components/form-stepper/form-steps/ApplicationTypeStep.vue';
import ContactStep from '@shared-ui/components/form-stepper/form-steps/ContactStep.vue';
import FileUploadStep from '@shared-ui/components/form-stepper/form-steps/FileUploadStep.vue';
import IdBirthInfoStep from '@shared-ui/components/form-stepper/form-steps/IdBirthInfoStep.vue';
import PersonalInfoStep from '@shared-ui/components/form-stepper/form-steps/PersonalInfoStep.vue';
import PhysicalAppearanceStep from '@shared-ui/components/form-stepper/form-steps/PhysicalAppearanceStep.vue';
import SignatureStep from '@shared-ui/components/form-stepper/form-steps/SignatureStep.vue';
import WorkInfoStep from '@shared-ui/components/form-stepper/form-steps/WorkInfoStep.vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useRouter } from 'vue-router/composables';
import { onMounted, reactive } from 'vue';

interface IWrapperProps {
  admin: boolean;
  routes: any;
}

const props = defineProps<IWrapperProps>();

const applicationStore = useCompleteApplicationStore();
const router = useRouter();

const stepIndex = reactive({
  step: 1,
  previousStep: 0,
});

onMounted(() => {
  stepIndex.step = applicationStore.completeApplication.application.currentStep;

  if (stepIndex.step > 9) {
    router.push(props.routes.QUALIFYING_QUESTIONS_ROUTE_PATH);
  }
});

function handleNextSection() {
  stepIndex.previousStep = stepIndex.step;
  stepIndex.step += 1;
}

function handlePreviousSection() {
  stepIndex.previousStep = stepIndex.step - 2;
  stepIndex.step -= 1;
}
</script>

<style lang="scss" scoped>
.form-card {
  height: auto;
  min-height: 45vh;
}
</style>
