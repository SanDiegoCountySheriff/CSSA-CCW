import { withKnobs } from '@storybook/addon-knobs';
import IdInfoSection from '../components/info-sections/IdInfoSection.vue';

export default {
  title: 'IdInfoSection',
  decorators: [withKnobs],
};

export const IdInfoSection = () => ({
  props: {
    color: 'teal lighten-5',
    idInfo: {
      idNumber: 'test1',
      issuingState: 'testState',
    },
  },
  components: { IdInfoSection },
  template: `<IdInfoSection :color='$props.color' :id-info='$props.idInfo'></IdInfoSection>`,
});

IdInfoSection.story = {
  name: 'IdInfoSection',
};
