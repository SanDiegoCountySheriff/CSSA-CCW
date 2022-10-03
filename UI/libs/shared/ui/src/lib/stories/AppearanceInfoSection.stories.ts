import { withKnobs } from '@storybook/addon-knobs';
import AppearanceInfoSection from '../components/info-sections/AppearanceInfoSection.vue';

export default {
  title: 'AppearanceInfoSection',
  decorators: [withKnobs],
};

export const AppearanceInfoSection = () => ({
  props: {
    color: 'teal lighten-5',
    appearanceInfo: {
      gender: 'testGender',
      heightFeet: 5,
      heightInch: 5,
      weight: 5,
      hairColor: 'testHair',
      eyeColor: 'testEye',
      physicalDesc: ' testDisc',
    },
  },
  components: { AppearanceInfoSection },
  template: `<AppearanceInfoSection :color='$props.color' :appearance-info='$props.appearanceInfo'/>`,
});

AppearanceInfoSection.story = {
  name: 'AppearanceInfoSection',
};
