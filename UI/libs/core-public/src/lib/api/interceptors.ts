import axios from 'axios';
import { useAuthStore } from '@shared-ui/stores/auth';

// Setup Axios Request Interceptors
// to add the authentication token to each header
export default function interceptors() {
  const authStore = useAuthStore();

  axios.interceptors.request.use(
    async req => {
      if (req.url === '/config.json' && !authStore.isAuthenticated) {
        return req;
      }

      req.headers.Authorization = `Bearer ${authStore.getAuthState.jwtToken}`;

      return req;
    }
    // error => {
    //   return Promise.reject(error);
    // }
  );
}
