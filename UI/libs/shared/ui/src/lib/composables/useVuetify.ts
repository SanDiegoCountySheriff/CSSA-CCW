import { getCurrentInstance } from 'vue'

export const useVuetify = () => {
  const vm = getCurrentInstance()

  if (vm) {
    return vm.proxy?.$vuetify || undefined
  }
}
