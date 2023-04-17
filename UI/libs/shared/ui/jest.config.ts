module.exports = {
  displayName: 'shared-ui',
  preset: '../../../jest.preset.js',
  setupFilesAfterEnv: ['<rootDir>/jest.setup.ts'],
  transform: {
    '^.+.vue$': '@vue/vue2-jest',
    '.+.(css|styl|less|sass|scss|svg|png|jpg|ttf|woff|woff2)$':
      'jest-transform-stub',
    '^.+.tsx?$': 'ts-jest',
  },
  moduleFileExtensions: ['ts', 'tsx', 'vue', 'js', 'json'],
  coverageDirectory: '../../../coverage/libs/shared/ui',
  snapshotSerializers: ['jest-serializer-vue'],
  globals: {
    'ts-jest': {
      tsconfig: 'libs/shared/ui/tsconfig.spec.json',
      babelConfig: 'libs/shared/ui/babel.config.js',
    },
    'vue-jest': {
      tsConfig: 'libs/shared/ui/tsconfig.spec.json',
      babelConfig: 'libs/shared/ui/babel.config.js',
    },
  },
}
