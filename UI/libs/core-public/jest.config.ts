module.exports = {
  displayName: 'core-public',
  preset: '../../jest.preset.js',
  setupFilesAfterEnv: ['<rootDir>/jest.setup.ts'],
  transform: {
    '^.+.vue$': '@vue/vue2-jest',
    '.+.(css|styl|less|sass|scss|svg|png|jpg|ttf|woff|woff2)$':
      'jest-transform-stub',
    '^.+.tsx?$': 'ts-jest',
  },
  moduleFileExtensions: ['ts', 'tsx', 'vue', 'js', 'json'],
  coverageDirectory: '../../coverage/libs/core-public',
  snapshotSerializers: ['jest-serializer-vue'],
  globals: {
    'ts-jest': {
      tsconfig: 'libs/core-public/tsconfig.spec.json',
      babelConfig: 'libs/core-public/babel.config.js',
    },
    'vue-jest': {
      tsConfig: 'libs/core-public/tsconfig.spec.json',
      babelConfig: 'libs/core-public/babel.config.js',
    },
  },
}
