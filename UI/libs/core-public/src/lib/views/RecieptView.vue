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
            Thank you for completing your CCW application. The Licensing Staff
            will see you at your appointment.
          </p>
          <v-row><v-col></v-col></v-row>
          <v-row>
            <v-col
              cols="5"
              class="mt-12 mb-4"
            >
              <v-alert
                type="info"
                outlined
                class="custom-alert"
              >
                Please read the following:
              </v-alert>
            </v-col>
          </v-row>
        </v-card-text>
        <v-row justify="center">
          <v-col
            cols="12"
            sm="11"
            offset-sm="0"
          >
            <v-card-text
              class="left mt-1 ml-2"
              style="line-height: 2.5"
            >
              <ol>
                <li style="font-size: 0.975rem">
                  If any of your contact information <strong>changes</strong>,
                  please log in and <strong>update</strong> your application
                  <strong>immediately</strong>.
                </li>
                <li style="font-size: 0.975rem">
                  You can view the <strong>status</strong> of your application
                  by logging in and pressing the
                  <strong>"View Application"</strong> button.
                </li>
                <li style="font-size: 0.975rem">
                  You will be <strong>notified</strong> when
                  <strong>payment</strong> is due.
                </li>
              </ol>
              <v-row justify="center">
                <v-btn
                  justify="center"
                  color="primary"
                  class="mt-15"
                  @click="goToDashBoard"
                >
                  Back To Home
                </v-btn>
              </v-row>
            </v-card-text>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useRouter } from 'vue-router/composables'

const router = useRouter()
const brandStore = useBrandStore()

const goToDashBoard = () => {
  router.push('/')
}
</script>
<style lang="scss">
.v-alert {
  position: fixed;
  left: 50%;
  transform: translate(-50%, -50%);
  margin: 0 auto;
}

.custom-alert {
  height: 50px;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
}
</style>
