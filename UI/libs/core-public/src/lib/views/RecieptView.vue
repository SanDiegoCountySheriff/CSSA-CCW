<template>
  <v-container>
    <v-card>
      <v-card-title>
        {{ $t('Your application has been submitted') }}
      </v-card-title>
      <v-card-text>
        {{
          $t(
            'You can view the status of your application by logging in and pressing the "VIEW APPLICATION" button.'
          )
        }}
        {{
          $t(
            `The Sheriff's Department may reach out to you for any additional information or documents.`
          )
        }}
      </v-card-text>
      <v-card-text>
        <v-btn
          color="primary"
          outlined
          @click="router.push('/')"
        >
          {{ $t('back to home') }}
        </v-btn>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router/composables'

const router = useRouter()
</script>
