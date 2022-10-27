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
      <v-sheet height="675">
        <v-calendar
          ref="calendar"
          v-model="state.focus"
          color="primary"
          first-time="8"
          first-interval="8"
          interval-width="80"
          interval-count="16"
          :type="state.type"
          :events="state.events"
          @click:date="viewDay($event)"
          @click:event="selectEvent($event)"
        >
        </v-calendar>
        <v-menu
          v-model="state.selectedOpen"
          :activator="state.selectedElement"
          min-width="250px"
          min-height="150px"
          max-height="250px"
          max-width="450px"
        >
          <v-card
            flat
            min-width="250px"
            min-height="150px"
            max-height="250px"
            max-width="450px"
          >
            <v-card-title>
              {{ $t('Confirm Appointment Selection') }}
            </v-card-title>
            <v-card-text class="button-card">
              <v-btn
                color="primary"
                @click="handleConfirm"
                class="m-3"
              >
                {{ $t('Confirm') }}
              </v-btn>
              <v-btn
                class="m-3"
                color="error"
                @click="state.selectedOpen = false"
              >
                {{ $t('Cancel') }}
              </v-btn>
            </v-card-text>
          </v-card>
        </v-menu>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { EventType } from '@shared-utils/types/defaultTypes';
import { onMounted, reactive } from 'vue';

const state = reactive({
  focus: '',
  type: 'month',
  selectedEvent: {} as EventType,
  selectedOpen: false,
  selectedElement: null,
  selectedDay: '',
  // TODO: this need to change to a props with the api.
  events: [] as Array<EventType>,
});

function viewDay({ date }) {
  state.focus = date;
  state.type = 'day';
}

function setToday() {
  state.focus = 'date';
  state.type = 'day';
}

function selectEvent(event) {
  state.selectedEvent = event.event;
  state.selectedElement = event.nativeEvent.target;
  state.selectedOpen = true;
}

function handleConfirm() {
  // TODO: Call the api to confirm that the appointment is still available
  state.events = state.events.filter(
    event => event.start !== state.selectedEvent.start
  );
}

/**
 * This is a throw away function just to create random data for testing
 */
function createTestEvent() {
  for (let i = 0; i < 100; i++) {
    const year = Math.floor(Math.random() + 0.4) + 2022;
    const month = Math.floor(Math.random() * 10) + 2;
    const day = Math.floor(Math.random() * 12) + month;
    const min = Math.floor(Math.random() + 0.4) * 30;

    const randomDate = new Date(`${year} ${month} ${day} ${month}:${min}`);
    let endMin = min;
    let endHour = month;

    if (min >= 30) {
      endMin = 0;
      endHour += 1;
    }

    const endDate = new Date(`${year} ${month} ${day} ${endHour}:${endMin}`);
    const body = {
      name: 'opening',
      start: randomDate,
      end: endDate,
      color: 'blue',
      timed: true,
    };

    state.events.push(body);
  }
}

onMounted(() => {
  createTestEvent();
});
</script>

<style lang="scss" scoped>
.calendar-container {
  height: 800px;
  overflow: scroll;
  margin: 2em 0;
}
.button-card {
  height: 100%;
  display: flex;
  justify-content: space-around;
  align-items: center;
}
</style>
