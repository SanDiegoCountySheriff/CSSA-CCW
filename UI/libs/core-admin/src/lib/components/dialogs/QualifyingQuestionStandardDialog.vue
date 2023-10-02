<template>
  <v-dialog
    v-model="dialog"
    max-width="800"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-bind="attrs"
        v-on="on"
        icon
      >
        <v-icon
          v-if="
            permitStore.getPermitDetail.application.qualifyingQuestions
              .questionThree.temporaryExplanation
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
      <v-card-title>Flag Question {{ question }}</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-textarea
              v-model="requestedInformation"
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
import { ref } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useMutation } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationStatus,
  CommentType,
  QualifyingQuestionStandard,
} from '@shared-utils/types/defaultTypes'

interface IQualifyingQuestionsProps {
  question: string
}

const props = defineProps<IQualifyingQuestionsProps>()

const dialog = ref(false)
const comment = ref('')
const historyMessage = ref('')
const requestedInformation = ref('')
const permitStore = usePermitsStore()
const authStore = useAuthStore()

const { mutate: updatePermitDetails } = useMutation({
  mutationFn: () => permitStore.updatePermitDetailApi(historyMessage.value),
})

function handleCopy() {
  requestedInformation.value =
    permitStore.getPermitDetail.application.qualifyingQuestions[
      `question${props.question}`
    ].explanation
}

function handleSaveFlag(args) {
  convertToQualifyingQuestionStandard(
    permitStore.getPermitDetail.application.qualifyingQuestions[
      `question${args.question}`
    ]
  ).temporaryExplanation = args.temporaryExplanation

  if (args.question !== '') {
    const newComment: CommentType = {
      text: args.question,
      commentDateTimeUtc: new Date().toISOString(),
      commentMadeBy: authStore.auth.userEmail,
    }

    permitStore.getPermitDetail.application.comments.push(newComment)
  }

  historyMessage.value = `Flagged Qualifying Question ${args.question} for review`
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

  historyMessage.value = ''
}

function convertToQualifyingQuestionStandard(item) {
  return item as QualifyingQuestionStandard
}
</script>
