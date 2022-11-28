<template>
  <v-container>
    <v-card>
      <v-card-title>
        {{ $t('Your application has been submitted') }}
      </v-card-title>
      <v-card-text>
        {{
          $t(
            'You can use the view status on the home page after login. In the application status section.'
          )
        }}
        {{
          $t('Your sheriff may reach out to you for addition info or documents')
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
// TODO: add the order id to the view,

const router = useRouter();
</script>

<style lang="scss" scoped></style>
