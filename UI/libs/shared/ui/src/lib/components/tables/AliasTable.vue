<template>
  <v-simple-table class="my-3">
    <template #default>
      <thead>
        <tr>
          <th>Previous Last name</th>
          <th>Previous First name</th>
          <th>Previous Middle name</th>
          <th>City where changed</th>
          <th>State or region where changed</th>
          <th>Court file number</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(alias, index) in aliases" :key="index">
          <td>
            {{ alias.prevLastName }}
          </td>
          <td>
            {{ alias.prevFirstName }}
          </td>
          <td>
            {{ alias.prevMiddleName }}
          </td>
          <td>
            {{ alias.cityWhereChanged }}
          </td>
          <td>
            {{ alias.stateWhereChanged }}
          </td>
          <td>
            {{ alias.courtFileNumber }}
          </td>
        </tr>
      </tbody>
    </template>
  </v-simple-table>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'AliasTable',
  props: {
    aliases: Array,
  },
});
</script>
