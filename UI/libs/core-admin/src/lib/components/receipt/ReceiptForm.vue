<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable vue-a11y/form-has-label -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/require-v-for-key -->
<template>
  <div
    class="receipt-container"
    id="print-container"
  >
    <div class="main-content">
      <div id="invoice-app">
        <v-card-title class="text-h5 mr-2"> {{ $t('Receipt') }}</v-card-title>
        <v-row>
          <v-col
            cols="12"
            md="6"
            sm="12"
          >
            <v-text-field
              placeholder="Order ID"
              class="company-name"
              :label="$t('Order ID')"
              v-model="state.orderID"
              dense
              outlined
            />
          </v-col>
        </v-row>
        <v-row>
          <v-col
            cols="12"
            md="6"
            sm="12"
          >
            <v-menu
              v-model="state.menu"
              :close-on-content-click="false"
              transition="scale-transition"
              offset-y
              min-width="auto"
            >
              <template #activator="{ on, attrs }">
                <v-text-field
                  v-model="state.invoiceDate"
                  :label="$t('Today\'s Date')"
                  hint="YYYY-MM-DD format"
                  persistent-hint
                  readonly
                  v-bind="attrs"
                  v-on="on"
                  dense
                  outlined
                >
                </v-text-field>
              </template>
              <v-date-picker
                v-model="state.invoiceDate"
                no-title
                @input="state.menu = false"
                scrollable
              >
              </v-date-picker>
            </v-menu>
          </v-col>
          <v-col
            cols="12"
            md="6"
            sm="12"
          >
            <v-text-field
              placeholder="Agency Name"
              class="company-name"
              :label="$t('Agency Name')"
              v-model="state.company.name"
              dense
              outlined
            />
          </v-col>
        </v-row>
        <v-row>
          <v-col
            cols="12"
            md="6"
            sm="12"
          >
            <v-textarea
              :label="$t('Agency Details')"
              v-model="state.company.contact"
              :rules="[
                v =>
                  !v ||
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
              dense
              outlined
            ></v-textarea>
          </v-col>
          <v-col
            cols="12"
            md="6"
            sm="12"
          >
            <v-textarea
              :label="$t('Client Details')"
              v-model="state.client"
              :rules="[
                v =>
                  !v ||
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
              dense
              outlined
            ></v-textarea>
          </v-col>
        </v-row>
        <table
          class="responsive-table"
          aria-label="Receipt form"
        >
          <thead>
            <tr class="text-left">
              <th scope="col">No</th>
              <th scope="col">Item</th>
              <th scope="col">Price</th>
              <th scope="col">Quantity</th>
              <th scope="col">Total</th>
              <th scope="col"></th>
            </tr>
          </thead>
          <tr v-for="(item, index) in state.items">
            <td data-label="No">
              {{ index + 1 }}
            </td>
            <td data-label="Item">
              <v-text-field
                label="License"
                type="text"
                placeholder="License type"
                v-model="item.description"
              />
            </td>
            <td data-label="Cost">
              <div class="cell-with-input">
                <v-text-field
                  :label="$t('Cost')"
                  :rules="[v => !!v || $t('Cost Fee is required')]"
                  :hint="$t('Add cost fees for permit ')"
                  :prefix="state.invoiceCurrency.symbol"
                  v-model="item.price"
                  placeholder="XX.XX"
                  type="number"
                  :color="$vuetify.theme.dark ? '' : 'blue1'"
                  min="0"
                  required
                />
              </div>
            </td>
            <td data-label="Quantity">
              <v-text-field
                :label="$t('Quantity')"
                :rules="[v => !!v || $t('Quantity is required')]"
                :hint="$t('Add quantity for permit ')"
                v-model="item.quantity"
                placeholder="XX.XX"
                type="number"
                :color="$vuetify.theme.dark ? '' : 'blue1'"
                min="0"
                required
              />
            </td>
            <td data-label="Total">
              {{ decimalDigits(item.price * item.quantity) }}
            </td>
            <td class="text-right">
              <v-btn
                color="error"
                @click="deleteItem(index)"
                @keydown="deleteItem(index)"
              >
                DELETE
              </v-btn>
            </td>
          </tr>
        </table>
        <v-btn
          :color="$vuetify.theme.dark ? '' : 'blue'"
          class="white--text"
          @click="addNewItem"
          @keydown="addNewItem"
        >
          ADD
        </v-btn>
        <v-row>
          <v-col
            cols="12"
            md="4"
            sm="12"
          >
            <v-simple-table class="text-left my-3">
              <tr>
                <td>Subtotal</td>
                <td>{{ decimalDigits(subTotal) }}</td>
              </tr>
              <tr>
                <td>
                  <div class="cell-with-input">
                    Discount
                    <input
                      class="text-right"
                      type="number"
                      min="0"
                      max="100"
                      v-model="state.discountRate"
                    />%
                  </div>
                </td>
                <td>{{ decimalDigits(discountTotal) }}</td>
              </tr>
              <tr>
                <td>
                  <div class="cell-with-input">
                    Tax
                    <input
                      class="text-right"
                      type="number"
                      min="0"
                      max="100"
                      v-model="state.taxRate"
                    />%
                  </div>
                </td>
                <td>{{ decimalDigits(taxTotal) }}</td>
              </tr>
              <tr class="text-bold">
                <td>Grand Total</td>
                <td>{{ decimalDigits(grandTotal) }}</td>
              </tr>
            </v-simple-table>
          </v-col>
        </v-row>
        <div class="section-spacer">
          <v-textarea
            label="Notes"
            :rules="[
              v =>
                !v ||
                (v && v.length <= 1000) ||
                $t('Maximum 1000 characters are allowed'),
            ]"
            dense
            outlined
          ></v-textarea>
        </div>

        <div class="section-spacer">
          <v-textarea
            label="Terms"
            :rules="[
              v =>
                !v ||
                (v && v.length <= 1000) ||
                $t('Maximum 1000 characters are allowed'),
            ]"
            dense
            outlined
          ></v-textarea>
        </div>
        <v-btn
          color="accent"
          @click="printInvoice"
          @keydown="printInvoice"
        >
          Print Receipt
        </v-btn>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { computed, reactive } from 'vue';

