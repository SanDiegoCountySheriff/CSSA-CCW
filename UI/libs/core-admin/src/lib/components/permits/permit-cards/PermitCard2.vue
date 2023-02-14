<!-- eslint-disable vue-a11y/form-has-label -->
<!-- eslint-disable vue/multiline-html-element-content-newline -->
<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    class="mt-6 mb-2"
    elevation="0"
    :color="$vuetify.theme.dark ? '#303030' : ''"
  >
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          v-if="isLoading"
          elevation="2"
          fluid
        >
          <v-skeleton-loader
            fluid
            class="fill-height"
            type="list-item,divider,list-item"
          >
          </v-skeleton-loader>
        </v-card>

        <v-card
          class="mx-auto text-left"
          elevation="2"
          height="200"
          v-else
        >
          <v-card-title class="py-1">
            Full Name:
            {{ permitStore.getPermitDetail.application.personalInfo.lastName }},
            {{ permitStore.getPermitDetail.application.personalInfo.firstName }}
          </v-card-title>
          <v-card-subtitle
            class="py-2"
            :style="$vuetify.theme.dark ? '' : { color: '#111 !important' }"
          >
            <v-row>
              <v-col>
                Date of Birth:
                <span class="ml-1">
                  {{
                    formatDate(
                      permitStore.getPermitDetail.application.dob.birthDate
                    )
                  }}
                </span>
              </v-col>
              <v-col>
                ID Number:
                <span class="ml-1">
                  {{ permitStore.getPermitDetail.application.idInfo.idNumber }}
                </span>
              </v-col>
            </v-row>
          </v-card-subtitle>
          <v-divider class="mb-2"></v-divider>
          <div class="card-1-text p-2">
            <div class="button-container">
              No previous applications
              <div class="button-inner">
                <v-row>
                  <v-col
                    cols="12"
                    xl="3"
                    lg="6"
                    md="12"
                    class="pa-0"
                  >
                    <FileUploadDialog
                      :icon="'mdi-camera'"
                      :default-selection="'portrait'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                  <v-col
                    cols="12"
                    xl="3"
                    lg="6"
                    md="12"
                    class="pa-0"
                  >
                    <FileUploadDialog
                      :icon="'mdi-fingerprint'"
                      :default-selection="'thumbprint'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                  <v-col
                    cols="12"
                    xl="3"
                    lg="6"
                    md="12"
                    class="pa-0"
                  >
                    <FileUploadDialog
                      :icon="'mdi-file-upload'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                  <v-col
                    cols="12"
                    xl="3"
                    lg="6"
                    md="12"
                    class="pa-0"
                  >
                    <div class="file-button-container">
                      <v-btn
                        small
                        :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                        style="cursor: pointer"
                        class="mr-1"
                      >
                        <v-menu bottom>
                          <template #activator="{ on, attrs }">
                            <v-btn
                              v-bind="attrs"
                              v-on="on"
                              icon
                              small
                            >
                              <v-icon>mdi-printer</v-icon>
                            </v-btn>
                          </template>
                          <v-list
                            align="left"
                            justify="left"
                          >
                            <v-list-item
                              @click="printPdf('printApplicationApi')"
                            >
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
                      </v-btn>
                    </div>
                  </v-col>
                </v-row>
              </div>
            </div>
            <div class="ml-7">
              <img
                id="user-photo"
                :src="
                  state.userPhoto
                    ? state.userPhoto
                    : 'https://upload.wikimedia.org/wikipedia/commons/6/65/No-Image-Placeholder.svg'
                "
                alt="unk"
                height="100"
                width="100"
              />
            </div>
          </div>
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
        </v-card>
      </v-col>

      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          v-if="isLoading"
          fluid
        >
          <v-skeleton-loader
            fluid
            class="fill-height"
            type="list-item,divider,list-item"
          >
          </v-skeleton-loader>
        </v-card>
        <v-card
          class="mx-auto"
          elevation="2"
          height="200"
          v-else
        >
          <v-list-item three-line>
            <v-list-item-content>
              <div
                class="text-overline mb-4"
                v-if="!permitStore.getPermitDetail.application.isComplete"
              >
                <v-icon
                  color="error lighten-2"
                  class="mr-1"
                >
                  mdi-alert
                </v-icon>
                Missing Requirement
              </div>
              <div
                class="text-overline mb-4"
                v-else
              >
                <v-icon
                  color="success lighten-2"
                  class="mr-1"
                >
                  mdi-shield-check
                </v-icon>
                Requirement Fulfilled
              </div>
              <!-- <v-list-item-title class="mt-8 font-weight-bold">
              </v-list-item-title> -->
              <v-list-item-content>
                <v-row class="mt-3">
                  <v-col class="button-col"> No Requests Sent </v-col>
                </v-row>
                <v-row>
                  <v-col class="button-col">
                    <v-chip
                      :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                      class="mx-4"
                      :href="`mailto:${permitStore.getPermitDetail.application.userEmail}`"
                      target="_blank"
                      label
                    >
                      <v-icon class="mr-1"> mdi-email-outline </v-icon>
                      Send Request
                    </v-chip>
                  </v-col>
                </v-row>
              </v-list-item-content>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>

      <v-col
        :class="$vuetify.breakpoint.lgAndDown ? 'add-height' : ''"
        cols="12"
        md="4"
        sm="12"
      >
        <v-card
          v-if="isLoading"
          class="mr-8"
          fluid
        >
          <v-skeleton-loader
            fluid
            class="fill-height"
            type="list-item,divider,list-item"
          >
          </v-skeleton-loader>
        </v-card>
        <v-card
          class="mx-auto mr-8"
          elevation="2"
          height="200"
          v-else
        >
          <v-list-item three-line>
            <v-list-item-content>
              <div
                v-if="
                  permitStore.getPermitDetail.application.appointmentDateTime
                "
                class="text-overline mb-4"
              >
                <v-icon
                  color="success lighten-2"
                  class="mr-1"
                >
                  mdi-shield-check </v-icon
                >{{ appointmentTime }} ON {{ appointmentDate }}
              </div>
              <div
                v-else
                class="text-overline mb-4"
              >
                <v-icon
                  color="error lighten-2"
                  class="mr-1"
                >
                  mdi-alert </v-icon
                >Not Scheduled
              </div>

              <v-list-item-content>
                <v-row class="mt-3">
                  <v-col class="button-col">
                    <span class="mr-4 text-decoration-underline"
                      >No-show &nbsp;</span
                    >
                    <span class="mr-4 text-decoration-underline">Check-in</span>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="button-col">
                    <DateTimePicker
                      v-model="datetime"
                      label
                    />
                    <v-dialog
                      v-model="dialog"
                      width="1200"
                    >
                      <template #activator="{ on, attrs }">
                        <v-chip
                          :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                          v-bind="attrs"
                          v-on="on"
                          class="mr-4"
                          label
                        >
                          <v-icon class="mr-1">
                            mdi-calendar-multiple-check
                          </v-icon>
                          Reschedule
                        </v-chip>
                      </template>

                      <v-card>
                        <Schedule />
                      </v-card>
                    </v-dialog>
                  </v-col>
                </v-row>
              </v-list-item-content>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import DateTimePicker from '@core-admin/components/appointment/DateTimePicker.vue';
