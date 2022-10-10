Function New-AppRegistration
{
    param (
        [Parameter(Mandatory = $true, HelpMessage = "The name of the App Registration that should be created")]
        $DisplayName, 
        [Parameter(Mandatory = $true, HelpMessage = "The description of the group that should be created")]
		$Description, 
        [Parameter(Mandatory = $true, HelpMessage = "The web application reply URI for post authentication")]
		$ReplyUri
	)

    Write-Host "New-AppRegistration starting"

    $manifestJsonFilePath = ".\GraphPolicyManifest.json"
    $claimsJsonFilePath = ".\ClaimsManifest.json"
    $rolesJsonFilePath = ".\RolesManifest.json"
    $spaRedirectUrlFilePath = ".\SpaRedirectUrl.json"
    $oAuthPermissionsFilePath = ".\OAuthPermissions.json"

    $waitTime = 30
	
    $ErrorActionPreference = "SilentlyContinue"

    Write-Host "Create app with display name $DisplayName"
    $adApplication = az ad app create `
        --display-name $DisplayName `
        --sign-in-audience AzureADMyOrg `
        --enable-access-token-issuance $false `
        --enable-id-token-issuance $true `
        --optional-claims $claimsJsonFilePath `
        --app-roles $rolesJsonFilePath `
        --is-fallback-public-client $false `
        | ConvertFrom-Json

        
    $ErrorActionPreference = "Stop"
    
    $appId = $adApplication.appId

    
    $cloud = (az cloud show) | ConvertFrom-Json
    Write-Host "Connecting to cloud: $cloud"
    
    $msGraphBaseUrl = "https://graph.microsoft.com"
    if($cloud.name -eq "AzureUSGovernment")
    {
        $msGraphBaseUrl = "https://graph.microsoft.us"
    }

    Write-Host "Setting SPA redirect URL" $ReplyUri
    $spaRedirectUrlJson = (Get-Content $spaRedirectUrlFilePath).Replace("__SPA_REDIRECT_URL__", $ReplyUri)
    Set-Content -Force -Path .\spaTemp.json -Value $spaRedirectUrlJson
    $msGraphUrl = $msGraphBaseUrl + "/v1.0/myorganization/applications/" + $adApplication.id
    az rest --method patch --url $msGraphUrl --body "@spaTemp.json"

    Write-Host "Waiting $waitTime seconds for app registration to propogate..."
    Start-Sleep -Seconds $waitTime

    Write-Host "Assigning app ownership to current user"
    $currentUserObjectId = (az ad signed-in-user show | ConvertFrom-Json).id
    az ad app owner add --id $appId --owner-object-id $currentUserObjectId

    Write-Host "Waiting $waitTime seconds for app registration owner to propogate..."
    Start-Sleep -Seconds $waitTime
    
    Write-Host "Checking if the service pricipal exists"
    Write-Host "***************************** Ignore any onscreen errors *****************************"
    $ErrorActionPreference = "Continue"
    $spExists = az ad sp show --id $appId
    $ErrorActionPreference = "Stop"
    if(!$spExists)
    {
        Write-Host "Creating service pricipal manually"
        $spExists = az ad sp create --id $appId
    }

    Write-Host "Waiting $waitTime seconds for app required permissions to propogate..."
    Start-Sleep -Seconds $waitTime

    Write-Host "Setting app API uri"
    az ad app update --id $appId --identifier-uris "api://$($appId)"

    Write-Host "Setting OAuth User Impersonation Permissions"
    $oAuthPermissionsJson = Get-Content $oAuthPermissionsFilePath
    $oAuthPermissionsGuid = New-Guid
    $oAuthPermissions = $oAuthPermissionsJson.Replace("__PERMISSIONS_GUID__", $oAuthPermissionsGuid)
    Set-Content -Force -Path .\oauthPremissionsTemp.json -Value $oAuthPermissions
    az rest --method patch --url $msGraphUrl --body "@oauthPremissionsTemp.json"

    Write-Host "Setting app registration/service principal API permission"
    $graphManifestJson = Get-Content $manifestJsonFilePath
    $scopeJson = $graphManifestJson.Replace("__APPID__", $appId).Replace("__SCOPEID__", $oAuthPermissionsGuid)
    Set-Content -Force -Path .\scopeJson.json -Value $scopeJson
    $adApplication = az ad app update `
        --id $appId `
        --required-resource-access "@scopeJson.json" `
        | ConvertFrom-Json

    Write-Host "Waiting $waitTime seconds for app service principal to propogate..."
    Start-Sleep -Seconds $waitTime

    Write-Host "Turning on role assignment required flag"
    $servicePrincipal = az ad sp update --id $appId --set appRoleAssignmentRequired=true

    Write-Host "Setting the Enterprise Application flag"
    $newGraphUrl = "$msGraphBaseUrl/v1.0/servicePrincipals/" + ($spExists | ConvertFrom-Json).id 
    az rest --method PATCH --url $newGraphUrl --body '{\"tags\":[\"WindowsAzureActiveDirectoryIntegratedApp\"]}'

    # az ad sp update --id $appId --add tags WindowsAzureActiveDirectoryIntegratedApp
	
    $cloud = (az cloud show) | ConvertFrom-Json
    Write-Host $cloud
    if($cloud.name -eq "AzureCloud")
    {
        Write-Host "Apply admin consent for app permissions"
        az ad app permission admin-consent --id $appId
    }

    Write-Host "Get final app details"
    $adApplication = az ad app list --display-name $DisplayName | ConvertFrom-Json
		
    Write-Host "Get service principal id for app $DisplayName"
    $servicePrincipal = az ad sp list --display-name $DisplayName | ConvertFrom-Json
    
    $adApplicationProperties = @{
        ApplicationId = $adApplication.appId
        DisplayName = $adApplication.displayName
	    IdentifierUris = $adApplication.identifierUris
	    ServicePrincipalId =  $servicePrincipal.id
        ReplyUrls = $ServicePrincipal.replyUrls
    } | ConvertTo-Json

    Write-Host "Created app with display name $DisplayName"
    Write-Host $adApplicationProperties

    return $adApplicationProperties
}

Export-ModuleMember -Function New-AppRegistration