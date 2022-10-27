import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('PersonalInfoSection', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    //@ts-ignore
    wrapper = mount(PersonalInfoSection, {
      localVue,
      vuetify,
      mocks: {
        $t: text => {
          text;
        },
      },
      propsData: {
        color: 'teal lighten-5',
        personalInfo: {
          lastName: 'last',
          firstName: 'first',
          middleName: 'middle',
          noMiddleName: false,
          maidenName: '',
          suffix: '',
          ssn: 'ssn',
          maritalStatus: 'single',
        },
      },
    });
  });

  afterEach(() => {
    wrapper.destroy();
  });

  it('should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
