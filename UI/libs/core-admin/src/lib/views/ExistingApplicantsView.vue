<template>
  <v-container fluid>
    <v-card :loading="isLoading || isFetching">
      <v-card-title>Link Existing Applications</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-data-table
              :items="data"
              :headers="headers"
            >
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title> Applicants </v-toolbar-title>
                </v-toolbar>
              </template>
            </v-data-table>
          </v-col>

          <v-col>
            <v-data-table>
              <template #top>
                <v-toolbar flat>
                  <v-toolbar-title> Legacy Applications </v-toolbar-title>
                </v-toolbar>
              </template>
            </v-data-table>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query'
import { useUserStore } from '@shared-ui/stores/userStore'

const userStore = useUserStore()

const { data, isLoading, isFetching } = useQuery(
  ['getUnmatchedUsers'],
  userStore.getUnmatchedUsers
)

const headers = [
  {
    text: 'Email',
    value: 'email',
  },
]
</script>
