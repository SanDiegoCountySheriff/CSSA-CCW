<template>
  <div>
    <v-container
      fill-height
      fluid
    >
      <img
        alt="Agency logo"
        :src="brandStore.documents.agencyLandingPageImage"
      />
      <h1>CONFIRMATION</h1>
    </v-container>
    <v-row>
      <v-col>
        <br />
        <p>
          This is not a proof of payment or approved application. This document
          indicates that either payment was attempted online, via credit / debit
          card or that a valid coupon code was submitted . Payment for coupon
          codes will be collected by the Sheriff's Payment Policy
        </p>
        <br />
        <p>
          Payments will be confirmed only alter payments have been verified or
          coupons have been issued. Processed Applications will receive an email
          noting that their applications have been reviewed
        </p>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <p>
          A COPY OF THIS RECEIPT SHOULD BE KEPT AND REFERENCED IN ALL FOLLOW-ON
          CORRESPONDENCE.
        </p>
        <p>NO REFUNDS ARE ALLOWED.</p>
      </v-col>
    </v-row>
    <v-divider></v-divider>
    <v-container fluid>
      <v-simple-table dense>
        <template #default>
          <tbody>
            <tr
              v-for="item in desserts"
              :key="item.label"
            >
              <td>{{ item.label }}</td>
              <td>{{ item.value }}</td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
    </v-container>
    <v-row>
      <v-col>
        <p>NO REFUNDS ARE ALLOWED.</p>
        <p>
          A COPY OF THIS RECEIPT SHOULD BE KEPT AND REFERENCED IN ALL FOLLOW-ON
          CORRESPONDENCE.
        </p>
      </v-col>
    </v-row>
  </div>
</template>
<script setup lang="ts">
import { capitalize } from '@shared-utils/formatters/defaultFormatters';
import { computed } from 'vue';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const brandStore = useBrandStore();
const permitStore = usePermitsStore();

const licenseType = computed(() => {
  if (
    permitStore.getPermitDetail.application.applicationType.startsWith('renew')
  ) {
    return 'Renew';
  }

  if (
    permitStore.getPermitDetail.application.applicationType.startsWith('modify')
  ) {
    return 'Modify';
  }

  return 'New';
});

const desserts = [
  {
    label: 'Transaction Date:',
    value: '03/02/2021 20:27:49',
  },
  {
    label: 'Name:',
    value: `${permitStore.getPermitDetail.application.personalInfo.lastName},${permitStore.getPermitDetail.application.personalInfo.firstName}`,
  },
  {
    label: 'Purchased:',
    value: `${licenseType.value} ${capitalize(
      permitStore.getPermitDetail.application.applicationType
    )} 2 YEAR CCW License`,
  },
  {
    label: 'Order Number:',
    value: `${permitStore.getPermitDetail.application.orderId}`,
  },
  {
    label: 'Vendor Info',
    value: '',
  },
  {
    label: 'Acct Number:',
    value: 'XXXX-XXXX-XXXX-XXXX',
  },
  {
    label: 'Card Type:',
    value: '',
  },
  {
    label: 'Total Charge:',
    value: `$ ${
      brandStore.getBrand.cost[licenseType.value.toLowerCase()][
        permitStore.getPermitDetail.application.applicationType
      ]
    }`,
  },
  {
    label: 'Authorization Code:',
    value: '21793Z',
  },
  {
    label: 'ADV Code',
    value: '',
  },
  {
    label: 'Transaction Id:',
    value: 62886007826,
  },
];
</script>
<style lang="scss" scoped>
@page {
  margin-top: 5cm;
  margin-left: 2cm;
  margin-bottom: 2cm;
  margin-right: 2cm;
}
</style>
