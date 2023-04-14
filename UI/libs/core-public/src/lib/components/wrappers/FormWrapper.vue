<template>
  <div>
    <v-container v-if="state.isLoading && !state.isError">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
        divider, list-item-three-line,
        card-heading, image, image, image,
        image, actions"
      >
      </v-skeleton-loader>
    </v-container>
    <v-container
      v-else-if="!$vuetify.breakpoint.xsOnly"
      fluid
    >
      <v-stepper
        v-model="stepIndex.step"
        non-linear
        class="stepper"
        @change="updateMutation"
      >
        <v-stepper-header
          :class="$vuetify.theme.dark ? 'sticky-dark' : 'sticky-light'"
        >
          <v-stepper-step
            editable
            :complete="stepOneValid"
            :edit-icon="stepOneValid ? 'mdi-check' : '$edit'"
            :color="stepOneValid ? 'success' : 'primary'"
            :step="1"
          >
            {{ $t('Personal') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="2"
          >
            {{ $t('Citizenship') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="3"
          >
            {{ $t('Address') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="4"
          >
            {{ $t('Appearance') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="5"
          >
            {{ $t('Contact') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="6"
          >
            {{ $t(' Employment & Weapons') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="7"
          >
            {{ $t('Application Type') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="8"
          >
            {{ $t(' Upload Files') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="9"
          >
            {{ $t('Qualifying Questions') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="10"
          >
            {{ $t('Signature') }}
          </v-stepper-step>
          <v-progress-linear
            :active="isLoading || isSaveLoading"
            indeterminate
          ></v-progress-linear>
        </v-stepper-header>

        <v-stepper-items>
          <v-stepper-content :step="1">
            <PersonalInfoStep
              v-model="applicationStore.completeApplication"
              :step-one-valid="stepOneValid"
              @update-step-one-valid="handleUpdateStepOneValid"
              @handle-save="handleSave"
              @handle-submit="handleSubmit"
            />
          </v-stepper-content>
          <v-stepper-content :step="2">
            <IdBirthInfoStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="3">
            <AddressInfoStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="4">
            <PhysicalAppearanceStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="5">
            <ContactStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="6">
            <WorkInfoStep
              :routes="routes"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="7">
            <ApplicationTypeStep
              v-if="stepIndex.step === 7"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="8">
            <FileUploadStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="9">
            <QualifyingQuestionsStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="10">
            <SignatureStep
              :routes="routes"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-container>

    <v-container
      v-else
      class="pa-0"
    >
      <v-progress-circular
        v-if="isLoading || isSaveLoading"
        indeterminate
        absolute
        class="progress-circular"
      ></v-progress-circular>
      <v-expansion-panels
        v-model="expansionStep"
        accordion
        @change="updateMutation"
      >
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Personal') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <PersonalInfoStep
              v-model="applicationStore.completeApplication"
              :step-one-valid="stepOneValid"
              @update-step-one-valid="handleUpdateStepOneValid"
              @handle-save="handleSave"
              @handle-submit="handleSubmit"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Citizenship') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <IdBirthInfoStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Address') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <AddressInfoStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Appearance') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <PhysicalAppearanceStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Contact') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <ContactStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t(' Employment & Weapons') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <WorkInfoStep
              :routes="routes"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Application Type') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <ApplicationTypeStep
              v-if="stepIndex.step === 7"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t(' Upload Files') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <FileUploadStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Qualifying Questions') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <QualifyingQuestionsStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Signature') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <SignatureStep
              :routes="routes"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-container>
    <!-- <v-snackbar
      :value="snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar> -->
  </div>
</template>

<script setup lang="ts">
import AddressInfoStep from '@core-public/components/form-stepper/form-steps/AddressInfoStep.vue';
import ApplicationTypeStep from '@core-public/components/form-stepper/form-steps/ApplicationTypeStep.vue';
import ContactStep from '@core-public/components/form-stepper/form-steps/ContactStep.vue';
import FileUploadStep from '@core-public/components/form-stepper/form-steps/FileUploadStep.vue';
import IdBirthInfoStep from '@core-public/components/form-stepper/form-steps/IdBirthInfoStep.vue';
import PersonalInfoStep from '@core-public/components/form-stepper/form-steps/PersonalInfoStep.vue';
import PhysicalAppearanceStep from '@core-public/components/form-stepper/form-steps/PhysicalAppearanceStep.vue';
import QualifyingQuestionsStep from '@core-public/components/form-stepper/form-steps/QualifyingQuestionsStep.vue';
import SignatureStep from '@core-public/components/form-stepper/form-steps/SignatureStep.vue';
import WorkInfoStep from '@core-public/components/form-stepper/form-steps/WorkInfoStep.vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRoute } from 'vue-router/composables';
import { useRouter } from 'vue-router/composables';
import { computed, onMounted, reactive, ref } from 'vue';

interface IWrapperProps {
  admin: boolean;
  routes: unknown;
}

const props = defineProps<IWrapperProps>();

const applicationStore = useCompleteApplicationStore();
const route = useRoute();
const router = useRouter();
const stepOneValid = ref(false);

const stepIndex = reactive({
  step: 0,
  previousStep: 0,
});

const state = reactive({
  isLoading: false,
  isError: false,
});

const expansionStep = computed(() => stepIndex.step - 1);

const { isLoading, mutate: updateMutation } = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication();
  },
  // onError: () => {
  //   submited.value = false;
  //   valid.value = true;
  //   snackbar.value = true;
  // },
});

const { isLoading: isSaveLoading, mutate: saveMutation } = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push('/');
  },
  // onError: () => {
  //   snackbar.value = true;
  // },
});

onMounted(() => {
  if (!applicationStore.completeApplication.application.orderId) {
    state.isLoading = true;
    applicationStore
      .getCompleteApplicationFromApi(
        route.query.applicationId,
        route.query.isComplete
      )
      .then(res => {
        applicationStore.setCompleteApplication(res);
        state.isLoading = false;
        stepIndex.step =
          applicationStore.completeApplication.application.currentStep;
      })
      .catch(() => {
        state.isError = true;
      });
  }

  stepIndex.step = applicationStore.completeApplication.application.currentStep;
});

function handleSave() {
  saveMutation();
}

function handleSubmit() {
  applicationStore.completeApplication.application.currentStep =
    stepIndex.step + 1;
  updateMutation();
  stepIndex.previousStep = stepIndex.step;
  stepIndex.step += 1;
}

function handleNextSection() {
  stepIndex.previousStep = stepIndex.step;
  stepIndex.step += 1;
}

function handlePreviousSection() {
  stepIndex.previousStep = stepIndex.step - 2;
  stepIndex.step -= 1;
}

function handleUpdateStepOneValid(value: boolean) {
  stepOneValid.value = value;
}
</script>

<style lang="scss">
@media only screen and (max-width: 1900px) {
  .v-stepper:not(.v-stepper--vertical) .v-stepper__label {
    display: none !important;
  }
}

.theme--dark.v-expansion-panels .v-expansion-panel {
  background: #303030;
}

.v-expansion-panel-content__wrap {
  padding-left: 5px !important;
  padding-right: 5px !important;
}

.stepper {
  overflow: visible;
}

.sticky-light {
  position: sticky;
  top: 64px;
  z-index: 1;
  background-color: white;
}

.sticky-dark {
  position: sticky;
  top: 64px;
  z-index: 1;
  background-color: #303030;
}

.progress-circular {
  position: fixed;
  top: 75px;
  left: 50%;
  z-index: 2;
}

.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
