import { useAppConfigStore } from '@shared-ui/stores/configStore'

export default class Endpoints {
  /********CONFIG******************/

  static get GET_CONFIG_ENDPOINT() {
    return '/config.json'
  }

  /********SYSTEM SETTINGS******************/

  static get GET_SETTINGS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.adminApiBaseUrl
    }/admin/v1/systemsettings/get`
  }

  static get PUT_SETTINGS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.adminApiBaseUrl
    }/admin/v1/systemsettings/update`
  }

  /********PERMITS******************/

  static get GET_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getApplication`
  }

  static get GET_AGENCY_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getUserApplication`
  }

  static get GET_PERMIT_HISTORY_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getHistory`
  }

  static get GET_AGREEMENT_PDF_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getAgreementPdf`
  }

  static get GET_PRINT_REVOCATION_LETTER_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/printRevocationLetter`
  }

  static get GET_PRINT_APPLICATION_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/printApplication`
  }

  static get GET_PRINT_LIVE_SCAN_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/printLiveScan`
  }

  static get GET_PRINT_OFFICIAL_LICENSE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/printOfficialLicense`
  }

  static get GET_PRINT_UNOFFICIAL_LICENSE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/printUnofficialLicense`
  }

  static get GET_ALL_BY_USER_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getApplications`
  }

  static get GET_ALL_PERMITS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getAll`
  }

  static get GET_ALL_PERMITS_SUMMARY_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getAllPermitsSummary`
  }

  static get PUT_UPDATE_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/updateApplication`
  }

  static get PUT_UPDATE_MULTIPLE_PERMITS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/updateAssignedMultipleUsersApplications`
  }

  static get PUT_UPDATE_AGENCY_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/updateUserApplication`
  }

  static get GET_AGENCY_SEARCH_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/search`
  }

  static get PUT_CREATE_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/create`
  }

  static get DELETE_PERMIT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/deleteApplication`
  }

  static get GET_PERMIT_SSN_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getUserSSN`
  }

  static get GET_APPLICATION_SUMMARY_COUNT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getApplicationSummaryCount`
  }

  static get GET_ASSIGNED_APPLICATIONS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/getAssignedApplicationsSummary`
  }

  static get PUT_ADD_HISTORICAL_APPLICATION_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.applicationApiBaseUrl
    }/application/v1/permitapplication/putAddHistoricalApplication`
  }

  /********APPOINTMENTS******************/

  static get GET_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getAll`
  }

  static get GET_AVAILABLE_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getAvailability`
  }

  static get PUT_RESCHEDULE_APPOINTMENT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/rescheduleAppointment`
  }

  static get PUT_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/updateUserAppointment`
  }

  static get PUT_PUBLIC_APPOINTMENT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/update`
  }

  static get POST_UPLOAD_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/uploadFile`
  }

  static get PUT_CREATE_MANUAL_APPOINTMENT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/create`
  }

  static get CREATE_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/createNewAppointments`
  }

  static get GET_SINGLE_APPOINTMENT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/get`
  }

  static get PUT_USER_APPOINTMENT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/create`
  }

  static get DELETE_APPOINTMENT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/delete`
  }

  static get DELETE_APPOINTMENTS_BY_DATE() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/deleteAppointmentsByDate`
  }

  static get DELETE_APPOINTMENTS_BY_TIME_SLOT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/deleteAppointmentsByTimeSlot`
  }

  static get DELETE_USER_FROM_APPOINTMENT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/reopenSlot`
  }

  static get DELETE_APPOINTMENT_BY_APPLICATION_ID() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/deleteSlotByApplicationId`
  }

  static get REOPEN_APPOINTMENT_BY_APPLICATION_ID() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/reopenSlotByApplicationId`
  }

  static get REMOVE_APPLICATION_FROM_APPOINTMENT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/removeApplicationFromAppointment`
  }

  static get PUT_CHECK_IN_APPOINTMENT_BY_APPOINTMENT_ID() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/checkInAppointment`
  }

  static get PUT_NO_SHOW_APPOINTMENT_BY_APPOINTMENT_ID() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/noShowAppointment`
  }

  static get PUT_SET_APPOINTMENT_SCHEDULED_BY_APPOINTMENT_ID() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/setAppointmentScheduled`
  }

  static get GET_NUMBER_OF_NEW_APPOINTMENTS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getNumberOfNewAppointments`
  }

  static get GET_NEXT_AVAILABLE_APPOINTMENT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getNextAvailableAppointment`
  }

  static get GET_HOLIDAYS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getHolidays`
  }

  static get SAVE_HOLIDAYS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/postOrganizationHolidays`
  }

  static get GET_APPOINTMENT_MANAGEMENT_TEMPLATE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getAppointmentManagementTemplate`
  }

  static get GET_ORGANIZATION_HOLIDAYS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.scheduleApiBaseUrl
    }/schedule/v1/appointment/getAgencyHolidays`
  }

  /********USER PROFILE******************/

  static get POST_VERIFY_USER_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.userProfileApiBaseUrl
    }/userprofile/v1/user/verifyEmail`
  }

  static get PUT_CREATE_USER_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.userProfileApiBaseUrl
    }/userprofile/v1/userprofile/createUserProfile`
  }

  static get GET_USER_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.userProfileApiBaseUrl
    }/userprofile/v1/userprofile/getUserProfile`
  }

  static get GET_ADMIN_USER_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.userProfileApiBaseUrl
    }/userprofile/v1/adminuser/getAdminUser`
  }

  static get PUT_CREATE_ADMIN_USER_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.userProfileApiBaseUrl
    }/userprofile/v1/adminuser/createAdminUser`
  }

  static get GET_ALL_ADMIN_USERS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.userProfileApiBaseUrl
    }/userprofile/v1/adminuser/getAllAdminUsers`
  }

  /********DOCUMENTS******************/

  static get GET_DOCUMENT_AGENCY_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/downloadAgencyLogo`
  }

  static get GET_PDF_VALIDATION_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/getPdfFormValidation`
  }

  static get GET_DOCUMENT_AGENCY_LANDING_PAGE_IMAGE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/downloadAgencyLandingPageImage`
  }

  static get GET_DOCUMENT_AGENCY_HOME_PAGE_IMAGE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/downloadAgencyHomePageImage`
  }

  static get GET_DOCUMENT_AGENCY_SIGNATURE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/downloadAgencySignature`
  }

  static get POST_DOCUMENT_AGENCY_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/uploadAgencyLogo`
  }

  static get POST_DOCUMENT_AGENCY_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/uploadAgencyFile`
  }

  static get POST_DOCUMENT_IMAGE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/uploadApplicantFile`
  }

  static get GET_DOCUMENT_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/downloadApplicantFile`
  }

  static get GET_ADMIN_USER_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/downloadAdminUserFile`
  }

  static get GET_ADMIN_APPLICATION_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/downloadAdminApplicationFile`
  }

  static get GET_DOCUMENT_AGENCY_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/downloadUserApplicantFile`
  }

  static get POST_AGENCY_DOCUMENT_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/uploadUserApplicantFile`
  }

  static get POST_UPLOAD_ADMIN_USER_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/uploadAdminUserFile`
  }

  static get DELETE_ADMIN_APPLICATION_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/deleteAdminApplicationFile`
  }

  static get DELETE_DOCUMENT_FILE_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/deleteApplicantFile`
  }

  static get DELETE_DOCUMENT_FILE_PUBLIC_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/deleteApplicantFilePublic`
  }

  static get POST_ADMIN_APPLICATION_FILE_RENAME_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/updateAdminApplicationFileName`
  }

  static get POST_APPLICATION_FILE_RENAME_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/updateApplicationFileName`
  }

  static get GET_USER_PORTRAIT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.documentApiBaseUrl
    }/document/v1/document/getUserPortrait`
  }

  /********PAYMENTS******************/

  static get GET_PAYMENT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.paymentApiBaseUrl
    }/payment/v1/payment/makePayment`
  }

  static get REFUND_PAYMENT_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.paymentApiBaseUrl
    }/payment/v1/payment/refundPayment`
  }

  static get UPDATE_PAYMENT_HISTORY_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.paymentApiBaseUrl
    }/payment/v1/payment/updatePaymentHistory`
  }

  static get REQUEST_REFUND_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.paymentApiBaseUrl
    }/payment/v1/payment/requestRefund`
  }

  static get GET_ALL_REFUND_REQUESTS_ENDPOINT() {
    return `${
      useAppConfigStore().appConfig.paymentApiBaseUrl
    }/payment/v1/payment/getRefundRequests`
  }
}
