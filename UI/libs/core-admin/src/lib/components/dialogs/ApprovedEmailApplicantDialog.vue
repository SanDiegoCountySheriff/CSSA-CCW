<template>
  <div>
    <v-dialog
      width="600"
      v-model="state.dialog"
    >
      <v-card>
        <v-card-title>
          {{ $t('Send Email To Applicant') }}
        </v-card-title>

        <v-card-text>
          <v-row>
            <v-col>
              {{ $t('Do you want to email applicant: ') }}
              {{ props.applicantName }}
              {{
                ' to upload the 8-hour safety course and make final payment?'
              }}
            </v-col>
          </v-row>
          <v-row> </v-row>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="primary"
            @click="handleSubmit"
          >
            Open Email
          </v-btn>
          <v-btn
            color="error"
            @click="state.dialog = false"
          >
            Cancel
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'

interface ApprovedEmailApplicantDialog {
  applicantEmail: string
  applicantName: string
  showDialog: boolean
}

const props = defineProps<ApprovedEmailApplicantDialog>()

const state = reactive({
  dialog: props.showDialog,
  enteredName: '',
})

function handleSubmit() {
  const recipientEmail = props.applicantEmail
  const emailSubject = 'CCW Approved: More Actions Required '
  const emailBody =
    'Your CCW has been approved. You must now take the 8-hour safety course and upload the document to your application. Once we receive your 8-hour safety course completion document and your final payment, your license will be processed and sent out.'

  window.location.href = `mailto:${recipientEmail}?subject=${encodeURIComponent(
    emailSubject
  )}&body=${encodeURIComponent(emailBody)}`
}
</script>
