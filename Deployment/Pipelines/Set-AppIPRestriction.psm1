
function Set-AppIPRestriction {
    Param(
        [Parameter(Mandatory = $true, HelpMessage = "Resource group name")] 
        $ResourceGroupName,
        [Parameter(Mandatory = $true, HelpMessage = "Web App name")] 
        $WebAppName,
        [Parameter(Mandatory = $true, HelpMessage = "Restricted IP address without CIDR (Example: 172.16.0.0)")] 
        $IPAddress,
        [Parameter(Mandatory = $true, HelpMessage = "IP Restriction rule name")] 
        $RuleName,
        [Parameter(Mandatory = $true, HelpMessage = "IP Restriction rule action (Allow/Deny)")] 
        $RuleAction,
        [Parameter(Mandatory = $true, HelpMessage = "IP Restriction rule priority (1-999)")] 
        $RulePriority,
        [Parameter(Mandatory = $false, HelpMessage = "IP Restriction rule description")] 
        $RuleDescription = "Temporary IP restriction for ADO CI/CD"
    )

    Write-Host "Set-AppIPRestriction starting"
 
    $APIVersion = ((Get-AzResourceProvider -ProviderNamespace Microsoft.Web).ResourceTypes | Where-Object ResourceTypeName -eq sites).ApiVersions[0]

    Write-Host $APIVersion

    $WebAppNameConfig = (Get-AzResource -ResourceType Microsoft.Web/sites/config -Name $WebAppName -ResourceGroupName $ResourceGroupName -ApiVersion $APIVersion)

    Write-Host $WebAppNameConfig

    $IpSecurityRestrictions = $WebAppNameConfig.Properties.ipsecurityrestrictions

    Write-Host $IpSecurityRestrictions
 
    $cidr = "$($IPAddress)/32"
    
    if ($cidr -in $IpSecurityRestrictions.ipAddress) {
        Write-Host "IP address $IPAddress is already added as restricted to $WebAppName."         
    }
    else {
        
        Write-Host "Configuring IP Restriction"
        
        $webIP = [PSCustomObject]@{name = ''; ipAddress = ''; Action = ''; Priority = ''; description = ''}
        $webIP.ipAddress = $cidr
        $webIP.name = $RuleName
        $webIP.action = $RuleAction
        $webIP.priority = $RulePriority
        $webIP.description = $RuleDescription

        if($IpSecurityRestrictions -eq $null){
            $IpSecurityRestrictions = @()
        }
 
        [System.Collections.ArrayList]$ArrayList = $IpSecurityRestrictions
        $ArrayList.Add($webIP) | Out-Null
 
        $WebAppNameConfig.properties.ipSecurityRestrictions = $ArrayList
        $WebAppNameConfig | Set-AzResource  -ApiVersion $APIVersion -Force | Out-Null

        Write-Host "New restricted IP address $IPAddress has been added to WebApp $WebAppName"
    }

    Write-Host "Set-AppIPRestriction finished"
}

Export-ModuleMember -Function Set-AppIPRestriction