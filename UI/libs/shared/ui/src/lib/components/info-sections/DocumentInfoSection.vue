<template>
  <div>
    <v-card-title> {{ $t('Documentation-Header') }}</v-card-title>
    <p class="ml-5">
      {{ $t('Documentation-Subheader') }}
    </p>
    <v-card-text>
      <div class="text-line">
        <h3 class="text-label">
          {{ $t('EVERYONE') }}
        </h3>
        <p class="text-info">
          {{ $t('Documentation-Everyone') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('MILITARY') }}
        </h3>
        <p class="text-info">
          {{ $t('Documentation-Military') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('NOT-BORN-IN-US') }}
        </h3>
        <p class="text-info">
          {{ $t('Documentation-NotBornInUs') }}
        </p>
      </div>
      <div class="mb-3">
        <ul>
          <li>{{ $t('Documentation-bullet1') }}</li>
          <li>{{ $t('Documentation-bullet2') }}</li>
          <li>{{ $t('Documentation-bullet3') }}</li>
          <li>{{ $t('Documentation-bullet4') }}</li>
        </ul>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('SUPPORTING') }}
        </h3>
        <p class="text-info">
          {{ $t('Documentation-Supporting') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('LEGAL-NAME-CHANGE') }}
        </h3>
        <p class="text-info">
          {{ $t('Documentation-LegalNameChange') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('JUDICIAL') }}
        </h3>
        <p class="text-info">
          {{ $t('Documentation-Judicial') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('RESERVE') }}
        </h3>
        <p class="text-info">
          {{ $t('Documentation-Reserve') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('MOD-INFO') }}
        </h3>
        <p class="text-info">
          {{ $t('MODIFICATION-DOCUMENTS') }}
        </p>
      </div>

      <div class="text-line">
        <h3 class="text-label">
          {{ $t('DUP-INFO') }}
        </h3>
        <p class="text-info">
          {{ $t('DUPLICATE-DOCUMENTS') }}
        </p>
      </div>
    </v-card-text>
  </div>
</template>

<script setup lang="ts"></script>

<style lang="scss" scoped>
.text {
  &-line {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
  }

  &-label {
    width: fit-content;
  }

  &-info {
    margin-left: 0.5rem;
  }
}
</style>
