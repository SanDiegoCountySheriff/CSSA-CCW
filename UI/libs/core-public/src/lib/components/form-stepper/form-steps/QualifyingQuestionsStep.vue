<template>
  <v-container class="mb-10">
    <v-container
      v-if="state.isLoading && !state.isError"
      fluid
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
        divider, list-item-three-line,
        card-heading, image, image, image,
        image, actions"
      >
      </v-skeleton-loader>
    </v-container>
    <v-sheet
      v-else
      class="rounded p-4"
    >
      <v-subheader class="sub-header font-weight-bold mb-2">
        {{ $t('Qualifying Questions') }}
      </v-subheader>
      <v-form
        class="ml-5"
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
              v-model="
                state.application.application.qualifyingQuestions.questionOne
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionOne !== null,
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
          v-if="state.application.application.qualifyingQuestions.questionOne"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions.questionOneExp
                  .length >
                config.getAppConfig.questions.one - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.one"
              :label="
                $t(
                  'Provide issuing agency name, issue date and CCW license number.'
                )
              "
              v-model="
                state.application.application.qualifyingQuestions.questionOneExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions.questionOneExp
                  .length >
                config.getAppConfig.questions.one - 20
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
            {{ $t('QUESTION-TWO') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionTwo
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionTwo !== null,
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
          v-if="state.application.application.qualifyingQuestions.questionTwo"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions.questionTwoExp
                  .length >
                config.getAppConfig.questions.two - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.two"
              :label="
                $t(
                  'Provide issuing agency name, issue date and CCW license number.'
                )
              "
              v-model="
                state.application.application.qualifyingQuestions.questionTwoExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions.questionTwoExp
                  .length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionThree
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionThree !== null,
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
          v-if="state.application.application.qualifyingQuestions.questionThree"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionThreeExp.length >
                config.getAppConfig.questions.three - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.three"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionThreeExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionThreeExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionFour
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionFour !== null,
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
          v-if="state.application.application.qualifyingQuestions.questionFour"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionFourExp.length >
                config.getAppConfig.questions.four - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.four"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFourExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>

            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionFourExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionFive
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionFive !== null,
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
          v-if="state.application.application.qualifyingQuestions.questionFive"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :maxlength="config.getAppConfig.questions.five"
              :color="
                state.application.application.qualifyingQuestions
                  .questionFiveExp.length >
                config.getAppConfig.questions.five - 20
                  ? 'warning'
                  : ''
              "
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFiveExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionFiveExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionSix
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionSix !== null,
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
          v-if="state.application.application.qualifyingQuestions.questionSix"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions.questionSixExp
                  .length >
                config.getAppConfig.questions.six - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.six"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions.questionSixExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions.questionSixExp
                  .length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionSeven
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionSeven !== null,
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
          v-if="state.application.application.qualifyingQuestions.questionSeven"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionSevenExp.length >
                config.getAppConfig.questions.seven - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.seven"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSevenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>

            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionSevenExp.length >
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
                state.application.application.qualifyingQuestions
                  .questionEight !== null,
              ]"
              v-model="
                state.application.application.qualifyingQuestions.questionEight
              "
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
          v-if="state.application.application.qualifyingQuestions.questionEight"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionEightExp.length >
                config.getAppConfig.questions.eight - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.eight"
              :label="
                $t('Provide date, violation/accident, Agency, Citation No. ')
              "
              v-model="
                state.application.application.qualifyingQuestions
                  .questionEightExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionEightExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionNine
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionNine !== null,
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

        <v-row
          v-if="state.application.application.qualifyingQuestions.questionNine"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionNineExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions
                  .questionNineExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>

            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionNineExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionTen
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionTen !== null,
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
          v-if="state.application.application.qualifyingQuestions.questionTen"
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions.questionTenExp
                  .length >
                config.getAppConfig.questions.ten - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.ten"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions.questionTenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
              <v-alert
                outlined
                type="warning"
                v-if="
                  state.application.application.qualifyingQuestions
                    .questionTenExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionEleven
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionEleven !== null,
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
          v-if="
            state.application.application.qualifyingQuestions.questionEleven
          "
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionElevenExp.length >
                config.getAppConfig.questions.eleven - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.eleven"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionElevenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionElevenExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions.questionTwelve
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionTwelve !== null,
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
          v-if="
            state.application.application.qualifyingQuestions.questionTwelve
          "
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionTwelveExp.length >
                config.getAppConfig.questions.twelve - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.twelve"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionTwelveExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionTwelveExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions
                  .questionThirteen
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionThirteen !== null,
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
          v-if="
            state.application.application.qualifyingQuestions.questionThirteen
          "
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionThirteenExp.length >
                config.getAppConfig.questions.thirteen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.thirteen"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionThirteenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionThirteenExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFourteen
              "
              row
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionFourteen !== null,
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
        <v-row
          v-if="
            state.application.application.qualifyingQuestions.questionFourteen
          "
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionFourteenExp.length >
                config.getAppConfig.questions.fourteen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.fourteen"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFourteenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionFourteenExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFifteen
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionFifteen !== null,
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
          v-if="
            state.application.application.qualifyingQuestions.questionFifteen
          "
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionFifteenExp.length >
                config.getAppConfig.questions.fifteen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.fifteen"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFifteenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionFifteenExp.length >
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
                state.application.application.qualifyingQuestions
                  .questionSixteen !== null,
              ]"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSixteen
              "
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
          v-if="
            state.application.application.qualifyingQuestions.questionSixteen
          "
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionSixteenExp.length >
                config.getAppConfig.questions.sixteen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.sixteen"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSixteenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionSixteenExp.length >
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
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSeventeen
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionSeventeen !== null,
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

        <v-row
          v-if="
            state.application.application.qualifyingQuestions.questionSeventeen
          "
        >
          <v-col class="mx-8">
            <v-textarea
              outlined
              counter
              :color="
                state.application.application.qualifyingQuestions
                  .questionSeventeenExp.length >
                config.getAppConfig.questions.seventeen - 20
                  ? 'warning'
                  : ''
              "
              :maxlength="config.getAppConfig.questions.seventeen"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSeventeenExp
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
            >
            </v-textarea>
            <v-alert
              outlined
              type="warning"
              v-if="
                state.application.application.qualifyingQuestions
                  .questionSixteenExp.length >
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
        :submitting="state.submited"
        @submit="handleSubmit"
        @save="saveMutation.mutate"
        @back="goBackMutation.mutate"
        @cancel="router.push('/')"
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
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import { onMounted, reactive, ref } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRoute, useRouter } from 'vue-router/composables';
import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import { useAppConfigStore } from '@shared-ui/stores/configStore';

