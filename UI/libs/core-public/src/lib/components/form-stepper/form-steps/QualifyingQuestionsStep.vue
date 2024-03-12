<template>
  <v-container class="mb-10">
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
            v-model="model.application.qualifyingQuestions.questionOne.selected"
            :rules="[
              model.application.qualifyingQuestions.questionOne.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionOne.selected"
      >
        <v-col class="mx-8">
          <v-text-field
            outlined
            counter
            dense
            :color="'text'"
            maxlength="50"
            :label="$t('Issuing Agency')"
            v-model="model.application.qualifyingQuestions.questionOne.agency"
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-text-field>
        </v-col>
        <v-col class="mx-8">
          <v-text-field
            outlined
            counter
            dense
            :color="'text'"
            maxlength="50"
            :label="$t('Issuing State')"
            v-model="
              model.application.qualifyingQuestions.questionOne.issuingState
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
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
                  model.application.qualifyingQuestions.questionOne.issueDate
                "
                :label="$t('Issue Date')"
                :rules="[v => !!v || $t('Date is required')]"
                :disabled="model.application.applicationType.includes('renew')"
                prepend-inner-icon="mdi-calendar"
                v-bind="attrs"
                v-on="on"
              ></v-text-field>
            </template>
            <v-date-picker
              v-model="
                model.application.qualifyingQuestions.questionOne.issueDate
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
            v-model="model.application.qualifyingQuestions.questionOne.number"
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
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
            v-model="model.application.qualifyingQuestions.questionTwo.selected"
            :rules="[
              model.application.qualifyingQuestions.questionTwo.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionTwo.selected"
      >
        <v-col class="mx-8">
          <v-text-field
            outlined
            counter
            dense
            :color="'text'"
            maxlength="50"
            :label="$t('Agency Name')"
            v-model="model.application.qualifyingQuestions.questionTwo.agency"
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
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
                  model.application.qualifyingQuestions.questionTwo.denialDate
                "
                :label="$t('Denial Date')"
                :rules="[v => !!v || $t('Date is required')]"
                :disabled="model.application.applicationType.includes('renew')"
                prepend-inner-icon="mdi-calendar"
                v-bind="attrs"
                v-on="on"
              ></v-text-field>
            </template>
            <v-date-picker
              v-model="
                model.application.qualifyingQuestions.questionTwo.denialDate
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
            :label="$t('Reason for denial')"
            v-model="
              model.application.qualifyingQuestions.questionTwo.denialReason
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-text-field>
        </v-col>
      </v-row>

      <v-row class="ml-5">
        <v-col
          cols="12"
          lg="4"
          class="text-left"
        >
          {{ $t('QUESTION-THREE') }}
        </v-col>
        <v-col
          cols="12"
          lg="4"
        >
          <v-radio-group
            v-model="
              model.application.qualifyingQuestions.questionThree.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionThree.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        <v-col
          cols="12"
          lg="4"
        >
          <v-radio-group
            v-model="
              model.application.qualifyingQuestions.questionThree.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionThree.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionThree.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionThree.explanation
                .length >
              config.appConfig.questions.three - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.three"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionThree.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionThree.explanation
                .length >
              config.appConfig.questions.three - 20
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
              model.application.qualifyingQuestions.questionFour.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionFour.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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

      <v-row v-if="model.application.qualifyingQuestions.questionFour.selected">
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionFour.explanation
                .length >
              config.appConfig.questions.four - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.four"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionFour.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>

          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionFour.explanation
                .length >
              config.appConfig.questions.four - 20
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
              model.application.qualifyingQuestions.questionFive.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionFive.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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

      <v-row v-if="model.application.qualifyingQuestions.questionFive.selected">
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :maxlength="config.appConfig.questions.five"
            :color="
              model.application.qualifyingQuestions.questionFive.explanation
                .length >
              config.appConfig.questions.five - 20
                ? 'warning'
                : ''
            "
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionFive.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionFive.explanation
                .length >
              config.appConfig.questions.five - 20
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
            v-model="model.application.qualifyingQuestions.questionSix.selected"
            :rules="[
              model.application.qualifyingQuestions.questionSix.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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

      <v-row v-if="model.application.qualifyingQuestions.questionSix.selected">
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionSix.explanation
                .length >
              config.appConfig.questions.six - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.six"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionSix.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionSix.explanation
                .length >
              config.appConfig.questions.six - 20
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
              model.application.qualifyingQuestions.questionSeven.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionSeven.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionSeven.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionSeven.explanation
                .length >
              config.appConfig.questions.seven - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.seven"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionSeven.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>

          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionSeven.explanation
                .length >
              config.appConfig.questions.seven - 20
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
            v-model="
              model.application.qualifyingQuestions.questionEight.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionEight.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionEight.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionEight.explanation
                .length >
              config.appConfig.questions.eight - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.eight"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionEight.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionEight.explanation
                .length >
              config.appConfig.questions.eight - 20
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
              model.application.qualifyingQuestions.questionNine.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionNine.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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

      <v-row v-if="model.application.qualifyingQuestions.questionNine.selected">
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionNine.explanation
                .length >
              config.appConfig.questions.nine - 2
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.nine"
            :label="
              $t(
                'Please explain including the date, agency, charges and disposition.'
              )
            "
            v-model="
              model.application.qualifyingQuestions.questionNine.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>

          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionNine.explanation
                .length >
              config.appConfig.questions.nine - 20
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
            v-model="model.application.qualifyingQuestions.questionTen.selected"
            :rules="[
              model.application.qualifyingQuestions.questionTen.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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

      <v-row v-if="model.application.qualifyingQuestions.questionTen.selected">
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionTen.explanation
                .length >
              config.appConfig.questions.ten - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.ten"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionTen.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
            <v-alert
              outlined
              type="warning"
              v-if="
                model.application.qualifyingQuestions.questionTen.explanation
                  .length >
                config.appConfig.questions.ten - 20
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
              model.application.qualifyingQuestions.questionEleven.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionEleven.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionEleven.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionEleven.explanation
                .length >
              config.appConfig.questions.eleven - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.eleven"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionEleven.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionEleven.explanation
                .length >
              config.appConfig.questions.eleven - 20
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
            :rules="[
              model.application.qualifyingQuestions.questionTwelve.selected !==
                null,
            ]"
            v-model="
              model.application.qualifyingQuestions.questionTwelve.selected
            "
            @change="handleChangeQuestionTwelve"
            row
            :disabled="model.application.applicationType.includes('renew')"
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

      <template
        v-if="model.application.qualifyingQuestions.questionTwelve.selected"
      >
        <v-row
          v-for="index of model.application.qualifyingQuestions.questionTwelve
            .trafficViolations.length"
          :key="index"
        >
          <v-col>
            <v-menu
              v-model="menu[index]"
              :close-on-content-click="false"
              transition="scale-transition"
              offset-y
              min-width="auto"
            >
              <template #activator="{ on, attrs }">
                <v-text-field
                  v-model="
                    model.application.qualifyingQuestions.questionTwelve
                      .trafficViolations[index - 1].date
                  "
                  :label="$t('Date')"
                  :rules="[v => !!v || $t('Date is required')]"
                  :disabled="
                    model.application.applicationType.includes('renew')
                  "
                  outlined
                  hint="YYYY-MM-DD format"
                  prepend-inner-icon="mdi-calendar"
                  v-bind="attrs"
                  v-on="on"
                ></v-text-field>
              </template>
              <v-date-picker
                v-model="
                  model.application.qualifyingQuestions.questionTwelve
                    .trafficViolations[index - 1].date
                "
                color="primary"
                no-title
                scrollable
              >
              </v-date-picker>
            </v-menu>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                model.application.qualifyingQuestions.questionTwelve
                  .trafficViolations[index - 1].violation
              "
              outlined
              label="Violation/Accident"
              :rules="[v => !!v || $t('Violation is required')]"
              :disabled="model.application.applicationType.includes('renew')"
            ></v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                model.application.qualifyingQuestions.questionTwelve
                  .trafficViolations[index - 1].agency
              "
              :rules="[v => !!v || $t('Agency is required')]"
              :disabled="model.application.applicationType.includes('renew')"
              outlined
              label="Agency"
            ></v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                model.application.qualifyingQuestions.questionTwelve
                  .trafficViolations[index - 1].citationNumber
              "
              :rules="[v => !!v || $t('Citation number is required')]"
              :disabled="model.application.applicationType.includes('renew')"
              outlined
              label="Citation Number"
              hint="If unknown please enter unknown"
            ></v-text-field>
          </v-col>
        </v-row>
      </template>

      <v-row
        v-if="model.application.qualifyingQuestions.questionTwelve.selected"
      >
        <v-col>
          <v-btn
            @click="addTrafficViolation"
            color="primary"
            class="mr-3"
          >
            <v-icon left>mdi-plus</v-icon>Add
          </v-btn>
          <v-btn
            @click="removeTrafficViolation"
            color="primary"
          >
            <v-icon left>mdi-minus</v-icon>Remove
          </v-btn>
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
              model.application.qualifyingQuestions.questionThirteen.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionThirteen
                .selected !== null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionThirteen.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionThirteen.explanation
                .length >
              config.appConfig.questions.thirteen - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.thirteen"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionThirteen.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionThirteen.explanation
                .length >
              config.appConfig.questions.thirteen - 20
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
              model.application.qualifyingQuestions.questionFourteen.selected
            "
            row
            :rules="[
              model.application.qualifyingQuestions.questionFourteen
                .selected !== null,
            ]"
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionFourteen.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionFourteen.explanation
                .length >
              config.appConfig.questions.fourteen - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.fourteen"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionFourteen.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionFourteen.explanation
                .length >
              config.appConfig.questions.fourteen - 20
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
              model.application.qualifyingQuestions.questionFifteen.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionFifteen.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionFifteen.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionFifteen.explanation
                .length >
              config.appConfig.questions.fifteen - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.fifteen"
            :label="
              $t(
                'Please explain including the date, agency, charges, and disposition.'
              )
            "
            v-model="
              model.application.qualifyingQuestions.questionFifteen.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionFifteen.explanation
                .length >
              config.appConfig.questions.fifteen - 20
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
              model.application.qualifyingQuestions.questionSixteen.selected !==
                null,
            ]"
            v-model="
              model.application.qualifyingQuestions.questionSixteen.selected
            "
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionSixteen.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionSixteen.explanation
                .length >
              config.appConfig.questions.sixteen - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.sixteen"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionSixteen.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionSixteen.explanation
                .length >
              config.appConfig.questions.sixteen - 20
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
              model.application.qualifyingQuestions.questionSeventeen.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionSeventeen
                .selected !== null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionSeventeen.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionSeventeen
                .explanation.length >
              config.appConfig.questions.seventeen - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.seventeen"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionSeventeen
                .explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionSeventeen
                .explanation.length >
              config.appConfig.questions.seventeen - 20
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
          {{ $t('QUESTION-EIGHTEEN') }}
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-radio-group
            v-model="
              model.application.qualifyingQuestions.questionEighteen.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionEighteen
                .selected !== null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionEighteen.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionEighteen.explanation
                .length >
              config.appConfig.questions.eighteen - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.eighteen"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionEighteen.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionEighteen.explanation
                .length >
              config.appConfig.questions.eighteen - 20
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
          {{ $t('QUESTION-NINETEEN') }}
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-radio-group
            v-model="
              model.application.qualifyingQuestions.questionNineteen.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionNineteen
                .selected !== null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionNineteen.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionNineteen.explanation
                .length >
              config.appConfig.questions.nineteen - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.nineteen"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionNineteen.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionNineteen.explanation
                .length >
              config.appConfig.questions.nineteen - 20
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
          {{ $t('QUESTION-TWENTY') }}
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-radio-group
            v-model="
              model.application.qualifyingQuestions.questionTwenty.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionTwenty.selected !==
                null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionTwenty.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionTwenty.explanation
                .length >
              config.appConfig.questions.twenty - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.twenty"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionTwenty.explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionTwenty.explanation
                .length >
              config.appConfig.questions.twenty - 20
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
          {{ $t('QUESTION-TWENTYONE') }}
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-radio-group
            v-model="
              model.application.qualifyingQuestions.questionTwentyOne.selected
            "
            :rules="[
              model.application.qualifyingQuestions.questionTwentyOne
                .selected !== null,
            ]"
            row
            :disabled="model.application.applicationType.includes('renew')"
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
        v-if="model.application.qualifyingQuestions.questionTwentyOne.selected"
      >
        <v-col class="mx-8">
          <v-textarea
            outlined
            counter
            :color="
              model.application.qualifyingQuestions.questionTwentyOne
                .explanation.length >
              config.appConfig.questions.twentyone - 20
                ? 'warning'
                : ''
            "
            :maxlength="config.appConfig.questions.twentyone"
            :label="$t('Please explain')"
            v-model="
              model.application.qualifyingQuestions.questionTwentyOne
                .explanation
            "
            :rules="[v => !!v || $t('Field cannot be blank')]"
            :disabled="model.application.applicationType.includes('renew')"
          >
          </v-textarea>
          <v-alert
            outlined
            type="warning"
            v-if="
              model.application.qualifyingQuestions.questionTwentyOne
                .explanation.length >
              config.appConfig.questions.twentyone - 20
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
      @continue="handleContinue"
      @save="handleSave"
    />

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
import { computed, onMounted, reactive, ref, watch } from 'vue'

