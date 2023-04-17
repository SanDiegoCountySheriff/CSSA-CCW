module.exports = {
  displayName: 'admin',
  preset: '../../jest.preset.js',
  transform: {
    '^.+.vue$': '@vue/vue2-jest',
    '.+.(css|styl|less|sass|scss|svg|png|jpg|ttf|woff|woff2)$':
      'jest-transform-stub',
    '^.+.tsx?$': 'ts-jest',
  },
  moduleFileExtensions: ['ts', 'tsx', 'vue', 'js', 'json'],
  coverageDirectory: '../../coverage/apps/admin',
  snapshotSerializers: ['jest-serializer-vue'],
  globals: {
    'ts-jest': {
      tsconfig: 'apps/admin/tsconfig.spec.json',
      babelConfig: 'apps/admin/babel.config.js',
    },
    'vue-jest': {
      tsConfig: 'apps/admin/tsconfig.spec.json',
      babelConfig: 'apps/admin/babel.config.js',
    },
  },
}
