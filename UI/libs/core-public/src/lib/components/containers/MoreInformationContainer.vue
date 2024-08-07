// eslint-disable vue/no-mutating-props */
<template>
  <v-container>
    <v-card>
      <v-card-title>Privacy Policy</v-card-title>

      <v-card-text>
        We respect and are committed to protecting your privacy. We may collect
        personally identifiable information when you visit our site. We also
        automatically receive and record information on our server logs from
        your browser including your IP address, cookie information and the
        page(s) you visited. We will not sell your personally identifiable
        information to anyone.
      </v-card-text>

      <v-card-title>Security Policy</v-card-title>

      <v-card-text>
        Your payment and personal information is always safe. Our Secure Sockets
        Layer (SSL) software is the industry standard and among the best
        software available today for secure commerce transactions. It encrypts
        all of your personal information, including credit card number, name,
        and address, so that it cannot be read over the internet.
      </v-card-text>

      <v-card-title>Refund Policy</v-card-title>

      <v-card-text>
        All refunds will be provided as a credit to the credit card used at the
        time of purchase within five (5) business days.
      </v-card-text>

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
        {{ $t('application.pageOne.whoShallBeIssued') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.pageOne.whoShallBeIssuedSectionParagraphOne') }}
      </v-card-text>

      <v-card-text>
        {{ $t('application.pageOne.whoShallBeIssuedSectionParagraphTwo') }}
      </v-card-text>

      <v-card-text>
        {{ $t('application.pageOne.whoShallBeIssuedSectionParagraphThree') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.pageOne.disqualifiedPersons') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.pageOne.disqualifiedPersonsSection') }}
        <ol class="mt-2">
          <li>
            {{ $t('application.pageOne.disqualifiedPersonsSectionBulletOne') }}
          </li>
          <li>
            {{ $t('application.pageOne.disqualifiedPersonsSectionBulletTwo') }}
          </li>
          <li>
            {{
              $t('application.pageOne.disqualifiedPersonsSectionBulletThree')
            }}
          </li>
          <li>
            {{ $t('application.pageOne.disqualifiedPersonsSectionBulletFour') }}
          </li>
          <li>
            {{ $t('application.pageOne.disqualifiedPersonsSectionBulletFive') }}
          </li>
          <li>
            {{ $t('application.pageOne.disqualifiedPersonsSectionBulletSix') }}
          </li>
          <li>
            {{
              $t('application.pageOne.disqualifiedPersonsSectionBulletSeven')
            }}
          </li>
          <li>
            {{
              $t('application.pageOne.disqualifiedPersonsSectionBulletEight')
            }}
          </li>
          <li>
            {{ $t('application.pageOne.disqualifiedPersonsSectionBulletNine') }}
          </li>
          <li>
            {{ $t('application.pageOne.disqualifiedPersonsSectionBulletTen') }}
          </li>
        </ol>
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
        <br />
        <br />
        {{ $t('application.pageOne.trainingRequiredSectionParagraphThree') }}
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
      </v-card-text>

      <v-card-text>
        {{ $t('application.pageTwo.sectionOneParagraphTwo') }}
      </v-card-text>

      <v-card-text>
        {{ $t('application.pageTwo.sectionOneParagraphThree') }}
      </v-card-text>

      <v-card-text>
        {{ $t('application.pageTwo.sectionOneParagraphFour') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.sectionFive.Title') }}
      </v-card-title>

      <v-card-text>
        {{ $t('application.sectionFive.ParagraphOne') }}
        <br />
        <br />
        {{ $t('application.sectionFive.ParagraphTwo') }}
        <br />
        <br />
        {{ $t('application.sectionFive.ParagraphThree') }}
        <ul class="my-2">
          <li>{{ $t('application.sectionFive.BulletOne') }}</li>
          <li>{{ $t('application.sectionFive.BulletTwo') }}</li>
          <li>{{ $t('application.sectionFive.BulletThree') }}</li>
          <li>{{ $t('application.sectionFive.BulletFour') }}</li>
          <li>{{ $t('application.sectionFive.BulletFive') }}</li>
          <li>{{ $t('application.sectionFive.BulletSix') }}</li>
          <li>{{ $t('application.sectionFive.BulletSeven') }}</li>
          <li>{{ $t('application.sectionFive.BulletEight') }}</li>
          <li>{{ $t('application.sectionFive.BulletNine') }}</li>
          <li>{{ $t('application.sectionFive.BulletTen') }}</li>
          <li>{{ $t('application.sectionFive.BulletEleven') }}</li>
          <li>{{ $t('application.sectionFive.BulletTwelve') }}</li>
        </ul>
        {{ $t('application.sectionFive.ParagraphFour') }}
        <br />
        <br />
        {{ $t('application.sectionFive.ParagraphFive') }}
      </v-card-text>

      <v-card-title>
        {{ $t('application.sectionSix.Title') }}
      </v-card-title>

      <v-card-text>
        <strong>{{ $t('application.sectionSix.paragraphOne') }}</strong>
        <br />
        <br />
        <strong>{{ $t('application.sectionSix.penalCodeOne.title') }}</strong>
        <br />
        <ol>
          {{
            $t('application.sectionSix.penalCodeOne.a.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeOne.b.text')
          }}
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeOne.b.1') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeOne.b.2') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeOne.b.3') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeOne.b.4') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeOne.b.5') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeOne.b.6') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeOne.b.7') }}</li>
          </ol>
        </ol>
        <br />
        <br />
        <strong>{{ $t('application.sectionSix.penalCodeTwo.title') }}</strong>
        <br />
        {{ $t('application.sectionSix.penalCodeTwo.text') }}
        <ol class="mt-2">
          {{
            $t('application.sectionSix.penalCodeTwo.a.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeTwo.b.text')
          }}
        </ol>
        <br />
        <br />
        <strong>{{ $t('application.sectionSix.penalCodeThree.title') }}</strong>
        <br />
        {{ $t('application.sectionSix.penalCodeThree.text') }}
        <ol class="mt-2">
          <li>{{ $t('application.sectionSix.penalCodeThree.1') }}</li>
          <li>{{ $t('application.sectionSix.penalCodeThree.2') }}</li>
          <li>{{ $t('application.sectionSix.penalCodeThree.3') }}</li>
          <li>{{ $t('application.sectionSix.penalCodeThree.4') }}</li>
        </ol>
        <br />
        <br />
        <strong>{{ $t('application.sectionSix.penalCodeFour.title') }}</strong>
        <br />
        {{ $t('application.sectionSix.penalCodeFour.text') }}
        <br />
        <br />
        <br />
        <strong>{{ $t('application.sectionSix.penalCodeFive.title') }}</strong>
        <br />
        <ol>
          {{
            $t('application.sectionSix.penalCodeFive.a.text')
          }}
          <br />
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeFive.a.1') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeFive.a.2') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeFive.a.3') }}</li>
          </ol>
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeFive.b.text')
          }}
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeFive.b.1') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeFive.b.2') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeFive.b.3') }}</li>
          </ol>
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeFive.c.text')
          }}
        </ol>
        <br />
        <br />
        <strong>
          {{ $t('application.sectionSix.penalCodeSix.title') }}
        </strong>
        <br />
        {{ $t('application.sectionSix.penalCodeSix.text') }}
        <ol>
          {{
            $t('application.sectionSix.penalCodeSix.a')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSix.b')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSix.c')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSix.d')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSix.e')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSix.f')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSix.g')
          }}
        </ol>
        <br />
        <br />
        <strong>
          {{ $t('application.sectionSix.penalCodeSeven.title') }}
        </strong>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSeven.a.text')
          }}
          <br />
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeSeven.a.1') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeSeven.a.2') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeSeven.a.3') }}</li>
          </ol>
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSeven.b.text')
          }}
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeSeven.b.1') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeSeven.b.2') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeSeven.b.3') }}</li>
          </ol>
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSeven.c.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeSeven.d.text')
          }}
        </ol>
        <br />
        <br />
        <strong>
          {{ $t('application.sectionSix.penalCodeEight.title') }}
        </strong>
        <ol>
          {{
            $t('application.sectionSix.penalCodeEight.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeEight.a')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeEight.b')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeEight.c')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeEight.d')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeEight.e')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeEight.f')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeEight.g')
          }}
        </ol>
        <br />
        <br />
        <strong>
          {{ $t('application.sectionSix.penalCodeNine.title') }}
        </strong>
        <br />
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.a.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.b.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.c.text')
          }}
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeNine.c.1') }}</li>
            <li>
              {{ $t('application.sectionSix.penalCodeNine.c.2.lineOne') }}
              <br />{{ $t('application.sectionSix.penalCodeNine.c.2.lineTwo') }}
            </li>
            <li>{{ $t('application.sectionSix.penalCodeNine.c.3') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeNine.c.4') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeNine.c.5') }}</li>
          </ol>
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.d.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.e.text')
          }}
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeNine.e.1') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeNine.e.2') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeNine.e.3') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeNine.e.4') }}</li>
          </ol>
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.f.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.g.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.h.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.i.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeNine.j.text')
          }}
        </ol>
        <br />
        <br />
        <strong>
          {{ $t('application.sectionSix.penalCodeTen.title') }}
        </strong>
        <br />
        <ol>
          {{
            $t('application.sectionSix.penalCodeTen.a.text')
          }}
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.1') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.2') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.3') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.4') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.5') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.6') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.7') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.8') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.9') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.10') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.11') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.12') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.13') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.14') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.15') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.16') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.17') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.18') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.19') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.20') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.21') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.22') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.23') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.24') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.25') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.26') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.27') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.28') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.a.29') }}</li>
          </ol>
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeTen.b.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeTen.c.text')
          }}
          <ol>
            <li>{{ $t('application.sectionSix.penalCodeTen.c.1') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.c.2') }}</li>
            <li>{{ $t('application.sectionSix.penalCodeTen.c.3') }}</li>
          </ol>
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeTen.d.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeTen.e.text')
          }}
        </ol>
        <ol>
          {{
            $t('application.sectionSix.penalCodeTen.f.text')
          }}
        </ol>
      </v-card-text>
      <v-card-title>
        {{ $t('application.sectionSeven.Title') }}
      </v-card-title>
      <v-card-text>
        {{ $t('application.sectionSeven.paragraphOne') }}
        <br />
        <br />
        {{ $t('application.sectionSeven.paragraphTwo') }}
        <br />
        <br />
        {{ $t('application.sectionSeven.paragraphThree') }}
        <br />
        <br />
        {{ $t('application.sectionSeven.paragraphFour') }}
        <br />
        <br />
        {{ $t('application.sectionSeven.paragraphFive') }}
        <br />
        <br />
        <v-row
          v-if="!props.informationOnly"
          justify="center"
        >
          <v-card-title style="padding-bottom: 1px; padding-left: 20px">
            {{ $t('Please Sign Here') }}
          </v-card-title>
          <v-col
            cols="12"
            class="d-flex align-center justify-center"
          >
            <v-card
              light
              flat
              :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
              height="100px"
              outlined
              style="border: solid 2px black"
            >
              <canvas
                :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
                height="100px"
                id="signature"
                class="signature"
              ></canvas>
            </v-card>
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

      <v-card-actions v-if="props.informationOnly">
        <v-btn
          @click="$router.go(-1)"
          color="primary"
        >
          Back
        </v-btn>
      </v-card-actions>
    </v-card>

    <v-snackbar
      v-if="!props.informationOnly"
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

interface IMoreInformationProps {
  informationOnly: boolean
}

const props = withDefaults(defineProps<IMoreInformationProps>(), {
  informationOnly: true,
})

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
  completeApplicationStore.completeApplication = defaultPermitState
  completeApplicationStore.completeApplication.application.cost = {
    new: {
      standard: brandStore.brand.cost.new.standard,
      judicial: brandStore.brand.cost.new.judicial,
      reserve: brandStore.brand.cost.new.reserve,
      employment: brandStore.brand.cost.new.employment,
    },
    renew: {
      standard: brandStore.brand.cost.renew.standard,
      judicial: brandStore.brand.cost.renew.judicial,
      reserve: brandStore.brand.cost.renew.reserve,
      employment: brandStore.brand.cost.renew.employment,
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
ol {
  margin-left: 10px;
  padding-left: 1em;
  text-indent: -1.6em;
  counter-reset: item;
}
ol li {
  list-style: none;
  position: relative;
}
ol > li:before {
  counter-increment: item;
  content: '(' counter(item) ') ';
}
</style>
