<!-- eslint-disable vue-a11y/form-has-label -->
<!-- eslint-disable vue/multiline-html-element-content-newline -->
<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container class="px-0 py-0">
    <v-row>
      <v-col
        cols="4"
        class="pt-0 pr-0"
      >
        <v-card
          v-if="isLoading"
          height="200"
          outlined
        >
          <v-skeleton-loader type="list-item,divider,list-item" />
        </v-card>

        <v-card
          v-else
          class="text-left"
          height="200"
          outlined
        >
          <v-card-title class="justify-center">
            {{ permitStore.getPermitDetail.application.personalInfo.lastName }},
            {{ permitStore.getPermitDetail.application.personalInfo.firstName }}
          </v-card-title>
          <v-card-subtitle class="py-1">
            <v-row>
              <v-col>
                <div class="text-no-wrap">
                  Date of Birth:
                  {{
                    formatDate(
                      permitStore.getPermitDetail.application.dob.birthDate
                    )
                  }}
                </div>
              </v-col>
              <v-col>
                <div class="text-no-wrap">
                  ID Number:
                  {{ permitStore.getPermitDetail.application.idInfo.idNumber }}
                </div>
              </v-col>
            </v-row>
          </v-card-subtitle>

          <v-divider></v-divider>

          <v-card-text>
            <v-row align="center">
              <v-col cols="8">
                <v-row>
                  <v-col
                    cols="6"
                    sm="6"
                  >
                    <FileUploadDialog
                      :icon="'mdi-camera'"
                      :default-selection="'portrait'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                  <v-col
                    cols="6"
                    sm="6"
                  >
                    <FileUploadDialog
                      :icon="'mdi-fingerprint'"
                      :default-selection="'thumbprint'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                </v-row>
                <v-row>
                  <v-col
                    cols="6"
                    sm="6"
                  >
                    <FileUploadDialog
                      :icon="'mdi-file-upload'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                  <v-col
                    cols="6"
                    sm="6"
                  >
                    <v-menu bottom>
                      <template #activator="{ on, attrs }">
                        <v-btn
                          block
                          v-bind="attrs"
                          v-on="on"
                          color="primary"
                          small
                        >
                          <v-icon>mdi-printer</v-icon>
                        </v-btn>
                      </template>
                      <v-list>
                        <v-list-item @click="printPdf('printApplicationApi')">
                          <v-list-item-title
                            >Print Application</v-list-item-title
                          >
                        </v-list-item>
                        <v-list-item
                          @click="printPdf('printOfficialLicenseApi')"
                        >
                          <v-list-item-title
                            >Print Official License</v-list-item-title
                          >
                        </v-list-item>
                        <v-list-item
                          @click="printPdf('printUnofficialLicenseApi')"
                        >
                          <v-list-item-title
                            >Print Unofficial License</v-list-item-title
                          >
                        </v-list-item>
                        <v-list-item @click="printLivescan">
                          <v-list-item-title
                            >Print LiveScan Document</v-list-item-title
                          >
                        </v-list-item>
                      </v-list>
                    </v-menu>
                  </v-col>
                </v-row>
              </v-col>
              <v-col
                align="center"
                cols="4"
              >
                <v-img
                  id="user-photo"
                  :src="
                    state.userPhoto ? state.userPhoto : 'img/icons/no-photo.png'
                  "
                  alt="user-photo"
                  height="100"
                  width="100"
                  contain
                />
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col
        cols="4"
        class="pt-0 pr-0"
      >
        <v-card
          v-if="isLoading"
          height="200"
          outlined
        >
          <v-skeleton-loader type="list-item,divider,list-item" />
        </v-card>

        <v-card
          v-else
          height="200"
          class="d-flex flex-column"
          outlined
        >
          <v-card-title
            v-if="!permitStore.getPermitDetail.application.isComplete"
            class="justify-center"
          >
            <v-icon
              color="error"
              class="mr-2"
              >mdi-alert</v-icon
            >
            Missing Requirement
          </v-card-title>

          <v-card-title
            v-else
            class="justify-center"
          >
            <v-icon
              color="success"
              class="mr-2"
              >mdi-shield-check</v-icon
            >
            Requirements Fulfilled
          </v-card-title>

          <v-spacer></v-spacer>

          <v-card-text class="text-center">
            <v-btn
              color="primary"
              :href="`mailto:${permitStore.getPermitDetail.application.userEmail}`"
              target="_blank"
            >
              <v-icon left> mdi-email-outline </v-icon>
              Send Request
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col
        cols="4"
        class="pt-0"
      >
        <v-card
          v-if="isLoading"
          height="200"
          outlined
        >
          <v-skeleton-loader type="list-item,divider,list-item" />
        </v-card>

        <v-card
          v-else
          height="200"
          class="d-flex flex-column"
          outlined
        >
          <v-card-title
            v-if="permitStore.getPermitDetail.application.appointmentDateTime"
            class="justify-center"
          >
            <v-icon
              color="success"
              class="mr-2"
              >mdi-shield-check</v-icon
            >
            {{ appointmentTime }} on {{ appointmentDate }}
          </v-card-title>

          <v-card-title
            v-else
            class="justify-center"
          >
            <v-icon
              color="error"
              class="mr-2"
              >mdi-alert</v-icon
            >
            Not Scheduled
          </v-card-title>

          <v-spacer></v-spacer>

          <v-card-text class="text-center">
            <v-row>
              <v-col>
                <v-btn
                  color="primary"
                  small
                  block
                  >No Show</v-btn
                >
              </v-col>
              <v-col>
                <v-btn
                  color="primary"
                  small
                  block
                  >Check In</v-btn
                >
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <DateTimePicker
                  v-model="datetime"
                  label
                />
              </v-col>
              <v-col>
                <Schedule />
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-snackbar
      v-model="state.snackbar"
      :multi-line="state.multiLine"
    >
      {{ state.text }}

      <template #action="{ attrs }">
        <v-btn
          :color="$vuetify.theme.dark ? '' : 'red'"
          text
          v-bind="attrs"
          @click="state.snackbar = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>
