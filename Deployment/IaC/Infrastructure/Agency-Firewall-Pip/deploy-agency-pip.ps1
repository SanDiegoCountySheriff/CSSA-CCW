$originalErrorActionPreference = $ErrorActionPreference

$ErrorActionPreference = "Stop"

$agency_abbreviation = "sdsd"
$environment = "test"
$applicationName = "ccw"
$cssa_network_hub_rg = "rg-cssa-it-network-shd"
$firewallName = "fw-cssa-it-shd-001"
$firewallvNetName = "vnet-cssa-it-hub-shd-001"
$firewallPolicyName = "fwp-cssa-it-shd-001"
$allowedInboundAddresses = "0.0.0./0"
$agecnyAppGatewayIpAddress = "172.16.0.4"

$parameterFileName = "$agency_abbreviation-$environment-template-parameters.json"
Write-Host "Using parameter file:" $parameterFileName

$currentDate = Get-Date
$deploymentName = "$agency_abbreviation-$environment-" + $currentDate.Year.ToString() + $currentDate.Month.ToString() + $currentDate.Day.ToString() + $currentDate.Minute
Write-Host "Using deployment name:" $deploymentName

$result = (az deployment group create -g $cssa_network_hub_rg -f template.json -p $parameterFileName -n $deploymentName) | ConvertFrom-Json

az network firewall ip-config create --firewall-name $firewallName --name $result.pipName --public-ip-address $result.pipId --resource-group $cssa_network_hub_rg --vnet-name $firewallvNetName


az extension add --name azure-firewall --yes

Write-Output "-----------------------"
Write-Output "Creating the DNAT rules"
Write-Output "-----------------------"

$dnatCollectionName = $agency_abbreviation.ToUpper() + "-" + $applicationName.ToUpper() + "-" + $environment.ToUpper() + "-DNAT-RULES"

az network firewall policy rule-collection-group collection add-nat-collection `
    --collection-priority 100 `
    --name "$($dnatCollectionName)" `
    --policy-name "$($firewallPolicyName)" `
    --rcg-name "DefaultDnatRuleCollectionGroup" `
    --ip-protocols 'TCP' `
    --resource-group "$($cssa_network_hub_rg)" `
    --action 'Dnat' `
    --dest-addr "$($result.pipAddress)" `
    --destination-ports '80' `
    --rule-name "$($dnat_http_rule_name)" `
    --source-addresses "$($allowedInboundAddresses)" `
    --translated-address "$($agecnyAppGatewayIpAddress)" `
    --translated-port '80'

az network firewall policy rule-collection-group collection rule add `
    --collection-name "DefaultDnatRuleCollectionGroup" `
    --name "$($dnat_https_rule_name)" `
    --policy-name "$($firewall_policy_name)" `
    --rcg-name "DefaultDnatRuleCollectionGroup" `
    --resource-group "$($cssa_network_hub_rg)" `
    --rule-type 'NatRule' `
    --dest-addr "$($result.pipAddress)" `
    --destination-ports '443' `
    --ip-protocols 'TCP' `
    --source-addresses "$($allowedInboundAddresses)" `
    --translated-address "$($agecnyAppGatewayIpAddress)" `
    --translated-port '443'      

$ErrorActionPreference = $originalErrorActionPreference