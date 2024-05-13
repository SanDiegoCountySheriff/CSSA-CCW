<template>
  <div>
    <v-navigation-drawer
      app
      v-model="drawer"
      :mini-variant.sync="mini"
      @transitionend="onTransitionEnd"
      :color="app?.proxy.$vuetify.theme.dark ? 'grey darken-4' : ''"
    >
      <v-list nav>
        <v-list-item
          @click="$router.push(Routes.HOME_ROUTE_PATH)"
          class="px-0"
        >
          <v-list-item-avatar class="mr-1">
            <v-img
              :src="brandStore.getDocuments.agencyLogo"
              alt="Image"
              contain
            />
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title :class="wrapText ? 'text-wrap' : ''">
              {{ getAppTitle.name }} <small>{{ getAppTitle.env }}</small>
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <SearchBar />
        </v-list-item>

        <v-list dense>
          <v-list-item
            :to="Routes.HOME_ROUTE_PATH"
            link
          >
            <v-list-item-icon>
              <v-icon>mdi-view-dashboard</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title class="text-left">
                {{ $t('Dashboard') }}
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item
            :to="Routes.PERMITS_ROUTE_PATH"
            link
          >
            <v-list-item-icon>
              <v-icon>mdi-file-document</v-icon>
            </v-list-item-icon>
            <v-list-item-title class="text-left">
              {{ $t('Applications') }}
              <v-chip
                class="float-right"
                color="primary"
                x-small
              >
                {{ permitStore.summaryCount?.submittedStatus }}
              </v-chip>
            </v-list-item-title>
          </v-list-item>

          <v-list-item
            v-if="authStore.auth.roles.includes('CCW-SYSTEM-ADMINS-ROLE')"
            :to="Routes.APPOINTMENT_MANAGEMENT_ROUTE_PATH"
            link
          >
            <v-list-item-icon>
              <v-icon>mdi-calendar-clock</v-icon>
            </v-list-item-icon>
            <v-list-item-title class="text-left">
              {{ $t('Appointment Management') }}
            </v-list-item-title>
          </v-list-item>

          <v-list-item
            v-if="authStore.auth.roles.includes('CCW-ADMIN-ROLE')"
            :to="Routes.REFUND_REQUESTS_PATH"
            link
          >
            <v-list-item-icon>
              <v-icon>mdi-credit-card-refund</v-icon>
            </v-list-item-icon>
            <v-list-item-title class="text-left">
              {{ $t('Refund Requests') }}
              <v-chip
                class="float-right"
                color="primary"
                x-small
              >
                {{ paymentStore.refundRequestCount }}
              </v-chip>
            </v-list-item-title>
          </v-list-item>

          <v-list-item
            v-if="authStore.auth.roles.includes('CCW-ADMIN-ROLE')"
            :to="Routes.EXISTING_APPLICANTS_PATH"
            link
          >
            <v-list-item-icon>
              <v-icon>mdi-account-star</v-icon>
            </v-list-item-icon>
            <v-list-item-title class="text-left">
              {{ $t('Existing Applicants') }}
              <v-chip
                class="float-right"
                color="primary"
                x-small
              >
                {{ userStore.unmatchedUsersCount }}
              </v-chip>
            </v-list-item-title>
          </v-list-item>

          <v-list-item
            :to="Routes.SETTINGS_ROUTE_PATH"
            link
          >
            <v-list-item-icon>
              <v-icon>mdi-cog</v-icon>
            </v-list-item-icon>
            <v-list-item-title class="text-left">
              {{ $t('Admin Settings') }}
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </v-list>

      <template #append>
        <v-list
          dense
          nav
        >
          <v-list-item @click="mini = !mini">
            <v-list-item-icon>
              <v-icon>
                {{ mini ? 'mdi-menu-right-outline' : 'mdi-menu-left-outline' }}
              </v-icon>
            </v-list-item-icon>

            <v-list-item-title class="text-left">
              {{ $t('Collapse Menu') }}
            </v-list-item-title>
          </v-list-item>

          <v-list-item>
            <v-list-item-title class="text-center">
              {{ getVersion }}
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </template>
    </v-navigation-drawer>
  </div>
</template>

<script setup lang="ts">
import Routes from '@core-admin/router/routes'
import SearchBar from '@core-admin/components/search/SearchBar.vue'
import VERSION from '@shared-utils/version'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import useEnvName from '@shared-ui/composables/useEnvName'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useUserStore } from '@shared-ui/stores/userStore'
import { computed, getCurrentInstance, ref, watch } from 'vue'

interface ISideBarProps {
  expandMenu: boolean
}

const props = withDefaults(defineProps<ISideBarProps>(), {
  expandMenu: true,
})

const emit = defineEmits(['on-change-drawer'])

const mini = ref(false)
const wrapText = ref(true)
const drawer = ref(true)
const authStore = useAuthStore()
const permitStore = usePermitsStore()
const paymentStore = usePaymentStore()
const brandStore = useBrandStore()
const userStore = useUserStore()
const app = getCurrentInstance()

const getAppTitle = useEnvName()
const getVersion = computed(() => VERSION)

function onTransitionEnd() {
  mini.value ? (wrapText.value = false) : (wrapText.value = true)
}

watch(
  () => props.expandMenu,
  () => {
    drawer.value = !drawer.value
  }
)
watch(
  () => drawer.value,
  () => {
    emit('on-change-drawer')
  }
)
</script>
