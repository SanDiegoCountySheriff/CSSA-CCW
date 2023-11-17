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
        {{ $t('application.sectionFive.penalCodeOne.a.text') }}
        <br />
        {{ $t('application.sectionFive.penalCodeOne.b.text') }}
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
        {{ $t('application.sectionFive.penalCodeTwo.a.text') }}
        <br />
        {{ $t('application.sectionFive.penalCodeTwo.b.text') }}
        <br />
        <br />
        <strong>{{
          $t('application.sectionFive.penalCodeThree.title')
        }}</strong>
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
        {{ $t('application.sectionFive.penalCodeSix.a.text') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeSix.a.1') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeSix.a.2') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeSix.a.3') }}
        <br />
        {{ $t('application.sectionFive.penalCodeSix.b.text') }}
        <br />
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
        <br />
        {{ $t('application.sectionFive.penalCodeSeven.text') }}
        <br />
        {{ $t('application.sectionFive.penalCodeSeven.a') }}
        <br />
        {{ $t('application.sectionFive.penalCodeSeven.b') }}
        <br />
        {{ $t('application.sectionFive.penalCodeSeven.c') }}
        <br />
        {{ $t('application.sectionFive.penalCodeSeven.d') }}
        <br />
        {{ $t('application.sectionFive.penalCodeSeven.e') }}
        <br />
        {{ $t('application.sectionFive.penalCodeSeven.f') }}
        <br />
        {{ $t('application.sectionFive.penalCodeSeven.g') }}
        <br />
        <br />
        <strong>
          {{ $t('application.sectionFive.penalCodeEight.title') }}
        </strong>
        <br />
        {{ $t('application.sectionFive.penalCodeEight.a.text') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeEight.a.1') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeEight.a.2') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeEight.a.3') }}
        <br />
        {{ $t('application.sectionFive.penalCodeEight.b.text') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeEight.b.1') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeEight.b.2') }}
        <br />
        &emsp;{{ $t('application.sectionFive.penalCodeEight.b.3') }}
        <br />
        {{ $t('application.sectionFive.penalCodeEight.c.text') }}
        <br />
        {{ $t('application.sectionFive.penalCodeEight.d.text') }}
        <br />
        <br />
        <strong>
          {{ $t('application.sectionFive.penalCodeNine.title') }}
        </strong>
        <br />
        {{ $t('application.sectionFive.penalCodeNine.text') }}
        <br />
        {{ $t('application.sectionFive.penalCodeNine.a') }}
        <br />
        {{ $t('application.sectionFive.penalCodeNine.b') }}
        <br />
        {{ $t('application.sectionFive.penalCodeNine.c') }}
        <br />
        {{ $t('application.sectionFive.penalCodeNine.d') }}
        <br />
        {{ $t('application.sectionFive.penalCodeNine.e') }}
        <br />
        {{ $t('application.sectionFive.penalCodeNine.f') }}
        <br />
        {{ $t('application.sectionFive.penalCodeNine.g') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.sectionSix.Title') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.sectionSix.paragraphOne') }}
        <br />
        <br />
        {{ $t('application.sectionSix.paragraphTwo') }}
        <br />
        <br />
        {{ $t('application.sectionSix.paragraphThree') }}
        <br />
        <br />
        {{ $t('application.sectionSix.paragraphFour') }}
        <br />
        <br />
        {{ $t('application.sectionSix.paragraphFive') }}
        <br />
        <br />
        <v-row justify="center">
          <v-card-title style="padding-bottom: 1px; padding-left: 20px">
            {{ $t('Please Sign Here') }}
          </v-card-title>
          <v-col
            cols="12"
            class="text-center"
          >
            <canvas
              :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
              id="signature"
              class="signature"
            ></canvas>
          </v-col>
          <v-col
            cols="12"
            class="text-center"
          >
            <v-btn
              @click="handleClearSignature"
              :disabled="isLoading"
              color="primary"
              class="mr-4"
              text
            >
              {{ $t('Clear Signature') }}
            </v-btn>
            <v-btn
              @click="handleFinalAcceptAndCreate"
              :loading="isLoading"
              :disabled="isSignaturePadEmpty"
              color="primary"
              class="ml-4"
              text
            >
              {{ $t('Accept') }}
            </v-btn>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>

    <!-- <v-card class="my-5 pb-3">
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
    </v-card> -->
    <v-snackbar
      v-model="snackbar"
      color="primary"
      timeout="-1"
      top
      app
    >
      Please read and accept at the bottom

      <template #action="{ attrs }">
        <v-btn
          v-bind="attrs"
          @click="snackbar = false"
          color="white"
          text
          icon
        >
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import Routes from '@core-public/router/routes'
import SignaturePad from 'signature_pad'
import { defaultPermitState } from '@shared-utils/lists/defaultConstants'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { computed, nextTick, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router/composables'

const snackbar = ref(true)
const signaturePad = ref<SignaturePad>()
const brandStore = useBrandStore()
const authStore = useAuthStore()
const completeApplicationStore = useCompleteApplicationStore()
const router = useRouter()
const route = useRoute()
const isSignaturePadEmpty = computed(() => {
  return signaturePad.value?.isEmpty()
})

onMounted(() => {
  nextTick(() => {
    const canvas = document.getElementById('signature') as HTMLCanvasElement

    signaturePad.value = new SignaturePad(canvas, {
      backgroundColor: 'rgba(255, 255, 255, 0)',
    })
  })
})

const { isLoading, mutate: createMutation } = useMutation({
  mutationFn: completeApplicationStore.createApplication,
  onSuccess: () => {
    router.push({
      path: `${Routes.FORM_ROUTE_PATH}?orderId=${route.params}`,
      query: {
        applicationId: completeApplicationStore.completeApplication.id,
        isComplete:
          completeApplicationStore.completeApplication.application.isComplete.toString(),
      },
    })
  },
  onError: () => null,
})

function handleFinalAcceptAndCreate() {
  completeApplicationStore.setCompleteApplication(defaultPermitState)
  completeApplicationStore.completeApplication.application.cost = {
    new: {
      standard: brandStore.brand.cost.new.standard,
      judicial: brandStore.brand.cost.new.judicial,
      reserve: brandStore.brand.cost.new.reserve,
    },
    renew: {
      standard: brandStore.brand.cost.renew.standard,
      judicial: brandStore.brand.cost.renew.judicial,
      reserve: brandStore.brand.cost.renew.reserve,
    },
    issuance: brandStore.brand.cost.issuance,
    modify: brandStore.brand.cost.modify,
    creditFee: brandStore.brand.cost.creditFee,
    convenienceFee: brandStore.brand.cost.convenienceFee,
  }
  completeApplicationStore.completeApplication.application.userEmail =
    authStore.auth.userEmail
  completeApplicationStore.completeApplication.id = window.crypto.randomUUID()
  createMutation()
}

function handleClearSignature() {
  signaturePad.value?.clear()
}
</script>

<style lang="scss" scoped>
.signature {
  border: 2px solid black;
  border-radius: 5px;
}
</style>
