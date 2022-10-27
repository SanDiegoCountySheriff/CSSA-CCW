<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/singleline-html-element-content-newline -->
<template>
  <v-navigation-drawer
    app
    v-model="drawer"
    :mini-variant="$vuetify.breakpoint.mdAndDown"
    :clipped="$vuetify.breakpoint.mdAndUp"
    class="sidebar"
    permanent
  >
    <v-list>
      <v-list-item
        class="px-2 logo"
        to="/"
        link
      >
        <v-list-item-avatar
          height="36"
          width="36"
        >
          <img
            src="/img/icons/agency_logo.svg"
            width="36"
            height="36"
            alt="Image"
            loading="lazy"
          />
        </v-list-item-avatar>
        <v-list-item-content>
          <v-list-item-title class="text-h6"> CCW </v-list-item-title>
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
          to="dashboard"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-icon>
          <v-list-item-title>{{ $t('Dashboard') }}</v-list-item-title>
        </v-list-item>
        <v-list-item
          to="/appointment"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-calendar-blank-outline</v-icon>
          </v-list-item-icon>
          <v-list-item-title>{{ $t('Schedule') }}</v-list-item-title>
        </v-list-item>
        <v-list-item
          to="/permits"
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
          to="/numbers"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-chart-timeline-variant-shimmer</v-icon>
          </v-list-item-icon>
          <v-list-item-title>{{ $t('Numbers') }}</v-list-item-title>
        </v-list-item>
        <v-list-item
          to="/brand"
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
import { ref } from 'vue';
import LoginButton from '@core-admin/components/login/LoginButton.vue';
import SearchBar from '@core-admin/components/search/SearchBar.vue';

const drawer = ref(true);
</script>

<style lang="scss" scoped>
.sidebar {
  max-width: 260px;
  .logo {
    .v-list-item__title {
      color: #344054;
    }
  }

  .v-list-item {
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
  }
}
</style>
