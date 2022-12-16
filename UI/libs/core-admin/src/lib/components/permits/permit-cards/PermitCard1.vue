<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    class="mt-3 ml-8 mr-8 fixed-permit-card"
    elevation="2"
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
    <v-row
      class="ml-5"
      v-else
    >
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          elevation="0"
          class="text-left"
        >
          <div class="font-weight-bold grey--text text--darken-3">
            Application #{{ permitStore.getPermitDetail.application.orderId }}
          </div>
          <span class="grey--text body-2">
            Submitted on {{ submittedDate }}</span
          >
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          elevation="0"
          class="mt-2"
        >
          <v-chip
            class="ml-4"
            :color="$vuetify.theme.dark ? '' : 'blue lighten-4'"
            :text-color="$vuetify.theme.dark ? '' : 'blue'"
          >
            New ...
          </v-chip>
          <v-chip
            class="ml-4"
            :text-color="$vuetify.theme.dark ? '' : 'grey darken-2'"
          >
            2 year ...
          </v-chip>
          <v-chip
            class="ml-4"
            :color="$vuetify.theme.dark ? '' : 'green lighten-3'"
            :text-color="$vuetify.theme.dark ? '' : 'green darken-4'"
          >
            <v-icon
              left
              small
            >
              mdi-checkbox-marked-circle
            </v-icon>
            Paid ...
          </v-chip>
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          elevation="0"
          class="text-right mr-4 mt-2"
        >
          <v-badge
            bordered
            :color="$vuetify.theme.dark ? '' : 'blue'"
            icon="mdi-sticker-check"
            :value="permitStore.getPermitDetail.application.status === 5"
            overlap
          >
            <v-chip
              class="ml-4"
              :color="$vuetify.theme.dark ? '' : 'warning lighten-5'"
              :text-color="$vuetify.theme.dark ? '' : 'warning'"
              @click="updateApplicationStatus(5)"
              label
            >
              {{ $t(' Withdraw') }}
            </v-chip>
          </v-badge>
          <v-badge
            bordered
            :color="$vuetify.theme.dark ? '' : 'blue'"
            icon="mdi-sticker-check"
            :value="permitStore.getPermitDetail.application.status === 4"
            overlap
          >
            <v-chip
              class="ml-4"
              :color="$vuetify.theme.dark ? '' : 'error lighten-5'"
              :text-color="$vuetify.theme.dark ? '' : 'error'"
              @click="updateApplicationStatus(4)"
              label
            >
              {{ $t('Deny') }}
            </v-chip>
          </v-badge>
          <v-badge
            bordered
            :color="$vuetify.theme.dark ? '' : 'blue'"
            icon="mdi-sticker-check"
            :value="permitStore.getPermitDetail.application.status === 6"
            overlap
          >
            <v-chip
              class="ml-4"
              :color="$vuetify.theme.dark ? '' : 'green lighten-3'"
              :text-color="$vuetify.theme.dark ? '' : 'green darken-4'"
              @click="updateApplicationStatus(6)"
              label
            >
              {{ $t('Approve') }}
            </v-chip>
          </v-badge>
        </v-card>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import { computed } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useRoute } from 'vue-router/composables';

const route = useRoute();
const permitStore = usePermitsStore();

const { isLoading } = useQuery(['permitDetail', route.params.orderId], () =>
  permitStore.getPermitDetailApi(route.params.orderId)
);

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  permitStore.updatePermitDetailApi,
  {
    enabled: false,
  }
);

const submittedDate = computed(
  () =>
    new Date(
      permitStore.getPermitDetail?.history[0]?.changeDateTimeUtc
    )?.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    }) || ''
);

function updateApplicationStatus(status) {
  permitStore.getPermitDetail.application.status = status;
  updatePermitDetails();
}
</script>
<style lang="scss" scoped>
.fixed-permit-card {
  position: -webkit-sticky;
  position: sticky;
  top: 4rem;
  z-index: 7;

  .v-tabs-bar__content {
    padding-top: 15px;
  }
}
</style>
