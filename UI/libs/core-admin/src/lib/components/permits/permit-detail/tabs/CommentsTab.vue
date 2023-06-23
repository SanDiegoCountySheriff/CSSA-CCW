<template>
  <div>
    <template
      v-for="(comment, index) in permitStore.getPermitDetail.application
        .comments"
    >
      <v-list-item :key="index">
        <v-list-item-content>
          <v-list-item-title class="text-wrap">
            {{ comment.text }}
          </v-list-item-title>
          <v-list-item-subtitle>
            Comment Made By: {{ comment.commentMadeBy }}
          </v-list-item-subtitle>
          <v-list-item-subtitle>
            Comment Made On:
            {{ new Date(comment.commentDateTimeUtc).toLocaleString() }}
          </v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </template>

    <v-textarea
      v-model="commentText"
      label="Comment"
      outlined
      auto-grow
      clearable
    ></v-textarea>

    <v-btn
      color="primary"
      @click="handleAddComment"
      small
    >
      {{ $t('Add Comment') }}
    </v-btn>
  </div>
</template>

<script setup lang="ts">
import { CommentType } from '@shared-utils/types/defaultTypes'
import { ref } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'

const permitStore = usePermitsStore()
const authStore = useAuthStore()
const commentText = ref('')

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi('Added Comment'),
  {
    enabled: false,
  }
)

function handleAddComment() {
  const comment: CommentType = {
    text: commentText.value,
    commentDateTimeUtc: new Date().toISOString(),
    commentMadeBy: authStore.auth.userEmail,
  }

  permitStore.getPermitDetail.application.comments.push(comment)

  updatePermitDetails()

  commentText.value = ''
}
</script>
