<template>
  <div class="finalize-view">
    <v-container
      fluid
      v-if="isLoading && !isError"
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item, divider, list-item-three-line,
       acations"
      >
      </v-skeleton-loader>
    </v-container>
    <div>
      <SideBar
        :options="options"
        :title="'Information Sections'"
        :handle-selection="handleSelection"
      />
      <FinalizeContainer />
      <PaymentContainer :toggle-payment="togglePaymentComplete" />
      <AppointmentContainer
        :events="state.appointments"
        :toggle-appointment="toggleAppointmentComplete"
        :reschedule="false"
      />
      <v-container class="finalize-submit">
        <v-btn
          :disabled="!state.appointmentComplete || !state.paymentComplete"
          :color="$vuetify.theme.dark ? 'accent' : 'primary'"
          @click="handleSubmit"
        >
          {{ $t('Submit Application') }}
        </v-btn>
        <v-btn
          color="error"
          to="/"
        >
          {{ $t('Cancel') }}
        </v-btn>
      </v-container>
    </div>
    <v-snackbar
      :value="state.snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </div>
</template>

<script lang="ts" setup>
import AppointmentContainer from '@core-public/components/containers/AppointmentContainer.vue';
import { AppointmentType } from '@shared-utils/types/defaultTypes';
import FinalizeContainer from '@core-public/components/containers/FinalizeContainer.vue';
import PaymentContainer from '@core-public/components/containers/PaymentContainer.vue';
import Routes from '@core-public/router/routes';
import SideBar from '@core-public/components/navbar/SideBar.vue';
import { reactive } from 'vue';
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useCurrentInfoSection } from '@core-public/stores/currentInfoSection';
import { useRouter } from 'vue-router/composables';
import { useMutation, useQuery } from '@tanstack/vue-query';

const currentInfoSectionStore = useCurrentInfoSection();

const options = [
  'Personal Information',
  'Spouse Information',
  'Alias Information',
  'Id Information',
  'Birth Information',
  'Citizenship Information',
  'Current Address Information',
  'Previous Address Information',
  'Mailing Address Information',
  'Physical Appearance',
  'Contact Information',
  'Employment Information',
  'Weapons Information',
];
const state = reactive({
  snackbar: false,
  paymentComplete: false,
  appointmentComplete: false,
  appointments: [] as Array<AppointmentType>,
  applicationLoaded: false,
});
const completeApplicationStore = useCompleteApplicationStore();
const appointmentsStore = useAppointmentsStore();
const router = useRouter();

const { isLoading, isError } = useQuery(['getIncompleteApplications'], () => {
  const appRes = appointmentsStore.getAvailableAppointments();

  appRes.then((data: Array<AppointmentType>) => {
    data.forEach(event => {
      let start = new Date(event.start);
      let end = new Date(event.end);

      let formatedStart = `${start.getFullYear()}-${
        start.getMonth() + 1
      }-${start.getDate()} ${start.getHours()}:${start.getMinutes()}`;

      let formatedEnd = `${end.getFullYear()}-${
        end.getMonth() + 1
      }-${end.getDate()} ${end.getHours()}:${end.getMinutes()}`;

      event.start = formatedStart;
      event.end = formatedEnd;
    });
    state.appointments = data;
  });
});

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Application Complete');
  },
  onSuccess: () => {
    router.push(Routes.RECEIPT_PATH);
  },
  onError: () => {
    state.snackbar = true;
  },
});

function handleSelection(target: number) {
  currentInfoSectionStore.setCurrentInfoSection(target);
}

async function handleSubmit() {
  completeApplicationStore.completeApplication.application.isComplete = true;
  completeApplicationStore.completeApplication.application.status = 2;
  updateMutation.mutate();
}

function togglePaymentComplete() {
  state.paymentComplete = !state.paymentComplete;
}

function toggleAppointmentComplete() {
  state.appointmentComplete = !state.appointmentComplete;
}
</script>

<style lang="scss" scoped>
.finalize {
  &-view {
    height: 100%;
    width: 100%;
  }
  &-submit {
    display: flex;
    justify-content: space-around;
    height: 20em;
    width: 80%;
  }
}
</style>
