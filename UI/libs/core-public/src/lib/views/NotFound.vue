<template>
  <div class="not-found">
    <v-container
      bg
      fill-height
      fluid
    >
      <v-layout
        row
        wrap
        align-center
      >
        <v-flex xs12>
          <v-card
            height="400"
            color="transparent"
            flat
          >
            <div class="display-3 mt-5">
              {{ $t('Page not found') }}
            </div>
            <div class="grey--text lighten-5">
              {{
                $t(
                  'The page you are trying to get to never existed in this reality\, or has migrated to a parallel universe.'
                )
              }}
            </div>
            <div class="paragraph-text mt-2">
              <br />
              <br />{{ $t('Try going back to home page') }}
              <v-container>
                <router-link :to="{ path: '/' }">
                  <Button
                    color="primary"
                    text="Home"
                  >
                  </Button>
                </router-link>
              </v-container>
            </div>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import Button from '@shared-ui/components/Button.vue'
</script>

<style lang="scss">
.paragraph-text {
  font-size: 18px;
}

.not-found {
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
  min-height: 100vh;
}
</style>
