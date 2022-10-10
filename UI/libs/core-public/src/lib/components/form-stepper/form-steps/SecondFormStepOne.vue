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
            :items="employmentStatus"
            :label="$t(' Employment Status')"
            :rules="[v => !!v || $t(' Employment status is required')]"
            @change="
              v => {
                state.employment = v.toLowerCase();
              }
            "
          >
          </v-select>
        </v-col>
      </v-row>
    </v-form>
    <div>
      <WeaponsTable :weapons="state.weapons" />
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
import { useEmploymentStore } from '@shared-ui/stores/employment';
import { useWeaponsStore } from '@shared-ui/stores/weapons';
import { employmentStatus } from '@shared-utils/lists/defaultList';
import { WeaponInfoType } from '@shared-utils/types/defaultTypes';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import WeaponsDialog from '@core-public/components/dialogs/WeaponsDialog.vue';
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';

interface ISecondFormStepOneProps {
  handleNextSection: CallableFunction;
}

const props = defineProps<ISecondFormStepOneProps>();
const state = reactive({
  employment: '',
  weapons: [] as Array<WeaponInfoType>,
  valid: false,
});

const employmentStore = useEmploymentStore();
const weaponStore = useWeaponsStore();

function handleSubmit() {
  employmentStore.setEmployment(state.employment);
  weaponStore.setWeapons(state.weapons);
  props.handleNextSection();
}

function getWeaponFromDialog(weapon) {
  state.weapons.push(weapon);
}
</script>

<style lang="scss" scoped></style>
