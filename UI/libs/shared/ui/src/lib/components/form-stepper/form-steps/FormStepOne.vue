<template>
  <div>

    <v-form
      ref="form"
      v-model="valid"
    >
      <v-subheader class="subHeader font-weight-bold"> Personal Information</v-subheader>

      <v-row class="ml-5">
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="personalInfo.lastName"
            label="Last name"
            :rules="[v => !!v || 'Last name is required']"
            required
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="personalInfo.firstName"
            label="First name"
            :rules="[v => !!v || 'First name is required']"
            required
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="personalInfo.middleName"
            label="Middle name"
            :rules="[
              v => (!!v && !personalInfo.noMiddleName) || 'Middle name is required or you must select no middle name',
            ]"
            required
          />
        </v-col>


        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="personalInfo.suffix"
            label="suffix"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
        >
          <v-checkbox
            class="ml-3"
            v-model="personalInfo.noMiddleName"
            :label="'No middle name'"
          />

        </v-col>
      </v-row>
      <v-divider class="my-3" />

      <v-sub-header class="sub-header font-weight-bold"> Social Security Information </v-sub-header>

      <v-row class="ml-5">

        <v-col
          cols="7"
          md="5"
          m="3"
        >
          <!-- TODO: Add further validation to this once we decide of SSN formatting -->
          <v-text-field
            v-model="personalInfo.ssn"
            label="SSN"
            :rules="[v => !!v || 'SSN is required']"
            required
          />
        </v-col>

        <v-col
          cols="7"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="ssnConfirm"
            label="Confirm SSN"
            :rules="[v => !!v && ssnConfirmError]"
            required
          />

        </v-col>
      </v-row>

      <v-divider class="my-3" />
      <v-row class="ml-1">

        <v-subheader class="subHeader font-weight-bold"> Marital status </v-subheader>
        <v-col
          cols="16"
          md="5"
          sm="3"
        >
          <v-radio-group
            class="ml-3 mt-0"
            row
            :hint="'You must select a value'"
            :persistent-hint="true"
            :rules="[v => !!v || 'You must select a status']"
            v-model="personalInfo.maritalStatus"
          >
            <v-radio
              class="pb-1"
              label="Married"
              value="married"
            />
            <v-radio
              class="pb-1"
              label="Single"
              value="single"
            />

          </v-radio-group>
        </v-col>
      </v-row>
    </v-form>

    <v-divider />
    <v-sub-header class="sub-header font-weight-bold"> Aliases </v-sub-header>
    <div class="alias-components-container">
      <AliasTable :aliases="aliases" />
      <AliasDialog :save-alias="getAliasFromDialog" />
    </div>
    <div class="form-btn-container">
      <v-btn
        color="success mr-2"
        @click="handleSubmit"
        :disabled="!valid"
      >
        Continue
      </v-btn>
      <!-- TODO: this needs to save to save the current state to local storage or call the api to save in
      the db
       -->

      <v-btn color="secondary mr-2">Save and Exit</v-btn>

      <!-- TODO: Make this return to the home page with out saving the form at all -->
      <v-btn color="error mr-2"> Cancel </v-btn>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { mapActions } from 'vuex';
import AliasDialog from '../../dialogs/AliasDialog';
import AliasTable from '../../tables/AliasTable';
import { Alias, PersonalInfo } from '@shared-ui/types/defualtTypes';

export default defineComponent({
  name: 'FormStepOne',
  components: { AliasTable, AliasDialog },
  props: {
    handleNextSection: {
      type: Function as PropType<() => void>,
      default: () => null,
    },
  },
  data() {
    return {
      personalInfo: {} as PersonalInfo,
      ssnConfirm: '' as string,
      aliases: [] as Array<Alias>,
      valid: false,
    };
  },
  computed: {
    ssnConfirmError():string | boolean {
      if (this.ssnConfirm !== '' && this.ssnConfirm !== this.personalInfo.ssn) {
        return "SSN's do not match"
      }
      if (this.ssnConfirm === '') {
        return 'SSN confirm can not be empty';
      }
      return false
    },
  },

  methods: {
    ...mapActions({
      addAlias: 'addAlias',
      addPersonalInfo: 'addPersonalInfo',
    }),
    handleSubmit() {
      this.addPersonalInfo(this.personalInfo);
      this.addAlias(this.aliases);
      this.handleNextSection();
    },

    getAliasFromDialog(alias) {
      this.aliases.unshift(alias);
    },
  },
});
</script>

<style lang="scss" scoped>

.subHeader {
  font-size: 1.5rem;
}

.form-btn-container {
  display: flex;
  width: 90%;
  justify-content: flex-end;
}
</style>