interface IProps {
  value: CompleteApplication
}

const props = defineProps<IProps>()
const emit = defineEmits([
  'input',
  'handle-continue',
  'handle-save',
  'update-step-seven-valid',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const menu = ref([false])
const form = ref()
const snackbar = ref(false)
const valid = ref(false)
const config = useAppConfigStore()
const state = reactive({
  menu: false,
})

onMounted(() => {
  if (form.value) {
    form.value.validate()
  }
})

function handleContinue() {
  emit('update-step-seven-valid', true)
  emit('handle-continue')
}

function handleSave() {
  emit('update-step-seven-valid', true)
  emit('handle-save')
}

function addTrafficViolation() {
  menu.value.push(false)
  model.value.application.qualifyingQuestions.questionTwelve.trafficViolations.push(
    {
      date: '',
      violation: '',
      agency: '',
      citationNumber: '',
    }
  )
}

function removeTrafficViolation() {
  menu.value.pop()
  model.value.application.qualifyingQuestions.questionTwelve.trafficViolations.pop()
}

function handleChangeQuestionTwelve() {
  if (model.value.application.qualifyingQuestions.questionTwelve.selected) {
    model.value.application.qualifyingQuestions.questionTwelve.trafficViolations.push(
      {
        date: '',
        violation: '',
        agency: '',
        citationNumber: '',
      }
    )
  } else {
    model.value.application.qualifyingQuestions.questionTwelve.trafficViolations =
      []
  }
}

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-seven-valid', newValue)
  }
})
</script>

<style>
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
