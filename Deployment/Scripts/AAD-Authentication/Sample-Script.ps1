#######################################################################################
#
# This script requires both az cli and Powershell logins to be completed before running
#
#

cls

$environment = "DEV"
$tenantId = "00000000-0000-0000-0000-000000000000"
$subscriptionId = "00000000-0000-0000-0000-000000000000"
$cloud = "AzureCloud"
$cloud = "AzureUSGovernment"

Write-Host "Using tenant:" $tenantId
Write-Host "Using subscription:" $subscriptionId

# Log with az cli
# Set cloud environment
az cloud set -n $cloud
Write-Host "Using cloud:" (az cloud show | ConvertFrom-Json).name

# Log into desired tenant 
Write-Host "Logging into az cli"
az login --tenant $tenantId

# Set default subscription
Write-Host "Setting az cli default subscription:" $subscriptionId
az account set -s $subscriptionId

# log with Powershell
Write-Host "Logging into Azure PowerShell"
Connect-AzAccount -Environment $cloud -Tenant $tenantId

# Set default subscription
Write-Host "Setting Azure PowerShell default subscription:" $subscriptionId
Set-AzContext -Subscription $subscriptionId

# Navigate to script files
Write-Host "Navigating to aad-authentication root folder"
cd c:\insight\CSSA\CSSA-CCW\Deployment\Scripts\AAD-Authentication

# Create Application Authentication Scheme
Write-Host "Executing main script"
.\New-AuthenticationScheme.ps1 -DisplayName "CCW-ADMIN-AUTH-SP-$environment" -ReplyUri "http://localhost:4000" -SystemAdminGroupName "CCW-SYSTEM-ADMINS-$environment" -AdminGroupName "CCW-ADMINS-$environment" -ProcessorGroupName "CCW-PROCESSORS-$environment" -ReaderGroupName "CCW-READERS-$environment" -DebugOutput $false