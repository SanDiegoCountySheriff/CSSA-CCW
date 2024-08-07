<template>
  <v-dialog
    v-model="dialog"
    max-width="800"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-bind="attrs"
        v-on="on"
        :disabled="readonly"
        icon
      >
        <v-icon :color="flagColor"> mdi-flag </v-icon>
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Flag Question {{ question }}</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-textarea
              v-model="temporaryExplanation"
              label="Found information, this is what the customer will verify"
              color="primary"
              outlined
            ></v-textarea>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-textarea
              v-model="comment"
              label="Comments, not seen by customer"
              color="primary"
              outlined
            ></v-textarea>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-actions>
        <v-btn
          text
          @click="dialog = false"
          color="error"
        >
          Cancel
        </v-btn>
        <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template #activator="{ on }">
            <v-btn
              text
              @click="handleCopy"
              color="primary"
              v-on="on"
              slot="activator"
            >
              <v-icon>mdi-content-copy</v-icon>
            </v-btn>
          </template>
          Copy Response
        </v-tooltip>
        <v-btn
          text
          @click="handleSaveFlag"
          color="primary"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { useAuthStore } from '@shared-ui/stores/auth'
import { useMutation } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationStatus,
  CommentType,
  QualifyingQuestionStandard,
} from '@shared-utils/types/defaultTypes'
import { computed, ref } from 'vue'

interface IQualifyingQuestionsProps {
  question: string
  readonly: boolean
}

const props = withDefaults(defineProps<IQualifyingQuestionsProps>(), {
  readonly: false,
})

const dialog = ref(false)
const comment = ref('')
const historyMessage = ref('')
const temporaryExplanation = ref('')
const permitStore = usePermitsStore()
const authStore = useAuthStore()
const flagColor = computed(() => {
  if (permitStore.getPermitDetail.application.qualifyingQuestions) {
    return permitStore.getPermitDetail.application.qualifyingQuestions[
      `question${props.question}`
    ].temporaryExplanation
      ? 'error'
      : 'primary'
  }

  return 'primary'
})

const { mutate: updatePermitDetails } = useMutation({
  mutationFn: () => permitStore.updateUserApplication(historyMessage.value),
})

function handleCopy() {
  if (permitStore.getPermitDetail.application.qualifyingQuestions) {
    temporaryExplanation.value =
      permitStore.getPermitDetail.application.qualifyingQuestions[
        `question${props.question}`
      ].explanation
  }
}

function handleSaveFlag() {
  if (permitStore.getPermitDetail.application.qualifyingQuestions) {
    convertToQualifyingQuestionStandard(
      permitStore.getPermitDetail.application.qualifyingQuestions[
        `question${props.question}`
      ]
    ).temporaryExplanation = temporaryExplanation.value
  }

  if (comment.value !== '') {
    const newComment: CommentType = {
      text: comment.value,
      commentDateTimeUtc: new Date().toISOString(),
      commentMadeBy: authStore.auth.userEmail,
    }

    permitStore.getPermitDetail.application.comments.push(newComment)
  }

  historyMessage.value = `Flagged Qualifying Question ${props.question} for review`
  permitStore.getPermitDetail.application.flaggedForCustomerReview = true

  if (
    permitStore.getPermitDetail.application.status !==
    ApplicationStatus['Flagged For Review']
  ) {
    permitStore.getPermitDetail.application.originalStatus =
      permitStore.getPermitDetail.application.status
  }

  permitStore.getPermitDetail.application.status =
    ApplicationStatus['Flagged For Review']

  updatePermitDetails()

  dialog.value = false
}

function convertToQualifyingQuestionStandard(item) {
  return item as QualifyingQuestionStandard
}
</script>
