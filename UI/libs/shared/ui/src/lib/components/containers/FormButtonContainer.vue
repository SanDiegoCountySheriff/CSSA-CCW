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

        <v-btn
          v-if="!props.isFinalStep"
          :disabled="!props.valid || props.loading || !props.allStepsComplete"
          :loading="props.loading"
          @click="handleContinue"
          color="primary"
        >
          {{ getButtonText }}
          <v-icon
            v-if="!props.isFinalStep"
            right
          >
            mdi-chevron-right
          </v-icon>
        </v-btn>

        <v-btn
          v-if="
            applicationStore.completeApplication.isMatchUpdated === false &&
            props.isFinalStep
          "
          :disabled="!allStepsComplete"
          @click="handleSaveMatched"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>
          {{ $t('Save') }}
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
  if (props.isModification && props.isFinalStep) {
    return 'Finalize Modification'
  }

  return props.isFinalStep ? 'Finalize Application' : 'Next Step'
})

function handleContinue() {
  emit('continue')
}

function handlePreviousStep() {
  emit('previous-step')
}

function handleSave() {
  if (applicationStore.completeApplication.application.isUpdatingApplication) {
    applicationStore.completeApplication.application.isUpdatingApplication =
      false
  }

  emit('save')
}

function handleSaveMatched() {
  emit('save-match')
}
</script>
