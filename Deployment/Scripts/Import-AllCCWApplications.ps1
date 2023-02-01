Write-Host "Removing existing Az module"
Uninstall-Module Az -AllVersions -Force 

Write-Host "Installing Az v6.6.0 due to Az.Account version in image"
Install-Module Az -Repository PSGallery -RequiredVersion 6.6.0 -AllowClobber -Force

# Write-Host "Installing Az"
# Install-Module Az -Repository PSGallery -AllowClobber -Force

Write-Host "Importing Az"
Import-Module Az -Force

apt-get update
apt-get --yes upgrade
apt install curl
curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# $env:CSSA_SP_APP_ID="12341234-1234-1234-1234-123412341234"
# $env:CSSA_SP_SECRET="*****************************"
# $env:CSSA_TENANT_ID="12341234-1234-1234-1234-123412341234"
# $env:APP_SUBSCRIPTION_ID="12341234-1234-1234-1234-123412341234"
# $env:APP_RESOURCE_GROUP_NAME="some-resource-group"

## UI config.json settings 
# $env:AUTH_SP_APP_ID="12341234-1234-1234-1234-123412341234"
# $env:AUTH_TENANT_ID="12341234-1234-1234-1234-123412341234"
# $env:AUTH_AUTHORITY="https://login.microsoftonline.com/$env:AUTH_TENANT_ID"
# $env:AUTH_PRIMARY_DOMAIN="somedomain.gov"
# $env:DEFAULT_COUNTY="Some County"

# $env:AGENCY_ABBREVIATION="sdsd"
# $env:ENVIRONMENT_TYPE="PROD"
# $env:ENABLE_BEATS="false"
# $env:ENABLE_STOP_DEBUGGER="false"
# $env:DEPLOY_WEB_CONFIG_JSON="false"

Write-Host "CSSA_SP_APP_ID: $env:CSSA_SP_APP_ID"
Write-Host "CSSA_TENANT_ID: $env:CSSA_TENANT_ID"
Write-Host "APP_RESOURCE_GROUP_NAME: $env:APP_RESOURCE_GROUP_NAME"
Write-Host "DEPLOY_WEB_CONFIG_JSON: $env:DEPLOY_WEB_CONFIG_JSON"

Write-Host "logging into azure powershell"
[string]$username = $env:CSSA_SP_APP_ID
[string]$userpassword = $env:CSSA_SP_SECRET
[securestring]$secstringpassword = convertto-securestring $userpassword -asplaintext -force
[pscredential]$credobject = new-object system.management.automation.pscredential ($username, $secstringpassword)
Connect-AzAccount -environment azureusgovernment -Tenant $env:CSSA_TENANT_ID -Subscription $env:APP_SUBSCRIPTION_ID -ServicePrincipal -Credential $credobject

Write-Host "checking login context"
Get-AzContext

Write-Host "logging into azure cli"
az cloud set -n azureusgovernment 
az login --service-principal --tenant $env:CSSA_TENANT_ID -u $env:CSSA_SP_APP_ID -p $env:CSSA_SP_SECRET
az account set -s $env:APP_SUBSCRIPTION_ID

Write-Host "checking cli login context"
az account show

$webappNames = (az webapp list -g $env:APP_RESOURCE_GROUP_NAME --query "[].{Name:name}" -o json) | ConvertFrom-Json
$webappNames = $webappNames | Sort-Object -Property Name

