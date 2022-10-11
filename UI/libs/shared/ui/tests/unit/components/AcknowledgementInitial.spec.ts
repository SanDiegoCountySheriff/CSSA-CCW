import Vuetify from 'vuetify';
import AcknowledgementInitial from '@shared-ui/components/acknowledgement-section/AcknowledgementInitial.vue';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe(' AcknowledgementInitial', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    //@ts-ignore
    wrapper = mount(AcknowledgementInitial, {
      localVue,
      vuetify,
      mocks: {
        $t: text => {
          text;
        },
      },
      propsData: {
        handleAccept: () => null,
        handleDecline: () => null,
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
