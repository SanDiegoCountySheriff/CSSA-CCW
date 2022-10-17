<template>
  <div>
    <v-row class="mb-1">
      <v-col>
        <PersonalInfoSection
          :color="'teal lighten-4'"
          :personal-info="
            completeApplicationStore.completeApplication.personalInfo
          "
        />
      </v-col>
      <v-col>
        <CitizenInfoSection
          :color="'pink lighten-4'"
          :citizenship-info="
            completeApplicationStore.completeApplication.citizenship
          "
        />
        <IdInfoSection
          class="mt-2"
          :color="'indigo lighten-4'"
          :id-info="completeApplicationStore.completeApplication.id"
        />
      </v-col>
    </v-row>
    <v-divider />
    <v-row class="my-1">
      <v-col>
        <AliasInfoSection
          :color="'teal lighten-4'"
          :alias-info="completeApplicationStore.completeApplication.aliases"
        />
      </v-col>
    </v-row>
    <v-divider />
    <v-row class="my-1">
      <v-col>
        <DOBinfoSection
          :color="'pink lighten-4'"
          :d-o-b-info="completeApplicationStore.completeApplication.DOB"
        />
      </v-col>
    </v-row>
    <v-row class="my-1">
      <AppearanceInfoSection
        :color="'indigo lighten-4'"
        :appearance-info="
          completeApplicationStore.completeApplication.physicalAppearance
        "
      />
    </v-row>
    <v-divider />
    <v-row class="my-1">
      <AddressInfoSection
        :color="'teal lighten-4'"
        :address-info="
          completeApplicationStore.completeApplication.currentAddress
        "
      />
    </v-row>

    <v-divider />
    <v-row class="my-1">
      <PreviousAddressInfoSection
        :color="'indigo lighten-4'"
        :previous-address="
          completeApplicationStore.completeApplication.previousAddress
        "
      />
    </v-row>

    <v-divider />
    <v-row class="my-1">
      <ContactInfoSection
        :color="'teal lighten-4'"
        :contact-info="completeApplicationStore.completeApplication.contact"
      />
    </v-row>
    <v-row>
      <ConfirmationSelectionContainer
        @confirm="onConfirm"
        @cancel="onCancel"
      />
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useRouter } from 'vue-router/composables';

import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue';
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue';
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue';
import AliasInfoSection from '@shared-ui/components/info-sections/AliasInfoSection.vue';
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue';
import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue';
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue';
import PreviousAddressInfoSection from '@shared-ui/components/info-sections/PreviousAddressInfoSection.vue';
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue';
import ConfirmationSelectionContainer from '@shared-ui/components/containers/ConfimationSelectionContainer.vue';

const completeApplicationStore = useCompleteApplicationStore();

interface IFormStepSixProps {
  resetForm: CallableFunction;
}
const router = useRouter();
const props = defineProps<IFormStepSixProps>();

function onConfirm() {
  props.resetForm();
  router.push('/form-2');
}

function onCancel() {
  props.resetForm();
}
</script>

<style lang="scss" scoped></style>
