import { withKnobs } from '@storybook/addon-knobs';
import AddressInfoSection from '../components/info-sections/AddressInfoSection.vue';

export default {
  title: 'AddressInfoSection',
  decorators: [withKnobs],
};

export const AddressInfoSection = () => ({
  props: {
    color: 'teal lighten-5',
    addressInfo: {
      addressLine1: 'testLine1',
      addressLine2: 'testLine2',
      city: 'testCity',
      county: "testCounty",
      state: 'testState',
      zip: 'testZip',
      country: 'testCountry',
    },
  },
  components: { AddressInfoSection },
  template: `<AddressInfoSection :color='$props.color' :address-info='$props.addressInfo'></AddressInfoSection>`,
});

AddressInfoSection.story = {
  name: 'AddressInfoSection',
};
