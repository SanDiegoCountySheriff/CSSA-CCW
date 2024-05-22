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
          :disabled="readonly"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-card-text
        v-if="permitStore.getPermitDetail.application.qualifyingQuestions"
      >
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
                :disabled="readonly"
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

              <QualifyingQuestionOneDialog :readonly="readonly" />
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
              :readonly="readonly"
              :label="$t('Agency')"
              :rules="[v => !!v || $t('An Agency is required.')]"
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOne.issuingState
              "
              :readonly="readonly"
              :label="$t('Issuing State')"
              :rules="[v => !!v || $t('An Issuing State is required.')]"
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOne.issueDate
              "
              :readonly="readonly"
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionTwoDialog :readonly="readonly" />
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
              :readonly="readonly"
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
              :readonly="readonly"
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Three'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Four'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Five'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Six'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Seven'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Eight'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionEight.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionEight.explanation
              "
              :readonly="readonly"
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
            {{ $t('QUESTION-NINE') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionNine.selected
                "
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Nine'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Ten'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Eleven'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionTwelveDialog :readonly="readonly" />
            </v-row>
          </v-col>
        </v-row>

        <v-row
          v-if="
            permitStore.getPermitDetail.application.qualifyingQuestions
              .questionTwelve.selected
          "
        >
          <template
            v-for="(violation, index) of permitStore.getPermitDetail.application
              .qualifyingQuestions.questionTwelve.trafficViolations"
          >
            <v-row :key="index">
              <v-col cols="3">
                <v-text-field
                  v-model="violation.date"
                  :readonly="readonly"
                  label="Date"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.agency"
                  :readonly="readonly"
                  label="Agency"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.violation"
                  :readonly="readonly"
                  label="Violation/Accident"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.citationNumber"
                  :readonly="readonly"
                  label="Citation Number"
                  outlined
                ></v-text-field>
              </v-col>
            </v-row>
          </template>

          <v-row
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionTwelve.trafficViolationsExplanation
            "
          >
            <v-col>
              <v-textarea
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionTwelve.trafficViolationsExplanation
                "
                :readonly="readonly"
                label="Traffic Violations Explanation"
              >
              </v-textarea>
            </v-col>
          </v-row>
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Thirteen'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Fourteen'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Fifteen'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Sixteen'"
              />
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
              :readonly="readonly"
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
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Seventeen'"
              />
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
              :readonly="readonly"
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
            {{ $t('QUESTION-EIGHTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionEighteen.selected
                "
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Eighteen'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionEighteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionEighteen.explanation
              "
              :readonly="readonly"
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
            {{ $t('QUESTION-NINETEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionNineteen.selected
                "
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Nineteen'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionNineteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionNineteen.explanation
              "
              :readonly="readonly"
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
            {{ $t('QUESTION-TWENTY') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionTwenty.selected
                "
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Twenty'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionTwenty.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwenty.explanation
              "
              :readonly="readonly"
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
            {{ $t('QUESTION-TWENTYONE') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionTwentyOne.selected
                "
                :disabled="readonly"
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

              <QualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'TwentyOne'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionTwentyOne.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwentyOne.explanation
              "
              :readonly="readonly"
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
      <v-card-text
        v-else-if="
          permitStore.getPermitDetail.application.legacyQualifyingQuestions
        "
      >
        <v-row align="center">
          <v-col>
            {{ $t('LEGACY-QUESTION-ONE') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionOne.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionOneDialog :readonly="readonly" />
            </v-row>
          </v-col>
        </v-row>

        <v-row
          v-if="
            permitStore.getPermitDetail.application.legacyQualifyingQuestions
              .questionOne.selected
          "
        >
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionOne.agency
              "
              :readonly="readonly"
              :label="$t('Agency')"
              :rules="[v => !!v || $t('An Agency is required.')]"
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionOne.issuingState
              "
              :readonly="readonly"
              :label="$t('Issuing State')"
              :rules="[v => !!v || $t('An Issuing State is required.')]"
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionOne.issueDate
              "
              :readonly="readonly"
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
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionOne.number
              "
              :readonly="readonly"
              :label="$t('Number')"
              :rules="[v => !!v || $t('An Issue Number is required.')]"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col>
            {{ $t('LEGACY-QUESTION-TWO') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionTwo.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionTwoDialog :readonly="readonly" />
            </v-row>
          </v-col>
        </v-row>

        <v-row
          v-if="
            permitStore.getPermitDetail.application.legacyQualifyingQuestions
              .questionTwo.selected
          "
        >
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionTwo.agency
              "
              :readonly="readonly"
              :label="$t('Agency')"
              :rules="[v => !!v || $t('An Agency is required.')]"
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionTwo.denialDate
              "
              :readonly="readonly"
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
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionTwo.denialReason
              "
              :readonly="readonly"
              :label="$t('Denial Reason')"
              :rules="[v => !!v || $t('A Denial Explanation is required.')]"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row align="center">
          <v-col>
            {{ $t('LEGACY-QUESTION-THREE') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionThree.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Three'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionThree.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionThree.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-FOUR') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionFour.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Four'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionFour.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionFour.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-FIVE') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionFive.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Five'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionFive.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionFive.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-SIX') }}
          </v-col>

          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionSix.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Six'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionSix.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionSix.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-SEVEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionSeven.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Seven'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionSeven.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionSeven.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-EIGHT') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionEight.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionEightDialog :readonly="readonly" />
            </v-row>
          </v-col>
        </v-row>

        <v-row
          v-if="
            permitStore.getPermitDetail.application.legacyQualifyingQuestions
              .questionEight.selected
          "
        >
          <template
            v-for="(violation, index) of permitStore.getPermitDetail.application
              .legacyQualifyingQuestions.questionEight.trafficViolations"
          >
            <v-row :key="index">
              <v-col cols="3">
                <v-text-field
                  v-model="violation.date"
                  :readonly="readonly"
                  label="Date"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.agency"
                  :readonly="readonly"
                  label="Agency"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.violation"
                  :readonly="readonly"
                  label="Violation/Accident"
                  outlined
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                <v-text-field
                  v-model="violation.citationNumber"
                  :readonly="readonly"
                  label="Citation Number"
                  outlined
                ></v-text-field>
              </v-col>
            </v-row>
          </template>

          <v-row
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionEight.trafficViolationsExplanation
            "
          >
            <v-col>
              <v-textarea
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionEight
                    .trafficViolationsExplanation
                "
                :readonly="readonly"
                label="Traffic Violations Explanation"
              >
              </v-textarea>
            </v-col>
          </v-row>
        </v-row>

        <v-row align="center">
          <v-col>
            {{ $t('LEGACY-QUESTION-NINE') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionNine.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Nine'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionNine.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionNine.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-TEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionTen.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Ten'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionTen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionTen.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-ELEVEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionEleven.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Eleven'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionEleven.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionEleven.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-TWELVE') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionTwelve.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Twelve'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionTwelve.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionTwelve.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-THIRTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionThirteen.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Thirteen'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionThirteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionThirteen.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-FOURTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionFourteen.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Fourteen'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionFourteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionFourteen.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-FIFTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionFifteen.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Fifteen'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionFifteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionFifteen.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-SIXTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionSixteen.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Sixteen'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionSixteen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionSixteen.explanation
              "
              :readonly="readonly"
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
            {{ $t('LEGACY-QUESTION-SEVENTEEN') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application
                    .legacyQualifyingQuestions.questionSeventeen.selected
                "
                :disabled="readonly"
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

              <LegacyQualifyingQuestionStandardDialog
                :readonly="readonly"
                :question="'Seventeen'"
              />
            </v-row>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.legacyQualifyingQuestions
                .questionSeventeen.selected
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application
                  .legacyQualifyingQuestions.questionSeventeen.explanation
              "
              :readonly="readonly"
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
import LegacyQualifyingQuestionEightDialog from '@core-admin/components/dialogs/LegacyQualifyingQuestionEightDialog.vue'
import LegacyQualifyingQuestionOneDialog from '@core-admin/components/dialogs/LegacyQualifyingQuestionOneDialog.vue'
import LegacyQualifyingQuestionStandardDialog from '@core-admin/components/dialogs/LegacyQualifyingQuestionStandardDialog.vue'
import LegacyQualifyingQuestionTwoDialog from '@core-admin/components/dialogs/LegacyQualifyingQuestionTwoDialog.vue'
import QualifyingQuestionOneDialog from '@core-admin/components/dialogs/QualifyingQuestionOneDialog.vue'
import QualifyingQuestionStandardDialog from '@core-admin/components/dialogs/QualifyingQuestionStandardDialog.vue'
import QualifyingQuestionTwelveDialog from '@core-admin/components/dialogs/QualifyingQuestionTwelveDialog.vue'
import QualifyingQuestionTwoDialog from '@core-admin/components/dialogs/QualifyingQuestionTwoDialog.vue'
import ReviewDialog from '@core-admin/components/dialogs/ReviewDialog.vue'
import SaveButton from './SaveButton.vue'
import { inject } from 'vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const emit = defineEmits(['on-save'])

const permitStore = usePermitsStore()
const readonly = inject<boolean>('readonly')

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
