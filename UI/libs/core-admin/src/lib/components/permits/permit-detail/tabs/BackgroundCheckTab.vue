<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-list>
    <v-list-item-group
      v-model="settings"
      multiple
      active-class=""
      aria-label="Background Checklist"
    >
      <div
        v-for="(item, index) in checklistData"
        :key="index"
        class="text-left"
      >
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-checkbox
                color="blue"
                :name="item"
                :input-value="active"
              ></v-checkbox>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>{{ item }}</v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
      </div>
    </v-list-item-group>
  </v-list>
</template>
<script setup lang="ts">
import { ref } from 'vue';

const settings = ref([]);
const checklistData = ref([
  'Proof of ID',
  'Proof of Residency',
  'NCIC Wants/Warrants',
  'Locals',
  'Probations',
  'DMV Record',
  "AKS's Checked",
  'Coplink',
  'Traffic/Court Portal',
  'Property Assessor',
  'Voter Registration',
  'DOJ Approval Letter',
  'CII Number',
  'DOJ',
  'FBI',
  'SR14',
  'Firearms Reg',
  "All Dear Chief LTR's RCRD",
  'Safety Certificate',
  'Restrictions',
]);
</script>
