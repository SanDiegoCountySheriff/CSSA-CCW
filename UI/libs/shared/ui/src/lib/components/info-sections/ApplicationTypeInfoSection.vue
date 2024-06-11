<template>
  <v-container
    fluid
    class="confirm-info-section rounded mt-5"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t(' Application Type') }}
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
            mdi-file-document
          </v-icon>
          <strong>
            {{ $t('Application Type: ') }}
          </strong>
          {{
            ApplicationType[
              applicationStore.completeApplication.application.applicationType
            ].toString()
          }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { ApplicationType } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'

const applicationStore = useCompleteApplicationStore()
</script>

<style lang="scss" scoped>
.type-info-section {
  width: 100%;
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
