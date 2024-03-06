<template>
  <v-card :loading="isLoading">
    <v-card-title>Modification</v-card-title>

    <v-card-text>
      <v-row>
        <v-col>
          <v-text-field label="Modify"></v-text-field>
        </v-col>

        <v-col>
          <v-text-field label="Modify"></v-text-field>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script lang="ts" setup>
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useQuery } from '@tanstack/vue-query'
import { onMounted, ref } from 'vue'

const applicationStore = useCompleteApplicationStore()
const isApplicationValid = ref(false)

onMounted(() => {
  window.scrollTo(0, 0)
  isApplicationValid.value = Boolean(applicationStore.completeApplication.id)
})

const { isLoading } = useQuery(
  ['getApplicationsByUser'],
  () => applicationStore.getAllUserApplicationsApi(),
  {
    enabled: !isApplicationValid.value,
    onSuccess: data => {
      applicationStore.setCompleteApplication(data[0] as CompleteApplication)
    },
  }
)
</script>
