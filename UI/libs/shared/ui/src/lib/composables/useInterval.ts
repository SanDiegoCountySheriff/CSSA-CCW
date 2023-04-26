import { onBeforeUnmount, onMounted } from 'vue'

/**
 * useInterval composable which emits every x milliseconds
 * @param delay number in milliseconds
 */
export default function useInterval(fn, delay: number) {
  let interval

  onMounted(() => {
    interval = window.setInterval(fn, delay * 1000)
  })

  onBeforeUnmount(() => window.clearInterval(interval))
}