foreach ($webappName in $webappNames) {
    
    $webappNameParts = $webappName.Name.Split("-")
    $appName = $webappNameParts[$webappNameParts.Length - 2]

    Write-Host "Deploying & Importing API: $appName"

    Write-Host "Getting API Version info:" $webappName.Name

    $APIVersion = ((Get-AzResourceProvider -ProviderNamespace Microsoft.Web).ResourceTypes | Where-Object ResourceTypeName -eq sites).ApiVersions[0]
    Write-Host "APIVersion: $APIVersion"

    Write-Host "Getting existing IP restrictions"
    $WebAppConfig = (Get-AzResource -ResourceType "Microsoft.Web/sites/config" -ResourceGroupName $env:APP_RESOURCE_GROUP_NAME -ApiVersion $APIVersion -Name $webappName.Name)
    $existingIpSecurityRestrictions = $WebAppConfig.Properties.ipSecurityRestrictions
    Write-Host "Existing Ip restrictions"
    Write-Host $existingIpSecurityRestrictions
    Write-Host

    Write-Host "Removing existing IP restriction"
    $WebAppConfig.properties.ipSecurityRestrictions = @()
    $WebAppConfig | Set-AzResource -ApiVersion $APIVersion -Force | Out-Null

    Write-Host "Deploying function:" $webappName.Name
    $fileName = (Get-ChildItem -Filter "*$appName-api.zip").Name
    Write-Host "Deploying package:" $fileName
    az webapp deployment source config-zip -g $env:APP_RESOURCE_GROUP_NAME --src "./$fileName" -n $webappName.Name

    Write-Host "Resetting IP restriction"
    $WebAppConfig = (Get-AzResource -ResourceType Microsoft.Web/sites/config -ResourceGroupName $env:APP_RESOURCE_GROUP_NAME -ApiVersion $APIVersion -Name $webappName.Name)
    $WebAppConfig.properties.ipSecurityRestrictions = $existingIpSecurityRestrictions
    $WebAppConfig | Set-AzResource -ApiVersion $APIVersion -Force | Out-Null
}

Write-Host "Publishing Admin UI package"
$uiStorageAccountName = ((az storage account list -g $env:APP_RESOURCE_GROUP_NAME --query "[? contains(name, 'aui')].{Name:name}" -o json) | ConvertFrom-Json).Name
Write-Host "Publishing to:" $uiStorageAccountName

$fileName = (Get-ChildItem -Path "./" -Filter "admin.zip").Name
Write-Host "Deploying Admin UI package:" $fileName
Expand-Archive -Path "./$fileName" -DestinationPath "./" -Force
az storage blob upload-batch --overwrite true --timeout 300 -d '$web' --account-name $uiStorageAccountName -s './dist'

if("True" -eq $env:DEPLOY_WEB_CONFIG_JSON)
{
    Write-Host "Creating config.json"
    
    Write-Host "AUTH_SP_APP_ID: $env:ADMIN_AUTH_SP_APP_ID"
    Write-Host "AUTH_TENANT_ID: $env:ADMIN_AUTH_TENANT_ID"
    Write-Host "AUTH_AUTHORITY: $env:ADMIN_AUTH_AUTHORITY"
    Write-Host "AUTH_PRIMARY_DOMAIN: $env:ADMIN_AUTH_PRIMARY_DOMAIN"
    Write-Host "DEFAULT_COUNTY: $env:DEFAULT_COUNTY"
    
    Write-Host "AGENCY_ABBREVIATION: $env:AGENCY_ABBREVIATION"
    Write-Host "ENVIRONMENT_TYPE: $env:ENVIRONMENT_TYPE"
    Write-Host "ENABLE_BEATS: $env:ENABLE_BEATS"
    Write-Host "MODIFY_BEAT_ID: $env:MODIFY_BEAT_ID"
    Write-Host "BEAT_ID_NUMBER_OF_DIGITS: $env:BEAT_ID_NUMBER_OF_DIGITS"
    Write-Host "DISPLAY_REPORTING_EMAIL": $env:DISPLAY_REPORTING_EMAIL
    Write-Host "REPORTING_EMAIL_ADDRESS": $env:REPORTING_EMAIL_ADDRESS
    Write-Host "ENABLE_STOP_DEBUGGER: $env:ENABLE_STOP_DEBUGGER"

    $configFilePath = "./$env:TEMPLATE_VERSION_FORMATTED-admin-config.json"
    $configJson = Get-Content -Path $configFilePath
    $configJson = $configJson.Replace("__ENVIRONMENT_TYPE__", $env:ENVIRONMENT_TYPE)
    $configJson = $configJson.Replace("__AUTH_SP_APP_ID__", $env:ADMIN_AUTH_SP_APP_ID)
    $configJson = $configJson.Replace("__AUTH_AUTHORITY__", $env:ADMIN_AUTH_AUTHORITY)
    $configJson = $configJson.Replace("__AUTH_TENANT_ID__", $env:ADMIN_AUTH_TENANT_ID)
    $configJson = $configJson.Replace("__AUTH_PRIMARY_DOMAIN__", $env:ADMIN_AUTH_PRIMARY_DOMAIN)
    $configJson = $configJson.Replace("__DEFAULT_COUNTY__", $env:DEFAULT_COUNTY)
    $configJson = $configJson.Replace("__ENABLE_STOP_DEBUGGER__", $env:ENABLE_STOP_DEBUGGER)

    Write-Host "Saving config.json"
    Set-Content -Path $configFilePath -Value $configJson -Force

    Write-Host "Uploading config.json"
    az storage blob upload --overwrite true --timeout 300 --account-name $uiStorageAccountName -n "config.json" -c '$web' -f "./$env:TEMPLATE_VERSION_FORMATTED-admin-config.json" 
}

