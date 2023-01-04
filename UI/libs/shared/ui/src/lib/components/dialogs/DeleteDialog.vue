<template>
  <div>
    <v-dialog
      max-width="50%"
      class="outer-container"
      v-model="state.dialog"
    >
      <template #activator="{ on, attrs }">
        <v-btn
          text
          color="error"
          v-bind="attrs"
          v-on="on"
        >
          <v-icon color="error"> mdi-delete </v-icon>
        </v-btn>
      </template>
      <div
        :style="{
          background: $vuetify.theme.dark ? '#222' : '#EEE',
        }"
      >
        <v-card class="confirm-container">
          <v-card-title class="title">
            {{ $t('Confirm Deletion') }}
          </v-card-title>
          <v-card-text class="button-container">
            <v-btn
              color="info"
              @click="handleAccept"
            >
              {{ $t('ACCEPT') }}
            </v-btn>
            <v-btn
              color="error"
              @click="handleCancel"
            >
              {{ $t('CANCEL') }}
            </v-btn>
          </v-card-text>
        </v-card>
      </div>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';

interface IProps {
  deleteFunction: CallableFunction;
}

const props = defineProps<IProps>();

const state = reactive({
  dialog: false,
});

function handleAccept() {
  props.deleteFunction();
}

function handleCancel() {
  state.dialog = false;
}
</script>
<style lang="scss">
.confirm-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-content: center;
}
.button-container {
  display: flex;
  justify-content: space-around;
  align-content: space-around;
}
.outer-container {
  overflow-y: hidden !important;
}
.title {
  display: flex;
  justify-content: center;
  align-content: center;
}
</style>
