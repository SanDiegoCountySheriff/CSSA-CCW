################################################## License ##################################################
#
# This script and associated files where copied from Martin Simecek's GitHug repository below.
# Use of this code is subject to his licensing agreement. 
# 
# https://codez.deedx.cz/posts/automating-azure-ad-b2c-creation-with-powershell/
# https://github.com/msimecek/AAD-automations/
#
#############################################################################################################

# Windows PowerShell and PowerShell Core are supported.
# - Microsoft.Graph PowerShell module needs to be installed.
# - Azure CLI needs to be installed and authenticated for the owning tenant.

Import-Module .\Import-Users.psm1 -force

function Get-AzureUrl {
  param(
    [string] $urlKey
  )

  if ($null -eq $azureUrls) {
    $azureUrls = (az cloud show --query endpoints) | ConvertFrom-Json 
  }

  $fullUrl = $azureUrls.$urlKey
  if ($fullUrl.EndsWith('/')) {
    $ComputerName = $ComputerName
    $fullUrl = $fullUrl.Substring(0, $fullUrl.Length - 1)
  }

  return $fullUrl
}
function Get-AzureDomainSuffix {
  
  if ($null -eq $azureCloudName) {
    $azureCloudName = (az cloud show --query name) | ConvertFrom-Json 
  }

  if ($azureCloudName -eq "AzureCloud") {
    return "com"
  }
  else {
    return "us"
  }
}

# Main entry point for this script. ALthough each function can be used in isolation
# This method will create an Azure B2C Tenant, User workflow, and App Registration
# for SPA logins & authorization
#
# Create new Azure AD B2C tenant in a specific subscription and resource group.
# 
# Required: Azure CLI authenticated for the target subscription.
# Required: Resource provider: "Microsoft.AzureActiveDirectory". The function will attempt to register if not done so yet.
#   az provider register --namespace Microsoft.AzureActiveDirectory
#
function New-B2CTenant {
  [CmdletBinding()]
  param (
    [Parameter(Mandatory = $true, HelpMessage = "B2C tenant name, without the '.onmicrosoft.com' or '.onmicrosoft.us'.")]
    [string] $B2CTenantName,
      
    [Parameter(Mandatory = $true, HelpMessage = "Name of the Azure Resource Group to put the B2C resource into. Will be created if it does not exist.")]
    [string] $ResourceGroupName
  )

  if (Get-Module -ListAvailable -Name Microsoft.Graph.Applications) { }
  else {
    Install-Module Microsoft.Graph.Applications -AllowClobber -Force
  }
  
  Import-Module Microsoft.Graph.Applications -Force

  # Must identify if the user is using Public Azure or Government Azure
  $cloudName = (az cloud show --query name -o tsv)
  if ($cloudName -eq "AzureCloud") {
    $msRootDomainSuffix = "com"
  }
  else {
    $msRootDomainSuffix = "us"
  }

  # Create the B2C tenant resource in Azure
  New-AzureADB2CTenant `
    -B2CTenantName $B2CTenantName `
    -AzureResourceGroup $ResourceGroupName `
    -RootDomainSuffix $msRootDomainSuffix
  
  # Call the init API
  Invoke-TenantInit `
    -B2CTenantName $B2CTenantName `
    -RootDomainSuffix $msRootDomainSuffix
  
  Write-Host "Waiting for 30 seconds for previous command to propagate..."
  Start-Sleep -Seconds 30

  # Interactive login, so that we don't have to create a separate service principal and handle secrets.
  # Make sure that the user has administrative permissions in the tenant.
  Write-Host "Interactive login to the Graph API. Watch for a newly opened browser window (or device flow instructions) and complete the sign in."
  if ($cloudName -eq "AzureCloud") {
    $graphEnvironment = "Global"
  }
  else {
    $graphEnvironment = "USGov"
  }
  $b2cTenantId = "$($B2CTenantName).onmicrosoft.$(Get-AzureDomainSuffix)"
  Connect-MgGraph -Environment $graphEnvironment -TenantId $b2cTenantId -Scopes "User.ReadWrite.All", "Application.ReadWrite.All", "Directory.AccessAsUser.All", "Directory.ReadWrite.All"
  
  # Add custom attribute called "PhoneNumber"
  Add-CustomAttribute `
    -B2CTenantName $B2CTenantName `
    -AttributeName "PhoneNumber" `
    -Description "Primary phone number" `
    -RootDomainSuffix $msRootDomainSuffix
  
  # Add user signbup/signin user flow
  Add-UserFlow `
    -B2CTenantName $B2CTenantName `
    -DefinitionFilePath ./signup-signin-userflow.json `
    -RootDomainSuffix $msRootDomainSuffix
  
  # Create an Authentication Application for the UI
  $appRegistration = New-B2CAuthenticationApp `
    -B2CTenantName $B2CTenantName `
    -RootDomainSuffix $msRootDomainSuffix
  
  return @{
    UIAppClientID = $appRegistration.ClientID
  }
}
  
