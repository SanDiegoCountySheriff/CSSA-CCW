<template>
  <v-container class="align-center justify-center">
    <v-row
      justify="center"
      align="center"
    >
      <v-col>
        <v-card
          flat
          height="700px"
        >
          <v-card-title class="justify-center mt-10">
            <v-icon
              large
              color="success"
            >
              mdi-check-circle
            </v-icon>
          </v-card-title>
          <v-card-text class="text-center mb-12">
            <h2
              class="mb-10"
              style="color: black"
              :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
            >
              Your application has been submitted!
            </h2>
            <p
              class="subtitle-1"
              style="color: black"
              :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
            >
              Thank you for completing your CCW application. The Licensing Staff
              will see you at your appointment.
            </p>
            <v-row><v-col></v-col></v-row>
            <v-row justify="center">
              <v-col
                cols="5"
                class="mr-5"
              >
                <v-sheet
                  outlined
                  rounded
                  color="info"
                  class="ml-6"
                >
                  <v-card flat>
                    <v-card-title class="justify-center">
                      <v-icon
                        left
                        color="blue"
                        >mdi-information</v-icon
                      >
                      <v-span
                        text="info"
                        :class="
                          themeStore.getThemeConfig.isDark ? 'white--text' : ''
                        "
                        >Please read the following:
                      </v-span>
                    </v-card-title>
                    <v-card-text>
                      <ol
                        class="mb-2"
                        style="color: black; text-align: left"
                        :class="
                          themeStore.getThemeConfig.isDark ? 'white--text' : ''
                        "
                      >
                        <li style="font-size: 0.975rem">
                          If any of your contact information
                          <strong>changes</strong>, please log in and
                          <strong>update</strong> your application
                          <strong>immediately</strong>.
                        </li>
                        <li style="font-size: 0.975rem">
                          You can view the <strong>status</strong> of your
                          application by logging in and pressing the
                          <strong>"View Application"</strong> button.
                        </li>
                        <li style="font-size: 0.975rem">
                          You will be <strong>notified</strong> when
                          <strong>payment</strong> is due.
                        </li>
                      </ol>
                    </v-card-text>
                  </v-card>
                </v-sheet>
              </v-col>
            </v-row>
          </v-card-text>
          <v-row>
            <v-col>
              <v-card-text class="left">
                <v-row justify="center">
                  <v-btn
                    color="primary"
                    @click="goToDashBoard"
                  >
                    Back To Home
                  </v-btn>
                </v-row>
              </v-card-text>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useRouter } from 'vue-router/composables'

const router = useRouter()
const brandStore = useBrandStore()
const themeStore = useThemeStore()

const goToDashBoard = () => {
  router.push('/')
}
</script>
<style lang="scss"></style>
