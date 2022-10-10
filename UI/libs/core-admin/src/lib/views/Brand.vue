<template>
  <BrandForm></BrandForm>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import BrandForm from '../components/brand/BrandForm.vue';

export default defineComponent({
  name: 'Brand',
  components: { BrandForm },
});
</script>