import FileUploadDialog from '@core-admin/components/dialogs/FileUploadDialog.vue';
import Schedule from '@core-admin/components/appointment/Schedule.vue';
import { liveScanUrl } from '@shared-utils/lists/defaultConstants';
import { formatDate } from '@shared-utils/formatters/defaultFormatters';
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

const dialog = ref(false);
const datetime = ref(null);
const route = useRoute();
const permitStore = usePermitsStore();
const documentsStore = useDocumentsStore();

const allowedExtension = ['.png', '.jpeg'];

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

    let a = document.createElement('a');

    window.open(fileURL);
  });
}

function printLivescan() {
  let a = document.createElement('a');

  a.href = liveScanUrl;
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

  date.setHours(date.getTimezoneOffset());

  return date.toLocaleTimeString('en-US', {
    hour12: true,
    timeStyle: 'short',
  });
});
</script>

<style lang="scss" scoped>
.add-height {
  height: fit-content !important;
}

.button-col {
  margin: 3px;
  padding: 0;
}

.button-container {
  display: flex;
  flex-direction: column;
  justify-content: space-around;
  height: 100px;
  width: 60%;
  padding: 3px;
}

.button-inner {
  display: flex;
  justify-content: space-around;
  margin-right: 1.5em;
}

.card-1-text {
  display: flex;
  width: 100%;
}

.file-button-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 2px 0;
}
</style>
