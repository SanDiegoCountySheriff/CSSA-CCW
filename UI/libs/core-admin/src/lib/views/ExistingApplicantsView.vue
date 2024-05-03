<template>
  <v-container fluid>
    <v-card>
      <v-card-title>Link Existing Applications</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-data-table>
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

<script setup lang="ts"></script>
