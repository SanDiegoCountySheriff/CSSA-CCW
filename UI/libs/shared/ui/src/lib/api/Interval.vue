<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue'

const emit = defineEmits(['tick'])
const id = ref()

onMounted(() => {
  id.value = window.setInterval(() => emit('tick'), 5000)
})

onUnmounted(() => {
  window.clearInterval(id.value)
})
</script>
