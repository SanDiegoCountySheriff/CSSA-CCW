<template>
  <v-container>
    <v-row>
      <v-col align="center">
        <v-card
          v-if="step.index === 0"
          max-width="1000"
          min-height="500"
          class="text-left"
        >
          <v-card-title>{{ $t('Acknowledgements') }}</v-card-title>

          <AcknowledgementInitial
            :handle-accept="handleAccept"
            :handle-decline="handleDecline"
          />
        </v-card>

        <v-card
          v-if="step.index === 1"
          max-width="1000"
          min-height="500"
          class="text-left"
        >
          <v-card-title> {{ $t('acknowledgement-title-part2') }} </v-card-title>

          <AcknowledgementPart
            :handle-accept="handleAccept"
            :handle-decline="handleDecline"
            :text-body="'acknowledgement-part1'"
          />
        </v-card>

        <v-card
          v-if="step.index === 2"
          max-width="1000"
          min-height="500"
          class="text-left"
        >
          <v-card-title> {{ $t('acknowledgement-title-part3') }} </v-card-title>

          <AcknowledgementPart
            :handle-accept="handleAccept"
            :handle-decline="handleDecline"
            :text-body="'acknowledgement-part2'"
          />
        </v-card>

        <v-card
          v-if="step.index === 3"
          max-width="1000"
          min-height="500"
          class="text-left"
        >
          <v-card-title> {{ $t('acknowledgement-title-part4') }} </v-card-title>

          <AcknowledgementPart
            :handle-accept="handleAccept"
            :handle-decline="handleDecline"
            :text-body="'acknowledgement-part3'"
          />
        </v-card>

        <v-card
          v-if="step.index === 4"
          max-width="1000"
          min-height="500"
          class="text-left"
        >
          <v-card-title> {{ $t('acknowledgement-title-part5') }} </v-card-title>

          <AcknowledgementPart
            :handle-accept="handleAccept"
            :handle-decline="handleDecline"
            :text-body="'acknowledgement-part4'"
          />
        </v-card>

        <v-card
          v-if="step.index === 5"
          max-width="1000"
          min-height="500"
          class="text-left"
        >
          <v-card-title> {{ $t('acknowledgement-title-part6') }} </v-card-title>

          <AcknowledgementPart
            :handle-accept="handleAccept"
            :handle-decline="handleDecline"
            :text-body="'acknowledgement-part5'"
            :link="Routes.PENAL_CODE_PATH"
          />
        </v-card>

        <v-card
          v-if="step.index === 6"
          max-width="1000"
          min-height="500"
          class="text-left"
        >
          <v-card-title> {{ $t('acknowledgement-title-part7') }} </v-card-title>

          <AcknowledgementPart
            :handle-accept="handleAccept"
            :handle-decline="handleDecline"
            :text-body="'acknowledgement-part6'"
          />
        </v-card>

        <v-card
          v-if="step.index === 7"
          :loading="isLoading"
          max-width="1000"
          min-height="500"
          class="text-left"
        >
          <v-card-title> {{ $t('acknowledgement-title-part8') }} </v-card-title>

          <AcknowledgementPart
            :handle-accept="handleFinalAcceptAndCreate"
            :handle-decline="handleDecline"
            :text-body="'acknowledgement-part7'"
          />
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import AcknowledgementInitial from '@shared-ui/components/acknowledgement-section/AcknowledgementInitial.vue'
import AcknowledgementPart from '@shared-ui/components/acknowledgement-section/AcknowledgementPart.vue'
import Routes from '@core-public/router/routes'
import { defaultPermitState } from '@shared-utils/lists/defaultConstants'
import { reactive } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'

const completeApplicationStore = useCompleteApplicationStore()
const authStore = useAuthStore()
const brandStore = useBrandStore()

interface AcknowledgementProps {
  nextRoute: string
}

const router = useRouter()
const step = reactive({
  index: 0,
})
const props = defineProps<AcknowledgementProps>()

const { isLoading, mutate: createMutation } = useMutation({
  mutationFn: completeApplicationStore.createApplication,
  onSuccess: () => {
    router.push({
      path: props.nextRoute,
      query: {
        applicationId: completeApplicationStore.completeApplication.id,
        isComplete:
          completeApplicationStore.completeApplication.application.isComplete.toString(),
      },
    })
  },
  onError: () => null,
})

function handleAccept() {
  step.index += 1
}

function handleDecline() {
  router.push('/')
}

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
</script>
