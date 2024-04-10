<template>
  <div>
    <v-container fluid>
      <v-row
        align="center"
        justify="center"
      >
        <v-col
          cols="12"
          md="10"
          lg="8"
        >
          <v-card
            class="pa-5"
            elevation="2"
          >
            <v-form>
              <v-row>
                <v-col>
                  <div class="display-1 text-center mb-4">Existing License</div>
                </v-col>
              </v-row>

              <v-row>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    label="First Name"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
                <v-col
                  cols="12"
                  sm="6"
                >
                  <v-text-field
                    label="Last Name"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
              </v-row>

              <v-row>
                <v-col cols="12">
                  <v-text-field
                    label="Address"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
              </v-row>

              <v-row>
                <v-col cols="12">
                  <v-text-field
                    label="Date of Birth"
                    placeholder="YYYY-MM-DD"
                    outlined
                    dense
                  ></v-text-field>
                </v-col>
              </v-row>

              <v-row> </v-row>

              <FormButtonContainer
                :valid="valid"
                @continue="handleContinue"
                @save="handleSave"
              />
            </v-form>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script lang="ts" setup>
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { computed, onMounted, reactive, ref, watch } from 'vue'

const emit = defineEmits(['handle-continue', 'handle-save'])

const valid = ref(false)

function handleContinue() {
  emit('handle-continue')
}

function handleSave() {
  emit('handle-save')
}
</script>
