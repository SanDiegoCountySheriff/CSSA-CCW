<template>
  <div class="mode">
    <v-tooltip bottom>
      <template #activator="{ on, attrs }">
        <v-btn
          aria-label="Change dark/light mode"
          icon
          small
          @click="handleThemeChange"
          v-bind="attrs"
          v-on="on"
        >
          <v-icon class="white--text">
            {{ getThemeIcon }}
          </v-icon>
        </v-btn>
      </template>
      <span>{{ getThemeTooltip }}</span>
    </v-tooltip>
  </div>
</template>
<script setup lang="ts">
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { computed, getCurrentInstance } from 'vue'

const app = getCurrentInstance()
const themeStore = useThemeStore()

const getThemeIcon = computed(() =>
  app?.proxy?.$vuetify.theme.dark
    ? 'mdi-lightbulb-on-outline'
    : 'mdi-moon-last-quarter'
)

const getThemeTooltip = computed(() =>
  app?.proxy?.$vuetify.theme.dark ? 'View light mode' : 'View dark mode'
)

function handleThemeChange() {
  app.proxy.$vuetify.theme.dark = !app?.proxy?.$vuetify.theme.dark
  themeStore.setThemeConfig({ isDark: app.proxy.$vuetify.theme.dark })
}
</script>

<style lang="scss" scoped>
.mode {
  .v-list-item {
    position: absolute;
    bottom: 60px;
    height: 46px;

    &__title {
      color: #667085;
      font-size: 16px;
      line-height: 26px;
      text-align: left;
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
