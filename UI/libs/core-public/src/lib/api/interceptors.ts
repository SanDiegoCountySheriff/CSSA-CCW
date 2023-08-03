import { MsalBrowser } from '@shared-ui/api/auth/authentication'
import axios from 'axios'
import { useAuthStore } from '@shared-ui/stores/auth'

// Setup Axios Request Interceptors
// to add the authentication token to each header
export default async function interceptors(msalInstance: MsalBrowser) {
  const authStore = useAuthStore()

  axios.interceptors.request.use(async req => {
    if (req.url === '/config.json' && !authStore.getAuthState.isAuthenticated) {
      return req
    }

    const token = await msalInstance.acquireToken()

    if (req.headers) {
      req.headers.Authorization = `Bearer ${token}`
    }

    return req
  })

  axios.interceptors.response.use(
    async res => {
      return res
    },
    async error => {
      if (error.response.status === 401) {
        await msalInstance.acquireToken()
      }

      return Promise.reject(error)
    }
  )
}
