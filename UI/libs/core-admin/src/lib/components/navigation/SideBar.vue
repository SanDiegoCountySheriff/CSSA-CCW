<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/singleline-html-element-content-newline -->
<template>
  <v-navigation-drawer
    app
    v-model="drawer"
    :mini-variant.sync="mini"
    class="sidebar"
    permanent
    floating
  >
    <v-list
      class="mt-1"
      nav
    >
      <v-list-item
        class="px-2 logo"
        :to="Routes.HOME_ROUTE_PATH"
        link
        style="background: none"
      >
        <v-list-item-avatar
          height="32"
          width="32"
          color="black"
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
          <v-list-item-title class="font-weight-medium">
            {{ getAppTitle.name }} <small> {{ getAppTitle.env }} </small>
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>

      <v-card class="mt-6 mb-6 elevation-0">
        <v-list-item>
          <SearchBar />
        </v-list-item>
      </v-card>

      <v-list
        nav
        dense
      >
        <v-list-item style="display: none"></v-list-item>
        <v-card class="mt-2 mb-2 elevation-0">
          <v-list-item
            :to="Routes.HOME_ROUTE_PATH"
            link
          >
            <v-list-item-icon>
              <v-icon>mdi-view-dashboard</v-icon>
            </v-list-item-icon>
            <v-list-item-title>{{ $t('Dashboard') }}</v-list-item-title>
          </v-list-item>
        </v-card>
        <v-card class="mt-2 mb-2 elevation-0">
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
                class="ml-5 font-weight-bold"
                :color="$vuetify.theme.dark ? '' : 'light-blue lighten-4'"
                x-small
              >
                {{ aptStore.getNewAptCount }}
              </v-chip>
            </v-list-item-title>
          </v-list-item>
        </v-card>
        <v-card class="mt-2 mb-2 elevation-0">
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
                class="ml-8 font-weight-bold"
                :color="$vuetify.theme.dark ? '' : 'light-blue lighten-4'"
                x-small
              >
                {{ permitStore.getOpenPermits }}
              </v-chip>
            </v-list-item-title>
          </v-list-item>
        </v-card>
        <v-card class="mt-2 mb-2 elevation-0">
          <v-list-item
            :to="Routes.SETTINGS_ROUTE_PATH"
            link
          >
            <v-list-item-icon>
              <v-icon>mdi-cog</v-icon>
            </v-list-item-icon>
            <v-list-item-title>{{ $t('Admin Settings') }}</v-list-item-title>
          </v-list-item>
        </v-card>
      </v-list>

      <v-list
        class="bottom-list"
        nav
        dense
      >
        <v-card class="mt-2 mb-2 elevation-0">
          <v-list-item
            link
            @click="mini = !mini"
          >
            <v-list-item-icon>
              <v-icon>
                {{ mini ? 'mdi-menu-right-outline' : 'mdi-menu-left-outline' }}
              </v-icon>
            </v-list-item-icon>
            <v-list-item-title>{{ $t('Collapse Menu') }}</v-list-item-title>
          </v-list-item>
        </v-card>
      </v-list>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import Routes from '@core-admin/router/routes';
import SearchBar from '@core-admin/components/search/SearchBar.vue';
import { ref } from 'vue';
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import useEnvName from '@shared-ui/composables/useEnvName';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const mini = ref(false);
const drawer = ref(true);
const aptStore = useAppointmentsStore();
const permitStore = usePermitsStore();
const brandStore = useBrandStore();

const { isLoading } = useQuery(['logo']);

const getAppTitle = useEnvName();
</script>

<style lang="scss" scoped>
.theme--light.sidebar {
  background: #f5f5f5;
}
.sidebar {
  max-width: 265px;

  .v-avatar {
    min-width: 32px !important;
  }

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

  .theme--light.v-navigation-drawer {
    background-color: #f7f9fb !important;
  }

  .theme--dark {
    .logo {
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

  .v-list .v-list-item--active .theme--light {
    background: #eff8ff;
    color: #2e90fa;

    &__title {
      color: #2e90fa;
    }
  }

  .v-list-item {
    height: 46px;

    &__icon {
      margin-top: 10px;
      min-width: 20px;
    }

    &__title {
      text-align: left;
      font-size: 16px;
      font-weight: 400;
      line-height: 26px;
      color: #667085;
    }

    &__subtitle {
      text-align: left;
    }

    &:not(:last-child):not(:only-child) {
      margin-bottom: 8px;
    }
  }
  .bottom-list {
    .v-card {
      position: absolute;
      bottom: 20px;
      width: 240px;
    }
  }
}
</style>
