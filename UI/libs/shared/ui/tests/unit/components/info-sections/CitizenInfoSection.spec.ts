/* eslint-disable @typescript-eslint/ban-ts-comment */
import Vuetify from 'vuetify';
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('CitizenInfoSection', () => {
  let vuetify;

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