Write-Host "Publishing Public UI package"
$uiStorageAccountName = ((az storage account list -g $env:APP_RESOURCE_GROUP_NAME --query "[? contains(name, 'pui')].{Name:name}" -o json) | ConvertFrom-Json).Name
Write-Host "Publishing to:" $uiStorageAccountName

$fileName = (Get-ChildItem -Path "./" -Filter "public.zip").Name
Write-Host "Deploying Public UI package:" $fileName
Expand-Archive -Path "./$fileName" -DestinationPath "./" -Force
az storage blob upload-batch --overwrite true --timeout 300 -d '$web' --account-name $uiStorageAccountName -s './dist'

if("True" -eq $env:DEPLOY_WEB_CONFIG_JSON)
{
    Write-Host "Creating config.json"
    
    Write-Host "AUTH_SP_APP_ID: $env:PUBLIC_AUTH_SP_APP_ID"
    Write-Host "AUTH_TENANT_ID: $env:PUBLIC_AUTH_TENANT_ID"
    Write-Host "AUTH_AUTHORITY: $env:PUBLIC_AUTH_AUTHORITY"
    Write-Host "AUTH_PRIMARY_DOMAIN: $env:PUBLIC_AUTH_PRIMARY_DOMAIN"
    Write-Host "DEFAULT_COUNTY: $env:DEFAULT_COUNTY"
    
    Write-Host "AGENCY_ABBREVIATION: $env:AGENCY_ABBREVIATION"
    Write-Host "ENVIRONMENT_TYPE: $env:ENVIRONMENT_TYPE"
    Write-Host "ENABLE_BEATS: $env:ENABLE_BEATS"
    Write-Host "MODIFY_BEAT_ID: $env:MODIFY_BEAT_ID"
    Write-Host "BEAT_ID_NUMBER_OF_DIGITS: $env:BEAT_ID_NUMBER_OF_DIGITS"
    Write-Host "DISPLAY_REPORTING_EMAIL": $env:DISPLAY_REPORTING_EMAIL
    Write-Host "REPORTING_EMAIL_ADDRESS": $env:REPORTING_EMAIL_ADDRESS
    Write-Host "ENABLE_STOP_DEBUGGER: $env:ENABLE_STOP_DEBUGGER"

    $configFilePath = "./$env:TEMPLATE_VERSION_FORMATTED-public-config.json"
    $configJson = Get-Content -Path $configFilePath
    $configJson = $configJson.Replace("__ENVIRONMENT_TYPE__", $env:ENVIRONMENT_TYPE)
    $configJson = $configJson.Replace("__AUTH_SP_APP_ID__", $env:PUBLIC_AUTH_SP_APP_ID)
    $configJson = $configJson.Replace("__AUTH_AUTHORITY__", $env:PUBLIC_AUTH_AUTHORITY)
    $configJson = $configJson.Replace("__AUTH_TENANT_ID__", $env:PUBLIC_AUTH_TENANT_ID)
    $configJson = $configJson.Replace("__AUTH_PRIMARY_DOMAIN__", $env:PUBLIC_AUTH_PRIMARY_DOMAIN)
    $configJson = $configJson.Replace("__DEFAULT_COUNTY__", $env:DEFAULT_COUNTY)
    $configJson = $configJson.Replace("__ENABLE_STOP_DEBUGGER__", $env:ENABLE_STOP_DEBUGGER)

    Write-Host "Saving config.json"
    Set-Content -Path $configFilePath -Value $configJson -Force

    Write-Host "Uploading config.json"
    az storage blob upload --overwrite true --timeout 300 --account-name $uiStorageAccountName -n "config.json" -c '$web' -f "./$env:TEMPLATE_VERSION_FORMATTED-public-config.json" 
}

Write-Host "Finished deploying & importing applications"