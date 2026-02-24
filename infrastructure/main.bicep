// Azure Bicep template for Pet Food Analyzer infrastructure

@description('Location for all resources')
param location string = resourceGroup().location

@description('Base name for resources')
param baseName string = 'petfoodanalyzer'

@description('Environment name')
@allowed(['dev', 'staging', 'prod'])
param environment string = 'dev'

var uniqueSuffix = uniqueString(resourceGroup().id)
var appServicePlanName = '${baseName}-plan-${environment}'
var backendAppName = '${baseName}-api-${environment}-${uniqueSuffix}'
var frontendAppName = '${baseName}-web-${environment}-${uniqueSuffix}'

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
  properties: {
    reserved: false
  }
}

// Backend API App Service
resource backendApp 'Microsoft.Web/sites@2023-01-01' = {
  name: backendAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      netFrameworkVersion: 'v8.0'
    }
  }
}

// Frontend Static Web App
resource frontendApp 'Microsoft.Web/staticSites@2023-01-01' = {
  name: frontendAppName
  location: location
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {}
}

output backendUrl string = 'https://${backendApp.properties.defaultHostName}'
output frontendUrl string = 'https://${frontendApp.properties.defaultHostName}'
