/* eslint-disable @typescript-eslint/ban-ts-comment */
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('DOBinfoSection', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(DOBinfoSection, {
      localVue,
      vuetify,
      mocks: {
        $t: text => text,
      },
      propsData: {
        DOBInfo: {
          birthDate: '01/01/01',
          birthCity: 'city',
          birthState: 'state',
          birthCountry: 'country',
          currentAge: 'age',
        },
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
