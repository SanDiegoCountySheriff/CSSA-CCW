import { computed } from 'vue';
import { useAppConfigStore } from '@shared-ui/stores/appConfig';
import { useBrandStore } from '@shared-ui/stores/brandStore';

export default function useEnvName() {
  const appStore = useAppConfigStore();
  const brandStore = useBrandStore();

  const getAppTitle = computed(() => {
    switch (appStore.getAppConfig.environmentName) {
      case 'DEV':
        return { name: brandStore.getBrand.agencyName || 'CCW', env: '(DEV)' };
      case 'QA':
        return { name: brandStore.getBrand.agencyName || 'CCW', env: '(QA)' };
      default:
        return { name: brandStore.getBrand.agencyName || 'CCW', env: '' };
    }
  });

  return getAppTitle;
}
