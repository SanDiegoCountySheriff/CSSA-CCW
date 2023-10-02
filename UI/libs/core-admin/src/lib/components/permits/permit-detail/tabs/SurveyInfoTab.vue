<template>
  <div>
    <v-card elevation="0">
      <v-card-title>
        <div style="display: flex; align-items: center">
          {{ $t('Qualifying Questions') }}
          <v-btn
            v-if="
              permitStore.getPermitDetail.application.flaggedForLicensingReview
            "
            @click="showReviewDialog"
            color="error"
            class="ml-8"
          >
            {{ $t('Review Required') }}
          </v-btn>
        </div>

        <v-spacer></v-spacer>

        <SaveButton
          :disabled="false"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-dialog
        v-model="reviewDialog"
        max-width="800"
      >
        <v-card>
          <v-card-title>
            <v-icon
              large
              class="mr-3"
            >
              mdi-information-outline
            </v-icon>
            {{ flaggedQuestionHeader }}
          </v-card-title>

          <v-card-text>
            <div class="text-h6 font-weight-bold dark-grey--text mt-5 mb-5">
              The applicant has approved the changes. Please confirm if you
              would like to overwrite their previous response with the revised
              changes.
            </div>

            <v-textarea
              v-if="flaggedQuestionText"
              class="mt-7"
              outlined
              rows="6"
              auto-grow
              :value="flaggedQuestionText"
              readonly
              style="font-size: 18px"
            ></v-textarea>
          </v-card-text>

          <v-card-actions>
            <v-btn
              text
              color="error"
              @click="cancelChanges"
            >
              Cancel
            </v-btn>

            <v-spacer></v-spacer>

            <v-btn
              text
              color="primary"
              @click="acceptChanges"
            >
              Accept
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

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

              <v-btn
                @click="handleQuestionOneFlag"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionOne.temporaryAgency ||
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionOne.temporaryNumber ||
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionOne.temporaryIssueDate
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
              <v-btn
                @click="handleQuestionTwoFlag"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionTwo.temporaryAgency ||
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionTwo.temporaryDenialDate ||
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionTwo.temporaryDenialReason
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

              <v-btn
                @click="handleFlag('Three')"
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

              <v-btn
                @click="handleFlag('Four')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionFour.temporaryExplanation
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

              <v-btn
                @click="handleFlag('Five')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionFive.temporaryExplanation
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

              <v-btn
                @click="handleFlag('Six')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionSix.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Seven')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionSeven.temporaryExplanation
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
              <v-btn
                @click="handleQuestionEightFlag"
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
              <v-btn
                @click="handleFlag('Nine')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionNine.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Ten')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionTen.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Eleven')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionEleven.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Twelve')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionTwelve.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Thirteen')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionThirteen.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Fourteen')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionFourteen.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Fifteen')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionFifteen.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Sixteen')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionSixteen.temporaryExplanation
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
              <v-btn
                @click="handleFlag('Seventeen')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionSeventeen.temporaryExplanation
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

    <v-dialog
      v-model="flagQuestionOneDialog"
      max-width="800"
    >
      <v-card>
        <v-card-title>Flag Question One</v-card-title>

        <v-card-text>
          <v-row>
            <v-col>
              <v-text-field
                v-model="questionOneAgencyTemp"
                label="Correct agency, this is what the customer will verify"
                color="primary"
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                v-model="questionOneIssueDateTemp"
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
                v-model="questionOneNumberTemp"
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
                v-model="commentText"
                color="primary"
                outlined
              ></v-textarea>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-actions>
          <v-btn
            text
            @click="flagQuestionOneDialog = false"
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

    <v-dialog
      v-model="flagQuestionTwoDialog"
      max-width="800"
    >
      <v-card>
        <v-card-title>Flag Question Two</v-card-title>

        <v-card-text>
          <v-row>
            <v-col>
              <v-text-field
                v-model="questionTwoAgencyTemp"
                label="Correct agency, this is what the customer will verify"
                color="primary"
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                v-model="questionTwoDenialDateTemp"
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
                v-model="questionTwoDenialReasonTemp"
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
                v-model="commentText"
                color="primary"
                outlined
              ></v-textarea>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-actions>
          <v-btn
            text
            @click="flagQuestionTwoDialog = false"
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

    <v-dialog
      v-model="flagQuestionEightDialog"
      max-width="1200"
    >
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
                    {{ temporaryTrafficViolations }}

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
                v-model="commentText"
                color="primary"
                outlined
              ></v-textarea>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-actions>
          <v-btn
            text
            @click="flagQuestionEightDialog = false"
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

    <v-dialog
      v-model="flagDialog"
      max-width="800"
    >
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
                v-model="commentText"
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
            @click="flagDialog = false"
            color="error"
          >
            Cancel
          </v-btn>
          <v-spacer></v-spacer>
          <v-tooltip bottom>
            <template #activator="{ on }">
              <v-btn
                text
                @click="() => handleCopy(question)"
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
            @click="() => handleSaveFlag(question)"
            color="primary"
          >
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import { i18n } from '@shared-ui/plugins'
import { ref } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import {
  ApplicationStatus,
  CommentType,
  TrafficViolation,
} from '@shared-utils/types/defaultTypes'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()
const flagDialog = ref(false)
const flagQuestionOneDialog = ref(false)
const flagQuestionTwoDialog = ref(false)
const flagQuestionEightDialog = ref(false)
const question = ref('')
const requestedInformation = ref('')
const commentText = ref('')
const authStore = useAuthStore()
const questionOneAgencyTemp = ref('')
const questionOneIssueDateTemp = ref('')
const questionOneNumberTemp = ref('')
const questionTwoAgencyTemp = ref('')
const questionTwoDenialDateTemp = ref('')
const questionTwoDenialReasonTemp = ref('')
const historyMessage = ref('')
const reviewDialog = ref(false)
const flaggedQuestionText = ref('')
const flaggedQuestionHeader = ref('')
const temporaryTrafficViolations = ref<Array<TrafficViolation>>([])
const editedTrafficViolation = ref<TrafficViolation>({
  date: '',
  agency: '',
  violation: '',
  citationNumber: '',
})
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
const trafficViolationDialog = ref(false)

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => {
    return permitStore.updatePermitDetailApi(historyMessage.value)
  },
  {
    enabled: false,
  }
)

