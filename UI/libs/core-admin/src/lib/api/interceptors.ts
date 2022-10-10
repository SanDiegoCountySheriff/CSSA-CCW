import axios from 'axios';
import auth from '@shared-ui/api/auth/authentication';

// Setup Axios Request Interceptors
// to add the authentication token to each header
export default function interceptors() {
  axios.interceptors.request.use(
    async req => {
      if (req.url === '/config.json') {
        return req;
      } else {
        const token = await auth.acquireToken();
        req.headers.Authorization = `Bearer ${token}`;
        return req;
      }
    },
    function (error) {
      return Promise.reject(error);
    }
  );
}
