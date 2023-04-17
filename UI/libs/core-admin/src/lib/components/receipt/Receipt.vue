<template>
  <v-container
    fluid
    class="text-center"
  >
    <v-row class="receipt-title text-center">
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
interface RecieptProps {
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

const props = defineProps<RecieptProps>()
</script>
<style lang="scss" scoped>
@page {
  margin-top: 5cm;
  margin-left: 2cm;
  margin-bottom: 2cm;
  margin-right: 2cm;
}
.receipt-title {
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
