import { useAppConfigStore } from '@shared-ui/stores/configStore';

export default class Endpoints {
  /********CONFIG******************/

  static get GET_CONFIG_ENDPOINT() {
    return '/config.json';
  }

  /********SYSTEM SETTINGS******************/

  static get GET_SETTINGS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.adminApiBaseUrl
    }/admin/v1/systemsettings/get`;
  }

  static get PUT_SETTINGS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.adminApiBaseUrl
    }/admin/v1/systemsettings/update`;
  }

  /********PERMITS******************/

  static get GET_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getApplication`;
  }

  static get GET_AGENCY_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getUserApplication`;
  }

  static get GET_PERMIT_HISTORY_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getHistory`;
  }

  static get GET_PRINT_APPLICATION_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/printApplication`;
  }

  static get GET_PRINT_OFFICIAL_LICENSE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/printOfficialLicense`;
  }

  static get GET_PRINT_UNOFFICIAL_LICENSE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/printUnofficialLicense`;
  }

  static get GET_ALL_BY_USER_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getApplications`;
  }

  static get GET_ALL_PERMITS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getAll`;
  }

  static get PUT_UPDATE_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/updateApplication`;
  }

  static get PUT_UPDATE_AGENCY_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/updateUserApplication`;
  }

  static get GET_AGENCY_SEARCH_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/search`;
  }

  static get PUT_CREATE_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/create`;
  }

  static get DELETE_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/deleteApplication`;
  }

  /********APPOINTMENTS******************/

  static get GET_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getAll`;
  }

  static get GET_AVAILABLE_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getAvailability`;
  }

  static get PUT_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/update`;
  }

  static get POST_UPLOAD_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/uploadFile`;
  }

  static get GET_SINGLE_APPOINTMENT() {
    return `${
      useAppConfigStore().getAppConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/get`;
  }

  /********USER PROFILE******************/

  static get POST_VERIFY_USER_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.userProfileApiBaseUrl
    }/userprofile/v1/user/verifyEmail`;
  }

  static get PUT_CREATE_USER_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.userProfileApiBaseUrl
    }/userprofile/v1/user/create`;
  }

  /********DOCUMENTS******************/

  static get GET_DOCUMENT_AGENCY_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.documentApiBaseUrl
    }/document/v1/document/downloadAgencyLogo`;
  }

  static get POST_DOCUMENT_AGENCY_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.documentApiBaseUrl
    }/document/v1/document/uploadAgencyLogo`;
  }

  static get POST_DOCUMENT_IMAGE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.documentApiBaseUrl
    }/document/v1/document/uploadApplicantFile`;
  }

  static get GET_DOCUMENT_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.documentApiBaseUrl
    }/document/v1/document/downloadApplicantFile`;
  }

  static get GET_DOCUMENT_AGENCY_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.documentApiBaseUrl
    }/document/v1/document/downloadUserApplicantFile`;
  }

  static get POST_DOCUMENT_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.documentApiBaseUrl
    }/document/v1/document/uploadApplicantFile`;
  }
  static get POST_AGENCY_DOCUMENT_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().getAppConfig.documentApiBaseUrl
    }/document/v1/document/uploadUserApplicantFile`;
  }
}
