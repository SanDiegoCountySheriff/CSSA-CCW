// Parameters
@description('Cosmos DB account name, max length 44 characters, lowercase')
param accountName string

@description('Location for the Cosmos DB account.')
param location string = resourceGroup().location

// Tag values
@description('Department Name.')
param dept string = 'it'

@description('Environment Name')
@allowed([
  'dev'
  'qa'
  'prod'
])
param environment string = 'dev'

@description('Owner User Id.  Used to combine into an email address.')
param owner_name string = 'jkellash'

@description('Agency identifier.')
param agency string = 'sdsd'

@description('Application identifier')
param application string = 'ccw'

@description('The default consistency level of the Cosmos DB account.')
@allowed([
  'Eventual'
  'ConsistentPrefix'
  'Session'
  'BoundedStaleness'
  'Strong'
])
param defaultConsistencyLevel string = 'Session'

@description('Max stale requests. Required for BoundedStaleness. Valid ranges, Single Region: 10 to 2147483647. Multi Region: 100000 to 2147483647.')
@minValue(10)
@maxValue(2147483647)
param maxStalenessPrefix int = 100000

@description('Max lag time (minutes). Required for BoundedStaleness. Valid ranges, Single Region: 5 to 84600. Multi Region: 300 to 86400.')
@minValue(5)
@maxValue(86400)
param maxIntervalInSeconds int = 300

@description('The name for the database')
param databaseName string

@description('The name for the container')
param containerName string

@description('Maximum autoscale throughput for the container')
@minValue(1000)
@maxValue(1000000)
param autoscaleMaxThroughput int = 1000

// Variables

var consistencyPolicy = {
  Eventual: {
    defaultConsistencyLevel: 'Eventual'
  }
  ConsistentPrefix: {
    defaultConsistencyLevel: 'ConsistentPrefix'
  }
  Session: {
    defaultConsistencyLevel: 'Session'
  }
  BoundedStaleness: {
    defaultConsistencyLevel: 'BoundedStaleness'
    maxStalenessPrefix: maxStalenessPrefix
    maxIntervalInSeconds: maxIntervalInSeconds
  }
  Strong: {
    defaultConsistencyLevel: 'Strong'
  }
}

var locations = [
  {
    locationName: location
    failoverPriority: 0
    isZoneRedundant: false
  }
]

// Resources
resource account 'Microsoft.DocumentDB/databaseAccounts@2022-08-15' = {
  name: toLower(accountName)
  kind: 'GlobalDocumentDB'
  location: location
  tags: {
    defaultExperience: 'Core (SQL)'
    'hidden-cosmos-mmspecial': ''
    agency_name: agency
    agency_ori: 'ca0370000'
    application_name: application
    business_unit: dept
    criticality: 'low'
    data_classification: 'general'
    environment: environment
    owner_name: '${owner_name}@californiasheriffs.onmicrosoft.com'
    start_date: '09/29/2022'

  }
  properties: {
    consistencyPolicy: consistencyPolicy[defaultConsistencyLevel]
    locations: locations
    databaseAccountOfferType: 'Standard'
    enableAutomaticFailover: false
    enableMultipleWriteLocations: false
    isVirtualNetworkFilterEnabled: false
    virtualNetworkRules: []
    disableKeyBasedMetadataWriteAccess: false
    enableFreeTier: false
    enableAnalyticalStorage: false
    analyticalStorageConfiguration: {
      schemaType: 'WellDefined'
    }
    defaultIdentity: 'FirstPartyIdentity'
    networkAclBypass: 'None'
    disableLocalAuth: false
    enablePartitionMerge: false
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
    cors: []
    ipRules: []
    backupPolicy: {
      type: 'Periodic'
      periodicModeProperties: {
        backupIntervalInMinutes: 240
        backupRetentionIntervalInHours: 8
        backupStorageRedundancy: 'Geo'
      }
    }
    networkAclBypassResourceIds: []
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-05-15' = {
  parent: account
  name: databaseName
  properties: {
    resource: {
      id: databaseName
    }
  }
}

resource container 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-05-15' = {
  parent: database
  name: containerName
  properties: {
    resource: {
      id: containerName
      partitionKey: {
        paths: [
          '/myPartitionKey'
        ]
        kind: 'Hash'
      }
      indexingPolicy: {
        indexingMode: 'consistent'
        includedPaths: [
          {
            path: '/*'
          }
        ]
        excludedPaths: [
          {
            path: '/myPathToNotIndex/*'
          }
          {
            path: '/_etag/?'
          }
        ]
        compositeIndexes: [
          [
            {
              path: '/name'
              order: 'ascending'
            }
            {
              path: '/age'
              order: 'descending'
            }
          ]
        ]
        spatialIndexes: [
          {
            path: '/path/to/geojson/property/?'
            types: [
              'Point'
              'Polygon'
              'MultiPolygon'
              'LineString'
            ]
          }
        ]
      }
      defaultTtl: 86400
      uniqueKeyPolicy: {
        uniqueKeys: [
          {
            paths: [
              '/phoneNumber'
            ]
          }
        ]
      }
    }
    options: {
      autoscaleSettings: {
        maxThroughput: autoscaleMaxThroughput
      }
    }
  }
}
