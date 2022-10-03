import Vuetify from 'vuetify';
import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('PersonalInfoSection', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
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

  it('should display the correct last name', () => {
    expect(wrapper.findAll('p').at(0).element.textContent).toContain('last');
  });

  it('should display the correct first name', () => {
    expect(wrapper.findAll('p').at(1).element.textContent).toContain('first');
  });


  it('should display the correct middle name', () => {
    expect(wrapper.findAll('p').at(2).element.textContent).toContain('middle');
  });


  it('should display the correct middle name status', () => {
    expect(wrapper.findAll('p').at(3).element.textContent).toContain('false');
  });

  it('should display the correct maiden name', () => {
    expect(wrapper.findAll('p').at(4).element.textContent).toContain('');
  });

  it('should display the correct suffix name', () => {
    expect(wrapper.findAll('p').at(5).element.textContent).toContain('');
  });

  it('should display the correct ssn ', () => {
    expect(wrapper.findAll('p').at(6).element.textContent).toContain('ssn');
  });

  it('should display the correct marital status ', () => {
    expect(wrapper.findAll('p').at(7).element.textContent).toContain('single');
  });
});
