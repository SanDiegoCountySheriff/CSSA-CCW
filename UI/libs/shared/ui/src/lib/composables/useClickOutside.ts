import { onMounted, onBeforeUnmount, Ref } from 'vue';

/**
 * On Click Outside composable which detects clicks outside of provided ref and runs the callback
 * @param ref Ref
 * @param cb Callback
 * @returns
 */
export default function useClickOutside(ref: Ref, cb: CallableFunction) {
  if (!ref) return;

  const listener = e => {
    if (e.target === ref.value || e.composedPath().includes(ref.value)) {
      return;
    }
    if (typeof cb === 'function') {
      cb();
    }
  };

  onMounted(() => {
    window.addEventListener('click', listener);
  });

  onBeforeUnmount(() => {
    window.removeEventListener('click', listener);
  });

  return {
    listener,
  };
}
