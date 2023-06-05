<template>
  <v-container class="mb-10">
    <v-sheet class="rounded p-4">
      <v-card-title>
        {{ $t('Qualifying Questions') }}
      </v-card-title>
      <v-form
        ref="form"
        v-model="valid"
      >
        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="12"
            lg="6"
          >
            {{ $t('QUESTION-ONE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionOne"
              :rules="[
                model.application.qualifyingQuestions.questionOne !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row
          cols="12"
          lg="6"
          v-if="model.application.qualifyingQuestions.questionOne"
        >
          <v-col class="mx-8">
            <v-text-field
              outlined
              counter
              dense
              :color="'text'"
              maxlength="50"
              :label="$t('Issuing Agency')"
              v-model="model.application.qualifyingQuestions.questionOneAgency"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-text-field>
          </v-col>
          <v-col class="mx-8">
            <v-menu
              :v-model="state.menu"
              :close-on-content-click="false"
              transition="scale-transition"
              offset-y
              min-width="auto"
            >
              <template #activator="{ on, attrs }">
                <v-text-field
                  outlined
                  dense
                  readonly
                  class="pl-6"
                  v-model="
                    model.application.qualifyingQuestions.questionOneIssueDate
                  "
                  :label="$t('Issue Date')"
                  :rules="[v => !!v || $t('Date is required')]"
                  prepend-inner-icon="mdi-calendar"
                  v-bind="attrs"
                  v-on="on"
                ></v-text-field>
              </template>
              <v-date-picker
                v-model="
                  model.application.qualifyingQuestions.questionOneIssueDate
                "
                no-title
                scrollable
              >
              </v-date-picker>
            </v-menu>
          </v-col>
          <v-col class="mx-8">
            <v-text-field
              outlined
              dense
              counter
              :color="'text'"
              maxlength="50"
              :label="$t('CCW number')"
              v-model="model.application.qualifyingQuestions.questionOneNumber"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-TWO') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionTwo"
              :rules="[
                model.application.qualifyingQuestions.questionTwo !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row v-if="model.application.qualifyingQuestions.questionTwo">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionTwoExp.length >
                config.getAppConfig.questions.two - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.two"
              :label="
                $t(
                  'Provide the name of the agency, the date of denial, and the reason given for the denial.'
                )
              "
              v-model="model.application.qualifyingQuestions.questionTwoExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionTwoExp.length >
                config.getAppConfig.questions.two - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-THREE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionThree"
              :rules="[
                model.application.qualifyingQuestions.questionThree !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row v-if="model.application.qualifyingQuestions.questionThree">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionThreeExp.length >
                config.getAppConfig.questions.three - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.three"
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionThreeExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionThreeExp.length >
                config.getAppConfig.questions.three - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-FOUR') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionFour"
              :rules="[
                model.application.qualifyingQuestions.questionFour !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionFour">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionFourExp.length >
                config.getAppConfig.questions.four - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.four"
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionFourExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>

            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionFourExp.length >
                config.getAppConfig.questions.four - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-FIVE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionFive"
              :rules="[
                model.application.qualifyingQuestions.questionFive !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionFive">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :maxlength="config.getAppConfig.questions.five"
              :color="
                model.application.qualifyingQuestions.questionFiveExp.length >
                config.getAppConfig.questions.five - 20
                  ? 'warning'
                  : ''
              "
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionFiveExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionFiveExp.length >
                config.getAppConfig.questions.five - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-SIX') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionSix"
              :rules="[
                model.application.qualifyingQuestions.questionSix !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionSix">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionSixExp.length >
                config.getAppConfig.questions.six - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.six"
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionSixExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionSixExp.length >
                config.getAppConfig.questions.six - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-SEVEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionSeven"
              :rules="[
                model.application.qualifyingQuestions.questionSeven !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionSeven">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionSevenExp.length >
                config.getAppConfig.questions.seven - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.seven"
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionSevenExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>

            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionSevenExp.length >
                config.getAppConfig.questions.seven - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-EIGHT') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              :rules="[
                model.application.qualifyingQuestions.questionEight !== null,
              ]"
              v-model="model.application.qualifyingQuestions.questionEight"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionEight">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionEightExp.length >
                config.getAppConfig.questions.eight - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.eight"
              :label="
                $t('Provide date, violation/accident, Agency, Citation No. ')
              "
              v-model="model.application.qualifyingQuestions.questionEightExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionEightExp.length >
                config.getAppConfig.questions.eight - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-NINE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionNine"
              :rules="[
                model.application.qualifyingQuestions.questionNine !== null,
              ]"
              row
            >
              <v-radio
                :label="$t('YES')"
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionNine">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionNineExp.length >
                config.getAppConfig.questions.nine - 2
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.nine"
              :label="
                $t(
                  'Please explain including the date, agency, charges and disposition.'
                )
              "
              v-model="model.application.qualifyingQuestions.questionNineExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>

            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionNineExp.length >
                config.getAppConfig.questions.nine - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-TEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionTen"
              :rules="[
                model.application.qualifyingQuestions.questionTen !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionTen">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionTenExp.length >
                config.getAppConfig.questions.ten - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.ten"
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionTenExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
              <v-alert
                outlined
                type="warning"
                v-if="
                  model.application.qualifyingQuestions.questionTenExp.length >
                  config.getAppConfig.questions.ten - 20
                "
              >
                {{
                  $t(
                    'You are approaching the character limit and may have to reword your answer.'
                  )
                }}
              </v-alert>
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-ELEVEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionEleven"
              :rules="[
                model.application.qualifyingQuestions.questionEleven !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionEleven">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionElevenExp.length >
                config.getAppConfig.questions.eleven - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.eleven"
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionElevenExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionElevenExp.length >
                config.getAppConfig.questions.eleven - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-TWELVE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionTwelve"
              :rules="[
                model.application.qualifyingQuestions.questionTwelve !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionTwelve">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionTwelveExp.length >
                config.getAppConfig.questions.twelve - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.twelve"
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionTwelveExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionTwelveExp.length >
                config.getAppConfig.questions.twelve - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-THIRTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionThirteen"
              :rules="[
                model.application.qualifyingQuestions.questionThirteen !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionThirteen">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionThirteenExp
                  .length >
                config.getAppConfig.questions.thirteen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.thirteen"
              :label="$t('Please explain')"
              v-model="
                model.application.qualifyingQuestions.questionThirteenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionThirteenExp
                  .length >
                config.getAppConfig.questions.thirteen - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-FOURTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionFourteen"
              row
              :rules="[
                model.application.qualifyingQuestions.questionFourteen !== null,
              ]"
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row v-if="model.application.qualifyingQuestions.questionFourteen">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionFourteenExp
                  .length >
                config.getAppConfig.questions.fourteen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.fourteen"
              :label="$t('Please explain')"
              v-model="
                model.application.qualifyingQuestions.questionFourteenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionFourteenExp
                  .length >
                config.getAppConfig.questions.fourteen - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-FIFTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionFifteen"
              :rules="[
                model.application.qualifyingQuestions.questionFifteen !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionFifteen">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionFifteenExp
                  .length >
                config.getAppConfig.questions.fifteen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.fifteen"
              :label="
                $t(
                  'Please explain including the date, agency, charges, and disposition.'
                )
              "
              v-model="model.application.qualifyingQuestions.questionFifteenExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionFifteenExp
                  .length >
                config.getAppConfig.questions.fifteen - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="12"
            lg="6"
          >
            {{ $t('QUESTION-SIXTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              :rules="[
                model.application.qualifyingQuestions.questionSixteen !== null,
              ]"
              v-model="model.application.qualifyingQuestions.questionSixteen"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row v-if="model.application.qualifyingQuestions.questionSixteen">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionSixteenExp
                  .length >
                config.getAppConfig.questions.sixteen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.sixteen"
              :label="$t('Please explain')"
              v-model="model.application.qualifyingQuestions.questionSixteenExp"
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionSixteenExp
                  .length >
                config.getAppConfig.questions.sixteen - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-SEVENTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="model.application.qualifyingQuestions.questionSeventeen"
              :rules="[
                model.application.qualifyingQuestions.questionSeventeen !==
                  null,
              ]"
              row
            >
              <v-radio
                :label="$t('YES')"
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>

        <v-row v-if="model.application.qualifyingQuestions.questionSeventeen">
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                model.application.qualifyingQuestions.questionSeventeenExp
                  .length >
                config.getAppConfig.questions.seventeen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.seventeen"
              :label="$t('Please explain')"
              v-model="
                model.application.qualifyingQuestions.questionSeventeenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionSixteenExp
                  .length >
                config.getAppConfig.questions.sixteen - 20
              "
            >
              {{
                $t(
                  'You are approaching the character limit and may have to reword your answer.'
                )
              }}
            </v-alert>
          </v-col>
        </v-row>
      </v-form>
      <FormButtonContainer
        :valid="valid"
        @submit="handleSubmit"
        @save="handleSave"
      />
    </v-sheet>
    <v-snackbar
      :value="snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { computed, reactive, ref } from 'vue'

interface IProps {
  value: CompleteApplication
}

const props = defineProps<IProps>()
const emit = defineEmits([
  'input',
  'handle-submit',
  'handle-save',
  'update-step-eight-valid',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const snackbar = ref(false)
const valid = ref(false)
const config = useAppConfigStore()
const state = reactive({
  menu: false,
})

function handleSubmit() {
  emit('update-step-eight-valid', true)
  emit('handle-submit')
}

function handleSave() {
  emit('update-step-eight-valid', true)
  emit('handle-save')
}
</script>
