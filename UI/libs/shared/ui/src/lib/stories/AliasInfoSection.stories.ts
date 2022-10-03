import { withKnobs } from '@storybook/addon-knobs';
import AliasInfoSection from '../components/info-sections/AliasInfoSection.vue';

export default {
  title: 'AliasInfoSection',
  decorators: [withKnobs],
};

export const AliasInfoSection = () => ({
  props: {
    color: 'teal lighten-5',
    aliasInfo: {
      prevLastName: 'testLastName',
      prevFirstName: 'testFirst',
      prevMiddleName: 'testMiddle',
      cityWhereChanged: 'testCity',
      stateWhereChanged: 'testState',
      courtFileNumber: 'testNumber',
    },
  },
  components: { AliasInfoSection },
  template: `<AliasInfoSection :color='$props.color' :alias-info='$props.aliasInfo'></AliasInfoSection>`,
});

AliasInfoSection.story = {
  name: 'PersonalInfoSection',
};
