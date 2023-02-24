<template>
  <v-app-bar
    app
    color="accent"
    class="flex-grow-0 white--text"
    clipped-right
  >
    <v-btn
      text
      color="white"
      large
      v-if="authStore.getAuthState.isAuthenticated"
      @click="handleEditAdminUser"
    >
      {{ authStore.getAuthState.userName }}
      <v-icon
        right
        dark
      >
        mdi-cog
      </v-icon>
    </v-btn>

    <v-dialog
      v-model="adminUserNotFound"
      persistent
      max-width="800px"
    >
      <v-card :loading="isLoading">
        <v-card-title class="headline">
          {{ $t('Setup User Information') }}
        </v-card-title>
        <v-card-text>
          <v-form v-model="valid">
            <v-text-field
              v-model="adminUser.badgeNumber"
              label="Badge Number"
              :rules="[v => !!v || 'Badge Number is required']"
            ></v-text-field>
          </v-form>
          <div class="text-h6">Signature</div>
          <canvas
            width="500px"
            height="100px"
            id="signature"
            class="signature"
          ></canvas>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="primary"
            text
            @click="handleClearSignature"
          >
            {{ $t('Clear Signature') }}
          </v-btn>
          <v-btn
            color="primary"
            text
            :disabled="!valid"
            @click="handleSaveAdminUser"
          >
            {{ $t('Save') }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-spacer></v-spacer>
    <div class="mr-4 ml-1">
      <ThemeMode />
    </div>
    <div
      v-if="
        authStore.getAuthState.isAuthenticated && $vuetify.breakpoint.mdAndUp
      "
      class="caption font-weight-bold mr-4 ml-1"
    >
      {{ $t('Session started at') }} {{ formatTime(sessionTime) }}
    </div>
    <v-btn
      v-if="authStore.getAuthState.isAuthenticated"
      aria-label="Sign out of application"
      @click="signOut"
      class="mr-4 ml-1"
      :color="$vuetify.breakpoint.mdAndDown ? 'accent' : 'accent lighten-2'"
      small
    >
      <!--eslint-disable-next-line vue/singleline-html-element-content-newline -->
      <v-icon
        v-if="$vuetify.breakpoint.mdAndDown"
        class="pr-1 white--text"
      >
        mdi-logout-variant
      </v-icon>
      <span
        v-else
        class="white--text"
        >{{ $t('Sign out') }}</span
      >
    </v-btn>
  </v-app-bar>
</template>

<script setup lang="ts">
import {
  AdminUserType,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes';
import SignaturePad from 'signature_pad';
import ThemeMode from '@shared-ui/components/mode/ThemeMode.vue';
import auth from '@shared-ui/api/auth/authentication';
import { defaultAdminUser } from '@shared-utils/lists/defaultConstants';
import { formatTime } from '@shared-utils/formatters/defaultFormatters';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { useDocumentsStore } from '@core-admin/stores/documentsStore';
import { useMutation } from '@tanstack/vue-query';
import { computed, nextTick, onBeforeUnmount, onMounted, ref } from 'vue';

const authStore = useAuthStore();
const brandStore = useBrandStore();
const documentsStore = useDocumentsStore();
const sessionTime = computed(() => authStore.getAuthState.sessionStarted);
const adminUserNotFound = ref(false);
const adminUser = ref<AdminUserType>(defaultAdminUser);
const valid = ref(false);
const signaturePad = ref<SignaturePad>();

const { isLoading, mutate: createAdminUser } = useMutation(
  ['createAdminUser'],
  () => authStore.putCreateAdminUserApi(adminUser.value),
  {
    onSuccess: () => {
      adminUserNotFound.value = false;
    },
  }
);

let silentRefresh;

onMounted(() => {
  if (authStore.getAuthState.isAuthenticated) {
    silentRefresh = setInterval(
      auth.acquireToken,
      brandStore.getBrand.refreshTokenTime * 1000 * 60
    );
  }

  if (!authStore.getAuthState.adminUser.badgeNumber) {
    adminUserNotFound.value = true;
  } else {
    adminUser.value = authStore.getAuthState.adminUser;
  }
});

onBeforeUnmount(() => clearInterval(silentRefresh));

async function signOut() {
  await auth.signOut();
}

function handleEditAdminUser() {
  adminUserNotFound.value = !adminUserNotFound.value;
  nextTick(() => {
    const canvas = document.getElementById('signature') as HTMLCanvasElement;

    signaturePad.value = new SignaturePad(canvas, {
      backgroundColor: 'rgb(255, 255, 255)',
    });
  });
}

function handleClearSignature() {
  signaturePad.value?.clear();
}

async function handleSaveAdminUser() {
  // create the form data file
  const form = new FormData();
  const canvas = document.getElementById('signature') as HTMLCanvasElement;

  canvas.toBlob(async blob => {
    form.append('fileToUpload', blob as Blob);

    // upload the file through the document store
    await documentsStore.postUploadAdminUserFile(form);

    // create the uploadedDocType
    const uploadDoc: UploadedDocType = {
      documentType: 'adminUserSignature',
      name: 'jake.png',
      uploadedBy: 'me',
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    };

    // attach to the adminUser.value
    adminUser.value.uploadedDocuments.push(uploadDoc);

    // createAdminUser();
    await createAdminUser();
  });
}
</script>

<style lang="scss" scoped>
.signature {
  border: 2px solid black;
  border-radius: 5px;
}
</style>
