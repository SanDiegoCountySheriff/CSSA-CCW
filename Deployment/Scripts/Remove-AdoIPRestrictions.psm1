Function Remove-AdoIPRestrictions
{
    Param(
        [Parameter(Mandatory = $true, HelpMessage = "Resource group name")] 
        $ResourceGroupName,
        [Parameter(Mandatory = $true, HelpMessage = "Web App name")] 
        $WebAppName
    )

    Write-Host "Remove-AdoIPRestrictions starting:" $WebAppName 

    $APIVersion = ((Get-AzResourceProvider -ProviderNamespace Microsoft.Web).ResourceTypes | Where-Object ResourceTypeName -eq sites).ApiVersions[0]
    $WebAppNameConfig = (Get-AzResource -ResourceType Microsoft.Web/sites/config -Name $WebAppName -ResourceGroupName $ResourceGroupName -ApiVersion $APIVersion)
    $IpSecurityRestrictions = $WebAppNameConfig.Properties.ipsecurityrestrictions

    [System.Collections.ArrayList]$baseIpRestrictions = @()

    # Find any restriction that is NOT for ADO deployments
    foreach($ip in $IpSecurityRestrictions)
    {
        Write-Host $ip.name

        if($ip.name -ne 'AllowAdoDeployment')
        {
            $baseIpRestrictions.Add($ip)
        }
    }

    # Reset to base IPAddress restrictions
    $WebAppNameConfig.properties.ipSecurityRestrictions = $baseIpRestrictions
    $WebAppNameConfig | Set-AzResource -ApiVersion $APIVersion -Force | Out-Null

    Write-Host "Removed restricted IP address $IPAddress from Web App $WebAppName"
    
    Write-Host "Remove-AdoIPRestrictions finished:" $WebAppName
}

Export-ModuleMember -Function Remove-AdoIPRestrictions