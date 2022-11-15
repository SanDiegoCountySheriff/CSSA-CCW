import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import Endpoints from '@shared-ui/api/endpoints';
import { PermitsType } from '@core-admin/types';
import axios from 'axios';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
import {
  defaultAllPermitsState,
  defaultPermitState,
} from '@shared-utils/lists/defaultConstants';
import {
  formatAddress,
  formatInitials,
  formatName,
} from '@shared-utils/formatters/defaultFormatters';

export const usePermitsStore = defineStore('PermitsStore', () => {
  const permits = ref<Array<PermitsType>>([defaultAllPermitsState]);
  const openPermits = ref<number>(0);
  const permitDetail = ref<CompleteApplication>(defaultPermitState);

  const getPermits = computed(() => permits.value);
  const getOpenPermits = computed(() => openPermits.value);
  const getPermitDetail = computed(() => permitDetail.value);

  function setPermits(payload: Array<PermitsType>) {
    permits.value = payload;
  }

  function setOpenPermits(payload: number) {
    openPermits.value = payload;
  }

  function setPermitDetail(payload: CompleteApplication) {
    permitDetail.value = payload;
  }

  async function getAllPermitsApi() {
    const res = await axios
      .get(Endpoints.GET_ALL_PERMITS_ENDPOINT)
      .catch(err => window.console.log(err));

    const permitsData: Array<PermitsType> = res?.data?.map(data => ({
      ...data,
      status: 'New',
      appointmentStatus: 'Scheduled',
      initials: formatInitials(data.firstName, data.lastName),
      name: formatName(data),
      address: formatAddress(data),
      rowClass: 'permits-table__row',
    }));

    setOpenPermits(
      /* permitsData?.filter(
        permit => permit.application.applicationType === 'New'
      ).length */
      10
    );
    setPermits(permitsData);

    return permitsData;
  }

  async function getPermitDetailApi(orderId: string) {
    const res = await axios.get(
      `${Endpoints.GET_PERMIT_ENDPOINT}?userEmailOrOrderId=${orderId}&isOrderId=true`
    );

    setPermitDetail(res?.data);

    return res?.data || {};
  }

  return {
    permits,
    openPermits,
    permitDetail,
    getPermits,
    getOpenPermits,
    getPermitDetail,
    setPermits,
    setOpenPermits,
    setPermitDetail,
    getAllPermitsApi,
    getPermitDetailApi,
  };
});
