<template>
  <v-card flat>
    <v-card-title> Upload Documents </v-card-title>
    <strong
      ><p class="ml-4">{{ $t('Documentation-Subheader') }}</p>
    </strong>
    <v-card-text>
      <div>
        <h4>
          {{ $t('EVERYONE') }}
        </h4>
        <p>
          {{ $t('Documentation-Everyone') }}
        </p>
      </div>
      <div>
        <h4>
          {{ $t('MILITARY') }}
        </h4>
        <p>
          {{ $t('Documentation-Military') }}
        </p>
      </div>
      <div>
        <h4>
          {{ $t('NOT-BORN-IN-US') }}
        </h4>
        <p>
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
      <div>
        <h4>
          {{ $t('SUPPORTING') }}
        </h4>
        <p>
          {{ $t('Documentation-Supporting') }}
        </p>
      </div>
      <div>
        <h4>
          {{ $t('LEGAL-NAME-CHANGE') }}
        </h4>
        <p>
          {{ $t('Documentation-LegalNameChange') }}
        </p>
      </div>
      <div>
        <h4>
          {{ $t('JUDICIAL') }}
        </h4>
        <p>
          {{ $t('Documentation-Judicial') }}
        </p>
      </div>

      <div>
        <h4>
          {{ $t('RESERVE') }}
        </h4>
        <p>
          {{ $t('Documentation-Reserve') }}
        </p>
      </div>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts"></script>
