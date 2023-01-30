# CCW DevOps Documentation

## Overview

All of the Infrastructure code and DevOps pipelines are contained within the `Deployment` folder.

## Infrastructure

The `Deployment\IaC\Infrastructure` folder contains the sandbox ARM template, `sandboxTemplate.json`.  This template has been used to create resources in the Dev resource groups.  The sandbox template contains all the resources needed by the CCW application with the exception of the deployment scripts, which are contained in the Marketplace template.

A sample ARM Template Parameters file, `sandboxTemplate.parameters.json` is included and contains the parameter values needed for deployment to the Dev environment.

Note:  The sandbox template does not include the deployment scripts that are included in the marketplace template `mainTemplate.json`.

## Marketplace

The `Deployment\Marketplace` folder contains the files need to create the Marketplace package for upload to the storage account.

### createUiDefinition.json

This file contains the UI definitions and configurations used by the Azure Marketplace when selecting options.

### mainTemplate.json

This file contains all of the IaC definitions for the resources as well as several deployment scripts resources that apply the application code to the resources after deployment.

## Pipelines

### API Pipeline

The `azure-api-pipeline-template.yml` file contains the YAML commands to build and deploy any of the APIs to the Dev environment.  It also saves the build artifacts for use during the Marketplace Install and Update pipelines.

The Azure DevOps Pipelines UI has one pipeline for each API, with each pipeline pointing to this template file and sending parameters for which API to process.

### IaC Pipeline

The `azure-pipelines-iac.yml` file contains the YAML commands to build and deploy the ARM template `sandboxTemplate.json` to the Dev environment.

Note:  This pipeline **only** processes the sandbox template and its use is only intended for development purposes.

### UI Pipeline

The `azure-pipelines-ui.yml` file contains the YAML commands to build and deploy any of the UI applications to the Dev environment.  It also saves the build artifacts for use during the Marketplace Install and Update pipelines.

### Marketplace Pipelines

The `azure-pipelines-mp-install.yml` file contains the YAML commands to:
- Create the marketplace install package zip file.
- Create and save secrets in the shared KeyVault.
- Upload the marketplace zip file to the shared storage account.
- Upload application build artifacts and scripts to the shared storage account.

## Scripts

This folder contains Powershell and shell scripts used as either as-needed utility, run by a pipeline, or uploaded and run during Marketplace deployments.
