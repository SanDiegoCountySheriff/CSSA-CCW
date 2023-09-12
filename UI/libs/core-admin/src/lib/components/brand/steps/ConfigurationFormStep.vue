<template>
  <div>
    <v-card-text>
      <v-form
        ref="form"
        v-model="valid"
      >
        <v-row>
          <v-col
            sm="6"
            cols="12"
          >
            <v-text-field
              v-model.number="brandStore.brand.expiredApplicationRenewalPeriod"
              label="Expired application renewal grace period, days"
              hint="Expired applications will have this many days after expiration to submit their renewal"
              :rules="[v => !!v || 'An expiration renewal period is required']"
              type="number"
              color="primary"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            sm="6"
            cols="12"
          >
            <v-text-field
              v-model.number="
                brandStore.brand.archivedApplicationRetentionPeriod
              "
              label="Archived application retention period, years"
              hint="Applications that are inactive will be retained for this many years"
              :rules="[
                v =>
                  !!v || 'An archived application retention period is required',
              ]"
              type="number"
              color="primary"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>

    <v-card-actions>
      <v-btn
        @click="handleResetStep"
        color="primary"
      >
        {{ $t('Cancel') }}
      </v-btn>
      <v-btn
        @click="props.handleBackStep"
        color="primary"
      >
        {{ $t('Back') }}
      </v-btn>
      <v-btn
        :disabled="!valid"
        @click="setFormValues"
        color="primary"
      >
        {{ $t('Publish') }}
      </v-btn>
    </v-card-actions>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useMutation } from '@tanstack/vue-query'

interface IAgencyFormStepProps {
  handleNextStep: () => void
  handleBackStep: () => void
  handleResetStep: () => void
}

const props = withDefaults(defineProps<IAgencyFormStepProps>(), {
  handleNextStep: () => null,
  handleBackStep: () => null,
  handleResetStep: () => null,
})

const brandStore = useBrandStore()
const valid = ref(false)

const setBrandSettings = useMutation({
  mutationFn: () => brandStore.setBrandSettingApi(),
  onSuccess: () => props.handleNextStep(),
})

async function setFormValues() {
  setBrandSettings.mutate()
}
</script>