# Create new Azure AD B2C tenant in a specific subscription and resource group.
# Must be followed by Invoke-TenantInit to finalize the creation of default apps.
#
# Required: Azure CLI authenticated for the target subscription.
# Required: Resource provider: "Microsoft.AzureActiveDirectory". The function will attempt to register if not done so yet.
#   az provider register --namespace Microsoft.AzureActiveDirectory
#
# Azure PowerShell Alternative: Invoke-AzRestMethod
function New-AzureADB2CTenant {
  param(
    # Tenant name without the '.onmicrosoft.com' part.
    [string] $B2CTenantName,
  
    [Parameter()]
  
    # Under which Azure subscription will this B2C tenant reside. If not provided, use the current subscription from Azure CLI.
    [string] $AzureSubscriptionId = $null,
  
    # Under which Azure resource group will this B2C tenant reside.
    [string] $AzureResourceGroup,

    # Base domain suffix for all MS URLs. I.e. .com or .us
    [string] $RootDomainSuffix
  )
  
  if (!$AzureSubscriptionId) {
    Write-Host "Getting subscription ID from the current account..."
    $AzureSubscriptionId = $(az account show --query "id" -o tsv)
    Write-Host $AzureSubscriptionId
  }
  
  $aadProviderRegState = $(az provider show -n Microsoft.AzureActiveDirectory --query "registrationState" -o tsv)
  if ($aadProviderRegState -ne "Registered") {
    Write-Host "Resource Provider 'Microsoft.AzureActiveDirectory' not registered yet. Registering now..."
    az provider register --namespace Microsoft.AzureActiveDirectory
  
    while ($(az provider show -n Microsoft.AzureActiveDirectory --query "registrationState" -o tsv) -ne "Registered") {
      Write-Host "Resource Provider registration not yet finished. Waiting..."
      Start-Sleep -Seconds 10
    }
    Write-Host "Resource Provider registration finished."
  }
  
  Write-Host
  Write-Host "***************************** Ignore any onscreen errors *****************************" -ForegroundColor Red
  Write-Host "Checking if Resource Group $AzureResourceGroup exists..."
  $checkRg = (az group exists --name $AzureResourceGroup) | ConvertFrom-Json
  
  if ($LastExitCode -ne 0) {
    throw "Error on using Azure CLI. Make sure the CLI is installed, up-to-date and you are signed in. Run 'az login' to sign in."
  }
  
  if (!$checkRg) {
    Write-Warning "Resource Group $AzureResourceGroup does not exist."
    throw "The Resource Group provided does not exists. Please create the resource group before running thi script."
  }
  
  $resourceId = "/subscriptions/$AzureSubscriptionId/resourceGroups/$AzureResourceGroup/providers/Microsoft.AzureActiveDirectory/b2cDirectories/$B2CTenantName.onmicrosoft.$(Get-AzureDomainSuffix)"
  Write-Host "Using resource id:" $resourceId

  Write-Host
  Write-Host "***************************** Ignore any onscreen errors *****************************" -ForegroundColor Red
  Write-Host "Checking if tenant '$B2CTenantName' already exists..."
  az resource show --id $resourceId | Out-Null
  if ($LastExitCode -eq 0) {
    # No error means, the resource exists
    Write-Warning "Tenant '$B2CTenantName' already exists. Not attempting to recreate it."
    return
  }
  
  $reqBody = @"
    {
      "location":"Global",
      "sku": {
          "name":"Standard",
          "tier":"A0"
      },
      "properties": {
          "createTenantProperties": {
              "isGoLocalTenant":false,
              "displayName":"$($B2CTenantName)",
              "countryCode":"US"
          }
      }
    }
"@
  
  # Flatten the JSON to make Azure CLI happy, otherwise it complains about incorrect content type.
  $reqBody = $reqBody.Replace("`n", "").Replace("`"", "\`"")
  
  Write-Host
  Write-Host "Creating B2C tenant $B2CTenantName..."
  $managementUrl = "$(Get-AzureUrl -urlKey 'resourceManager')$($resourceId)?api-version=2021-04-01"
  $restResult = (az rest --method PUT --url $managementUrl --body $reqBody)
  
  if ($LastExitCode -ne 0) {
    throw "Error on creating new B2C tenant!"
  }
  
  Write-Host "*** B2C Tenant creation started. It can take a moment to complete."
  
  do {
    Write-Host "Waiting for 10 seconds for B2C tenant creation..."
    Start-Sleep -Seconds 10
  
    az resource show --id $resourceId
  }
  while ($LastExitCode -ne 0)
}

# Finalize initialization of newly created B2C tenant.
# This function needs to be called once the tenant is created and before any other steps, because it creates the b2c-extensions-app.
#
# Required: Azure CLI authenticated with owner permissions for the tenant.
function Invoke-TenantInit {
  param (
    [string] $B2CTenantName,

    # Base domain suffix for all MS URLs. I.e. .com or .us
    [string] $RootDomainSuffix
  )
  
  $B2CTenantId = "$($B2CTenantName).onmicrosoft.$(Get-AzureDomainSuffix)"
  
  # Get access token for the B2C tenant with audience "management.core.windows.net".
  $managementAccessToken = $(az account get-access-token --tenant "$B2CTenantId" --query accessToken -o tsv)
  
  # Invoke tenant initialization which happens through the portal automatically.
  # Ref: https://stackoverflow.com/questions/67706798/creation-of-the-b2c-extensions-app-by-script
  $reqUrl = "https://main.b2cadmin.ext.azure.$(Get-AzureDomainSuffix)/api/tenants/GetAndInitializeTenantPolicy?tenantId=$($B2CTenantId)&skipInitialization=false"
  Write-Host $reqUrl
  
  do {  
    try {
      Write-Host "Invoking tenant initialization..."

      $response = Invoke-WebRequest -Uri $reqUrl `
        -Method "GET" `
        -Headers @{
        "Authorization" = "Bearer $($managementAccessToken)"
      }

      $StatusCode = $response.StatusCode
    } 
    catch {
      $StatusCode = $_.Exception.Response.StatusCode.value__
      Write-Host "Error initializing the B2C Tenant..." -ForegroundColor Red
      Write-Host "Waiting for 10 seconds for B2C tenant initialization..." -ForegroundColor Red
      Start-Sleep -Seconds 10
    }
  }
  while ($StatusCode -gt 399)
}

# Create a custom user attribute in the tenant.
#
# Requires: Azure CLI authenticated with owner permissions for the tenant.
# Alternatively, the /beta/identity/userFlowAttributes Graph endpoint can be used.
function Add-CustomAttribute {
  param (
    [string] $B2CTenantName,
      
    [string] $AttributeName,
    [string] $Description,
    [string] $DataType = 1,

    # Base domain suffix for all MS URLs. I.e. .com or .us
    [string] $RootDomainSuffix
  )

  $B2CTenantId = "$($B2CTenantName).onmicrosoft.$(Get-AzureDomainSuffix)"
  
  $reqBody = @"
  {
    "dataType": $($DataType),
    "label": "$($AttributeName)",
    "adminHelpText": "$($Description)",
    "userInputType": 1,
    "userAttributeOptions": [],
    "attributeType": 3
  }
"@

  Write-Host $reqBody
  
  Write-Host "Creating custom attribute $($AttributeName)..."
  $reqUrl = "https://main.b2cadmin.ext.azure.$(Get-AzureDomainSuffix)/api/userAttribute?tenantId=$($B2CTenantId)&`$orderby=DisplayName"

  Write-Host $reqUrl

  # Get access token for the B2C tenant with audience "management.core.windows.net".
  $managementAccessToken = $(az account get-access-token --tenant $B2CTenantId --query accessToken -o tsv)

  Invoke-WebRequest -Uri $reqUrl `
    -Method "POST" `
    -Headers @{
    "Authorization" = "Bearer $($managementAccessToken)";
    "Content-Type"  = "application/json"
  } `
    -Body $reqBody
}

# Create user flow based on JSON definition from a file.
#
# Requires: Azure CLI authenticated with owner permissions for the tenant.
function Add-UserFlow {
  param(
    [string] $B2CTenantName,
    [string] $DefinitionFilePath,

    # Base domain suffix for all MS URLs. I.e. .com or .us
    [string] $RootDomainSuffix
  )
  
  $B2CTenantId = "$($B2CTenantName).onmicrosoft.$(Get-AzureDomainSuffix)"
  
  # Get access token for the B2C tenant with audience "management.core.windows.net".
  $managementAccessToken = $(az account get-access-token --tenant $B2CTenantId --query accessToken -o tsv)
  
  Write-Host "Creating $($DefinitionFilePath) user flow..."
  $signinFlowContent = Get-Content $DefinitionFilePath

  # Using WebRequest here, because Microsoft Graph is currently not able to create user flows with custom attributes.
  Invoke-WebRequest -Uri "https://main.b2cadmin.ext.azure.$(Get-AzureDomainSuffix)/api/adminuserjourneys?tenantId=$($B2CTenantId)" `
    -Method "POST" `
    -Headers @{
    "Authorization" = "Bearer $($managementAccessToken)";
    "Content-Type"  = "application/json"
  } `
    -Body $signinFlowContent
}

# Creates a B2C application registration to be used for users to sign-in.
#
# This includes custom scope called user_impersonation.
#
function New-B2CAuthenticationApp {
  param (
    [string] $B2CTenantName,

    # Base domain suffix for all MS URLs. I.e. .com or .us
    [string] $RootDomainSuffix
  )
  
  # Create the user_impersonation permission scope
  $impersonateAccessScope = New-Object -TypeName Microsoft.Graph.PowerShell.Models.MicrosoftGraphPermissionScope
  $impersonateAccessScope.AdminConsentDescription = "Allows the app to access API data on behalf of a user."
  $impersonateAccessScope.AdminConsentDisplayName = "User Impersonation"
  $impersonateAccessScope.Id = New-Guid
  $impersonateAccessScope.IsEnabled = $true
  $impersonateAccessScope.Type = "Admin"
  $impersonateAccessScope.Value = "user_impersonation"
  
  # Create the UI application
  $appRegistration = New-Object -TypeName Microsoft.Graph.PowerShell.Models.MicrosoftGraphApplication
  $appRegistration.DisplayName = "CCW Public Application"
  $appRegistration.SignInAudience = "AzureADandPersonalMicrosoftAccount"
  $appRegistration.Spa.RedirectUris = "http://localhost:4000"
  $appRegistration.Web.ImplicitGrantSettings.EnableAccessTokenIssuance = $true
  $appRegistration.Web.ImplicitGrantSettings.EnableIdTokenIssuance = $true
  $appRegistration.IsFallbackPublicClient = $false
  $appRegistration.Api.Oauth2PermissionScopes = $impersonateAccessScope
  
  Write-Host "Creating B2C Authentication application..."
  $appRegistration = New-MgApplication -BodyParameter $appRegistration
  Write-Host "Successfully created UI app with applicationId $($appRegistration.AppId)"

  # Well-known ID for offline_access = 7427e0e9-2fba-42fe-b0c0-848c9e6a8182
  $offlineAccessScope = @{ Id = "7427e0e9-2fba-42fe-b0c0-848c9e6a8182"; Type = "Scope" }
  
  # Well-known ID for openid = 37f7f235-527c-4136-accd-4a02d197296e
  $openidScope = @{ Id = "37f7f235-527c-4136-accd-4a02d197296e"; Type = "Scope" }
  
  # Well-known ID for User.Read.All = df021288-bdef-4463-88db-98f22de89214
  $graphReadallRole = @{ Id = "df021288-bdef-4463-88db-98f22de89214"; Type = "Role" }
  
  # Adding user_impersonation API permission
  $impersonationRRA = New-Object -TypeName Microsoft.Graph.PowerShell.Models.MicrosoftGraphRequiredResourceAccess
  $impersonationRRA.ResourceAccess = @{ Id = $impersonateAccessScope.Id; Type = "Scope" }
  $impersonationRRA.ResourceAppId = $appRegistration.AppId
  
  $msGraphGuid = "00000003-0000-0000-c000-000000000000" # Well-known MS Graph ID
  $graphRRA = New-Object -TypeName Microsoft.Graph.PowerShell.Models.MicrosoftGraphRequiredResourceAccess
  $graphRRA.ResourceAccess = @($offlineAccessScope, $openidScope, $graphReadallRole)
  $graphRRA.ResourceAppId = $msGraphGuid 
  
  $resourceAccessList = @(
    $impersonationRRA
    $graphRRA
  )
  
  $b2cFQDN = "$($B2CTenantName).onmicrosoft.$(Get-AzureDomainSuffix)"

  Write-Host "Assigning user_impersonation and openid permissions for the UI application..."
  Update-MgApplication `
    -ApplicationId $appRegistration.Id `
    -RequiredResourceAccess $resourceAccessList `
    -IdentifierUris "https://$($b2cFQDN)/$($appRegistration.AppId)"
  
  New-MgServicePrincipal -AppId $appRegistration.AppId

  $suffix = Get-AzureDomainSuffix
  if ($suffix.Contains("com")) {

    Write-Host "Logging into Azure CLI to 'Grant Admin Concent' to the required permissions"
    az login --tenant $b2cFQDN --allow-no-subscriptions
    
    Write-Host "Waiting 30 seconds for App Registration & Enterise App to propagate"
    Start-Sleep -Seconds 30
    
    az ad app permission admin-consent --id $appRegistration.AppId
  }
  else {
    Write-Host
    Write-Host "*******************************************************************************************" -ForegroundColor Red
    Write-Host "                                                                                           " -ForegroundColor Red
    Write-Host "WARNING: You are installing to Azure US Government. You must manually 'Grant Admin Concent'" -ForegroundColor Red
    Write-Host "                                                                                           " -ForegroundColor Red
    Write-Host "*******************************************************************************************" -ForegroundColor Red
  }

  Write-Host
  Write-Host "*** Azure AD B2C Application '$($appRegistration.DisplayName)' created."
  Write-Host "*** Client ID: $($appRegistration.AppId)"
  Write-Host
  
  return @{
    ClientID = $appRegistration.AppId
  }
}

Export-ModuleMember New-B2CTenant 
Export-ModuleMember New-AzureADB2CTenant
Export-ModuleMember Invoke-TenantInit
Export-ModuleMember Add-CustomAttribute
Export-ModuleMember New-B2CAuthenticationApp
Export-ModuleMember Add-UserFlow