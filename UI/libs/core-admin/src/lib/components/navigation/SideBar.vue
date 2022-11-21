<template>
  <v-navigation-drawer
    app
    v-model="drawer"
    :mini-variant="$vuetify.breakpoint.mdAndDown"
    class="sidebar"
    permanent
  >
    <v-list
      class="mt-1"
      nav
    >
      <v-list-item
        class="px-2 logo"
        :to="Routes.HOME_ROUTE_PATH"
        link
      >
        <v-list-item-avatar
          height="32"
          width="32"
        >
          <v-skeleton-loader
            v-if="isLoading"
            width="32"
            height="32"
            type="card-avatar"
          />
          <img
            v-else
            :src="brandStore.getDocuments.agencyLogo"
            width="32"
            height="32"
            alt="Image"
            loading="lazy"
          />
        </v-list-item-avatar>
        <v-list-item-content>
          <v-list-item-title class="text-h6">
            {{ getAppTitle }}
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>

      <v-list-item>
        <SearchBar />
      </v-list-item>

      <v-list
        nav
        dense
      >
        <v-list-item
          :to="Routes.HOME_ROUTE_PATH"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-icon>
          <v-list-item-title>{{ $t('Dashboard') }}</v-list-item-title>
        </v-list-item>
        <v-list-item
          :to="Routes.APPOINTMENTS_ROUTE_PATH"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-calendar-blank-outline</v-icon>
          </v-list-item-icon>
          <v-list-item-title>
            {{ $t('Appointments') }}
            <v-chip
              v-if="aptStore.getNewAptCount !== 0"
              class="ml-5"
              color="light-blue lighten-5"
              text-color="blue"
              x-small
            >
              {{ aptStore.getNewAptCount }}
            </v-chip>
          </v-list-item-title>
        </v-list-item>
        <v-list-item
          :to="Routes.PERMITS_ROUTE_PATH"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-file-document</v-icon>
          </v-list-item-icon>
          <v-list-item-title>
            {{ $t('Applications') }}
            <v-chip
              v-if="permitStore.getOpenPermits !== 0"
              class="ml-5"
              color="light-blue lighten-5"
              text-color="blue"
              x-small
            >
              {{ permitStore.getOpenPermits }}
            </v-chip>
          </v-list-item-title>
        </v-list-item>
        <v-list-item
          to="/work"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-clock-outline</v-icon>
          </v-list-item-icon>
          <v-list-item-title>{{ $t('My Work') }}</v-list-item-title>
        </v-list-item>
        <v-list-item
          :to="Routes.NUMBERS_ROUTE_PATH"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-chart-timeline-variant-shimmer</v-icon>
          </v-list-item-icon>
          <v-list-item-title>{{ $t('Numbers') }}</v-list-item-title>
        </v-list-item>
        <v-list-item
          :to="Routes.SETTINGS_ROUTE_PATH"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-cog</v-icon>
          </v-list-item-icon>
          <v-list-item-title>{{ $t('Settings') }}</v-list-item-title>
        </v-list-item>
      </v-list>

      <v-list
        nav
        dense
      >
        <LoginButton />
      </v-list>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import LoginButton from '@core-admin/components/login/LoginButton.vue';
import Routes from '@core-admin/router/routes';
import SearchBar from '@core-admin/components/search/SearchBar.vue';
import { ref } from 'vue';
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';
import { useBrandStore } from '@core-admin/stores/brandStore';
import useEnvName from '@shared-ui/composables/useEnvName';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const drawer = ref(true);
const aptStore = useAppointmentsStore();
const permitStore = usePermitsStore();
const brandStore = useBrandStore();

const { isLoading } = useQuery(['logo']);

const getAppTitle = useEnvName();
</script>

<style lang="scss" scoped>
.sidebar {
  max-width: 265px;

  .v-list--nav {
    padding-left: 4px;
    padding-right: 4px;
  }

  .theme--light .logo {
    &::before {
      opacity: 0 !important;
    }

    .v-list-item__title {
      color: #344054;
    }
  }
  .theme--dark {
    .logo {
      background: transparent !important;

      .v-list-item__title {
        color: white;
      }
    }
    .v-list-item {
      &__title {
        color: white;
      }
    }
  }

  .v-list-item {
    height: 46px;

    &__title {
      text-align: left;
      font-size: 16px;
      line-height: 26px;
      color: #667085;
    }

    &__subtitle {
      text-align: left;
    }

    &:not(:last-child):not(:only-child) {
      margin-bottom: 8px;
    }

    &--active:not(:first-child) {
      background: #eff8ff;
      color: #2e90fa;

      .v-list-item__title {
        color: #2e90fa;
      }
    }

    &--active:first-child {
      background: #ffffff;
    }
  }
}
</style>
