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
            permitStore.getPermitDetail.application.legacyQualifyingQuestions
              ?.questionTwo.temporaryAgency ||
            permitStore.getPermitDetail.application.legacyQualifyingQuestions
              ?.questionTwo.temporaryDenialDate ||
            permitStore.getPermitDetail.application.legacyQualifyingQuestions
              ?.questionTwo.temporaryDenialReason
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
      <v-card-title>Flag Question Two</v-card-title>

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
              v-model="temporaryDenialDate"
              label="Correct denial date, this is what the customer will verify"
              color="primary"
              outlined
              type="date"
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              v-model="temporaryDenialReason"
              label="Correct denial reason, this is what the customer will verify"
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
          @click="handleSaveQuestionTwoFlag"
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
const temporaryAgency = ref('')
const temporaryDenialDate = ref('')
const temporaryDenialReason = ref('')
const historyMessage = ref('')

const { mutate: updatePermitDetails } = useMutation({
  mutationFn: () => permitStore.updateUserApplication(historyMessage.value),
})

function handleSaveQuestionTwoFlag() {
  if (permitStore.getPermitDetail.application.legacyQualifyingQuestions) {
    permitStore.getPermitDetail.application.legacyQualifyingQuestions.questionTwo.temporaryAgency =
      temporaryAgency.value
    permitStore.getPermitDetail.application.legacyQualifyingQuestions.questionTwo.temporaryDenialDate =
      temporaryDenialDate.value
    permitStore.getPermitDetail.application.legacyQualifyingQuestions.questionTwo.temporaryDenialReason =
      temporaryDenialReason.value
  }

  const newComment: CommentType = {
    text: comment.value,
    commentDateTimeUtc: new Date().toISOString(),
    commentMadeBy: authStore.auth.userEmail,
  }

  historyMessage.value = 'Flagged Qualifying Question Two for review'
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
