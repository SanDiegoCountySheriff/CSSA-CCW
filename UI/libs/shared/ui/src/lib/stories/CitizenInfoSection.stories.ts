import { withKnobs } from '@storybook/addon-knobs';
import CitizenshipInfoSection from '../components/info-sections/CitizenInfoSection.vue';

export default {
  title: 'CitizenshipInfoSection',
  decorators: [withKnobs],
};

export const CitizenshipInfoSection = () => ({
  props: {
    color: 'teal lighten-5',
    citizenshipInfo: {
      citizen: true,
      militaryStatus: 'testStatus',
    },
  },
  components: { CitizenshipInfoSection },
  template: `<CitizenshipInfoSection :color='$props.color' :citizenship-info='$props.citizenshipInfo'/>`,
});

CitizenshipInfoSection.story = {
  name: 'CitizenshipInfoSection',
};