interface IProps {
  handleNextSection: CallableFunction;
  handlePreviousSection: CallableFunction;
}

const props = defineProps<IProps>();

const snackbar = ref(false);
const valid = ref(false);
const applicationStore = useCompleteApplicationStore();
const config = useAppConfigStore();
const router = useRouter();
const route = useRoute();
const state = reactive({
  application: applicationStore.getCompleteApplication as CompleteApplication,
  isLoading: true,
  isError: false,
  submited: false,
});

onMounted(() => {
  if (!applicationStore.completeApplication.application.orderId) {
    applicationStore
      .getCompleteApplicationFromApi(
        route.query.applicationId,
        route.query.isComplete
      )
      .then(res => {
        applicationStore.setCompleteApplication(res);
        state.application = res;
        state.isLoading = false;
      })
      .catch(() => {
        state.isError = true;
      });
  } else {
    state.isLoading = false;
  }
});

const updateMutation = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication();
  },
  onSuccess: () => {
    props.handleNextSection();
  },
  onError: () => {
    state.submited = false;
    snackbar.value = true;
  },
});

const goBackMutation = useMutation({
  mutationFn: () => {
    applicationStore.completeApplication.application.currentStep = 9;

    return applicationStore.updateApplication();
  },
  onSuccess: () => {
    props.handlePreviousSection();
  },
  onError: () => {
    snackbar.value = true;
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    snackbar.value = true;
  },
});

function handleSubmit() {
  state.submited = true;
  updateMutation.mutate();
}
</script>
