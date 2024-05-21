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
        <v-icon
          v-if="
            permitStore.getPermitDetail.application.qualifyingQuestions
              ?.questionOne.temporaryAgency ||
            permitStore.getPermitDetail.application.qualifyingQuestions
              ?.questionOne.temporaryNumber ||
            permitStore.getPermitDetail.application.qualifyingQuestions
              ?.questionOne.temporaryIssueDate
          "
          color="error"
        >
          mdi-flag
        </v-icon>

        <v-icon
          v-else
          color="primary"
        >
          mdi-flag
        </v-icon>
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Flag Question One</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              v-model="temporaryAgency"
              label="Correct agency, this is what the customer will verify"
              color="primary"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              v-model="temporaryIssuingState"
              label="Correct state, this is what the customer will verify"
              color="primary"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              v-model="temporaryIssueDate"
              label="Correct issue date, this is what the customer will verify"
              color="primary"
              outlined
              type="date"
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              v-model="temporaryNumber"
              label="Correct permit number, this is what the customer will verify"
              color="primary"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-textarea
              label="Comments, not seen by customer"
              v-model="comment"
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
        <v-btn
          text
          @click="handleSaveQuestionOneFlag"
          color="primary"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useMutation } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationStatus,
  CommentType,
} from '@shared-utils/types/defaultTypes'

const props = withDefaults(defineProps<{ readonly: boolean }>(), {
  readonly: false,
})

const permitStore = usePermitsStore()
const authStore = useAuthStore()
const dialog = ref(false)
const comment = ref('')
const historyMessage = ref('')
const temporaryAgency = ref('')
const temporaryIssueDate = ref('')
const temporaryNumber = ref('')
const temporaryIssuingState = ref('')

const { mutate: updatePermitDetails } = useMutation({
  mutationFn: () => permitStore.updatePermitDetailApi(historyMessage.value),
})

function handleSaveQuestionOneFlag() {
  if (permitStore.getPermitDetail.application.qualifyingQuestions) {
    permitStore.getPermitDetail.application.qualifyingQuestions.questionOne.temporaryAgency =
      temporaryAgency.value
    permitStore.getPermitDetail.application.qualifyingQuestions.questionOne.temporaryIssueDate =
      temporaryIssueDate.value
    permitStore.getPermitDetail.application.qualifyingQuestions.questionOne.temporaryNumber =
      temporaryNumber.value
    permitStore.getPermitDetail.application.qualifyingQuestions.questionOne.temporaryIssuingState =
      temporaryIssuingState.value
  }

  const newComment: CommentType = {
    text: comment.value,
    commentDateTimeUtc: new Date().toISOString(),
    commentMadeBy: authStore.auth.userEmail,
  }

  historyMessage.value = 'Flagged Qualifying Question One for review'
  permitStore.getPermitDetail.application.comments.push(newComment)
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
</script>
