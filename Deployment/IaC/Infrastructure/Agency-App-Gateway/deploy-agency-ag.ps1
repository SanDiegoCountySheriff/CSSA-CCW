$originalErrorActionPreference = $ErrorActionPreference

$ErrorActionPreference = "Stop"

$agency_abbreviation = "sdsd"
$environment = "test"
$deploymentResourceGroupName = "rg-sdsd-it-ccw-dev-004"
$agvNetName = "vnet-sdsd-it-ag-dev-001"
$agSubnetName = "AppGatewaySubnet"
$agSubnetAddressRange = "172.16.21.0/24"
$agencyAppGatewayIpAddress = "172.16.21.4"

$parameterFileName = "$agency_abbreviation-$environment-template-parameters.json"
Write-Host "Using parameter file:" $parameterFileName

#  -n $agSubnetName
$vnetSubnetList = (az network vnet subnet list -g $deploymentResourceGroupName --vnet-name $agvNetName) | ConvertFrom-Json
$agSubnet = $vnetSubnetList | Where-Object -Filter {$_.name -eq $agSubnetName} 
Write-Host "App Gateway subnet exists:" ($agSubnet -ne $null)

if($agSubnet -eq $null)
{
    Write-Host "Creating AG Subnet"
    $agSubnet = (az network vnet subnet create `
                    --name $agSubnetName `
                    --resource-group $deploymentResourceGroupName  `
                    --vnet-name $agvNetName `
                    --address-prefixes $agSubnetAddressRange) | ConvertFrom-Json
}

$agSubnetId = $agSubnet.id
Write-Host "Using Subnet ID:" $agSubnetId

$currentDate = Get-Date
$deploymentName = "$agency_abbreviation-$environment-" + $currentDate.Year.ToString() + $currentDate.Month.ToString() + $currentDate.Day.ToString() + $currentDate.Minute
Write-Host "Using deployment name:" $deploymentName

$result = (az deployment group create `
                -g $deploymentResourceGroupName `
                -f template.json `
                -p $parameterFileName  `
                --parameters ag_vnet_subnet_id=$agSubnetId ag_private_ip_address=$agencyAppGatewayIpAddress `
                -n $deploymentName ) | ConvertFrom-Json

$result

az network vnet subnet update `
    -g $deploymentResourceGroupName `
    -n $agSubnetName `
    --vnet-name $agvNetName `
    --nsg "nsg-$agency_abbreviation-ag-$environment-shd" `
    --route-table "rt-$agency_abbreviation-ag-$environment-shd"

$ErrorActionPreference = $originalErrorActionPreference