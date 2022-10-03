import { withKnobs } from '@storybook/addon-knobs';
import DOBinfoSection from '../components/info-sections/DOBinfoSection.vue';

export default {
  title: 'DOBinfoSection',
  decorators: [withKnobs],
};

export const DOBinfoSection = () => ({
  props: {
    color: 'teal lighten-5',
    DOBInfo: {
      DOB: '04/02/00',
      birthCity: 'testCity',
      birthState: 'testState',
      birthContry: 'testCountry',
      currentAge: 21,
    },
  },
  components: { DOBinfoSection },
  template: `<DOBinfoSection color='$props.color' d-o-b-info='$props.DOBInfo'/>`,
});

DOBinfoSection.story = {
  name: 'DOBinfoSection',
};
