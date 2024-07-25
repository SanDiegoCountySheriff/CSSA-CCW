import { computed } from 'vue'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'

const adminUserStore = useAdminUserStore()

export const sortAdminUsers = computed(() => {
  if (adminUserStore.allAdminUsers) {
    return [...adminUserStore.allAdminUsers].sort((a, b) =>
      a.name.localeCompare(b.name)
    )
  }

  return adminUserStore.allAdminUsers
})
