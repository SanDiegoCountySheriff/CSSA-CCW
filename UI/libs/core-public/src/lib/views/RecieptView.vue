<template>
  <v-container>
    <v-card>
      <v-card-title>
        {{ $t('Your application has been submitted') }}
      </v-card-title>
      <v-card-text>
        {{
          $t(
            'You can view the status of your applications after logging in by pressing the view applications button and selecting the submitted application.'
          )
        }}
        {{
          $t(
            `The sheriff's department may reach out to you for any addition info or documents.`
          )
        }}
      </v-card-text>
      <v-card-text>
        <v-btn
          text
          color="primary"
          @click="router.push('/')"
        >
          {{ $t('back to home') }}
        </v-btn>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router/composables';

const router = useRouter();
</script>
