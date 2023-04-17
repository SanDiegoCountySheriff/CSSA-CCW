/* eslint-disable @typescript-eslint/ban-ts-comment */

import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue'
import Vuetify from 'vuetify'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()

describe('AppearanceInfoSection', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(AppearanceInfoSection, {
      localVue,
      vuetify,
      mocks: {
        $t: text => text,
      },
      propsData: {
        appearanceInfo: {
          gender: 'male',
          heightFeet: 5,
          heightInch: 5,
          weight: 5,
          hairColor: 'black',
          eyeColor: 'black',
          physicalDesc: 'something',
        },
      },
    })
  })

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot()
  })
})
