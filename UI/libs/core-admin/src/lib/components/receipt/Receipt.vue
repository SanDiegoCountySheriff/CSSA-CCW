<template>
  <v-container fluid>
    <v-row>
      <v-col>
        <h4>
          {{ props.date }}
        </h4>
        <h5 v-if="props.transactionId">
          Transaction Id: {{ props.transactionId }}
        </h5>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <h4>Name:</h4>
      </v-col>
      <v-col>
        <h5>
          {{ props.name }}
        </h5>
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        <h4>Order Id:</h4>
      </v-col>
      <v-col>
        <h5>
          {{ props.orderId }}
        </h5>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <h4>Application Type:</h4>
      </v-col>
      <v-col>
        <h5>
          {{ props.applicationType }}
        </h5>
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        <h4>Payment Type:</h4>
      </v-col>
      <v-col>
        <h5>
          {{ props.paymentType }}
        </h5>
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        <h4>Total Amount:</h4>
      </v-col>
      <v-col>
        <h4>$ {{ parseInt(props.total).toFixed(2) }}</h4>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <h4>Vendor Information:</h4>
      </v-col>
      <v-col>
        <h4>{{ props.vendorInfo }}</h4>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <h4>Authorization:</h4>
      </v-col>
      <v-col>
        <h4>{{ props.auth }}</h4>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
interface ReceiptProps {
  name: string
  applicationType: string
  paymentType: string
  vendorInfo?: string
  date: string
  total: string
  orderId: string
  transactionId?: string
  auth: string
}

const props = defineProps<ReceiptProps>()
</script>
