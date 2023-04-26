import { ThemeConfigType } from '@shared-utils/types/defaultTypes'
import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export const useThemeStore = defineStore(
  'ThemeStore',
  () => {
    const themeConfig = ref<ThemeConfigType>({
      isDark: false,
    })
    const getThemeConfig = computed(() => themeConfig.value)

    function setThemeConfig(payload: ThemeConfigType) {
      themeConfig.value = payload
    }

    return { themeConfig, getThemeConfig, setThemeConfig }
  },
  {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    //@ts-ignore
    persist: true,
  }
)
