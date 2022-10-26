/* eslint-disable @typescript-eslint/ban-ts-comment */
import AliasTable from '@shared-ui/components/tables/AliasTable.vue';
import Vue from 'vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();
const alias = {
  prevLastName: 'pLast',
  prevFirstName: 'pFirst',
  prevMiddleName: 'pMiddle',
  cityWhereChanged: 'city',
  stateWhereChanged: 'state',
  courtFileNumber: 'court',
};

describe('AliasTable', () => {
  let vuetify;
  const tMock = {
    $t: text => text,
  };

  beforeEach(() => {
    Vue.use(Vuetify);
    vuetify = new Vuetify();
  });

  it('should match the snapshot', () => {
    //@ts-ignore
    const wrapper = mount(AliasTable, {
      localVue,
      vuetify,
      mocks: tMock,
    });

    expect(wrapper.html()).toMatchSnapshot();
  });

  it('Should render the Alias table with the correct text in the first row', () => {
    //@ts-ignore
    const wrapper = mount(AliasTable, {
      localVue,
      vuetify,
      propsData: {
        aliases: [alias],
      },
      mocks: tMock,
    });

    expect(wrapper.findAll('td').at(0).element.textContent).toContain('pLast');
  });

  it('Should render the Alias table with the correct text in the second row', () => {
    //@ts-ignore
    const wrapper = mount(AliasTable, {
      localVue,
      vuetify,
      propsData: {
        aliases: [alias],
      },
      mocks: tMock,
    });

    expect(wrapper.findAll('td').at(1).element.textContent).toContain('pFirst');
  });

  it('Should render the Alias table with the correct text in the third row', () => {
    //@ts-ignore
    const wrapper = mount(AliasTable, {
      localVue,
      vuetify,
      propsData: {
        aliases: [alias],
      },
      mocks: tMock,
    });

    expect(wrapper.findAll('td').at(2).element.textContent).toContain(
      'pMiddle'
    );
  });
  it('Should render the Alias table with the correct text in the fourth row', () => {
    //@ts-ignore
    const wrapper = mount(AliasTable, {
      localVue,
      vuetify,
      propsData: {
        aliases: [alias],
      },
      mocks: tMock,
    });

    expect(wrapper.findAll('td').at(3).element.textContent).toContain('city');
  });
  it('Should render the Alias table with the correct text in the fifth row', () => {
    //@ts-ignore
    const wrapper = mount(AliasTable, {
      localVue,
      vuetify,
      propsData: {
        aliases: [alias],
      },
      mocks: tMock,
    });

    expect(wrapper.findAll('td').at(4).element.textContent).toContain('state');
  });
  it('Should render the Alias table with the correct text in the sixth row', () => {
    //@ts-ignore
    const wrapper = mount(AliasTable, {
      localVue,
      vuetify,
      propsData: {
        aliases: [alias],
      },
      mocks: tMock,
    });

    expect(wrapper.findAll('td').at(5).element.textContent).toContain('court');
  });
});
