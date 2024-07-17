<template>
  <div>
    <v-app-bar
      app
      color="primary"
      class="white--text"
      clipped-right
    >
      <template v-if="!permitsStore.viewingPermitDetail">
        <v-btn
          icon
          v-if="!props.drawerShowing"
        >
          <v-icon
            color="white"
            @click="handleExpandMenu"
          >
            mdi-menu
          </v-icon>
        </v-btn>

        <v-btn
          v-if="authStore.getAuthState.isAuthenticated"
          @click="handleEditAdminUser(false)"
          color="white"
          large
          text
        >
          {{ authStore.getAuthState.userName }}
          <v-icon
            right
            dark
          >
            mdi-cog
          </v-icon>
        </v-btn>
      </template>

      <template v-if="permitsStore.viewingPermitDetail">
        <PaymentDialog :loading="isHistoricalApplicationLoading" />

        <FileUploadDialog
          :loading="isFetching || isHistoricalApplicationLoading"
          :eight-hour-safety-input="false"
          @get-file-from-dialog="onFileChanged"
          title="Upload"
        />

        <v-btn
          :loading="isHistoricalApplicationLoading"
          :href="`mailto:${permitsStore.getPermitDetail.application.userEmail}`"
          color="white"
          target="_blank"
          text
        >
          <v-icon left>mdi-email-outline</v-icon>
          Send Email
        </v-btn>

        <v-menu
          v-if="!permitsStore.viewingHistorical"
          offset-y
          @input="getHistoricalApplicationSummary"
        >
          <template #activator="{ on, attrs }">
            <v-btn
              :loading="isGetHistoricalApplicationSummaryLoading"
              v-bind="attrs"
              v-on="on"
              color="white"
              text
            >
              <v-icon left>mdi-history</v-icon>
              {{ historicalButtonText }}
            </v-btn>
          </template>

          <v-list>
            <v-list-item
              v-for="(item, index) in historicalApplications"
              :key="index"
            >
              <v-btn
                text
                @click="handleViewHistoricalApplication(item.id)"
              >
                {{
                  item.historicalDate
                    ? new Date(item.historicalDate).toLocaleDateString()
                    : 'Import'
                }}
                -
                {{ ApplicationType[item.applicationType] }}
              </v-btn>
            </v-list-item>
          </v-list>
        </v-menu>

        <v-btn
          v-else
          :loading="isHistoricalApplicationLoading"
          @click="handleBack"
          color="white"
          text
        >
          <v-icon left>mdi-arrow-left</v-icon>
          Back
        </v-btn>
      </template>

      <v-dialog
        v-model="showAdminUserDialog"
        :persistent="persistentDialog"
        max-width="600px"
      >
        <v-card
          :loading="
            isCreateAdminUserLoading || isUploadAdminUserDocumentLoading
          "
        >
          <v-card-title>
            {{ $t('Setup User Information') }}
          </v-card-title>

          <v-card-text>
            <v-row>
              <v-col>
                <v-form v-model="valid">
                  <v-text-field
                    v-model="adminUser.badgeNumber"
                    :rules="[v => !!v || 'Badge Number is required']"
                    label="Badge Number"
                    outlined
                  ></v-text-field>
                </v-form>
              </v-col>

              <v-col>
                <v-form v-model="valid">
                  <v-text-field
                    v-model="adminUser.jobTitle"
                    :rules="[v => !!v || 'Job Title is required']"
                    label="Job Title"
                    outlined
                  ></v-text-field>
                </v-form>
              </v-col>
            </v-row>
          </v-card-text>

          <v-card-title>Signature</v-card-title>

          <v-card-text>
            <v-card
              light
              flat
              width="555px"
              height="105px"
              outlined
              style="border: solid 2px black"
            >
              <canvas
                width="550px"
                height="100px"
                id="signature"
                class="signature"
              ></canvas>
            </v-card>
          </v-card-text>

          <v-card-actions>
            <v-btn
              color="primary"
              @click="handleClearSignature"
            >
              {{ $t('Clear Signature') }}
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
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

      <v-btn
        v-if="authStore.getAuthState.isAuthenticated"
        aria-label="Sign out of application"
        @click="signOut"
        class="mr-4 ml-1"
        color="primary"
        text
        small
      >
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

    <v-snackbar v-model="snackbar">
      {{ text }}

      <template #action="{ attrs }">
        <v-btn
          @click="snackbar = false"
          v-bind="attrs"
          color="primary"
          text
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import FileUploadDialog from '@core-admin/components/dialogs/FileUploadDialog.vue'
import { MsalBrowser } from '@shared-ui/api/auth/authentication'
import PaymentDialog from '@core-admin/components/dialogs/PaymentDialog.vue'
import SignaturePad from 'signature_pad'
import ThemeMode from '@shared-ui/components/mode/ThemeMode.vue'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationType,
  HistoricalApplicationSummary,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'
import { computed, inject, nextTick, onMounted, ref, watch } from 'vue'
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query'

const authStore = useAuthStore()
const documentsStore = useDocumentsStore()
const adminUserStore = useAdminUserStore()
const permitsStore = usePermitsStore()
const adminUser = computed(() => adminUserStore.adminUser)
const valid = ref(false)
const signaturePad = ref<SignaturePad>()
const persistentDialog = ref(true)
const validAdminUser = ref(adminUserStore.validAdminUser)
const showAdminUserDialog = ref(false)
const form = new FormData()
const msalInstance = ref(inject('msalInstance') as MsalBrowser)
const text = ref('')
const snackbar = ref(false)
const historicalApplications = ref<Array<HistoricalApplicationSummary>>()

