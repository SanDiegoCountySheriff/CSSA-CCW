Write-Host "Removing existing Az module"
Uninstall-Module Az -AllVersions -Force 

Write-Host "Installing Az v6.6.0 due to Az.Account version in image"
Install-Module Az -Repository PSGallery -RequiredVersion 6.6.0 -AllowClobber -Force

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

# # UI config.json settings 
# $env:AUTH_SP_APP_ID="12341234-1234-1234-1234-123412341234"
# $env:AUTH_TENANT_ID="12341234-1234-1234-1234-123412341234"
# $env:AUTH_AUTHORITY="https://login.microsoftonline.com/$env:AUTH_TENANT_ID"
# $env:AUTH_PRIMARY_DOMAIN="somedomain.gov"
# $env:DEFAULT_COUNTY="Some County"

# $env:AGENCY_ABBREVIATION="sdsd"
# $env:ENVIRONMENT="PROD"
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

    Write-Host "Deploying API: $appName"

    Write-Host "API Web App:" $webappName.Name

    Write-Host "Enabling SCM Access Restrictions"
    $accessRestriction = (az webapp update -g $env:APP_RESOURCE_GROUP_NAME -n $webappName.Name  --set publicNetworkAccess=Enabled)

    Write-Host "Deploying function:" $webappName.Name
    $fileName = (Get-ChildItem -Filter "*$appName-api.zip").Name
    Write-Host "Deploying package:" $fileName
    az webapp deployment source config-zip -g $env:APP_RESOURCE_GROUP_NAME --src "./$fileName" -n $webappName.Name


    Write-Host "Disabling SCM Access Restrictions"
    $accessRestriction = (az webapp update -g $env:APP_RESOURCE_GROUP_NAME -n $webappName.Name  --set publicNetworkAccess=Disabled)

    Write-Host "Deployment complete:" $webappName.Name
    Write-Host
}

Write-Host "Publishing Admin UI package"
$uiStorageAccountName = ((az storage account list -g $env:APP_RESOURCE_GROUP_NAME --query "[? ends_with(name, 'a')].{Name:name}" -o json) | ConvertFrom-Json).Name
Write-Host "Publishing to:" $uiStorageAccountName

$fileName = (Get-ChildItem -Path "./" -Filter "*admin.zip").Name
Write-Host "Deploying Admin UI package:" $fileName
Expand-Archive -Path "./$fileName" -DestinationPath "./admin-dist" -Force
az storage blob upload-batch --overwrite true --timeout 300 -d '$web' --account-name $uiStorageAccountName -s './admin-dist'

