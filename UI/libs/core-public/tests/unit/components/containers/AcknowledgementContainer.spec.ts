import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('AcknowledgementContainer', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(AcknowledgementContainer, {
      localVue,
      vuetify,
      mocks: tMock,
      propsData: {
        nextRoute: 'test',
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
