import { computed } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useMutation } from '@tanstack/vue-query'

const brandStore = useBrandStore()

export function useTanstack() {
  const { mutate: setBrandSettings, isLoading: isSetBrandSettingsLoading } =
    useMutation({
      mutationFn: brandStore.setBrandSettingApi,
    })

  const { mutate: setAgencyLogo, isLoading: isSetAgencyLogoLoading } =
    useMutation({
      mutationFn: (logo: FormData) =>
        brandStore.setAgencyDocument(logo, 'agency_logo'),
    })

  const { mutate: setAgencySignature, isLoading: isSetAgencySignatureLoading } =
    useMutation({
      mutationFn: (logo: FormData) =>
        brandStore.setAgencyDocument(logo, 'agency_sheriff_signature_image'),
    })

  const loading = computed(() => {
    return (
      isSetBrandSettingsLoading ||
      isSetAgencyLogoLoading ||
      isSetAgencySignatureLoading
    )
  })

  return {
    setBrandSettings,
    setAgencyLogo,
    setAgencySignature,
    loading,
  }
}
