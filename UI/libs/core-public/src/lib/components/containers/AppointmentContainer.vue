<template>
  <v-row class="calendar-container">
    <v-col>
      <v-sheet height="64">
        <v-toolbar
          flat
          color="primary"
        >
          <v-btn
            outlined
            class="mr-4"
            color="white"
            @click="setToday"
          >
            {{ $t('Today') }}
          </v-btn>

          <v-btn
            fab
            text
            small
            color="white"
            @click="$refs.calendar.prev()"
          >
            <v-icon> mdi-chevron-left </v-icon>
          </v-btn>
          <v-btn
            fab
            text
            small
            color="white"
            @click="$refs.calendar.next()"
          >
            <v-icon> mdi-chevron-right </v-icon>
          </v-btn>
          <v-toolbar-title
            v-if="$refs.calendar"
            :style="{
              color: 'white',
            }"
            class="ml-5"
          >
            {{ $refs.calendar.title }}
          </v-toolbar-title>
          <v-spacer />
          <v-menu>
            <template #activator="{ on, attrs }">
              <v-btn
                outlined
                color="white"
                v-bind="attrs"
                v-on="on"
              >
                {{ $t(state.type) }}
                <v-icon right> mdi-menu-down </v-icon>
              </v-btn>
            </template>
            <v-list>
              <v-list-item @click="state.type = 'day'">
                <v-list-item-title>{{ $t('Day') }}</v-list-item-title>
              </v-list-item>
              <v-list-item @click="state.type = 'week'">
                <v-list-item-title>{{ $t('Week') }}</v-list-item-title>
              </v-list-item>
              <v-list-item @click="state.type = 'month'">
                <v-list-item-title>{{ $t('Month') }}</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>
        </v-toolbar>
      </v-sheet>
      <!-- Put the calendar here. -->
      <v-calendar
        ref="calendar"
        v-model="state.focus"
        color="primary"
        first-time="8"
        first-interval="8"
        interval-minutes="30"
        interval-width="80"
        interval-count="16"
        :type="state.type"
        :events="state.events"
        @click:date="viewDay($event)"
        @click:time="createEvent($event)"
      >
      </v-calendar>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { reactive } from 'vue';

const state = reactive({
  focus: '',
  type: 'month',
  selectedEvent: {},
  selectedDay: '',
  // TODO: this need to change to a props with the api.
  events: [] as Array<any>,
});
function viewDay({ date }) {
  state.focus = date;
  state.type = 'day';
}

function setToday() {
  state.focus = 'date';
  state.type = 'day';
}

function createEvent(event) {
  let minutes = '';

  if (event.minute > 0 && event.minute < 30) {
    minutes = '00';
  } else {
    minutes = '30';
  }
  const startDate = new Date(`${event.date} ${event.hour}:${minutes}`);
  const endDate = new Date(`${event.date} ${event.hour + 1}:${minutes}`);
  const body = {
    name: `appointment ${event.hour}:${minutes}`,
    start: startDate,
    end: endDate,
    color: 'blue',
    timed: true,
  };
  state.events.push(body);
  // TODO: incorporate the api call to check for appointment availability then set the event.
}
</script>

<style lang="scss" scoped>
.calendar-container {
  height: 600px;
  max-height: 600px;
  overflow: scroll;
  margin: 2em 0;
}
</style>
