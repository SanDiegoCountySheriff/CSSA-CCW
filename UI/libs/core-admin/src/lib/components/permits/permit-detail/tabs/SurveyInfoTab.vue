<template>
  <div>
    <v-card elevation="0">
      <v-card-title>
        <div style="display: flex; align-items: center">
          {{ $t('Qualifying Questions') }}
          <ReviewDialog />
        </div>

        <v-spacer></v-spacer>

        <SaveButton
          :disabled="false"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-card-text>
        <v-row align="center">
          <v-col>
            {{ $t('QUESTION-ONE') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionOne.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionOneDialog />
            </v-row>
          </v-col>
        </v-row>

        <v-row
          v-if="
            permitStore.getPermitDetail.application.qualifyingQuestions
              .questionOne.selected
          "
        >
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOne.agency
              "
              :label="$t('Agency')"
              :rules="[v => !!v || $t('An Agency is required.')]"
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOne.issueDate
              "
              :label="$t('Issue Date')"
              :rules="[v => !!v || $t('An Issue Date is required.')]"
              type="date"
              append-icon="mdi-calendar"
              clearable
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOne.number
              "
              :label="$t('Number')"
              :rules="[v => !!v || $t('An Issue Number is required.')]"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col>
            {{ $t('QUESTION-TWO') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionTwo.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionTwoDialog />
            </v-row>
          </v-col>
        </v-row>

        <v-row
          v-if="
            permitStore.getPermitDetail.application.qualifyingQuestions
              .questionTwo.selected
          "
        >
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwo.agency
              "
              :label="$t('Agency')"
              :rules="[v => !!v || $t('An Agency is required.')]"
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwo.denialDate
              "
              :label="$t('Denial Date')"
              :rules="[v => !!v || $t('A Denial Date is required.')]"
              type="date"
              append-icon="mdi-calendar"
              clearable
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwo.denialReason
              "
              :label="$t('Denial Reason')"
              :rules="[v => !!v || $t('A Denial Explanation is required.')]"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col>
            {{ $t('QUESTION-THREE') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionThree.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Three'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionThree.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionThree.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col>
            {{ $t('QUESTION-FOUR') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionFour.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Four'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionFour.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFour.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-FIVE') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionFive.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Five'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionFive.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFive.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-SIX') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionSix.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Six'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionSix.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSix.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-SEVEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionSeven.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Seven'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionSeven.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSeven.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-EIGHT') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionEight.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionEightDialog />
            </v-row>
          </v-col>
        </v-row>

        <v-row
          v-if="
            permitStore.getPermitDetail.application.qualifyingQuestions
              .questionEight.selected
          "
        >
          <template
            v-for="(violation, index) of permitStore.getPermitDetail.application
              .qualifyingQuestions.questionEight.trafficViolations"
          >
            <v-row :key="index">
              <v-col cols="3">
                <v-text-field
                  v-model="violation.date"
                  label="Date"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.agency"
                  label="Agency"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.violation"
                  label="Violation/Accident"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.citationNumber"
                  label="Citation Number"
                  outlined
                ></v-text-field>
              </v-col>
            </v-row>
          </template>
        </v-row>

        <v-row align="center">
          <v-col>
            {{ $t('QUESTION-NINE') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionNine.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Nine'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionNine.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionNine.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-TEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionTen.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Ten'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionTen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTen.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-ELEVEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionEleven.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Eleven'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionEleven.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionEleven.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-TWELVE') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionTwelve.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Twelve'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionTwelve.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwelve.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-THIRTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionThirteen.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Thirteen'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionThirteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionThirteen.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-FOURTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionFourteen.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Fourteen'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionFourteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFourteen.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-FIFTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionFifteen.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Fifteen'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionFifteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFifteen.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col>
            {{ $t('QUESTION-SIXTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionSixteen.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Sixteen'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionSixteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSixteen.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col class="text-left">
            {{ $t('QUESTION-SEVENTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionSeventeen.selected
                "
                row
              >
                <v-radio
                  color="primary"
                  :label="$t('YES')"
                  :value="true"
                />

                <v-radio
                  color="primary"
                  :label="$t('NO')"
                  :value="false"
                />
              </v-radio-group>

              <QualifyingQuestionStandardDialog :question="'Seventeen'" />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionSeventeen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSeventeen.explanation
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import QualifyingQuestionEightDialog from '@core-admin/components/dialogs/QualifyingQuestionEightDialog.vue'
import QualifyingQuestionOneDialog from '@core-admin/components/dialogs/QualifyingQuestionOneDialog.vue'
import QualifyingQuestionStandardDialog from '@core-admin/components/dialogs/QualifyingQuestionStandardDialog.vue'
import QualifyingQuestionTwoDialog from '@core-admin/components/dialogs/QualifyingQuestionTwoDialog.vue'
import ReviewDialog from '@core-admin/components/dialogs/ReviewDialog.vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const emit = defineEmits(['on-save'])

const permitStore = usePermitsStore()

function handleSave() {
  emit('on-save', 'Qualifying Questions')
}
</script>

<style scoped>
::-webkit-calendar-picker-indicator {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  width: auto;
  height: auto;
  color: transparent;
  background: transparent;
}
input::-webkit-date-and-time-value {
  text-align: left;
}
</style>
