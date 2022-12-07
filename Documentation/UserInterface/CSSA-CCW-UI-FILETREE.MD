# Project File Structure

## Overview
 This is a basic guide for understanding the project structure for those that are new to the project and Nx workspaces.

### Project Structure
In this Nx workspace there are some divisions that you need to be aware of 
1. Applications (apps)
2. Libraries(libs)

#### Applications
 The basic structure and configuration of the separate applications.
  - **Keynotes:**
    - Apps should be kept as small as possible (Avoid importing libraries into Apps)
    - Apps should only contain the need code to make the app run
    - only application specific configuration should be present in the app


#### Libraries
 The libraries should contain the majority of the functional code.
  - **Keynotes:**
    -  Application specific code should be placed in the library corresponding to the the application
      - admin app specific code should be placed in the `core-admin` lib
      - public app specific code should be place in the `core-public` lib
    - components and code that will work for both applications should be placed in the shared lib.
    - The shared ui is split into to main sections
      - ui
      - utils
    - `shared-ui` should container things like components that are visible to the user
    - `shared-utils` should contain thing like shared types, lists, utility functions.


## Finding Files
1. If you are looking for the components from the site first determine the application you are using
  - Admin
  - Public
2. Look in the core-<app-name> file then open the src and then open the lib folders
   - Here you will find some common files
   - components: Contain all custom-made components for the projects and are broken down into subfolders
   - plugins: Vue plugin and app specific plugin configurations
   - router: Vue route and app specific config
   - stores: App Specific State stores
   - stories: App Specific Storybook testing stories
   - views: The view are components that routes correspond to i.e www.localhost:4000/form will route to
     the FormView component
3. Look in the shared lib
   - Here everything is pretty much the same except
     - There should be no views here
     - plugins: changes should only be made here if it will affect all applications

4. Finally if all else fails look in the root/apps/<app-name> folder or enable breadcrumbs in your IDE to easily determine your current corresponding folder tree.
