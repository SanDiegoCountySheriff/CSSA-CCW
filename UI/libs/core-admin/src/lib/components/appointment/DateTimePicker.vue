<template>
  <v-dialog
    v-model="display"
    :width="dialogWidth"
  >
    <template #activator="{ on }">
      <v-btn
        :disabled="disabled"
        :loading="loading"
        v-on="on"
        color="primary"
        small
        block
      >
        <v-icon left> mdi-calendar-blank </v-icon>
        Schedule
        <template #progress>
          <slot name="progress">
            <v-progress-linear
              color="accent"
              indeterminate
              absolute
              height="2"
            ></v-progress-linear>
          </slot>
        </template>
      </v-btn>
    </template>

    <v-card>
      <v-card-text class="px-0 py-0">
        <v-tabs
          fixed-tabs
          v-model="activeTab"
        >
          <v-tab key="calendar">
            <slot name="dateIcon">
              <v-icon>mdi-calendar</v-icon>
            </slot>
          </v-tab>
          <v-tab
            key="timer"
            :disabled="dateSelected"
          >
            <slot name="timeIcon">
              <v-icon>mdi-clock</v-icon>
            </slot>
          </v-tab>
          <v-tab-item key="calendar">
            <v-date-picker
              v-model="date"
              v-bind="datePickerProps"
              @input="showTimePicker"
              full-width
            ></v-date-picker>
          </v-tab-item>
          <v-tab-item key="timer">
            <v-time-picker
              ref="timer"
              class="v-time-picker-custom"
              v-model="time"
              v-bind="timePickerProps"
              full-width
            ></v-time-picker>
          </v-tab-item>
        </v-tabs>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <slot name="actions">
          <v-btn
            text
            @click.native="clearHandler"
          >
            {{ clearText }}
          </v-btn>
          <v-btn
            color="success"
            text
            @click="okHandler"
          >
            {{ okText }}
          </v-btn>
        </slot>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { format, parse } from 'date-fns';

const DEFAULT_DATE = '';
const DEFAULT_TIME = '00:00:00';
const DEFAULT_DATE_FORMAT = 'yyyy-MM-dd';
const DEFAULT_TIME_FORMAT = 'HH:mm:ss';
const DEFAULT_DIALOG_WIDTH = 600;
const DEFAULT_CLEAR_TEXT = 'CLEAR';
const DEFAULT_OK_TEXT = 'OK';

export default defineComponent({
  name: 'VDatetimePicker',
  model: {
    prop: 'datetime',
    event: 'input',
  },
  props: {
    datetime: {
      type: [Date, String],
      default: null,
    },
    disabled: {
      type: Boolean,
    },
    loading: {
      type: Boolean,
    },
    label: {
      type: Boolean,
      default: true,
    },
    dialogWidth: {
      type: Number,
      default: DEFAULT_DIALOG_WIDTH,
    },
    dateFormat: {
      type: String,
      default: DEFAULT_DATE_FORMAT,
    },
    timeFormat: {
      type: String,
      default: 'HH:mm',
    },
    clearText: {
      type: String,
      default: DEFAULT_CLEAR_TEXT,
    },
    okText: {
      type: String,
      default: DEFAULT_OK_TEXT,
    },
    // eslint-disable-next-line vue/require-default-prop
    textFieldProps: {
      type: Object,
    },
    // eslint-disable-next-line vue/require-default-prop
    datePickerProps: {
      type: Object,
    },
    // eslint-disable-next-line vue/require-default-prop
    timePickerProps: {
      type: Object,
    },
  },
  data() {
    return {
      display: false,
      activeTab: 0,
      date: DEFAULT_DATE,
      time: DEFAULT_TIME,
    };
  },
  setup() {
    const permitStore = usePermitsStore();

    return { permitStore };
  },
  mounted() {
    this.init();
  },
  computed: {
    dateTimeFormat() {
      return `${this.dateFormat} ${this.timeFormat}`;
    },
    defaultDateTimeFormat() {
      return `${DEFAULT_DATE_FORMAT} ${DEFAULT_TIME_FORMAT}`;
    },
    formattedDatetime() {
      return this.selectedDatetime
        ? format(this.selectedDatetime, this.dateTimeFormat)
        : '';
    },
    selectedDatetime() {
      if (this.date && this.time) {
        let datetimeString = `${this.date} ${this.time}`;

        if (this.time.length === 5) {
          datetimeString += ':00';
        }

        return parse(datetimeString, this.defaultDateTimeFormat, new Date());
      }

      return null;
    },
    dateSelected() {
      return !this.date;
    },
  },
  methods: {
    init() {
      if (!this.datetime) {
        return;
      }

      let initDateTime;

      if (this.datetime instanceof Date) {
        initDateTime = this.datetime;
      } else if (
        typeof this.datetime === 'string' ||
        this.datetime instanceof String
      ) {
        // see https://stackoverflow.com/a/9436948
        initDateTime = parse(this.datetime, this.dateTimeFormat, new Date());
      }

      this.date = format(initDateTime, DEFAULT_DATE_FORMAT);
      this.time = format(initDateTime, DEFAULT_TIME_FORMAT);
    },
    okHandler() {
      this.resetPicker();
      this.$emit('input', this.selectedDatetime);
      this.permitStore.getPermitDetail.application.appointmentDateTime =
        new Date(this.selectedDatetime).toISOString();
      this.permitStore.updatePermitDetailApi('Rescheduled Appointment');
    },
    clearHandler() {
      this.resetPicker();
      this.date = DEFAULT_DATE;
      this.time = DEFAULT_TIME;
      this.$emit('input', null);
    },
    resetPicker() {
      this.display = false;
      this.activeTab = 0;

      if (this.$refs.timer) {
        this.$refs.timer.selectingHour = true;
      }
    },
    showTimePicker() {
      this.activeTab = 1;
    },
  },
  watch: {
    datetime() {
      this.init();
    },
  },
});
</script>
