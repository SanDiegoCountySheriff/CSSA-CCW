<template>
  <v-container class="align-center justify-center">
    <v-row
      justify="center"
      align="center"
    >
      <v-col>
        <v-card flat>
          <v-card-title class="justify-center mt-10">
            <v-icon
              large
              color="success"
            >
              mdi-check-circle
            </v-icon>
          </v-card-title>
          <v-card-text class="text-center mb-12">
            <h1
              class="mb-13"
              style="color: black; line-height: 2"
              :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
            >
              Your application has been submitted!
            </h1>
            <v-row justify="center">
              <v-col md="6">
                <v-sheet
                  outlined
                  rounded
                  color="info"
                >
                  <v-card flat>
                    <v-card-title class="justify-center">
                      <v-icon
                        left
                        color="blue"
                      >
                        mdi-information
                      </v-icon>
                      <v-span
                        class="mt-1"
                        text="info"
                        :class="
                          themeStore.getThemeConfig.isDark ? 'white--text' : ''
                        "
                      >
                        Please read the following:
                      </v-span>
                    </v-card-title>
                    <v-card-text>
                      <ol
                        class="mb-2"
                        style="color: black; text-align: left; line-height: 2"
                        :class="
                          themeStore.getThemeConfig.isDark ? 'white--text' : ''
                        "
                      >
                        <li style="font-size: 1.1rem">
                          If any of your contact information
                          <strong>changes</strong>, please log in and
                          <strong>update</strong> your application
                          <strong>immediately</strong>.
                        </li>
                        <li style="font-size: 1.1rem">
                          You can view your application
                          <strong>status</strong> by logging in and pressing the
                          <strong>"View Application"</strong> button.
                        </li>
                        <li style="font-size: 1.1rem">
                          You will be <strong>notified</strong> when
                          <strong>payment</strong> is due.
                        </li>
                      </ol>
                    </v-card-text>
                  </v-card>
                </v-sheet>
                <v-row justify="center">
                  <p
                    v-if="isRenewOrModify"
                    class="mt-15"
                    style="color: black; font-size: 1.1rem; line-height: 2"
                    :class="
                      themeStore.getThemeConfig.isDark ? 'white--text' : ''
                    "
                  >
                    Thank you for completing your CCW renewal/modification
                    application. The Licensing Staff will notify you of their
                    evaluation.
                  </p>
                  <p
                    v-else
                    class="mt-15"
                    style="color: black; font-size: 1.11rem; line-height: 2"
                    :class="
                      themeStore.getThemeConfig.isDark ? 'white--text' : ''
                    "
                  >
                    Thank you for completing your CCW application. The Licensing
                    Staff will see you at your appointment.
                  </p>
                </v-row>
              </v-col>
            </v-row>
          </v-card-text>
          <v-row>
            <v-col>
              <v-card-text class="left">
                <v-row justify="center">
                  <v-btn
                    large
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
import {
  ApplicationType,
  CompleteApplication,
} from '@shared-utils/types/defaultTypes'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { computed } from 'vue'

const router = useRouter()
const brandStore = useBrandStore()
const themeStore = useThemeStore()
const completeApplication = useCompleteApplicationStore()

const goToDashBoard = () => {
  router.push('/')
}

const isRenewOrModify = computed(() => {
  const applicationType =
    completeApplication.getCompleteApplication.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment'] ||
    applicationType === ApplicationType['Modify Standard'] ||
    applicationType === ApplicationType['Modify Reserve'] ||
    applicationType === ApplicationType['Modify Judicial'] ||
    applicationType === ApplicationType['Modify Employment']
  )
})
</script>
