@description('Name of the resource group to deploy into.')
param location string = resourceGroup().location

@description('Base name for the Azure resources.')
param baseName string = 'reactdotnetsample'

resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: '${baseName}-plan'
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
}

resource apiApp 'Microsoft.Web/sites@2023-01-01' = {
  name: '${baseName}-api'
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      appSettings: []
    }
  }
}

resource frontendApp 'Microsoft.Web/sites@2023-01-01' = {
  name: '${baseName}-frontend'
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'NODE|20-lts'
      appSettings: []
    }
  }
}

output apiUrl string = 'https://${apiApp.properties.defaultHostName}'
output frontendUrl string = 'https://${frontendApp.properties.defaultHostName}'
