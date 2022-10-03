import { withKnobs } from '@storybook/addon-knobs';
import PersonalInfoSection from '../components/info-sections/PersonalInfoSection.vue';

export default {
  title: 'PersonalInfoSection',
  decorators: [withKnobs],
};

export const PersonalInfoSection = () => ({
  props: {
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
  components: { PersonalInfoSection },
  template: `<PersonalInfoSection :color='$props.color' :personal-info='$props.personalInfo'></PersonalInfoSection>`,
});

PersonalInfoSection.story = {
  name: 'PersonalInfoSection',
};
