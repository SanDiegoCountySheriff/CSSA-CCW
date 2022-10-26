/* eslint-disable @typescript-eslint/ban-ts-comment */
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('AddressInfoSection', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(AddressInfoSection, {
      localVue,
      vuetify,
      mocks: {
        $t: text => text,
      },
      propsData: {
        addressInfo: {
          addressLine1: 'line1',
          addressLine2: 'line2',
          city: 'city',
          state: 'state',
          county: 'county',
          zip: 'zip',
          country: 'country',
        },
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