const brandStore = useBrandStore();

const state = reactive({
  invoiceCurrency: {
    symbol: '$',
    name: 'US Dollar',
    symbol_native: '$',
    decimal_digits: 2,
    rounding: 0,
    code: 'USD',
    name_plural: 'US dollars',
  },
  taxRate: 20,
  discountRate: 0,
  items: [
    { description: '', quantity: 0, price: 0 },
    { description: '', quantity: 0, price: 0 },
  ],
  company: {
    name: brandStore.getBrand.agencyName,
    contact: `${brandStore.getBrand.agencyAddress}\n${brandStore.getBrand.agencyTelephone}\n${brandStore.getBrand.agencyEmail}`,
  },
  client: 'Client information',
  invoiceDate: '',
  orderID: '',
  menu: false,
});

function addNewItem() {
  state.items.push({ description: 'Item name', quantity: 0, price: 0 });
}

function deleteItem(index) {
  state.items.splice(index, 1);
}

function decimalDigits(value) {
  return `${state.invoiceCurrency.symbol} ${value.toFixed(
    state.invoiceCurrency.decimal_digits
  )}`;
}

function printInvoice() {
  printPageArea('print-container');
}

const subTotal = computed(() => {
  let total = state.items.reduce((accumulator, item) => {
    return accumulator + item.price * item.quantity;
  }, 0);

  return total;
});

const discountTotal = computed(() => {
  let total = subTotal.value * (state.discountRate / 100);

  return total;
});

const taxTotal = computed(() => {
  let total = (subTotal.value - discountTotal.value) * (state.taxRate / 100);

  return total;
});

const grandTotal = computed(() => {
  let total = subTotal.value - discountTotal.value + taxTotal.value;

  return total;
});

function printPageArea(areaID) {
  let printContent = document.getElementById(areaID).innerHTML;
  let originalContent = document.body.innerHTML;

  document.body.innerHTML = printContent;
  window.print();
  document.body.innerHTML = originalContent;
}
</script>

<style lang="scss">
$red: #ff5f6d;
$yellow: #ffc371;
$green: #81cf71;
$white: #faebd7;
$grey: darken($white, 10%);

#invoice-app {
  padding: 2rem;
  border-radius: 0.5rem;
}

.header {
  @media (min-width: 761px) {
    display: flex;
  }

  > div {
    &:nth-child(-n + 1) {
      @media (min-width: 761px) {
        order: 1;
        flex: 1;
        text-align: right;
        padding-left: 1rem;
      }
    }
  }
}

.section-spacer {
  margin: 1rem 0;
}

input,
select,
textarea {
  background-color: transparentize($color: white, $amount: 0.7);
  border: none;
  display: inline-block;
  transition: background-color 0.3s ease-in-out;
  width: 100%;

  &:focus {
    outline-color: $yellow;
    background-color: transparentize($color: white, $amount: 0.4);
  }

  &:hover {
    background-color: transparentize($color: white, $amount: 0.5);
  }

  @media print {
    background-color: transparent;
  }

  @media only screen and (min-width: 761px) {
    width: auto;
  }
}

textarea {
  width: 100%;
  min-height: 80px;
}

select {
  @media only screen and (max-width: 760px) {
    width: 100%;
  }

  @media print {
    appearance: none;
  }
}

.company-name {
  font-size: 2rem;
}

table {
  width: auto;
  border-collapse: collapse;
  margin: 2rem 0;

  thead {
    th {
      padding: 0.5rem 1rem;

      &:nth-child(-n + 1) {
        padding-left: 0;
      }

      &:nth-last-child(-n + 1) {
        padding-right: 0;
      }
    }
  }

  tr {
    border-bottom: 1px solid $grey;

    td {
      padding: 0.5rem 1rem;

      &:nth-child(-n + 1) {
        padding-left: 0;
      }

      &:nth-last-child(-n + 1) {
        padding-right: 0;
      }

      input {
        width: 100%;
      }

      .cell-with-input {
        display: flex;

        input {
          margin: 0 0.2rem;
        }
      }
    }
  }
}

.responsive-table {
  width: 100%;

  @media only screen and (max-width: 760px) {
    table,
    thead,
    tbody,
    th,
    td,
    tr {
      display: block;
    }

    thead tr {
      position: absolute;
      top: -9999px;
      left: -9999px;
    }

    tr {
      padding: 2rem 0;
    }

    td[data-label] {
      position: relative;
      padding-left: 40%;
      display: flex;
      align-items: center;

      &:before {
        content: attr(data-label);
        position: absolute;
        top: 0.5rem;
        left: 0;
        width: 35%;
        padding-right: 10px;
        white-space: nowrap;
        font-weight: bold;
      }
    }
  }
}

button {
  border: none;
  border-radius: 100px;
  padding: 0.5rem 1rem;
  cursor: pointer;
  transition: background-color 0.3s ease-in-out;

  @media print {
    display: none;
  }
}

.text-right {
  text-align: right;
}

.text-bold {
  font-weight: bold;
}
</style>