interface IHeaderProps {
  drawerShowing: boolean
}

const props = withDefaults(defineProps<IHeaderProps>(), {
  drawerShowing: true,
})

const allowedExtension = [
  '.png',
  '.jpeg',
  '.jpg',
  '.pjp',
  '.pjpeg',
  '.jfif',
  '.bmp',
  '.pdf',
]

const queryClient = useQueryClient()

const { isLoading: isCreateAdminUserLoading, mutate: createAdminUser } =
  useMutation(
    ['createAdminUser'],
    async () => await adminUserStore.putCreateAdminUserApi(adminUser.value),
    {
      onSuccess: async () => {
        showAdminUserDialog.value = false
        await adminUserStore.getAdminUserApi()
      },
    }
  )

const {
  isLoading: isHistoricalApplicationLoading,
  mutate: getHistoricalApplicationDetail,
} = useMutation(['historicalPermitDetail'], (id: string) => {
  permitsStore.viewingHistorical = true

  return permitsStore.getHistoricalPermitDetailApi(id)
})

function handleViewHistoricalApplication(id: string) {
  getHistoricalApplicationDetail(id)
}

async function handleBack() {
  await queryClient.invalidateQueries(['permitDetail'])
  permitsStore.viewingHistorical = false
}

const {
  isLoading: isGetHistoricalApplicationSummaryLoading,
  mutate: getHistoricalApplications,
} = useMutation(['historicalApplicationSummary'], (orderId: string) => {
  return permitsStore.getHistoricalApplicationSummary(orderId).then(res => {
    historicalApplications.value = res
  })
})

const {
  isLoading: isUploadAdminUserDocumentLoading,
  mutate: uploadAdminUserDocument,
} = useMutation(
  ['uploadAdminUserDocument'],
  async () => await documentsStore.postUploadAdminUserFile(form, 'signature'),
  {
    onSuccess: async () => {
      const uploadDoc: UploadedDocType = {
        documentType: 'adminUserSignature',
        name: 'signature.png',
        uploadedBy: authStore.getAuthState.userName,
        uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
      }

      if (adminUser.value.uploadedDocuments) {
        adminUser.value.uploadedDocuments =
          adminUser.value.uploadedDocuments.filter(document => {
            return document.documentType !== 'adminUserSignature'
          })
      } else {
        adminUser.value.uploadedDocuments = []
      }

      adminUser.value.name = authStore.getAuthState.userName
      adminUser.value.uploadedDocuments.push(uploadDoc)

      createAdminUser()
    },
  }
)

onMounted(async () => {
  if (!validAdminUser.value) {
    handleEditAdminUser(true)
  }
})

const emit = defineEmits(['on-expand-menu'])

const historicalButtonText = computed(() => {
  return `${permitsStore.historicalApplicationCount} Audit${
    permitsStore.historicalApplicationCount === 1 ? '' : 's'
  }`
})

async function signOut() {
  msalInstance.value.logOut()
}

function getHistoricalApplicationSummary(event) {
  if (event) {
    getHistoricalApplications(permitsStore.permitDetail.application.orderId)
  }
}

function handleEditAdminUser(persist: boolean) {
  persistentDialog.value = persist
  showAdminUserDialog.value = true
  nextTick(() => {
    const canvas = document.getElementById('signature') as HTMLCanvasElement

    signaturePad.value = new SignaturePad(canvas, {
      backgroundColor: 'rgb(0, 0, 0, 0)',
    })

    if (adminUserStore.adminUserSignature) {
      const signature = adminUserStore.adminUserSignature
      const image = new Image()

      image.src = signature
      image.onload = () => {
        signaturePad.value?.fromDataURL(signature, {
          ratio: 1,
          width: image.width,
          height: image.height,
        })
      }
    }
  })
}

function handleClearSignature() {
  signaturePad.value?.clear()
}

async function handleSaveAdminUser() {
  const canvas = document.getElementById('signature') as HTMLCanvasElement

  canvas.toBlob(async blob => {
    form.append('fileToUpload', blob as Blob)

    uploadAdminUserDocument()
  })
}

function handleExpandMenu() {
  emit('on-expand-menu')
}

const { isFetching, refetch } = useQuery(
  ['permitDetailHeader'],
  () =>
    permitsStore.getPermitDetailApi(
      permitsStore.permitDetail.application.orderId
    ),
  {
    enabled: false,
  }
)

function onFileChanged({ file, target }: { file: File; target: string }) {
  if (allowedExtension.some(ext => file.name.toLowerCase().endsWith(ext))) {
    documentsStore
      .setUserApplicationFile(file, target)
      .then(() => {
        text.value = 'Successfully uploaded file.'
        snackbar.value = true
        refetch()
      })
      .catch(() => {
        text.value = 'There was a problem uploading the file.'
        snackbar.value = true
      })
  } else {
    text.value = 'Invalid file type provided.'
    snackbar.value = true
  }
}

watch(
  () => validAdminUser.value,
  newVal => {
    if (!newVal) {
      handleEditAdminUser(true)
    }
  }
)
</script>

<style lang="scss" scoped>
.signature {
  border: black;
  border-radius: 5px;
}
</style>
