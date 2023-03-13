import { AdminUserType } from '@shared-utils/types/defaultTypes';
import Endpoints from '@shared-ui/api/endpoints';
import axios from 'axios';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useAdminUserStore = defineStore('AdminUserStore', () => {
  const adminUser = ref<AdminUserType>({} as AdminUserType);
  const adminUserSignature = ref('');
  const validAdminUser = ref(true);

  const setAdminUser = (user: AdminUserType) => {
    adminUser.value = user;
  };

  const setAdminUserSignature = (file: string) => {
    adminUserSignature.value = file;
  };

  const setValidAdminUser = (value: boolean) => {
    validAdminUser.value = value;
  };

  async function getAdminUserApi() {
    const res = await axios.get(Endpoints.GET_ADMIN_USER_ENDPOINT);
    const imageResponse = await axios.get(
      `${Endpoints.GET_ADMIN_USER_FILE_ENDPOINT}?adminUserFileName=signature.png`
    );

    if (imageResponse?.data) setAdminUserSignature(imageResponse.data);

    if (res?.data) setAdminUser(res.data);

    if (!res?.data.badgeNumber && res?.data.uploadedDocuments.length === 0) {
      setValidAdminUser(false);
    }

    return res?.data || {};
  }

  async function putCreateAdminUserApi(user) {
    const res = await axios.put(Endpoints.PUT_CREATE_ADMIN_USER_ENDPOINT, user);

    if (res?.data) setAdminUser(res.data);

    return res?.data || {};
  }

  return {
    adminUser,
    adminUserSignature,
    validAdminUser,
    getAdminUserApi,
    putCreateAdminUserApi,
    setAdminUserSignature,
    setValidAdminUser,
  };
});