if("True" -eq $env:DEPLOY_WEB_CONFIG_JSON)
{
    Write-Host "Creating admin config.json"
    
    Write-Host "AUTH_SP_APP_ID: $env:ADMIN_AUTH_SP_APP_ID"
    Write-Host "AUTH_TENANT_ID: $env:ADMIN_AUTH_TENANT_ID"
    Write-Host "AUTH_AUTHORITY: $env:ADMIN_AUTH_AUTHORITY"
    Write-Host "AUTH_PRIMARY_DOMAIN: $env:ADMIN_AUTH_PRIMARY_DOMAIN"
    Write-Host "DEFAULT_COUNTY: $env:DEFAULT_COUNTY"
    
    Write-Host "AGENCY_ABBREVIATION: $env:AGENCY_ABBREVIATION"
    Write-Host "ENVIRONMENT: $env:ENVIRONMENT"
    # Write-Host "DISPLAY_REPORTING_EMAIL": $env:DISPLAY_REPORTING_EMAIL
    # Write-Host "REPORTING_EMAIL_ADDRESS": $env:REPORTING_EMAIL_ADDRESS
    Write-Host "ENABLE_STOP_DEBUGGER: $env:ENABLE_STOP_DEBUGGER"

    Write-Host "Expanding admin config file"
    Remove-Item -Path "config.json" -Force
    $fileName = (Get-ChildItem -Path "./" -Filter "*admin-config.zip").Name
    Expand-Archive -Path "./$fileName" -DestinationPath "./" -Force
    Write-Host "Filename:" $fileName
    Get-ChildItem -Path "./"

    Rename-Item -Path $fileName -NewName "config.json" -Force
    
    Write-Host "Displaying current config.json"
    Get-Content -Path "config.json"
    
    Write-Host "Executing variable replacement"
    $configJson = Get-Content -Path "config.json"
    $configJson = $configJson.Replace("#{ClientId}#", $env:ADMIN_AUTH_SP_APP_ID)
    $configJson = $configJson.Replace("#{AuthorityUrl}#", $env:ADMIN_AUTH_AUTHORITY)
    $configJson = $configJson.Replace("#{KnownAuthorities}#", $env:ADMIN_AUTH_AUTHORITY)
    $configJson = $configJson.Replace("#{TenantId}#", $env:ADMIN_AUTH_TENANT_ID)
    $configJson = $configJson.Replace("#{PrimaryDomain}#", $env:ADMIN_AUTH_PRIMARY_DOMAIN)
    
    $configJson = $configJson.Replace("#{Environment}#", $env:ENVIRONMENT)

    $configJson = $configJson.Replace("#{PrimaryDomain}#", $env:ADMIN_AUTH_PRIMARY_DOMAIN)
    $configJson = $configJson.Replace("#{PrimaryDomain}#", $env:ADMIN_AUTH_PRIMARY_DOMAIN)
    $configJson = $configJson.Replace("#{PrimaryDomain}#", $env:ADMIN_AUTH_PRIMARY_DOMAIN)
    $configJson = $configJson.Replace("#{PrimaryDomain}#", $env:ADMIN_AUTH_PRIMARY_DOMAIN)
    $configJson = $configJson.Replace("#{PrimaryDomain}#", $env:ADMIN_AUTH_PRIMARY_DOMAIN)
    $configJson = $configJson.Replace("#{PrimaryDomain}#", $env:ADMIN_AUTH_PRIMARY_DOMAIN)

    $configJson = $configJson.Replace("#{DefaultCounty}#", $env:DEFAULT_COUNTY)
    $configJson = $configJson.Replace("#{DisplayDebugger}#", $env:ENABLE_STOP_DEBUGGER)

    $appName = ($webappNames | Where-Object { $_.Name.Contains("admin") }).Name
    $configJson = $configJson.Replace("#{AdminServicesBaseUrl}#", $appName)

    $appName = ($webappNames | Where-Object { $_.Name.Contains("application") }).Name
    $configJson = $configJson.Replace("#{ApplicationServicesBaseUrl}#", $appName)
    
    $appName = ($webappNames | Where-Object { $_.Name.Contains("document") }).Name
    $configJson = $configJson.Replace("#{DocumentServicesBaseUrl}#", $appName)
    
    $appName = ($webappNames | Where-Object { $_.Name.Contains("payment") }).Name
    $configJson = $configJson.Replace("#{PaymentServicesBaseUrl}#", $appName)
    
    $appName = ($webappNames | Where-Object { $_.Name.Contains("schedule") }).Name
    $configJson = $configJson.Replace("#{ScheduleServicesBaseUrl}#", $appName)
    
    $appName = ($webappNames | Where-Object { $_.Name.Contains("userprofile") }).Name
    $configJson = $configJson.Replace("#{UserProfileServicesBaseUrl}#", $appName)

    Write-Host "Saving config.json"
    Set-Content -Path "config.json" -Value $configJson -Force

    Write-Host "Reviewing config.json"
    Get-Content -Path "config.json"

    Write-Host "Uploading config.json"
    az storage blob upload --overwrite true --timeout 300 --account-name $uiStorageAccountName -n "config.json" -c '$web' -f "config.json" 
}

Write-Host "Publishing Public UI package"
$uiStorageAccountName = ((az storage account list -g $env:APP_RESOURCE_GROUP_NAME --query "[? ends_with(name, 'p')].{Name:name}" -o json) | ConvertFrom-Json).Name
Write-Host "Publishing to:" $uiStorageAccountName

$fileName = (Get-ChildItem -Path "./" -Filter "*public.zip").Name
Write-Host "Deploying Public UI package:" $fileName
Expand-Archive -Path "./$fileName" -DestinationPath "./public-dist" -Force
az storage blob upload-batch --overwrite true --timeout 300 -d '$web' --account-name $uiStorageAccountName -s './public-dist'

