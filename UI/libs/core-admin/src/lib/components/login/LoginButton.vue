<template>
  <div>
    <template v-if="authStore.getAuthState.isAuthenticated">
      <div class="text-center">
        <v-menu
          v-model="menu"
          :close-on-content-click="false"
          :nudge-width="200"
          offset-x
        >
          <template #activator="{ on, attrs }">
            <v-btn
              color="indigo"
              dark
              v-bind="attrs"
              v-on="on"
            >
              {{ authStore.getAuthState.userName }}
            </v-btn>
          </template>

          <v-card>
            <v-list>
              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title>
                    {{ authStore.getAuthState.userName }}
                  </v-list-item-title>
                  <v-list-item-subtitle>
                    {{ $t('Sr Software Engineer') }}
                  </v-list-item-subtitle>
                </v-list-item-content>

                <v-list-item-action>
                  <v-btn
                    :class="fav ? 'red--text' : ''"
                    icon
                    @click="fav = !fav"
                  >
                    <v-icon>{{ $t('mdi-heart') }}</v-icon>
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list>

            <v-divider></v-divider>

            <v-list>
              <v-list-item>
                <v-list-item-action>
                  <v-switch
                    v-model="message"
                    color="purple"
                  ></v-switch>
                </v-list-item-action>
                <v-list-item-title>
                  {{ $t('Enable messages') }}
                </v-list-item-title>
              </v-list-item>

              <v-list-item>
                <v-list-item-action>
                  <v-switch
                    v-model="hints"
                    color="purple"
                  ></v-switch>
                </v-list-item-action>
                <v-list-item-title>{{ $t('Enable hints') }}</v-list-item-title>
              </v-list-item>
            </v-list>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="primary"
                text
                @click="acquireToken"
              >
                {{ $t('Acquire Token') }}
              </v-btn>
              <v-btn
                color="secondary"
                text
                @click="logout"
              >
                {{ $t('Logout') }}
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-menu>
      </div>
    </template>
    <div v-else>
      <Button
        color="primary"
        text="Login"
        @click="handleLogIn"
      >
      </Button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import Button from '@shared-ui/components/Button.vue';
import auth from '@shared-ui/api/auth/authentication';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useQuery } from '@tanstack/vue-query';

const menu = ref(false);
const fav = ref(true);
const message = ref(false);
const hints = ref(true);

const authStore = useAuthStore();

onMounted(() => {
  if (authStore.getAuthState.isAuthenticated) {
    useQuery(['verifyEmail'], authStore.postVerifyUserApi);
  }
});

async function acquireToken() {
  await auth.acquireToken();
}

async function logout() {
  await auth.signOut();
}

function handleLogIn() {
  auth.signIn();
}
</script>
