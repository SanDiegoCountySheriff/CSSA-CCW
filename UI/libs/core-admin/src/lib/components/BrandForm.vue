<template>
  <div>
    <v-container
      fluid
      class="form-container blue-grey lighten-3 elevation-3 rounded"
    >
      <v-card
        class="form-card rounded elevation-2"
        :class="{ 'dark-card': $vuetify.theme.dark }"
      >
        <v-form
          @submit.prevent="getFormValues"
          v-model="valid"
        >
          <BrandLogo />
          <v-row>
            <v-col
              cols="12"
              sm="6"
              md="3"
            >
              <BrandTitle />
            </v-col>
          </v-row>
          <v-row class="ml-5">
            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-btn
                :disabled="!valid"
                color="success"
                class="mr-4"
                type="submit"
              >
                {{ $t('Publish') }}
              </v-btn>
            </v-col>
          </v-row>
        </v-form>
      </v-card>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import BrandLogo from './BrandLogo.vue';
import BrandTitle from './BrandTitle.vue';

function getFormValues(submitEvent) {
  // Push model JSON to API endpoint for Public App consumption"
  console.log(submitEvent.target);
}
</script>
