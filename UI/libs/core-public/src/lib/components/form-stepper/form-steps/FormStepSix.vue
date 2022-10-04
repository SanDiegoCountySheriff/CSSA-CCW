<template>
  <div>
    <!-- need to create sections for the info -->
    <v-row class="mb-1">
      <v-col>
        <PersonalInfoSection
          :color="'teal lighten-5'"
          :personal-info="state.personalInfo"
        />
      </v-col>
      <v-col>
        <CitizenInfoSection
          :color="'pink lighten-5'"
          :citizenship-info="state.citizenshipInfo"
        />
        <IdInfoSection
          class="mt-2"
          :color="'indigo lighten-5'"
          :id-info="state.id"
        />
      </v-col>
    </v-row>
    <v-divider />
    <v-row class="my-1">
      <v-col>
        <AliasInfoSection
          :color="'teal lighten-5'"
          :alias-info="state.aliases"
        />
      </v-col>
    </v-row>
    <v-divider />
    <v-row class="my-1">
      <v-col>
        <DOBinfoSection
          :color="'pink lighten-5'"
          :d-o-b-info="state.DOBInfo"
        />
      </v-col>
    </v-row>
    <v-row class="my-1">
      <AppearanceInfoSection
        :color="'indigo lighten-5'"
        :appearance-info="state.appearance"
      />
    </v-row>
    <v-divider />
    <v-row class="my-1">
      <AddressInfoSection
        :color="'teal lighten-5'"
        :address-info="state.address"
      />
    </v-row>

    <v-divider />
    <v-row class="my-1">
      <PreviousAddressInfoSection
        :color="'indigo lighten-5'"
        :previous-address="state.previousAddress"
      />
    </v-row>

    <v-divider />
    <v-row class="my-1">
      <ContactInfoSection
        :color="'teal lighten-5'"
        :contact-info="state.contact"
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
import { useCurrentAddressStore } from '@shared-ui/stores/currentAddress';
import { useAliasStore } from '@shared-ui/stores/alias';
import { usePhysicalAppearanceStore } from '@shared-ui/stores/physicalAppearance';
import { useCitizenshipStore } from '@shared-ui/stores/citizenship';
import { reactive } from 'vue';
import { useContactStore } from '@shared-ui/stores/contact';
import { useDOBStore } from '@shared-ui/stores/DOB';
import { useIdStore } from '@shared-ui/stores/id';
import { usePersonalInfoStore } from '@shared-ui/stores/personalInfo';
import { usePreviousAddressesStore } from '@shared-ui/stores/previousAddress';

import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue';
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue';
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue';
import AliasInfoSection from '@shared-ui/components/info-sections/AliasInfoSection.vue';
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue';
import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue';
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue';
import PreviousAddressInfoSection from '@shared-ui/components/info-sections/PreviousAddressInfoSection.vue';
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue';
import ConfirmationSelectionContainer
  from '@shared-ui/components/containers/ConfimationSelectionContainer.vue';
import { useRouter } from 'vue-router/composables';

const addressStore = useCurrentAddressStore();
const aliaseStore = useAliasStore();
const appearanceStore = usePhysicalAppearanceStore();
const citizenshipStore = useCitizenshipStore();
const contactStore = useContactStore();
const DOBStore = useDOBStore();
const idStore = useIdStore();
const personalInfoStore = usePersonalInfoStore();
const previousAddressStore = usePreviousAddressesStore();
const router = useRouter();

interface IFormStepSixProps {
  resetForm: CallableFunction;
}
const props = defineProps<IFormStepSixProps>();

const state = reactive({
  address: addressStore.getCurrentAddress,
  aliases: aliaseStore.getAliases,
  appearance: appearanceStore.getPhysicalAppearance,
  citizenshipInfo: citizenshipStore.getCitizenshipInfo,
  contact: contactStore.getContactInfo,
  DOBInfo: DOBStore.getDOBInfo,
  id: idStore.getId,
  personalInfo: personalInfoStore.getPersonalInfo,
  previousAddress: previousAddressStore.getPreviousAddresses,
});

function onConfirm() {
  router.push('form-part-2');
}
function onCancel() {
  props.resetForm();
}
</script>

<style lang="scss" scoped></style>
