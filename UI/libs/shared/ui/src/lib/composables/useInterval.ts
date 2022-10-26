import { onBeforeUnmount, onMounted, ref } from 'vue';

/**
 * useInterval composable which emits every x milliseconds
 * @param delay number in milliseconds
 */
export default function useInterval(fn, delay: number) {
  const interval = ref<TimerHandler>;

  onMounted(() => {
    interval.value = window.setInterval(fn, delay);
  });

  onBeforeUnmount(() => window.clearInterval(interval.value));
}
