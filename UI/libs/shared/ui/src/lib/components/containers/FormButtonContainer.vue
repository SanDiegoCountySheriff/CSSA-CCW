<template>
  <v-card-actions>
    <v-row>
      <v-col
        :class="props.isFinalStep ? 'text-center' : ''"
        cols="12"
        :lg="props.isFinalStep ? '12' : '6'"
      >
        <v-btn
          v-if="!props.isFirstStep"
          :loading="props.loading"
          @click="handlePreviousStep"
          color="primary"
          class="mr-3"
          outlined
        >
          <v-icon
            :color="themeStore.getThemeConfig.isDark ? 'white' : 'primary'"
            left
          >
            mdi-chevron-left
          </v-icon>
          <span :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''">
            Previous Step
          </span>
        </v-btn>

        <!-- Finalize Initial/Modification/Renewal -->
        <v-btn
          v-if="isFinalizeInitialModificationRenewal"
          :loading="props.loading"
          :disabled="!props.valid || props.loading || !props.allStepsComplete"
          @click="handleContinue"
          color="primary"
        >
          {{ getButtonText }}
        </v-btn>

        <!-- Save and Exit Matched with appointment and delivered -->
        <v-btn
          v-else-if="isSubmittedAndMatchedWithAppointmentOrDelivered"
          :loading="props.loading"
          :disabled="!props.valid || props.loading || !props.allStepsComplete"
          @click="handleSaveMatched"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>
          {{ $t('Save') }}
        </v-btn>

        <!-- Finalize Matched Submitted Without Appointment -->
        <v-btn
          v-else-if="isSubmittedAndMatchedWithoutAppointment"
          :loading="props.loading"
          :disabled="!props.valid || props.loading || !props.allStepsComplete"
          @click="handleContinue"
          color="primary"
        >
          {{ $t('Verify Information and Save') }}
        </v-btn>

        <!-- Save and Exit Update application -->
        <v-btn
          v-else-if="isUpdatingOnFinalStep"
          :loading="props.loading"
          @click="handleSave"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>
          {{ $t('Save And Exit') }}
        </v-btn>

        <!-- Next Step -->
        <v-btn
          v-else-if="!props.isFinalStep"
          :disabled="!props.valid || props.loading || !props.allStepsComplete"
          :loading="props.loading"
          @click="handleContinue"
          color="primary"
        >
          {{ $t('Next Step') }}
          <v-icon right> mdi-chevron-right </v-icon>
        </v-btn>
      </v-col>

      <v-spacer />

      <v-col
        :class="$vuetify.breakpoint.mdAndDown ? 'text-left' : 'text-right'"
        cols="12"
        lg="6"
      >
        <v-btn
          v-if="!props.isFinalStep"
          color="primary"
          outlined
          @click="handleSave"
          :disabled="
            props.loading ||
            (!isUpdatingAllStepsComplete &&
              applicationStore.completeApplication.application
                .isUpdatingApplication)
          "
          :loading="props.loading"
        >
          <v-icon
            :color="themeStore.getThemeConfig.isDark ? 'white' : 'primary'"
            left
          >
            mdi-content-save
          </v-icon>
          <span :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''">
            {{ $t('Save and Continue Later') }}
          </span>
        </v-btn>
      </v-col>
    </v-row>
  </v-card-actions>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { computed, inject } from 'vue'

const isUpdatingAllStepsComplete = inject('allStepsComplete')
const applicationStore = useCompleteApplicationStore()
const themeStore = useThemeStore()

interface FormButtonContainerProps {
  valid?: boolean
  loading?: boolean
  allStepsComplete?: boolean
  isFinalStep?: boolean
  isFirstStep?: boolean
  isModification?: boolean
}

const props = withDefaults(defineProps<FormButtonContainerProps>(), {
  valid: false,
  loading: false,
  allStepsComplete: true,
  isFinalStep: false,
  isFirstStep: false,
  isModification: false,
})

const emit = defineEmits(['continue', 'save', 'previous-step', 'save-match'])

const getButtonText = computed(() => {
  if (props.isModification) {
    return 'Finalize Modification'
  }

  return 'Finalize Application'
})

const isUpdatingNotOnFinalStep = computed(() => {
  return (
    applicationStore.completeApplication.application.isUpdatingApplication &&
    !props.isFinalStep
  )
})

const isSubmittedAndMatchedWithoutAppointment = computed(() => {
  return (
    props.isFinalStep &&
    applicationStore.completeApplication.isMatchUpdated === false &&
    applicationStore.completeApplication.application.appointmentDateTime ===
      null &&
    applicationStore.completeApplication.application.status ===
      ApplicationStatus.Submitted
  )
})

const isUpdatingOnFinalStep = computed(() => {
  return (
    applicationStore.completeApplication.application.isUpdatingApplication &&
    props.isFinalStep
  )
})

const isFinalizeInitialModificationRenewal = computed(() => {
  return (
    props.isFinalStep === true &&
    applicationStore.completeApplication.application.isUpdatingApplication ===
      false &&
    applicationStore.completeApplication.isMatchUpdated !== false &&
    isSubmittedAndMatchedWithoutAppointment.value === false &&
    isUpdatingNotOnFinalStep.value === false
  )
})

const isSubmittedAndMatchedWithAppointmentOrDelivered = computed(() => {
  return (
    props.isFinalStep === true &&
    applicationStore.completeApplication.application.isUpdatingApplication ===
      true &&
    applicationStore.completeApplication.isMatchUpdated === false &&
    isSubmittedAndMatchedWithoutAppointment.value === false &&
    isUpdatingNotOnFinalStep.value === false
  )
})

function handleContinue() {
  emit('continue')
}

function handlePreviousStep() {
  emit('previous-step')
}

function handleSave() {
  emit('save')
}

function handleSaveMatched() {
  emit('save-match')
}
</script>
