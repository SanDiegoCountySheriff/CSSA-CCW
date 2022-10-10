param (
    [Parameter(Mandatory = $true, HelpMessage = "The name of the App Registration that should be created")]
    $DisplayName, 
    [Parameter(Mandatory = $true, HelpMessage = "The web application reply URI for post authentication")]
    $ReplyUri,
    [Parameter(Mandatory = $true, HelpMessage = "The name of the AAD Group that will implement the application system administors role")]
    $SystemAdminGroupName,
    [Parameter(Mandatory = $true, HelpMessage = "The name of the AAD Group that will implement the application administors role")]
    $AdminGroupName,
    [Parameter(Mandatory = $true, HelpMessage = "The name of the AAD Group that will implement the application processors role")]
    $ProcessorGroupName,
    [Parameter(Mandatory = $true, HelpMessage = "The name of the AAD Group that will implement the application readers role")]
    $ReaderGroupName, 
    [Parameter(Mandatory = $false, HelpMessage = "The name of the AAD Group that will implement the application readers role")] 
    [Boolean]$DebugOutput
)

##############################################################################################################################################################
#
# Example:
# New-AuthenticationScheme.ps1 \
#   -DisplayName "The name you wish to use for the App Registration" \
#   -ReplyUri "The Single Page Application Redirect URL this App Registration will allow users authorize from" \
#   -SystemAdminGroupName "The name of the group to create/use for System Administrators" \
#   -AdminGroupName "The name of the group to create/use for Application Administrators" \
#   -ProcessorGroupName "The name of the group to create/use for Application Processors" \
#   -ReaderGroupName "The name of the group to create/use for Application Readers"
#
# .\New-AuthenticationScheme.ps1 -DisplayName "CCW-ADMIN-AUTH-SP-DEV" -ReplyUri "http://localhost:4000" -SystemAdminGroupName "CCW-SYSTEM-ADMINS-DEV" -AdminGroupName "CCW-ADMINS-DEV" -ProcessorGroupName "CCW-PROCESSORS-DEV" -ReaderGroupName "CCW-READERS-DEV"
# .\New-AuthenticationScheme.ps1 -DisplayName "CCW-ADMIN-AUTH-SP-QA" -ReplyUri "http://localhost:4000" -SystemAdminGroupName "CCW-SYSTEM-ADMINS-QA" -AdminGroupName "CCW-ADMINS-QA" -ProcessorGroupName "CCW-PROCESSORS-QA" -ReaderGroupName "CCW-READERS-QA"
# .\New-AuthenticationScheme.ps1 -DisplayName "CCW-ADMIN-AUTH-SP-UAT" -ReplyUri "http://localhost:4000" -SystemAdminGroupName "CCW-SYSTEM-ADMINS-UAT" -AdminGroupName "CCW-ADMINS-UAT" -ProcessorGroupName "CCW-PROCESSORS-UAT" -ReaderGroupName "CCW-READERS-UAT"
# .\New-AuthenticationScheme.ps1 -DisplayName "CCW-ADMIN-AUTH-SP" -ReplyUri "http://localhost:4000" -SystemAdminGroupName "CCW-SYSTEM-ADMINS" -AdminGroupName "CCW-ADMINS" -ProcessorGroupName "CCW-PROCESSORS" -ReaderGroupName "CCW-READERS"


Import-Module .\New-AADGroup.psm1 -Force
Import-Module .\New-AppRegistration.psm1 -Force
Import-Module .\New-RBACRoleAssignment.psm1 -Force

$waitTime = 30

Write-Host "Creating App Registration & Service Princpal"
$AppRegistration = New-AppRegistration `
    -DisplayName $DisplayName `
    -Description "The App Registration that provides login and authorization to application users." `
    -ReplyUri $ReplyUri `
    | ConvertFrom-Json

Write-Host "Create AAD Groups"
$systemAdminGroupId = New-AADGroup -GroupName $SystemAdminGroupName -Description " system admins group"
$adminGroupId = New-AADGroup -GroupName $AdminGroupName -Description " admins group"
$processorsGroupId = New-AADGroup -GroupName $ProcessorGroupName -Description " processors group"
$readersGroupId = New-AADGroup -GroupName $ReaderGroupName -Description " readers group"

Write-Host "Waiting $waitTime seconds for AAD Groups propogate..."
Start-Sleep -Seconds $waitTime

Write-Host "Creating Role Assignments"
New-RBACRoleAssignment -EnterpriseAppObjectId $AppRegistration.ServicePrincipalId -ADGroupId $systemAdminGroupId -AppRoleId "96aac8d2-30dc-42a7-879f-41a3049dad37" -DebugOutput $DebugOutput
New-RBACRoleAssignment -EnterpriseAppObjectId $AppRegistration.ServicePrincipalId -ADGroupId $adminGroupId -AppRoleId "6edf8375-6cc0-4971-81de-99b507fb8415" -DebugOutput $DebugOutput
New-RBACRoleAssignment -EnterpriseAppObjectId $AppRegistration.ServicePrincipalId -ADGroupId $processorsGroupId -AppRoleId "9dd7132e-402e-4972-aad5-28faabc395f6" -DebugOutput $DebugOutput
New-RBACRoleAssignment -EnterpriseAppObjectId $AppRegistration.ServicePrincipalId -ADGroupId $readersGroupId -AppRoleId "e0e9bde6-052f-4a85-902e-fee67f50b1b2" -DebugOutput $DebugOutput

Write-Host "Finished creating App Registration & Service Princpal"