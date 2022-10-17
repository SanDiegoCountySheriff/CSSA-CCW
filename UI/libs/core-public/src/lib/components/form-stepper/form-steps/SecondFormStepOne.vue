<template>
  <div>
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header, font-weight-bold">
        {{ $t(' Employment Status') }}
      </v-subheader>

      <v-row>
        <v-col>
          <v-select
            id="select"
            :items="employmentStatus"
            :label="$t(' Employment Status')"
            :rules="[v => !!v || $t(' Employment status is required')]"
            @change="
              v => {
                completeApplicationStore.completeApplication.employment =
                  v.toLowerCase();
              }
            "
          >
          </v-select>
        </v-col>
      </v-row>
    </v-form>
    <div>
      <WeaponsTable
        :weapons="completeApplicationStore.completeApplication.weapons"
      />
      <WeaponsDialog :save-weapon="getWeaponFromDialog" />
    </div>
    <v-divider />
    <FormButtonContainer
      :valid="state.valid"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { employmentStatus } from '@shared-utils/lists/defaultConstants';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import WeaponsDialog from '@core-public/components/dialogs/WeaponsDialog.vue';
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';

interface ISecondFormStepOneProps {
  handleNextSection: CallableFunction;
}
const completeApplicationStore = useCompleteApplicationStore();

const props = defineProps<ISecondFormStepOneProps>();

const state = reactive({
  valid: false,
});

function handleSubmit() {
  props.handleNextSection();
}

function getWeaponFromDialog(weapon) {
  completeApplicationStore.completeApplication.weapons.push(weapon);
}
</script>

<style lang="scss" scoped></style>
