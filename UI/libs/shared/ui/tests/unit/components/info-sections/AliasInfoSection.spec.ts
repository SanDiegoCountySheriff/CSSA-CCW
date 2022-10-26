/* eslint-disable @typescript-eslint/ban-ts-comment */
import AliasInfoSection from '@shared-ui/components/info-sections/AliasInfoSection.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('AliasInfosection', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(AliasInfoSection, {
      localVue,
      vuetify,
      mocks: {
        $t: text => text,
      },
      propsData: {
        prevLastName: 'last',
        prevFirstName: 'first',
        prevMiddleName: 'middle',
        cityWhereChanged: 'changed',
        stateWhereChanged: 'state',
        courtFileNumber: 'number',
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
