// eslint-disable vue/no-mutating-props */
<template>
  <v-container fluid>
    <v-subheader class="sub-header font-weight-bold mt-5">
      <h1 class="text-md-h5">
        {{ $t('Concealed Carry Weapon License Requirements:') }}
      </h1>
    </v-subheader>

    <v-carousel
      class="text-carousel mt-3"
      hide-delimiters
      hide-delimiter-background
      v-model="useAppInfoStepStore.step"
    >
      <v-carousel-item class="text-section">
        <v-card class="text-left">
          <v-card-title>
            {{ $t('Completing the Application') }}
          </v-card-title>
          <v-card-text>
            {{ $t('COMPLETE-BODY') }}
          </v-card-text>
          <v-card-text>
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
              <li>
                {{ $t('COMPLETE-LIST-FOUR') }}
              </li>
            </ul>
          </v-card-text>
          <v-card-text>
            {{ $t('COMPLETE-ENDING') }}
            <strong class="red--text text--darken-2">
              {{ $t('COMPLETE-WARNING') }}</strong
            >
          </v-card-text>
        </v-card>
      </v-carousel-item>

      <!-- DMV -->
      <v-carousel-item class="text-section">
        <v-card class="text-left">
          <v-card-title>
            {{ $t('DMV') }}
          </v-card-title>
          <v-card-text>
            {{ $t('DMV-BODY') }}
          </v-card-text>
        </v-card>
      </v-carousel-item>

      <!-- Military -->
      <v-carousel-item class="text-section">
        <v-card class="text-left">
          <v-card-title>
            {{ $t('Military') }}
          </v-card-title>
          <v-card-text>
            {{ $t('MILITARY-BODY') }}
          </v-card-text>
        </v-card>
      </v-carousel-item>

      <!-- US residency -->
      <v-carousel-item class="text-section">
        <v-card class="text-left">
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
        </v-card>
      </v-carousel-item>

      <!-- Proof of Residency -->
      <v-carousel-item class="text-section">
        <v-card class="text-left">
          <v-card-title>
            {{ $t(' Proof of Residency  ') }}
          </v-card-title>
          <v-card-text>
            {{ $t('PROOF-RESIDENCY') }}
          </v-card-text>
          <v-card-text>
            <ul>
              <li>
                {{ $t('PROOF-LIST-ONE') }}
              </li>
              <li>
                {{ $t('PROOF-LIST-TWO') }}
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
        </v-card>
      </v-carousel-item>

      <!-- Firearms -->
      <v-carousel-item>
        <v-card class="text-left">
          <v-card-title>
            {{ $t('Firearms') }}
          </v-card-title>
          <v-card-text>
            {{ $t('FIREARMS-BODY') }}
          </v-card-text>
        </v-card>
      </v-carousel-item>
    </v-carousel>

    <v-container class="">
      <v-sheet class="rounded">
        <v-subheader class="sub-header font-weight-bold">
          {{ $t('Applications:') }}
        </v-subheader>
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
        </v-card>
      </v-sheet>
    </v-container>

    <v-container class="p-4">
      <v-sheet class="rounded">
        <v-subheader class="sub-header font-weight-bold">
          {{ $t('FMI') }} {{ $t('CONTACT') }}
        </v-subheader>
        <v-card class="contact-card text-left">
          <v-card-title>
            {{ brand.agencyName }}
          </v-card-title>
          <v-card-text>
            <strong>{{ $t(' Address: ') }}</strong>
            {{ brand.agencyAddress }}
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
      </v-sheet>
    </v-container>
  </v-container>
</template>

<script setup lang="ts">
import { useAppInfoStep } from '@core-public/stores/appInfoStepStore';
import { useBrandStore } from '@shared-ui/stores/brandStore';

const useAppInfoStepStore = useAppInfoStep();
const { brand } = useBrandStore();
</script>

<style lang="scss" scoped>
.text {
  &-section {
    display: flex;
    justify-content: center;
    align-content: center;
    width: 100%;
    padding: 0 4rem;
  }
}
.contact-card {
  max-width: 50%;
  margin-left: 0.5rem;
}
</style>
