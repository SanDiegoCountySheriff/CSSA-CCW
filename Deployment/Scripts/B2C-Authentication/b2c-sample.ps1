Clear-Host

Write-Host "***************************** Ignore any onscreen errors *****************************" -ForegroundColor Red
Disconnect-MgGraph

Import-Module .\New-AzureB2C.psm1 -Force

$counter = 39
$counter += 1
$tenantName = "lesmusb2c$counter"
# $tenantName = "lesmusb2c"

New-B2CTenant -ResourceGroupName rg-b2c-test -B2CTenantName $tenantName