function handleSave() {
  emit('on-save', 'Qualifying Questions')
}

function handleQuestionOneFlag() {
  flagQuestionOneDialog.value = true
}

function handleQuestionTwoFlag() {
  flagQuestionTwoDialog.value = true
}

function handleQuestionEightFlag() {
  flagQuestionEightDialog.value = true
}

function handleFlag(questionNumber: string) {
  question.value = questionNumber
  flagDialog.value = true
  requestedInformation.value = ''
}

function handleSaveFlag(questionNumber: string) {
  // attach requested information to permit
  permitStore.getPermitDetail.application.qualifyingQuestions[
    `question${questionNumber}.temporaryExplanation`
  ] = requestedInformation.value

  // attach comment to permit
  if (commentText.value !== '') {
    const newComment: CommentType = {
      text: commentText.value,
      commentDateTimeUtc: new Date().toISOString(),
      commentMadeBy: authStore.auth.userEmail,
    }

    permitStore.getPermitDetail.application.comments.push(newComment)
  }

  historyMessage.value = `Flagged Qualifying Question ${questionNumber} for review`

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

  commentText.value = ''
  requestedInformation.value = ''

  flagDialog.value = false
}

function handleSaveQuestionOneFlag() {
  // attach requested information to permit
  permitStore.getPermitDetail.application.qualifyingQuestions.questionOne.temporaryAgency =
    questionOneAgencyTemp.value

  permitStore.getPermitDetail.application.qualifyingQuestions.questionOne.temporaryIssueDate =
    questionOneIssueDateTemp.value

  permitStore.getPermitDetail.application.qualifyingQuestions.questionOne.temporaryNumber =
    questionOneNumberTemp.value

  // attach comment to permit
  const newComment: CommentType = {
    text: commentText.value,
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

  historyMessage.value = ''

  questionOneAgencyTemp.value = ''
  questionOneIssueDateTemp.value = ''
  questionOneNumberTemp.value = ''
  commentText.value = ''
  requestedInformation.value = ''

  flagQuestionOneDialog.value = false
}

function handleSaveQuestionTwoFlag() {
  permitStore.getPermitDetail.application.qualifyingQuestions.questionTwo.temporaryAgency =
    questionTwoAgencyTemp.value
  permitStore.getPermitDetail.application.qualifyingQuestions.questionTwo.temporaryDenialDate =
    questionTwoDenialDateTemp.value
  permitStore.getPermitDetail.application.qualifyingQuestions.questionTwo.temporaryDenialReason =
    questionTwoDenialReasonTemp.value

  const newComment: CommentType = {
    text: commentText.value,
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

  historyMessage.value = ''
  questionTwoAgencyTemp.value = ''
  questionTwoDenialDateTemp.value = ''
  questionTwoDenialReasonTemp.value = ''
  commentText.value = ''
  requestedInformation.value = ''
  flagQuestionTwoDialog.value = false
}

function handleSaveQuestionEightFlag() {
  permitStore.getPermitDetail.application.qualifyingQuestions.questionEight.temporaryTrafficViolations =
    temporaryTrafficViolations.value

  const newComment: CommentType = {
    text: commentText.value,
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
  temporaryTrafficViolations.value = []
  commentText.value = ''
  flagQuestionEightDialog.value = false
}

function showReviewDialog() {
  const qualifyingQuestions =
    permitStore.getPermitDetail.application.qualifyingQuestions

  flaggedQuestionText.value = ''

  const questionOneAgencyTempValue =
    qualifyingQuestions.questionOne.temporaryAgency || ''
  const questionOneIssueDateTempValue =
    qualifyingQuestions.questionOne.temporaryIssueDate || ''
  const questionOneNumberTempValue =
    qualifyingQuestions.questionOne.temporaryNumber || ''

  const questionTwoAgencyTempValue =
    qualifyingQuestions.questionTwo.temporaryAgency || ''
  const questionTwoDenialDateTempValue =
    qualifyingQuestions.questionTwo.temporaryDenialDate || ''
  const questionTwoDenialReasonTempValue =
    qualifyingQuestions.questionTwo.temporaryDenialReason || ''

  if (
    questionOneAgencyTempValue ||
    questionOneIssueDateTempValue ||
    questionOneNumberTempValue
  ) {
    flaggedQuestionText.value += `${i18n.t('QUESTION-ONE')}\n\n`

    flaggedQuestionText.value += `Original Response:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionOne.agency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionOne.issueDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionOne.number || 'N/A'
    }\n\n`

    flaggedQuestionText.value += `Revised Changes:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionOne.temporaryAgency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionOne.temporaryIssueDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionOne.temporaryNumber || 'N/A'
    }\n\n`
  }

  if (
    questionTwoAgencyTempValue ||
    questionTwoDenialDateTempValue ||
    questionTwoDenialReasonTempValue
  ) {
    flaggedQuestionText.value += `${i18n.t('QUESTION-TWO')}\n\n`

    flaggedQuestionText.value += `Original Response:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionTwo.agency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Denial Date: ${
      qualifyingQuestions.questionTwo.denialDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `Denial Reason Number: ${
      qualifyingQuestions.questionTwo.denialReason || 'N/A'
    }\n\n`

    flaggedQuestionText.value += `Revised Changes:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionTwo.temporaryAgency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionTwo.temporaryDenialDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionTwo.temporaryDenialReason || 'N/A'
    }\n\n`
  }

  if (qualifyingQuestions.questionEight.temporaryTrafficViolations.length > 0) {
    flaggedQuestionText.value += `${i18n.t('QUESTION-EIGHT')}\n\n`

    for (const trafficViolation of qualifyingQuestions.questionEight
      .temporaryTrafficViolations) {
      flaggedQuestionText.value += `Additional Citations Found: \n`
      flaggedQuestionText.value += `Date: ${trafficViolation.date}\n`
      flaggedQuestionText.value += `Agency: ${trafficViolation.agency}\n`
      flaggedQuestionText.value += `Violation: ${trafficViolation.violation}\n`
      flaggedQuestionText.value += `Citation Number: ${trafficViolation.citationNumber}\n\n`
    }
  }

  for (const [key, value] of Object.entries(qualifyingQuestions)) {
    if (
      key.endsWith('.temporaryExplanation') &&
      value != null &&
      !key.startsWith('questionOne')
    ) {
      const questionNumber = key
        .replace('.temporaryExplanation', '')
        .replace('question', '')

      const originalResponse =
        qualifyingQuestions[`question${questionNumber}Exp`]

      const revisedChanges = value

      flaggedQuestionText.value += `Question: ${i18n.t(
        `QUESTION-${questionNumber.toUpperCase()}`
      )}\n\n`
      flaggedQuestionText.value += `Original Response:  ${originalResponse}\n\n`
      flaggedQuestionText.value += `Revised Changes: ${revisedChanges}\n\n`
    }
  }

  if (flaggedQuestionText.value !== '') {
    reviewDialog.value = true
    flaggedQuestionHeader.value = 'Review Required'
  }
}

function acceptChanges() {
  const qualifyingQuestions =
    permitStore.getPermitDetail.application.qualifyingQuestions

  const questionOneKeys = [
    'questionOneAgencyTemp',
    'questionOneIssueDateTemp',
    'questionOneNumberTemp',
  ]

  const questionTwoKeys = [
    'questionTwoAgencyTemp',
    'questionTwoDenialDateTemp',
    'questionTwoDenialReasonTemp',
  ]

  questionOneKeys.forEach(key => {
    if (qualifyingQuestions[key]) {
      const regularKey = key.replace('Temp', '')

      qualifyingQuestions[regularKey] = qualifyingQuestions[key]
      qualifyingQuestions[key] = null
      qualifyingQuestions.questionOne.selected = true
    }
  })

  questionTwoKeys.forEach(key => {
    if (qualifyingQuestions[key]) {
      const regularKey = key.replace('Temp', '')

      qualifyingQuestions[regularKey] = qualifyingQuestions[key]
      qualifyingQuestions[key] = null
      qualifyingQuestions.questionTwo.selected = true
    }
  })

  for (const trafficViolation of qualifyingQuestions.questionEight
    .temporaryTrafficViolations) {
    qualifyingQuestions.questionEight.trafficViolations.push(trafficViolation)
  }

  qualifyingQuestions.questionEight.temporaryTrafficViolations = []

  for (const [key, value] of Object.entries(qualifyingQuestions)) {
    if (value !== null && key.endsWith('.temporaryExplanation')) {
      const regularKey = key.replace('.temporaryExplanation', 'Exp')
      const yesNoKey = regularKey.replace('Exp', '')

      qualifyingQuestions[regularKey] = value
      qualifyingQuestions[yesNoKey] = true
      qualifyingQuestions[key] = null
    }
  }

  permitStore.getPermitDetail.application.flaggedForLicensingReview = false

  permitStore.getPermitDetail.application.flaggedForCustomerReview = false

  permitStore.getPermitDetail.application.status =
    permitStore.getPermitDetail.application.originalStatus

  reviewDialog.value = false

  historyMessage.value = `Updated Qualifying Questions`

  updatePermitDetails()

  historyMessage.value = ''
}

function cancelChanges() {
  reviewDialog.value = false
}

function handleCopy(questionNumber: string) {
  requestedInformation.value =
    permitStore.getPermitDetail.application.qualifyingQuestions[
      `question${questionNumber}`
    ].explanation
}

function deleteViolation(item) {
  window.console.log(item)
}

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
