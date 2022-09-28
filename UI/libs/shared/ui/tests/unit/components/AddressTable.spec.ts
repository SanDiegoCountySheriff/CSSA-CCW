import Vuetify from 'vuetify';
import AddressTable from '@shared-ui/components/tables/AddressTable.vue';
import { createLocalVue, shallowMount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('AddressTable', () => {
  let vuetify;

  beforeEach(() => {
    vuetify = new Vuetify();
  });

  it('Should render the address table in the correct order', () => {
    //@ts-ignore
    const wrapper = shallowMount(AddressTable, {
      localVue,
      vuetify,
      mocks: {
        $t: text => {
          text;
        },
      },
      propsData: {
        addresses: [
          {
            addressLine1: 'line1',
            addressLine2: 'line2',
            city: 'city',
          },
        ],
      },
    });
    expect(wrapper.findAll('td').at(0).element.textContent).toContain('line1');
    expect(wrapper.findAll('td').at(1).element.textContent).toContain('line2');
    expect(wrapper.findAll('td').at(2).element.textContent).toContain('city');
  });
});
