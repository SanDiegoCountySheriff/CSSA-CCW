<template>
  <v-dialog
    v-model="dialog"
    max-width="1200"
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
              .questionEight.temporaryTrafficViolations.length > 0
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
      <v-card-title>Flag Question Eight</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-data-table
              :items="temporaryTrafficViolations"
              :headers="headers"
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title>
                    Additional Traffic Violations
                  </v-toolbar-title>

                  <v-spacer />

                  <v-dialog
                    v-model="trafficViolationDialog"
                    max-width="1000"
                  >
                    <template #activator="{ on, attrs }">
                      <v-btn
                        color="primary"
                        v-bind="attrs"
                        v-on="on"
                      >
                        Add Item
                      </v-btn>
                    </template>
                    <v-card>
                      <v-card-title>Add Violation</v-card-title>
                      <v-card-text>
                        <v-row>
                          <v-col cols="6">
                            <v-text-field
                              v-model="editedTrafficViolation.date"
                              label="Date"
                              color="primary"
                              type="date"
                              outlined
                            ></v-text-field>
                          </v-col>

                          <v-col cols="6">
                            <v-text-field
                              v-model="editedTrafficViolation.agency"
                              label="Agency"
                              color="primary"
                              outlined
                            ></v-text-field>
                          </v-col>

                          <v-col cols="6">
                            <v-text-field
                              v-model="editedTrafficViolation.violation"
                              label="Violation"
                              color="primary"
                              outlined
                            ></v-text-field>
                          </v-col>

                          <v-col cols="6">
                            <v-text-field
                              v-model="editedTrafficViolation.citationNumber"
                              label="Citation Number"
                              color="primary"
                              outlined
                            ></v-text-field>
                          </v-col>
                        </v-row>
                      </v-card-text>
                      <v-card-actions>
                        <v-btn
                          color="error"
                          text
                          @click="trafficViolationDialog = false"
                        >
                          Cancel
                        </v-btn>
                        <v-spacer></v-spacer>
                        <v-btn
                          color="primary"
                          text
                          @click="saveViolation"
                        >
                          Save
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-dialog>
                </v-toolbar>
              </template>

              <template #[`item.actions`]="{ item }">
                <v-icon
                  small
                  @click="deleteViolation(item)"
                >
                  mdi-delete
                </v-icon>
              </template>
            </v-data-table>
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
          @click="handleSaveQuestionEightFlag"
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
  TrafficViolation,
} from '@shared-utils/types/defaultTypes'

const comment = ref('')
const historyMessage = ref('')
const dialog = ref(false)
const trafficViolationDialog = ref(false)
const temporaryTrafficViolations = ref<Array<TrafficViolation>>([])
const headers = [
  {
    text: 'Date',
    value: 'date',
  },
  {
    text: 'Agency',
    value: 'agency',
  },
  {
    text: 'Violation',
    value: 'violation',
  },
  {
    text: 'Citation Number',
    value: 'citationNumber',
  },
  { text: 'Actions', value: 'actions' },
]
const editedTrafficViolation = ref<TrafficViolation>({
  date: '',
  agency: '',
  violation: '',
  citationNumber: '',
})
const permitStore = usePermitsStore()
const authStore = useAuthStore()

const { mutate: updatePermitDetails } = useMutation({
  mutationFn: () => permitStore.updatePermitDetailApi(historyMessage.value),
})

function saveViolation() {
  temporaryTrafficViolations.value.push({ ...editedTrafficViolation.value })
  editedTrafficViolation.value = {
    date: '',
    agency: '',
    violation: '',
    citationNumber: '',
  }
  trafficViolationDialog.value = false
}

function deleteViolation(item: TrafficViolation) {
  temporaryTrafficViolations.value = temporaryTrafficViolations.value.filter(
    t => {
      return t !== item
    }
  )
}

function handleSaveQuestionEightFlag() {
  permitStore.getPermitDetail.application.qualifyingQuestions.questionEight.temporaryTrafficViolations =
    temporaryTrafficViolations.value

  const newComment: CommentType = {
    text: comment.value,
    commentDateTimeUtc: new Date().toISOString(),
    commentMadeBy: authStore.auth.userEmail,
  }

  historyMessage.value = 'Flagged Qualifying Question Eight for review'
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

  historyMessage.value = ''
  dialog.value = false
}
</script>
