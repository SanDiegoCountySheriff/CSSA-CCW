#######################################################################################
#
# This script requires both az cli and Powershell logins to be completed before running
#
#

cls

$tenantId = "00000000-0000-0000-0000-000000000000"
$subscriptionId = "00000000-0000-0000-0000-000000000000"

# Log with az cli
# Set cloud environment
az cloud set -n AzureCloud
#az cloud set -n AzureUSGovernment

# Log into desired tenant 
az login --tenant $tenantId

# Set default subscription
az account set -s $subscriptionId

# log with Powershell

Connect-AzAccount -Environment AzureCloud -Tenant $tenantId
# Connect-AzAccount -Environment AzureCloud

# Set default subscription
Set-AzContext -Subscription $subscriptionId

# Navigate to script files
cd c:\insight\CSSA\CSSA-CCW\Deployment\Scripts\AAD-Authentication

# Create Application Authentication Scheme
.\New-AuthenticationScheme.ps1 -DisplayName "CCW-ADMIN-AUTH-SP-DEV" -ReplyUri "http://localhost:4000" -SystemAdminGroupName "CCW-SYSTEM-ADMINS-DEV" -AdminGroupName "CCW-ADMINS-DEV" -ProcessorGroupName "CCW-PROCESSORS-DEV" -ReaderGroupName "CCW-READERS-DEV" -DebugOutput $false