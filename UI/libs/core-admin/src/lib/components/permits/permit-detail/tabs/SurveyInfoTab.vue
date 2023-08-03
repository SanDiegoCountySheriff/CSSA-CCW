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
          <v-card-title
            class="headline"
            style="background-color: #bdbdbd"
          >
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
              elevation="2"
              color="error"
              @click="cancelChanges"
            >
              Cancel
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn
              elevation="2"
              color="primary"
              @click="acceptChanges"
              class="white--text"
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
                    .questionOne
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
                      .questionOneAgencyTemp ||
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionOneNumberTemp ||
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionOneNumberTemp
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
              .questionOne
          "
        >
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOneAgency
              "
              :label="$t('Agency')"
              :rules="[v => !!v || $t('An Agency is required.')]"
            ></v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOneIssueDate
              "
              :label="$t('Issue Date')"
              :rules="[v => !!v || $t('An Issue Date is required.')]"
              type="date"
              append-icon="mdi-calendar"
              clearable
            ></v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOneNumber
              "
              :label="$t('Number')"
              :rules="[v => !!v || $t('An Issue Number is required.')]"
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
                    .questionTwo
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
                @click="handleFlag('Two')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionTwoTempExplanation
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
                .questionTwo
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwoExp
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
            {{ $t('QUESTION-THREE') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionThree
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
                      .questionThreeTempExplanation
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
                .questionThree
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionThreeExp
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
                    .questionFour
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
                      .questionFourTempExplanation
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
                .questionFour
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFourExp
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
                    .questionFive
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
                      .questionFiveTempExplanation
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
                .questionFive
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFiveExp
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
                    .questionSix
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
                      .questionSixTempExplanation
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
                .questionSix
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSixExp
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
                    .questionSeven
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
                      .questionSevenTempExplanation
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
                .questionSeven
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSevenExp
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
                    .questionEight
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
                @click="handleFlag('Eight')"
                icon
              >
                <v-icon
                  v-if="
                    permitStore.getPermitDetail.application.qualifyingQuestions
                      .questionEightTempExplanation
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
                .questionEight
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionEightExp
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
            {{ $t('QUESTION-NINE') }}
          </v-col>
          <v-col>
            <v-row align="center">
              <v-radio-group
                v-model="
                  permitStore.getPermitDetail.application.qualifyingQuestions
                    .questionNine
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
                      .questionNineTempExplanation
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
                .questionNine
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionNineExp
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
                    .questionTen
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
                      .questionTenTempExplanation
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
                .questionTen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTenExp
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
                    .questionEleven
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
                      .questionElevenTempExplanation
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
                .questionEleven
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionElevenExp
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
                    .questionTwelve
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
                      .questionTwelveTempExplanation
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
                .questionTwelve
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwelveExp
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
                    .questionThirteen
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
                      .questionThirteenTempExplanation
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
                .questionThirteen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionThirteenExp
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
                    .questionFourteen
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
                      .questionFourteenTempExplanation
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
                .questionFourteen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFourteenExp
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
                    .questionFifteen
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
                      .questionFifteenTempExplanation
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
                .questionFifteen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFifteenExp
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
                    .questionSixteen
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
                      .questionSixteenTempExplanation
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
                .questionSixteen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSixteenExp
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
                    .questionSeventeen
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
                      .questionSeventeenTempExplanation
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
                .questionSeventeen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSeventeenExp
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
            @click="handleSaveQuestionOneFlag"
            color="primary"
          >
            Save
          </v-btn>
          <v-btn
            @click="flagQuestionOneDialog = false"
            color="primary"
          >
            Close
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
            @click="() => handleSaveFlag(question)"
            color="primary"
          >
            Save
          </v-btn>
          <v-btn
            @click="flagDialog = false"
            color="primary"
          >
            Close
          </v-btn>
          <v-btn
            @click="() => handleCopy(question)"
            color="primary"
            class="ml-auto"
          >
            <v-icon>mdi-content-copy</v-icon>
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { CommentType } from '@shared-utils/types/defaultTypes'
import SaveButton from './SaveButton.vue'
import { i18n } from '@shared-ui/plugins'
import { ref } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()
const flagDialog = ref(false)
const flagQuestionOneDialog = ref(false)
const question = ref('')
const requestedInformation = ref('')
const commentText = ref('')
const authStore = useAuthStore()
const questionOneAgencyTemp = ref('')
const questionOneIssueDateTemp = ref('')
const questionOneNumberTemp = ref('')
const historyMessage = ref('')
const reviewDialog = ref(false)
const flaggedQuestionText = ref('')
const flaggedQuestionHeader = ref('')

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

function handleFlag(questionNumber: string) {
  question.value = questionNumber
  flagDialog.value = true
  requestedInformation.value = ''
}

function handleSaveFlag(questionNumber: string) {
  // attach requested information to permit
  permitStore.getPermitDetail.application.qualifyingQuestions[
    `question${questionNumber}TempExplanation`
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

  permitStore.getPermitDetail.application.status = 14

  updatePermitDetails()

  historyMessage.value = ''

  commentText.value = ''
  requestedInformation.value = ''

  flagDialog.value = false
}

function handleSaveQuestionOneFlag() {
  // attach requested information to permit
  permitStore.getPermitDetail.application.qualifyingQuestions.questionOneAgencyTemp =
    questionOneAgencyTemp.value

  permitStore.getPermitDetail.application.qualifyingQuestions.questionOneIssueDateTemp =
    questionOneIssueDateTemp.value

  permitStore.getPermitDetail.application.qualifyingQuestions.questionOneNumberTemp =
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

  permitStore.getPermitDetail.application.status = 14

  updatePermitDetails()

  historyMessage.value = ''

  questionOneAgencyTemp.value = ''
  questionOneIssueDateTemp.value = ''
  questionOneNumberTemp.value = ''
  commentText.value = ''
  requestedInformation.value = ''

  flagDialog.value = false
}

function showReviewDialog() {
  const qualifyingQuestions =
    permitStore.getPermitDetail.application.qualifyingQuestions

  flaggedQuestionText.value = ''

  const questionOneAgencyTempValue =
    qualifyingQuestions.questionOneAgencyTemp || ''
  const questionOneIssueDateTempValue =
    qualifyingQuestions.questionOneIssueDateTemp || ''
  const questionOneNumberTempValue =
    qualifyingQuestions.questionOneNumberTemp || ''

  if (
    questionOneAgencyTempValue ||
    questionOneIssueDateTempValue ||
    questionOneNumberTempValue
  ) {
    flaggedQuestionText.value += `${i18n.t('QUESTION-ONE')}\n\n`

    flaggedQuestionText.value += `Original Response:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionOneAgency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionOneIssueDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionOneNumber || 'N/A'
    }\n\n`

    flaggedQuestionText.value += `Revised Changes:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionOneAgencyTemp || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionOneIssueDateTemp || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionOneNumberTemp || 'N/A'
    }\n\n`
  }

  for (const [key, value] of Object.entries(qualifyingQuestions)) {
    if (
      key.endsWith('TempExplanation') &&
      value != null &&
      !key.startsWith('questionOne')
    ) {
      const questionNumber = key
        .replace('TempExplanation', '')
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

  questionOneKeys.forEach(key => {
    if (qualifyingQuestions[key]) {
      const regularKey = key.replace('Temp', '')

      qualifyingQuestions[regularKey] = qualifyingQuestions[key]
      qualifyingQuestions[key] = null
      qualifyingQuestions.questionOne = true
    }
  })

  for (const [key, value] of Object.entries(qualifyingQuestions)) {
    if (value !== null && key.endsWith('TempExplanation')) {
      const regularKey = key.replace('TempExplanation', 'Exp')
      const yesNoKey = regularKey.replace('Exp', '')

      qualifyingQuestions[regularKey] = value
      qualifyingQuestions[yesNoKey] = true
      qualifyingQuestions[key] = null
    }
  }

  permitStore.getPermitDetail.application.flaggedForLicensingReview = false

  permitStore.getPermitDetail.application.flaggedForCustomerReview = false

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
      `question${questionNumber}Exp`
    ]
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