if("True" -eq $env:DEPLOY_WEB_CONFIG_JSON)
{
    Write-Host "Creating public config.json"
    
    Write-Host "AUTH_SP_APP_ID: $env:PUBLIC_AUTH_SP_APP_ID"
    Write-Host "AUTH_TENANT_ID: $env:PUBLIC_AUTH_TENANT_ID"
    Write-Host "AUTH_AUTHORITY: $env:PUBLIC_AUTH_AUTHORITY"
    Write-Host "AUTH_PRIMARY_DOMAIN: $env:PUBLIC_AUTH_PRIMARY_DOMAIN"
    Write-Host "DEFAULT_COUNTY: $env:DEFAULT_COUNTY"
    
    Write-Host "AGENCY_ABBREVIATION: $env:AGENCY_ABBREVIATION"
    Write-Host "ENVIRONMENT: $env:ENVIRONMENT"
    # Write-Host "DISPLAY_REPORTING_EMAIL": $env:DISPLAY_REPORTING_EMAIL
    # Write-Host "REPORTING_EMAIL_ADDRESS": $env:REPORTING_EMAIL_ADDRESS
    Write-Host "ENABLE_STOP_DEBUGGER: $env:ENABLE_STOP_DEBUGGER"

    Write-Host "Expanding public config file"
    Remove-Item -Path "config.json" -Force
    $fileName = (Get-ChildItem -Path "./" -Filter "*public-config.zip").Name
    Expand-Archive -Path "./$fileName" -DestinationPath "./" -Force
    Write-Host "Filename:" $fileName
    Get-ChildItem -Path "./"

    Rename-Item -Path $fileName -NewName "config.json" -Force
    
    Write-Host "Displaying current config.json"
    Get-Content -Path "config.json"

    Write-Host "Executing variable replacement"
    $configJson = Get-Content -Path "config.json"
    $configJson = $configJson.Replace("#{ClientId}#", $env:PUBLIC_AUTH_SP_APP_ID)
    $configJson = $configJson.Replace("#{AuthorityUrl}#", $env:PUBLIC_AUTH_AUTHORITY)
    $configJson = $configJson.Replace("#{KnownAuthorities}#", $env:PUBLIC_AUTH_AUTHORITY)
    $configJson = $configJson.Replace("#{TenantId}#", $env:PUBLIC_AUTH_TENANT_ID)
    $configJson = $configJson.Replace("#{PrimaryDomain}#", $env:PUBLIC_AUTH_PRIMARY_DOMAIN)
    
    $configJson = $configJson.Replace("#{Environment}#", $env:ENVIRONMENT)

    $configJson = $configJson.Replace("#{DefaultCounty}#", $env:DEFAULT_COUNTY)
    $configJson = $configJson.Replace("#{DisplayDebugger}#", $env:ENABLE_STOP_DEBUGGER)

    $appName = ($webappNames | Where-Object { $_.Name.Contains("admin") }).Name
    $configJson = $configJson.Replace("#{AdminServicesBaseUrl}#", $appName)

    $appName = ($webappNames | Where-Object { $_.Name.Contains("application") }).Name
    $configJson = $configJson.Replace("#{ApplicationServicesBaseUrl}#", $appName)
    
    $appName = ($webappNames | Where-Object { $_.Name.Contains("document") }).Name
    $configJson = $configJson.Replace("#{DocumentServicesBaseUrl}#", $appName)
    
    $appName = ($webappNames | Where-Object { $_.Name.Contains("payment") }).Name
    $configJson = $configJson.Replace("#{PaymentServicesBaseUrl}#", $appName)
    
    $appName = ($webappNames | Where-Object { $_.Name.Contains("schedule") }).Name
    $configJson = $configJson.Replace("#{ScheduleServicesBaseUrl}#", $appName)
    
    $appName = ($webappNames | Where-Object { $_.Name.Contains("userprofile") }).Name
    $configJson = $configJson.Replace("#{UserProfileServicesBaseUrl}#", $appName)

    Write-Host "Saving config.json"
    Set-Content -Path "config.json" -Value $configJson -Force

    Write-Host "Reviewing config.json"
    Get-Content -Path "config.json"

    Write-Host "Uploading config.json"
    az storage blob upload --overwrite true --timeout 300 --account-name $uiStorageAccountName -n "config.json" -c '$web' -f "config.json" 
}

Write-Host "Finished deploying & importing applications"