<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t('Character References:') }}
    </v-banner>
    <v-row
      v-for="(reference, refIndex) in $props.characterReferences"
      :key="refIndex"
    >
      <v-col
        cols="12"
        md="3"
        v-for="(field, fieldIndex) in [
          { key: 'name', label: 'Name' },
          { key: 'relationship', label: 'Relationship' },
          { key: 'phoneNumber', label: 'Phone Number' },
          { key: 'email', label: 'Email Address' },
        ]"
        :key="fieldIndex"
      >
        <v-banner
          rounded
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="primary"
          >
            mdi-numeric-{{ refIndex + 1 }}-circle
          </v-icon>
          <strong>{{ field.label + ': ' }}</strong> {{ reference[field.key] }}
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { CharacterReferenceType } from '@shared-utils/types/defaultTypes'

interface ICharacterReferenceInfoSectionProps {
  characterReferences: CharacterReferenceType[]
  color: string
}

const props = defineProps<ICharacterReferenceInfoSectionProps>()
</script>
