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
          :to="Routes.DASHBOARD_ROUTE_PATH"
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
          <v-list-item-title>{{ $t('Permits') }}</v-list-item-title>
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
          <v-list-item-title>{{ $t('System Settings') }}</v-list-item-title>
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
import { useAppConfigStore } from '@shared-ui/stores/appConfig';
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';
import { useBrandStore } from '@core-admin/stores/brandStore';
import { useQuery } from '@tanstack/vue-query';
import { computed, ref } from 'vue';

const drawer = ref(true);
const aptStore = useAppointmentsStore();
const appStore = useAppConfigStore();
const brandStore = useBrandStore();

const { isLoading } = useQuery(['logo']);

const getAppTitle = computed(() => {
  switch (appStore.getAppConfig.environmentName) {
    case 'DEV':
      return 'CCW (DEV)';
    case 'QA':
      return 'CCW (QA)';
    default:
      return 'CCW';
  }
});
</script>

<style lang="scss" scoped>
.sidebar {
  max-width: 265px;

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
