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
          height="150"
          v-else
        >
          <v-list-item three-line>
            <v-list-item-content>
              <div class="text-overline mb-4 d-flex">
                Date of Birth:
                <span class="ml-1">
                  {{
                    formatDate(
                      permitStore.getPermitDetail.application.dob.birthDate
                    )
                  }}
                </span>
                <v-spacer></v-spacer>
                <v-spacer></v-spacer>
                <v-spacer></v-spacer>
                ID:
                <span class="ml-1">
                  {{ permitStore.getPermitDetail.application.idInfo.idNumber }}
                </span>
              </div>
              <v-list-item-title class="font-weight-bold">
                <v-divider class="mb-2"></v-divider>
                <div>
                  Full Name:
                  {{
                    permitStore.getPermitDetail.application.personalInfo
                      .lastName
                  }},
                  {{
                    permitStore.getPermitDetail.application.personalInfo
                      .firstName
                  }}
                </div>
              </v-list-item-title>
              <v-list-item-subtitle>
                No previous applications
                <v-list-item-avatar
                  tile
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  @click="handleFileImport('picture')"
                  style="margin-left: 170px !important"
                  size="40"
                >
                  <v-icon> mdi-camera-outline </v-icon>
                  <input
                    ref="picUploader"
                    label="Upload Personal Picture"
                    class="d-none"
                    type="file"
                    @change="onFileChanged($event, 'personalPicture')"
                    @click="onInputClick"
                    @keydown="onInputClick"
                    accept=".png, .jpeg, .jpg"
                  />
                </v-list-item-avatar>
                <v-list-item-avatar
                  tile
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  @click="handleFileImport('signature')"
                  style="cursor: pointer"
                  size="40"
                >
                  <input
                    ref="signatureUploader"
                    label="Upload Signature"
                    class="d-none"
                    type="file"
                    @change="onFileChanged($event, 'new_signature')"
                    @click="onInputClick"
                    @keydown="onInputClick"
                    accept=".png, .jpeg, .jpg"
                  />
                  <v-icon> mdi-account-edit-outline</v-icon>
                </v-list-item-avatar>
                <v-list-item-avatar
                  tile
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  style="cursor: pointer"
                  size="40"
                >
                  <v-menu bottom>
                    <template #activator="{ on, attrs }">
                      <v-btn
                        v-bind="attrs"
                        v-on="on"
                        icon
                      >
                        <v-icon>mdi-printer</v-icon>
                      </v-btn>
                    </template>
                    <v-list
                      align="left"
                      justify="left"
                    >
                      <v-list-item @click="printPdf('printApplicationApi')">
                        <v-list-item-title>Print Application</v-list-item-title>
                      </v-list-item>
                      <v-list-item @click="printPdf('printOfficialLicenseApi')">
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
                    </v-list>
                  </v-menu>
                </v-list-item-avatar>
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
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
          height="150"
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
              <v-list-item-title class="mt-8 font-weight-bold">
              </v-list-item-title>
              <v-list-item-subtitle>
                No Requests Sent
                <v-chip
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  class="ml-8"
                  :href="`mailto:${permitStore.getPermitDetail.application.userEmail}`"
                  target="_blank"
                  label
                >
                  <v-icon class="mr-1"> mdi-email-outline </v-icon>
                  Send Request
                </v-chip>
                <v-chip
                  :color="$vuetify.theme.dark ? '' : 'grey lighten-2'"
                  @click="generateReceipt()"
                  class="ml-8"
                  label
                >
                  <v-icon class="mr-1"> mdi-file-outline </v-icon>
                  Generate Receipt
                </v-chip>
                <vue-html2pdf
                  :show-layout="false"
                  :float-layout="true"
                  :enable-download="false"
                  :preview-modal="true"
                  :paginate-elements-by-height="1400"
                  filename="receipt"
                  :pdf-quality="2"
                  :manual-pagination="false"
                  pdf-format="a4"
                  :pdf-margin="[20, 20, 20, 20]"
                  pdf-orientation="portrait"
                  pdf-content-width="800px"
                  ref="html2Pdf"
                >
                  <section slot="pdf-content">
                    <Receipt />
                  </section>
                </vue-html2pdf>
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>
      <v-col
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
          height="150"
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
              <v-list-item-title class="mt-8 font-weight-bold">
              </v-list-item-title>
              <v-list-item-subtitle>
                <span class="mr-4 text-decoration-underline"
                  >No-show &nbsp;</span
                >
                <span class="mr-4 text-decoration-underline">Check-in</span>
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
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-card>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import DateTimePicker from '@core-admin/components/appointment/DateTimePicker.vue';
import Receipt from '@core-admin/components/receipt/Receipt.vue';
import Schedule from '@core-admin/components/appointment/Schedule.vue';
import VueHtml2pdf from 'vue-html2pdf';
import { formatDate } from '@shared-utils/formatters/defaultFormatters';
import { useDocumentsStore } from '@core-admin/stores/documentsStore';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useRoute } from 'vue-router/composables';
import { computed, reactive, ref } from 'vue';

const state = reactive({
  isSelecting: false,
  snack: false,
  snackColor: '',
  snackText: '',
  multiLine: false,
  snackbar: false,
  text: `Invalid file type provided.`,
});

const dialog = ref(false);
const datetime = ref(null);
const html2Pdf = ref(null);
const route = useRoute();
const permitStore = usePermitsStore();
const documentsStore = useDocumentsStore();

const picUploader = ref(null);
const signatureUploader = ref(null);

const allowedExtension = ['.png', '.jpeg', 'jpg'];

const { isLoading } = useQuery(['permitDetail', route.params.orderId], () =>
  permitStore.getPermitDetailApi(route.params.orderId)
);

function generateReceipt() {
  html2Pdf.value.generatePdf();
}

function handleFileImport(uploader) {
  state.isSelecting = true;
  window.addEventListener('focus', () => {
    state.isSelecting = false;
  });

  if (uploader === 'signature') {
    signatureUploader.value.click();
  } else {
    picUploader.value.click();
  }
}

function onInputClick(e) {
  e.target.value = '';
}

function onFileChanged(e, target) {
  if (
    allowedExtension.some(ext =>
      e.target.files[0].name.toLowerCase().endsWith(ext)
    )
  ) {
    documentsStore
      .setUserApplicationFile(e.target.files[0], target)
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
}

function printPdf(type) {
  permitStore[type]().then(res => {
    if (res.headers['content-type'] === 'application/pdf') {
      let file = new Blob([res.data], { type: 'application/pdf' });
      // eslint-disable-next-line node/no-unsupported-features/node-builtins
      let fileURL = URL.createObjectURL(file);

      window.open(fileURL);
    } else {
      let image = new Image();

      image.src = res.data;
      let w = window.open('');

      w.document.write(image.outerHTML);
    }
  });
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

const appointmentTime = computed(
  () =>
    new Date(
      permitStore.getPermitDetail?.application.appointmentDateTime
    )?.toLocaleTimeString('en-US', {
      timeStyle: 'short',
    }) || ''
);
</script>
