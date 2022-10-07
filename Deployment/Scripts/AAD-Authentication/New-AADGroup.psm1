﻿Function New-AADGroup
{
    param (
        [Parameter(Mandatory = $true, HelpMessage = "The name of the group that should be created")] 
        $GroupName, 
        [Parameter(Mandatory = $true, HelpMessage = "The description of the group that should be created")] 
		$Description
	)

    $currentErrorActionPreference = $ErrorActionPreference

    Write-Host "Checking for group" $GroupName "*************** Ignore any onscreen errors ***************"
    $ErrorActionPreference = "Continue"
    $groupExists = az ad group show --only-show-errors --group $GroupName | ConvertFrom-Json
    $ErrorActionPreference = "Stop"

    if($groupExists)
    {
        Write-Host "Group already exists"
        return $groupExists.id
    }

    try {
        Write-Host "Creating group" $GroupName
        $adminGroup = az ad group create --display-name $GroupName  --mail-nickname $GroupName --description $Description | ConvertFrom-Json
    }
    finally {
        $ErrorActionPreference = $currentErrorActionPreference
    }
    
    return $adminGroup.id
}

Export-ModuleMember -Function New-AADGroup