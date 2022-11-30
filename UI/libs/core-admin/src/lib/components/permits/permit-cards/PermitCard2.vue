<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    class="mt-8 mb-2"
    elevation="0"
  >
    <v-row class="ml-5">
      <v-col cols="4">
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
              <div class="text-overline mb-4">
                <span>
                  {{ permitStore.getPermitDetail.application.dob.birthDate }}
                </span>
                <span>
                  {{ permitStore.getPermitDetail.application.idInfo.idNumber }}
                </span>
                <v-divider></v-divider>
              </div>
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
                  color="grey lighten-2"
                  size="40"
                >
                  <v-icon> mdi-camera-outline </v-icon>
                </v-list-item-avatar>
                <v-list-item-avatar
                  tile
                  color="grey lighten-2"
                  size="40"
                >
                  <v-icon> mdi-account-edit-outline</v-icon>
                </v-list-item-avatar>
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>
      <v-col cols="4">
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
              <div class="text-overline mb-4">
                <v-icon color="error lighten-2"> mdi-alert </v-icon>1 Missing
                Requirement
              </div>
              <v-list-item-subtitle>
                No Requests Sent
                <v-chip
                  color="grey lighten-2"
                  label
                >
                  <v-icon> mdi-email-outline </v-icon>
                  Send Request
                </v-chip>
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>
      <v-col cols="4">
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
              <div class="text-overline mb-4">
                <v-icon color="error lighten-2"> mdi-alert </v-icon>11:00 AM on
                March 13, 2024
              </div>
              <v-list-item-subtitle>
                No Show
                <v-chip
                  color="grey lighten-2"
                  label
                >
                  <v-icon> mdi-email-outline </v-icon>
                  Reschedule
                </v-chip>
                <v-chip
                  color="grey lighten-2"
                  label
                >
                  <v-icon> mdi-email-outline </v-icon>
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
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const { isLoading } = useQuery(['permitDetail']);

const permitStore = usePermitsStore();
</script>
