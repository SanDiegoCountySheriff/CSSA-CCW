import { useMutation } from '@tanstack/vue-query'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { computed } from 'vue'

const brandStore = useBrandStore()

export function useTanstack() {
  const { mutate: setBrandSettings, isLoading } = useMutation({
    mutationFn: brandStore.setBrandSettingApi,
  })

  const loading = computed(() => {
    return isLoading
  })

  return {
    setBrandSettings,
    loading
  }
}
