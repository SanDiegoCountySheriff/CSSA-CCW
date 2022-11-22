@description('Location for the resources.')
param location string = resourceGroup().location

// Tag and name builder params
@description('Agency abbreviation')
param agency string

@description('Department name')
param dept string

@description('Environment Name')
@allowed([
  'dev'
  'qa'
  'prod'
])
param environment string

@description('Resource suffix number.')
param cosmosdb_number string = '001'

@description('Resource suffix number.')
param appconfig_number string = '001'

var app_name = 'ccw'

var appconfig_name = 'ac-${agency}-${dept}-${app_name}-${environment}-${appconfig_number}'
var cosmosdb_name = 'cdb-${agency}-${dept}-${app_name}-${environment}-${cosmosdb_number}'

module appConfig 'modules/appConfig.bicep' = {
  name: 'appConfigDeploy'
  params: {
    appconfig_name: appconfig_name
    location: location
  }
}
module cosmosdb 'modules/CosmosDB.bicep' = {
  name: 'cosmosDeploy'
  params: {
    cosmos_account_name: cosmosdb_name
    location: location
  }
}
