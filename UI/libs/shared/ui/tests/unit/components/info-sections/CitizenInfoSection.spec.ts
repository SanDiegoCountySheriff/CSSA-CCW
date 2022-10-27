/* eslint-disable @typescript-eslint/ban-ts-comment */
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('CitizenInfoSection', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(CitizenInfoSection, {
      localVue,
      vuetify,
      mocks: {
        $t: text => text,
      },
      propsData: {
        citizenshipInfo: {
          citizen: true,
          militaryStatus: 'active',
        },
        immigrantInfo: {},
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
