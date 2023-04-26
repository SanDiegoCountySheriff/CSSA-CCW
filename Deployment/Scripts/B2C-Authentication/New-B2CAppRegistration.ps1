Install-Module Microsoft.Graph.Applications -AllowClobber -Force
Import-Module Microsoft.Graph.Applications -Force
Import-Module .\New-AzureB2C.psm1 -Force

$graphEnvironment = "Global" # "USGov"
$b2cTenantName = "sdsherifftestb2c"
$b2cDomainSuffix = ".com"
$b2cTenantId = "$($b2cTenantName).onmicrosoft$($b2cDomainSuffix)"

Connect-MgGraph `
    -Environment $graphEnvironment `
    -TenantId $b2cTenantId `
    -Scopes "User.ReadWrite.All", "Application.ReadWrite.All", "Directory.AccessAsUser.All", "Directory.ReadWrite.All"

New-B2CAuthenticationApp `
    -B2CTenantName $b2cTenantName `
    -RootDomainSuffix $b2cDomainSuffix  