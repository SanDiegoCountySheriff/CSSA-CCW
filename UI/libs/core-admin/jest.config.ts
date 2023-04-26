module.exports = {
  displayName: 'core-admin',
  preset: '../../jest.preset.js',
  setupFilesAfterEnv: ['<rootDir>/jest.setup.ts'],
  transform: {
    '^.+.vue$': '@vue/vue2-jest',
    '.+.(css|styl|less|sass|scss|svg|png|jpg|ttf|woff|woff2)$':
      'jest-transform-stub',
    '^.+.tsx?$': 'ts-jest',
  },
  moduleFileExtensions: ['ts', 'tsx', 'vue', 'js', 'json'],
  coverageDirectory: '../../coverage/libs/core-admin',
  snapshotSerializers: ['jest-serializer-vue'],
  globals: {
    'ts-jest': {
      tsconfig: 'libs/core-admin/tsconfig.spec.json',
      babelConfig: 'libs/core-admin/babel.config.js',
    },
    'vue-jest': {
      tsConfig: 'libs/core-admin/tsconfig.spec.json',
      babelConfig: 'libs/core-admin/babel.config.js',
    },
  },
}
