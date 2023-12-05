<template>
  <v-container>
    <v-row
      justify="center"
      align="center"
    >
      <v-col
        cols="12"
        sm="8"
        md="6"
      >
        <v-card class="elevation-1 pa-5">
          <v-card-title class="justify-center">
            <v-icon
              large
              color="success"
            >
              mdi-check-circle
            </v-icon>
          </v-card-title>
          <v-card-text class="text-center">
            <h2>Your application has been submitted!</h2>
            <p class="subtitle-1 mt-7">
              Thank you for completing your CCW application. Your submission was
              successful! San Diego Sheriff Licensing Staff will contact you
              once the application has been reviewed. Due to the volume of
              applications submitted, it may be several months before staff
              contacts you.
            </p>
            <span
              style="color: red"
              class="subtitle-1"
              >If any of your contact information changes, please log in and
              modify your application immediately.</span
            >
          </v-card-text>
          <v-card-text class="text-center">
            <p>
              You can view the status of your application by logging in and
              pressing the "View Application" button.
            </p>
            <v-btn
              color="primary"
              class="mt-5"
              @click="goToDashBoard"
            >
              Back To Home
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router/composables'

const router = useRouter()

const goToDashBoard = () => {
  router.push('/')
}
</script>
