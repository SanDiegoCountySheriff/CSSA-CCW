import { useAppConfigStore } from '@shared-ui/stores/appConfig';

export default class Endpoints {
  /********CONFIG******************/

  static get GET_CONFIG_ENDPOINT() {
    return '/config.json';
  }

  /********SYSTEM SETTINGS******************/

  static get GET_SETTINGS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/admin/v1/systemsettings/get`;
  }

  static get PUT_SETTINGS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/admin/v1/systemsettings/update`;
  }

  /********PERMITS******************/

  static get GET_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/application/v1/permitapplication/get`;
  }

  static get PUT_UPDATE_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/application/v1/permitapplication/update`;
  }

  static get PUT_CREATE_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/application/v1/permitapplication/create`;
  }

  /********APPOINTMENTS******************/

  static get GET_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/schedule/v1/appointment/getAll`;
  }

  /********USER PROFILE******************/

  static get POST_VERIFY_USER_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/userprofile/v1/user/verifyEmail`;
  }

  static get PUT_CREATE_USER_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/userprofile/v1/user/create`;
  }

  /********DOCUMENTS******************/

  static get POST_DOCUMENT_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/document/v1/document/uploadImage`;
  }

  static get POST_DOCUMENT_IMAGE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.apiBaseUrl
    }/document/v1/document/uploadFile`;
  }
}
