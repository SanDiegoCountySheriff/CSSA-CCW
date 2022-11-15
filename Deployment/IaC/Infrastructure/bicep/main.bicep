@description('Location for the resources.')
param location string = resourceGroup().location

// Tag and name builder params
param agency string
param dept string
param environment string

param cosmosDB object

var app_name = 'ccw'
var cosmosDB_name = 'cdb-${agency}-${dept}-${app_name}-${environment}-${cosmosDB.number}'

module cosmosdb 'modules/CosmosDB.bicep' = {
  name: 'cosmosDeploy'
  params: {
    location: location
    accountName: cosmosDB_name
    containerName: cosmosDB.containerName
    databaseName: cosmosDB.dbName
  }
}
