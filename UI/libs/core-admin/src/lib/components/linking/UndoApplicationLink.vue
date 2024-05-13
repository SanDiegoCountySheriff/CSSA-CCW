<template>
  <div>
    <v-card flat>
      <v-card-text>
        <v-data-table
          :items="items"
          :headers="headers"
        >
          <template #top>
            <v-toolbar flat>
              <v-toolbar-title> Undo Application Match </v-toolbar-title>

              <v-spacer />

              <v-text-field
                v-model="search"
                label="Search"
                color="primary"
                hide-details
                outlined
                clearable
              ></v-text-field>
            </v-toolbar>
          </template>

          <template #[`item.orderId`]="{ item }">
            <router-link
              :to="{
                name: 'PermitDetail',
                params: { orderId: item.orderId },
              }"
              style="text-decoration: underline; color: inherit"
            >
              {{ item.orderId }}
            </router-link>
          </template>

          <template #[`item.actions`]="item">
            <v-btn
              @click="handleUndo(item.orderId)"
              color="primary"
            >
              <v-icon left>mdi-undo</v-icon>
              Undo Match
            </v-btn>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const items = [
  {
    orderId: 'DDTR4CFDG',
    name: 'Jake Kellas',
    permitNumber: 'CA12345',
  },
]
const search = ref('')
const headers = [
  {
    text: 'Order ID',
    value: 'orderId',
  },
  { text: 'Name', value: 'name' },
  { text: 'Permit Number', value: 'permitNumber' },
  { text: 'Actions', value: 'actions' },
]

function handleUndo(orderId: string) {
  window.console.log(orderId)
}
</script>
