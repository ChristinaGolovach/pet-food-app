// Azure Bicep template for Pet Food Analyzer infrastructure

@description('Location for all resources')
param location string = resourceGroup().location

@description('Location for Static Web App (limited region availability)')
param staticWebAppLocation string = 'eastus2'

@description('Location for OpenAI (not available in all regions)')
param openAiLocation string = 'swedencentral'

@description('Base name for resources')
param baseName string = 'pfa'

@description('Environment name')
@allowed(['dev', 'staging', 'prod'])
param environment string = 'dev'

var uniqueSuffix = uniqueString(resourceGroup().id)
var appServicePlanName = '${baseName}-plan-${environment}'
var backendAppName = '${baseName}-api-${environment}-${uniqueSuffix}'
var frontendAppName = '${baseName}-web-${environment}-${uniqueSuffix}'
var keyVaultName = '${baseName}-kv-${uniqueSuffix}'
var visionName = '${baseName}-vision-${environment}'
var openAiName = '${baseName}-oai-${environment}'


// ============================================
// Azure Key Vault
// ============================================
resource keyVault 'Microsoft.KeyVault/vaults@2025-05-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 7
  }
}

// ============================================
// Azure AI Vision (Free F0)
// ============================================
resource computerVision 'Microsoft.CognitiveServices/accounts@2025-09-01' = {
  name: visionName
  location: location
  kind: 'ComputerVision'
  sku: {
    name: 'F0'
  }
  properties: {
    publicNetworkAccess: 'Enabled'
  }
}

// ============================================
// Azure OpenAI (S0 — pay per use)
// ============================================
resource openAi 'Microsoft.CognitiveServices/accounts@2025-09-01' = {
  name: openAiName
  location: openAiLocation
  kind: 'OpenAI'
  sku: {
    name: 'S0'
  }
  properties: {
    publicNetworkAccess: 'Enabled'
  }
}

// ============================================
// GPT-4o-mini deployment (cheapest model)
// ============================================
resource gptDeployment 'Microsoft.CognitiveServices/accounts/deployments@2025-09-01' = {
  parent: openAi
  name: 'gpt-4o-mini'
  sku: {
    name: 'Standard'
    capacity: 1
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: 'gpt-4o-mini'
      version: '2024-07-18'
    }
  }
}

// ============================================
// App Service Plan (Free F1)
// ============================================
resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'F1'
    tier: 'Free'
  }
  properties: {
    reserved: false
  }
}

// ============================================
// Backend API App Service
// ============================================
resource backendApp 'Microsoft.Web/sites@2023-01-01' = {
  name: backendAppName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      netFrameworkVersion: 'v8.0'
      appSettings: [
        {
          name: 'KeyVaultUri'
          value: keyVault.properties.vaultUri
        }
        {
          name: 'AzureVision__Endpoint'
          value: computerVision.properties.endpoint
        }
        {
          name: 'AzureOpenAI__Endpoint'
          value: openAi.properties.endpoint
        }
        {
          name: 'AzureOpenAI__DeploymentName'
          value: gptDeployment.name
        }
      ]
    }
  }
}

// ============================================
// Frontend Static Web App (Free)
// ============================================
resource frontendApp 'Microsoft.Web/staticSites@2023-12-01' = {
  name: frontendAppName
  location: staticWebAppLocation
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {}
}


// ============================================
// RBAC: Give backend app access to Key Vault secrets
// ============================================
@description('Key Vault Secrets User role')
var keyVaultSecretsUserRoleId = '4633458b-17de-408a-b874-0445c86b69e6'

resource backendKeyVaultAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, backendApp.id, keyVaultSecretsUserRoleId)
  scope: keyVault
  properties: {
    principalId: backendApp.identity.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', keyVaultSecretsUserRoleId)
    principalType: 'ServicePrincipal'
  }
}

// ============================================
// Store secrets in Key Vault
// ============================================
resource visionKeySecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'AzureVision--Key'
  properties: {
    value: computerVision.listKeys().key1
  }
}

resource openAiKeySecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'AzureOpenAI--Key'
  properties: {
    value: openAi.listKeys().key1
  }
}

// ============================================
// Outputs
// ============================================
output backendUrl string = '<https://$>{backendApp.properties.defaultHostName}'
output frontendUrl string = '<https://$>{frontendApp.properties.defaultHostName}'
output keyVaultUri string = keyVault.properties.vaultUri
output visionEndpoint string = computerVision.properties.endpoint
output openAiEndpoint string = openAi.properties.endpoint