<script setup lang="ts">
import DateTimePicker from '@core-admin/components/appointment/DateTimePicker.vue';
import FileUploadDialog from '@core-admin/components/dialogs/FileUploadDialog.vue';
import Schedule from '@core-admin/components/appointment/Schedule.vue';
import { formatDate } from '@shared-utils/formatters/defaultFormatters';
import { liveScanUrl } from '@shared-utils/lists/defaultConstants';
import { useDocumentsStore } from '@core-admin/stores/documentsStore';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useRoute } from 'vue-router/composables';
import { computed, onMounted, reactive, ref } from 'vue';

const state = reactive({
  isSelecting: false,
  snack: false,
  snackColor: '',
  snackText: '',
  multiLine: false,
  snackbar: false,
  text: `Invalid file type provided.`,
  userPhoto: '',
});

const datetime = ref(null);
const route = useRoute();
const permitStore = usePermitsStore();
const documentsStore = useDocumentsStore();

const allowedExtension = [
  '.png',
  '.jpeg',
  '.jpg',
  '.pjp',
  '.pjpeg',
  '.jfif',
  '.bmp',
];

const { isLoading, refetch } = useQuery(
  ['permitDetail', route.params.orderId],
  () => permitStore.getPermitDetailApi(route.params.orderId)
);

onMounted(() => {
  permitStore.getPermitDetailApi(route.params.orderId).then(() => {
    documentsStore.getApplicationDocumentApi('portrait').then(res => {
      state.userPhoto = res;
    });
  });
});

function onFileChanged(e: File, target: string) {
  if (allowedExtension.some(ext => e.name.toLowerCase().endsWith(ext))) {
    if (target === 'protrait') {
      const reader = new FileReader();

      reader.onload = event => {
        let img = document.getElementById('user-photo');

        img?.setAttribute('src', event.target.result);
        img?.setAttribute('width', '100');
        img?.setAttribute('height', '100');
      };

      reader.readAsDataURL(e);
    }

    documentsStore
      .setUserApplicationFile(e, target)
      .then(() => {
        state.text = 'Successfully uploaded file.';
        state.snackbar = true;
      })
      .catch(() => {
        state.text = 'An API error occurred.';
        state.snackbar = true;
      });
  } else {
    state.text = 'Invalid file type provided.';
    state.snackbar = true;
  }

  refetch();
}

function printPdf(type) {
  permitStore[type]().then(res => {
    // eslint-disable-next-line node/no-unsupported-features/node-builtins
    let fileURL = URL.createObjectURL(res.data);

    window.open(fileURL);
  });
}

function printLivescan() {
  let a = document.createElement('a');

  a.href = liveScanUrl;
  a.target = '_blank';
  a.download = 'livescan.pdf';

  document.body.appendChild(a);
  a.click();
}

const appointmentDate = computed(
  () =>
    new Date(
      permitStore.getPermitDetail?.application.appointmentDateTime
    )?.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    }) || ''
);

const appointmentTime = computed(() => {
  const date = new Date(
    permitStore.getPermitDetail?.application.appointmentDateTime
  );

  return date.toLocaleTimeString('en-US', {
    hour12: true,
    timeStyle: 'short',
  });
});
</script>
