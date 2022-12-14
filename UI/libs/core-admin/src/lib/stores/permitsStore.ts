import Endpoints from '@shared-ui/api/endpoints';
import { PermitsType } from '@core-admin/types';
import axios from 'axios';
import { defineStore } from 'pinia';
import {
  CompleteApplication,
  HistoryType,
} from '@shared-utils/types/defaultTypes';
import { computed, ref } from 'vue';
import {
  defaultAllPermitsState,
  defaultPermitState,
} from '@shared-utils/lists/defaultConstants';
import {
  formatAddress,
  formatDate,
  formatInitials,
  formatName,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters';

export const usePermitsStore = defineStore('PermitsStore', () => {
  const permits = ref<Array<PermitsType>>([defaultAllPermitsState]);
  const openPermits = ref<number>(0);
  const permitDetail = ref<CompleteApplication>(defaultPermitState);
  const history = ref(defaultPermitState.history);
  const searchResults = ref([]);

  const getPermits = computed(() => permits.value);
  const getSearchResults = computed(() => searchResults.value);
  const getOpenPermits = computed(() => openPermits.value);
  const getPermitDetail = computed(() => permitDetail.value);
  const getHistory = computed(() => history.value);

  function setPermits(payload: Array<PermitsType>) {
    permits.value = payload;
  }

  function setOpenPermits(payload: number) {
    openPermits.value = payload;
  }

  function setPermitDetail(payload: CompleteApplication) {
    permitDetail.value = payload;
  }

  function setSearchResults(payload) {
    searchResults.value = payload;
  }

  function setHistory(payload: Array<HistoryType>) {
    history.value = payload;
  }

  async function getAllPermitsApi() {
    const res = await axios
      .get(Endpoints.GET_ALL_PERMITS_ENDPOINT)
      .catch(err => window.console.log(err));

    const permitsData: Array<PermitsType> = res?.data
      ?.map(data => ({
        ...data,
        status: 'New',
        appointmentStatus: 'Scheduled',
        initials: formatInitials(data.firstName, data.lastName),
        name: formatName(data),
        address: formatAddress(data),
        rowClass: 'permits-table__row',
        appointmentDateTime: `${formatTime(
          data.appointmentDateTime
        )} on ${formatDate(data.appointmentDateTime)}`,
      }))
      .reverse();

    setOpenPermits(permitsData.length);
    setPermits(permitsData);

    return permitsData;
  }

  async function getPermitDetailApi(orderId: string) {
    const isComplete =
      permits.value.filter(item => item.orderID === orderId)[0].isComplete ||
      false;

    const res = await axios.get(
      `${Endpoints.GET_AGENCY_PERMIT_ENDPOINT}?userEmailOrOrderId=${orderId}&isOrderId=true&isComplete=${isComplete}`
    );

    setPermitDetail(res?.data);

    return res?.data || {};
  }

  async function getHistoryApi() {
    const orderId = permitDetail.value.application.orderId;

    const res = await axios.get(
      `${Endpoints.GET_PERMIT_HISTORY_ENDPOINT}?applicationIdOrOrderId=${orderId}&isOrderId=true`
    );

    setHistory(res?.data);

    return res?.data || {};
  }

  async function updatePermitDetailApi() {
    const res = await axios.put(
      Endpoints.PUT_UPDATE_AGENCY_PERMIT_ENDPOINT,
      permitDetail.value
    );

    if (res?.data) setPermitDetail(res.data);

    return res?.data || {};
  }

  async function getPermitSearchApi(query) {
    const res = await axios.get(
      `${Endpoints.GET_AGENCY_SEARCH_ENDPOINT}?searchValue=${query}`
    );

    if (res?.data) setSearchResults(res.data);

    return res?.data || {};
  }

  return {
    permits,
    searchResults,
    openPermits,
    permitDetail,
    getPermits,
    getOpenPermits,
    getPermitDetail,
    getSearchResults,
    getHistory,
    setPermits,
    setOpenPermits,
    setSearchResults,
    setPermitDetail,
    getAllPermitsApi,
    getPermitDetailApi,
    getPermitSearchApi,
    getHistoryApi,
    updatePermitDetailApi,
  };
});
