<template>
  <v-container class="type-info-section rounded">
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t(' Application Type') }}
      <template #actions>
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <v-btn
              icon
              @click="handleEditRequest"
              v-bind="attrs"
              v-on="on"
            >
              <v-icon color="info"> mdi-square-edit-outline </v-icon>
            </v-btn>
          </template>
          {{ $t('Edit Section') }}
        </v-tooltip>
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
            color="accent"
          >
            mdi-file-document
          </v-icon>
          <strong >
            {{ $t('Application Type: ') }}
          </strong>
          {{ applicationStore.completeApplication.application.applicationType }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { useCompleteApplicationStore} from "@shared-ui/stores/completeApplication";
import { useRouter} from "vue-router/composables";

const applicationStore = useCompleteApplicationStore();
const router = useRouter();

function handleEditRequest(){
 applicationStore.completeApplication.application.currentStep = 7;
 router.push({
   path: '/form',
   query: {
   applicationId: applicationStore.completeApplication.id,
   isComplete: applicationStore.completeApplication.application.isComplete
   }
 })
}

</script>

<style lang="scss" scoped>
.type-info-section {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}

.info-row {
  display: flex;
  flex-direction: row;
  min-height: 1vh;
  max-height: 2vh;
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
