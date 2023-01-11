
$CSSA_SHD_SUBSCRIPTION_ID="5edf03ce-623c-47c3-b15f-dab2be8a319e"
$CSSA_RESOURCE_GROUP_NAME="rg-cssa-it-ops-shd"
$CSSA_DNS_ROOT_ZONE="cssa.cloud"
$CSSA_CDN_PROFILE_NAME="cssa-global-cdn"
$CSSA_CERT_KEY_VAULT_NAME="kv-cssa-it-ops-shd-001"
$CSSA_CERT_SECRET_NAME="star-cssa-cloud"
$dns_sub_domain_name="admin-ccw-dev1-sdsd"
$cname_alias="sasdsditccwdevaui001.z3.web.core.usgovcloudapi.net"
$WEB_CONTENT_URL="sasdsditccwdevaui001.z3.web.core.usgovcloudapi.net"
$endpoint_name="cep-sdsd-it-ccw-dev-admin-001"
$endpoint_host_name=$endpoint_name + ".azureedge.us"
$dns_host_name_name=$dns_sub_domain_name
$dns_host_name=$dns_sub_domain_name + ".cssa.cloud"


Write-Host "Creating DNS"
az network dns record-set cname set-record --subscription $CSSA_SHD_SUBSCRIPTION_ID -g $CSSA_RESOURCE_GROUP_NAME -z $CSSA_DNS_ROOT_ZONE -n $dns_sub_domain_name -c $endpoint_host_name
az network dns record-set cname set-record --subscription $CSSA_SHD_SUBSCRIPTION_ID -g $CSSA_RESOURCE_GROUP_NAME -z $CSSA_DNS_ROOT_ZONE -n "cdnverify.$dns_sub_domain_name" -c "cdnverify.$endpoint_host_name"

Write-Host "Creating endpoint"
az cdn endpoint create `
    --subscription $CSSA_SHD_SUBSCRIPTION_ID `
    --resource-group $CSSA_RESOURCE_GROUP_NAME `
    --location "Global" `
    --profile-name $CSSA_CDN_PROFILE_NAME `
    --name $endpoint_name `
    --origin $WEB_CONTENT_URL `
    --origin-host-header $WEB_CONTENT_URL `
    --enable-compression true `
    --no-http false `
    --no-https false `
   --tags agency=sdsd application=ccw

Write-Host "Creating custom domain"
az cdn custom-domain create `
    --subscription $CSSA_SHD_SUBSCRIPTION_ID `
    --resource-group $CSSA_RESOURCE_GROUP_NAME `
    --profile-name $CSSA_CDN_PROFILE_NAME `
    --endpoint-name $endpoint_name `
    --hostname $dns_host_name `
    --name $dns_host_name_name

Write-Host "Enabling HTTPS"
az cdn custom-domain enable-https `
    --subscription $CSSA_SHD_SUBSCRIPTION_ID `
    --resource-group $CSSA_RESOURCE_GROUP_NAME `
    --profile-name $CSSA_CDN_PROFILE_NAME `
    --endpoint-name $endpoint_name `
    --name $dns_host_name_name `
    --min-tls-version 1.2 `
    --user-cert-subscription-id $CSSA_SHD_SUBSCRIPTION_ID `
    --user-cert-group-name $CSSA_RESOURCE_GROUP_NAME `
    --user-cert-vault-name $CSSA_CERT_KEY_VAULT_NAME `
    --user-cert-secret-name $CSSA_CERT_SECRET_NAME `
    --user-cert-protocol-type 'sni'

Write-Host "Adding redirect rule"
az cdn endpoint rule add `
    --subscription $CSSA_SHD_SUBSCRIPTION_ID `
    --resource-group $CSSA_RESOURCE_GROUP_NAME `
    --profile-name $CSSA_CDN_PROFILE_NAME `
    --name $endpoint_name `
    --order 1 `
    --rule-name "HttpRedirect" `
    --match-variable RequestScheme `
    --operator Equal `
    --match-values HTTP `
    --action-name "UrlRedirect" `
    --redirect-protocol Https `
    --redirect-type PermanentRedirect

