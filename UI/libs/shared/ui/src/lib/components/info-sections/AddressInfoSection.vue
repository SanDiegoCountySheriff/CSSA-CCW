<template>
  <v-container
    fluid
    class="address-info-container rounded"
  >
    <v-banner
      single-line
      class="sub-header font-weight-bold text-xl text-left my-5"
    >
      {{ $t(props.title) }}
      <template #actions>
        <v-btn
          v-if="
            applicationStore.completeApplication.application.status ==
            ApplicationStatus.Incomplete
          "
          icon
          @click="handleEditRequest"
        >
          <v-icon :color="$vuetify.theme.dark ? 'info' : 'info'">
            mdi-square-edit-outline
          </v-icon>
        </v-btn>
      </template>
    </v-banner>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('Address Line 1: ') }}
          </strong>
          {{ props.addressInfo.addressLine1 }}
        </v-banner>
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('Address Line 2: ') }}
          </strong>
          {{ props.addressInfo.addressLine2 }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t(' Country: ') }}
          </strong>
          {{ props.addressInfo.country }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t(' State or Region: ') }}
          </strong>
          {{ props.addressInfo.state }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t(' City: ') }}
          </strong>
          {{ props.addressInfo.city }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t(' Zip: ') }}
          </strong>
          {{ props.addressInfo.zip }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t(' County: ') }}
          </strong>
          {{ props.addressInfo.county }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { AddressInfoType } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'

interface IAddressInfoSectionProps {
  addressInfo: AddressInfoType
  title: string
  color: string
}
const props = defineProps<IAddressInfoSectionProps>()
const applicationStore = useCompleteApplicationStore()
const router = useRouter()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 3
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  })
}
</script>

<style lang="scss" scoped>
.address-info-container {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}
.info-row {
  display: flex;
  flex-direction: row;
}

.info-text {
  margin-left: 0.5rem;
  text-align: start;
  height: 1.8em;
  width: 50%;
  margin-bottom: 0.5rem;
  padding-left: 1rem;
  padding-bottom: 0.5rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
