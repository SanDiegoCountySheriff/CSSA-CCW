/* eslint-disable @typescript-eslint/no-var-requires */
const rootMain = require('../../../../.storybook/main');
const path = require('path');

module.exports = {
  ...rootMain,

  stories: [
    ...rootMain.stories,
    '../src/lib/**/*.stories.mdx',
    '../src/lib/**/*.stories.@(js|jsx|ts|tsx)',
  ],
  addons: [...rootMain.addons],
  webpackFinal: async (config, { configType }) => {
    // apply any global webpack configs that might have been specified in .storybook/main.js
    if (rootMain.webpackFinal) {
      config = await rootMain.webpackFinal(config, { configType });
    }
    config.resolve.modules = [path.resolve(__dirname, '..'), 'node_modules'];
    config.resolve.alias = {
      ...config.resolve.alias,
      '~storybook': path.resolve(__dirname),
      '@shared-ui': path.resolve(__dirname, '../src/lib'),
    };
    config.resolve.extensions.push(
      '.vue',
      '.css',
      '.less',
      '.scss',
      '.sass',
      '.html',
      '.svg',
      '.ts',
      '.js'
    );

    // add your own webpack tweaks if needed

    return config;
  },
};
