<template>
  <v-container
    fluid
    class="blue-grey lighten-3 elevation-3 rounded form-container"
  >
    <FormWrapper />
  </v-container>
</template>

<script lang="ts">
import FormWrapper from '../wrappers/FormWrapper';
export default {
  name: 'FormContainer',
  components: { FormWrapper },
};
</script>

<style lang="scss" scoped>
.form-container {
  min-height: 50%;
}
</style>
