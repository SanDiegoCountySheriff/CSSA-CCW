
@description('App Config name')
param appconfig_name string

@description('Location for the Cosmos DB account.')
param location string = resourceGroup().location

@description('SKU for App Config')
param sku string = 'standard'

resource app_config 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = {
  name: appconfig_name
  location: location
  tags: {}
  sku: {
    name: sku
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    encryption: {
    }
    publicNetworkAccess: 'Disabled'
    disableLocalAuth: false
    softDeleteRetentionInDays: 1
    enablePurgeProtection: false
  }
}
