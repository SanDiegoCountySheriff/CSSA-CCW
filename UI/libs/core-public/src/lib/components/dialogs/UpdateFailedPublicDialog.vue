<template>
  <v-dialog
    v-model="permitStore.updateFailed"
    max-width="800"
    persistent
  >
    <v-card>
      <v-card-title> A change has been made by another user </v-card-title>

      <v-alert
        v-if="!permitStore.updateWasComment"
        class="ma-3"
        type="warning"
        text
        outlined
        prominent
      >
        <v-card-title>
          <span class="black--text">
            Click refresh and then make your changes to
            <strong>{{ permitStore.updateFailedItem }}</strong
            >.
          </span>
        </v-card-title>
      </v-alert>

      <v-alert
        v-else
        class="ma-3"
        type="warning"
        prominent
        outlined
        text
      >
        <v-card-title>
          <span class="black--text">
            Click refresh and then make your changes to
            <strong>{{ permitStore.updateFailedItem }}</strong
            >.

            <v-divider class="my-4 warning" />

            The comment has been added to your clipboard, you may paste it back
            into the comment field.
          </span>
        </v-card-title>
      </v-alert>

      <v-card-actions>
        <v-btn
          x-large
          text
          color="error"
          @click="permitStore.updateFailed = !permitStore.updateFailed"
        >
          Close
        </v-btn>

        <v-spacer />

        <v-btn
          x-large
          text
          color="success"
          @click="invalidateQuery"
        >
          Refresh
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useQueryClient } from '@tanstack/vue-query'

const permitStore = useCompleteApplicationStore()
const queryClient = useQueryClient()

async function invalidateQuery() {
  await queryClient.invalidateQueries(['permitDetail'])
  permitStore.updateFailed = false
  permitStore.updateWasComment = false
}
</script>

<style scoped>
.v-card__text,
.v-card__title {
  word-break: normal; /* maybe !important  */
}
</style>
