// eslint-disable vue/no-mutating-props */
<template>
  <v-container>
    <v-card>
      <v-card-title>
        {{ $t('Concealed Carry Weapon License Requirements:') }}
      </v-card-title>

      <v-card-title>
        {{ $t('application.pageOne.authority') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.pageOne.authoritySection') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.pageOne.whoMayBeIssued') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.pageOne.whoMayBeIssuedSection') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.pageOne.formatOfCCWLicense') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.pageOne.formatOfCCWLicenseSection') }}
        <ol class="mt-2">
          <li>
            {{ $t('application.pageOne.formatOfCCWLicenseSectionBulletOne') }}
          </li>
          <li>
            {{ $t('application.pageOne.formatOfCCWLicenseSectionBulletTwo') }}
          </li>
        </ol>
      </v-card-text>

      <v-card-title>
        {{ $t('application.pageOne.trainingRequired') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.pageOne.trainingRequiredSectionParagraphOne') }}
        <br />
        <br />
        {{ $t('application.pageOne.trainingRequiredSectionParagraphTwo') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.pageOne.psychologicalTesting') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.pageOne.psychologicalTestingSection') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.pageTwo.sectionOne') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.pageTwo.sectionOneParagraphOne') }}
        <ul class="my-2">
          <li>
            {{ $t('application.pageTwo.sectionOneBulletOne') }}
          </li>
          <li>
            {{ $t('application.pageTwo.sectionOneBulletTwo') }}
          </li>
          <li>
            {{ $t('application.pageTwo.sectionOneBulletThree') }}
          </li>
        </ul>
        {{ $t('application.pageTwo.sectionOneParagraphTwo') }}
        <br />
        <br />
        {{ $t('application.pageTwo.sectionOneParagraphThree') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.sectionFour.Title') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.sectionFour.ParagraphOne') }}
        <br />
        <br />
        {{ $t('application.sectionFour.ParagraphTwo') }}
        <ul class="my-2">
          <li>{{ $t('application.sectionFour.BulletOne') }}</li>
          <li>{{ $t('application.sectionFour.BulletTwo') }}</li>
          <li>{{ $t('application.sectionFour.BulletThree') }}</li>
          <li>{{ $t('application.sectionFour.BulletFour') }}</li>
          <li>{{ $t('application.sectionFour.BulletFive') }}</li>
          <li>{{ $t('application.sectionFour.BulletSix') }}</li>
          <li>{{ $t('application.sectionFour.BulletSeven') }}</li>
          <li>{{ $t('application.sectionFour.BulletEight') }}</li>
          <li>{{ $t('application.sectionFour.BulletNine') }}</li>
        </ul>
        {{ $t('application.sectionFour.ParagraphThree') }}
        <br />
        <br />
        {{ $t('application.sectionFour.ParagraphFour') }}
        <br />
        <br />
        {{ $t('application.sectionFour.ParagraphFive') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.sectionFive.Title') }}
      </v-card-title>

      <v-card-text>
        <strong>{{ $t('application.sectionFive.paragraphOne') }}</strong>
        <br />
        <br />
        <strong>{{ $t('application.sectionFive.penalCodeOne.title') }}</strong>
        <br />
        {{ $t('application.sectionFive.penalCodeOne.a') }}
        <br />
        {{ $t('application.sectionFive.penalCodeOne.b') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeOne.b.1') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeOne.b.2') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeOne.b.3') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeOne.b.4') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeOne.b.5') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeOne.b.6') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeOne.b.7') }}
        <br />
        <br />
        <strong>{{ $t('application.sectionFive.penalCodeTwo.title') }}</strong>
        <br />
        {{ $t('application.sectionFive.penalCodeTwo.text') }}
        <br />
        {{ $t('application.sectionFive.penalCodeTwo.a') }}
        <br />
        {{ $t('application.sectionFive.penalCodeTwo.b') }}
        <br />
        <br />
        <strong>{{ $t('application.sectionFive.penalCodeThreeTitle') }}</strong>
        <br />
        {{ $t('application.sectionFive.penalCodeThree.text') }}
        <ol class="mt-2">
          <li>{{ $t('application.sectionFive.penalCodeThree.1') }}</li>
          <li>{{ $t('application.sectionFive.penalCodeThree.2') }}</li>
          <li>{{ $t('application.sectionFive.penalCodeThree.3') }}</li>
          <li>{{ $t('application.sectionFive.penalCodeThree.4') }}</li>
        </ol>
        <br />
        <strong>{{ $t('application.sectionFive.penalCodeFour.title') }}</strong>
        <br />
        {{ $t('application.sectionFive.penalCodeFour.text') }}
        <br />
        <br />
        <strong>{{ $t('application.sectionFive.penalCodeFive.title') }}</strong>
        <br />
        {{ $t('application.sectionFive.penalCodeFive.text') }}
        <br />
        <br />
        <strong>{{ $t('application.sectionFive.penalCodeSix.title') }}</strong>
        <br />
        {{ $t('application.sectionFive.penalCodeSix.a') }}
        &emsp;{{ $t('application.sectionFive.penalCodeSix.a.1') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeSix.a.2') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeSix.a.3') }}
        <br />
        <br />
        {{ $t('application.sectionFive.penalCodeSix.b') }}
        &emsp;{{ $t('application.sectionFive.penalCodeSix.b.1') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeSix.b.2') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeSix.b.3') }}
        <br />
        <br />
        <strong>
          {{ $t('application.sectionFive.penalCodeSeven.title') }}
        </strong>
      </v-card-text>

      <!-- <v-card-text>
        <ul>
          <li>
            {{ $t('COMPLETE-LIST-ONE') }}
          </li>
          <li>
            {{ $t('COMPLETE-LIST-TWO') }}
          </li>
          <li>
            {{ $t('COMPLETE-LIST-THREE') }}
          </li>
        </ul>
      </v-card-text>
      <v-card-text>
        {{ $t('COMPLETE-ENDING') }}
        <strong class="red--text text--darken-2">
          {{ $t('COMPLETE-WARNING') }}</strong
        >
      </v-card-text>

      <v-card-title>
        {{ $t('DMV') }}
      </v-card-title>
      <v-card-text>
        {{ $t('DMV-BODY') }}
      </v-card-text>

      <v-card-title>
        {{ $t('Military') }}
      </v-card-title>
      <v-card-text>
        {{ $t('MILITARY-BODY') }}
      </v-card-text>

      <v-card-title>
        {{ $t(' US Residency') }}
      </v-card-title>
      <v-card-text>
        {{ $t('US-RESIDENCY') }}
      </v-card-text>
      <v-card-text>
        <ul>
          <li>
            {{ $t('US-LIST-ONE') }}
          </li>
          <li>
            {{ $t('US-LIST-TWO') }}
          </li>
          <li>
            {{ $t('US-LIST-THREE') }}
          </li>
          <li>
            {{ $t('US-LIST-FOUR') }}
          </li>
        </ul>
      </v-card-text>

      <v-card-title>
        {{ $t(' Proof of Residency  ') }}
      </v-card-title>
      <v-card-text>
        {{ $t('PROOF-RESIDENCE') }}
      </v-card-text>
      <v-card-text>
        <ul>
          <li>
            {{ $t('PROOF-LIST-ONE') }}
          </li>
          <li>
            {{ $t('PROOF-LIST-THREE') }}
          </li>
          <li>
            {{ $t('PROOF-LIST-FOUR') }}
          </li>
          <li>
            {{ $t('PROOF-LIST-FIVE') }}
          </li>
          <li>
            {{ $t('PROOF-LIST-SIX') }}
          </li>
          <li>
            {{ $t('PROOF-LIST-SEVEN') }}
          </li>
        </ul>
      </v-card-text>

      <v-card-title>
        {{ $t('Firearms') }}
      </v-card-title>
      <v-card-text>
        {{ $t('FIREARMS-BODY') }}
      </v-card-text>

      <v-card-title class="sub-header font-weight-bold">
        {{ $t('Applications:') }}
      </v-card-title>
      <v-card class="text-left">
        <v-card-title>
          {{ $t('APPLICATION-INFO-TITLE') }}
        </v-card-title>
        <v-card-text>
          <ul>
            <li>
              {{ $t('NEW-APPLICATION-LIST-ONE') }}
            </li>
            <li>
              {{ $t('NEW-APPLICATION-LIST-TWO') }}
            </li>
            <li>
              {{ $t('NEW-APPLICATION-LIST-THREE') }}
            </li>
            <li>
              {{ $t('NEW-APPLICATION-LIST-FOUR') }}
            </li>
          </ul>
        </v-card-text>
        <v-card-text>
          {{ $t('Please') }} <strong>{{ $t('DO NOT') }}</strong>
          {{ $t('NEW-APPLICATION-LIST-BODY') }}
        </v-card-text>
        <v-card-title>
          {{ $t('NEW-APPLICATION-STEP-TITLE') }}
        </v-card-title>
        <v-card-text>
          <ol>
            <li>
              {{ $t(' Submit a ') }}
              <strong>{{
                $t(
                  ' New Application (click on the appropriate link/type of application you are filing on the left side of the webpage).'
                )
              }}</strong>
              {{
                $t(
                  ' If you select the wrong type of application we cannot convert it over to the proper type for you (your application will be denied and you will have to start the process over).  To submit a New Application, please follow these steps:'
                )
              }}
              <ol type="a">
                <li>
                  {{ $t('NEW-APPLICATION-STEP-A') }}
                </li>
                <li>
                  {{ $t('NEW-APPLICATION-STEP-B') }}
                </li>
                <li>
                  {{ $t('NEW-APPLICATION-STEP-C') }}
                </li>
                <li>
                  {{ $t('NEW-APPLICATION-STEP-D') }}
                </li>
                <li>
                  {{ $t('NEW-APPLICATION-STEP-E') }}
                </li>
              </ol>
            </li>
            <li>
              {{ $t('NEW-APPLICATION-STEP-TWO') }}
            </li>
            <li>
              {{ $t('NEW-APPLICATION-STEP-THREE') }}
            </li>
            <li>
              {{ $t('NEW-APPLICATION-STEP-FOUR') }}
            </li>
          </ol>
        </v-card-text>
        <v-card-title>
          {{ $t('DENIALS: ') }}
        </v-card-title>
        <v-card-text>
          {{ $t('DENIALS-BODY') }}
        </v-card-text>
        <v-card-text class="font-weight-bold">
          {{ $t('RENEWAL-TITLE') }}
        </v-card-text>

        <v-card-text>
          <strong>
            {{ $t('NOTICE-TO-ALL') }}
          </strong>
          {{ $t('CHANGE-OF-ADDRESS') }}

          <strong class="red--text text--darken-2">
            {{ $t('FAILURE-COA') }}
          </strong>
        </v-card-text>

        <v-card-text>
          {{ $t('PRIOR-RENEWAL') }}
          <ul>
            <li>
              {{ $t('PRIOR-RENEWAL-ONE') }}
            </li>
            <li>
              {{ $t('PRIOR-RENEWAL-TWO') }}
            </li>
            <li>
              {{ $t('PRIOR-RENEWAL-THREE') }}
            </li>
            <li>
              {{ $t('PRIOR-RENEWAL-FOUR') }}
            </li>
            <li>
              {{ $t('PRIOR-RENEWAL-FIVE') }}
            </li>
          </ul>
          <strong>{{ $t('PLEASE NOTE:') }}</strong>
          {{ $t('RENEWAL-APPLICANTS') }}
          <br />
          <strong class="red--text text--darken-2 mt-2">
            {{ $t('RENEWAL-WARNING') }}
          </strong>
        </v-card-text>
        <v-card-title>
          {{ $t(' MODIFICATION Application:') }}
        </v-card-title>
        <v-card-text>
          {{ $t('MODIFICATION-BODY') }}
          <br />
          <strong>
            {{ $t('MODIFICATION-PRIOR') }}
          </strong>
          <ul>
            <li>
              {{ $t('MOD-ONE') }}
            </li>
            <li>
              {{ $t('MOD-TWO') }}
            </li>
            <li>
              {{ $t('MOD-THREE') }}
            </li>
          </ul>
          {{ $t('MOD-END') }}
        </v-card-text>
        <v-card-title>
          {{ $t('DUP-TITLE') }}
        </v-card-title>
        <v-card-text>
          {{ $t('DUP-BODY') }}
          <br />
          <strong>
            {{ $t('DUP-STRONG') }}
          </strong>
          <ul>
            <li>
              {{ $t('DUP-ONE') }}
            </li>
            <li>
              {{ $t('DUP-TWO') }}
            </li>
          </ul>
        </v-card-text>
      </v-card> -->
    </v-card>

    <v-card class="my-5 pb-3">
      <v-card-title> {{ $t('FMI') }} {{ $t('CONTACT') }} </v-card-title>
      <v-card>
        <v-card-title>
          {{ brand.agencyName }}
        </v-card-title>
        <v-card-text>
          <strong>{{ $t(' Address: ') }}</strong>
          {{ brand.agencyStreetAddress }}
        </v-card-text>
        <v-card-text>
          <strong>{{ $t(' Telephone: ') }}</strong>
          {{ brand.agencyTelephone }}
        </v-card-text>
        <v-card-text>
          <strong>{{ $t(' Fax: ') }}</strong>
          {{ brand.agencyFax }}
        </v-card-text>
        <v-card-text>
          <strong>{{ $t(' Email: ') }}</strong>
          {{ brand.agencyEmail }}
        </v-card-text>
      </v-card>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { useBrandStore } from '@shared-ui/stores/brandStore'

const { brand } = useBrandStore()
</script>
