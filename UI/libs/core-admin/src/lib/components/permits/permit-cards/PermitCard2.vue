<!-- eslint-disable vue/multiline-html-element-content-newline -->
<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    class="mt-6 mb-2"
    elevation="0"
    :color="$vuetify.theme.dark ? '#303030' : ''"
  >
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-container
          v-if="isLoading"
          fluid
        >
          <v-skeleton-loader
            fluid
            class="fill-height"
            type="list-item,divider,list-item"
          >
          </v-skeleton-loader>
        </v-container>
        <v-card
          class="mx-auto text-left"
          elevation="2"
          height="150"
          v-else
        >
          <v-list-item three-line>
            <v-list-item-content>
              <div class="text-overline mb-4 d-flex">
                <span>
                  {{
                    formatDate(
                      permitStore.getPermitDetail.application.dob.birthDate
                    )
                  }}
                </span>
                <v-spacer></v-spacer>
                <v-spacer></v-spacer>
                <v-spacer></v-spacer>
                <span>
                  {{ permitStore.getPermitDetail.application.idInfo.idNumber }}
                </span>
              </div>
              <v-divider></v-divider>
              <v-list-item-title class="mb-1 font-weight-bold">
                {{
                  permitStore.getPermitDetail.application.personalInfo.lastName
                }},
                {{
                  permitStore.getPermitDetail.application.personalInfo.firstName
                }}
              </v-list-item-title>
              <v-list-item-subtitle>
                No previous applications
                <v-list-item-avatar
                  tile
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  class="ml-8"
                  size="40"
                >
                  <v-icon> mdi-camera-outline </v-icon>
                </v-list-item-avatar>
                <v-list-item-avatar
                  tile
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  size="40"
                >
                  <v-icon> mdi-account-edit-outline</v-icon>
                </v-list-item-avatar>
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-container
          v-if="isLoading"
          fluid
        >
          <v-skeleton-loader
            fluid
            class="fill-height"
            type="list-item,divider,list-item"
          >
          </v-skeleton-loader>
        </v-container>
        <v-card
          class="mx-auto"
          elevation="2"
          height="150"
          v-else
        >
          <v-list-item three-line>
            <v-list-item-content>
              <div
                class="text-overline mb-4"
                v-if="!permitStore.getPermitDetail.application.isComplete"
              >
                <v-icon
                  color="error lighten-2"
                  class="mr-1"
                >
                  mdi-alert
                </v-icon>
                Missing Requirement
              </div>
              <div
                class="text-overline mb-4"
                v-else
              >
                <v-icon
                  color="success lighten-2"
                  class="mr-1"
                >
                  mdi-shield-check
                </v-icon>
                Requirement Fulfilled
              </div>
              <v-list-item-title class="mt-8 font-weight-bold">
              </v-list-item-title>
              <v-list-item-subtitle>
                No Requests Sent
                <v-chip
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  class="ml-8"
                  :href="`mailto:${permitStore.getPermitDetail.application.userEmail}`"
                  target="_blank"
                  label
                >
                  <v-icon class="mr-1"> mdi-email-outline </v-icon>
                  Send Request
                </v-chip>
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-container
          v-if="isLoading"
          fluid
        >
          <v-skeleton-loader
            fluid
            class="fill-height"
            type="list-item,divider,list-item"
          >
          </v-skeleton-loader>
        </v-container>
        <v-card
          class="mx-auto mr-8"
          elevation="2"
          height="150"
          v-else
        >
          <v-list-item three-line>
            <v-list-item-content>
              <div
                v-if="
                  permitStore.getPermitDetail.application.appointmentDateTime
                "
                class="text-overline mb-4"
              >
                <v-icon
                  color="success lighten-2"
                  class="mr-1"
                >
                  mdi-shield-check </v-icon
                >{{ appointmentTime }} ON {{ appointmentDate }}
              </div>
              <div
                v-else
                class="text-overline mb-4"
              >
                <v-icon
                  color="error lighten-2"
                  class="mr-1"
                >
                  mdi-alert </v-icon
                >Not Scheduled
              </div>
              <v-list-item-title class="mt-8 font-weight-bold">
              </v-list-item-title>
              <v-list-item-subtitle>
                No Show

                <v-dialog
                  v-model="dialog"
                  width="500"
                >
                  <template #activator="{ on, attrs }">
                    <v-chip
                      :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                      class="ml-4"
                      v-bind="attrs"
                      v-on="on"
                      label
                    >
                      <v-icon class="mr-1"> mdi-calendar-edit </v-icon>
                      Reschedule
                    </v-chip>
                  </template>

                  <v-card>
                    <Schedule />
                  </v-card>
                </v-dialog>
                <v-chip
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  class="ml-4"
                  label
                >
                  <v-icon class="mr-1"> mdi-clock-outline </v-icon>
                  Check-in
                </v-chip>
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import Schedule from '@core-admin/components/appointment/Schedule.vue';
import { formatDate } from '@shared-utils/formatters/defaultFormatters';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useRoute } from 'vue-router/composables';
import { computed, ref } from 'vue';

const dialog = ref(false);
const route = useRoute();
const permitStore = usePermitsStore();

const { isLoading } = useQuery(['permitDetail', route.params.orderId], () =>
  permitStore.getPermitDetailApi(route.params.orderId)
);

const appointmentDate = computed(
  () =>
    new Date(
      permitStore.getPermitDetail?.application.appointmentDateTime
    )?.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    }) || ''
);

const appointmentTime = computed(
  () =>
    new Date(
      permitStore.getPermitDetail?.application.appointmentDateTime
    )?.toLocaleTimeString('en-US', {
      timeStyle: 'short',
    }) || ''
);
</script>
